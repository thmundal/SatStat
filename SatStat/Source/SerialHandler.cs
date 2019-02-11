using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System.Management;

namespace SatStat
{
    class SerialHandler : DataStream
    {
        private string input = "";

        private Action<object> dataReceivedCallback;


        private Thread readThread;
        private bool connected;
        private SerialPort connection;

        private static Hashtable availableCOMPorts;

        public SerialHandler()
        {
            connection = new SerialPort();
            connected = false;
            //PortName = "";
            connection.BaudRate = 1115200;
            connection.Parity = Parity.None;
            connection.DataBits = 8;
            connection.StopBits = StopBits.One;

            readThread = new Thread(ReadData);
            availableCOMPorts = new Hashtable();
        }

        public void SetComPort(string portName)
        {
            if(portName != null && portName != "")
            {
                connection.PortName = portName;
            } else
            {
                throw new ArgumentException("Port name cannot be empty or null");
            }
        }
        
        public void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            input = connection.ReadLine();
            dataReceivedCallback.Invoke(input);
        }
        public bool Connect()
        {
            //string port = Program.settings.selectedComPort;
            if (connection.PortName != "")
            {
                Console.WriteLine("Starting SerialHandler read thread");

                //Set the datareceived event handler
                //sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                //Open the serial port
            
                connection.Open();

                connected = true;
                readThread.Start();
                return true;
            }

            return false;
        }

        public static Hashtable GetPortListInformation() {
            // https://stackoverflow.com/a/2876126
            var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort");
            
            string[] portnames = SerialPort.GetPortNames();
            var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
            var tList = (from n in portnames
                            join p in ports on n equals p["DeviceID"].ToString()
                            select n + " - " + p["Caption"]).ToList();

            foreach(ManagementBaseObject p in ports)
            {
                if(!availableCOMPorts.ContainsKey(p["DeviceID"]))
                {
                    availableCOMPorts.Add(p["DeviceID"], p["Caption"]);
                }
            }
            
            
            return availableCOMPorts;
        }
        

        public void ReadData()
        {
            while(connected && connection.IsOpen)
            {
                input = connection.ReadLine();

                //object inputObject = JSON.parse<object>(input);

                //dataReceivedCallback.Invoke(input);
                Parse(input);
                DeliverSubscriptions();
            }

            Console.WriteLine("SerialHandler read thread stopped");
        }

        public void WriteData(string data)
        {
            if(connection.PortName != "")
            {
                connection.WriteLine(data);
            }
        }

        public void Stop()
        {
            if(connected)
            {
                connected = false;
                connection.Close();
            }
        }

        public string GetData()
        {
            return input;
        }

        public void OnDataReceived(Action<object> cb)
        {
            dataReceivedCallback = cb;
        }
    }
}
