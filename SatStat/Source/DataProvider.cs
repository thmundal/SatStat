using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    class DataProvider<T>
    {
        private T payload;
        private DataReceiver<T> receiver;

        public DataProvider(DataReceiver<T> res)
        {
            receiver = res;
        }

        public void DeliverPayload()
        {
            receiver.ReceivePayload(payload);
        }

        public void SetPayload(T pl)
        {
            payload = pl;
        }
    }
}
