using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;

namespace SatStat
{
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
        /// A list of all subscribers on this datastream
        /// </summary>
        private List<DataSubscription> subscriptions;

        /// <summary>
        /// An action to invoke when output is received on this stream
        /// </summary>
        private Action<string> OutputReceivedCallback;

        public DataStream()
        {
            subscriptions = new List<DataSubscription>();
            inputBuffer = new List<JObject>();
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

                    foreach (DataSubscription subscriber in subscriptions)
                    {
                        string attribute = subscriber.attribute;
                        if (key == attribute)
                        {
                            //Console.Write("Delivering to subscribers: ");
                            //Debug.Log(value);
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
        /// Add a subscriber to this data stream
        /// </summary>
        /// <param name="subscription">The subscriber to add to the list</param>
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