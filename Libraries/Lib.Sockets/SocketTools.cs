using System.Net;

namespace Lib.Sockets
{
    public static class SocketTools
    {
        public static IPAddress GetIPAddress(string ipAddress)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            return ip;
        }
    }
}
