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


namespace SatStat
{
    public partial class ComSettingsForm : Form
    {
        public ComSettingsForm()
        {
            InitializeComponent();

            string[] comPorts = SerialPort.GetPortNames();
            foreach(string n in comPorts)
            {
                comSourcesListUI.Items.Add(n);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comSourcesListUI.SelectedIndex >= 0)
            {
                string a = comSourcesListUI.Items[comSourcesListUI.SelectedIndex].ToString();
                Program.settings.selectedComPort = a;

                Program.StartReader();
                Console.WriteLine(a);
            }
        }
    }
}
