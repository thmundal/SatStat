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

        private TestConfiguration activeTestConfiguration = new TestConfiguration();
        private int last_instruction_index = -1;

        private Hashtable observedDataRows = new Hashtable();
        private Hashtable liveDataList = new Hashtable();
        private Dictionary<string, string> sensor_information = new Dictionary<string, string>();

        private bool hasMovedPlotFromMainControl = false;

        public List<ParameterControlTemplate> parameterControlTemplates;
        private ParameterControlTemplate activeParameterControlTemplate;
        private ParameterControlTemplate autoParamControlTemplate;
        private ObservableNumericValueCollection autoObservableValues;
        private List<string> observedValueLabels = new List<string>();

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

            SetUpTestConfigurationPanel();
            DiscoverComDevices();
        }
        
        private void DiscoverComDevices()
        {
            Task.Run(() => {
                while(Program.serial.ConnectionStatus == ConnectionStatus.Disconnected)
                {
                    SerialHandler.Discover();
                }
            });
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

        public void ReceiveInstructionList(object payload, string attribute, string label)
        {
            ThreadHelper.UI_Invoke(this, null, UITestConfigInstructionParameterGrid, (Hashtable d) =>
            {
                instruction_list = (JObject)payload;


                foreach (KeyValuePair<string, JToken> instruction in instruction_list)
                {
                    InstructionUIEntry entry = new InstructionUIEntry
                    {
                        label = instruction.Key,
                        attribute = instruction.Key
                    };

                    if(!UITestConfigInstructionSelect.Items.Contains(instruction.Key))
                    {
                        UITestConfigInstructionSelect.Items.Add(instruction.Key);
                    }
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
            ThreadHelper.UI_Invoke(this, null, UITestDeviceSelect, (data) =>
            {
                if(!UITestDeviceSelect.Items.Contains(stream))
                {
                    int index = UITestDeviceSelect.Items.Add(stream);
                }
            }, null);

        }

        public void OnStreamDisconnected(DataStream stream)
        {
            ThreadHelper.UI_Invoke(this, null, UITestDeviceSelect, (data) =>
            {
                if (UITestDeviceSelect.Items.Contains(stream))
                {
                    UITestDeviceSelect.Items.Remove(stream);
                }

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
                        int checkboxIndex = UITestConfigOutputParamChecklist.Items.Add(sensor_name);

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
                    DataReceiver.SetObservableNumericValues(autoObservableValues);
                }
            }, new Hashtable {
                { "form", this },
                { "panel", null },
                { "control", UISensorCheckboxList },
                { "sensor_list", sensor_list }
            });
        }

        private void OnTestQueueAdvance(InstructionEntry instructionEntry)
        {
            UITestConfigIntructionListGrid.ClearSelection();
            Instruction instruction = instructionEntry.instruction;

            int index = instructionEntry.instruction._ui_index; // activeTestConfiguration.InstructionEntryIndex(instructionEntry);

            if (last_instruction_index > -1)
            {
                UITestConfigIntructionListGrid.Rows[last_instruction_index].Cells[2].Value = "Finished ";
                UITestConfigIntructionListGrid.Rows[last_instruction_index].Cells[3].Value = instruction.feedbackStatus.ToString();
            }

            UITestConfigIntructionListGrid.Rows[index].Cells[2].Value = "Running ";
            UITestConfigIntructionListGrid.Rows[index].Selected = true;
            last_instruction_index = index;
        }

        private void OnTestQueueComplete(InstructionEntry instructionEntry)
        {
            ThreadHelper.UI_Invoke(this, null, UITestConfigIntructionListGrid, (data) => {
                Instruction instruction = instructionEntry.instruction;
                UITestConfigIntructionListGrid.Rows[last_instruction_index].Cells[2].Value = "Finished";
                UITestConfigIntructionListGrid.Rows[last_instruction_index].Cells[3].Value = instruction.feedbackStatus.ToString();

                UITestConfigParameterTemplateSelect.Enabled = true;
            }, null);
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

        #region TestConfiguration methods
        public void SetUpTestConfigurationPanel()
        {
            foreach(ParameterControlTemplate template in parameterControlTemplates)
            {
                UITestConfigParameterTemplateSelect.Items.Add(template.Name);
            }

            using(LiteDatabase db = new LiteDatabase(Program.settings.DatabasePath))
            {
                LiteCollection<TestConfiguration> collection = db.GetCollection<TestConfiguration>(Program.settings.TestConfigDatabase);

                IEnumerable<TestConfiguration> result = collection.FindAll();

                foreach(TestConfiguration config in result)
                {
                    UITestConfigSavedConfigsSelect.Items.Add(config);
                }
            }
        }

        private void loadTestConfiguration(TestConfiguration selectedConfig)
        {
            activeTestConfiguration = selectedConfig;

            foreach(InstructionEntry entry in selectedConfig.instructionEntries)
            {
                AddTestConfigInstructionRow(entry);
            }
        }

        private int AddTestConfigInstructionRow(InstructionEntry instructionEntry)
        {
            Instruction instruction = instructionEntry.instruction;
            int index = UITestConfigIntructionListGrid.Rows.Add(new string[] { instruction.Label, instruction.SerializedParamTable, "pending" });

            UITestConfigIntructionListGrid.Rows[index].Tag = instructionEntry;
            return index;
        }

        private void RunTestConfiguration(TestConfiguration config, DataStream stream)
        {
            if (config.HasInstructionEntries())
            {
                // Disable UI elements
                UITestConfigParameterTemplateSelect.Enabled = false;

                last_instruction_index = -1;
                config.OnQueueAdvance(OnTestQueueAdvance);
                config.OnQueueComplete(OnTestQueueComplete);
                config.Run(stream);
            }
            else
            {
                MessageBox.Show("There are no instructions in the queue");
                return;
            }
        }

        private void AbortTestConfiguration()
        {
            if(activeTestConfiguration != null)
            {
                activeTestConfiguration.Abort();
            }
        }

        private void SelectConfigParameterTemplate()
        {
            if(activeTestConfiguration.IsRunning)
            {
                MessageBox.Show("Cannot modify control parameters when test is running");
                return;
            }

            object selectedItem = UITestConfigParameterTemplateSelect.SelectedItem;

            activeParameterControlTemplate = null;
            activeTestConfiguration.ParameterControlTemplate = null;

            if (selectedItem != null)
            {
                activeParameterControlTemplate = parameterControlTemplates[UITestConfigParameterTemplateSelect.SelectedIndex];
                activeTestConfiguration.ParameterControlTemplate = activeParameterControlTemplate;
            }
        }
        #endregion

        #region event handler callbacks
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
            sensorListReceiver.Subscribe(Program.streamSimulator, "available_data", "JObject");
            instructionListReceiver.Subscribe(Program.streamSimulator, "available_instructions", "JObject");

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

                    Program.serial.Output(Request.Subscription("subscribe", attribute));
                }

                if(Program.socketHandler != null)
                {
                    dataReceiver.Subscribe(Program.socketHandler, attribute, type);
                }

                Debug.Log("Subscribed to " + attribute);
            } else
            {
                if(Program.serial != null)
                {
                    Program.serial.Output(Request.Subscription("unsubscribe", attribute));
                }

                dataReceiver.Unsubscribe(attribute);
                Debug.Log("Unsubscribed to " + attribute);
            }

        }

        private void startSocketServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sensorListReceiver.Subscribe(Program.socketHandler, "available_data", "JObject");
            instructionListReceiver.Subscribe(Program.socketHandler, "available_instructions", "JObject");
            Program.socketHandler.Connect();
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
                Program.serial.Output(Instruction.Create("rotate_degrees", "degrees", 3.25 * angle));
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
                Program.serial.Output(Instruction.Create("rotate_steps", "steps", steps));
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

                Program.serial.Connect(new ConnectionParameters { com_port = savedComSettings.PortName });
            }
        }

        private void UIParameterControlInput_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Implement better UI response to error handling so that user understands when an invalid input is entered
            UIParameterControlInput.CurrentCell.ErrorText = "";

            if(activeParameterControlTemplate == null)
            {
                activeParameterControlTemplate = new ParameterControlTemplate();
            }

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

        private void saveParameterControlTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParameterControlTemplateDialog parameterControlTemplateDialog = new ParameterControlTemplateDialog(false);
            parameterControlTemplateDialog.ShowDialog();
        }

        private void loadTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParameterControlTemplateDialog parameterControlTemplateDialog = new ParameterControlTemplateDialog(true);
            parameterControlTemplateDialog.ShowDialog();
        }

        private void UITestConfigInstructionSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            object sel = UITestConfigInstructionSelect.SelectedItem;
            UITestConfigInstructionParameterGrid.Rows.Clear();

            foreach (KeyValuePair<string, JToken> instruction in instruction_list)
            {
                if(sel.ToString().Equals(instruction.Key))
                {
                    foreach (KeyValuePair<string, JToken> param in (JObject)instruction.Value)
                    {
                        UITestConfigInstructionParameterGrid.Rows.Add(new string[] { param.Key, "" });
                    }
                }
            }
        }

        private void UITestConfigAddInstructionBtn_Click(object sender, EventArgs e)
        {
            if(UITestConfigInstructionSelect.SelectedItem == null || activeTestConfiguration.IsRunning)
            {
                return;
            }

            string instruction_name = UITestConfigInstructionSelect.SelectedItem.ToString();
            JObject paramTable = new JObject();

            // Get parameters from the parameters data grid input
            for(int row=0; row<UITestConfigInstructionParameterGrid.Rows.Count; row++)
            {
                string key = null;
                object value = null;

                for(int col=0; col<UITestConfigInstructionParameterGrid.ColumnCount; col++)
                {
                    var cell = UITestConfigInstructionParameterGrid.Rows[row].Cells[col].Value;
                    if(col % 2 == 0)
                    {
                        // Key
                        key = cell.ToString();
                    } else
                    {
                        // Value
                        string instruction_type = instruction_list[instruction_name][key].Value<string>();
                        value = Cast.ToType(cell, instruction_type); // This casts, BUT only result we need here is that value is 0 if string is empty....
                    }
                }

                if(key != null && value != null)
                {
                    paramTable[key] = JToken.FromObject(value);
                }
            }

            if(paramTable.Count > 0)
            {
                List<string> obsLabels = new List<string>(observedValueLabels);

                Instruction thatInstruction = new Instruction(instruction_name, paramTable);

                InstructionEntry entry = new InstructionEntry
                {
                    instruction = thatInstruction,
                    observedValueLabels = obsLabels
                };
                int index = AddTestConfigInstructionRow(entry);
                entry.setInstructionIndex(index);
                activeTestConfiguration.AddInstructionEntry(entry);

                observedValueLabels.Clear();
                UITestConfigOutputParamChecklist.ClearSelected();
                foreach(int i in UITestConfigOutputParamChecklist.CheckedIndices)
                {
                    UITestConfigOutputParamChecklist.SetItemCheckState(i, CheckState.Unchecked);
                }
            }
        }

        private void UITestConfigRunTestBtn_Click(object sender, EventArgs e)
        {
            if(activeTestConfiguration != null)
            {
                if(UITestDeviceSelect.SelectedIndex > -1)
                {
                    DataStream stream = (DataStream)UITestDeviceSelect.SelectedItem;
                    RunTestConfiguration(activeTestConfiguration, stream);
                } else
                {
                    MessageBox.Show("You have to select a device to test on");
                }
            } else
            {
                MessageBox.Show("There is no active test configuration");
            }
        }

        private void UITestConfigInstructionMoveDownBtn_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = UITestConfigIntructionListGrid.SelectedRows;

            if (selectedRows.Count == 0 || activeTestConfiguration.IsRunning)
            {
                return;
            }

            int oldIndex = UITestConfigIntructionListGrid.Rows.IndexOf(selectedRows[0]);
            int newIndex = oldIndex + 1;

            if (newIndex + selectedRows.Count <= UITestConfigIntructionListGrid.Rows.Count)
            {
                UITestConfigIntructionListGrid.ClearSelection();
                foreach (DataGridViewRow row in selectedRows)
                {
                    UITestConfigIntructionListGrid.Rows.RemoveAt(row.Index);
                }

                for (int i = 0; i < selectedRows.Count; i++)
                {
                    UITestConfigIntructionListGrid.Rows.Insert(newIndex, selectedRows[i]);
                    UITestConfigIntructionListGrid.Rows[newIndex].Selected = true;
                    newIndex++;
                }
            }
        }

        private void UITestConfigInstructionMoveUpBtn_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = UITestConfigIntructionListGrid.SelectedRows;

            if (selectedRows.Count == 0 || activeTestConfiguration.IsRunning)
            {
                return;
            }

            int oldIndex = UITestConfigIntructionListGrid.Rows.IndexOf(selectedRows[0]);
            int newIndex = oldIndex - 1;

            if (newIndex >= 0)
            {
                UITestConfigIntructionListGrid.ClearSelection();
                foreach (DataGridViewRow row in selectedRows)
                {
                    UITestConfigIntructionListGrid.Rows.RemoveAt(row.Index);
                }

                for (int i = 0; i < selectedRows.Count; i++)
                {
                    UITestConfigIntructionListGrid.Rows.Insert(newIndex, selectedRows[i]);
                    UITestConfigIntructionListGrid.Rows[newIndex].Selected = true;
                    newIndex++;
                }
            }
        }

        private void UITestConfigInstructionDeleteBtn_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = UITestConfigIntructionListGrid.SelectedRows;

            int[] selectedIndices = new int[selectedRows.Count];

            if(selectedRows.Count == 0 || activeTestConfiguration.IsRunning)
            {
                return;
            }

            int i = 0;
            foreach(DataGridViewRow row in selectedRows)
            {
                selectedIndices[i++] = row.Index;
                InstructionEntry instr = (InstructionEntry)row.Tag;
                int instructionIndex = activeTestConfiguration.InstructionEntryIndex(instr);
                UITestConfigIntructionListGrid.Rows.RemoveAt(instructionIndex);
                activeTestConfiguration.RemoveInstructionEntry(instr);
            }

            foreach(int index in selectedIndices)
            {
                if(index < UITestConfigIntructionListGrid.Rows.Count)
                {
                    UITestConfigIntructionListGrid.Rows[index].Selected = true;
                }
            }
        }
        
        private void UITestConfigParameterTemplateSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectConfigParameterTemplate();
        }

        private void UITestConfigName_TextChanged(object sender, EventArgs e)
        {
            if(activeTestConfiguration != null)
            {
                activeTestConfiguration.Name = UITestConfigName.Text;
            }
        }

        private void UITestConfigSaveButton_Click(object sender, EventArgs e)
        {
            if (activeTestConfiguration != null)
            {
                activeTestConfiguration.Save((config) => {
                    UITestConfigSavedConfigsSelect.Items.Add(config);
                });
            }
        }

        private void UITestConfigUseCurrentParamConfigCheck_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if(activeTestConfiguration.IsRunning)
            {
                return;
            }

            if(cb.Checked)
            {
                activeParameterControlTemplate = autoParamControlTemplate;
                activeParameterControlTemplate.SetCollection(dataReceiver.ObservedValues);
                
                activeTestConfiguration.ParameterControlTemplate = activeParameterControlTemplate;
            } else
            {
                SelectConfigParameterTemplate();
            }
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isPlotTab = false;

            if (MainTabControl.SelectedTab.Tag != null && MainTabControl.SelectedTab.Tag.Equals("plotViewTab"))
            {
                isPlotTab = true;
            }

            if (!isPlotTab && !hasMovedPlotFromMainControl)
            {
                oxPlot.Parent.Controls.Remove(oxPlot);
                UIliveOutputValuesList.Parent.Controls.Remove(UIliveOutputValuesList);
                diagnosticLiveOutputValues.Controls.Add(oxPlot);
                hasMovedPlotFromMainControl = true;
            }

            else if (isPlotTab && hasMovedPlotFromMainControl)
            {
                diagnosticLiveOutputValues.Controls.Remove(oxPlot);
                diagnosticLiveOutputValues.Controls.Add(UIliveOutputValuesList);
                UIPlotViewTab.Controls.Add(oxPlot);
                hasMovedPlotFromMainControl = false;
            }
        }

        private void UITestConfigOutputParamChecklist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(UITestConfigOutputParamChecklist.SelectedItem != null)
            {
                string label = UITestConfigOutputParamChecklist.SelectedItem.ToString();
                bool is_checked = UITestConfigOutputParamChecklist.CheckedItems.IndexOf(label) > -1;

                if(is_checked)
                {
                    if(!observedValueLabels.Contains(label))
                    {
                        observedValueLabels.Add(label);
                    }
                } else
                {
                    if (observedValueLabels.Contains(label))
                    {
                        observedValueLabels.Remove(label);
                    }
                }
            }
        }

        private void disconnectFromSerialDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.serial.Disconnect();
        }

        private void disconnectFromStreamSimulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.streamSimulator.Disconnect();
        }

        private void stopSocketServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.socketHandler.Disconnect();
        }

        private void UIAbortTestBtn_Click(object sender, EventArgs e)
        {
            AbortTestConfiguration();
        }

        private void UITestConfigSavedConfigsSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            TestConfiguration selectedConfig = (TestConfiguration) UITestConfigSavedConfigsSelect.SelectedItem;

            loadTestConfiguration(selectedConfig);
        }

        #endregion
    }
}
