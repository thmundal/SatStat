using LiteDB;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    class DB_SensorDataItem
    {
        public ObjectId Id { get; set; }
        public string title { get; set; }
        public List<object> values { get; set; }
        public List<object> times { get; set; }

        public DateTime time()  {
            return Id.CreationTime;
        }
    }
}
