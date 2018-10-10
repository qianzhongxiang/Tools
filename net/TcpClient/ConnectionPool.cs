using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationProtocolProxy
{
    //public class CustomSocket : TcpClient
    //{
    //    private DateTime _TimeCreated;

    //    public DateTime TimeCreated
    //    {
    //        get { return _TimeCreated; }
    //        set { _TimeCreated = value; }
    //    }

    //    public CustomSocket(string host, int port)
    //        : base(host, port)
    //    {
    //        _TimeCreated = DateTime.Now;
    //    }
    //}
    public static class TCPReciveClientPool
    {
        /// <summary>
        /// Queue of available socket connections.
        /// </summary>
        private static Queue<TcpClient> availableSockets = null;
        /// <summary>
        /// host IP Address
        /// </summary>
        private static string hostIP = string.Empty;
        /// <summary>
        /// host Port
        /// </summary>
        private static int hostPort = 0;
        /// <summary>
        /// Initial number of connections
        /// </summary>
        private static int POOL_MIN_SIZE = 1;
        /// <summary>
        /// The maximum size of the connection pool.
        /// </summary>
        private static int POOL_MAX_SIZE = 50;
        /// <summary>
        /// Created host Connection counter 
        /// </summary>
        private static int SocketCounter = 0;

        public static bool Initialized = false;

        private static TcpListener tcpListener = null;
        static TCPReciveClientPool()
        {
            
        }
        public static void Distory() {
            tcpListener.Start();
        }

        /// <summary>
        /// Initialize host Connection pool
        /// </summary>
        /// <param name="hostIP">host IP Address</param>
        /// <param name="hostPort">host Port</param>
        /// <param name="minConnections">Initial number of connections</param>
        /// <param name="maxConnections">The maximum size of the connection pool</param>
        public static void InitializeConnectionPool(string hostIPAddress, int hostPortNumber, int minConnections, int maxConnections)
        {
            POOL_MAX_SIZE = maxConnections;
            POOL_MIN_SIZE = minConnections;
            hostIP = hostIPAddress;
            hostPort = hostPortNumber;
            availableSockets = new Queue<TcpClient>();

            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            tcpListener = new TcpListener(localAddr, 10020);
            tcpListener.Start();

            for (int i = 0; i < minConnections; i++)
            {
                TcpClient cachedSocket = OpenSocket();
                PutSocket(cachedSocket);
            }

            Initialized = true;

            System.Diagnostics.Trace.WriteLine("Connection Pool is initialized with Max Number of " +
                    POOL_MAX_SIZE.ToString() + " And Min number of " + availableSockets.Count.ToString());
        }

        /// <summary>
        /// Get(Dequeue) an open socket from the connection pool. 
        /// </summary>
        /// <returns>Socket opened returned from the pool. </returns>
        public static TcpClient GetSocket()
        {
            if (TCPReciveClientPool.availableSockets.Count > 0)
            {
                lock (availableSockets)
                {
                    TcpClient socket = null;
                    while (TCPReciveClientPool.availableSockets.Count > 0)
                    {
                        socket = TCPReciveClientPool.availableSockets.Dequeue();

                        if (socket.Connected)
                        {
                            System.Diagnostics.Trace.WriteLine("Socket Dequeued -> Pool size: " +
                                TCPReciveClientPool.availableSockets.Count.ToString());
                            return socket;
                        }
                        else
                        {
                            socket.Close();
                            System.Threading.Interlocked.Decrement(ref SocketCounter);
                            System.Diagnostics.Trace.WriteLine("GetSocket -- Close -- Count: " + SocketCounter.ToString());
                        }
                    }
                }
            }

            return null;
        }

        public static TcpClient GetSocket(string ip)
        {
            lock (availableSockets)
            {
                foreach (var i in TCPReciveClientPool.availableSockets)
                {
                    if (i.Connected && i.Client.RemoteEndPoint.ToString() == ip) return i;
                }
            }
            return null;
        }

        public static void Enumerate(Action<TcpClient> act)
        {
            lock (availableSockets)
            {
                foreach (var i in TCPReciveClientPool.availableSockets)
                {
                    if (i.Connected) act(i);
                }
            }
        }

        /// <summary>
        /// Return the given socket back to the socket pool.
        /// </summary>
        /// <param name="socket">Socket connection to return.</param>
        public static void PutSocket(TcpClient socket)
        {
            lock (TCPReciveClientPool.availableSockets)
            {
                if (TCPReciveClientPool.availableSockets.Count >= TCPReciveClientPool.POOL_MAX_SIZE)
                {
                    var count = TCPReciveClientPool.availableSockets.Count;
                    while ((count--) > 0)
                    {
                        var i = TCPReciveClientPool.availableSockets.Dequeue();
                        if (i.Connected) TCPReciveClientPool.availableSockets.Enqueue(i);
                    }
                }
                if (TCPReciveClientPool.availableSockets.Count < TCPReciveClientPool.POOL_MAX_SIZE)// Configuration Value
                {
                    if (socket != null)
                    {
                        if (socket.Connected)
                        {
                            TCPReciveClientPool.availableSockets.Enqueue(socket);

                            System.Diagnostics.Trace.WriteLine("Socket Queued -> Pool size: " +
                                TCPReciveClientPool.availableSockets.Count.ToString());
                        }
                        else
                        {
                            socket.Close();
                        }
                    }
                }
                else
                {
                    socket.Close();
                    System.Diagnostics.Trace.WriteLine("PutSocket - Socket is forced to closed -> Pool size: " +
                                    TCPReciveClientPool.availableSockets.Count.ToString());
                }
            }
        }

        /// <summary>
        /// Open a new socket connection.
        /// </summary>
        /// <returns>Newly opened socket connection.</returns>
        public static TcpClient OpenSocket()
        {
            var client = TCPReciveClientPool.tcpListener.AcceptTcpClient();
            System.Threading.Interlocked.Increment(ref SocketCounter);
            System.Diagnostics.Trace.WriteLine("Created host Connections count: " + SocketCounter.ToString());
            return client;
        }

        /// <summary>
        /// Populate host socket exception on sending or receiveing
        /// </summary>
        public static void PopulateSocketError()
        {
            System.Threading.Interlocked.Decrement(ref SocketCounter);
            System.Diagnostics.Trace.WriteLine("Populate Socket Error host Connections count: " + SocketCounter.ToString());
        }
    }
}
