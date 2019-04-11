using System;
using System.Collections;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using LiteDB;
using System.IO;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System.Linq;

namespace SatStat
{
    partial class SatStatMainForm : Form
    {
        private double lastTimeVal = 0;

        private DataReceiver dataReceiver;
        private DataReceiver sensorListReceiver;

        private Hashtable lineSeriesTable = new Hashtable();

        private PlotModel plotModel;
        private LinearAxis xAxis;
        private LinearAxis yAxis;
        private DateTime startTime;

        private DB_ComSettingsItem savedComSettings;

        private ObservableNumericValueCollection observedValues;

        public SatStatMainForm()
        {
            InitializeComponent();

            observedValues = new ObservableNumericValueCollection();

            using (LiteDatabase db = new LiteDatabase(Program.settings.DatabasePath))
            {
                LiteCollection<DB_ComSettingsItem> collection = db.GetCollection<DB_ComSettingsItem>(Program.settings.COMSettingsDB);
                IEnumerable<DB_ComSettingsItem> result = collection.FindAll();


                if(result.Count() > 0)
                {
                    savedComSettings = result.First();
                    ToolStripMenuItem recentConnect = new ToolStripMenuItem
                    {
                        Name = "UICOMRecentConnect",
                        Text = "Connect to " + result.First().PortDescription,
                    };

                    recentConnect.Click += new EventHandler(ConnectToRecent);

                    settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                            recentConnect
                        }
                    );
                }
            }

            dataReceiver = new DataReceiver();
            dataReceiver.Observe = true;
            dataReceiver.OnPayloadReceived((object payload, string attribute) =>
            {
                 ReceivePayload(Convert.ToDouble(payload), attribute);
            });

