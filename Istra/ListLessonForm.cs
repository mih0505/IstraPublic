using Istra.Documents;
using Istra.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Istra
{
    public partial class ListLessonForm : Form
    {
        IstraContext db = new IstraContext();
        string sortColumn;
        SortOrder sortOrder;

        public ListLessonForm()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            InitializeComponent();
        }

        private void ListLessonForm_Load(object sender, EventArgs e)
        {
            try
            {
                //загрузка фильтра направлений
                var direction = db.Directions.Where(a => a.IsRemoved == false).ToList();
                cbDirectionOfTraining.DataSource = direction;
                cbDirectionOfTraining.DisplayMember = "Name";
                cbDirectionOfTraining.ValueMember = "Id";
                cbDirectionOfTraining.SelectedIndex = -1;

                //загрузка фильтра групп
                var actGroups = db.Groups.Where(a => a.Activity.Name != "Закрытые" && a.Individual == false).OrderBy(a => a.Name).ToList();
                cbGroups.DataSource = actGroups;
                cbGroups.DisplayMember = "Name";
                cbGroups.ValueMember = "Id";
                cbGroups.SelectedIndex = -1;

                //загрузка фильтра преподавателей
                var teachers = db.Workers.Where(a => a.Role.Name == "Преподаватель")
                .OrderBy(a => a.Lastname).ThenBy(a => a.Firstname).ToList();
                cbTeachers.DataSource = teachers;
                cbTeachers.DisplayMember = "LastnameFM";
                cbTeachers.ValueMember = "Id";
                cbTeachers.SelectedIndex = -1;

                //загрузка фильтра корпусов
                var housing = db.Housings.Where(a => a.IsRemoved == false).ToList();
                cbHousings.DataSource = housing;
                cbHousings.DisplayMember = "Name";
                cbHousings.ValueMember = "Id";
                cbHousings.SelectedIndex = -1;

                //установка дат
                var date = dtpBegin.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                date = date.AddMonths(1).AddDays(-1);
                dtpEnd.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, date.Day);


                //добавление программной сортировки
                foreach (DataGridViewColumn col in dgvListLessons.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.Programmatic;
                }

                
                //загрузка списка занятий
                Filter(null, null, dtpBegin.Value.Date, dtpEnd.Value.Date, null, null, sortColumn, sortOrder);

                //оформление таблицы
                dgvListLessons.Columns["GroupId"].Visible = dgvListLessons.Columns["DirectionId"].Visible =
                    dgvListLessons.Columns["TeacherId"].Visible = dgvListLessons.Columns["HousingId"].Visible =
                    dgvListLessons.Columns["Wage"].Visible = dgvListLessons.Columns["Wages"].Visible = false;

                dgvListLessons.Columns["DirectionName"].HeaderText = "Направление";
                dgvListLessons.Columns["GroupName"].HeaderText = "Группа";
                dgvListLessons.Columns["Branch"].HeaderText = "Корпус";
                dgvListLessons.Columns["Students"].HeaderText = "Учащихся";
                dgvListLessons.Columns["Class"].HeaderText = "Класс";
                dgvListLessons.Columns["DateLesson"].HeaderText = "Дата";
                dgvListLessons.Columns["DateLesson"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvListLessons.Columns["Number"].HeaderText = "Номер";
                dgvListLessons.Columns["DurationLesson"].HeaderText = "Продолж.";
                dgvListLessons.Columns["CourseName"].HeaderText = "Курс";
                dgvListLessons.Columns["Teacher"].HeaderText = "Преподаватель";                
                dgvListLessons.Columns["Topic"].HeaderText = "Тема";
                dgvListLessons.Columns["Topic"].Width = 300;

                dgvListLessons.Focus(); dgvListLessons.Select();
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Filter(int? directionId, int? groupId, DateTime dateBegin, DateTime dateEnd, int? teacherId, int? housingId, string column, SortOrder? sortOrder)
        {
            try
            {
                dateEnd = dateEnd.AddDays(1).Date;
                var listLessons = from lessons in db.Lessons
                                 join teachers in db.Workers on lessons.TeacherId equals teachers.Id into outerTeacher
                                 from teachers in outerTeacher.DefaultIfEmpty()
                                 join groups in db.Groups on lessons.GroupId equals groups.Id
                                 join courses in db.Courses on groups.CourseId equals courses.Id
                                 join directions in db.Directions on courses.DirectionId equals directions.Id
                                 join classes in db.Classes on lessons.ClassId equals classes.Id into outerClass
                                 from classes in outerClass.DefaultIfEmpty()
                                 join branches in db.Housings on classes.HousingId equals branches.Id into outerBranch
                                 from branches in outerBranch.DefaultIfEmpty()
                                 join topics in db.Topics on lessons.TopicId equals topics.Id into outerTopics
                                 from topics in outerTopics.DefaultIfEmpty()
                                 where lessons.Date >= dateBegin && lessons.Date < dateEnd
                                 select new ListLessons
                                 {
                                     GroupId = groups.Id,
                                     DirectionId = directions.Id,                                     
                                     TeacherId = teachers.Id,
                                     HousingId = branches.Id,
                                     Teacher = teachers.Lastname + " " + teachers.Firstname.Substring(0, 1) + "." + teachers.Middlename.Substring(0, 1) + ".",
                                     DateLesson = lessons.Date,
                                     Number = lessons.Number,
                                     GroupName = groups.Name,
                                     Students = db.Enrollments.Where(a => a.GroupId == groups.Id && a.DateExclusion == null).Count(),
                                     DurationLesson = lessons.DurationLesson,
                                     CourseName = courses.Name,
                                     DirectionName = directions.Name,                                     
                                     Branch = branches.Name,
                                     Class = classes.Name,                                     
                                     Topic = topics.Name,                                     
                                 };

                if (directionId != null) listLessons = listLessons.Where(d => d.DirectionId == directionId);
                if (teacherId != null) listLessons = listLessons.Where(d => d.TeacherId == teacherId);
                if (groupId != null) listLessons = listLessons.Where(d => d.GroupId == groupId);
                if (housingId != null) listLessons = listLessons.Where(d => d.HousingId == housingId);
                
                //сортировка                
                if (column != null)
                {
                    switch (column)
                    {
                        case "Teacher":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listLessons = listLessons.OrderBy(x => x.Teacher);
                                }
                                else
                                {
                                    listLessons = listLessons.OrderByDescending(x => x.Teacher);
                                }
                                break;
                            }
                        case "DateLesson":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listLessons = listLessons.OrderBy(x => x.DateLesson);
                                }
                                else
                                {
                                    listLessons = listLessons.OrderByDescending(x => x.DateLesson);
                                }
                                break;
                            }
                        case "Number":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listLessons = listLessons.OrderBy(x => x.Number);
                                }
                                else
                                {
                                    listLessons = listLessons.OrderByDescending(x => x.Number);
                                }
                                break;
                            }
                        case "GroupName":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listLessons = listLessons.OrderBy(x => x.GroupName);
                                }
                                else
                                {
                                    listLessons = listLessons.OrderByDescending(x => x.GroupName);
                                }
                                break;
                            }
                        case "CourseName":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listLessons = listLessons.OrderBy(x => x.CourseName);
                                }
                                else
                                {
                                    listLessons = listLessons.OrderByDescending(x => x.CourseName);
                                }
                                break;
                            }
                        case "DirectionName":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listLessons = listLessons.OrderBy(x => x.DirectionName);
                                }
                                else
                                {
                                    listLessons = listLessons.OrderByDescending(x => x.DirectionName);
                                }
                                break;
                            }
                        case "Branch":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listLessons = listLessons.OrderBy(x => x.Branch);
                                }
                                else
                                {
                                    listLessons = listLessons.OrderByDescending(x => x.Branch);
                                }
                                break;
                            }
                        case "Class":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listLessons = listLessons.OrderBy(x => x.Class);
                                }
                                else
                                {
                                    listLessons = listLessons.OrderByDescending(x => x.Class);
                                }
                                break;
                            }
                        case "Topic":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listLessons = listLessons.OrderBy(x => x.Topic);
                                }
                                else
                                {
                                    listLessons = listLessons.OrderByDescending(x => x.Topic);
                                }
                                break;
                            }
                    }
                }
                else
                    listLessons = listLessons.OrderBy(f => f.DateLesson);
                dgvListLessons.DataSource = listLessons.ToList();

                if (sortOrder != SortOrder.None)
                    dgvListLessons.Columns[sortColumn].HeaderCell.SortGlyphDirection = (SortOrder)sortOrder;
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
            //загрузка списка занятий
            Filter((int?)cbDirectionOfTraining.SelectedValue, (int?)cbGroups.SelectedValue, dtpBegin.Value.Date, dtpEnd.Value.Date, (int?)cbTeachers.SelectedValue, (int?)cbHousings.SelectedValue, sortColumn, sortOrder);
        }

        private void dgvListLessons_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
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

            Filter((int?)cbDirectionOfTraining.SelectedValue, (int?)cbGroups.SelectedValue, dtpBegin.Value.Date, dtpEnd.Value.Date, (int?)cbTeachers.SelectedValue, (int?)cbHousings.SelectedValue, sortColumn, sortOrder);
            grid.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            cbHousings.SelectedIndex = cbTeachers.SelectedIndex = cbGroups.SelectedIndex = cbDirectionOfTraining.SelectedIndex = -1;
            //загрузка списка занятий
            Filter(null, null, dtpBegin.Value.Date, dtpEnd.Value.Date, null, null, sortColumn, sortOrder);
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            if (dgvListLessons.Rows.Count != 0)
            {
                var exportLessons = new Report();
                exportLessons.ExportExcelLessons(dgvListLessons, CurrentSession.CurrentRole.Name);
            }
        }
    }
}
