using System;
using System.Net.Sockets;
using System.Threading;
using Xunit;

namespace TcpClientTest
{
    public class UnitTest1
    {
        NetClient.TcpClient Client;
       public UnitTest1()
        {
            Client = new NetClient.TcpClient();
            Client.BeginAccept("127.0.0.1", 22012, (e, s) =>
        {
        });
        }
        [Fact]
        public void Test1()
        {
            var c = new TcpClient();
            c.Connect("127.0.0.1", 22012);
            c.GetStream().Write(new byte[] { 112, 221, 222 });
            c.Close();
            Thread.Sleep(61000);
            Assert.Equal(0, Client.ClientCount());
        }
    }
}