            dataReceiver.OnObservedvalueChanged((IObservableNumericValue val) =>
            {
                DataGridViewRow row = null;

                foreach (DataGridViewRow r in UIObservedValuesOuputGrid.Rows)
                {
                    if(r.Tag == val.Label)
                    {
                        row = r;
                        break;
                    }
                }

                if(row != null)
                {
                    string status = "Unknown";
                    if (val.Over())
                    {
                        status = "Over";
                    }

                    if(val.Under())
                    {
                        status = "Under";
                    }

                    if(val.Stable())
                    {
                        status = "Stable";
                    }

                    int diff = 0;

                    row.Cells["observedValue"].Value = val.Value;
                    row.Cells["status"].Value = status;
                    row.Cells["difference"].Value = val.Diff().ToString();

                }
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
            lock(plotModel.SyncRoot)
            {
                // Add data in plot
                if (lineSeriesTable[attribute] != null)
                {
                    LineSeries series = (LineSeries) lineSeriesTable[attribute];

                    double timeVal = DateTimeAxis.ToDouble(DateTime.Now);
                    series.Points.Add(new DataPoint(timeVal, payload));

                    double elapsedTime = timeVal - lastTimeVal;
                
                    if (DateTime.Now > startTime.AddSeconds(60) && lastTimeVal != 0)
                    {
                        double panStep = -elapsedTime * xAxis.Scale;
                        xAxis.Pan(panStep);
                    }

                    oxPlot.Invalidate();
                    lastTimeVal = timeVal;
                }
                else
                {
                    Debug.Log("The line series object is null");
                }
            }

            // Add/update data in live view
            ThreadHelperClass.UI_TaskInvoke(this, null, UIliveOutputValuesList, (a) =>
            {
                LiveDataRow row;

                if (!liveDataList.ContainsKey(attribute))
                {
                    row = new LiveDataRow
                    {
                        name = attribute,
                        value = payload.ToString()
                    };

                    row.index = UIliveOutputValuesList.Rows.Add(new string[]{ row.name, row.value });
                    liveDataList.Add(attribute, row);

                    if(dataReceiver.Observe)
                    {
                        int rowIndex = UIParameterControlInput.Rows.Add(new string[] { row.name, row.value, row.value });
                        UIParameterControlInput.Rows[rowIndex].Tag = attribute;

                        rowIndex = UIObservedValuesOuputGrid.Rows.Add(new string[] { row.name, row.value, "Not configured", "Not configured" });
                        UIObservedValuesOuputGrid.Rows[rowIndex].Tag = attribute;
                    }
                }

                row = (LiveDataRow) liveDataList[attribute];
                row.value = payload.ToString();
                UIliveOutputValuesList.Rows[row.index].SetValues(new string[] { row.name, row.value });
            }, null);
        }

        private struct LiveDataRow
        {
            public int index;
            public string value;
            public string name;
        }

        private Hashtable liveDataList = new Hashtable();

        private Hashtable sensor_information = new Hashtable();

        [STAThread]
        private void ReceiveSensorList(JObject sensor_list)
        {
            ThreadHelperClass.UI_Invoke(this, null, UISensorCheckboxList, (data) =>
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

        public void SetCOMConnectionStatus(string status)
        {
            UICOMConnectionStatus.Text = status;
        }

        public void SetNetworkConnectionStatus(string status)
        {
            UINetworkConnectionStatus.Text = status;
        }

        public Control GetConnectionStatusControl()
        {
            return UIStatusStrip;
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
            foreach(int index in UISensorCheckboxList.SelectedIndices)
            {
                bool isChecked = UISensorCheckboxList.GetItemChecked(index);
                UISensorCheckboxList.SetItemChecked(index, !isChecked);
            }
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

                Debug.Log("Subscribed to " + attribute);
            } else
            {
                dataReceiver.Unsubscribe(attribute);
                Debug.Log("Unsubscribed to " + attribute);
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
            using (var db = new LiteDatabase(@path))
            {
                LiteCollection<DB_SensorDataItem> col = db.GetCollection<DB_SensorDataItem>("ChartData");

                foreach (DictionaryEntry line in lineSeriesTable)
                {
                    LineSeries lineSeries = (LineSeries)line.Value;

                    var title = lineSeries.Title;

                    List<object> values = new List<object>();
                    List<object> times = new List<object>();
                    foreach (DataPoint it in lineSeries.Points)
                    {
                        values.Add(it.Y);
                        times.Add(it.X);
                    }

                    DB_SensorDataItem item = new DB_SensorDataItem()
                    {
                        title = title,
                        values = values,
                        times = times
                    };
                    col.Insert(item);
                }
            }
        }

        private void displayDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseViewer databaseViewer = new DatabaseViewer();
            databaseViewer.ShowDialog();
        }

        private void ConnectToRecent(object sender, EventArgs e)
        {
            if(savedComSettings != null)
            {
                Program.serial.Disconnect();

                Program.settings.comSettings = savedComSettings.toComSettings();

                Program.serial.OnHandshakeResponse((settings) =>
                {
                    Debug.Log("Handshake response");
                    Program.serial.ConnectionRequest(savedComSettings.toComSettings());
                });

                Program.serial.DefaultConnect(savedComSettings.PortName);
            }
        }

        private void UIParameterControlInput_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Implement better UI response to error handling so that user understands when an invalid input is entered
            UIParameterControlInput.CurrentCell.ErrorText = "";

            if (UIParameterControlInput.Rows[e.RowIndex].IsNewRow)
            {
                return;
            }

            if (!double.TryParse(e.FormattedValue.ToString(), out double numericValue) && e.ColumnIndex > 0)
            {
                UIParameterControlInput.CurrentCell.ErrorText = "The value must be a number";
            }
            else
            {
                DataGridViewCell current = UIParameterControlInput.CurrentCell;
                string tag = current.OwningRow.Tag.ToString();

                if (dataReceiver.ObservedValues.ContainsLabel(tag))
                {
                    IObservableNumericValue obsValue = dataReceiver.ObservedValues[tag];

                    object castNumericValue = Convert.ChangeType(numericValue, obsValue.type);

                    if (current.OwningColumn.Name == "ParamMin")
                    {
                        obsValue.Min = castNumericValue;
                    } else if(current.OwningColumn.Name == "ParamMax")
                    {
                        obsValue.Max = castNumericValue;
                    }
                }
            }
        }
        #endregion

        private void UIParameterControlInput_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
