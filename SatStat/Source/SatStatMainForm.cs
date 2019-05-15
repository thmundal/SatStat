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
using SatStat.Utils;
using System.Threading.Tasks;
using System.Threading;
using OxyPlot.Annotations;

namespace SatStat
{
    partial class SatStatMainForm : Form
    {
        private double lastTimeVal = 0;

        private DataReceiver dataReceiver;
        private DataReceiver sensorListReceiver;
        private DataReceiver instructionListReceiver;

        public DataReceiver DataReceiver
        {
            get { return dataReceiver; }
        }

        private Hashtable lineSeriesTable = new Hashtable();

        private PlotModel plotModel;
        private LinearAxis xAxis;
        private LinearAxis yAxis;
        private DateTime startTime;

        private DB_ComSettingsItem savedComSettings;
        private JObject instruction_list;
        public JObject InstructionList { get { return instruction_list; } }

        private Hashtable observedDataRows = new Hashtable();
        private Hashtable liveDataList = new Hashtable();
        private Dictionary<string, string> sensor_information = new Dictionary<string, string>();

        private bool hasMovedPlotFromMainControl = false;

        public List<ParameterControlTemplate> parameterControlTemplates;
        public ParameterControlTemplate activeParameterControlTemplate;
        public ParameterControlTemplate autoParamControlTemplate;
        private ObservableNumericValueCollection autoObservableValues;
        public ObservableNumericValueCollection AutoObservableValues { get { return autoObservableValues; } }
        private List<string> observedValueLabels = new List<string>();
        public List<string> ObservedValueLabels { get { return observedValueLabels; } }

        private Dictionary<string, DataStream> activeStreams;
        private Thread discoverThread;
        private ComSettingsForm comSettings;

        private struct LiveDataRow
        {
            public int index;
            public string value;
            public string name;
        }

        private struct ObservedDataRow
        {
            public int input_index;
            public int output_index;
            public string value;
            public string name;
            public string min;
            public string max;
        }

