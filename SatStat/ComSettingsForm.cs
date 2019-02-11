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
                comSourcesListUI.Items.Add(n.Value.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comSourcesListUI.SelectedIndex >= 0)
            {
                string a = portNames[comSourcesListUI.SelectedIndex];
                Program.settings.selectedComPort = a;

                Program.serial.Stop();
                Program.serial.SetComPort(a);

                Program.StartReader();
                Console.WriteLine(a);
            }
        }
    }
}
