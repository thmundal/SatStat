namespace SatStat
{
    partial class ParameterControlTemplateDialog
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.UIParameterControlTemplateList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.UIParameterControlTemplateSaveDialog_CancelButton = new System.Windows.Forms.Button();
            this.UITemplateLoadButton = new System.Windows.Forms.Button();
            this.UITemplateSaveButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.UIParameterControlTemplateDescriptionInput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.UIParameterTemplateNameInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.UIParameterTemplateDeleteBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.04041F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.95959F));
            this.tableLayoutPanel1.Controls.Add(this.UIParameterControlTemplateList, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.UIParameterTemplateDeleteBtn, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.41558F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.58442F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(594, 251);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // UIParameterControlTemplateList
            // 
            this.UIParameterControlTemplateList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UIParameterControlTemplateList.FormattingEnabled = true;
            this.UIParameterControlTemplateList.Location = new System.Drawing.Point(3, 23);
            this.UIParameterControlTemplateList.Name = "UIParameterControlTemplateList";
            this.UIParameterControlTemplateList.Size = new System.Drawing.Size(315, 188);
            this.UIParameterControlTemplateList.TabIndex = 0;
            this.UIParameterControlTemplateList.SelectedIndexChanged += new System.EventHandler(this.UIParameterControlTemplateList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(315, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Template list";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.UIParameterControlTemplateSaveDialog_CancelButton);
            this.flowLayoutPanel1.Controls.Add(this.UITemplateLoadButton);
            this.flowLayoutPanel1.Controls.Add(this.UITemplateSaveButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(342, 217);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(249, 31);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // UIParameterControlTemplateSaveDialog_CancelButton
            // 
            this.UIParameterControlTemplateSaveDialog_CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.UIParameterControlTemplateSaveDialog_CancelButton.Location = new System.Drawing.Point(171, 3);
            this.UIParameterControlTemplateSaveDialog_CancelButton.Name = "UIParameterControlTemplateSaveDialog_CancelButton";
            this.UIParameterControlTemplateSaveDialog_CancelButton.Size = new System.Drawing.Size(75, 23);
            this.UIParameterControlTemplateSaveDialog_CancelButton.TabIndex = 3;
            this.UIParameterControlTemplateSaveDialog_CancelButton.Text = "Cancel";
            this.UIParameterControlTemplateSaveDialog_CancelButton.UseVisualStyleBackColor = true;
            this.UIParameterControlTemplateSaveDialog_CancelButton.Click += new System.EventHandler(this.UIParameterControlTemplateSaveDialog_CancelButton_Click);
            // 
            // UITemplateLoadButton
            // 
            this.UITemplateLoadButton.Location = new System.Drawing.Point(90, 3);
            this.UITemplateLoadButton.Name = "UITemplateLoadButton";
            this.UITemplateLoadButton.Size = new System.Drawing.Size(75, 23);
            this.UITemplateLoadButton.TabIndex = 5;
            this.UITemplateLoadButton.Text = "Load";
            this.UITemplateLoadButton.UseVisualStyleBackColor = true;
            this.UITemplateLoadButton.Click += new System.EventHandler(this.UITemplateLoadButton_Click);
            // 
            // UITemplateSaveButton
            // 
            this.UITemplateSaveButton.Location = new System.Drawing.Point(9, 3);
            this.UITemplateSaveButton.Name = "UITemplateSaveButton";
            this.UITemplateSaveButton.Size = new System.Drawing.Size(75, 23);
            this.UITemplateSaveButton.TabIndex = 4;
            this.UITemplateSaveButton.Text = "Save";
            this.UITemplateSaveButton.UseVisualStyleBackColor = true;
            this.UITemplateSaveButton.Click += new System.EventHandler(this.UITemplateSaveButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.UIParameterControlTemplateDescriptionInput);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.UIParameterTemplateNameInput);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(324, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(267, 188);
            this.panel1.TabIndex = 3;
            // 
            // UIParameterControlTemplateDescriptionInput
            // 
            this.UIParameterControlTemplateDescriptionInput.Location = new System.Drawing.Point(10, 65);
            this.UIParameterControlTemplateDescriptionInput.Multiline = true;
            this.UIParameterControlTemplateDescriptionInput.Name = "UIParameterControlTemplateDescriptionInput";
            this.UIParameterControlTemplateDescriptionInput.Size = new System.Drawing.Size(248, 108);
            this.UIParameterControlTemplateDescriptionInput.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Description";
            // 
            // UIParameterTemplateNameInput
            // 
            this.UIParameterTemplateNameInput.Location = new System.Drawing.Point(7, 21);
            this.UIParameterTemplateNameInput.Name = "UIParameterTemplateNameInput";
            this.UIParameterTemplateNameInput.Size = new System.Drawing.Size(251, 20);
            this.UIParameterTemplateNameInput.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Template name";
            // 
            // UIParameterTemplateDeleteBtn
            // 
            this.UIParameterTemplateDeleteBtn.Location = new System.Drawing.Point(3, 217);
            this.UIParameterTemplateDeleteBtn.Name = "UIParameterTemplateDeleteBtn";
            this.UIParameterTemplateDeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.UIParameterTemplateDeleteBtn.TabIndex = 4;
            this.UIParameterTemplateDeleteBtn.Text = "Delete";
            this.UIParameterTemplateDeleteBtn.UseVisualStyleBackColor = true;
            this.UIParameterTemplateDeleteBtn.Click += new System.EventHandler(this.UIParameterTemplateDeleteBtn_Click);
            // 
            // ParameterControlTemplateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 251);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ParameterControlTemplateDialog";
            this.Text = "Save parameter control template";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox UIParameterControlTemplateList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button UITemplateLoadButton;
        private System.Windows.Forms.Button UITemplateSaveButton;
        private System.Windows.Forms.Button UIParameterControlTemplateSaveDialog_CancelButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox UIParameterTemplateNameInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox UIParameterControlTemplateDescriptionInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button UIParameterTemplateDeleteBtn;
    }
}