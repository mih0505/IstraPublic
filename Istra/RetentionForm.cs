using Istra.Entities;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Istra
{
    public partial class RetentionForm : Form
    {
        IstraContext db = new IstraContext();
        Retention retention;
        public RetentionForm(int? id)
        {
            InitializeComponent();

            //загрузка фильтра преподавателей           
            var teachers = from teach in db.Workers
                           join depart in db.Departments on teach.DepartmentId equals depart.Id
                           where teach.IsRemoved == false
                           orderby teach.Lastname, teach.Firstname, teach.Middlename
                           select new
                           {
                               Id = teach.Id,
                               Teacher = teach.Lastname + " " + teach.Firstname.Substring(0, 1) + "." + teach.Middlename.Substring(0, 1) + ".  (" + depart.Name + ")",
                               RoleId = teach.RoleId
                           };

            cbTeachers.DataSource = teachers.OrderBy(a => a.Teacher).ToList();
            cbTeachers.DisplayMember = "Teacher";
            cbTeachers.ValueMember = "Id";
            cbTeachers.SelectedIndex = -1;

            //загрузка оснований начислений/удержаний
            var bases = db.Bases.OrderBy(a => a.Name).ToList();
            cbBase.DataSource = bases;
            cbBase.DisplayMember = "Name";
            cbBase.ValueMember = "Id";

            if (id != null)
            {
                retention = db.Retentions.Find(id);
            }
            else
            {
                retention = new Retention();
            }
        }

        private void RetentionForm_Load(object sender, EventArgs e)
        {
            if (retention.Id != 0)
            {
                dtpDate.Value = retention.Date;
                cbTeachers.SelectedValue = retention.WorkerId;
                dtpDate.Value = retention.Date;
                dtpMonth.Value = (retention.Month != null) ? retention.Month : DateTime.Now;
                switch (retention.TypeId)
                {
                    case 1:
                        rbRetention.Checked = true;
                        break;
                    case 2:
                        rbProfit.Checked = true;
                        break;
                    case 3:
                        rbPay.Checked = true;
                        break;
                }
                if (retention.BaseId != null)
                    cbBase.SelectedValue = retention.BaseId;
                else
                {
                    cbBase.Text = retention.Name + " Кол:" + retention.Count + " Час:" + retention.Hours + " Ст:" + retention.Wage;
                    cbBase.Enabled = false;
                }
                tbValue.Text = retention.Value.ToString();
            }
            else
            {
                dtpDate.Value = dtpMonth.Value = DateTime.Now;
                rbRetention.Checked = true;
                tbValue.Text = "0";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbTeachers.SelectedIndex == -1 || tbValue.Text == "0" || String.IsNullOrEmpty(cbBase.Text))
            {
                MessageBox.Show("Не все поля формы заполнены", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //проверка основания и сохранение его, если оно отсутствует
            string @base = cbBase.Text;
            var baseDb = db.Bases.FirstOrDefault(a => a.Name == @base);
            if (baseDb == null)
            {
                baseDb = new Base();
                baseDb.Name = cbBase.Text;
                db.Bases.Add(baseDb);
                db.SaveChanges();
            }

            //сохранение удержания/начисления/выплаты
            retention.Date = dtpDate.Value;
            retention.WorkerId = (int)cbTeachers.SelectedValue;
            if (rbRetention.Checked) retention.TypeId = 1;
            else if (rbProfit.Checked) retention.TypeId = 2;
            else retention.TypeId = 3;            
            retention.Value = Convert.ToDecimal(tbValue.Text);
            retention.Month = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
                        
            if (retention.Id == 0)
            {
                retention.BaseId = baseDb.Id;
                db.Retentions.Add(retention);
                db.SaveChanges();
            }
            else
            {
                //пересчет ставки, если сумма меняется в начислениях за занятия
                if (retention.Name != null)
                {                    
                    retention.Wage = retention.Value / (int)retention.Count;
                }
                else
                    retention.BaseId = baseDb.Id;

                db.Entry(retention).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
