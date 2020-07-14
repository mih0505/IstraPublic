using Istra.Documents;
using Istra.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Istra
{
    public partial class ListReturnPaysForm : Form
    {
        private bool isPickedDateBegin; // Выбрана ли начальная дата?
        private bool isPickedDateEnd; // Выбрана ли конечная дата?

        public ListReturnPaysForm()
        {
            InitializeComponent();
        }

        IstraContext db = new IstraContext();
        IQueryable<ListPays> payments;

        public IQueryable<ListPays> GetPaymentsList()
        {
            return from payment in db.Payments
                   join enrollment in db.Enrollments on payment.EnrollmentId equals enrollment.Id
                   join groups in db.Groups on enrollment.GroupId equals groups.Id
                   join student in db.Students on enrollment.StudentId equals student.Id
                   join worker in db.Workers on payment.WorkerId equals worker.Id
                   where payment.ValuePayment < 0 && payment.IsDeleted == false && payment.AdditionalPay == false
                   orderby payment.DatePayment
                   select new ListPays
                   {
                       IsSelected = false,
                       PaymentId = payment.Id,
                       EnrollmentId = enrollment.Id,
                       WorkerId = worker.Id,
                       GroupId = groups.Id,
                       StudentId = student.Id,
                       DatePayment = payment.DatePayment,
                       ValuePayment = payment.ValuePayment,
                       GroupName = groups.Name,
                       StudentLastname = student.Lastname,
                       StudentFirstname = student.Firstname,
                       StudentMiddlename = student.Middlename,
                       WorkerLastnameFM = worker.Lastname + " " + worker.Firstname.Substring(0, 1) + "." + worker.Middlename.Substring(0, 1) + ".",
                       Note = payment.Note
                   };
        }

        private void Filter(string lastname, string group, DateTime dateBegin, DateTime dateEnd, int? workerId)
        {
            try
            {
                payments = GetPaymentsList();

                if (lastname != String.Empty)
                    payments = payments.Where(Payment => (Payment.StudentLastname.Contains(lastname)));

                if (group != String.Empty)
                    payments = payments.Where(Payment => (Payment.GroupName.Contains(group)));

                if (isPickedDateBegin && isPickedDateEnd)
                {
                    dateBegin = dateBegin.Date;
                    dateEnd = dateEnd.Date;

                    payments = payments.Where(Payment =>
                    ((DateTime.Compare(dateBegin, Payment.DatePayment) <= 0) && (DateTime.Compare(dateEnd, Payment.DatePayment) >= 0)));
                }

                if (workerId != null)
                    payments = payments.Where(Payment => (workerId == Payment.WorkerId));

                dgvPayments.DataSource = payments.ToList();
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListPaymentsForm_Load(object sender, EventArgs e)
        {
            try
            {
                dtpBegin.Value = dtpEnd.Value = DateTime.Now;
                payments = GetPaymentsList();

                dgvPayments.DataSource = payments.ToList();

                dgvPayments.Columns["DatePayment"].ReadOnly = dgvPayments.Columns["ValuePayment"].ReadOnly = dgvPayments.Columns["GroupName"].ReadOnly =
                    dgvPayments.Columns["StudentLastname"].ReadOnly = dgvPayments.Columns["StudentFirstname"].ReadOnly =
                    dgvPayments.Columns["StudentMiddlename"].ReadOnly = dgvPayments.Columns["WorkerLastnameFM"].ReadOnly = true;

                dgvPayments.Columns["PaymentId"].Visible = dgvPayments.Columns["EnrollmentId"].Visible = dgvPayments.Columns["WorkerId"].Visible =
                    dgvPayments.Columns["GroupId"].Visible = dgvPayments.Columns["StudentId"].Visible = false;

                dgvPayments.Columns["IsSelected"].HeaderText = "Выбрано";
                dgvPayments.Columns["IsSelected"].Width = 60;

                dgvPayments.Columns["DatePayment"].HeaderText = "Дата платежа";
                dgvPayments.Columns["DatePayment"].DefaultCellStyle.Format = "dd/MM/yyyy";

                dgvPayments.Columns["ValuePayment"].HeaderText = "Сумма возврата";

                dgvPayments.Columns["GroupName"].HeaderText = "Группа";

                dgvPayments.Columns["StudentLastname"].HeaderText = "Фамилия студента";

                dgvPayments.Columns["StudentFirstname"].HeaderText = "Имя студента";

                dgvPayments.Columns["StudentMiddlename"].HeaderText = "Отчество студента";

                dgvPayments.Columns["WorkerLastnameFM"].HeaderText = "Работник";

                dgvPayments.Columns["Housing"].HeaderText = "Корпус";

                dgvPayments.Columns["Note"].HeaderText = "Примечание";
                dgvPayments.Columns["Note"].Width = 360;

                cbWorkers.DataSource = db.Workers.Where(Worker => (Worker.RoleId == 1)).ToList();

                cbWorkers.DisplayMember = "LastnameFM";

                cbWorkers.ValueMember = "Id";

                cbWorkers.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btCleanFilter_Click(object sender, EventArgs e)
        {
            try
            {
                dtpBegin.Value = dtpEnd.Value = DateTime.Now;

                tbLastname.Clear();

                tbGroup.Clear();

                cbWorkers.SelectedIndex = -1;

                isPickedDateBegin = isPickedDateEnd = false;

                label2.Enabled = dtpEnd.Enabled = false;

                payments = GetPaymentsList();

                dgvPayments.DataSource = payments.ToList();
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbLastname_TextChanged(object sender, EventArgs e)
        {
            Filter(tbLastname.Text, tbGroup.Text, dtpBegin.Value, dtpEnd.Value, (int?)cbWorkers.SelectedValue);
        }

        private void tbGroup_TextChanged(object sender, EventArgs e)
        {
            Filter(tbLastname.Text, tbGroup.Text, dtpBegin.Value, dtpEnd.Value, (int?)cbWorkers.SelectedValue);
        }

        private void dtpBegin_ValueChanged(object sender, EventArgs e)
        {
            isPickedDateBegin = true;
            label2.Enabled = dtpEnd.Enabled = true;
            Filter(tbLastname.Text, tbGroup.Text, dtpBegin.Value, dtpEnd.Value, (int?)cbWorkers.SelectedValue);
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            isPickedDateEnd = true;
            Filter(tbLastname.Text, tbGroup.Text, dtpBegin.Value, dtpEnd.Value, (int?)cbWorkers.SelectedValue);
        }

        private void cbWorkers_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter(tbLastname.Text, tbGroup.Text, dtpBegin.Value, dtpEnd.Value, (int?)cbWorkers.SelectedValue);
        }

        private void btPrint_Click(object sender, EventArgs e)
        {
            var paymentsPrintForm = new PaymentsPrintForm();
            var dr = paymentsPrintForm.ShowDialog();

            if (dr == DialogResult.OK)
            {
                List<int> idPays = new List<int>();
                foreach (DataGridViewRow row in dgvPayments.Rows)
                    if ((bool)row.Cells["IsSelected"].Value == true)
                        idPays.Add((int)row.Cells["PaymentId"].Value);

                var listPays = db.Payments.Where(a => idPays.Contains(a.Id)).ToList();
                if (listPays.Count != 0)
                {
                    var receipt = new Report();
                    receipt.Receipts(listPays, CurrentSession.dateOrder, CurrentSession.namePaymaster, CurrentSession.enableDate);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chbSelecting.Checked)
            {
                foreach (DataGridViewRow row in dgvPayments.Rows)
                    row.Cells["IsSelected"].Value = true;
            }
            else
            {
                foreach (DataGridViewRow row in dgvPayments.Rows)
                    row.Cells["IsSelected"].Value = false;
            }
        }
                
        private void подробныйСписокToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var paymentsPrintForm = new PaymentsPrintForm();
            var dr = paymentsPrintForm.ShowDialog();

            if (dr == DialogResult.OK)
            {
                List<int> idPays = new List<int>();
                dgvPayments.EndEdit();//завершаем редактирование, т.к. не выбирается последняя строка при экспорте квитанций
                foreach (DataGridViewRow row in dgvPayments.Rows)
                    if ((bool)row.Cells["IsSelected"].Value == true)
                        idPays.Add((int)row.Cells["PaymentId"].Value);

                var listPays = db.Payments.Where(a => idPays.Contains(a.Id)).ToList();
                if (listPays.Count != 0)
                {
                    var receipt = new Report();
                    receipt.Receipts(listPays, CurrentSession.dateOrder, CurrentSession.namePaymaster, CurrentSession.enableDate);
                }
            }
        }

        private void краткийСписокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPayments.Rows.Count != 0)
            {
                var exportPayments = new Report();
                exportPayments.ExportExcelPayments(dgvPayments);
            }
        }
    }
}
