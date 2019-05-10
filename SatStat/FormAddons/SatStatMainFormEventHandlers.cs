using System;
using System.Collections;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using LiteDB;
using System.IO;
using OxyPlot;
using OxyPlot.Series;
using SatStat.Utils;

namespace SatStat
{
    partial class SatStatMainForm
    {
        private bool isClosing = false;
        private void SatStatMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClosing = true;
            Program.serial.Disconnect();
            if (discoverThread != null && discoverThread.IsAlive)
            {
                discoverThread.Abort();
            }
        }

        private void cOMSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comSettings = new ComSettingsForm();
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

        private void UISensorCheckboxList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox sensor_select = (CheckedListBox)sender;

            string attribute = sensor_select.Items[e.Index].ToString();
            if (!dataReceiver.HasSubscription(attribute) && sensor_select.CheckedItems.IndexOf(sensor_select.Items[e.Index]) == -1)
            {
                string type = (string)sensor_information[attribute];

                // This has to only be done once?
                CreateDataSeries(plotModel, attribute);

                //if(Program.streamSimulator != null)
                //{
                //    dataReceiver.Subscribe(Program.streamSimulator, attribute, type);
                //}

                //if(Program.serial != null && Program.serial.ConnectionStatus == ConnectionStatus.Connected)
                //{
                //    dataReceiver.Subscribe(Program.serial, attribute, type);

                //    Program.serial.Output(Request.Subscription("subscribe", attribute));
                //}

                //if(Program.socketHandler != null)
                //{
                //    dataReceiver.Subscribe(Program.socketHandler, attribute, type);
                //}

                foreach (KeyValuePair<string, DataStream> stream in activeStreams)
                {
                    if (stream.Value.ConnectionStatus == ConnectionStatus.Connected)
                    {
                        dataReceiver.Subscribe(stream.Value, attribute, type);
                    }
                }

                Debug.Log("Subscribed to " + attribute);
            }
            else
            {
                foreach (KeyValuePair<string, DataStream> stream in activeStreams)
                {
                    if (stream.Value.ConnectionStatus == ConnectionStatus.Connected)
                    {
                        dataReceiver.Unsubscribe(attribute, stream.Value);
                    }
                }

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

            if (Int32.TryParse(motorSpeedInputText, out int motorSpeed))
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
            if (Single.TryParse(UIrotateAngleInput.Text, out float angle))
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
            if (savedComSettings != null)
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

            if (activeParameterControlTemplate == null)
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
                        if(obsValue.PlotAnnotationMinLine != null)
                        {
                            RemovePlotAnnotationLine(obsValue.PlotAnnotationMinLine);
                        }

                        obsValue.Min = castNumericValue;
                        obsValue.PlotAnnotationMinLine = AddPlotAnnotationLine(Convert.ToDouble(numericValue), tag, OxyColors.Red);
                    }
                    else if (current.OwningColumn.Name == "ParamMax")
                    {
                        if (obsValue.PlotAnnotationMaxLine != null)
                        {
                            RemovePlotAnnotationLine(obsValue.PlotAnnotationMaxLine);
                        }
                        obsValue.Max = castNumericValue;
                        obsValue.PlotAnnotationMaxLine = AddPlotAnnotationLine(Convert.ToDouble(numericValue), tag, OxyColors.Red);
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
    }
}
