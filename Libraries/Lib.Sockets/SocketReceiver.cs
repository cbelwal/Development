using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.Sockets
{
    public class SocketReceiver
    {
        private TcpListener _serverSocket;
        private Thread _masterThread;
        private NetworkStream _stream;

        IPAddress _ipv4Address;
        short _port;

        public delegate void StatusUpdateHandler(object sender,
            string status);
        public event StatusUpdateHandler OnStatusUpdate;

        public delegate void DataRXHandler(object sender,
            string rxData);
        public event DataRXHandler OnRX;

        public delegate void SocketExceptionHandler(object sender, string exceptionText);
        public event SocketExceptionHandler OnSocketException;


        public SocketReceiver()
        {
            
        }

        public void SetDataToSend(string textToSend)
        {           
            if (textToSend.CompareTo("") != 0)
            {
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(textToSend
                    );
                // Send back a response.
                _stream.Write(msg, 0, msg.Length);
                OnStatusUpdate(this, msg.Length + " byte(s) sent over connection");
             
            }
        }

        public void StopServer()
        {
            if(_masterThread != null)
            {
                OnStatusUpdate(this, "Aborting server master thread ...");
                if(_serverSocket != null)
                    _serverSocket.Stop();
                if (_masterThread.IsAlive)
                {                   
                    _masterThread.Abort();
                    OnStatusUpdate(this, "Master server thread aborted"); 
                }
            }
        }

        public void CreateServer (string ipv4Address,
                                short port)
        {            
               IPAddress ip = SocketTools.GetIPAddress(ipv4Address);
               CreateServer(ip, port);           
        }

        public void CreateServer(IPAddress ipv4Address, 
                                short port)
        {
            try
            {
                _ipv4Address = ipv4Address;
                _port = port;
                _serverSocket = new TcpListener(_ipv4Address, _port);

                _masterThread = new Thread(() =>
                {
                    StartServer();
                });
                _masterThread.Start();
                OnStatusUpdate(this, "Master server thread started");
            }
            catch(Exception ex)
            {
                _serverSocket.Stop();
                OnSocketException(this, "Error in creating server," + ex.Message);
            }
        }   
        

        private void StartServer()
        {
            try
            {                
                _serverSocket.Start();

                // Buffer for reading data
                byte[] bytes = new byte[1024];
                string data;

                //Enter the listening loop 
                while (true) //Will continue listening till thread is killed
                {
                    OnStatusUpdate(this, "Waiting for a connection on " + _ipv4Address.ToString() + " port number " + _port.ToString() + "...");

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = _serverSocket.AcceptTcpClient();
                    OnStatusUpdate(this, "Connected");

                    // Get a stream object for reading and writing
                    _stream = client.GetStream();

                    int bCount;

                    // Loop to receive all the data sent by the client.
                    bCount = _stream.Read(bytes, 0, bytes.Length);

                    while (bCount != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, bCount);
                        OnRX(this, data);

                       
                        bCount = _stream.Read(bytes, 0, bytes.Length);
                        System.Threading.Thread.Sleep(300);
                    }

                    // Shutdown and end connection
                    client.Close();
                    OnStatusUpdate(this, "Connection closed");
                }
                //Thread should be killed here
            }
            catch (SocketException ex)
            {                               
                OnSocketException(this,"Error in creating listen port," + ex.Message); 
                //Thread will end automatically
            }
        }
    }
}
