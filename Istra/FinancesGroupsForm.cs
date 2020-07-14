using Istra.Documents;
using Istra.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Istra
{
    public partial class FinancesGroupsForm : Form
    {
        IstraContext db = new IstraContext();
        string sortColumn;
        SortOrder sortOrder;

        public FinancesGroupsForm()
        {
            InitializeComponent();
        }

        private void FinancesGroupsForm_Load(object sender, EventArgs e)
        {
            try
            {
                //загрузка фильтра направлений
                var direction = db.Directions.Where(a => a.IsRemoved == false).ToList();
                cbDirectionOfTraining.DataSource = direction;
                cbDirectionOfTraining.DisplayMember = "Name";
                cbDirectionOfTraining.ValueMember = "Id";
                cbDirectionOfTraining.SelectedIndex = -1;

                //загрузка фильтра тип обучения 
                var type = db.Years.Where(a => a.IsRemoved == false).OrderBy(a => a.SortIndex).ToList();
                cbTypeGroup.DataSource = type;
                cbTypeGroup.DisplayMember = "Name";
                cbTypeGroup.ValueMember = "Id";
                cbTypeGroup.SelectedIndex = -1;

                //загрузка статусов групп
                cbActivity.DataSource = db.ActivityGroups.Where(a => a.IsRemoved == false).ToList();
                cbActivity.DisplayMember = "Name";
                cbActivity.ValueMember = "Id";

                //выбор активного статуса
                var currentActivity = db.ActivityGroups.FirstOrDefault(a => a.Name == "Активные" && a.IsRemoved == false);
                cbActivity.SelectedValue = currentActivity.Id;

                //добавление программной сортировки
                foreach (DataGridViewColumn col in dgvListGroups.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.Programmatic;
                }

                //загрузка списка слушателей
                Filter(null, null, (int?)cbTypeGroup.SelectedValue, cbActivity.Text, sortColumn, dtpStart.Value, dtpEnd.Value, sortOrder);

                //установка дат
                var date = dtpStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                date = date.AddMonths(1).AddDays(-1);
                dtpEnd.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, date.Day);

                //оформление таблицы
                dgvListGroups.Columns["GroupId"].Visible = dgvListGroups.Columns["DirectionId"].Visible =
                    dgvListGroups.Columns["ActivityId"].Visible =
                    dgvListGroups.Columns["Activity"].Visible = false;

                dgvListGroups.Columns["Direction"].HeaderText = "Направление";
                dgvListGroups.Columns["Name"].HeaderText = "Группа";
                dgvListGroups.Columns["CourseName"].HeaderText = "Курс";
                dgvListGroups.Columns["Year"].HeaderText = "Уч. год";
                dgvListGroups.Columns["Price"].HeaderText = "Цена";
                dgvListGroups.Columns["StudentsP"].HeaderText = "Учащ. План";
                dgvListGroups.Columns["StudentsF"].HeaderText = "Учащ. Факт";
                dgvListGroups.Columns["Accrual"].HeaderText = "Начислено";
                dgvListGroups.Columns["AccrualDiscount"].HeaderText = "Начислено со скидкой";
                dgvListGroups.Columns["Payment"].HeaderText = "Платежи";
                dgvListGroups.Columns["PlanAccrual"].HeaderText = "План выручки";
                dgvListGroups.Columns["Saldo"].HeaderText = "Остаток";
                dgvListGroups.Focus(); dgvListGroups.Select();
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Filter(int? directionId, string name, int? type, string activeGroupsName, string column, DateTime dtStart, DateTime dtEnd, SortOrder? sortOrder)
        {
            try
            {
                var listGroups = from groups in db.Groups
                                 join courses in db.Courses on groups.CourseId equals courses.Id
                                 join directions in db.Directions on courses.DirectionId equals directions.Id
                                 join teachers in db.Workers on groups.TeacherId equals teachers.Id into outerTeacher
                                 from teachers in outerTeacher.DefaultIfEmpty()
                                 join activities in db.ActivityGroups on groups.ActivityId equals activities.Id
                                 join years in db.Years on groups.YearId equals years.Id into outer
                                 from years in outer.DefaultIfEmpty()
                                 select new FinanceGroups
                                 {
                                     GroupId = groups.Id,
                                     DirectionId = directions.Id,
                                     Direction = directions.Name,
                                     ActivityId = activities.Id,
                                     YearId = years.Id,
                                     CourseName = courses.Name,
                                     Name = groups.Name,
                                     Year = years.Name,
                                     StudentsP = groups.PlanEnroll,
                                     StudentsF = db.Enrollments.Where(a => a.GroupId == groups.Id && a.DateExclusion == null).Count(),
                                     Activity = activities.Name,
                                     Price = db.Schedules.Where(a => a.GroupId == groups.Id && a.Source == 2 && a.EnrollmentId == null && a.DateBegin >= dtpStart.Value.Date
                                       && a.DateBegin <= dtpEnd.Value.Date).Select(a => new { a.Value }).Sum(s => (double?)(s.Value)) ?? 0,
                                     Accrual = (from scheds in db.Schedules
                                                join enrolls in db.Enrollments on scheds.EnrollmentId equals enrolls.Id
                                                where scheds.Source == 2 && scheds.EnrollmentId != null && scheds.GroupId == groups.Id && enrolls.DateExclusion == null
                                                && scheds.DateBegin >= dtpStart.Value.Date && scheds.DateBegin <= dtpEnd.Value.Date
                                                select new { SchedSum = scheds.Value }).Sum(s => (double?)(s.SchedSum)) ?? 0,
                                     AccrualDiscount = (Double?)(from scheds in db.Schedules
                                                                 join enrolls in db.Enrollments on scheds.EnrollmentId equals enrolls.Id
                                                                 where scheds.Source == 2 && scheds.EnrollmentId != null && scheds.GroupId == groups.Id && enrolls.DateExclusion == null
                                                                 && scheds.DateBegin >= dtpStart.Value.Date && scheds.DateBegin <= dtpEnd.Value.Date
                                                                 select new { SchedSum = scheds.Value - scheds.Discount }).Sum(s => (double?)(s.SchedSum)) ?? 0,
                                     Payment = (Double?)(from pays in db.Payments
                                                         join enrolls in db.Enrollments on pays.EnrollmentId equals enrolls.Id
                                                         where enrolls.GroupId == groups.Id && enrolls.DateExclusion == null && pays.IsDeleted == false && pays.AdditionalPay == false
                                                         && pays.DatePayment >= dtpStart.Value && pays.DatePayment <= dtpEnd.Value
                                                         select new { PaySum = pays.ValuePayment }).Sum(s => (double?)(s.PaySum)) ?? 0,

                                 };

                if (directionId != null) listGroups = listGroups.Where(d => d.DirectionId == directionId);
                if (name != null && name != "") listGroups = listGroups.Where(e => e.Name.StartsWith(name));
                                
                if (type != null) listGroups = listGroups.Where(d => d.YearId == type);

                if (activeGroupsName == "Активные")
                    listGroups = listGroups.Where(a => a.Activity != "Закрытые");
                else listGroups = listGroups.Where(a => a.Activity == activeGroupsName);

                //сортировка                
                if (column != null)
                {
                    switch (column)
                    {
                        case "Direction":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listGroups = listGroups.OrderBy(x => x.Direction);
                                }
                                else
                                {
                                    listGroups = listGroups.OrderByDescending(x => x.Direction);
                                }
                                break;
                            }
                        case "Name":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listGroups = listGroups.OrderBy(x => x.Name);
                                }
                                else
                                {
                                    listGroups = listGroups.OrderByDescending(x => x.Name);
                                }
                                break;
                            }
                        case "Year":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listGroups = listGroups.OrderBy(x => x.Year);
                                }
                                else
                                {
                                    listGroups = listGroups.OrderByDescending(x => x.Year);
                                }
                                break;
                            }
                        case "StudentsF":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listGroups = listGroups.OrderBy(x => x.StudentsF);
                                }
                                else
                                {
                                    listGroups = listGroups.OrderByDescending(x => x.StudentsF);
                                }
                                break;
                            }
                    }
                }
                else
                    listGroups = listGroups.OrderBy(f => f.Direction);
                                
                dgvListGroups.DataSource = listGroups.ToList();

                ///////////////////////расчет итогов/////////////////////////////
                //количество групп
                toolStripStatusLabel2.Text = dgvListGroups.Rows.Count.ToString();
                double summScheds = 0;
                double summPays = 0;
                double summDebt = 0;
                double summPrepayment = 0;
                foreach (DataGridViewRow row in dgvListGroups.Rows)
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
                    dgvListGroups.Columns[sortColumn].HeaderCell.SortGlyphDirection = (SortOrder)sortOrder;

                toolStripStatusLabel4.Text = summScheds.ToString("C", CultureInfo.CurrentCulture);
                toolStripStatusLabel6.Text = summPays.ToString("C", CultureInfo.CurrentCulture);
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

        private void cbDirectionOfTraining_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, cbActivity.Text, sortColumn, dtpStart.Value, dtpEnd.Value, sortOrder);
        }

        private void tbNameGroup_TextChanged(object sender, EventArgs e)
        {
            Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, cbActivity.Text, sortColumn, dtpStart.Value, dtpEnd.Value, sortOrder);
        }

        private void cbTypeGroup_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, cbActivity.Text, sortColumn, dtpStart.Value, dtpEnd.Value, sortOrder);
        }

        private void chbActive_CheckedChanged(object sender, EventArgs e)
        {
            Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, cbActivity.Text, sortColumn, dtpStart.Value, dtpEnd.Value, sortOrder);
        }

        private void btCleanFilter_Click(object sender, EventArgs e)
        {
            cbDirectionOfTraining.SelectedIndex = -1;
            cbTypeGroup.SelectedIndex = -1;
            tbNameGroup.Text = string.Empty;

            //загрузка списка слушателей
            Filter(null, null, (int?)cbTypeGroup.SelectedValue, cbActivity.Text, sortColumn, dtpStart.Value, dtpEnd.Value, sortOrder);
        }

        private void cbTeachers_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, cbActivity.Text, sortColumn, dtpStart.Value, dtpEnd.Value, sortOrder);
        }

        private void dgvListGroups_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                int currentRow = e.RowIndex;
                if (currentRow != -1)
                {
                    var idGroup = Convert.ToInt32(dgvListGroups.Rows[currentRow].Cells["GroupId"].Value);
                    var currentGroup = db.Groups.AsNoTracking().FirstOrDefault(a => a.Id == idGroup);
                    if (currentGroup != null)
                    {
                        var groupForm = new GroupForm(currentGroup);
                        groupForm.MdiParent = this.MdiParent;
                        groupForm.Show();
                        Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, cbActivity.Text, sortColumn, dtpStart.Value, dtpEnd.Value, sortOrder);

                        if (currentRow != dgvListGroups.Rows.Count)
                        {
                            if (dgvListGroups.Rows.Count != 0)
                            {
                                dgvListGroups.Rows[currentRow].Selected = true;
                                dgvListGroups.CurrentCell = dgvListGroups.Rows[currentRow].Cells["Name"];
                            }
                        }
                        else
                        {
                            dgvListGroups.Rows[dgvListGroups.Rows.Count - 1].Selected = true;
                            dgvListGroups.CurrentCell = dgvListGroups.Rows[currentRow].Cells["Name"];
                        }
                    }
                    else
                        MessageBox.Show("Не удалось найти группу в базе данных", "Ошибка", MessageBoxButtons.OK);
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

        private void подробныйСписокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvListGroups.Rows.Count != 0)
            {
                try
                {
                    var total = (from groups in db.Groups
                                 join courses in db.Courses on groups.CourseId equals courses.Id
                                 join directions in db.Directions on courses.DirectionId equals directions.Id
                                 join teachers in db.Workers on groups.TeacherId equals teachers.Id into outerTeacher
                                 from teachers in outerTeacher.DefaultIfEmpty()
                                 join activity in db.ActivityGroups on groups.ActivityId equals activity.Id
                                 join classes in db.Classes on groups.ClassId equals classes.Id into outerClass
                                 from classes in outerClass.DefaultIfEmpty()
                                 join branches in db.Housings on classes.HousingId equals branches.Id into outerBranch
                                 from branches in outerBranch.DefaultIfEmpty()
                                 join years in db.Years on groups.YearId equals years.Id into g
                                 from years in g.DefaultIfEmpty()
                                 where (activity.Name == "Текущие" || activity.Name == "В наборе")
                                 select new ListGroupFull
                                 {
                                     Id = groups.Id,
                                     Direction = directions.Name,
                                     Course = courses.Name,
                                     Group = groups.Name,
                                     StatusGroup = activity.Name,
                                     Branch = branches.Name,
                                     Class = classes.Name,
                                     Days = groups.Days,
                                     Begin = groups.Begin,
                                     DurationLesson = groups.DurationLesson,
                                     DurationCourse = groups.DurationCourse,
                                     CountLesson = (groups.DurationLesson != 0) ? (int)(groups.DurationCourse / groups.DurationLesson) : 0,
                                     PassedLesson = groups.Lessons.Count,
                                     Teacher = teachers.Lastname + " " + teachers.Firstname.Substring(0, 1) + "." + teachers.Middlename.Substring(0, 1) + ".",
                                     TwoTeachers = groups.TwoTeachers,
                                     Students = groups.Enrollments.Where(a => a.GroupId == groups.Id && a.DateExclusion == null).Count(),
                                     Year = years.Name,
                                     SchedSumNow = (from scheds in db.Schedules
                                                    where scheds.Source == 2 && scheds.EnrollmentId != null && scheds.GroupId == groups.Id && scheds.DateBegin <= DateTime.Now
                                                    select new { SchedSum = scheds.Value - scheds.Discount }).Sum(s => (double?)(s.SchedSum)) ?? 0,
                                     PaysSumNow = (from pays in db.Payments
                                                   join enrolls in db.Enrollments on pays.EnrollmentId equals enrolls.Id
                                                   where enrolls.GroupId == groups.Id && enrolls.DateExclusion == null && pays.IsDeleted == false && pays.AdditionalPay == false
                                                   select new { PaySum = pays.ValuePayment }).Sum(s => (double?)(s.PaySum)) ?? 0,
                                     //SaldoNow = String.Empty,

                                 }).ToList();
                    var exportGroupFull = new Report();
                    exportGroupFull.ExportExcelGroupFull(total);
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

        private void dgvListGroups_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
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

            Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, cbActivity.Text, sortColumn, dtpStart.Value, dtpEnd.Value, sortOrder);
            grid.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, cbActivity.Text, sortColumn, dtpStart.Value, dtpEnd.Value, sortOrder);
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, cbActivity.Text, sortColumn, dtpStart.Value, dtpEnd.Value, sortOrder);
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (dgvListGroups.Rows.Count != 0)
            {
                var exportGroup = new Report();
                exportGroup.ExportExcelGroup(dgvListGroups, true);
            }
        }
    }
}
