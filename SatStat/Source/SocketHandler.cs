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
                Console.WriteLine(e);
            }
        }

        public void StartListening()
        {

            // Bind socket
            try
            {
                server = new TcpListener(listen_addr, listen_port);
                server.Start();

                Byte[] bytes = new byte[1024];
                string data = null;

                while(true)
                {
                    Console.WriteLine("Waiting for a connection....");
                    TcpClient client = server.AcceptTcpClient();
                    connected_clients.Add(client);
                    Console.WriteLine("A client connected");

                    data = null;

                    NetworkStream stream = client.GetStream();
                    int i;
                    while((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.ASCII.GetString(bytes, 0, i);

                        // Send to subscribers here...
                        Parse(data);
                        DeliverSubscriptions();
                        Console.WriteLine("Received: {0}", data);
                    }

                    // Probably not close right away...
                    //client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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
