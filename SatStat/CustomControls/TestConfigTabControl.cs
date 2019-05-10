using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using SatStat.Utils;
using LiteDB;

namespace SatStat
{
    public partial class TestConfigTabControl : UserControl
    {
        private TestConfiguration activeTestConfiguration;
        private int last_instruction_index = -1;
        private List<InstructionUIEntry> instructionEntries;
        private ObservableNumericValueCollection observedValues;

        private List<string> observedValueLabels;

        public TestConfigTabControl()
        {
            InitializeComponent();

            activeTestConfiguration = new TestConfiguration();
            instructionEntries = new List<InstructionUIEntry>();
            observedValues = new ObservableNumericValueCollection();
            observedValueLabels = new List<string>();

            if(LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                SetUpTestConfigurationPanel();
            }
        }

        public void SetUpTestConfigurationPanel()
        {
            using (LiteDatabase db = new LiteDatabase(Program.settings.DatabasePath))
            {
                LiteCollection<ParameterControlTemplate> collection = db.GetCollection<ParameterControlTemplate>(Program.settings.ParameterControlDatabase);

                IEnumerable<ParameterControlTemplate> result = collection.FindAll();

                foreach (ParameterControlTemplate parameterControlTemplate in result)
                {
                    UITestConfigParameterTemplateSelect.Items.Add(parameterControlTemplate);
                }
            }
        }

        public void loadTestConfiguration(TestConfiguration selectedConfig)
        {
            UITestConfigIntructionListGrid.Rows.Clear();

            activeTestConfiguration = selectedConfig;

            foreach (InstructionEntry entry in selectedConfig.instructionEntries)
            {
                AddTestConfigInstructionRow(entry);
            }
        }

        public void SetObservableParameterList(ObservableNumericValueCollection observableNumericValues)
        {
            UITestConfigOutputParamChecklist.Items.Clear();
            UITestConfigOutputParamChecklist.Items.AddRange(observableNumericValues.ToArray());
        }

        public void ClearObservableParameterList()
        {
            UITestConfigOutputParamChecklist.Items.Clear();
        }

        public void SetDeviceList(List<DataStream> deviceList)
        {
            UITestDeviceSelect.Items.Clear();
            UITestDeviceSelect.Items.AddRange(deviceList.ToArray());
        }

        public void AddDeviceToList(DataStream device)
        {
            if(!UITestDeviceSelect.Items.Contains(device))
            {
                UITestDeviceSelect.Items.Add(device);
            }
        }

        public void RemoveDeviceFromList(DataStream device)
        {
            if(UITestDeviceSelect.Items.Contains(device))
            {
                UITestDeviceSelect.Items.Remove(device);
            }
        }

        private int AddTestConfigInstructionRow(InstructionEntry instructionEntry)
        {
            Instruction instruction = instructionEntry.instruction;
            int index = UITestConfigIntructionListGrid.Rows.Add(new string[] { instruction.Label, instruction.SerializedParamTable, "pending" });

            UITestConfigIntructionListGrid.Rows[index].Tag = instructionEntry;
            return index;
        }

        private void RunTestConfiguration(TestConfiguration testConfiguration, DataStream stream)
        {
            if (testConfiguration.HasInstructionEntries())
            {
                // Disable UI elements
                UITestConfigParameterTemplateSelect.Enabled = false;

                last_instruction_index = -1;
                testConfiguration.OnQueueAdvance(OnTestQueueAdvance);
                testConfiguration.OnQueueComplete(OnTestQueueComplete);
                testConfiguration.Run(stream);
            }
            else
            {
                MessageBox.Show("There are no instructions in the queue");
                return;
            }
        }

        private void AbortTestConfiguration()
        {
            if (activeTestConfiguration != null)
            {
                activeTestConfiguration.Abort();
            }
        }

        public void AddInstructionUIEntry(InstructionUIEntry entry)
        {
            if (!UITestConfigInstructionSelect.Items.Contains(entry))
            {
                UITestConfigInstructionSelect.Items.Add(entry);
            }
        }

        private void UITestConfigInstructionSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            InstructionUIEntry sel = (InstructionUIEntry) UITestConfigInstructionSelect.SelectedItem;
            UITestConfigInstructionParameterGrid.Rows.Clear();

            foreach(KeyValuePair<string, JToken> param in sel.parameters)
            {
                UITestConfigInstructionParameterGrid.Rows.Add(new string[] { param.Key, "" });
            }
        }

