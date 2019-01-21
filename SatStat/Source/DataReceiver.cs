using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    interface DataReceiver<T>
    {
        void ReceivePayload(T payload);
    }
}
