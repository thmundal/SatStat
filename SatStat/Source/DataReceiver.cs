using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    public class DataReceiver
    {
        /// <summary>
        /// This callback defines the operation to be performed on received data after the data has been received trough a subscription delivery
        /// </summary>
        public Action<object, string> OnPayloadReceived_Callback;

        /// <summary>
        /// Holds all active subscription objects
        /// </summary>
        private List<DataSubscription> subscriptions;

        /// <summary>
        /// A list of strings referring to active subscriptions, to more easily/cheaply identify what subscriptions this receiver has active
        /// </summary>
        private List<string> active_subscriptions;
        
        public DataReceiver()
        {
            subscriptions = new List<DataSubscription>();
            active_subscriptions = new List<string>();
        }

        /// <summary>
        /// Subscribe to data on a stream with a given attribute and data type
        /// </summary>
        /// <param name="stream">The data stream to receive data trough</param>
        /// <param name="attribute">The attribute to receive data from in the data stream</param>
        /// <param name="data_type">The datatype the data is represented as</param>
        public void Subscribe(DataStream stream, string attribute, string data_type)
        {
            DataSubscription sub = new DataSubscription(this, attribute, data_type);
            stream.AddSubscriber(sub);
            subscriptions.Add(sub);
            active_subscriptions.Add(attribute);
        }

        /// <summary>
        /// Unsubscribe from an attribute
        /// </summary>
        /// <remarks>
        /// This method should be expanded to unsubscribe from an attribute on a specific stream, in the case where this receiver subscribes on different streams
        /// </remarks>
        /// <param name="attribute">The attribute to unsubscribe to</param>
        public void Unsubscribe(string attribute)
        {
            foreach(DataSubscription subscription in subscriptions)
            {
                if(subscription.attribute == attribute)
                {
                    subscriptions.Remove(subscription);
                    active_subscriptions.Remove(attribute);
                    break;
                }
            }
        }

        /// <summary>
        /// Register a method to be invoken when the receiver has received data from a stream though a subscription
        /// </summary>
        /// <param name="cb">The method to invoke taking two parameters, the received data and the attribute name in that order.</param>
        public void OnPayloadReceived(Action<object, string> cb)
        {
            OnPayloadReceived_Callback = cb;
        }

        /// <summary>
        /// Procedure to run when payload is received from stream. Data is cast to the appropriate data type, and the OnPayloadReceived_Callback is invoked on all subscriptions that involves the given attribute
        /// </summary>
        /// <param name="payload">The data being received from the data stream</param>
        /// <param name="attribute">The attribute being received</param>
        /// <param name="data_type">The data type that the received data is represented as</param>
        public void ReceivePayload(object payload, string attribute, string data_type)
        {
            if(subscriptions.Count > 0)
            {
                foreach(DataSubscription subscription in subscriptions)
                {
                    if(subscription.attribute == attribute)
                    {
                        if (OnPayloadReceived_Callback != null)
                        {
                            try
                            {
                                switch (data_type)
                                {
                                    case "float":
                                        payload = Convert.ToSingle(payload);
                                        break;
                                    case "double":
                                        payload = Convert.ToDouble(payload);
                                        break;
                                    case "int":
                                        payload = Convert.ToInt32(payload);
                                        break;
                                    case "long":
                                        payload = Convert.ToInt64(payload);
                                        break;
                                    case "string":
                                        payload = Convert.ToString(payload);
                                        break;
                                    case "JObject":
                                        payload = (JObject)payload;
                                        break;
                                    case "JArray":
                                        payload = (JArray)payload;
                                        break;
                                }
                                OnPayloadReceived_Callback.Invoke(payload, attribute);
                            }
                            catch (InvalidCastException e)
                            {
                                Console.WriteLine("Cannot cast received " + payload.GetType() + " to type " + data_type);
                                Console.WriteLine(e.StackTrace);
                            }
                            catch (InvalidOperationException)
                            {
                                Console.WriteLine("Error when receiving data");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if this receiver is subscribing to a specific attribute
        /// </summary>
        /// <remarks>
        /// This method should be expanded to also specify a stream, in the case where the receiver subscribes on several streams
        /// </remarks>
        /// <param name="key">The attribute to check if the receiver subscribes to</param>
        /// <returns>true if a subscription exists, false otherwise</returns>
        public bool HasSubscription(string key)
        {
            return active_subscriptions.IndexOf(key) > -1;
        }
    }
}
