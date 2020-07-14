using Istra.Entities;
using System;
using System.Data.Entity;
using System.Windows.Forms;

namespace Istra
{
    public partial class GeneratePayForm : Form
    {
        public bool addPeriod;
        public bool editPeriod;
        IstraContext db = new IstraContext();
        public Schedule period;
        public GeneratePayForm(Schedule p, DateTime? dt)
        {
            InitializeComponent();
            if (p != null)
            {
                period = p;
                btSave.Text = "Сохранить";
                editPeriod = true;
                dtpBegin.Value = p.DateBegin;
                dtpEnd.Value = Convert.ToDateTime(p.DateEnd);
                tbPay.Text = period.Value.ToString();
            }
            else
            {
                addPeriod = true;
                period = new Schedule();
                if (dt != null)
                {
                    dtpBegin.Value = dt.Value.AddDays(1);
                    dtpEnd.Value = dt.Value.AddMonths(1);
                }
                else
                {
                    dtpBegin.Value = DateTime.Now;
                    dtpEnd.Value = DateTime.Now.AddMonths(1);
                }
                btSave.Text = "Добавить";
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (DateTime.Compare(dtpBegin.Value, dtpEnd.Value) > 0)
            {
                MessageBox.Show(this, "Выбранная начальная дата превышает значение конечной даты", "Ошибка выбора начальной и конечной даты", MessageBoxButtons.OK, MessageBoxIcon.Error);

                dtpBegin.Focus();

                return;
            }

            period.DateBegin = dtpBegin.Value;
            period.DateEnd = dtpEnd.Value;
            period.Value = Convert.ToDouble(tbPay.Text);
            period.Source = 1;
            period.GroupId = CurrentSession.GroupId;

            try
            {
                if (addPeriod)
                {
                    db.Schedules.Add(period);
                    db.SaveChanges();
                }
                if (editPeriod)
                {
                    db.Entry(period).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GeneratePayForm_Load(object sender, EventArgs e)
        {

        }
    }
}
