using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Lib.Sockets;

namespace Lib.Sockets.UI
{
    public class VMMain : INotifyPropertyChanged, IDisposable
    {
        private string _ipAddress;
        private short _port;
        private string _startStopText;
        private StringBuilder _completeStatus;
        private SocketReceiver _sr;


        public VMMain()
        {
            InitVariables();
            InitCommands();
        }
       

        private void InitVariables()
        {
            _ipAddress = AppSettings.Default.IPAddress;
            if (_ipAddress == "")
            {
                List<String> allIPs = Lib.Sockets.SocketInfo.GetAllLocalIPAddressString();
                if (allIPs != null && allIPs.Count > 0)
                    _ipAddress = allIPs[0];
                else
                    _ipAddress = "0.0.0.0";
            }
            _port =  AppSettings.Default.Port;
            
            _startStopText = Resources.SocketUIControlStrings.lblStart;

            EncodingEntries = new List<string>();
            EncodingEntries.Add("UTF-8");
            EncodingEntries.Add("UTF-16");
            EncodingEntries.Add("UTF-32");
            EncodingEntry = EncodingEntries[0];
            OnPropertyChanged("EncodingEntry");
            _completeStatus = new StringBuilder();
            ServerMode = true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        //Destructor/Finalizer will be called when GC destroys objects
        ~VMMain()
        {
            //Store settings 
            AppSettings.Default.Port = _port;
            AppSettings.Default.IPAddress = _ipAddress;
            AppSettings.Default.Save();
        }

        public void Dispose()
        {
        }

        #region Command Declarations
        public ICommand StartStopCommand { get; private set; }
        public ICommand SendDataCommand { get; private set; }
        public ICommand RXClearCommand { get; private set; }
        public ICommand RXCopyToClipCommand { get; private set; }
        private void InitCommands()
        {
            StartStopCommand = new RelayCommand(ExecuteStartStopCommand);
            SendDataCommand = new RelayCommand(ExecuteSendDataCommand);
            RXClearCommand = new RelayCommand(ExecuteRXClearCommand);
            RXCopyToClipCommand = new RelayCommand(ExecuteRXCopyToClipCommand);
        }

        private void ExecuteRXClearCommand(object obj)
        {
            RX = "";
            OnPropertyChanged("RX");
        }

        private void ExecuteRXCopyToClipCommand(object obj)
        {
            if (RX != null)
            {
                System.Windows.Forms.Clipboard.SetText(RX);
                SetStatus("RX data copied to clipboard ...");
            }
        }

        private void ExecuteSendDataCommand(object obj)
        {
            SendDataOverConnection();
        }

        private void ExecuteStartStopCommand(object obj)
        {
            if (_startStopText.CompareTo(Resources.SocketUIControlStrings.lblStart) == 0) //They match
            {
                StartService();                               
            }
            else
            {
                StopService();                
            }
            FlipStartStopText();
        } 
        
        private void FlipStartStopText()
        {
            if (_startStopText.CompareTo(Resources.SocketUIControlStrings.lblStart) == 0) //They match
                _startStopText = Resources.SocketUIControlStrings.lblStop;
            else
                _startStopText = Resources.SocketUIControlStrings.lblStart;
            OnPropertyChanged("StartStopText");
        }
        #endregion

        #region Binded properties
        public List<String> EncodingEntries { get; set; }
        public String EncodingEntry { get; set; }
        public String TX { private get; set; }
        public String RX { get; private set; }
        public String Status { get; private set; }
        public bool ServerMode { get; set; }

        public String CompleteStatus
        {
            get
            {
                return _completeStatus.ToString();
            }
        }
       
        public string IPAddress
        {
            get
            {
                return _ipAddress;
            }
            set
            {
                _ipAddress = value;
                OnPropertyChanged("IPAddress");
            }
        }

        public short Port {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }
       
        public string StartStopText
        {
            get
            {
                return _startStopText;
            }
            set
            {
                _startStopText = value;
            }
        }

        #endregion Binded properties

        
        private void StartService()
        {
            if (ServerMode)
                StartListenerService();
        }

        private void StopService()
        {
            if (ServerMode)
                StopListenerService();
        }

        private void SendDataOverConnection()
        {
            if (ServerMode)
                SendDataToServer(TX);
        }

        #region Socket Server Related
        private void StartListenerService()
        {
            try
            {
                SetStatus("Starting socket server ...");
                _sr = new SocketReceiver();
                SetStatus("Created socket receiver object ...");
                _sr.OnRX += new SocketReceiver.DataRXHandler(OnServerRX);
                _sr.OnStatusUpdate += new SocketReceiver.StatusUpdateHandler(OnStatusUpdate);
                _sr.OnSocketException += new SocketReceiver.SocketExceptionHandler(OnListenSocketException);
                _sr.CreateServer(_ipAddress, _port);
            }
            catch //Due to Async op of socket entry will not be here, but just in case
            {
                //Change button to start again as something as gone wrong
                FlipStartStopText();
            }
        }        
        
        private void SendDataToServer(string textToSend)
        {
             _sr.SetDataToSend(textToSend);
        }

        private void OnListenSocketException(object obj, string exception)
        {
            SetStatus("Exception found: " + exception);
            FlipStartStopText();
            //Do not create thread to abort as it goes into a deadlock. Thread is killed automatically
        }

        private void OnServerRX(object obj, string data)
        {
            RX = RX + data;
            OnPropertyChanged("RX");
        }

        private void StopListenerService()
        {
            SetStatus("Stopping server ...");
            if (_sr != null)
                _sr.StopServer();          
        }

        #endregion Socket Server Related


        private void OnStatusUpdate(object obj, string status)
        {
            SetStatus(status);
        }

        private void SetStatus(string status)
        {
            Status = status;
            _completeStatus.Append(status + "\r\n");
            OnPropertyChanged("Status");
            OnPropertyChanged("CompleteStatus");
        }
    }
}
