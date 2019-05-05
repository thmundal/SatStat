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
        public Action<object, string, string> OnPayloadReceived_Callback;

        /// <summary>
        /// A dictionary that holds all subscriptions on this receiver accessable by the attribute subscribed to
        /// </summary>
        private Dictionary<string, DataSubscription> subscriptions;

        /// <summary>
        /// A flag to tell if this datareceiver should observe its values and act when the value is changed
        /// </summary>
        private bool observe_values = false;

        /// <summary>
        /// Getter and setter for observe_values. If this is set to true, instantiate the observedValues collection if it is not instantiated yet.
        /// </summary>
        public bool Observe
        {
            get
            {
                return observe_values;
            }
            set
            {
                observe_values = value;
            }
        }

        /// <summary>
        /// The collection of observed values registered on this receiver
        /// </summary>
        private ObservableNumericValueCollection observedValues;

        /// <summary>
        /// Public getter for the observed values collection
        /// </summary>
        public ObservableNumericValueCollection ObservedValues { get { return observedValues; } }

        /// <summary>
        /// A callback to be invoked when an observed value has changed its value
        /// </summary>
        private Action<IObservableNumericValue> onObservableValueChanged_cb;

        public DataReceiver()
        {
            subscriptions = new Dictionary<string, DataSubscription>();
            observedValues = new ObservableNumericValueCollection();
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
            stream.AddReceiver(this);

            if(!HasSubscription(attribute))
            {
                stream.Output(Request.Subscription("subscribe", attribute));
                subscriptions.Add(attribute, sub);
            }
        }

        /// <summary>
        /// Unsubscribe from an attribute
        /// </summary>
        /// <remarks>
        /// This method should be expanded to unsubscribe from an attribute on a specific stream, in the case where this receiver subscribes on different streams
        /// </remarks>
        /// <param name="attribute">The attribute to unsubscribe to</param>
        public void Unsubscribe(string attribute, DataStream stream)
        {
            subscriptions.Remove(attribute);
            stream.Output(Request.Subscription("unsubscribe", attribute));
        }

        public void UnsubscribeAll()
        {
            foreach(KeyValuePair<string, DataSubscription> sub in subscriptions)
            {
                subscriptions.Remove(sub.Key);
            }
        }

        /// <summary>
        /// Register a method to be invoken when the receiver has received data from a stream though a subscription
        /// </summary>
        /// <param name="cb">The method to invoke taking two parameters, the received data and the attribute name in that order.</param>
        public void OnPayloadReceived(Action<object, string, string> cb)
        {
            OnPayloadReceived_Callback = cb;
        }

        /// <summary>
        /// Register a callback function to run when an observed value has changed.
        /// </summary>
        /// <param name="cb">The callback function to invoke passing the observable value as parameter</param>
        public void OnObservedvalueChanged(Action<IObservableNumericValue> cb)
        {
            onObservableValueChanged_cb = cb;
        }

        /// <summary>
        /// Set the observed values collection
        /// </summary>
        /// <param name="col">Collection of observable numeric values</param>
        public void SetObservableNumericValues(ObservableNumericValueCollection col)
        {
            observedValues = col;
        }

        /// <summary>
        /// Procedure to run when payload is received from stream. Data is cast to the appropriate data type, and the OnPayloadReceived_Callback is invoked on all subscriptions that involves the given attribute
        /// </summary>
        /// <param name="payload">The data being received from the data stream</param>
        /// <param name="attribute">The attribute being received</param>
        /// <param name="data_type">The data type that the received data is represented as</param>
        public void ReceivePayload(object payload, string attribute, string data_type, string stream_label)
        {
            if(subscriptions.Count > 0 && HasSubscription(attribute) && OnPayloadReceived_Callback != null)
            {
                DataSubscription subscription = subscriptions[attribute];
                payload = Utils.Cast.ToType(payload, data_type);

                OnPayloadReceived_Callback.Invoke(payload, attribute, stream_label);

                if(observe_values)
                {
                    if (!observedValues.ContainsLabel(attribute))
                    {
                        observedValues.AddWithType(attribute, data_type);
                    }

                    if (observedValues.ContainsLabel(attribute))
                    {
                        IObservableNumericValue observedAttribute = observedValues[attribute];
                        observedAttribute = observedValues[attribute];
                        observedAttribute.OnUpdate(onObservableValueChanged_cb);
                        observedAttribute.Value = payload;
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
            return subscriptions.ContainsKey(key);
        }

        /// <summary>
        /// Return a handle to a specific subscription.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataSubscription GetSubscription(string key)
        {
            if(HasSubscription(key))
            {
                return subscriptions[key];
            }

            return null;
        }
    }
}
