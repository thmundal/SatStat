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
            if (UIcomSourcesList.SelectedIndex >= 0)
            {
                string portName = portNames[UIcomSourcesList.SelectedIndex];
                Program.settings.portName = portName;
            }

            if (UIbaudRateInputList.SelectedIndex >= 0)
            {
                int baudRate = (int)UIbaudRateInputList.Items[UIbaudRateInputList.SelectedIndex];
                Program.settings.baud_rate = baudRate;
                Program.settings.comSettings.baud_rate = baudRate;
            }

            // Fix this
            Program.settings.comSettings.config = "8N1";
            Program.settings.comSettings.newline = "\r\n";
        }
    }
}