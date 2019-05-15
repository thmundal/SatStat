using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatStat
{
    public partial class TestConfigSaveDialog : Form
    {
        TestConfiguration testConfiguration;

        public TestConfigSaveDialog(TestConfiguration config, bool load = false)
        {
            InitializeComponent();
            testConfiguration = config;

            if(load)
            {
                UITestConfigLoadBtn.Visible = true;
                UITestConfigSaveButton.Visible = false;
                AcceptButton = UITestConfigLoadBtn;
            } else
            {
                AcceptButton = UITestConfigSaveButton;
                UITestConfigLoadBtn.Visible = false;
                UITestConfigSaveButton.Visible = true;
            }

            PopulateSavedList();
        }

        private void UITestConfigSaveButton_Click(object sender, EventArgs e)
        {
            if (testConfiguration != null)
            {
                testConfiguration.Name = UITestConfigName.Text;
                testConfiguration.Description = UITestConfigDescription.Text;

                TestConfiguration selectedItem = (TestConfiguration) UITestConfigDialogExistingConfigsList.SelectedItem;

                testConfiguration.Save(selectedItem, (config) => {
                    UITestConfigDialogExistingConfigsList.Items.Add(config);
                    Close();
                });
            }
        }

        private void UITestConfigLoadBtn_Click(object sender, EventArgs e)
        {
            if(UITestConfigDialogExistingConfigsList.SelectedItems.Count > 0)
            {
                TestConfiguration config = (TestConfiguration) UITestConfigDialogExistingConfigsList.SelectedItem;
                Program.app.TestConfigTab.loadTestConfiguration(config);
                Close();
            }
        }

        private void PopulateSavedList()
        {
            using (LiteDatabase db = new LiteDatabase(Program.settings.DatabasePath))
            {
                LiteCollection<TestConfiguration> collection = db.GetCollection<TestConfiguration>(Program.settings.TestConfigDatabase);

                IEnumerable<TestConfiguration> result = collection.FindAll();

                foreach(TestConfiguration row in result)
                {
                    UITestConfigDialogExistingConfigsList.Items.Add(row);
                }
            }
        }

        private void UITestConfigDialogExistingConfigsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            TestConfiguration current = (TestConfiguration) UITestConfigDialogExistingConfigsList.SelectedItem;

            if(current != null)
            {
                UITestConfigName.Text = current.Name;
                UITestConfigDescription.Text = current.Description;
            }
        }
    }
}
