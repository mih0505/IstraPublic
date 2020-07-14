using Istra.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Istra.Documents;
using System.Globalization;

namespace Istra
{
    public partial class FinancesStudentsForm : Form
    {
        IstraContext db = new IstraContext();
        
        IQueryable<Finance> listStudents;
        string sortColumn;
        SortOrder sortOrder;
        
        public FinancesStudentsForm()
        {
            InitializeComponent();
        }

        public void StudentsForm_Load(object sender, EventArgs e)
        {
            try
            {
                //загрузка фильтра групп
                var actGroups = db.Groups.Where(a => a.Activity.Name != "Закрытые" && a.Individual == false).OrderBy(a => a.Name).ToList();
                cbGroups.DataSource = actGroups;
                cbGroups.DisplayMember = "Name";
                cbGroups.ValueMember = "Id";
                cbGroups.SelectedIndex = -1;

                //загрузка фильтра оснований скидок
                var privileges = db.Privileges.ToList();
                cbPrivileges.DataSource = privileges;
                cbPrivileges.DisplayMember = "Name";
                cbPrivileges.ValueMember = "Id";
                cbPrivileges.SelectedIndex = -1;

                //загрузка фильтра тип обучения 
                var type = db.Years.Where(a => a.IsRemoved == false).OrderBy(a => a.SortIndex).ToList();
                cbTypeGroup.DataSource = type;
                cbTypeGroup.DisplayMember = "Name";
                cbTypeGroup.ValueMember = "Id";
                cbTypeGroup.SelectedIndex = -1;

                //заполнение статусов групп
                cbActivityGroup.DataSource = db.ActivityGroups.Where(a => a.IsRemoved == false).ToList();
                cbActivityGroup.DisplayMember = "Name";
                cbActivityGroup.ValueMember = "Id";

                //выбор активного статуса
                var currentActivity = db.ActivityGroups.FirstOrDefault(a => a.Name == "Активные");
                cbActivityGroup.SelectedValue = currentActivity.Id;

                //установка дат
                var date = dtpStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                date = date.AddMonths(1).AddDays(-1);
                dtpEnd.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, date.Day);

                //добавление программной сортировки
                foreach (DataGridViewColumn col in dgvStudents.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.Programmatic;
                }

                //загрузка списка слушателей
                Filter(null, null, null, cbActivityGroup.Text, null, dtpStart.Value, dtpEnd.Value, sortColumn, sortOrder);

                //оформление таблицы
                dgvStudents.Columns["StudentId"].Visible = dgvStudents.Columns["GroupId"].Visible =
                    dgvStudents.Columns["SchoolId"].Visible = dgvStudents.Columns["EnrollId"].Visible =
                    dgvStudents.Columns["StatusId"].Visible = dgvStudents.Columns["ActivityId"].Visible =                    
                    dgvStudents.Columns["Activity"].Visible = dgvStudents.Columns["YearId"].Visible =
                    dgvStudents.Columns["PrivilegeId"].Visible = false;

                dgvStudents.Columns["Lastname"].HeaderText = "Фамилия";
                dgvStudents.Columns["Firstname"].HeaderText = "Имя";
                dgvStudents.Columns["Middlename"].HeaderText = "Отчество";
                dgvStudents.Columns["Status"].HeaderText = "Статус";
                dgvStudents.Columns["NameGroup"].HeaderText = "Группа";
                dgvStudents.Columns["Privilege"].HeaderText = "Льгота";                          
                dgvStudents.Columns["School"].HeaderText = "Школа";
                dgvStudents.Columns["Class"].HeaderText = "Класс";
                dgvStudents.Columns["Payment"].HeaderText = "Платежи";
                dgvStudents.Columns["AccrualDiscount"].HeaderText = "Начислено со скидкой";
                dgvStudents.Columns["Sex"].HeaderText = "Пол";
                dgvStudents.Columns["Accrual"].HeaderText = "Начислено";
                dgvStudents.Columns["Saldo"].HeaderText = "Остаток";
                dgvStudents.Columns["Year"].HeaderText = "Форма\\Год";

                dgvStudents.Columns["Sex"].Width = dgvStudents.Columns["Class"].Width = 50;
                dgvStudents.Columns["School"].Width = 80;

            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            cbGroups.SelectedIndex = cbPrivileges.SelectedIndex = cbTypeGroup.SelectedIndex = -1;
            tbLastname.Text = String.Empty;

            Filter(null, null, null, cbActivityGroup.Text, null, dtpStart.Value, dtpEnd.Value, sortColumn, sortOrder);
        }

