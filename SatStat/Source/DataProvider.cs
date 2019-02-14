using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    public class DataProvider
    {
        private DataStream stream;

        public DataProvider(DataStream target)
        {
            stream = target;
        }

        public void Deliver(object data)
        {
            stream.Output(data);
        }
    }
}
