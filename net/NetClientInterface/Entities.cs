namespace NetClient.Entity
{
    public class ProtocolEntity
    {
        /// <summary>
        /// Object of Server&Client likes TCPClient, UdpClient
        /// </summary>
        public object Sender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RemoteHostName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RemotePort { get; set; }

        public string LocalHostName { get; set; }

        public int LocalPort { get; set; }
    }
}
