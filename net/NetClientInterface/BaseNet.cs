using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetClient
{
    public class BaseNet
    {
        protected IPEndPoint GetIpEndPoint(string hostName, int port)
        {
            IPAddress ipaddr = null;
            if (string.IsNullOrWhiteSpace(hostName) || hostName == "localhost" || hostName.StartsWith("127.") || hostName.StartsWith("255."))
                ipaddr = IPAddress.Any;// Dns.GetHostEntry(s).AddressList[0];
            else
                ipaddr = IPAddress.Parse(hostName);
            return new IPEndPoint(ipaddr, port);
        }
        protected IPAddress GetIp(string hostName)
        {
            IPAddress ipaddr = null;
            if (string.IsNullOrWhiteSpace(hostName) || hostName == "localhost" || hostName.StartsWith("127.") || hostName.StartsWith("255."))
                ipaddr = IPAddress.Any;// Dns.GetHostEntry(s).AddressList[0];
            else
                ipaddr = IPAddress.Parse(hostName);
            return ipaddr;
        }
    }
}
