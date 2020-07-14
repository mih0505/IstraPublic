using Istra.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace Istra
{
    public partial class PayForm : Form
    {
        public bool add = false;
        public int idEnroll, idWorker;
        IstraContext db = new IstraContext();
        public Payment currentPay;
        public PayForm(int idEnroll, int idWorker, int? idPayment)
        {
            InitializeComponent();
            try
            {
                var months = db.Months.ToList();
                cbMonth.DataSource = months;
                cbMonth.DisplayMember = "Name";
                cbMonth.ValueMember = "Id";

                var types = db.TypePayments.ToList();
                cbTypePayment.DataSource = types;
                cbTypePayment.DisplayMember = "Name";
                cbTypePayment.ValueMember = "Id";

                //основные платежи
                if (idPayment == null)
                {
                    add = true;
                    currentPay = new Payment();
                    currentPay.EnrollmentId = idEnroll;
                    currentPay.WorkerId = idWorker;

                    //определение текущего месяца и года
                    int currentMonth = DateTime.Now.Month;
                    cbMonth.SelectedValue = currentMonth;
                    nudYear.Value = DateTime.Now.Year;
                    dtpDatePay.Value = DateTime.Now;
                }
                else
                {
                    currentPay = db.Payments.FirstOrDefault(a => a.Id == idPayment);
                    tbPay.Text = currentPay.ValuePayment.ToString();
                    dtpDatePay.Value = currentPay.DatePayment;
                    if (!currentPay.AdditionalPay)
                    {
                        cbMonth.SelectedValue = currentPay.MonthId;
                        nudYear.Value = currentPay.Year;
                    }
                    else
                    {
                        cbMonth.Enabled = false;
                        nudYear.Enabled = false;
                    }
                    cbTypePayment.SelectedValue = currentPay.TypePaymentId;
                    tbNotePay.Text = currentPay.Note;
                    chbAdditionalPay.Checked = currentPay.AdditionalPay;

                }                
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PayForm_Load(object sender, EventArgs e)
        {
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbPay.Text != "")
                {
                    currentPay.ValuePayment = Convert.ToDouble(tbPay.Text);
                    currentPay.DatePayment = dtpDatePay.Value;
                    currentPay.WorkerId = CurrentSession.CurrentUser.Id;
                    if (!chbAdditionalPay.Checked)
                    {
                        currentPay.MonthId = Convert.ToInt32(cbMonth.SelectedValue);
                        currentPay.Year = (int)nudYear.Value;
                    }
                    else
                    {
                        currentPay.MonthId = null;
                    }
                    currentPay.TypePaymentId = Convert.ToInt32(cbTypePayment.SelectedValue);
                    currentPay.Note = tbNotePay.Text;
                    currentPay.AdditionalPay = chbAdditionalPay.Checked;
                    
                    if (add)
                    {
                        currentPay.HousingId = CurrentSession.CurrentHousing.Id;
                        db.Payments.Add(currentPay);
                    }
                    else
                    {
                        db.Entry(currentPay).State = EntityState.Modified;
                    }                  
                    db.SaveChanges();

                    //сохрание доп платежей в виде строки
                    if (chbAdditionalPay.Checked)
                    {
                        var enroll = db.Enrollments.Find(currentPay.EnrollmentId);
                        if (enroll != null)
                        {
                            enroll.AdditionalPays = "";
                            var addPays = db.Payments.Where(a => a.AdditionalPay == true && a.EnrollmentId == enroll.Id && a.IsDeleted == false).ToList();
                            foreach (var pay in addPays)
                            {
                                enroll.AdditionalPays += pay.ValuePayment.ToString() + " ";
                            }
                        }
                        db.Entry(enroll).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    Close();
                }
                else
                    MessageBox.Show("Введите сумму платежа", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
