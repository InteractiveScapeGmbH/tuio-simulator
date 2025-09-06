using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

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
                try
                {
                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (var ip in host.AddressList)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            _localIpAddresses.Add(ip.ToString());
                        }
                    }

                }
                catch (Exception e)
                {
                    Debug.LogWarning(e);
                }
                return _localIpAddresses;
            }
        }
    }
}