        private void UITestConfigAddInstructionBtn_Click(object sender, EventArgs e)
        {
            if (UITestConfigInstructionSelect.SelectedItem == null || activeTestConfiguration.IsRunning)
            {
                return;
            }

            string instruction_name = UITestConfigInstructionSelect.SelectedItem.ToString();

            InstructionUIEntry instructionUIEntry = (InstructionUIEntry)UITestConfigInstructionSelect.SelectedItem;
            JObject paramTable = new JObject();

            // Get parameters from the parameters data grid input
            for (int row = 0; row < UITestConfigInstructionParameterGrid.Rows.Count; row++)
            {
                string key = null;
                object value = null;

                for (int col = 0; col < UITestConfigInstructionParameterGrid.ColumnCount; col++)
                {
                    var cell = UITestConfigInstructionParameterGrid.Rows[row].Cells[col].Value;
                    if (col % 2 == 0)
                    {
                        // Key
                        key = cell.ToString();
                    }
                    else
                    {
                        // Value
                        string instruction_type = instructionUIEntry.parameters[key].Value<string>(); //instruction_list[instruction_name][key].Value<string>();
                        value = Cast.ToType(cell, instruction_type); // This casts, BUT only result we need here is that value is 0 if string is empty....
                    }
                }

                if (key != null && value != null)
                {
                    paramTable[key] = JToken.FromObject(value);
                }
            }

            if (paramTable.Count > 0)
            {
                foreach(IObservableNumericValue checkedObserveValue in UITestConfigOutputParamChecklist.CheckedItems)
                {
                    observedValueLabels.Add(checkedObserveValue.Label);
                }

                Instruction thatInstruction = new Instruction(instructionUIEntry.label, paramTable);

                InstructionEntry entry = new InstructionEntry
                {
                    instruction = thatInstruction,
                    observedValueLabels = observedValueLabels
                };

                int index = AddTestConfigInstructionRow(entry);
                entry.setInstructionIndex(index);
                activeTestConfiguration.AddInstructionEntry(entry);
            }
        }

        private void UITestConfigRunTestBtn_Click(object sender, EventArgs e)
        {
            if (activeTestConfiguration != null)
            {
                if (UITestDeviceSelect.SelectedIndex > -1)
                {
                    DataStream stream = (DataStream)UITestDeviceSelect.SelectedItem;
                    RunTestConfiguration(activeTestConfiguration, stream);
                }
                else
                {
                    MessageBox.Show("You have to select a device to test on");
                }
            }
            else
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

            if (selectedRows.Count == 0 || activeTestConfiguration.IsRunning)
            {
                return;
            }

            int i = 0;
            foreach (DataGridViewRow row in selectedRows)
            {
                selectedIndices[i++] = row.Index;
                InstructionEntry instr = (InstructionEntry)row.Tag;
                int instructionIndex = activeTestConfiguration.InstructionEntryIndex(instr);
                UITestConfigIntructionListGrid.Rows.RemoveAt(instructionIndex);
                activeTestConfiguration.RemoveInstructionEntry(instr);
            }

            foreach (int index in selectedIndices)
            {
                if (index < UITestConfigIntructionListGrid.Rows.Count)
                {
                    UITestConfigIntructionListGrid.Rows[index].Selected = true;
                }
            }
        }

        private ParameterControlTemplate GetSelectedParameterControlTemplate()
        {
            return (ParameterControlTemplate)UITestConfigParameterTemplateSelect.SelectedItem;
        }

        private void UITestConfigParameterTemplateSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (activeTestConfiguration.IsRunning)
            {
                MessageBox.Show("Cannot modify control parameters when test is running");
                return;
            }

            ParameterControlTemplate selectedItem = GetSelectedParameterControlTemplate();
            activeTestConfiguration.ParameterControlTemplate = null;

            if (selectedItem != null)
            {
                activeTestConfiguration.setParameterControlTemplate(selectedItem);
                SetObservableParameterList(selectedItem.GetCollection());
            }
        }

        private void UITestConfigUseCurrentParamConfigCheck_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (activeTestConfiguration.IsRunning)
            {
                return;
            }

            ClearObservableParameterList();

            if (cb.Checked)
            {
                activeTestConfiguration.setParameterControlTemplate(Program.app.autoParamControlTemplate);
                SetObservableParameterList(Program.app.autoParamControlTemplate.GetCollection());
            }
            else
            {
                ParameterControlTemplate selectedItem = GetSelectedParameterControlTemplate();
                activeTestConfiguration.ParameterControlTemplate = null;

                if (selectedItem != null)
                {
                    activeTestConfiguration.setParameterControlTemplate(selectedItem);
                    SetObservableParameterList(selectedItem.GetCollection());
                }
            }
        }

        private void UITestConfigOutputParamChecklist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UITestConfigOutputParamChecklist.SelectedItem != null)
            {
                IObservableNumericValue observableValue = (IObservableNumericValue) UITestConfigOutputParamChecklist.SelectedItem;
                bool is_checked = UITestConfigOutputParamChecklist.CheckedItems.IndexOf(observableValue) > -1;

                if (is_checked)
                {
                    if (!observedValues.Contains(observableValue))
                    {
                        observedValues.Add(observableValue);
                    }
                }
                else
                {
                    if (observedValues.Contains(observableValue))
                    {
                        observedValues.Remove(observableValue);
                    }
                }
            }
        }

        private void UIAbortTestBtn_Click(object sender, EventArgs e)
        {
            AbortTestConfiguration();
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
            ThreadHelper.UI_Invoke(Program.app, null, UITestConfigIntructionListGrid, (data) =>
            {
                Instruction instruction = instructionEntry.instruction;
                UITestConfigIntructionListGrid.Rows[last_instruction_index].Cells[2].Value = "Finished";
                UITestConfigIntructionListGrid.Rows[last_instruction_index].Cells[3].Value = instruction.feedbackStatus.ToString();

                UITestConfigParameterTemplateSelect.Enabled = true;
            }, null);
        }

        private void UITestConfigClearInstructionsBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
