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

        public int test = 1;

        public DateTime time()  {
            return Id.CreationTime;
        }

        //public DB_SensorDataItem(string title, IChartValues values)
        //{
        //    Id = ObjectId.NewObjectId();
        //    this.title = title;
        //    this.values = values;
        //}
    }
}
