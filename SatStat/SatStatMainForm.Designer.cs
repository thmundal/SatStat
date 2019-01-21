﻿namespace SatStat
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.inputDataSource1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.inputDataBtn1 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.solidGauge1 = new LiveCharts.WinForms.SolidGauge();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.BackColor = System.Drawing.SystemColors.Window;
            this.cartesianChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartesianChart1.Location = new System.Drawing.Point(0, 0);
            this.cartesianChart1.Margin = new System.Windows.Forms.Padding(4);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(595, 595);
            this.cartesianChart1.TabIndex = 0;
            this.cartesianChart1.Text = "cartesianChart1";
            this.cartesianChart1.UpdaterTick += new LiveCharts.Events.UpdaterTickHandler(this.ChartOnUpdaterTick);
            this.cartesianChart1.ChildChanged += new System.EventHandler<System.Windows.Forms.Integration.ChildChangedEventArgs>(this.cartesianChart1_ChildChanged);
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.cartesianChart1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(396, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(595, 595);
            this.panel1.TabIndex = 1;
            // 
            // inputDataSource1
            // 
            this.inputDataSource1.Location = new System.Drawing.Point(26, 38);
            this.inputDataSource1.Name = "inputDataSource1";
            this.inputDataSource1.Size = new System.Drawing.Size(139, 20);
            this.inputDataSource1.TabIndex = 2;
            this.inputDataSource1.TextChanged += new System.EventHandler(this.inputDataSource1_TextChanged);
            this.inputDataSource1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputDataSource1_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Input data point";
            // 
            // inputDataBtn1
            // 
            this.inputDataBtn1.Location = new System.Drawing.Point(175, 38);
            this.inputDataBtn1.Name = "inputDataBtn1";
            this.inputDataBtn1.Size = new System.Drawing.Size(52, 20);
            this.inputDataBtn1.TabIndex = 4;
            this.inputDataBtn1.Text = "Input";
            this.inputDataBtn1.UseVisualStyleBackColor = true;
            this.inputDataBtn1.Click += new System.EventHandler(this.inputDataBtn1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(233, 38);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 20);
            this.button1.TabIndex = 5;
            this.button1.Text = "Remove";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // solidGauge1
            // 
            this.solidGauge1.Location = new System.Drawing.Point(12, 386);
            this.solidGauge1.Name = "solidGauge1";
            this.solidGauge1.Size = new System.Drawing.Size(377, 197);
            this.solidGauge1.TabIndex = 7;
            this.solidGauge1.Text = "solidGauge1";
            // 
            // SatStatMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 595);
            this.Controls.Add(this.solidGauge1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.inputDataBtn1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inputDataSource1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SatStatMainForm";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox inputDataSource1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button inputDataBtn1;
        private System.Windows.Forms.Button button1;
        private LiveCharts.WinForms.SolidGauge solidGauge1;
    }
}

