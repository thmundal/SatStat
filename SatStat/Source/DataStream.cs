using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Serialization;

namespace SatStat
{
    /// <summary>
    /// A datastream that can be subscibed to trough a DataSubscription
    /// </summary>
    public class DataStream
    {
        private List<Dictionary<string, object>> inputBuffer;
        private List<DataSubscription> subscriptions;
        private Action<string> OutputReceivedCallback;

        public DataStream()
        {
            subscriptions = new List<DataSubscription>();
            inputBuffer = new List<Dictionary<string, object>>();

        }

        /// <summary>
        /// Deliver data requested to all subscribers
        /// </summary>
        public void DeliverSubscriptions()
        {
            for(int i=0; i<inputBuffer.Count; i++)
            {
                var input = inputBuffer[i];

                foreach(KeyValuePair<string, object> item in input)
                {
                    string key = item.Key;
                    object value = item.Value;

                    foreach (DataSubscription subscriber in subscriptions)
                    {
                        string attribute = subscriber.attribute;
                        if (key == attribute)
                        {
                            Console.Write("Delivering to subscribers: ");
                            Console.WriteLine(value);
                            subscriber.receive(value);
                        }
                    }
                }

                inputBuffer.RemoveAt(i);
            }
        }


        /// <summary>
        /// Parse input string as json key/value pairs and put the parsed data in the input buffer
        /// </summary>
        /// <param name="input"></param>
        public void Parse(string input)
        {   
            if(input.Length > 0)
            {
                try
                {
                    Dictionary<string, object> inputParsed = JSON.parse<Dictionary<string, object>>(input);
                    inputBuffer.Add(inputParsed);
                }
                catch (JsonSerializationException)
                {
                    Console.WriteLine("Invalid json received:" + input);
                }
                catch (JsonReaderException)
                {
                    Console.WriteLine("Invalid json received:" + input);
                }
                catch (System.IO.IOException)
                {
                    Console.WriteLine("Read thread aborted");
                }
            } else
            {
                Console.WriteLine("Input is empty, aborting parse");
            }
        }

        /// <summary>
        /// Add a subscriber to this data stream
        /// </summary>
        /// <param name="subscription"></param>
        public void AddSubscriber(DataSubscription subscription)
        {
            subscriptions.Add(subscription);
        }

        /// <summary>
        /// Send data trough the output channel of this data stream as a JSON serialized string
        /// </summary>
        /// <param name="data">Data object to serialize and send as JSON</param>
        public void Output(object data)
        {
            string json_serialized = JSON.serialize(data);
            //outputBuffer.Add(json_serialized);

            if(OutputReceivedCallback != null)
            {
                OutputReceivedCallback.Invoke(json_serialized);
            }
        }

        /// <summary>
        /// Register a callback function to invoke whenever this stream receives data for output
        /// </summary>
        /// <param name="cb">Data serialized to JSON string</param>
        public void OnOutputReceived(Action<string> cb)
        {
            OutputReceivedCallback = cb;
        }
    }
}