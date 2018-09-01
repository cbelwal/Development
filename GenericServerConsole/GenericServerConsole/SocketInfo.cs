using System;
using System.Collections.Generic;
using Lib.Sockets;

namespace GenericServerConsole
{
    public class SocketInfo
    {
        public void PrintAllIPAddress()
        {
            List<String> allIPs = Lib.Sockets.SocketInfo.GetAllLocalIPAddressString();

            if(allIPs != null)
            {
                foreach(string ip in allIPs)
                    Console.WriteLine(ip);
            }
            
        }

        public void PrintHostName()
        {
            Console.WriteLine("Hostname: "+ Lib.Sockets.SocketInfo.GetHostName());
        }
       
    }
}
