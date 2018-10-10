using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Net.NetworkInformation;
using NetClient.Entity;
using DONN.Tools.Logger;

namespace NetClient
{
    class ListenBegin
    {
        public TcpListener Listener { get; set; }
        public Action<System.Net.Sockets.TcpClient, MemoryStream, long> Action { get; set; }
        public TcpClient Sender { get; set; }
    }
    class ClientBegin
    {
        public System.Net.Sockets.TcpClient Client { get; set; }
        public TcpClient Sender { get; set; }
        public Stream stream { get; set; }
    }
    public class TcpClient : ITcpClient
    {
        private Dictionary<string, TcpListener> Listerners = new Dictionary<string, TcpListener>();
        private Dictionary<string, System.Net.Sockets.TcpClient> Clients = new Dictionary<string, System.Net.Sockets.TcpClient>();
        public TcpClient()
        {
            //定时清理Client
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(60000);
                    ClearClients();
                }
            });
        }
        private void ClearClients()
        {
            try
            {
                var keys = Clients.Keys.ToArray();
                foreach (var key in keys)
                {
                    //目前没有其他地方去remove client，所有不需要判断key是否存在
                    lock (Clients)
                    {
                        if (!IsConnected(Clients[key]))
                        {
                            Clients[key].Close();
                            Clients.Remove(key);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString(), e);
            }
        }
        private void BeginAccept(Action<System.Net.Sockets.TcpClient, MemoryStream, long> act, TcpListener listener)
        {
            var state = new ListenBegin { Listener = listener, Action = act, Sender = this };
            listener.BeginAcceptTcpClient(new AsyncCallback(ar =>
            {
                try
                {
                    var s = ar.AsyncState as ListenBegin;
                    if (!listener.Server.IsBound)
                    {
                        listener.Start();
                        BeginAccept(act, s.Listener);
                    }
                    var client = s.Listener.EndAcceptTcpClient(ar);
                    client.ReceiveTimeout = 10000;
                    //client.SendTimeout = 10000;

                    var endpoint = (IPEndPoint)client.Client.LocalEndPoint;
                    var rendpoint = (IPEndPoint)client.Client.RemoteEndPoint;
                    var key = s.Sender.ClientKey(endpoint.Address.ToString(), endpoint.Port, rendpoint.Address.ToString(), rendpoint.Port);
                    s.Sender.Clients[key] = client;

                    HandleClientRequest(client, s.Action);
                    BeginAccept(s.Action, s.Listener);
                }
                catch (Exception e)
                {
                    LogHelper.Error(e.ToString(), e);
                }
            }), state);
        }

        private void HandleClientRequest(System.Net.Sockets.TcpClient client, Action<System.Net.Sockets.TcpClient, MemoryStream, long> act)
        {
            Task.Run(() =>
            {
                using (NetworkStream ns = client.GetStream())
                {
                    while (ns.CanRead)
                    {
                        try
                        {
                            if (ns.DataAvailable)
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    byte[] readBuffer = new byte[1024];
                                    int length = 0, read = 0;
                                    while (ns.DataAvailable)
                                    {
                                        length += read = ns.Read(readBuffer, 0, readBuffer.Length);
                                        stream.Write(readBuffer, 0, read);
                                    }
                                    act(client, stream, length);
                                }
                        }
                        catch (Exception e)
                        {
                            LogHelper.Error(e.ToString(), e);
                        }
                        Thread.Sleep(10);
                    }
                }
            });
        }

        public bool IsConnected(System.Net.Sockets.TcpClient c)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections().Where(x => x.LocalEndPoint.Equals(c.Client.LocalEndPoint) && x.RemoteEndPoint.Equals(c.Client.RemoteEndPoint)).ToArray();

            if (tcpConnections != null && tcpConnections.Length > 0)
            {
                TcpState stateOfConnection = tcpConnections.First().State;
                return stateOfConnection == TcpState.Established;
            }
            return false;
        }
        public void Destory()
        {
            lock (Listerners)
            {
                foreach (var l in Listerners)
                {
                    l.Value.Stop();
                }
                Listerners.Clear();
            }
        }
        private TcpListener Listen(string hostName, int port)
        {
            IPAddress ipaddr = null;
            if (string.IsNullOrWhiteSpace(hostName) || hostName == "localhost" || hostName.StartsWith("127.") || hostName.StartsWith("255."))
                ipaddr = IPAddress.Any;// Dns.GetHostEntry(s).AddressList[0];
            else
                ipaddr = IPAddress.Parse(hostName);
            var listener = new TcpListener(ipaddr, port);
            listener.Start();
            return listener;
        }
        public void BeginAccept(string hostName, int port, Action<ProtocolEntity, Stream> callback)
        {
            var listener = this.Listerners[ListernerKey(hostName, port)] = Listen(hostName, port);
            BeginAccept((c, m, l) =>
            {
                var remoteEndPoint = (IPEndPoint)c.Client.RemoteEndPoint;
                string remoteHostname = remoteEndPoint.Address.ToString();  //提取客户端IP地址
                int remotePort = remoteEndPoint.Port;
                callback(new ProtocolEntity
                {
                    Sender = this,
                    RemoteHostName = remoteHostname,
                    RemotePort = remotePort,
                    LocalHostName = hostName,
                    LocalPort = port
                }, m);
            }, listener);
        }
        private string ClientKey(string localHostName, int localPort, string remoteHostName, int remotePort)
        {
            return $"{localHostName}:{localPort}-{remoteHostName}:{remotePort}";
        }
        public void Send(MemoryStream stream, ProtocolEntity protocolEntity)
        {
            var key = ClientKey(protocolEntity.LocalHostName, protocolEntity.LocalPort, protocolEntity.RemoteHostName, protocolEntity.RemotePort);
            stream.Seek(0, SeekOrigin.Begin);
            System.Net.Sockets.TcpClient client = null;
            if (Clients.ContainsKey(key))
            {
                client = Clients[key];
                if (IsConnected(client))
                {
                    stream.CopyTo(client.GetStream());
                    return;
                }
            }
            else
            {
                client = new System.Net.Sockets.TcpClient(new IPEndPoint(IPAddress.Parse(protocolEntity.LocalHostName), protocolEntity.LocalPort));
                Clients[key] = client;
            }
            var state = new ClientBegin { Sender = this, Client = client, stream = stream };
            client.BeginConnect(protocolEntity.RemoteHostName, protocolEntity.RemotePort, (ar) =>
            {
                var s = ar.AsyncState as ClientBegin;
                s.stream.Seek(0, SeekOrigin.Begin);
                s.stream.CopyTo(s.Client.GetStream());
            }, state);
        }
        private string ListernerKey(string hostName, int port)
        {
            return $"{hostName}:{port}";
        }
        public void Terminate(string hostName, int port)
        {
            var key = ListernerKey(hostName, port);
            lock (Listerners)
            {
                if (Listerners.Keys.Contains(key))
                {
                    Listerners[key].Stop();
                    Listerners.Remove(key);
                }
            }
        }
    }
}
