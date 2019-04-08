using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SatStat
{
    /// <summary>
    /// Socket handler class
    /// </summary>
    public class SocketHandler : DataStream
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        
        private IPAddress listen_addr;
        private int listen_port;
        private IPEndPoint localEndPoint;

        private List<TcpClient> connected_clients;
        private Thread serverListenerThread;
        private TcpListener server;

        public SocketHandler()
        {
            connected_clients = new List<TcpClient>();

            // https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            listen_addr = ipHostInfo.AddressList[0];
            listen_port = 11000;
            localEndPoint = new IPEndPoint(listen_addr, listen_port);

            Console.WriteLine("Host name is {0}", Dns.GetHostName());

            StartServer();

        }

        public void StartServer()
        {
            try
            {
                serverListenerThread = new Thread(new ThreadStart(StartListening));
                serverListenerThread.IsBackground = true;
                serverListenerThread.Start();
            } catch(Exception e)
            {
                Debug.Log(e);
            }
        }

        public void StartListening()
        {

            // Bind socket
            try
            {
                ThreadHelperClass.SetNetworkConnectionStatus("Listening on port " + listen_port.ToString());

                server = new TcpListener(IPAddress.Loopback, listen_port);
                server.Start();

                Byte[] bytes = new byte[1024];
                string data = null;

                while(true)
                {
                    Debug.Log("Waiting for a connection....");
                    TcpClient client = server.AcceptTcpClient();
                    connected_clients.Add(client);
                    Debug.Log("A client connected");

                    ThreadHelperClass.SetNetworkConnectionStatus("Client connected");

                    data = null;

                    NetworkStream stream = client.GetStream();
                    int i;
                    while((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        string[] buffer = data.Split('\n');

                        // Send to subscribers here...
                        foreach(string b in buffer)
                        {
                            if(b.Length > 0)
                            {
                                if(b.StartsWith("{") && b.EndsWith("}"))
                                {
                                    Parse(b);
                                    DeliverSubscriptions();
                                }
                            }
                        }
                        buffer = null;
                    }

                    connected_clients.Remove(client);
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
        

        public void Send(string data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            
            foreach(TcpClient client in connected_clients)
            {
                NetworkStream stream = client.GetStream();
                stream.Write(byteData, 0, byteData.Length);
            }
        }
        

        public void Disconnect(TcpClient client)
        {
            ThreadHelperClass.SetNetworkConnectionStatus("Disconnected");
            connected_clients.Remove(client);
            client.Close();
        }
    }

    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }
}
