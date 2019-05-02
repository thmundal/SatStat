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
    public partial class ParameterControlTemplateDialog : Form
    {
        private string db_path = Program.settings.DatabasePath;
        private string collection_name = Program.settings.ParameterControlDatabase;
        private List<ParameterControlTemplate> templateList;
        public bool load = false;

        public ParameterControlTemplateDialog(bool load)
        {
            InitializeComponent();

            if(load)
            {
                UITemplateLoadButton.Visible = true;
                UITemplateSaveButton.Visible = false;
                AcceptButton = UITemplateLoadButton;
            } else
            {
                UITemplateLoadButton.Visible = false;
                UITemplateSaveButton.Visible = true;
                AcceptButton = UITemplateSaveButton;
            }

            templateList = Program.app.parameterControlTemplates;
            
            foreach(ParameterControlTemplate template in templateList)
            {
                int add_index = UIParameterControlTemplateList.Items.Add(template.Name);
            }
        }

        private void UITemplateSaveButton_Click(object sender, EventArgs e)
        {
            string name = UIParameterTemplateNameInput.Text;

            if(name.Length > 0)
            {
                using (LiteDatabase db = new LiteDatabase(db_path))
                {
                    ObservableNumericValueCollection observedValues = Program.app.DataReceiver.ObservedValues;
                    LiteCollection<ParameterControlTemplate> collection = db.GetCollection<ParameterControlTemplate>(collection_name);

                    if(UIParameterControlTemplateList.SelectedIndex == -1)
                    {
                        ParameterControlTemplate template = new ParameterControlTemplate(observedValues);
                        template.Name = UIParameterTemplateNameInput.Text;
                        template.Description = UIParameterControlTemplateDescriptionInput.Text;

                        collection.Insert(template);
                    } else
                    {
                        ParameterControlTemplate template = templateList[UIParameterControlTemplateList.SelectedIndex];
                        template.Name = UIParameterTemplateNameInput.Text;
                        template.Description = UIParameterControlTemplateDescriptionInput.Text;
                        template.SetCollection(observedValues);

                        if (!collection.Update(template.Id, template))
                        {
                            Debug.Log("Update failed");
                        } else
                        {
                            Debug.Log("Update OK");
                        }
                    }
                }

                Close();
            } else
            {
                MessageBox.Show("Cannot save, no name given");
            }
        }

        private void UITemplateLoadButton_Click(object sender, EventArgs e)
        {
            if(UIParameterControlTemplateList.SelectedIndex > -1)
            {
                ParameterControlTemplate template = templateList[UIParameterControlTemplateList.SelectedIndex];

                if(template != null)
                {
                    Program.app.LoadParameterControlTemplate(template);
                    Close();
                } else
                {
                    MessageBox.Show("Cannot load template");
                }
            }
        }

        private void UIParameterControlTemplateSaveDialog_CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UIParameterControlTemplateList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(UIParameterControlTemplateList.SelectedIndex > -1)
            {
                ParameterControlTemplate template = templateList[UIParameterControlTemplateList.SelectedIndex];

                if(template  != null)
                {
                    UIParameterTemplateNameInput.Text = template.Name;
                    UIParameterControlTemplateDescriptionInput.Text = template.Description;
                }
            }
        }

        private void UIParameterTemplateDeleteBtn_Click(object sender, EventArgs e)
        {
            if(UIParameterControlTemplateList.SelectedIndex > -1)
            {
                DialogResult confirm = MessageBox.Show("Do you want to delete this template?\nThis action cannot be undone.", "Confirm delete", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.Yes)
                {
                    ParameterControlTemplate template = templateList[UIParameterControlTemplateList.SelectedIndex];

                    using (LiteDatabase db = new LiteDatabase(db_path))
                    {
                        LiteCollection<ParameterControlTemplate> collection = db.GetCollection<ParameterControlTemplate>(collection_name);

                        if(collection.Delete(template.Id))
                        {
                            UIParameterControlTemplateList.Items.RemoveAt(UIParameterControlTemplateList.SelectedIndex);
                            templateList.Remove(template);
                        }
                    }
                }
            } else
            {
                MessageBox.Show("You have to select a template to delete first.");
            }
        }
    }
}
