using System;
using System.Collections;
using System.Linq;
using System.IO.Ports;
using System.Management;

namespace SatStat
{
    /// <summary>
    /// A class that is responsible for communicating on a serial port connection, and at the same time act as a data stream object
    /// </summary>
    public class SerialHandler : DataStream
    {
        private bool connected;
        private SerialPort connection;
        private static Hashtable availableCOMPorts;

        /// <summary>
        /// Set up serial port connection and configure default settings
        /// </summary>
        public SerialHandler()
        {
            connection = new SerialPort();
            connected = false;
            //PortName = "";
            connection.BaudRate = 9600;
            connection.Parity = Parity.None;
            connection.DataBits = 8;
            connection.StopBits = StopBits.One;
            connection.NewLine = "\r\n";
            connection.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);

            availableCOMPorts = new Hashtable();
        }

        /// <summary>
        /// Set the desired COM port for communication
        /// </summary>
        /// <param name="portName">A string that descibes a valid COM port</param>
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
        
        /// <summary>
        /// The method to invoke when data is received on the serial port communication channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string input = "";
            try
            {
                input = connection.ReadLine();

                Parse(input);
                DeliverSubscriptions();
            }
            catch (Newtonsoft.Json.JsonSerializationException)
            {
                Console.WriteLine("Invalid json received:" + input);
            }
            catch(Newtonsoft.Json.JsonReaderException)
            {
                Console.WriteLine("Invalid json received:" + input);
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("Read thread aborted");
            }
        }

        /// <summary>
        /// Connect to the serial port channel if a valid port name is set
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            if (connection.PortName != "")
            {
                connected = true;
                connection.Open();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get a list of available COM ports together with a description of what device is connected on these ports
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Write data to the serial port connection
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(string data)
        {
            if(connection.PortName != "")
            {
                connection.WriteLine(data);
            }
        }

        /// <summary>
        /// Disconnect the serial port
        /// </summary>
        public void Disconnect()
        {
            if(connected)
            {
                Console.WriteLine("Stopping serial reader");
                connected = false;
                connection.Close();
            }
        }
    }
}
