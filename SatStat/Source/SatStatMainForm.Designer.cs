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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.oxPlot = new OxyPlot.WindowsForms.PlotView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.UImenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOMSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToStreamSimulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startSocketServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.SadmControlsGroupPanel = new System.Windows.Forms.GroupBox();
            this.UIrotateStepsInput = new System.Windows.Forms.TextBox();
            this.UIrotateAngleInput = new System.Windows.Forms.TextBox();
            this.UIrotateStepsLabel = new System.Windows.Forms.Label();
            this.UIrotateAngleLabel = new System.Windows.Forms.Label();
            this.UIrotateStepsBtn = new System.Windows.Forms.Button();
            this.UIrotateAngleBtn = new System.Windows.Forms.Button();
            this.UIsetMotorSpeedBtn = new System.Windows.Forms.Button();
            this.UImotorSpeedLabel = new System.Windows.Forms.Label();
            this.UImotorSpeedInput = new System.Windows.Forms.TextBox();
            this.UIautoRotateOnBtn = new System.Windows.Forms.Button();
            this.UIAutoRotateOffBtn = new System.Windows.Forms.Button();
            this.SensorDataListBoxGroupContainer = new System.Windows.Forms.GroupBox();
            this.UISensorCheckboxList = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.diagnosticLiveOutputValues = new System.Windows.Forms.GroupBox();
            this.UIliveOutputValuesList = new System.Windows.Forms.DataGridView();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UIStatusStrip = new System.Windows.Forms.StatusStrip();
            this.UICOMConnectionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.UINetworkConnectionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.UImenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SadmControlsGroupPanel.SuspendLayout();
            this.SensorDataListBoxGroupContainer.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.diagnosticLiveOutputValues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UIliveOutputValuesList)).BeginInit();
            this.UIStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(436, 3);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 2);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(984, 701);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.oxPlot);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(976, 675);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Plot 1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // oxPlot
            // 
            this.oxPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oxPlot.Location = new System.Drawing.Point(3, 3);
            this.oxPlot.Name = "oxPlot";
            this.oxPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.oxPlot.Size = new System.Drawing.Size(970, 669);
            this.oxPlot.TabIndex = 1;
            this.oxPlot.Text = "plotView1";
            this.oxPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.oxPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.oxPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(957, 675);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Plot 2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // UImenuStrip1
            // 
            this.UImenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.dataToolStripMenuItem});
            this.UImenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.UImenuStrip1.Name = "UImenuStrip1";
            this.UImenuStrip1.Size = new System.Drawing.Size(1423, 24);
            this.UImenuStrip1.TabIndex = 8;
            this.UImenuStrip1.Text = "menuStrip1";
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
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToDatabaseToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.importToolStripMenuItem,
            this.displayDatabaseToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.dataToolStripMenuItem.Text = "Data";
            // 
            // saveToDatabaseToolStripMenuItem
            // 
            this.saveToDatabaseToolStripMenuItem.Name = "saveToDatabaseToolStripMenuItem";
            this.saveToDatabaseToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.saveToDatabaseToolStripMenuItem.Text = "Save to database";
            this.saveToDatabaseToolStripMenuItem.Click += new System.EventHandler(this.saveToDatabaseToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.importToolStripMenuItem.Text = "Import";
            // 
            // displayDatabaseToolStripMenuItem
            // 
            this.displayDatabaseToolStripMenuItem.Name = "displayDatabaseToolStripMenuItem";
            this.displayDatabaseToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.displayDatabaseToolStripMenuItem.Text = "Display Database";
            this.displayDatabaseToolStripMenuItem.Click += new System.EventHandler(this.displayDatabaseToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(0, 728);
            this.panel1.TabIndex = 12;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.49895F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.50105F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.UIStatusStrip, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62.63736F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.36264F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1423, 728);
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
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.70934F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.29066F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(427, 437);
            this.tableLayoutPanel2.TabIndex = 13;
            // 
            // SadmControlsGroupPanel
            // 
            this.SadmControlsGroupPanel.Controls.Add(this.UIrotateStepsInput);
            this.SadmControlsGroupPanel.Controls.Add(this.UIrotateAngleInput);
            this.SadmControlsGroupPanel.Controls.Add(this.UIrotateStepsLabel);
            this.SadmControlsGroupPanel.Controls.Add(this.UIrotateAngleLabel);
            this.SadmControlsGroupPanel.Controls.Add(this.UIrotateStepsBtn);
            this.SadmControlsGroupPanel.Controls.Add(this.UIrotateAngleBtn);
            this.SadmControlsGroupPanel.Controls.Add(this.UIsetMotorSpeedBtn);
            this.SadmControlsGroupPanel.Controls.Add(this.UImotorSpeedLabel);
            this.SadmControlsGroupPanel.Controls.Add(this.UImotorSpeedInput);
            this.SadmControlsGroupPanel.Controls.Add(this.UIautoRotateOnBtn);
            this.SadmControlsGroupPanel.Controls.Add(this.UIAutoRotateOffBtn);
            this.SadmControlsGroupPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SadmControlsGroupPanel.Location = new System.Drawing.Point(3, 3);
            this.SadmControlsGroupPanel.Name = "SadmControlsGroupPanel";
            this.SadmControlsGroupPanel.Size = new System.Drawing.Size(421, 237);
            this.SadmControlsGroupPanel.TabIndex = 14;
            this.SadmControlsGroupPanel.TabStop = false;
            this.SadmControlsGroupPanel.Text = "SADM Controls";
            // 
            // UIrotateStepsInput
            // 
            this.UIrotateStepsInput.Location = new System.Drawing.Point(190, 76);
            this.UIrotateStepsInput.Name = "UIrotateStepsInput";
            this.UIrotateStepsInput.Size = new System.Drawing.Size(100, 20);
            this.UIrotateStepsInput.TabIndex = 23;
            // 
            // UIrotateAngleInput
            // 
            this.UIrotateAngleInput.Location = new System.Drawing.Point(190, 50);
            this.UIrotateAngleInput.Name = "UIrotateAngleInput";
            this.UIrotateAngleInput.Size = new System.Drawing.Size(100, 20);
            this.UIrotateAngleInput.TabIndex = 22;
            // 
            // UIrotateStepsLabel
            // 
            this.UIrotateStepsLabel.AutoSize = true;
            this.UIrotateStepsLabel.Location = new System.Drawing.Point(117, 78);
            this.UIrotateStepsLabel.Name = "UIrotateStepsLabel";
            this.UIrotateStepsLabel.Size = new System.Drawing.Size(34, 13);
            this.UIrotateStepsLabel.TabIndex = 21;
            this.UIrotateStepsLabel.Text = "Steps";
            // 
            // UIrotateAngleLabel
            // 
            this.UIrotateAngleLabel.AutoSize = true;
            this.UIrotateAngleLabel.Location = new System.Drawing.Point(117, 53);
            this.UIrotateAngleLabel.Name = "UIrotateAngleLabel";
            this.UIrotateAngleLabel.Size = new System.Drawing.Size(34, 13);
            this.UIrotateAngleLabel.TabIndex = 20;
            this.UIrotateAngleLabel.Text = "Angle";
            // 
            // UIrotateStepsBtn
            // 
            this.UIrotateStepsBtn.Location = new System.Drawing.Point(297, 74);
            this.UIrotateStepsBtn.Name = "UIrotateStepsBtn";
            this.UIrotateStepsBtn.Size = new System.Drawing.Size(75, 23);
            this.UIrotateStepsBtn.TabIndex = 19;
            this.UIrotateStepsBtn.Text = "Rotate";
            this.UIrotateStepsBtn.UseVisualStyleBackColor = true;
            this.UIrotateStepsBtn.Click += new System.EventHandler(this.UIrotateStepsBtn_Click);
            // 
            // UIrotateAngleBtn
            // 
            this.UIrotateAngleBtn.Location = new System.Drawing.Point(297, 48);
            this.UIrotateAngleBtn.Name = "UIrotateAngleBtn";
            this.UIrotateAngleBtn.Size = new System.Drawing.Size(75, 23);
            this.UIrotateAngleBtn.TabIndex = 18;
            this.UIrotateAngleBtn.Text = "Rotate";
            this.UIrotateAngleBtn.UseVisualStyleBackColor = true;
            this.UIrotateAngleBtn.Click += new System.EventHandler(this.UIrotateAngleBtn_Click);
            // 
            // UIsetMotorSpeedBtn
            // 
            this.UIsetMotorSpeedBtn.Location = new System.Drawing.Point(297, 18);
            this.UIsetMotorSpeedBtn.Name = "UIsetMotorSpeedBtn";
            this.UIsetMotorSpeedBtn.Size = new System.Drawing.Size(102, 23);
            this.UIsetMotorSpeedBtn.TabIndex = 17;
            this.UIsetMotorSpeedBtn.Text = "Set motor speed";
            this.UIsetMotorSpeedBtn.UseVisualStyleBackColor = true;
            this.UIsetMotorSpeedBtn.Click += new System.EventHandler(this.UIsetMotorSpeedBtn_Click);
            // 
            // UImotorSpeedLabel
            // 
            this.UImotorSpeedLabel.AutoSize = true;
            this.UImotorSpeedLabel.Location = new System.Drawing.Point(117, 23);
            this.UImotorSpeedLabel.Name = "UImotorSpeedLabel";
            this.UImotorSpeedLabel.Size = new System.Drawing.Size(68, 13);
            this.UImotorSpeedLabel.TabIndex = 16;
            this.UImotorSpeedLabel.Text = "Motor Speed";
            // 
            // UImotorSpeedInput
            // 
            this.UImotorSpeedInput.Location = new System.Drawing.Point(191, 19);
            this.UImotorSpeedInput.Name = "UImotorSpeedInput";
            this.UImotorSpeedInput.Size = new System.Drawing.Size(100, 20);
            this.UImotorSpeedInput.TabIndex = 15;
            // 
            // UIautoRotateOnBtn
            // 
            this.UIautoRotateOnBtn.Location = new System.Drawing.Point(6, 19);
            this.UIautoRotateOnBtn.Name = "UIautoRotateOnBtn";
            this.UIautoRotateOnBtn.Size = new System.Drawing.Size(105, 23);
            this.UIautoRotateOnBtn.TabIndex = 9;
            this.UIautoRotateOnBtn.Text = "Start Auto Rotate";
            this.UIautoRotateOnBtn.UseVisualStyleBackColor = true;
            this.UIautoRotateOnBtn.Click += new System.EventHandler(this.UIautoRotateOnBtn_Click);
            // 
            // UIAutoRotateOffBtn
            // 
            this.UIAutoRotateOffBtn.Location = new System.Drawing.Point(6, 48);
            this.UIAutoRotateOffBtn.Name = "UIAutoRotateOffBtn";
            this.UIAutoRotateOffBtn.Size = new System.Drawing.Size(105, 23);
            this.UIAutoRotateOffBtn.TabIndex = 10;
            this.UIAutoRotateOffBtn.Text = "Stop Auto Rotate";
            this.UIAutoRotateOffBtn.UseVisualStyleBackColor = true;
            this.UIAutoRotateOffBtn.Click += new System.EventHandler(this.UIAutoRotateOffBtn_Click);
            // 
            // SensorDataListBoxGroupContainer
            // 
            this.SensorDataListBoxGroupContainer.Controls.Add(this.UISensorCheckboxList);
            this.SensorDataListBoxGroupContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SensorDataListBoxGroupContainer.Location = new System.Drawing.Point(3, 246);
            this.SensorDataListBoxGroupContainer.Name = "SensorDataListBoxGroupContainer";
            this.SensorDataListBoxGroupContainer.Size = new System.Drawing.Size(421, 188);
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
            this.UISensorCheckboxList.Size = new System.Drawing.Size(415, 169);
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
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 446);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 96.61654F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.383459F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(427, 258);
            this.tableLayoutPanel3.TabIndex = 14;
            // 
            // diagnosticLiveOutputValues
            // 
            this.diagnosticLiveOutputValues.Controls.Add(this.UIliveOutputValuesList);
            this.diagnosticLiveOutputValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagnosticLiveOutputValues.Location = new System.Drawing.Point(3, 3);
            this.diagnosticLiveOutputValues.Name = "diagnosticLiveOutputValues";
            this.diagnosticLiveOutputValues.Size = new System.Drawing.Size(421, 243);
            this.diagnosticLiveOutputValues.TabIndex = 0;
            this.diagnosticLiveOutputValues.TabStop = false;
            this.diagnosticLiveOutputValues.Text = "Live output values";
            // 
            // UIliveOutputValuesList
            // 
            this.UIliveOutputValuesList.AllowUserToResizeColumns = false;
            this.UIliveOutputValuesList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.UIliveOutputValuesList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.UIliveOutputValuesList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.UIliveOutputValuesList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.UIliveOutputValuesList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UIliveOutputValuesList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.UIliveOutputValuesList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UIliveOutputValuesList.ColumnHeadersVisible = false;
            this.UIliveOutputValuesList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Key,
            this.Value});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.UIliveOutputValuesList.DefaultCellStyle = dataGridViewCellStyle2;
            this.UIliveOutputValuesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UIliveOutputValuesList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.UIliveOutputValuesList.Location = new System.Drawing.Point(3, 16);
            this.UIliveOutputValuesList.MultiSelect = false;
            this.UIliveOutputValuesList.Name = "UIliveOutputValuesList";
            this.UIliveOutputValuesList.ReadOnly = true;
            this.UIliveOutputValuesList.RowHeadersVisible = false;
            this.UIliveOutputValuesList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.UIliveOutputValuesList.Size = new System.Drawing.Size(415, 224);
            this.UIliveOutputValuesList.TabIndex = 0;
            // 
            // Key
            // 
            this.Key.HeaderText = "Key";
            this.Key.Name = "Key";
            this.Key.ReadOnly = true;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            // 
            // UIStatusStrip
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.UIStatusStrip, 2);
            this.UIStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UICOMConnectionStatus,
            this.UINetworkConnectionStatus});
            this.UIStatusStrip.Location = new System.Drawing.Point(0, 707);
            this.UIStatusStrip.Name = "UIStatusStrip";
            this.UIStatusStrip.Size = new System.Drawing.Size(1423, 21);
            this.UIStatusStrip.TabIndex = 15;
            this.UIStatusStrip.Text = "statusStrip";
            // 
            // UICOMConnectionStatus
            // 
            this.UICOMConnectionStatus.Name = "UICOMConnectionStatus";
            this.UICOMConnectionStatus.Size = new System.Drawing.Size(110, 16);
            this.UICOMConnectionStatus.Text = "COM Disconnected";
            // 
            // UINetworkConnectionStatus
            // 
            this.UINetworkConnectionStatus.Name = "UINetworkConnectionStatus";
            this.UINetworkConnectionStatus.Size = new System.Drawing.Size(127, 16);
            this.UINetworkConnectionStatus.Text = "Network Disconnected";
            // 
            // SatStatMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1423, 752);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.UImenuStrip1);
            this.MainMenuStrip = this.UImenuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SatStatMainForm";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SatStatMainForm_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.UImenuStrip1.ResumeLayout(false);
            this.UImenuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.SadmControlsGroupPanel.ResumeLayout(false);
            this.SadmControlsGroupPanel.PerformLayout();
            this.SensorDataListBoxGroupContainer.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.diagnosticLiveOutputValues.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UIliveOutputValuesList)).EndInit();
            this.UIStatusStrip.ResumeLayout(false);
            this.UIStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip UImenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cOMSettingsToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox SadmControlsGroupPanel;
        private System.Windows.Forms.Button UIautoRotateOnBtn;
        private System.Windows.Forms.Button UIAutoRotateOffBtn;
        private System.Windows.Forms.GroupBox SensorDataListBoxGroupContainer;
        private System.Windows.Forms.CheckedListBox UISensorCheckboxList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox diagnosticLiveOutputValues;
        private System.Windows.Forms.DataGridView UIliveOutputValuesList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.ToolStripMenuItem connectToStreamSimulatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startSocketServerToolStripMenuItem;
        private System.Windows.Forms.Button UIsetMotorSpeedBtn;
        private System.Windows.Forms.Label UImotorSpeedLabel;
        private System.Windows.Forms.TextBox UImotorSpeedInput;
        private System.Windows.Forms.TextBox UIrotateStepsInput;
        private System.Windows.Forms.TextBox UIrotateAngleInput;
        private System.Windows.Forms.Label UIrotateStepsLabel;
        private System.Windows.Forms.Label UIrotateAngleLabel;
        private System.Windows.Forms.Button UIrotateStepsBtn;
        private System.Windows.Forms.Button UIrotateAngleBtn;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayDatabaseToolStripMenuItem;
        private OxyPlot.WindowsForms.PlotView oxPlot;
        private System.Windows.Forms.StatusStrip UIStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel UICOMConnectionStatus;
        private System.Windows.Forms.ToolStripStatusLabel UINetworkConnectionStatus;
    }
}

