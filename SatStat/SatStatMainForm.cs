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
    partial class SatStatMainForm : Form
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

        private DataReceiver dataReceiver;

        public SatStatMainForm()
        {
            InitializeComponent();

            seriesCollection1 = new SeriesCollection();

            dataReceiver = new DataReceiver("double");
            IDataSubscription sub = DataSubscription<object>.CreateWithType(dataReceiver, "temperature", "double");
            dataReceiver.OnPayloadReceived((object payload) =>
            {
                ReceivePayload((double) payload);
            });
            Program.serial.AddSubscriber(sub);

            lineSeries1 = new LineSeries();
            lineSeries1.Title = "Series 1";
            lineSeries1.Values = new ChartValues<double>();
            lineSeries1.Fill = System.Windows.Media.Brushes.Transparent;

            seriesCollection1.Add(lineSeries1);
            
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

        private void button1_Click(object sender, EventArgs e)
        {
            if(lineSeries1.Values.Count > 0)
            {
                RemoveFirstDataPoint();
            }
        }

        private int counter = 0;
        public void ReceivePayload(double payload)
        {
            AddDataPoint(payload);
            if (counter >= maxTimeWindow)
            {
                xMinVal = counter - maxTimeWindow;
                xMaxVal = counter + 1;
            }
            lastVal = payload;
            Console.WriteLine("Received playload: " + payload);
            counter++;
        }

        private void SatStatMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.serial.Disconnect();
        }

        private void cOMSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComSettingsForm comSettings = new ComSettingsForm();
            comSettings.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.serial.WriteData("auto_start");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.serial.WriteData("auto_stop");
        }
    }
}