        public void Filter(int? idGroup, int? idYear, string lastname, string activeGroups, int? privilege, DateTime dtStart, DateTime dtEnd, string column, SortOrder? sortOrder)
        {
            try
            {
                listStudents = from enroll in db.Enrollments
                                join privileges in db.Privileges on enroll.PrivilegeId equals privileges.Id into g
                                from privileges in g.DefaultIfEmpty()
                                join groups in db.Groups on enroll.GroupId equals groups.Id
                                join activity in db.ActivityGroups on groups.ActivityId equals activity.Id
                                join students in db.Students on enroll.StudentId equals students.Id
                                join statuses in db.Statuses on students.StatusId equals statuses.Id
                                join years in db.Years on groups.YearId equals years.Id
                                where enroll.DateExclusion == null && enroll.ExclusionId == null
                                select new Finance
                                {
                                    StudentId = students.Id,
                                    GroupId = groups.Id,
                                    SchoolId = students.SchoolId,
                                    EnrollId = enroll.Id,
                                    PrivilegeId = enroll.PrivilegeId,
                                    ActivityId = activity.Id,
                                    YearId = years.Id,
                                    Activity = activity.Name,
                                    StatusId = students.StatusId,
                                    Lastname = students.Lastname,
                                    Firstname = students.Firstname,
                                    Middlename = students.Middlename,
                                    Sex = students.Sex,
                                    NameGroup = groups.Name,
                                    Year = years.Name,
                                    Status = statuses.Name,
                                    School = students.School.Name,
                                    Class = students.Class,
                                    Privilege = privileges.Name,
                                    Accrual = (from scheds in db.Schedules
                                               where scheds.Source == 2 && scheds.EnrollmentId == enroll.Id && scheds.DateBegin >= dtpStart.Value && scheds.DateBegin <= dtpEnd.Value
                                               select new { SchedsSum = scheds.Value }).Sum(s => (double?)(s.SchedsSum)) ?? 0,
                                    AccrualDiscount = (from scheds in db.Schedules
                                                       where scheds.Source == 2 && scheds.EnrollmentId == enroll.Id && scheds.DateBegin >= dtpStart.Value && scheds.DateBegin <= dtpEnd.Value
                                                       select new { SchedSum = scheds.Value - scheds.Discount }).Sum(s => (double?)(s.SchedSum)) ?? 0,
                                    Payment = (from pays in db.Payments
                                               where pays.EnrollmentId == enroll.Id && pays.DatePayment >= dtpStart.Value 
                                               && pays.DatePayment <= dtpEnd.Value && pays.IsDeleted == false && pays.AdditionalPay == false
                                               select new { PaySum = pays.ValuePayment }).Sum(s => (double?)(s.PaySum)) ?? 0,

                                };

                if (idGroup != null) listStudents = listStudents.Where(d => d.GroupId == idGroup);
                if (lastname != null && lastname != "") listStudents = listStudents.Where(e => e.Lastname.StartsWith(lastname));

                if (privilege != null) listStudents = listStudents.Where(e => e.PrivilegeId == privilege);

                if (idYear != null) listStudents = listStudents.Where(e => e.YearId == idYear);

                if (activeGroups == "Активные")
                    listStudents = listStudents.Where(a => a.Activity != "Закрытые");
                else listStudents = listStudents.Where(a => a.Activity == activeGroups);

                //сортировка
                if (column != null)
                {
                    switch (column)
                    {
                        case "Lastname":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.Lastname);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.Lastname);
                                }
                                break;
                            }
                        case "Firstname":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.Firstname);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.Firstname);
                                }
                                break;
                            }
                        case "Middlename":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.Middlename);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.Middlename);
                                }
                                break;
                            }
                        case "Status":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.Status);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.Status);
                                }
                                break;
                            }
                        case "NameGroup":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.NameGroup);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.NameGroup);
                                }
                                break;
                            }
                        case "Privilege":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.Privilege);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.Privilege);
                                }
                                break;
                            }                        
                        case "School":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.School);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.School);
                                }
                                break;
                            }
                        case "Class":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.Class);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.Class);
                                }
                                break;
                            }
                        case "Sex":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.Sex);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.Sex);
                                }
                                break;
                            }
                        case "Payment":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.Payment);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.Payment);
                                }
                                break;
                            }
                        case "AccrualDiscount":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.AccrualDiscount);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.AccrualDiscount);
                                }
                                break;
                            }
                        case "Accrual":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.Accrual);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.Accrual);
                                }
                                break;
                            }
                    }
                }
                else
                    listStudents = listStudents.OrderBy(f => f.Lastname);
                
                dgvStudents.DataSource = listStudents.ToList();
                ///////////////////////расчет итогов/////////////////////////////
                //количество групп
                toolStripStatusLabel2.Text = dgvStudents.Rows.Count.ToString();
                double summScheds = 0;
                double summPays = 0;
                double summDebt = 0;
                double summPrepayment = 0;
                foreach (DataGridViewRow row in dgvStudents.Rows)
                {
                    //итого начислено со скидкой
                    summScheds += (double)row.Cells["AccrualDiscount"].Value;
                    //итого платежей
                    summPays += (double)row.Cells["Payment"].Value;
                    //итого долг
                    summDebt += ((double)row.Cells["Saldo"].Value > 0) ? (double)row.Cells["Saldo"].Value : 0;
                    //итого аванс
                    summPrepayment += ((double)row.Cells["Saldo"].Value < 0) ? (double)row.Cells["Saldo"].Value : 0;
                }
                
                if (sortOrder != SortOrder.None)
                    dgvStudents.Columns[sortColumn].HeaderCell.SortGlyphDirection = (SortOrder)sortOrder;

                toolStripStatusLabel5.Text = summScheds.ToString("C", CultureInfo.CurrentCulture);
                toolStripStatusLabel8.Text = summPays.ToString("C", CultureInfo.CurrentCulture);
                toolStripStatusLabel9.Text = summDebt.ToString("C", CultureInfo.CurrentCulture);
                toolStripStatusLabel11.Text = (-summPrepayment).ToString("C", CultureInfo.CurrentCulture);
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
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, cbActivityGroup.Text, (int?)cbPrivileges.SelectedValue, dtpStart.Value, dtpEnd.Value, sortColumn, sortOrder);
        }

        private void cbGroups_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, cbActivityGroup.Text, (int?)cbPrivileges.SelectedValue, dtpStart.Value, dtpEnd.Value, sortColumn, sortOrder);
        }

        private void dgvStudents_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int currentRow = e.RowIndex;
            if (currentRow != -1)
            {
                var idStudent = Convert.ToInt32(dgvStudents.Rows[currentRow].Cells["StudentId"].Value);
                var studentForm = new StudentForm(idStudent, true);
                studentForm.MdiParent = this.MdiParent;
                studentForm.Show();

                Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, cbActivityGroup.Text, (int?)cbPrivileges.SelectedValue, dtpStart.Value, dtpEnd.Value, sortColumn, sortOrder);

                if (currentRow != dgvStudents.Rows.Count)
                {
                    if (dgvStudents.Rows.Count != 0)
                    {
                        dgvStudents.Rows[currentRow].Selected = true;
                        dgvStudents.CurrentCell = dgvStudents.Rows[currentRow].Cells["Lastname"];
                    }
                }
                else
                {
                    dgvStudents.Rows[dgvStudents.Rows.Count - 1].Selected = true;
                    dgvStudents.CurrentCell = dgvStudents.Rows[currentRow].Cells["Lastname"];
                }
            }
        }

        private void подробныйСписокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvStudents.Rows.Count != 0)
            {
                try
                {
                    var total = (from enroll in db.Enrollments
                                 join groups in db.Groups on enroll.GroupId equals groups.Id
                                 join activity in db.ActivityGroups on groups.ActivityId equals activity.Id
                                 join students in db.Students on enroll.StudentId equals students.Id
                                 join years in db.Years on groups.YearId equals years.Id into g
                                 from years in g.DefaultIfEmpty()
                                 where (activity.Name == "Текущие" || activity.Name == "В наборе") && enroll.DateExclusion == null
                                 select new ListStudentFull
                                 {
                                     EnrollmentId = enroll.Id,
                                     YearId = years.Id,
                                     Lastname = students.Lastname,
                                     Firstname = students.Firstname,
                                     Middlename = students.Middlename,
                                     DateOfBirth = students.DateOfBirth,
                                     Age = DateTime.Now.Year - students.DateOfBirth.Year,
                                     Sex = students.Sex,
                                     City = students.City.Name,
                                     StatusStudent = students.Status.Name,
                                     School = students.School.Name,
                                     Class = students.Class,
                                     Shift = students.Shift,
                                     PhoneNumber1 = students.StudentPhone,
                                     PhoneNumber2 = students.StudentPhone2,
                                     LastnameParent = students.LastnameParent,
                                     FirstnameParent = students.FirstnameParent,
                                     MiddlenameParent = students.MiddlenameParent,
                                     PhoneNumberParent = students.ParentsPhone,
                                     Group = groups.Name,
                                     Year = years.Name,
                                     StatusGroup = activity.Name,
                                     SchedSumNow = (from scheds in db.Schedules
                                                    where scheds.Source == 2 && scheds.EnrollmentId == enroll.Id && scheds.DateBegin <= DateTime.Now
                                                    select new { SchedSum = scheds.Value - scheds.Discount }).Sum(s => (double?)(s.SchedSum)) ?? 0,
                                     PaysSumNow = (from pays in db.Payments
                                                   where pays.EnrollmentId == enroll.Id && pays.IsDeleted == false && pays.AdditionalPay == false
                                                   select new { PaySum = pays.ValuePayment }).Sum(s => (double?)(s.PaySum)) ?? 0,
                                     //SaldoNow = String.Empty,

                                 }).ToList();
                    var exportStudentFull = new Report();
                    exportStudentFull.ExportExcelStudentFull(total);
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

        private void dgvStudents_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            sortOrder = SortOrder.None;
            sortColumn = grid.Columns[e.ColumnIndex].Name;
            if (grid.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.None ||
                grid.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Ascending)
            {
                sortOrder = SortOrder.Descending;
            }
            else
            {
                sortOrder = SortOrder.Ascending;
            }
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, cbActivityGroup.Text, (int?)cbPrivileges.SelectedValue, dtpStart.Value, dtpEnd.Value, sortColumn, sortOrder);
            grid.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
        }

        private void cbPrivileges_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, cbActivityGroup.Text, (int?)cbPrivileges.SelectedValue, dtpStart.Value, dtpEnd.Value, sortColumn, sortOrder);
        }

        private void cbActivityGroup_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //загрузка фильтра групп
            cbGroups.DataSource = null;
            List<Group> actGroups;
            if (cbActivityGroup.Text != "Активные")
                actGroups = db.Groups.Where(a => a.Activity.Name == cbActivityGroup.Text).ToList();
            else
                actGroups = db.Groups.Where(a => a.Activity.Name != "Закрытые").ToList();

            cbGroups.DataSource = actGroups;
            cbGroups.DisplayMember = "Name";
            cbGroups.ValueMember = "Id";
            cbGroups.SelectedIndex = -1;

            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, cbActivityGroup.Text, (int?)cbPrivileges.SelectedValue, dtpStart.Value, dtpEnd.Value, sortColumn, sortOrder);
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, cbActivityGroup.Text, (int?)cbPrivileges.SelectedValue, dtpStart.Value, dtpEnd.Value, sortColumn, sortOrder);
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, cbActivityGroup.Text, (int?)cbPrivileges.SelectedValue, dtpStart.Value, dtpEnd.Value, sortColumn, sortOrder);
        }

        private void cbTypeGroup_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, cbActivityGroup.Text, (int?)cbPrivileges.SelectedValue, dtpStart.Value, dtpEnd.Value, sortColumn, sortOrder);
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            if (dgvStudents.Rows.Count != 0)
            {
                var exportStudent = new Report();
                exportStudent.ExportExcelStudent(dgvStudents, true);
            }
        }
    }
}