using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    /// <summary>
    /// A provider of data from the SW layer to connected HW layer or stream. To send data out from the software to external connected device or simulation
    /// </summary>
    public class DataProvider
    {
        /// <summary>
        /// Specifies the stream on which to send data
        /// </summary>
        private DataStream stream;

        /// <summary>
        /// Constructor provides the target of the data deliverance
        /// </summary>
        /// <param name="target">The stream that should receive the output data</param>
        public DataProvider(DataStream target)
        {
            stream = target;
        }

        /// <summary>
        /// Deliver data to the stream
        /// </summary>
        /// <param name="data">The data that should be sent as output to the stream</param>
        public void Deliver(object data)
        {
            stream.Output(data);
        }
    }
}
