using LiteDB;
using LiveCharts;
using LiveCharts.Wpf;
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

                        SeriesCollection seriesCollection = new SeriesCollection();
                        

                        LineSeries lineSeries = new LineSeries();
                        lineSeries.Values = new ChartValues<float>();
                        lineSeries.Fill = System.Windows.Media.Brushes.Transparent;

                        foreach (var i in result.values)
                        {
                            lineSeries.Values.Add(Convert.ToSingle(i));
                        }
                        seriesCollection.Add(lineSeries);


                        UIdatabasePlotView.Visible = true;
                        UIdatabasePlotView.Series = seriesCollection;

                        Console.WriteLine(result.ToString());
                    }
                }
            }
        }
    }
}
