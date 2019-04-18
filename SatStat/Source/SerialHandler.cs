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

        public struct default_settings 
        {
            static public int BaudRate = 9600;
            static public Parity Parity = Parity.None;
            static public int DataBits = 8;
            static public StopBits StopBits = StopBits.One;
            static public string NewLine = "\r\n";
        };

        /// <summary>
        /// Set up serial port connection and configure default settings
        /// </summary>
        public SerialHandler()
        {
            connection = new SerialPort
            {
                // Set default protocol settings
                BaudRate = default_settings.BaudRate,
                Parity =  default_settings.Parity,
                DataBits = default_settings.DataBits,
                StopBits = default_settings.StopBits,
                NewLine = default_settings.NewLine,
                ReadTimeout = 1500
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
                ThreadHelperClass.SetCOMConnectionStatus("Disconnected");
                Debug.Log("Serial not connected");
                return;
            }

            try
            {
                //Debug.Log("Receiving...");
                string input = connection.ReadLine();

                //Debug.Log(input);
                Debug.Log(ConnectionStatus);

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
                    //Debug.Log("Param received:");
                    //Debug.Log(param);
                    if(param == "init")
                    {
                        Connect();
                    }
                }

                if(ConnectionStatus == ConnectionStatus.Connected)
                {
                    ThreadHelperClass.SetCOMConnectionStatus("Connected to " + Program.settings.portDescription);
                    DeliverSubscriptions();
                }
            }
            catch(Newtonsoft.Json.JsonReaderException ex)
            {
                Debug.Log("Invalid JSON");
                Debug.Log(ex);
            }
            catch (TimeoutException ex)
            {
                Debug.Log("Serial.OnDataReceived timeout exception captured\nSettings:");
                Debug.Log(connection.BaudRate);
                Debug.Log(ex);

                if(connection.IsOpen)
                {
                    Debug.Log(connection.ReadExisting());
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Exception caught in SerialHandler.OnDataReceived()");
                Debug.Log(ex);
            }

            if (!connection.IsOpen)
            {
                if(Program.settings.comSettings.baud_rate != 0)
                {
                    connection.BaudRate = Program.settings.comSettings.baud_rate;
                    connection.Parity = default_settings.Parity;
                    connection.DataBits = default_settings.DataBits;
                    connection.StopBits = default_settings.StopBits;
                    connection.NewLine = default_settings.NewLine;
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
            connection.BaudRate = default_settings.BaudRate;
            connection.Parity = default_settings.Parity;
            connection.DataBits = default_settings.DataBits;
            connection.StopBits = default_settings.StopBits;
            connection.NewLine = default_settings.NewLine;

            connection.PortName = portName;

            try
            {
                connection.Open();
                connectionStatus = ConnectionStatus.Handshake;

                StartHandshake();
            }
            catch (System.IO.IOException e)
            {
                Debug.Log(e.ToString());
            }            
        }

        /// <summary>
        /// Initialize handshake
        /// </summary>
        public void StartHandshake()
        {
            string handshake = "{\"serial_handshake\":\"init\"}";

            Debug.Log("Initializing handshake...");
            Debug.Log(connectionStatus);

            ThreadHelperClass.SetCOMConnectionStatus("Initializing handshake");

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
            Debug.Log("Running connect method");
            WriteData("{\"connect\":\"ok\"}");
            WriteData("{\"request\":\"available_data\"}");

            ThreadHelperClass.SetCOMConnectionStatus("Waiting for available data");

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
                Debug.Log("Stopping serial reader");
                connectionStatus = ConnectionStatus.Disconnected;
                connection.Close();
            } else
            {
                Debug.Log("Cannot disconnect");
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
