using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    /// <summary>
    /// A class to encapsulate a database item for saved com settings required for LiteDB. Use this class when saving or retrieving COM settings to/from database
    /// </summary>
    public class DB_ComSettingsItem
    {
        /// <summary>
        /// UID of the object required for saving to a LiteDatabase
        /// </summary>
        public ObjectId Id { get; set; }
        public int baud_rate { get; set; }
        public string Config { get; set; }
        public string NewLine { get; set; }
        public int Parity { get; set; }
        public int DataBits { get; set; }
        public int StopBits { get; set; }
        public string PortDescription { get; set; }
        public string PortName { get; set; }

        public COMSettings toComSettings()
        {
            return new COMSettings
            {
                baud_rate = baud_rate,
                config = Config,
                newline = "\r\n"    // Fix this hotfix hack
            };
        }
    }
}
