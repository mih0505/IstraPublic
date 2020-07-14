using Istra.Documents;
using Istra.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Istra
{
    public partial class ListGroupsForm : Form
    {
        IstraContext db = new IstraContext();
        string sortColumn;
        SortOrder sortOrder;

        public ListGroupsForm()
        {
            InitializeComponent();
        }

        private void ListGroupsForm_Load(object sender, EventArgs e)
        {
            try
            {
                //загрузка фильтра направлений
                var direction = db.Directions.Where(a => a.IsRemoved == false).ToList();
                cbDirectionOfTraining.DataSource = direction;
                cbDirectionOfTraining.DisplayMember = "Name";
                cbDirectionOfTraining.ValueMember = "Id";
                cbDirectionOfTraining.SelectedIndex = -1;

                //загрузка фильтра преподавателей
                var roleTeacher = db.Roles.Where(a => a.IsRemoved == false).FirstOrDefault(a => a.Name == "Преподаватель");
                if (roleTeacher != null)
                {
                    var teachers = from teach in db.Workers
                                   where teach.RoleId == roleTeacher.Id && teach.IsRemoved == false
                                   select new
                                   {
                                       Id = teach.Id,
                                       Teacher = teach.Lastname + " "
                                   + teach.Firstname.Substring(0, 1) + "."
                                   + teach.Middlename.Substring(0, 1) + ".",
                                       RoleId = teach.RoleId
                                   };

                    cbTeachers.DataSource = teachers.OrderBy(a => a.Teacher).ToList();
                    cbTeachers.DisplayMember = "Teacher";
                    cbTeachers.ValueMember = "Id";
                    cbTeachers.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Не удалось получить список преподавателей", "Ошибка", MessageBoxButtons.OK);
                }

                //загрузка фильтра тип обучения 
                var type = db.Years.Where(a => a.IsRemoved == false).OrderBy(a=>a.SortIndex).ToList();
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
                Filter(null, null, null, null, cbActivity.Text, sortColumn, sortOrder);

                //оформление таблицы
                dgvListGroups.Columns["GroupId"].Visible = dgvListGroups.Columns["DirectionId"].Visible =
                    dgvListGroups.Columns["TeacherId"].Visible = dgvListGroups.Columns["ActivityId"].Visible =
                    dgvListGroups.Columns["Activity"].Visible = dgvListGroups.Columns["YearId"].Visible =
                    dgvListGroups.Columns["Type"].Visible = false;

                dgvListGroups.Columns["Direction"].HeaderText = "Направление";
                dgvListGroups.Columns["Name"].HeaderText = "Группа";
                dgvListGroups.Columns["Branch"].HeaderText = "Корпус";
                dgvListGroups.Columns["Class"].HeaderText = "Класс";
                dgvListGroups.Columns["Days"].HeaderText = "Дни занятий";
                dgvListGroups.Columns["Begin"].HeaderText = "Начало";
                dgvListGroups.Columns["DurationLesson"].HeaderText = "Продолж.";
                dgvListGroups.Columns["Year"].HeaderText = "Уч. год";
                dgvListGroups.Columns["Teacher"].HeaderText = "Преподаватель";
                dgvListGroups.Columns["TwoTeachers"].HeaderText = "Микс";
                dgvListGroups.Columns["TwoTeachers"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgvListGroups.Columns["Note"].HeaderText = "Примечание";
                dgvListGroups.Columns["Students"].HeaderText = "Количество";
                dgvListGroups.Columns["Note"].Width = 200;
                dgvListGroups.Columns["Begin"].DefaultCellStyle.Format = "HH:mm";

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

        private void Filter(int? directionId, string name, int? typeId, int? teacherId, string activeGroupsName, string column, SortOrder? sortOrder)
        {
            try
            {
                var listGroups = from groups in db.Groups
                                 join courses in db.Courses on groups.CourseId equals courses.Id
                                 join directions in db.Directions on courses.DirectionId equals directions.Id
                                 join teachers in db.Workers on groups.TeacherId equals teachers.Id into outerTeacher
                                 from teachers in outerTeacher.DefaultIfEmpty()
                                 join activities in db.ActivityGroups on groups.ActivityId equals activities.Id
                                 join classes in db.Classes on groups.ClassId equals classes.Id into outerClass
                                 from classes in outerClass.DefaultIfEmpty()
                                 join branches in db.Housings on classes.HousingId equals branches.Id into outerBranch
                                 from branches in outerBranch.DefaultIfEmpty()
                                 join years in db.Years on groups.YearId equals years.Id into outer
                                 from years in outer.DefaultIfEmpty()
                                 select new ListGroups
                                 {
                                     GroupId = groups.Id,
                                     DirectionId = directions.Id,
                                     Direction = directions.Name,
                                     TeacherId = teachers.Id,
                                     ActivityId = activities.Id,
                                     YearId = years.Id,
                                     Name = groups.Name,
                                     Branch = branches.Name,
                                     Class = classes.Name,
                                     Days = groups.Days,
                                     Begin = groups.Begin,
                                     DurationLesson = groups.DurationLesson,
                                     Year = years.Name,
                                     Teacher = teachers.Lastname + " " + teachers.Firstname.Substring(0, 1) + "." + teachers.Middlename.Substring(0, 1) + ".",
                                     TwoTeachers = groups.TwoTeachers,
                                     Students = db.Enrollments.Where(a => a.GroupId == groups.Id && a.DateExclusion == null).Count(),
                                     Note = groups.Note,
                                     Activity = activities.Name
                                 };

                if (directionId != null) listGroups = listGroups.Where(d => d.DirectionId == directionId);
                if (teacherId != null) listGroups = listGroups.Where(d => d.TeacherId == teacherId);
                if (name != null && name != "") listGroups = listGroups.Where(e => e.Name.StartsWith(name));

                if (typeId != null) listGroups = listGroups.Where(d => d.YearId == typeId);

                if (activeGroupsName == "Активные")
                    listGroups = listGroups.Where(a => a.Activity != "Закрытые");
                else listGroups = listGroups.Where(a => a.Activity == activeGroupsName);

                //фильтрация по праву доступа к группам
                if (!CurrentSession.CurrentUser.AllAccessGroups)
                {
                    var listOpenGroups = db.AccessGroups.Where(a => a.WorkerId == CurrentSession.CurrentUser.Id).Select(a => a.GroupId).ToList();
                    listGroups = listGroups.Where(a => listOpenGroups.Contains(a.GroupId));
                }

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
                        case "Days":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listGroups = listGroups.OrderBy(x => x.Days);
                                }
                                else
                                {
                                    listGroups = listGroups.OrderByDescending(x => x.Days);
                                }
                                break;
                            }
                        case "DurationLesson":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listGroups = listGroups.OrderBy(x => x.DurationLesson);
                                }
                                else
                                {
                                    listGroups = listGroups.OrderByDescending(x => x.DurationLesson);
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
                        case "Teacher":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listGroups = listGroups.OrderBy(x => x.Teacher);
                                }
                                else
                                {
                                    listGroups = listGroups.OrderByDescending(x => x.Teacher);
                                }
                                break;
                            }
                        case "TwoTeachers":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listGroups = listGroups.OrderBy(x => x.TwoTeachers);
                                }
                                else
                                {
                                    listGroups = listGroups.OrderByDescending(x => x.TwoTeachers);
                                }
                                break;
                            }
                        case "Note":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listGroups = listGroups.OrderBy(x => x.Note);
                                }
                                else
                                {
                                    listGroups = listGroups.OrderByDescending(x => x.Note);
                                }
                                break;
                            }
                        case "Students":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listGroups = listGroups.OrderBy(x => x.Students);
                                }
                                else
                                {
                                    listGroups = listGroups.OrderByDescending(x => x.Students);
                                }
                                break;
                            }
                    }
                }
                else
                    listGroups = listGroups.OrderBy(f => f.Direction);
                dgvListGroups.DataSource = listGroups.ToList();

                if (sortOrder != SortOrder.None)
                    dgvListGroups.Columns[sortColumn].HeaderCell.SortGlyphDirection = (SortOrder)sortOrder;
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
            Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, (int?)cbTeachers.SelectedValue, cbActivity.Text, sortColumn, sortOrder);
        }

        private void tbNameGroup_TextChanged(object sender, EventArgs e)
        {
            Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, (int?)cbTeachers.SelectedValue, cbActivity.Text, sortColumn, sortOrder);
        }

        private void cbTypeGroup_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, (int?)cbTeachers.SelectedValue, cbActivity.Text, sortColumn, sortOrder);
        }

        private void chbActive_CheckedChanged(object sender, EventArgs e)
        {
            Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, (int?)cbTeachers.SelectedValue, cbActivity.Text, sortColumn, sortOrder);
        }

        private void btCleanFilter_Click(object sender, EventArgs e)
        {
            cbDirectionOfTraining.SelectedIndex = -1;
            cbTypeGroup.SelectedIndex = -1;
            tbNameGroup.Text = string.Empty;
            cbTeachers.SelectedIndex = -1;
            //загрузка списка слушателей
            Filter(null, null, null, null, cbActivity.Text, sortColumn, sortOrder);
        }

        private void cbTeachers_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, (int?)cbTeachers.SelectedValue, cbActivity.Text, sortColumn, sortOrder);
        }

        private void dgvListGroups_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                int currentRow = e.RowIndex;
                if (currentRow != -1)
                {
                    var idGroup = Convert.ToInt32(dgvListGroups.Rows[currentRow].Cells["GroupId"].Value);
                    Group currentGroup;
                    using (var db = new IstraContext())
                    {
                        currentGroup = db.Groups.FirstOrDefault(a => a.Id == idGroup);
                    }
                    if (currentGroup != null)
                    {
                        var groupForm = new GroupForm(currentGroup);
                        groupForm.MdiParent = this.MdiParent;
                        groupForm.Show();                        
                        Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, (int?)cbTeachers.SelectedValue, cbActivity.Text, sortColumn, sortOrder);

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

        private void btExportExcel_Click(object sender, EventArgs e)
        {
            if (dgvListGroups.Rows.Count != 0)
            {
                var exportGroup = new Report();
                exportGroup.ExportExcelGroup(dgvListGroups, false);
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
                                                    join enrolls in db.Enrollments on scheds.EnrollmentId equals enrolls.Id
                                                    where scheds.Source == 2 && scheds.EnrollmentId != null && scheds.GroupId == groups.Id && enrolls.DateExclusion == null
                                                    && scheds.DateBegin <= DateTime.Now
                                                    select new { SchedSum = scheds.Value - scheds.Discount }).Sum(s => (double?)(s.SchedSum)) ?? 0,
                                     PaysSumNow = (from pays in db.Payments
                                                   join enrolls in db.Enrollments on pays.EnrollmentId equals enrolls.Id
                                                   where enrolls.GroupId == groups.Id && enrolls.DateExclusion == null && pays.IsDeleted == false && pays.AdditionalPay == false
                                                   select new { PaySum = pays.ValuePayment }).Sum(s => (double?)(s.PaySum)) ?? 0,
                                     SaldoNow = String.Empty,

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
            
            Filter((int?)cbDirectionOfTraining.SelectedValue, tbNameGroup.Text, (int?)cbTypeGroup.SelectedValue, (int?)cbTeachers.SelectedValue, cbActivity.Text, sortColumn, sortOrder);
            grid.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
        }
                
    }
}
