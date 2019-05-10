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

        public TestConfigSaveDialog(TestConfiguration config)
        {
            InitializeComponent();
            testConfiguration = config;
        }

        private void UITestConfigSaveButton_Click(object sender, EventArgs e)
        {
            if (testConfiguration != null)
            {
                testConfiguration.Name = UITestConfigName.Text;

                testConfiguration.Save((config) => {
                    UITestConfigDialogExistingConfigsList.Items.Add(config);
                    Close();
                });
            }
        }
    }
}
