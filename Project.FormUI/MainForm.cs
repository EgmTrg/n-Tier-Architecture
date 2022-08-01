using Project.Entity;
using Project.ORM;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace Project.FormUI
{
    public partial class main_Form : Form
    {
        public main_Form() {
            InitializeComponent();
        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e) {
            List<Categories> categories = CategoriesORM.Current.Select().Data;
            FillWithData(categories);
        }

        private bool FillWithData<T>(List<T> values) {
            /* Fill columns header with class properties */
            List<string> props = GetProperties<Categories>();
            foreach (string header in props) {
                dataGridView1.Columns.Add(header, header);
                dataGridView1.Columns[header].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // Fill the datagridview
            dataGridView1.Rows.Add(values.Count);
            for (int i = 0; i < values.Count; i++) {
                for (int k = 0; k < props.Count; k++) {
                    dataGridView1.Rows[i].Cells[k].Value = values[k];
                }
            }

            // return if completed true otherwise false.

            return false;
        }


        public static List<string> GetProperties<T>() where T : class {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<string> props = new List<string>();
            foreach (var prop in properties) {
                props.Add(prop.Name);
            }
            return props;
        }
    }
}
