using Istra.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Istra
{
    public partial class DiscountForm : Form
    {
        IstraContext db = new IstraContext();
        public Enrollment enroll;

        public DiscountForm(int idEnroll)
        {
            InitializeComponent();
            try
            {
                enroll = db.Enrollments.Find(idEnroll);

                //заполнение привилегий
                var privileges = db.Privileges.ToList();
                cbPrivilege.DataSource = privileges;
                cbPrivilege.DisplayMember = "Name";
                cbPrivilege.ValueMember = "Id";
                //автопоиск льгот           
                AutoCompleteStringCollection privilegeList = new AutoCompleteStringCollection();
                for (int i = 0; i < privileges.Count; i++)
                {
                    privilegeList.Add(privileges[i].Name);
                }
                cbPrivilege.AutoCompleteSource = AutoCompleteSource.CustomSource;
                cbPrivilege.AutoCompleteCustomSource = privilegeList;
                cbPrivilege.AutoCompleteMode = AutoCompleteMode.SuggestAppend;


            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DataGridViewCheckBoxColumn c = new DataGridViewCheckBoxColumn();
            c.Name = "Check";
            c.HeaderText = "Выбрать";

            dgvSchedules.Columns.Add("Id", "Id");
            dgvSchedules.Columns.Add(c);
            dgvSchedules.Columns.Add("Date", "Дата");
            dgvSchedules.Columns.Add("Val", "Платеж");
            dgvSchedules.Columns.Add("Discount", "Скидка");
            dgvSchedules.Columns.Add("Note", "Основание");
            dgvSchedules.Columns["Id"].Visible = false;
            dgvSchedules.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvSchedules.Columns["Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvSchedules.Columns["Val"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvSchedules.Columns["Discount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

        }

        private void DiscountForm_Load(object sender, EventArgs e)
        {
            try
            {
                //проверяем есть ли индивидуальной график платежей
                var listSchedule = (from scheds in db.Schedules
                                    where scheds.EnrollmentId == enroll.Id && scheds.Source == 2
                                    orderby scheds.DateBegin
                                    select new ListSchedules
                                    {
                                        Id = scheds.Id,
                                        Discount = scheds.Discount,
                                        Date = scheds.DateBegin,
                                        Val = scheds.Value,
                                        Note = scheds.Note
                                    }).ToList();
                //если индивидуального нет, то загружаем график группы
                if (listSchedule.Count == 0)
                {
                    listSchedule = (from scheds in db.Schedules
                                    where scheds.GroupId == enroll.GroupId && scheds.Source == 2 && scheds.EnrollmentId == null
                                    orderby scheds.DateBegin
                                    select new ListSchedules
                                    {
                                        Id = scheds.Id,
                                        Discount = scheds.Discount,
                                        Date = scheds.DateBegin,
                                        Val = scheds.Value,
                                        Note = scheds.Note
                                    }).ToList();
                }

                double summPay = 0, summDiscount = 0;
                if (listSchedule.Count > 0)
                {
                    foreach (var sch in listSchedule)
                    {
                        dgvSchedules.Rows.Add(sch.Id, false, sch.Date, sch.Val, sch.Discount, sch.Note);
                        summPay += sch.Val;
                        summDiscount += sch.Discount;
                    }
                    dgvSchedules.Columns["Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvSchedules.Columns["Date"].ReadOnly = true;
                    dgvSchedules.Columns["Check"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvSchedules.Columns["Val"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvSchedules.Columns["Val"].ReadOnly = true;
                    dgvSchedules.Columns["Discount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvSchedules.Columns["Note"].ReadOnly = true;

                    label2.Text = summPay.ToString("C", CultureInfo.CurrentCulture);
                    label3.Text = summDiscount.ToString("C", CultureInfo.CurrentCulture);

                    
                }

                if (enroll.PrivilegeId != null)
                    cbPrivilege.SelectedValue = enroll.PrivilegeId;
                else
                    cbPrivilege.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btCalculate_Click(object sender, EventArgs e)
        {
            double summDiscount = 0;
            if (tbDiscount.Text != "")
            {
                if (rbCurrency.Checked)
                {
                    for (int i = 0; i < dgvSchedules.RowCount; i++)
                    {
                        if (Convert.ToBoolean(dgvSchedules.Rows[i].Cells["Check"].Value) == true)
                        {
                            dgvSchedules.Rows[i].Cells["Discount"].Value = tbDiscount.Text;
                            dgvSchedules.Rows[i].Cells["Note"].Value = cbPrivilege.Text;
                        }                    
                        summDiscount += Convert.ToDouble(dgvSchedules.Rows[i].Cells["Discount"].Value);
                    }
                }
                else
                {
                    for (int i = 0; i < dgvSchedules.RowCount; i++)
                    {
                        if (Convert.ToBoolean(dgvSchedules.Rows[i].Cells["Check"].Value) == true)
                        {
                            double percent = Convert.ToDouble(tbDiscount.Text);
                            int val = 0;
                            if (percent != 0)
                                val = Convert.ToInt32(Convert.ToDouble(dgvSchedules.Rows[i].Cells["Val"].Value) * percent / 100);
                            dgvSchedules.Rows[i].Cells["Discount"].Value = val;
                            dgvSchedules.Rows[i].Cells["Note"].Value = cbPrivilege.Text;
                        }
                        summDiscount += Convert.ToDouble(dgvSchedules.Rows[i].Cells["Discount"].Value);
                    }
                }
                tbDiscount.Text = "";
            }
            label3.Text = summDiscount.ToString("C", CultureInfo.CurrentCulture);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chbCheckAll.Checked)
                for (int i = 0; i < dgvSchedules.RowCount; i++)
                    dgvSchedules.Rows[i].Cells["Check"].Value = true;
            else
                for (int i = 0; i < dgvSchedules.RowCount; i++)
                    dgvSchedules.Rows[i].Cells["Check"].Value = false;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvSchedules.RowCount; i++)
            {
                if (Convert.ToBoolean(dgvSchedules.Rows[i].Cells["Check"].Value) == true)
                {
                    dgvSchedules.Rows[i].Cells["Discount"].Value = 0;
                    dgvSchedules.Rows[i].Cells["Note"].Value = "";
                }
            }
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                //сохранение скидки
                if (ValidationDiscount() && cbPrivilege.Text == "")
                    MessageBox.Show("Не выбрано основание для предоставления скидки!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    //сохранение основания предоставления скидки
                    Privilege currentPriviledge = new Privilege();
                    if (cbPrivilege.SelectedIndex == -1 && cbPrivilege.Text != String.Empty)
                    {
                        currentPriviledge.Name = cbPrivilege.Text;
                        db.Privileges.Add(currentPriviledge);
                        db.SaveChanges();
                        enroll.PrivilegeId = currentPriviledge.Id;
                    }
                    else enroll.PrivilegeId = Convert.ToInt32(cbPrivilege.SelectedValue);

                    //сохранение начисленной скидки
                    List<Schedule> list = new List<Schedule>();
                    list = db.Schedules.Where(a => a.EnrollmentId == enroll.Id && a.Source == 2).
                        OrderBy(a => a.DateBegin).ToList();
                    //если есть индивидуaльный график платежей, то сохраняем в него
                    if (list.Count > 0)
                    {
                        for (int i = 0; i < dgvSchedules.RowCount; i++)
                        {
                            list[i].Discount = Convert.ToDouble(dgvSchedules.Rows[i].Cells["Discount"].Value);
                            list[i].Note = (dgvSchedules.Rows[i].Cells["Note"].Value != null)?dgvSchedules.Rows[i].Cells["Note"].Value.ToString():null;
                            db.Entry(list[i]).State = System.Data.Entity.EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        for (int i = 0; i < dgvSchedules.RowCount; i++)
                        {
                            list.Add(new Schedule
                            {
                                Discount = Convert.ToDouble(dgvSchedules.Rows[i].Cells["Discount"].Value),
                                Value = Convert.ToDouble(dgvSchedules.Rows[i].Cells["Val"].Value),
                                Source = 2,
                                DateBegin = Convert.ToDateTime(dgvSchedules.Rows[i].Cells["Date"].Value),
                                EnrollmentId = enroll.Id,
                                WorkerId = CurrentSession.CurrentUser.Id,
                                GroupId = enroll.GroupId,
                                Note = (dgvSchedules.Rows[i].Cells["Note"].Value != null) ? dgvSchedules.Rows[i].Cells["Note"].Value.ToString() : null
                        });
                        }
                        db.Schedules.AddRange(list);
                        db.SaveChanges();
                    }
                    Close();
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

        private bool ValidationDiscount()
        {
            bool result = false;
            for (int i = 0; i < dgvSchedules.RowCount; i++)
            {
                if (dgvSchedules.Rows[i].Cells["Discount"].Value.ToString() != "0")
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
