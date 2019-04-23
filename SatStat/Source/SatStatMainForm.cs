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

        private TestConfiguration activeTestConfiguration;

        private Hashtable observedDataRows = new Hashtable();
        private Hashtable liveDataList = new Hashtable();
        private Hashtable sensor_information = new Hashtable();

        public SatStatMainForm()
        {
            InitializeComponent();

            instruction_list = new JObject();

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
            
            sensorListReceiver.OnPayloadReceived((object payload, string attribute) =>
            {
                ReceiveSensorList((JObject)payload);
            });

            sensorListReceiver.Subscribe(Program.serial, "available_data", "JObject");

            instructionListReceiver = new DataReceiver();

            instructionListReceiver.OnPayloadReceived((object payload, string attribute) =>
            {
                ThreadHelperClass.UI_Invoke(this, null, UITestConfigInstructionParameterGrid, (Hashtable d) =>
                    {
                        instruction_list = (JObject)payload;
                        
                        foreach(KeyValuePair<string, JToken> instruction in instruction_list)
                        {
                            UITestConfigInstructionSelect.Items.Add(instruction.Key);
                        }
                    },
                null);
            });

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
                LiveDataRow row = new LiveDataRow
                {
                    name = attribute,
                    value = payload.ToString()
                };

                ObservedDataRow observedRow = new ObservedDataRow
                {
                    name = attribute,
                    value = payload.ToString()
                };

                if (!liveDataList.ContainsKey(row.name))
                {
                    row.index = UIliveOutputValuesList.Rows.Add(new string[] { row.name, row.value });
                    liveDataList.Add(attribute, row);
                }

                if(!observedDataRows.ContainsKey(row.name))
                {
                    if(dataReceiver.Observe)
                    {
                        AddParameterControlRow(observedRow);
                    }
                }

                row = (LiveDataRow) liveDataList[attribute];
                row.value = payload.ToString();
                UIliveOutputValuesList.Rows[row.index].SetValues(new string[] { row.name, row.value });
            }, null);
        }

        private void AddParameterControlRow(ObservedDataRow row)
        {
            int rowIndex = UIParameterControlInput.Rows.Add(new string[] { row.name, row.min, row.max });
            UIParameterControlInput.Rows[rowIndex].Tag = row.name;
            row.input_index = rowIndex;

            rowIndex = UIObservedValuesOuputGrid.Rows.Add(new string[] { row.name, row.value, "Not configured", "Not configured" });
            UIObservedValuesOuputGrid.Rows[rowIndex].Tag = row.name;
            row.output_index = rowIndex;

            if(observedDataRows.ContainsKey(row.name))
            {
                observedDataRows[row.name] = row;
            } else
            {
                observedDataRows.Add(row.name, row);
            }
        }

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
                        UITestConfigOutputParamChecklist.Items.Add(elem.Key);
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

        public void LoadParameterControlTemplate(ParameterControlTemplate template)
        {
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

                    Program.serial.Output(Instruction.Subscription("subscribe", attribute));
                }

                if(Program.socketHandler != null)
                {
                    dataReceiver.Subscribe(Program.socketHandler, attribute, type);
                }

                Debug.Log("Subscribed to " + attribute);
            } else
            {
                Debug.Log("DO unsubscribe plx!");

                if(Program.serial != null)
                {
                    Program.serial.Output(Instruction.Subscription("unsubscribe", attribute));
                }

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
            if(UITestConfigInstructionSelect.SelectedItem == null)
            {
                return;
            }

            if(activeTestConfiguration == null)
            {
                activeTestConfiguration = new TestConfiguration();
            }

            string instruction_name = UITestConfigInstructionSelect.SelectedItem.ToString();
            JObject paramTable = new JObject();

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
                    paramTable.Add(key, JToken.Parse(value.ToString()));
                }
            }

            if(paramTable.Count > 0)
            {
                int index = UITestConfigIntructionListGrid.Rows.Add(new string[] { instruction_name, paramTable.ToString(), "pending" });

                activeTestConfiguration.AddInstruction(new Instruction(instruction_name, paramTable), index);
            }
        }

        private void UITestConfigRunTestBtn_Click(object sender, EventArgs e)
        {
            if(activeTestConfiguration != null)
            {
                RunTestConfiguration(activeTestConfiguration);
            } else
            {
                MessageBox.Show("There is no active test configuration");
            }
        }
        #endregion

        private void RunTestConfiguration(TestConfiguration config)
        {
            config.OnQueueAdvance(OnTestQueueAdvance);
            config.OnQueueComplete(OnTestQueueComplete);
            config.Run(Program.streamSimulator);
        }

        private int last_instruction_index = -1;
        private void OnTestQueueAdvance(Instruction instruction)
        {
            int index = instruction.UI_Index;

            if (last_instruction_index > -1)
            {
                UITestConfigIntructionListGrid.Rows[last_instruction_index].Cells[2].Value = "Finished";
            }

            UITestConfigIntructionListGrid.Rows[index].Cells[2].Value = "Running";
            last_instruction_index = index;
        }

        private void OnTestQueueComplete(Instruction instruction)
        {
            UITestConfigIntructionListGrid.Rows[last_instruction_index].Cells[2].Value = "Finished";
        }

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

        private void UITestConfigInstructionMoveDownBtn_Click(object sender, EventArgs e)
        {
            return; // Disabled for now;
            DataGridViewSelectedRowCollection selectedRows = UITestConfigIntructionListGrid.SelectedRows;

            if(selectedRows.Count == 0)
            {
                return;
            }

            int oldIndex = UITestConfigIntructionListGrid.Rows.IndexOf(selectedRows[0]);
            int newIndex = oldIndex + 1;

            if(newIndex + selectedRows.Count <= UITestConfigIntructionListGrid.Rows.Count)
            {
                UITestConfigIntructionListGrid.ClearSelection();
                foreach(DataGridViewRow row in selectedRows)
                {
                    UITestConfigIntructionListGrid.Rows.RemoveAt(row.Index);
                }

                for(int i=0; i<selectedRows.Count; i++)
                {
                    UITestConfigIntructionListGrid.Rows.Insert(newIndex, selectedRows[i]);
                    UITestConfigIntructionListGrid.Rows[newIndex].Selected = true;
                    newIndex++;
                }
            }
        }

        private void UITestConfigInstructionMoveUpBtn_Click(object sender, EventArgs e)
        {
            return; // Disabled for now
            DataGridViewSelectedRowCollection selectedRows = UITestConfigIntructionListGrid.SelectedRows;

            if (selectedRows.Count == 0)
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

        }
    }
}