        public SatStatMainForm()
        {
            InitializeComponent();

            instruction_list = new JObject();

            PopulateRecentConnect();

            parameterControlTemplates = ParameterControlTemplate.GetListFromDb();
            autoParamControlTemplate = new ParameterControlTemplate
            {
                Name = "Auto generated parameter control template"
            };

            Program.serial = new SerialHandler();
            Program.streamSimulator = new StreamSimulator();
            Program.socketHandler = new SocketHandler();

            Program.serial.OnConnected(OnStreamConnected);
            Program.streamSimulator.OnConnected(OnStreamConnected);
            Program.socketHandler.OnConnected(OnStreamConnected);

            Program.serial.OnDisconnected(OnStreamDisconnected);
            Program.streamSimulator.OnDisconnected(OnStreamDisconnected);
            Program.socketHandler.OnDisconnected(OnStreamDisconnected);

            activeStreams = new Dictionary<string, DataStream>();
            activeStreams.Add("serial", Program.serial);
            activeStreams.Add("streamSimulator", Program.streamSimulator);
            activeStreams.Add("socketHandler", Program.socketHandler);

            dataReceiver = new DataReceiver { Observe = true };
            dataReceiver.OnPayloadReceived(ReceivePayload);
            dataReceiver.OnObservedvalueChanged(ObservedValueChanged);
            
            sensorListReceiver = new DataReceiver();
            sensorListReceiver.OnPayloadReceived(ReceiveSensorList);
            sensorListReceiver.Subscribe(Program.serial, "available_data", "JObject");

            instructionListReceiver = new DataReceiver();
            instructionListReceiver.OnPayloadReceived(ReceiveInstructionList);
            instructionListReceiver.Subscribe(Program.serial, "available_instructions", "JObject");

            startTime = DateTime.Now;
            xAxis = new DateTimeAxis {
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                Title = "Time",
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

            DiscoverComDevices();
        }
        
        private void DiscoverComDevices()
        {
            discoverThread = new Thread(() => {
                while(Program.serial.ConnectionStatus == ConnectionStatus.Disconnected)
                {
                    if(SerialHandler.Discover())
                    {
                        return;
                    }
                    Thread.Sleep(3000);
                }
            });
            discoverThread.Start();
        }

        private void CreateDataSeries(PlotModel model, string title)
        {
            if (!lineSeriesTable.ContainsKey(title))
            {
                LineSeries ls = new LineSeries { Title = title, MarkerType = MarkerType.None };

                model.Series.Add(ls);

                oxPlot.Invalidate();
                lineSeriesTable.Add(title, ls);
                plotModel.InvalidatePlot(true);
            }
        }

        private LineAnnotation AddPlotAnnotationLine(double yval, string label, OxyColor color)
        {
            LineAnnotation line = new LineAnnotation
            {
                StrokeThickness = 1,
                Color = color,
                Type = LineAnnotationType.Horizontal,
                Text = label,
                TextColor = OxyColors.Black,
                Y = yval,
                X = 0
            };

            if(!plotModel.Annotations.Contains(line))
            {
                plotModel.Annotations.Add(line);
                int index = plotModel.Annotations.IndexOf(line);
                plotModel.InvalidatePlot(true);
            }

            return line;
        }

        private void RemovePlotAnnotationLine(LineAnnotation line)
        {
            if(plotModel.Annotations.Contains(line))
            {
                plotModel.Annotations.Remove(line);
                plotModel.InvalidatePlot(true);
            }
        }

        public void ReceiveInstructionList(object payload, string attribute, string label)
        {
            ThreadHelper.UI_Invoke(this, null, TestConfigTab.UITestConfigInstructionParameterGrid, (Hashtable d) =>
            {
                instruction_list = (JObject)payload;

                foreach (KeyValuePair<string, JToken> instruction in instruction_list)
                {
                    InstructionUIEntry entry = new InstructionUIEntry
                    {
                        label = instruction.Key,
                        parameters = instruction.Value as JObject
                    };
                    TestConfigTab.AddInstructionUIEntry(entry);
                }
            },
            null);
        }

        void PopulateRecentConnect()
        {
            using (LiteDatabase db = new LiteDatabase(Program.settings.DatabasePath))
            {
                LiteCollection<DB_ComSettingsItem> collection = db.GetCollection<DB_ComSettingsItem>(Program.settings.COMSettingsDB);
                IEnumerable<DB_ComSettingsItem> result = collection.FindAll();

                if (result.Count() > 0)
                {
                    savedComSettings = result.First();
                    ToolStripMenuItem recentConnect = new ToolStripMenuItem
                    {
                        Name = "UICOMRecentConnect",
                        Text = "Connect to " + result.First().PortDescription,
                    };

                    recentConnect.Click += new EventHandler(ConnectToRecent);

                    settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                            recentConnect
                        }
                    );
                }
            }
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

        #region Callbacks
        public void OnStreamConnected(DataStream stream)
        {
            ThreadHelper.UI_Invoke(this, null, TestConfigTab.UITestDeviceSelect, (data) =>
            {
                TestConfigTab.AddDeviceToList(stream);

                if(stream == Program.serial)
                {
                    if(comSettings != null)
                    {
                        comSettings.Close();
                    }
                }
            }, null);

        }

        public void OnStreamDisconnected(DataStream stream)
        {
            if (isClosing)
            {
                return;
            }
            ThreadHelper.UI_Invoke(this, null, TestConfigTab.UITestDeviceSelect, (data) =>
            {

                TestConfigTab.RemoveDeviceFromList(stream);

                if (stream == Program.serial)
                {
                    SetCOMConnectionStatus("Serial disconnected");
                }

                if(stream == Program.socketHandler)
                {
                    SetNetworkConnectionStatus("Network disconnected");
                }
            }, null);
        }

        public void ReceivePayload(object _payload, string attribute, string label)
        {
            double payload = Convert.ToDouble(_payload);
            
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
            ThreadHelper.UI_TaskInvoke(this, null, UIliveOutputValuesList, (a) =>
            {
                LiveDataRow row = new LiveDataRow
                {
                    name = attribute,
                    value = payload.ToString()
                };

                if (!liveDataList.ContainsKey(row.name))
                {
                    row.index = UIliveOutputValuesList.Rows.Add(new string[] { row.name, row.value });
                    liveDataList.Add(attribute, row);
                }

                row = (LiveDataRow) liveDataList[attribute];
                row.value = payload.ToString();
                UIliveOutputValuesList.Rows[row.index].SetValues(new string[] { row.name, row.value });
            }, null);
        }

