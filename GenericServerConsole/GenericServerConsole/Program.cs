using System;
using System.Threading;
using System.Reflection;
using Lib.Sockets.UI;


namespace GenericServerConsole
{
    class Program
    {
        //[STAThread] //This is required else WPF window does not load 
        static void Main(string[] args)
        {
            Console.WriteLine("Generic socket server/client version " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Console.WriteLine("Questions/comments to Chaitanya Belwal, cbelwal@gmail.com");
            if (args.Length==0)
            {
                Console.WriteLine(GetHelpText());
                return;
            }            
            switch (args[0])
            {
                case "-i":
                    ViewInformation(args);
                    break;

                case "-server":
                    //SetInformation(args);
                    break;

                case "-gui":
                    LaunchGUI();
                    return;                    

                default:
                    Console.WriteLine(GetHelpText());
                    break;
            }           
        }
        
        private static void LaunchGUI()
        {
            UIAccess uia = new UIAccess();                      
            uia.ShowSocketUI();            
        }

        private static void ViewInformation(string[] args)
        {
            SocketInfo si = new SocketInfo();
            string flag="";
            if(args.Length >=2)
            {
                flag = args[1];
            }

            switch (flag)
            {
                case "h":
                    si.PrintHostName();
                    break;
                case "ip":
                    si.PrintAllIPAddress();
                    break;
                default:
                    si.PrintHostName();
                    si.PrintAllIPAddress();
                    break;
            }                     
        }
        
        private static string GetHelpText()
        {
            return "";
        }

    }
}
