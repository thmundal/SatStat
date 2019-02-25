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
            ApplyComSettings();
                
            Program.serial.Disconnect();

            Program.serial.Connect();
        }

        private void UIComSettingsApplyBtn_Click(object sender, EventArgs e)
        {
        }

        private void ApplyComSettings()
        {
            if (UIcomSourcesList.SelectedIndex >= 0)
            {
                string portName = portNames[UIcomSourcesList.SelectedIndex];
                Program.settings.selectedComPort = portName;
            }

            if (UIbaudRateInputList.SelectedIndex >= 0)
            {
                int baudRate = (int)UIbaudRateInputList.Items[UIbaudRateInputList.SelectedIndex];
                Program.settings.selectedBaudRate = baudRate;
            }
        }
    }
}