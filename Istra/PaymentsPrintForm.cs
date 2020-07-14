using Istra.Entities;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Istra
{
    public partial class PaymentsPrintForm : Form
    {
        IstraContext db = new IstraContext();

        public PaymentsPrintForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CurrentSession.dateOrder = dtpDate.Value;
            CurrentSession.namePaymaster = cbPaymaster.Text;
            if (dtpDate.Enabled) CurrentSession.enableDate = true; else CurrentSession.enableDate = false;
            Close();
        }

        private void PaymentsPrintForm_Load(object sender, EventArgs e)
        {
            try
            {
                var paymasters = db.Workers.Where(Worker => (Worker.RoleId == 3)).OrderBy(Worker => (Worker.Lastname));

                cbPaymaster.DataSource = paymasters.ToList();
                cbPaymaster.DisplayMember = "LastnameFM";
                cbPaymaster.ValueMember = "Id";

                dtpDate.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbEnableDate.Checked)
                dtpDate.Enabled = true;
            else
                dtpDate.Enabled = false;
        }
    }
}
