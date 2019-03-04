namespace SatStat
{
    partial class SatStatMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOMSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToStreamSimulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startSocketServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.SadmControlsGroupPanel = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SensorDataListBoxGroupContainer = new System.Windows.Forms.GroupBox();
            this.UISensorCheckboxList = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.diagnosticLiveOutputValues = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SadmControlsGroupPanel.SuspendLayout();
            this.SensorDataListBoxGroupContainer.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.diagnosticLiveOutputValues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.BackColor = System.Drawing.SystemColors.Window;
            this.cartesianChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartesianChart1.Location = new System.Drawing.Point(3, 3);
            this.cartesianChart1.Margin = new System.Windows.Forms.Padding(4);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(947, 691);
            this.cartesianChart1.TabIndex = 0;
            this.cartesianChart1.Text = "cartesianChart1";
            this.cartesianChart1.UpdaterTick += new LiveCharts.Events.UpdaterTickHandler(this.ChartOnUpdaterTick);
            this.cartesianChart1.ChildChanged += new System.EventHandler<System.Windows.Forms.Integration.ChildChangedEventArgs>(this.cartesianChart1_ChildChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(454, 3);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 2);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(961, 723);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cartesianChart1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(953, 697);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(781, 592);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1418, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cOMSettingsToolStripMenuItem,
            this.connectToStreamSimulatorToolStripMenuItem,
            this.startSocketServerToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // cOMSettingsToolStripMenuItem
            // 
            this.cOMSettingsToolStripMenuItem.Name = "cOMSettingsToolStripMenuItem";
            this.cOMSettingsToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.cOMSettingsToolStripMenuItem.Text = "COM Settings";
            this.cOMSettingsToolStripMenuItem.Click += new System.EventHandler(this.cOMSettingsToolStripMenuItem_Click);
            // 
            // connectToStreamSimulatorToolStripMenuItem
            // 
            this.connectToStreamSimulatorToolStripMenuItem.Name = "connectToStreamSimulatorToolStripMenuItem";
            this.connectToStreamSimulatorToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.connectToStreamSimulatorToolStripMenuItem.Text = "Connect to stream simulator";
            this.connectToStreamSimulatorToolStripMenuItem.Click += new System.EventHandler(this.connectToStreamSimulatorToolStripMenuItem_Click);
            // 
            // startSocketServerToolStripMenuItem
            // 
            this.startSocketServerToolStripMenuItem.Name = "startSocketServerToolStripMenuItem";
            this.startSocketServerToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.startSocketServerToolStripMenuItem.Text = "Start socket server";
            this.startSocketServerToolStripMenuItem.Click += new System.EventHandler(this.startSocketServerToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(0, 729);
            this.panel1.TabIndex = 12;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.81199F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.18801F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.63291F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 59.36709F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1418, 729);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.SadmControlsGroupPanel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.SensorDataListBoxGroupContainer, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.99127F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62.00873F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(445, 290);
            this.tableLayoutPanel2.TabIndex = 13;
            // 
            // SadmControlsGroupPanel
            // 
            this.SadmControlsGroupPanel.Controls.Add(this.button2);
            this.SadmControlsGroupPanel.Controls.Add(this.button3);
            this.SadmControlsGroupPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SadmControlsGroupPanel.Location = new System.Drawing.Point(3, 3);
            this.SadmControlsGroupPanel.Name = "SadmControlsGroupPanel";
            this.SadmControlsGroupPanel.Size = new System.Drawing.Size(439, 104);
            this.SadmControlsGroupPanel.TabIndex = 14;
            this.SadmControlsGroupPanel.TabStop = false;
            this.SadmControlsGroupPanel.Text = "SADM Controls";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Start Auto Rotate";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 48);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(105, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Stop Auto Rotate";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // SensorDataListBoxGroupContainer
            // 
            this.SensorDataListBoxGroupContainer.Controls.Add(this.UISensorCheckboxList);
            this.SensorDataListBoxGroupContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SensorDataListBoxGroupContainer.Location = new System.Drawing.Point(3, 113);
            this.SensorDataListBoxGroupContainer.Name = "SensorDataListBoxGroupContainer";
            this.SensorDataListBoxGroupContainer.Size = new System.Drawing.Size(439, 174);
            this.SensorDataListBoxGroupContainer.TabIndex = 15;
            this.SensorDataListBoxGroupContainer.TabStop = false;
            this.SensorDataListBoxGroupContainer.Text = "Sensor data";
            // 
            // UISensorCheckboxList
            // 
            this.UISensorCheckboxList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UISensorCheckboxList.FormattingEnabled = true;
            this.UISensorCheckboxList.Location = new System.Drawing.Point(3, 16);
            this.UISensorCheckboxList.Name = "UISensorCheckboxList";
            this.UISensorCheckboxList.Size = new System.Drawing.Size(433, 155);
            this.UISensorCheckboxList.TabIndex = 14;
            this.UISensorCheckboxList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.UISensorCheckboxList_ItemCheck);
            this.UISensorCheckboxList.SelectedIndexChanged += new System.EventHandler(this.UISensorCheckboxList_SelectedIndexChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.diagnosticLiveOutputValues, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 299);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(445, 427);
            this.tableLayoutPanel3.TabIndex = 14;
            // 
            // diagnosticLiveOutputValues
            // 
            this.diagnosticLiveOutputValues.Controls.Add(this.dataGridView1);
            this.diagnosticLiveOutputValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagnosticLiveOutputValues.Location = new System.Drawing.Point(3, 3);
            this.diagnosticLiveOutputValues.Name = "diagnosticLiveOutputValues";
            this.diagnosticLiveOutputValues.Size = new System.Drawing.Size(439, 207);
            this.diagnosticLiveOutputValues.TabIndex = 0;
            this.diagnosticLiveOutputValues.TabStop = false;
            this.diagnosticLiveOutputValues.Text = "Live output values";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Key,
            this.Value});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 16);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(433, 188);
            this.dataGridView1.TabIndex = 0;
            // 
            // Key
            // 
            this.Key.HeaderText = "Key";
            this.Key.Name = "Key";
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // SatStatMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1418, 753);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SatStatMainForm";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SatStatMainForm_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.SadmControlsGroupPanel.ResumeLayout(false);
            this.SensorDataListBoxGroupContainer.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.diagnosticLiveOutputValues.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cOMSettingsToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox SadmControlsGroupPanel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox SensorDataListBoxGroupContainer;
        private System.Windows.Forms.CheckedListBox UISensorCheckboxList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox diagnosticLiveOutputValues;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.ToolStripMenuItem connectToStreamSimulatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startSocketServerToolStripMenuItem;
    }
}

