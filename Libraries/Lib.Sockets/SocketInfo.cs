using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Lib.Sockets
{
    public static class SocketInfo
    {
        public static List<IPAddress> GetAllLocalIPAddress()
        {
            List<IPAddress> allIPs = new List<IPAddress>();
            var host = Dns.GetHostEntry(Dns.GetHostName());
            try
            {
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        allIPs.Add(ip);
                    }
                }
            }
            catch
            {

            }
            return allIPs;
        }

        public static List<string> GetAllLocalIPAddressString()
        {
            List<string> allIPs = new List<string>();            
            var host = Dns.GetHostEntry(Dns.GetHostName());
            try
            {
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        
                        allIPs.Add(ip.ToString() + "\r\n");
                    }
                }
            }
            catch
            {

            }
            return allIPs;
        }

        public static string GetHostName()
        {           
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());            
            return host.HostName;                            
        }


    }
}
