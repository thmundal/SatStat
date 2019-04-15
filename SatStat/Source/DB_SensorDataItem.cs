using LiteDB;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    /// <summary>
    /// A class to encapsulate plot data so that it can be saved to a LiteDatabase. Use this class when saving or loading plot data to/from database.
    /// </summary>
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
