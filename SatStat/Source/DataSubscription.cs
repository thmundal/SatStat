using Newtonsoft.Json.Linq;
using System;

namespace SatStat
{
    /// <summary>
    /// A DataSubscription contains information about what attribute and datatype a receiver is listening for, and provides methods for delivering the data to its receiver, and setting up a subscription internally
    /// </summary>
    public class DataSubscription
    {
        public string attribute { get; set; }
        public string data_type { get; set; }
        public DataReceiver receiver { get; set; }

        /// <summary>
        /// A DataSubscription needs a receiver to receive data trough this subscription, to define the attribute that contains the relevant data that the receiver wants to receive, and to define what datatype this data is represented as so it can be correctly cast on delivery
        /// </summary>
        /// <param name="r"></param>
        /// <param name="attr"></param>
        /// <param name="type"></param>
        public DataSubscription(DataReceiver r, string attr, string type)
        {
            receiver = r;
            attribute = attr;
            data_type = type;
        }
        
        /// <summary>
        /// Defines procedure to follow when delivering data trough this subscription
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public object receive(object data)
        {   
            receiver.ReceivePayload(data, attribute, data_type);
            return data;
        }

        /// <summary>
        /// Shortcut function for creating a subscription based on attribute and type
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="attr"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataSubscription Create(DataReceiver receiver, string attr, string type)
        {
            return new DataSubscription(receiver, attr, type);
        }
    }
}