        public void ObservedValueChanged(IObservableNumericValue val)
        {
            DataGridViewRow row = null;

            foreach (DataGridViewRow r in UIObservedValuesOuputGrid.Rows)
            {
                if (r.Tag.Equals(val.Label))
                {
                    row = r;
                    break;
                }
            }

            if (row != null)
            {
                row.Cells["observedValue"].Value = val.Value;
                row.Cells["status"].Value = val.Status().ToString();
                row.Cells["difference"].Value = val.Diff().ToString();
            }
        }

        private void ReceiveSensorList(object _sensor_list, string attribute, string label)
        {
            Debug.Log("Receive sensor list " + attribute);
            JObject sensor_list = (JObject)_sensor_list;
            ThreadHelper.UI_Invoke(this, null, UISensorCheckboxList, (data) =>
            {
                autoObservableValues = new ObservableNumericValueCollection();
                foreach (var elem in (JObject) data["sensor_list"])
                {
                    if(!sensor_information.ContainsKey(elem.Key))
                    {
                        //string sensor_name = label + ": " + elem.Key;
                        string sensor_name = elem.Key;

                        sensor_information.Add(sensor_name, elem.Value.ToString());

                        UISensorCheckboxList.Items.Add(sensor_name);

                        if (DataReceiver.Observe)
                        {
                            AddParameterControlRow(new ObservedDataRow
                                {
                                    name = elem.Key,
                                    value = ""
                                }
                            );

                            autoObservableValues.AddWithType(elem.Key, elem.Value.ToString());
                        }
                    }
                }
                if(autoObservableValues.Count > 0)
                {
                    autoParamControlTemplate.SetCollection(autoObservableValues);
                    DataReceiver.SetObservableNumericValues(autoObservableValues);
                }
            }, new Hashtable {
                { "form", this },
                { "panel", null },
                { "control", UISensorCheckboxList },
                { "sensor_list", sensor_list }
            });
        }
        #endregion

        #region ParameterControl methods
        private void AddParameterControlRow(ObservedDataRow row)
        {
            int rowIndex = UIParameterControlInput.Rows.Add(new string[] { row.name, row.min, row.max });
            UIParameterControlInput.Rows[rowIndex].Tag = row.name;
            row.input_index = rowIndex;

            rowIndex = UIObservedValuesOuputGrid.Rows.Add(new string[] { row.name, row.value, "Not configured", "Not configured" });
            UIObservedValuesOuputGrid.Rows[rowIndex].Tag = row.name;
            row.output_index = rowIndex;

            if (observedDataRows.ContainsKey(row.name))
            {
                observedDataRows[row.name] = row;
            }
            else
            {
                observedDataRows.Add(row.name, row);
            }
        }

        public void LoadParameterControlTemplate(ParameterControlTemplate template)
        {
            activeParameterControlTemplate = template;

            UIParameterControlInput.Rows.Clear();
            UIObservedValuesOuputGrid.Rows.Clear();
            DataReceiver.ObservedValues.Clear();

            foreach (IObservableNumericValue v in template.Collection)
            {
                DataReceiver.ObservedValues.Add(v);
                ObservedDataRow row = new ObservedDataRow
                {
                    name = v.Label,
                    value = v.Value.ToString(),
                    min = v.Min.ToString(),
                    max = v.Max.ToString()
                };
                AddParameterControlRow(row);
            }
        }

        #endregion

        private void SaveTestConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(TestConfigTab.ActiveTestConfiguration != null)
            {
                TestConfigSaveDialog dialog = new TestConfigSaveDialog(TestConfigTab.ActiveTestConfiguration, false);
                dialog.ShowDialog();
            } else
            {
                MessageBox.Show("There is no active test configuration set up");
            }
        }

        private void LoadTestConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestConfigSaveDialog dialog = new TestConfigSaveDialog(null, true);
            dialog.ShowDialog();
        }
    }
}
