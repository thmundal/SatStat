using System;
using System.Collections;
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
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using LiteDB;
using System.IO;

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
        private DataReceiver sensorListReceiver;

        private Hashtable lineSeriesTable = new Hashtable();
        
        public SatStatMainForm()
        {
            InitializeComponent();

            seriesCollection1 = new SeriesCollection();

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
        private void AddDataPoint(double dp, LineSeries series)
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
                
                series.Values.Add(dp);
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
        
        private void CreateDataSeries(SeriesCollection collection, string title)
        {
            if(!lineSeriesTable.ContainsKey(title))
            {
                LineSeries ls = new LineSeries();
                ls.Title = title;
                ls.Values = new ChartValues<double>();
                ls.Fill = System.Windows.Media.Brushes.Transparent;

                collection.Add(ls);
                lineSeriesTable.Add(title, ls);
            }
        }

        private int counter = 0;
        public void ReceivePayload(double payload, string attribute)
        {
            if(lineSeriesTable[attribute] != null)
            {
                AddDataPoint(payload, (LineSeries) lineSeriesTable[attribute]);
                if (counter >= maxTimeWindow)
                {
                    xMinVal = counter - maxTimeWindow;
                    xMaxVal = counter + 1;
                }
                lastVal = payload;
                Console.WriteLine("Received playload: " + payload);
                counter++;
            } else
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
            
            sensorListReceiver.Subscribe(Program.streamSimulator, "available_sensors", "JObject");

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

                if(Program.streamSimulator != null)
                {
                    CreateDataSeries(seriesCollection1, attribute);

                    if(!dataReceiver.HasSubscription(attribute))
                    {
                        dataReceiver.Subscribe(Program.streamSimulator, attribute, type);
                    }
                }

                if(Program.serial != null && Program.serial.ConnectionStatus == ConnectionStatus.Connected)
                {
                    CreateDataSeries(seriesCollection1, attribute);

                    if (!dataReceiver.HasSubscription(attribute))
                    {
                        dataReceiver.Subscribe(Program.serial, attribute, type);
                    }
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
            SocketHandler server = new SocketHandler();
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
                Debug.Log("Invalid input, bust be floating point number (single)");
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
                Debug.Log("Invalid input, bust be floating point number (single)");
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
                    foreach(object it in lineSeries.Values)
                    {
                        values.Add(it);
                    }

                    DB_SensorDataItem item = new DB_SensorDataItem() {
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
