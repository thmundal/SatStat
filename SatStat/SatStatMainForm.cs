using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using System.Threading;

namespace SatStat
{
    public partial class SatStatMainForm : Form, DataReceiver<double[]>
    {
        private SeriesCollection seriesCollection1;
        private LineSeries lineSeries1;
        private Axis timeAxis;
        private Axis valueAxis;
        private double yMinVal = -10;
        private double yMaxVal = 10;
        private double xMinVal = 0;
        private double xMaxVal = 10;
        private int maxTimeWindow = 10;
        private double lastVal = 0;

        public SatStatMainForm()
        {
            InitializeComponent();

            seriesCollection1 = new SeriesCollection();

            lineSeries1 = new LineSeries();
            lineSeries1.Title = "Series 1";
            lineSeries1.Values = new ChartValues<double>();
            lineSeries1.Fill = System.Windows.Media.Brushes.Transparent;

            seriesCollection1.Add(lineSeries1);
            //SimulateDataStream();
            
            cartesianChart1.Series = seriesCollection1;
            cartesianChart1.ScrollMode = ScrollMode.X;
            cartesianChart1.DisableAnimations = false;

            timeAxis = new Axis
            {
                IsMerged = true,
                Position = AxisPosition.LeftBottom,
                MaxValue = xMaxVal,
                MinValue = xMinVal
            };

            valueAxis = new Axis
            {
                IsMerged = false,
                Position = AxisPosition.LeftBottom,
                MinValue = yMinVal,
                MaxValue = yMaxVal
            };

            solidGauge1.From = 0;
            solidGauge1.To = 100;
            solidGauge1.Value = 0;

            cartesianChart1.AxisX.Add(timeAxis);
            cartesianChart1.AxisY.Add(valueAxis);
        }

        private void cartesianChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void ChartOnUpdaterTick(object sender)
        {
            valueAxis.MinValue = yMinVal;
            valueAxis.MaxValue = yMaxVal;
            timeAxis.MaxValue = xMaxVal;
            timeAxis.MinValue = xMinVal;

            solidGauge1.Value = (int) lastVal;
        }

        [STAThread]
        private void AddData(LineSeries data)
        {
            Task t1 = Task.Run(() =>
            {
                seriesCollection1.Add(data);
            });
        }

        [STAThread]
        private void AddDataPoint(double dp)
        {
            Task.Run(() =>
            {
                if(dp > yMaxVal)
                {
                    yMaxVal = dp + 50;
                } else if(dp < yMinVal)
                {
                    yMinVal = dp - 50;
                }
                
                lineSeries1.Values.Add(dp);
            });
        }

        [STAThread]
        private void RemoveFirstDataPoint()
        {
            Task.Run(() =>
            {
                lineSeries1.Values.RemoveAt(0);
            });
        }

        [STAThread]
        private void SimulateDataStream()
        {
            Random rand = new Random();
            Task dataStream = Task.Run(async () =>
            {
                int limit = 10;
                int i = 0;
                while(i < 100)
                {
                    AddDataPoint(rand.NextDouble() * 100);
                    
                    if(i >= limit)
                    {
                        xMinVal = i - limit;
                        xMaxVal = i - 1;
                    }

                    await Task.Delay(500);
                    i++;
                }
            });
        }

        private void inputDataSource1_TextChanged(object sender, EventArgs e)
        {
            string input = inputDataSource1.Text;
            char[] c_inputArr = input.ToCharArray();
            string output = "";

            foreach(char c in c_inputArr) {
                int intval = (int)c;
                
                if((c >= 48 && c <= 57) || (c == 45 || c == 46))
                {
                    output += c;
                } else
                {
                    Console.WriteLine(((int)c).ToString() + ":" + c + " is not a digit");
                }
            }
            
            inputDataSource1.Text = output;
            inputDataSource1.SelectionStart = output.Length;
            inputDataSource1.SelectionLength = 0;
            
        }

        private void inputDataBtn1_Click(object sender, EventArgs e)
        {
            string input = inputDataSource1.Text;

            if(double.TryParse(input, out double n))
            {
                AddDataPoint(n);
                inputDataSource1.Text = "";
            }
        }
        
        private void inputDataSource1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Return)
            {
                inputDataBtn1_Click(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(lineSeries1.Values.Count > 0)
            {
                RemoveFirstDataPoint();
            }
        }

        public void ReceivePayload(double[] payload)
        {
            AddDataPoint(payload[1]);

            if (payload[0] >= maxTimeWindow)
            {
                xMinVal = payload[0] - maxTimeWindow;
                xMaxVal = payload[0] + 1;
            }
            lastVal = payload[1];
            Console.WriteLine("Received playload: " + payload[1]);
        }

        private void SatStatMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.StopReader();
        }

        private void cOMSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComSettingsForm comSettings = new ComSettingsForm();
            comSettings.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.WriteSerialData("auto_start");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.WriteSerialData("auto_stop");
        }
    }
}
