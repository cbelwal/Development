using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lib.Sockets.UI
{
    public class UIAccess
    {
        public void ShowSocketUI()
        {
            //SocketUIMainWindow uiMain = new SocketUIMainWindow();
            //uiMain.Show();
            Thread thread = new Thread(() =>
            {
                SocketUIMainWindow uiMain = new SocketUIMainWindow();
                uiMain.Show();

                uiMain.Closed += (sender2, e2) =>
                    uiMain.Dispatcher.InvokeShutdown();

                System.Windows.Threading.Dispatcher.Run();
            });
             
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join(); //Block till window closed
            thread = null;
        }
    }
}
