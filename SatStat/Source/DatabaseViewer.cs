using LiteDB;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatStat
{
    public partial class DatabaseViewer : Form
    {
        string collectionName = Program.settings.PlotDatabase;
        string databasePath = Program.settings.DatabasePath;
        PlotModel plotModel;
        DateTimeAxis xAxis;
        LinearAxis yAxis;

        public DatabaseViewer()
        {
            InitializeComponent();

            plotModel = new PlotModel() { Title = "Database view" };

            xAxis = new DateTimeAxis
            {
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                Title = "Time",
                //Maximum = xMaxVal,
                //Minimum = xMinVal
                MinorIntervalType = DateTimeIntervalType.Minutes
            };
            
            yAxis = new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "Value",
                MinimumRange = 10
            };

            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);

            UIdatabasePlotView.Model = plotModel;

            DisplayCollectionList();
        }

        public void DisplayCollectionList()
        {
            if (File.Exists(databasePath))
            {
                using (LiteDatabase db = new LiteDatabase(databasePath))
                {
                    LiteCollection<DB_SensorDataItem> col = db.GetCollection<DB_SensorDataItem>(collectionName);

                    IEnumerable<DB_SensorDataItem> results = col.FindAll();

                    foreach(DB_SensorDataItem result in results)
                    {
                        ListViewItem _item = new ListViewItem();
                        _item.Text = result.time().ToString() + ": " + result.title;
                        _item.Tag = result.Id;
                        UIdatabaseCollectionList.Items.Add(_item);
                    }
                }
            }
        }

        private void UIdatabaseCollectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(UIdatabaseCollectionList.SelectedIndices.Count > 0)
            {
                plotModel.Series.Clear();
                
                for (int i=0; i<UIdatabaseCollectionList.SelectedIndices.Count; i++)
                {
                    ListViewItem item = UIdatabaseCollectionList.Items[UIdatabaseCollectionList.SelectedIndices[i]];

                    using (LiteDatabase db = new LiteDatabase(databasePath))
                    {
                        LiteCollection<DB_SensorDataItem> col = db.GetCollection<DB_SensorDataItem>(collectionName);

                        if (item.Tag != null)
                        {
                            DB_SensorDataItem result = col.FindById(new BsonValue(item.Tag));

                            plotModel.ResetAllAxes();

                            List<DataPoint> values = new List<DataPoint>();

                            for (int j=0; j<result.values.Count; j++)
                            {
                                values.Add(new DataPoint(Convert.ToDouble(result.times[j]), Convert.ToDouble(result.values[j])));
                            }

                            LineSeries lineSeries = new LineSeries();
                            lineSeries.Title = result.title;
                            plotModel.Series.Add(lineSeries);
                            lineSeries.Points.AddRange(values);

                            plotModel.InvalidatePlot(true);

                            UIdatabasePlotView.Visible = true;
                        }
                    }
                }
            }
        }
    }
}
