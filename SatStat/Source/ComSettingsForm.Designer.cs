namespace SatStat
{
    partial class ComSettingsForm
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
            this.UIcomSourcesList = new System.Windows.Forms.ListBox();
            this.UIComSettingsConnectBtn = new System.Windows.Forms.Button();
            this.UIcomPortSettingsStep1Panel = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.COMPortListLabel = new System.Windows.Forms.Label();
            this.UIbaudRateSelectionList = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.UIbaudRateInputList = new System.Windows.Forms.ListBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.UIComSettingsCancelBtn = new System.Windows.Forms.Button();
            this.UIComSettingsApplyBtn = new System.Windows.Forms.Button();
            this.UIcomPortSettingsStep1Panel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.UIbaudRateSelectionList.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // UIcomSourcesList
            // 
            this.UIcomSourcesList.FormattingEnabled = true;
            this.UIcomSourcesList.Location = new System.Drawing.Point(3, 16);
            this.UIcomSourcesList.Name = "UIcomSourcesList";
            this.UIcomSourcesList.Size = new System.Drawing.Size(235, 95);
            this.UIcomSourcesList.TabIndex = 0;
            // 
            // UIComSettingsConnectBtn
            // 
            this.UIComSettingsConnectBtn.Location = new System.Drawing.Point(275, 3);
            this.UIComSettingsConnectBtn.Name = "UIComSettingsConnectBtn";
            this.UIComSettingsConnectBtn.Size = new System.Drawing.Size(75, 23);
            this.UIComSettingsConnectBtn.TabIndex = 1;
            this.UIComSettingsConnectBtn.Text = "Next >>";
            this.UIComSettingsConnectBtn.UseVisualStyleBackColor = true;
            this.UIComSettingsConnectBtn.Click += new System.EventHandler(this.UIComSettingsConnectBtn_Click);
            // 
            // UIcomPortSettingsStep1Panel
            // 
            this.UIcomPortSettingsStep1Panel.AutoSize = true;
            this.UIcomPortSettingsStep1Panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.UIcomPortSettingsStep1Panel.BackColor = System.Drawing.SystemColors.Control;
            this.UIcomPortSettingsStep1Panel.Controls.Add(this.panel1);
            this.UIcomPortSettingsStep1Panel.Controls.Add(this.UIbaudRateSelectionList);
            this.UIcomPortSettingsStep1Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.UIcomPortSettingsStep1Panel.Location = new System.Drawing.Point(0, 0);
            this.UIcomPortSettingsStep1Panel.Name = "UIcomPortSettingsStep1Panel";
            this.UIcomPortSettingsStep1Panel.Size = new System.Drawing.Size(434, 117);
            this.UIcomPortSettingsStep1Panel.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.COMPortListLabel);
            this.panel1.Controls.Add(this.UIcomSourcesList);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(244, 111);
            this.panel1.TabIndex = 3;
            // 
            // COMPortListLabel
            // 
            this.COMPortListLabel.AutoSize = true;
            this.COMPortListLabel.Location = new System.Drawing.Point(0, 0);
            this.COMPortListLabel.Name = "COMPortListLabel";
            this.COMPortListLabel.Size = new System.Drawing.Size(53, 13);
            this.COMPortListLabel.TabIndex = 2;
            this.COMPortListLabel.Text = "COM Port";
            // 
            // UIbaudRateSelectionList
            // 
            this.UIbaudRateSelectionList.Controls.Add(this.label1);
            this.UIbaudRateSelectionList.Controls.Add(this.UIbaudRateInputList);
            this.UIbaudRateSelectionList.Location = new System.Drawing.Point(253, 3);
            this.UIbaudRateSelectionList.Name = "UIbaudRateSelectionList";
            this.UIbaudRateSelectionList.Size = new System.Drawing.Size(174, 111);
            this.UIbaudRateSelectionList.TabIndex = 4;
            this.UIbaudRateSelectionList.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Baud Rate";
            // 
            // UIbaudRateInputList
            // 
            this.UIbaudRateInputList.FormattingEnabled = true;
            this.UIbaudRateInputList.Location = new System.Drawing.Point(4, 16);
            this.UIbaudRateInputList.Name = "UIbaudRateInputList";
            this.UIbaudRateInputList.Size = new System.Drawing.Size(167, 95);
            this.UIbaudRateInputList.TabIndex = 1;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.UIComSettingsCancelBtn);
            this.flowLayoutPanel2.Controls.Add(this.UIComSettingsConnectBtn);
            this.flowLayoutPanel2.Controls.Add(this.UIComSettingsApplyBtn);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 117);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(434, 29);
            this.flowLayoutPanel2.TabIndex = 4;
            // 
            // UIComSettingsCancelBtn
            // 
            this.UIComSettingsCancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.UIComSettingsCancelBtn.Location = new System.Drawing.Point(356, 3);
            this.UIComSettingsCancelBtn.Name = "UIComSettingsCancelBtn";
            this.UIComSettingsCancelBtn.Size = new System.Drawing.Size(75, 23);
            this.UIComSettingsCancelBtn.TabIndex = 1;
            this.UIComSettingsCancelBtn.Text = "Cancel";
            this.UIComSettingsCancelBtn.UseVisualStyleBackColor = true;
            // 
            // UIComSettingsApplyBtn
            // 
            this.UIComSettingsApplyBtn.Location = new System.Drawing.Point(194, 3);
            this.UIComSettingsApplyBtn.Name = "UIComSettingsApplyBtn";
            this.UIComSettingsApplyBtn.Size = new System.Drawing.Size(75, 23);
            this.UIComSettingsApplyBtn.TabIndex = 2;
            this.UIComSettingsApplyBtn.Text = "Apply";
            this.UIComSettingsApplyBtn.UseVisualStyleBackColor = true;
            this.UIComSettingsApplyBtn.Visible = false;
            this.UIComSettingsApplyBtn.Click += new System.EventHandler(this.UIComSettingsApplyBtn_Click);
            // 
            // ComSettingsForm
            // 
            this.AcceptButton = this.UIComSettingsConnectBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.UIComSettingsCancelBtn;
            this.ClientSize = new System.Drawing.Size(434, 146);
            this.Controls.Add(this.UIcomPortSettingsStep1Panel);
            this.Controls.Add(this.flowLayoutPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 185);
            this.Name = "ComSettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "COM Settings";
            this.UIcomPortSettingsStep1Panel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.UIbaudRateSelectionList.ResumeLayout(false);
            this.UIbaudRateSelectionList.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox UIcomSourcesList;
        private System.Windows.Forms.Button UIComSettingsConnectBtn;
        private System.Windows.Forms.FlowLayoutPanel UIcomPortSettingsStep1Panel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label COMPortListLabel;
        private System.Windows.Forms.Panel UIbaudRateSelectionList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox UIbaudRateInputList;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button UIComSettingsCancelBtn;
        private System.Windows.Forms.Button UIComSettingsApplyBtn;
    }
}