using Newtonsoft.Json.Linq;
using System;

namespace SatStat
{
    /// <summary>
    /// A DataSubscription contains information about what attribute and datatype a receiver is listening for, and provides methods for delivering the data to its receiver, and setting up a subscription internally
    /// </summary>
    public class DataSubscription
    {
        /// <summary>
        /// Specifies the attribute that was subscribed to trough this subscription
        /// </summary>
        public string attribute;

        /// <summary>
        /// Specifies the datatype of the data that is delivered on this subscription
        /// </summary>
        public string data_type;

        /// <summary>
        /// Specifies the receiver of the data on this subscription
        /// </summary>
        public DataReceiver receiver;

        /// <summary>
        /// A DataSubscription needs a receiver to receive data trough this subscription, to define the attribute that contains the relevant data that the receiver wants to receive, and to define what datatype this data is represented as so it can be correctly cast on delivery
        /// </summary>
        /// <param name="r">The object that should receive the data</param>
        /// <param name="attr">The attribute to subscribe to</param>
        /// <param name="type">The datatype of the data delivered on the subscription</param>
        public DataSubscription(DataReceiver r, string attr, string type)
        {
            receiver = r;
            attribute = attr;
            data_type = type;
        }
        
        /// <summary>
        /// Defines procedure to follow when delivering data trough this subscription
        /// </summary>
        /// <param name="data">Returns the data received</param>
        /// <returns></returns>
        public object receive(object data)
        {   
            receiver.ReceivePayload(data, attribute, data_type);
            return data;
        }
    }
}