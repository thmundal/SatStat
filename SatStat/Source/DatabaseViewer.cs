using LiteDB;
using OxyPlot;
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
        string collectionName = "ChartData";
        string databasePath = Directory.GetCurrentDirectory() + @"\Database.db";

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
                ListViewItem item = UIdatabaseCollectionList.Items[UIdatabaseCollectionList.SelectedIndices[0]];
                Console.WriteLine("SelectedIndexChanged");
                Console.WriteLine(item.Tag);

                using (LiteDatabase db = new LiteDatabase(databasePath))
                {
                    LiteCollection<DB_SensorDataItem> col = db.GetCollection<DB_SensorDataItem>(collectionName);

                    if (item.Tag != null)
                    {
                        DB_SensorDataItem result = col.FindById(new BsonValue(item.Tag));

                        PlotModel plotModel = new PlotModel() { Title = result.title };

                        LineSeries lineSeries = new LineSeries();
                        List<DataPoint> values = new List<DataPoint>();


                        //foreach (var i in result.values)
                        for(int i=0; i<result.values.Count; i++)
                        {
                            //lineSeries.Values.Add();
                            values.Add(new DataPoint(Convert.ToSingle(i), Convert.ToSingle(result.values[i])));
                        }

                        lineSeries.Points.AddRange(values);
                        plotModel.Series.Add(lineSeries);

                        UIdatabasePlotView.Model = plotModel;
                        UIdatabasePlotView.Visible = true;

                        Console.WriteLine(result.ToString());
                    }
                }
            }
        }
    }
}
