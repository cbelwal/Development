using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lib.Sockets.UI
{
    /// <summary>
    /// Interaction logic for SocketMainUI.xaml
    /// </summary>
    public partial class SocketUIControl : UserControl
    {
        public SocketUIControl()
        {
            InitializeComponent();
            //Get ViewModel and Add Search Update function
            VMMain vm = this.DataContext as VMMain;
            vm.OnRXTextHighlight += new VMMain.RXHighlightTextHandler(RXHighlightText);
        }


        private void RXHighlightText(object sender, int start, int end)
        {
            txtRX.Focus();            
            txtRX.Select(start, end);            
        }

        //Not the best of ways, will address later
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).ScrollToEnd();
        }
    }
}
