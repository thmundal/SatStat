using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatStat
{
    public class DB_ComSettingsItem
    {
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
