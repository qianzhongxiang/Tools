using System;
using System.IO;
using NetClient.Entity;

namespace NetClient
{
    public interface IBaseNet
    {
        /// <summary>
        /// for tcp;udp
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        /// <param name="callback"></param>
        void BeginAccept(string hostName, int port, Action<ProtocolEntity, Stream> callback);
        /// <summary>
        /// send messages
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="protocolEntity"></param>
        void Send(MemoryStream stream, ProtocolEntity protocolEntity);
        void Terminate(string hostName, int port);
        void Destory();
    }
}
