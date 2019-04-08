using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Collections;
using Newtonsoft.Json.Linq;
using LiteDB;
using System.IO;

namespace SatStat
{
    public partial class ComSettingsForm : Form
    {
        private List<string> portNames = new List<string>();
        public ComSettingsForm()
        {
            InitializeComponent();

            Hashtable comPorts = SerialHandler.GetPortListInformation();

            foreach (DictionaryEntry n in comPorts)
            {
                portNames.Add(n.Key.ToString());
                UIcomSourcesList.Items.Add(n.Value.ToString());
            }
        }
        
        private void UIComSettingsConnectBtn_Click(object sender, EventArgs e)
        {
            // If serial is disconnected, perform handshake request trough default settings
            if(Program.serial.ConnectionStatus == ConnectionStatus.Disconnected)
            {
                ApplyComSettings();

                if (Program.settings.portName != null && Program.settings.portName != String.Empty)
                {
                    Program.serial.OnHandshakeResponse((available_settings) =>
                    {
                        ThreadHelperClass.SetBaudrates(this, UIbaudRateSelectionList, UIbaudRateInputList, available_settings);
                    });
                    UIComSettingsApplyBtn.Show();
                    UIComSettingsConnectBtn.Text = "Connect";
                    Program.serial.DefaultConnect(Program.settings.portName);
                }
            }
            // If we are in the middle of a handshake, then send the connection request with selected settings
            else if(Program.serial.ConnectionStatus == ConnectionStatus.Handshake)
            {
                ApplyComSettings();
                Program.serial.ConnectionRequest(Program.settings.comSettings);
            }
        }

        private void UIComSettingsApplyBtn_Click(object sender, EventArgs e)
        {
        }

        private void ApplyComSettings()
        {
            string portName = null;
            int baudRate = (int) SerialHandler.default_settings.BaudRate;
            COMSettings comSettings = new COMSettings();

            if (UIcomSourcesList.SelectedIndex >= 0)
            {
                portName = portNames[UIcomSourcesList.SelectedIndex];
                Program.settings.portName = portName;
                Program.settings.portDescription = UIcomSourcesList.Items[UIcomSourcesList.SelectedIndex].ToString();
            }

            if (UIbaudRateInputList.SelectedIndex >= 0)
            {
                baudRate = (int)UIbaudRateInputList.Items[UIbaudRateInputList.SelectedIndex];
                comSettings.baud_rate = baudRate;
            }

            // Fix this
            comSettings.config = "8N1";
            comSettings.newline = "\r\n";

            Program.settings.comSettings = comSettings;

            if(portName != null)
            {
                // Save last com settings to db
                string path = Program.settings.DatabasePath;
                using (LiteDatabase db = new LiteDatabase(@path))
                {
                    LiteCollection<DB_ComSettingsItem> collection = db.GetCollection<DB_ComSettingsItem>(Program.settings.COMSettingsDB);

                    IEnumerable<DB_ComSettingsItem> results = collection.FindAll();

                    DB_ComSettingsItem store = new DB_ComSettingsItem
                    {
                        baud_rate = baudRate,
                        Parity = (int)SerialHandler.default_settings.Parity,
                        DataBits = (int)SerialHandler.default_settings.DataBits,
                        StopBits = (int)SerialHandler.default_settings.StopBits,
                        NewLine = SerialHandler.default_settings.NewLine,
                        Config = "8N1",
                        PortDescription = Program.settings.portDescription,
                        PortName = Program.settings.portName
                    };
                    
                    if(results.Count() > 0)
                    {
                        // Update existing item
                        DB_ComSettingsItem existing = results.First();
                        ObjectId id = existing.Id;
                        store.Id = id;
                        collection.Update(store);
                    } else
                    {
                        // Add item
                        collection.Insert(store);
                    }
                }
            }
        }
    }
}