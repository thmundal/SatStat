using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using LiteDB;
using System.IO;
using LiveCharts.Geared;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace SatStat
{
    partial class SatStatMainForm : Form
    {
        private double yMinVal = -20;
        private double yMaxVal = 20;
        private double xMinVal = 0;
        private double xMaxVal = 1000;
        private int maxTimeWindow = 1000;
        private double lastVal = 0;

        private DataReceiver dataReceiver;
        private DataReceiver sensorListReceiver;

        private Hashtable lineSeriesTable = new Hashtable();

        private PlotModel plotModel;
        private LinearAxis xAxis;
        private LinearAxis yAxis;
        private DateTime startTime;

        public SatStatMainForm()
        {
            InitializeComponent();

            dataReceiver = new DataReceiver();
            dataReceiver.OnPayloadReceived((object payload, string attribute) =>
            {
                 ReceivePayload(Convert.ToDouble(payload), attribute);
            });

            sensorListReceiver = new DataReceiver();
            
            sensorListReceiver.OnPayloadReceived((object payload, string attribue) =>
            {
                ReceiveSensorList((JObject)payload);
            });

            sensorListReceiver.Subscribe(Program.serial, "available_data", "JObject");

            startTime = DateTime.Now;

            xAxis = new DateTimeAxis {
                Key ="xAxis",
                Position = AxisPosition.Bottom,
                Title = "Time",
                //Maximum = xMaxVal,
                //Minimum = xMinVal
                Minimum = DateTimeAxis.ToDouble(startTime),
                Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddMinutes(1)),
                MinorIntervalType = DateTimeIntervalType.Minutes
            };

            yAxis = new LinearAxis {
                Key ="yAxis",
                Position = AxisPosition.Left,
                Title = "Value",
                MinimumRange = 10
            };

            plotModel = new PlotModel { Title = "Live data view" };
            
            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);
            oxPlot.Model = plotModel;
        }

        private void CreateDataSeries(PlotModel model, string title)
        {
            if (!lineSeriesTable.ContainsKey(title))
            {
                LineSeries ls = new LineSeries { Title=title, MarkerType = MarkerType.None };

                model.Series.Add(ls);

                oxPlot.Invalidate();
                lineSeriesTable.Add(title, ls);
                PngExporter.Export(plotModel, "test.png", 100, 100);
            }
        }

        public void ReceivePayload(double payload, string attribute)
        {
            if (lineSeriesTable[attribute] != null)
            {
                // Oxyplot test
                LineSeries series = (LineSeries) lineSeriesTable[attribute];

                double timeVal = DateTimeAxis.ToDouble(DateTime.Now);
                series.Points.Add(new DataPoint(timeVal, payload));

                Console.WriteLine(payload);

                double elapsedTime = timeVal - lastVal;
                //series.Points.Add(new DataPoint(elapsedTime, payload));
                
                if (DateTime.Now > startTime.AddSeconds(5))
                {
                    double panStep = -elapsedTime * xAxis.Scale;
                    xAxis.Pan(panStep);
                }

                oxPlot.Invalidate();
                lastVal = timeVal;
            }
            else
            {
                Debug.Log("The line series object is null");
            }
        }

        private Hashtable sensor_information = new Hashtable();

        [STAThread]
        private void ReceiveSensorList(JObject sensor_list)
        {
            ThreadHelperClass.Invoke(this, null, UISensorCheckboxList, (data) =>
            {
                foreach (var elem in (JObject) data["sensor_list"])
                {
                    if(!sensor_information.ContainsKey(elem.Key))
                    {
                        sensor_information.Add(elem.Key, elem.Value.ToString());

                        UISensorCheckboxList.Items.Add(elem.Key);
                    }
                }
            }, new Hashtable {
                { "form", this },
                { "panel", null },
                { "control", UISensorCheckboxList },
                { "sensor_list", sensor_list }
            });
        }

        #region event listeners
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

        private void connectToStreamSimulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Program.streamSimulator = new StreamSimulator();
            
            sensorListReceiver.Subscribe(Program.streamSimulator, "available_data", "JObject");

            Program.streamSimulator.Connect();
        }

        private void UISensorCheckboxList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void UISensorCheckboxList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox sensor_select = (CheckedListBox)sender;

            string attribute = sensor_select.Items[e.Index].ToString();
            if(!dataReceiver.HasSubscription(attribute) && sensor_select.CheckedItems.IndexOf(sensor_select.Items[e.Index]) == -1)
            {
                string type = (string) sensor_information[attribute];

                // This has to only be done once?
                CreateDataSeries(plotModel, attribute);

                if(Program.streamSimulator != null)
                {
                    dataReceiver.Subscribe(Program.streamSimulator, attribute, type);
                }

                if(Program.serial != null && Program.serial.ConnectionStatus == ConnectionStatus.Connected)
                {
                    dataReceiver.Subscribe(Program.serial, attribute, type);
                }

                if(Program.socketHandler != null)
                {
                    dataReceiver.Subscribe(Program.socketHandler, attribute, type);
                }

                Console.WriteLine("Subscribed to " + attribute);
            } else
            {
                dataReceiver.Unsubscribe(attribute);
                Console.WriteLine("Unsubscribed to " + attribute);
            }

        }

        private void startSocketServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.socketHandler = new SocketHandler();

            sensorListReceiver.Subscribe(Program.socketHandler, "available_data", "JObject");
        }

        private void UIautoRotateOnBtn_Click(object sender, EventArgs e)
        {
            Program.serial.Output(Instruction.Create("auto_rotate", "enable", true));
        }

        private void UIAutoRotateOffBtn_Click(object sender, EventArgs e)
        {
            Program.serial.Output(Instruction.Create("auto_rotate", "enable", false));
        }

        private void UIsetMotorSpeedBtn_Click(object sender, EventArgs e)
        {
            string motorSpeedInputText = UImotorSpeedInput.Text;

            if(Int32.TryParse(motorSpeedInputText, out int motorSpeed))
            {
                Program.serial.Output(Instruction.Create("set_motor_speed", "speed", motorSpeed));
            }
            else
            {
                Debug.Log("Invalid input, must provide an integer");
            }
        }

        private void UIrotateAngleBtn_Click(object sender, EventArgs e)
        {
            if(Single.TryParse(UIrotateAngleInput.Text, out float angle))
            {
                Program.serial.Output(Instruction.Create("rotate", "deg", 3.25 * angle));
            }
            else
            {
                Debug.Log("Invalid input, must be floating point number (single)");
            }
        }

        private void UIrotateStepsBtn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(UIrotateStepsInput.Text, out int steps))
            {
                Program.serial.Output(Instruction.Create("rotate", "steps", steps));
            }
            else
            {
                Debug.Log("Invalid input, must be floating point number (single)");
            }
        }

        private void saveToDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory() + @"\Database.db";
            Console.WriteLine("Path is {0}", path);
            using (var db = new LiteDatabase(@path))
            {
                var col = db.GetCollection("ChartData");
                var col2 = db.GetCollection<DB_SensorDataItem>("ChartData");

                foreach (DictionaryEntry line in lineSeriesTable)
                {
                    LineSeries lineSeries = (LineSeries)line.Value;

                    var title = lineSeries.Title;

                    List<object> values = new List<object>();
                    foreach (DataPoint it in lineSeries.Points)
                    {
                        values.Add(it.Y);
                    }

                    DB_SensorDataItem item = new DB_SensorDataItem()
                    {
                        title = title,
                        values = values
                    };
                    col2.Insert(item);
                }
            }
        }

        private void displayDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseViewer databaseViewer = new DatabaseViewer();
            databaseViewer.ShowDialog();
        }
        #endregion

    }
}
