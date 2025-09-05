using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace TuioSimulator.Tuio.Common
{
    public static class IpHelper
    {
        private static HashSet<string> _localIpAddresses;
        public static HashSet<string> LocalIpAddresses
        {
            get
            {
                _localIpAddresses = new HashSet<string> { "127.0.0.1" };
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        _localIpAddresses.Add(ip.ToString());
                    }
                }
                return _localIpAddresses;
            }
        }
    }
}