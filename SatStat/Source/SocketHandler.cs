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
        private Socket listener;
        private IPEndPoint localEndPoint;
        private List<Socket> connected_clients;

        public SocketHandler()
        {
            connected_clients = new List<Socket>();

            // https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            listen_addr = ipHostInfo.AddressList[0];
            listen_port = 11000;
            localEndPoint = new IPEndPoint(listen_addr, listen_port);

            // Create socket
            listener = new Socket(listen_addr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Host name is {0}", Dns.GetHostName());

            Task.Run(() =>
            {
                StartListening();
            });

        }

        public async Task StartListening()
        {

            // Bind socket
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    allDone.Reset();
                    Console.WriteLine("Waiting for connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();  // Block thread until signal is received, aka allDone.Set()
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set(); // Continue main socket handler thread

            Socket listener = (Socket)ar.AsyncState;
            Socket client = listener.EndAccept(ar);

            IPEndPoint clientEndpoint = (IPEndPoint) client.RemoteEndPoint;
            string clientHostname = Dns.GetHostEntry(clientEndpoint.Address).HostName;

            StateObject state = new StateObject();
            state.workSocket = client;
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);

            connected_clients.Add(client);

            Console.Write("A client connected: {0}", clientHostname);
            Send(client, "You are now connected");
        }

        public void ReadCallback(IAsyncResult ar)
        {
            string content = String.Empty;
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;

            int bytesRead = client.EndReceive(ar);

            if(bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                content = state.sb.ToString();
                if(content.IndexOf("<EOF>") > -1)
                {
                    Console.WriteLine("Read {0} bytes from socket. \n Data: {1}", content.Length, content);

                    Send(client, "This is a response to your message");
                    // Run deliver subs here...
                    //Parse(content);
                    //DeliverSubscriptions();
                } else
                {
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }

        public void Send(Socket handler, string data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        public void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;

                int bytesSent = handler.EndSend(ar);

                // Not sure we want to do this now....
                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();

            } catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Disconnect(Socket client)
        {
            connected_clients.Remove(client);
            client.Shutdown(SocketShutdown.Both);
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
