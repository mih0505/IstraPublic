using Istra.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Istra
{
    public partial class PlanForm : Form
    {
        IstraContext db = new IstraContext();

        public PlanForm()
        {
            InitializeComponent();
        }

        private void PlanForm_Load(object sender, EventArgs e)
        {
            //заполнение списка учебного года
            var years = db.Years.OrderBy(a=>a.SortIndex).ToList();
            cbYear.DataSource = years;
            cbYear.DisplayMember = "Name";
            cbYear.ValueMember = "Id";
            //выбор последнего учебного года
            var maxYear = db.Years.Max(a => a.Id);
            cbYear.SelectedValue = maxYear;

            var groups = db.Groups.Where(a => a.YearId == (int)cbYear.SelectedValue && a.Activity.Name != "Закрытые").OrderBy(a=>a.Name).ToList();

            dataGridView1.DataSource = groups;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Name != "Name" && col.Name != "PlanEnroll")
                    col.Visible = false;
            }
            
            dataGridView1.Columns["Name"].HeaderText = "Группа";
            dataGridView1.Columns["PlanEnroll"].HeaderText = "План";

        }

        private void cbYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var groups = db.Groups.Where(a => a.YearId == (int)cbYear.SelectedValue && a.Activity.Name != "Закрытые").OrderBy(a => a.Name).ToList();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            dataGridView1.DataSource = groups;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Name != "Name" && col.Name != "PlanEnroll")
                    col.Visible = false;
            }

            dataGridView1.Columns["Name"].HeaderText = "Группа";
            dataGridView1.Columns["PlanEnroll"].HeaderText = "План";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var groups = db.Groups.Where(a => a.YearId == (int)cbYear.SelectedValue && a.Activity.Name != "Закрытые").OrderBy(a => a.Name).ToList();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["PlanEnroll"].Value != null)
                    db.Database.ExecuteSqlCommand("UPDATE Groups SET PlanEnroll="+ Convert.ToInt32(row.Cells["PlanEnroll"].Value) +" WHERE Id="+row.Cells["Id"].Value);
            }
            MessageBox.Show("План набора сохранен", "Сохранение", MessageBoxButtons.OK);
        }
    }
}
