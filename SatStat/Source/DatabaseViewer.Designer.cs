namespace SatStat
{
    partial class DatabaseViewer
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
            this.UIdatabaseCollectionList = new System.Windows.Forms.ListView();
            this.UIdatabasePlotView = new OxyPlot.WindowsForms.PlotView();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.74422F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.25578F));
            this.tableLayoutPanel1.Controls.Add(this.UIdatabaseCollectionList, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.UIdatabasePlotView, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1341, 777);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // UIdatabaseCollectionList
            // 
            this.UIdatabaseCollectionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UIdatabaseCollectionList.FullRowSelect = true;
            this.UIdatabaseCollectionList.GridLines = true;
            this.UIdatabaseCollectionList.Location = new System.Drawing.Point(3, 3);
            this.UIdatabaseCollectionList.Name = "UIdatabaseCollectionList";
            this.UIdatabaseCollectionList.Size = new System.Drawing.Size(299, 771);
            this.UIdatabaseCollectionList.TabIndex = 1;
            this.UIdatabaseCollectionList.UseCompatibleStateImageBehavior = false;
            this.UIdatabaseCollectionList.View = System.Windows.Forms.View.List;
            this.UIdatabaseCollectionList.SelectedIndexChanged += new System.EventHandler(this.UIdatabaseCollectionList_SelectedIndexChanged);
            // 
            // UIdatabasePlotView
            // 
            this.UIdatabasePlotView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UIdatabasePlotView.Location = new System.Drawing.Point(308, 3);
            this.UIdatabasePlotView.Name = "UIdatabasePlotView";
            this.UIdatabasePlotView.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.UIdatabasePlotView.Size = new System.Drawing.Size(1030, 771);
            this.UIdatabasePlotView.TabIndex = 2;
            this.UIdatabasePlotView.Text = "plotView1";
            this.UIdatabasePlotView.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.UIdatabasePlotView.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.UIdatabasePlotView.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // DatabaseViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1341, 777);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DatabaseViewer";
            this.Text = "DatabaseViewer";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView UIdatabaseCollectionList;
        private OxyPlot.WindowsForms.PlotView UIdatabasePlotView;
    }
}