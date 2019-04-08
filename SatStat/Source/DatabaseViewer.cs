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

        public DatabaseViewer()
        {
            InitializeComponent();
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
                // Change to support multiple selections!
                ListViewItem item = UIdatabaseCollectionList.Items[UIdatabaseCollectionList.SelectedIndices[0]];
                Debug.Log("SelectedIndexChanged");
                Debug.Log(item.Tag);

                using (LiteDatabase db = new LiteDatabase(databasePath))
                {
                    LiteCollection<DB_SensorDataItem> col = db.GetCollection<DB_SensorDataItem>(collectionName);

                    if (item.Tag != null)
                    {
                        DB_SensorDataItem result = col.FindById(new BsonValue(item.Tag));

                        PlotModel plotModel = new PlotModel() { Title = result.title };

                        LineSeries lineSeries = new LineSeries();
                        List<DataPoint> values = new List<DataPoint>();

                        DateTimeAxis xAxis = new DateTimeAxis
                        {
                            Key = "xAxis",
                            Position = AxisPosition.Bottom,
                            Title = "Time",
                            //Maximum = xMaxVal,
                            //Minimum = xMinVal
                            Minimum = DateTimeAxis.ToDouble(result.times[0]),
                            Maximum = DateTimeAxis.ToDouble(result.times[result.times.Count - 1]),
                            MinorIntervalType = DateTimeIntervalType.Minutes
                        };


                        LinearAxis yAxis = new LinearAxis
                        {
                            Key = "yAxis",
                            Position = AxisPosition.Left,
                            Title = "Value",
                            MinimumRange = 10
                        };

                        plotModel.Axes.Add(xAxis);
                        plotModel.Axes.Add(yAxis);

                        //foreach (var i in result.values)
                        for (int i=0; i<result.values.Count; i++)
                        {
                            //lineSeries.Values.Add();
                            values.Add(new DataPoint(Convert.ToDouble(result.times[i]), Convert.ToDouble(result.values[i])));
                        }

                        lineSeries.Points.AddRange(values);
                        plotModel.Series.Add(lineSeries);

                        UIdatabasePlotView.Model = plotModel;
                        UIdatabasePlotView.Visible = true;

                        Debug.Log(result.ToString());
                    }
                }
            }
        }
    }
}
