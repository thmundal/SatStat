using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    public class DataReceiver : IDataReceiver
    {
        public Action<object> OnPayloadReceived_Callback;
        private string dataType;

        public DataReceiver(string dataType)
        {
            this.dataType = dataType;
        }

        public void OnPayloadReceived(Action<object> cb)
        {
            OnPayloadReceived_Callback = cb;
        }

        public void ReceivePayload(object payload)
        {
            if (OnPayloadReceived_Callback != null)
            {
                switch (dataType)
                {
                    case "float":
                        payload = (float)payload;
                        break;
                    case "double":
                        payload = (double)payload;
                        break;
                    case "int":
                        payload = (int)payload;
                        break;
                    case "long":
                        payload = (long)payload;
                        break;
                    case "string":
                        payload = (string)payload;
                        break;
                    case "JObject":
                        payload = (JObject)payload;
                        break;
                    case "JArray":
                        payload = (JArray)payload;
                        break;
                }
                OnPayloadReceived_Callback.Invoke(payload);
            }
        }
    }

    public interface IDataReceiver
    {
        
    }
}
