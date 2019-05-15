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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.UImenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOMSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectFromSerialDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.connectToStreamSimulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectFromStreamSimulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.startSocketServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopSocketServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parameterControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveParameterControlTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.saveTestConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTestConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.runCurrentTestConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearCurrentTestConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.UIPlotViewTabContainer = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.UIPlotViewTab = new System.Windows.Forms.TabPage();
            this.oxPlot = new OxyPlot.WindowsForms.PlotView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.UIObservedValuesOuputGrid = new System.Windows.Forms.DataGridView();
            this.attribute = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.observedValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.difference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UIParameterControlInput = new System.Windows.Forms.DataGridView();
            this.ParamKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParamMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParamMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TestConfigTabContainer = new System.Windows.Forms.TabPage();
            this.TestConfigTab = new SatStat.TestConfigTabControl();
            this.UImenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SadmControlsGroupPanel.SuspendLayout();
            this.SensorDataListBoxGroupContainer.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.diagnosticLiveOutputValues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UIliveOutputValuesList)).BeginInit();
            this.UIStatusStrip.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.UIPlotViewTabContainer.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.UIPlotViewTab.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UIObservedValuesOuputGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UIParameterControlInput)).BeginInit();
            this.TestConfigTabContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // UImenuStrip1
            // 
            this.UImenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.dataToolStripMenuItem,
            this.parameterControlToolStripMenuItem,
            this.testConfigurationToolStripMenuItem});
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
            this.disconnectFromSerialDeviceToolStripMenuItem,
            this.toolStripSeparator3,
            this.connectToStreamSimulatorToolStripMenuItem,
            this.disconnectFromStreamSimulatorToolStripMenuItem,
            this.toolStripSeparator2,
            this.startSocketServerToolStripMenuItem,
            this.stopSocketServerToolStripMenuItem,
            this.toolStripSeparator1});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // cOMSettingsToolStripMenuItem
            // 
            this.cOMSettingsToolStripMenuItem.Name = "cOMSettingsToolStripMenuItem";
            this.cOMSettingsToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.cOMSettingsToolStripMenuItem.Text = "Connect to serial device...";
            this.cOMSettingsToolStripMenuItem.Click += new System.EventHandler(this.cOMSettingsToolStripMenuItem_Click);
            // 
            // disconnectFromSerialDeviceToolStripMenuItem
            // 
            this.disconnectFromSerialDeviceToolStripMenuItem.Name = "disconnectFromSerialDeviceToolStripMenuItem";
            this.disconnectFromSerialDeviceToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.disconnectFromSerialDeviceToolStripMenuItem.Text = "Disconnect from serial device";
            this.disconnectFromSerialDeviceToolStripMenuItem.Click += new System.EventHandler(this.disconnectFromSerialDeviceToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(251, 6);
            // 
            // connectToStreamSimulatorToolStripMenuItem
            // 
            this.connectToStreamSimulatorToolStripMenuItem.Name = "connectToStreamSimulatorToolStripMenuItem";
            this.connectToStreamSimulatorToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.connectToStreamSimulatorToolStripMenuItem.Text = "Connect to stream simulator";
            this.connectToStreamSimulatorToolStripMenuItem.Click += new System.EventHandler(this.connectToStreamSimulatorToolStripMenuItem_Click);
            // 
            // disconnectFromStreamSimulatorToolStripMenuItem
            // 
            this.disconnectFromStreamSimulatorToolStripMenuItem.Name = "disconnectFromStreamSimulatorToolStripMenuItem";
            this.disconnectFromStreamSimulatorToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.disconnectFromStreamSimulatorToolStripMenuItem.Text = "Disconnect from stream simulator";
            this.disconnectFromStreamSimulatorToolStripMenuItem.Click += new System.EventHandler(this.disconnectFromStreamSimulatorToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(251, 6);
            // 
            // startSocketServerToolStripMenuItem
            // 
            this.startSocketServerToolStripMenuItem.Name = "startSocketServerToolStripMenuItem";
            this.startSocketServerToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.startSocketServerToolStripMenuItem.Text = "Start socket server";
            this.startSocketServerToolStripMenuItem.Click += new System.EventHandler(this.startSocketServerToolStripMenuItem_Click);
            // 
            // stopSocketServerToolStripMenuItem
            // 
            this.stopSocketServerToolStripMenuItem.Name = "stopSocketServerToolStripMenuItem";
            this.stopSocketServerToolStripMenuItem.Size = new System.Drawing.Size(254, 22);
            this.stopSocketServerToolStripMenuItem.Text = "Stop socket server";
            this.stopSocketServerToolStripMenuItem.Click += new System.EventHandler(this.stopSocketServerToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(251, 6);
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
            // parameterControlToolStripMenuItem
            // 
            this.parameterControlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveParameterControlTemplateToolStripMenuItem,
            this.loadTemplateToolStripMenuItem});
            this.parameterControlToolStripMenuItem.Name = "parameterControlToolStripMenuItem";
            this.parameterControlToolStripMenuItem.Size = new System.Drawing.Size(114, 20);
            this.parameterControlToolStripMenuItem.Text = "Parameter control";
            // 
            // saveParameterControlTemplateToolStripMenuItem
            // 
            this.saveParameterControlTemplateToolStripMenuItem.Name = "saveParameterControlTemplateToolStripMenuItem";
            this.saveParameterControlTemplateToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.saveParameterControlTemplateToolStripMenuItem.Text = "Save template";
            this.saveParameterControlTemplateToolStripMenuItem.Click += new System.EventHandler(this.saveParameterControlTemplateToolStripMenuItem_Click);
            // 
            // loadTemplateToolStripMenuItem
            // 
            this.loadTemplateToolStripMenuItem.Name = "loadTemplateToolStripMenuItem";
            this.loadTemplateToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.loadTemplateToolStripMenuItem.Text = "Load template";
            this.loadTemplateToolStripMenuItem.Click += new System.EventHandler(this.loadTemplateToolStripMenuItem_Click);
            // 
            // testConfigurationToolStripMenuItem
            // 
            this.testConfigurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem,
            this.toolStripSeparator5,
            this.saveTestConfigurationToolStripMenuItem,
            this.loadTestConfigurationToolStripMenuItem,
            this.toolStripSeparator4,
            this.runCurrentTestConfigurationToolStripMenuItem,
            this.clearCurrentTestConfigurationToolStripMenuItem});
            this.testConfigurationToolStripMenuItem.Name = "testConfigurationToolStripMenuItem";
            this.testConfigurationToolStripMenuItem.Size = new System.Drawing.Size(115, 20);
            this.testConfigurationToolStripMenuItem.Text = "Test configuration";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.runToolStripMenuItem.Text = "Run";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(236, 6);
            // 
            // saveTestConfigurationToolStripMenuItem
            // 
            this.saveTestConfigurationToolStripMenuItem.Name = "saveTestConfigurationToolStripMenuItem";
            this.saveTestConfigurationToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.saveTestConfigurationToolStripMenuItem.Text = "Save test configuration";
            this.saveTestConfigurationToolStripMenuItem.Click += new System.EventHandler(this.SaveTestConfigurationToolStripMenuItem_Click);
            // 
            // loadTestConfigurationToolStripMenuItem
            // 
            this.loadTestConfigurationToolStripMenuItem.Name = "loadTestConfigurationToolStripMenuItem";
            this.loadTestConfigurationToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.loadTestConfigurationToolStripMenuItem.Text = "Load test configuration";
            this.loadTestConfigurationToolStripMenuItem.Click += new System.EventHandler(this.LoadTestConfigurationToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(236, 6);
            // 
            // runCurrentTestConfigurationToolStripMenuItem
            // 
            this.runCurrentTestConfigurationToolStripMenuItem.Name = "runCurrentTestConfigurationToolStripMenuItem";
            this.runCurrentTestConfigurationToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.runCurrentTestConfigurationToolStripMenuItem.Text = "Run current test configuration";
            // 
            // clearCurrentTestConfigurationToolStripMenuItem
            // 
            this.clearCurrentTestConfigurationToolStripMenuItem.Name = "clearCurrentTestConfigurationToolStripMenuItem";
            this.clearCurrentTestConfigurationToolStripMenuItem.Size = new System.Drawing.Size(239, 22);
            this.clearCurrentTestConfigurationToolStripMenuItem.Text = "Clear current test configuration";
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
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.UIStatusStrip, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.MainTabControl, 1, 0);
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
            this.tableLayoutPanel2.Size = new System.Drawing.Size(428, 437);
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
            this.SadmControlsGroupPanel.Size = new System.Drawing.Size(422, 237);
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
            this.SensorDataListBoxGroupContainer.Size = new System.Drawing.Size(422, 188);
            this.SensorDataListBoxGroupContainer.TabIndex = 15;
            this.SensorDataListBoxGroupContainer.TabStop = false;
            this.SensorDataListBoxGroupContainer.Text = "Sensor data";
            // 
            // UISensorCheckboxList
            // 
            this.UISensorCheckboxList.CheckOnClick = true;
            this.UISensorCheckboxList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UISensorCheckboxList.FormattingEnabled = true;
            this.UISensorCheckboxList.Location = new System.Drawing.Point(3, 16);
            this.UISensorCheckboxList.Name = "UISensorCheckboxList";
            this.UISensorCheckboxList.Size = new System.Drawing.Size(416, 169);
            this.UISensorCheckboxList.TabIndex = 14;
            this.UISensorCheckboxList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.UISensorCheckboxList_ItemCheck);
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
            this.tableLayoutPanel3.Size = new System.Drawing.Size(428, 258);
            this.tableLayoutPanel3.TabIndex = 14;
            // 
            // diagnosticLiveOutputValues
            // 
            this.diagnosticLiveOutputValues.Controls.Add(this.UIliveOutputValuesList);
            this.diagnosticLiveOutputValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagnosticLiveOutputValues.Location = new System.Drawing.Point(3, 3);
            this.diagnosticLiveOutputValues.Name = "diagnosticLiveOutputValues";
            this.diagnosticLiveOutputValues.Size = new System.Drawing.Size(422, 243);
            this.diagnosticLiveOutputValues.TabIndex = 0;
            this.diagnosticLiveOutputValues.TabStop = false;
            this.diagnosticLiveOutputValues.Text = "Live output values";
            // 
            // UIliveOutputValuesList
            // 
            this.UIliveOutputValuesList.AllowUserToResizeColumns = false;
            this.UIliveOutputValuesList.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            this.UIliveOutputValuesList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.UIliveOutputValuesList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.UIliveOutputValuesList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.UIliveOutputValuesList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UIliveOutputValuesList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.UIliveOutputValuesList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UIliveOutputValuesList.ColumnHeadersVisible = false;
            this.UIliveOutputValuesList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Key,
            this.Value});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.UIliveOutputValuesList.DefaultCellStyle = dataGridViewCellStyle6;
            this.UIliveOutputValuesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UIliveOutputValuesList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.UIliveOutputValuesList.Location = new System.Drawing.Point(3, 16);
            this.UIliveOutputValuesList.MultiSelect = false;
            this.UIliveOutputValuesList.Name = "UIliveOutputValuesList";
            this.UIliveOutputValuesList.ReadOnly = true;
            this.UIliveOutputValuesList.RowHeadersVisible = false;
            this.UIliveOutputValuesList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.UIliveOutputValuesList.Size = new System.Drawing.Size(416, 224);
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
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.UIPlotViewTabContainer);
            this.MainTabControl.Controls.Add(this.tabPage2);
            this.MainTabControl.Controls.Add(this.TestConfigTabContainer);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(437, 3);
            this.MainTabControl.Name = "MainTabControl";
            this.tableLayoutPanel1.SetRowSpan(this.MainTabControl, 2);
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(983, 701);
            this.MainTabControl.TabIndex = 16;
            this.MainTabControl.SelectedIndexChanged += new System.EventHandler(this.MainTabControl_SelectedIndexChanged);
            // 
            // UIPlotViewTabContainer
            // 
            this.UIPlotViewTabContainer.Controls.Add(this.tabControl2);
            this.UIPlotViewTabContainer.Location = new System.Drawing.Point(4, 22);
            this.UIPlotViewTabContainer.Name = "UIPlotViewTabContainer";
            this.UIPlotViewTabContainer.Padding = new System.Windows.Forms.Padding(3);
            this.UIPlotViewTabContainer.Size = new System.Drawing.Size(975, 675);
            this.UIPlotViewTabContainer.TabIndex = 0;
            this.UIPlotViewTabContainer.Tag = "plotViewTab";
            this.UIPlotViewTabContainer.Text = "Plot view";
            this.UIPlotViewTabContainer.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.UIPlotViewTab);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(969, 669);
            this.tabControl2.TabIndex = 13;
            // 
            // UIPlotViewTab
            // 
            this.UIPlotViewTab.Controls.Add(this.oxPlot);
            this.UIPlotViewTab.Location = new System.Drawing.Point(4, 22);
            this.UIPlotViewTab.Name = "UIPlotViewTab";
            this.UIPlotViewTab.Padding = new System.Windows.Forms.Padding(3);
            this.UIPlotViewTab.Size = new System.Drawing.Size(961, 643);
            this.UIPlotViewTab.TabIndex = 0;
            this.UIPlotViewTab.Text = "Plot 1";
            this.UIPlotViewTab.UseVisualStyleBackColor = true;
            // 
            // oxPlot
            // 
            this.oxPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oxPlot.Location = new System.Drawing.Point(3, 3);
            this.oxPlot.Name = "oxPlot";
            this.oxPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.oxPlot.Size = new System.Drawing.Size(955, 637);
            this.oxPlot.TabIndex = 1;
            this.oxPlot.Text = "plotView1";
            this.oxPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.oxPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.oxPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(961, 643);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Plot 2";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(975, 675);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Parameter control";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.5645F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.4355F));
            this.tableLayoutPanel4.Controls.Add(this.UIObservedValuesOuputGrid, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.UIParameterControlInput, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(969, 669);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // UIObservedValuesOuputGrid
            // 
            this.UIObservedValuesOuputGrid.AllowUserToResizeColumns = false;
            this.UIObservedValuesOuputGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            this.UIObservedValuesOuputGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.UIObservedValuesOuputGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.UIObservedValuesOuputGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.UIObservedValuesOuputGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UIObservedValuesOuputGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.UIObservedValuesOuputGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UIObservedValuesOuputGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.attribute,
            this.observedValue,
            this.status,
            this.difference});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.UIObservedValuesOuputGrid.DefaultCellStyle = dataGridViewCellStyle8;
            this.UIObservedValuesOuputGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UIObservedValuesOuputGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.UIObservedValuesOuputGrid.Location = new System.Drawing.Point(366, 3);
            this.UIObservedValuesOuputGrid.MultiSelect = false;
            this.UIObservedValuesOuputGrid.Name = "UIObservedValuesOuputGrid";
            this.UIObservedValuesOuputGrid.ReadOnly = true;
            this.UIObservedValuesOuputGrid.RowHeadersVisible = false;
            this.UIObservedValuesOuputGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.UIObservedValuesOuputGrid.Size = new System.Drawing.Size(600, 663);
            this.UIObservedValuesOuputGrid.TabIndex = 1;
            // 
            // attribute
            // 
            this.attribute.HeaderText = "Attribute";
            this.attribute.Name = "attribute";
            this.attribute.ReadOnly = true;
            // 
            // observedValue
            // 
            this.observedValue.HeaderText = "Value";
            this.observedValue.Name = "observedValue";
            this.observedValue.ReadOnly = true;
            // 
            // status
            // 
            this.status.HeaderText = "Status";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // difference
            // 
            this.difference.HeaderText = "Difference";
            this.difference.Name = "difference";
            this.difference.ReadOnly = true;
            // 
            // UIParameterControlInput
            // 
            this.UIParameterControlInput.AllowUserToAddRows = false;
            this.UIParameterControlInput.AllowUserToResizeColumns = false;
            this.UIParameterControlInput.AllowUserToResizeRows = false;
            this.UIParameterControlInput.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.UIParameterControlInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UIParameterControlInput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ParamKey,
            this.ParamMin,
            this.ParamMax});
            this.UIParameterControlInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UIParameterControlInput.Location = new System.Drawing.Point(3, 3);
            this.UIParameterControlInput.MultiSelect = false;
            this.UIParameterControlInput.Name = "UIParameterControlInput";
            this.UIParameterControlInput.RowHeadersVisible = false;
            this.UIParameterControlInput.Size = new System.Drawing.Size(357, 663);
            this.UIParameterControlInput.TabIndex = 0;
            this.UIParameterControlInput.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.UIParameterControlInput_CellValidating);
            // 
            // ParamKey
            // 
            this.ParamKey.HeaderText = "Data attribute";
            this.ParamKey.Name = "ParamKey";
            this.ParamKey.ReadOnly = true;
            this.ParamKey.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ParamMin
            // 
            this.ParamMin.HeaderText = "Minimum";
            this.ParamMin.Name = "ParamMin";
            this.ParamMin.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ParamMax
            // 
            this.ParamMax.HeaderText = "Maximum";
            this.ParamMax.Name = "ParamMax";
            this.ParamMax.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // TestConfigTabContainer
            // 
            this.TestConfigTabContainer.Controls.Add(this.TestConfigTab);
            this.TestConfigTabContainer.Location = new System.Drawing.Point(4, 22);
            this.TestConfigTabContainer.Name = "TestConfigTabContainer";
            this.TestConfigTabContainer.Padding = new System.Windows.Forms.Padding(3);
            this.TestConfigTabContainer.Size = new System.Drawing.Size(975, 675);
            this.TestConfigTabContainer.TabIndex = 3;
            this.TestConfigTabContainer.Text = "Test Configuration";
            this.TestConfigTabContainer.UseVisualStyleBackColor = true;
            // 
            // TestConfigTab
            // 
            this.TestConfigTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TestConfigTab.Location = new System.Drawing.Point(3, 3);
            this.TestConfigTab.Name = "TestConfigTab";
            this.TestConfigTab.Size = new System.Drawing.Size(969, 669);
            this.TestConfigTab.TabIndex = 0;
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
            this.MainTabControl.ResumeLayout(false);
            this.UIPlotViewTabContainer.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.UIPlotViewTab.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UIObservedValuesOuputGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UIParameterControlInput)).EndInit();
            this.TestConfigTabContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip UImenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cOMSettingsToolStripMenuItem;
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
        private System.Windows.Forms.StatusStrip UIStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel UICOMConnectionStatus;
        private System.Windows.Forms.ToolStripStatusLabel UINetworkConnectionStatus;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage UIPlotViewTabContainer;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage UIPlotViewTab;
        private OxyPlot.WindowsForms.PlotView oxPlot;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.DataGridView UIParameterControlInput;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParamKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParamMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParamMax;
        private System.Windows.Forms.DataGridView UIObservedValuesOuputGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn attribute;
        private System.Windows.Forms.DataGridViewTextBoxColumn observedValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn difference;
        private System.Windows.Forms.ToolStripMenuItem parameterControlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveParameterControlTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectFromSerialDeviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectFromStreamSimulatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem stopSocketServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem saveTestConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTestConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem runCurrentTestConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearCurrentTestConfigurationToolStripMenuItem;
        private System.Windows.Forms.TabPage TestConfigTabContainer;
        public TestConfigTabControl TestConfigTab;
    }
}

