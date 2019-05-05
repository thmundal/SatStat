using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;

namespace SatStat
{
    public struct ConnectionParameters
    {
        public string com_port;
        public string listen_addr;
    }

    public enum ConnectionStatus
    {
        Disconnected,
        Handshake,
        WaitingConnectInit,
        Connected,
    }

    /// <summary>
    /// A datastream that can be subscibed to trough a DataSubscription
    /// </summary>
    public class DataStream
    {
        /// <summary>
        /// A buffer containing received input
        /// </summary>
        private List<JObject> inputBuffer;

        /// <summary>
        /// A list of receivers that should receive data on this stream
        /// </summary>
        private List<DataReceiver> receivers;

        protected static int num_active_streams = 0;
        public static int NumActiveStreams
        {
            get
            {
                return num_active_streams;
            }
        }

        protected string stream_label = "DataStream_" + num_active_streams;
        public string Label
        {
            get
            {
                return stream_label;
            }
        }

        /// <summary>
        /// An action to invoke when output is received on this stream
        /// </summary>
        private Action<string> OutputReceivedCallback;
        protected Action<DataStream> OnConnected_cb;
        protected Action<DataStream> OnDisconnected_cb;
        protected ConnectionStatus connectionStatus = ConnectionStatus.Disconnected;

        public ConnectionStatus ConnectionStatus {
            get
            {
                return connectionStatus;
            }
        }

        public DataStream()
        {
            inputBuffer = new List<JObject>();
            receivers = new List<DataReceiver>();
            num_active_streams++;
        }

        ~DataStream()
        {
            num_active_streams--;
        }

        /// <summary>
        /// Deliver data requested to all subscribers
        /// </summary>
        public void DeliverSubscriptions()
        {
            for(int i=0; i<inputBuffer.Count; i++)
            {
                var input = inputBuffer[i];

                foreach(KeyValuePair<string, JToken> item in input)
                {
                    string key = item.Key;
                    object value = item.Value;
                    
                    foreach(DataReceiver receiver in receivers)
                    {
                        if(receiver.HasSubscription(key))
                        {
                            receiver.GetSubscription(key).receive(value, stream_label);
                        } 
                    }
                }

                inputBuffer.RemoveAt(i);
            }
        }

        /// <summary>
        /// Parse input string as json key/value pairs and put the parsed data in the input buffer
        /// </summary>
        /// <param name="input">The json string data to be parsed</param>
        /// <returns>Returns a JObject containing parsed data</returns>
        public JObject Parse(string input)
        {
            if(input.Length > 0)
            {
                try
                {
                    JObject inputParsed = JObject.Parse(input);
                    inputBuffer.Add(inputParsed);
                    return inputParsed;
                }
                catch (JsonSerializationException e)
                {
                    Debug.Log("Invalid json received:" + input);
                    Debug.Log(e.ToString());
                }
                catch (JsonReaderException e)
                {
                    Debug.Log("Invalid json received:" + input);
                    Debug.Log(e.ToString());
                }
                catch (System.IO.IOException e)
                {
                    Debug.Log("Read thread aborted");
                    Debug.Log(e.ToString());
                }
                catch(Exception e)
                {
                    Debug.Log(e.ToString());
                }
            } else
            {
                Debug.Log("Input is empty, aborting parse");
            }
            return null;
        }
        
        /// <summary>
        /// Add a receiver on this data stream
        /// </summary>
        /// <param name="r">A data receiver that should receive data on this stream</param>
        public void AddReceiver(DataReceiver r)
        {
            if(!receivers.Contains(r))
            {
                receivers.Add(r);
            }
        }

        /// <summary>
        /// Send data trough the output channel of this data stream as a JSON serialized string
        /// </summary>
        /// <param name="data">Data object to serialize and send as JSON</param>
        public virtual void Output(object data) { }

        protected virtual bool ConnectProcedure(ConnectionParameters prm) { return false; }
        protected virtual bool DisconnectProcedure() { return false; }

        public void Connect(ConnectionParameters prm = new ConnectionParameters())
        {
            if(ConnectProcedure(prm))
            {
                connectionStatus = ConnectionStatus.Connected;
                if (OnConnected_cb != null)
                {
                    OnConnected_cb.Invoke(this);
                }
                Debug.Log("Connected");
            }

        }

        public void Disconnect()
        {
            if(DisconnectProcedure())
            {
                connectionStatus = ConnectionStatus.Disconnected;
                if (OnDisconnected_cb != null)
                {
                    OnDisconnected_cb.Invoke(this);
                }
            }
        }

        public void OnConnected(Action<DataStream> cb)
        {
            OnConnected_cb = cb;
        }

        public void OnDisconnected(Action<DataStream> cb)
        {
            OnDisconnected_cb = cb;
        }
        
        /// <summary>
        /// Register a callback function to invoke whenever this stream receives data for output
        /// </summary>
        /// <param name="cb">Data serialized to JSON string</param>
        public void OnOutputReceived(Action<string> cb)
        {
            OutputReceivedCallback = cb;
        }

        public override string ToString()
        {
            return stream_label;
        }
    }
}