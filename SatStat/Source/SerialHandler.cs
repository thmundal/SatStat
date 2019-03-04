using System;
using System.Collections;
using System.Linq;
using System.IO.Ports;
using System.Management;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;

namespace SatStat
{
    public enum ConnectionStatus
    {
        Disconnected,
        Handshake,
        WaitingConnectInit,
        Connected,
    }

    /// <summary>
    /// A class that is responsible for communicating on a serial port connection, and at the same time act as a data stream object
    /// </summary>
    public class SerialHandler : DataStream
    {
        private SerialPort connection;
        private static Hashtable availableCOMPorts;
        private SerialSettingsCollection available_settings;
        private Action<SerialSettingsCollection> HandshakeResponse_callback;


        public SerialSettingsCollection AvailableSettings
        {
            get { return available_settings; }
        }

        private ConnectionStatus connectionStatus = ConnectionStatus.Disconnected;
        public ConnectionStatus ConnectionStatus
        {
            get { return connectionStatus;  }
        }

        private readonly Hashtable default_settings = new Hashtable()
        {
            {"BaudRate", 9600 },
            {"Parity", Parity.None },
            {"DataBits", 8 },
            {"StopBits", StopBits.One },
            {"NewLine", "\r\n" },
        };

        /// <summary>
        /// Set up serial port connection and configure default settings
        /// </summary>
        public SerialHandler()
        {
            connection = new SerialPort
            {
                // Set default protocol settings
                BaudRate = (int)default_settings["BaudRate"],
                Parity = (Parity)default_settings["Parity"],
                DataBits = (int)default_settings["DataBits"],
                StopBits = (StopBits)default_settings["StopBits"],
                NewLine = (string)default_settings["NewLine"],
                ReadTimeout = 1000
            };

            connection.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);

            availableCOMPorts = new Hashtable();
        }

        /// <summary>
        /// Set the desired COM port for communication
        /// </summary>
        /// <param name="portName">A string that descibes a valid COM port</param>
        public void SetComPort(string portName)
        {
            if (portName != null && portName != "")
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
            if (connectionStatus == ConnectionStatus.Disconnected || !connection.IsOpen)
            {
                Console.WriteLine("Serial not connected");
                return;
            }

            try
            {
                Console.WriteLine("Receiving...");
                string input = connection.ReadLine();

                Console.WriteLine(input);
                Console.WriteLine(ConnectionStatus);

                JObject inputParsed = Parse(input);

                if (connectionStatus == ConnectionStatus.Handshake && inputParsed.ContainsKey("serial_handshake"))
                {
                    available_settings = inputParsed["serial_handshake"].ToObject<SerialSettingsCollection>();
                    if(HandshakeResponse_callback != null)
                    {
                        InvokeHandshakeResponseCallback(available_settings);
                    }
                }

                if(ConnectionStatus == ConnectionStatus.WaitingConnectInit && inputParsed.ContainsKey("connect"))
                {
                    string param = inputParsed["connect"].ToObject<string>();
                    Console.WriteLine("Param received:");
                    Console.WriteLine(param);
                    if(param == "init")
                    {
                        Connect();
                    }
                }

                if(ConnectionStatus == ConnectionStatus.Connected)
                {
                    DeliverSubscriptions();
                }
            } catch(Exception ex)
            {
                Debug.Log("Serial.OnDataReceived exception captured\nSettings:");
                Debug.Log(connection.BaudRate);
                Debug.Log(ex);
                Debug.Log(connection.ReadExisting());
                connection.Close();
            }

            if (!connection.IsOpen)
            {
                if(Program.settings.baud_rate != 0)
                {
                    connection.BaudRate = Program.settings.baud_rate;
                    connection.Parity = (Parity)default_settings["Parity"];
                    connection.DataBits = (int)default_settings["DataBits"];
                    connection.StopBits = (StopBits)default_settings["StopBits"];
                    connection.NewLine = (string)default_settings["NewLine"];
                }
                
                Debug.Log("Reopening connection...");
                connection.Open();
            }
        }

        /// <summary>
        /// Register action to invoke when hanshake initialization request has received a response
        /// </summary>
        /// <param name="cb"></param>
        public void OnHandshakeResponse(Action<SerialSettingsCollection> cb)
        {
            HandshakeResponse_callback = cb;
        }

        /// <summary>
        /// Invoke procedure for when handshake returns to the client. Is needed for dispatcher pattern, since we are using threads/tasks in conjuction with Form Control update
        /// </summary>
        /// <param name="settings">Object containing settings for the COM connection</param>
        public void InvokeHandshakeResponseCallback(SerialSettingsCollection settings)
        {
            HandshakeResponse_callback.Invoke(available_settings);
        }

        /// <summary>
        /// Perform a connection on the serial port with default settings to initialize handshake
        /// </summary>
        /// <param name="portName"></param>
        public void DefaultConnect(string portName)
        {
            connection.BaudRate = (int)default_settings["BaudRate"];
            connection.Parity = (Parity)default_settings["Parity"];
            connection.DataBits = (int)default_settings["DataBits"];
            connection.StopBits = (StopBits)default_settings["StopBits"];
            connection.NewLine = (string)default_settings["NewLine"];

            connection.PortName = portName;
            connection.Open();

            connectionStatus = ConnectionStatus.Handshake;

            StartHandshake();
        }

        /// <summary>
        /// Initialize handshake
        /// </summary>
        public void StartHandshake()
        {
            string handshake = "{\"serial_handshake\":\"init\"}";

            Debug.Log("Initializing handshake...");
            Debug.Log(connectionStatus);

            WriteData(handshake);
        }

        /// <summary>
        /// Request connection on serial port with provided settings
        /// </summary>
        /// <param name="selected_settings"></param>
        public void ConnectionRequest(COMSettings selected_settings)
        {
            string settings = JSON.serialize(selected_settings);
            settings = "{\"connection_request\":"+settings+"}";
            WriteData(settings);
            connectionStatus = ConnectionStatus.WaitingConnectInit;
        }

        /// <summary>
        /// Send connection ok confirmation command to HW Layer
        /// </summary>
        /// <returns></returns>
        public void Connect()
        {
            WriteData("{\"connect\":\"ok\"}");

            connectionStatus = ConnectionStatus.Connected;
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
            if(connectionStatus != ConnectionStatus.Disconnected)
            {
                Debug.Log("writing data...");
                Debug.Log(data);
                connection.WriteLine(data);
            }
        }

        public new void Output(object data)
        {
            string data_serialized = JSON.serialize(data);
            WriteData(data_serialized);
        }

        /// <summary>
        /// Disconnect the serial port
        /// </summary>
        public void Disconnect()
        {
            if(connectionStatus != ConnectionStatus.Disconnected)
            {
                Console.WriteLine("Stopping serial reader");
                connectionStatus = ConnectionStatus.Disconnected;
                connection.Close();
            } else
            {
                Console.WriteLine("Cannot disconnect");
            }
        }
    }

    /// <summary>
    /// A struct containing fields for COM settings for serialization from JSON
    /// </summary>
    public struct SerialSettingsCollection
    {
        public int[] baud_rates;
        public string[] configs;
        public string[] newlines;
    }
}
