using Istra.Documents;
using Istra.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Istra
{
    public partial class WagesForm : Form
    {
        IstraContext db = new IstraContext();
        string sortColumn;
        SortOrder sortOrder;
        bool notFiltering = true;
        double countLesson, summary, countHours, totalPays, totalRetentions, totalProfit;
        DateTime dateStart, dateEnd;
        public List<ListLessons> listLessons;

        public WagesForm()
        {
            InitializeComponent();
        }

        private void WagesForm_Load(object sender, EventArgs e)
        {
            //установка периода на прошлый месяц
            dtpStart.Value = dtpDateBeginR.Value = dtpMonth.Value = DateTime.Now.AddMonths(-1);
            dateStart = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, 1);
            dateEnd = dtpStart.Value.AddMonths(1).AddDays(-1);

            //включение фильтрации
            notFiltering = false;

            //загрузка преподавателей
            var teachers = db.Workers.Where(a => a.Role.Name == "Преподаватель")
                .OrderBy(a => a.Lastname).ThenBy(a => a.Firstname).ToList();
            cbTeachers.DataSource = teachers;
            cbTeachers.DisplayMember = "LastnameFM";
            cbTeachers.ValueMember = "Id";
            cbTeachers.SelectedIndex = -1;

            //загрузка преподавателей для операций начислений/удержаний/выплат
            var teachersК = (from workers in db.Workers
                             join deps in db.Departments on workers.DepartmentId equals deps.Id
                            where workers.IsRemoved == false
                            orderby workers.Lastname, workers.Firstname, workers.Middlename
                            select new ListWorkers
                            {
                                Id = workers.Id,
                                Worker = workers.Lastname + " " + workers.Firstname.Substring(0, 1) + "." + workers.Middlename.Substring(0, 1) + ". ("+deps.Name+")",                                
                            }).ToList();
            cbTeacherR.DataSource = teachersК;
            cbTeacherR.DisplayMember = "Worker";
            cbTeacherR.ValueMember = "Id";
            cbTeacherR.SelectedIndex = -1;

            //заполнение списка учебного года
            var years = db.Years.ToList();
            cbYears.DataSource = years;
            cbYears.DisplayMember = "Name";
            cbYears.ValueMember = "Id";
            //выбор последнего учебного года
            var maxYear = db.Years.Max(a => a.Id);
            cbYears.SelectedValue = maxYear;

            //загрузка шаблонов зп            
            LoadTemplates();

            //загрузка всех групп учебного года
            LoadGroups();

            //загрузка оплаты и групп по шаблону
            if (lbTemplates.Items.Count > 0)
            {
                lbTemplates.SelectedIndex = 0;
                LoadTemplateRates((int)lbTemplates.SelectedValue);
                LoadTemplateGroups((int)lbTemplates.SelectedValue);
            }

            //заполнение таблицы с занятиями
            //добавление программной сортировки
            foreach (DataGridViewColumn col in dgvListLessons.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            //загрузка списка занятий
            Filter(dateStart, null);

            //оформление таблицы
            dgvListLessons.Columns["GroupId"].Visible = dgvListLessons.Columns["DirectionId"].Visible =
                dgvListLessons.Columns["TeacherId"].Visible = dgvListLessons.Columns["HousingId"].Visible =
            dgvListLessons.Columns["Topic"].Visible = false;

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
            dgvListLessons.Columns["Wage"].HeaderText = "Ставка";
            dgvListLessons.Columns["Wage"].DefaultCellStyle.Format = "0.00";
            dgvListLessons.Columns["Wages"].HeaderText = "Сумма";
            dgvListLessons.Columns["Wages"].DefaultCellStyle.Format = "0.00";

            dgvListLessons.Focus(); dgvListLessons.Select();

            //блокируем столбец с названиями групп в таблице со списком всех групп
            dgvAllGroups.Columns[1].ReadOnly = true;

            //загрузка списка операций по начислению/удержанию/выплатам
            FilterRetention(dateStart, null);
            dgvTransaction.Columns["Id"].Visible = false;
            dgvTransaction.Columns["Date"].HeaderText = "Дата";
            dgvTransaction.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvTransaction.Columns["Month"].HeaderText = "Месяц";
            dgvTransaction.Columns["Month"].DefaultCellStyle.Format = "MMM/yy";
            dgvTransaction.Columns["Teacher"].HeaderText = "Преподаватель";
            dgvTransaction.Columns["Teacher"].Width = 160;
            dgvTransaction.Columns["Name"].HeaderText = "Основание";
            dgvTransaction.Columns["Name"].Width = 400;
            dgvTransaction.Columns["Type"].HeaderText = "Операция";
            dgvTransaction.Columns["Value"].HeaderText = "Сумма";
            dgvTransaction.Columns["Value"].DefaultCellStyle.Format = "0.00";

            //загрузка списка отделов
            var departments = db.Departments.Where(a => a.IsRemoved == false).OrderBy(a => a.SortIndex).ToList();
            cbDepartments.DataSource = departments;
            cbDepartments.DisplayMember = "Name";
            cbDepartments.ValueMember = "Id";

            //загрузка списка сотрудников на вкладке расчетных листов
            FilterWorkers("Все");
            dgvListWorkers.Columns["Id"].Visible = false;
            dgvListWorkers.Columns["Worker"].HeaderText = "Сотрудник";
            dgvListWorkers.Columns["Department"].HeaderText = "Подразделение";

            //заполнение первого расчетного листа

            int idWorker = Convert.ToInt32(dgvListWorkers["Id", 0].Value);
            GetTransactionsList(dateStart, idWorker);

        }

        private void Total(DataGridView dgv)
        {
            countLesson = countHours = summary = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                countLesson = dgv.Rows.Count;
                countHours += Convert.ToDouble(row.Cells["DurationLesson"].Value);
                summary += Convert.ToDouble(row.Cells["Wages"].Value);
            }
            toolStripStatusLabel2.Text = countLesson.ToString();
            toolStripStatusLabel6.Text = countHours.ToString();
            toolStripStatusLabel4.Text = summary.ToString("C", CultureInfo.CurrentCulture);
        }

        private void TotalTransactions(DataGridView dgv)
        {
            totalPays = totalRetentions = totalProfit = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if(row.Cells["Type"].Value.ToString() == "Начисление")
                    totalProfit += Convert.ToDouble(row.Cells["Value"].Value);
                else if(row.Cells["Type"].Value.ToString() == "Удержание")
                    totalRetentions += Convert.ToDouble(row.Cells["Value"].Value);
                else
                    totalPays += Convert.ToDouble(row.Cells["Value"].Value);
            }
            toolStripStatusLabel8.Text = totalProfit.ToString("C", CultureInfo.CurrentCulture);
            toolStripStatusLabel10.Text = totalRetentions.ToString("C", CultureInfo.CurrentCulture);
            toolStripStatusLabel14.Text = (totalProfit - totalRetentions).ToString("C", CultureInfo.CurrentCulture);
            toolStripStatusLabel12.Text = totalPays.ToString("C", CultureInfo.CurrentCulture);
        }

        private List<ListLessons> Filter(DateTime dateBegin, int? teacherId)
        {
            try
            {
                dateEnd = dateBegin.AddMonths(1).Date;

                //string teacherFilter = (teacherId != null) ? "AND dbo.Workers.Id =" + teacherId : "";
                //string sql = "SELECT dbo.Groups.Id AS GroupId, dbo.Directions.Id AS DirectionId, dbo.Workers.Id AS TeacherId, dbo.Housings.Id AS HousingId, " +
                //         "dbo.Workers.Lastname + ' ' + LEFT(dbo.Workers.Firstname, 1) + '.' + LEFT(dbo.Workers.Middlename, 1) + '.' AS Teacher, " +
                //         "dbo.Lessons.[Date] AS DateLesson, dbo.Lessons.Number, dbo.Groups.Name AS GroupName, " +
                //         "(SELECT COUNT(Id) FROM dbo.Enrollments WHERE (DateExclusion IS NULL) AND (GroupId = Groups.Id)) AS Students, " +
                //         "dbo.Lessons.DurationLesson, dbo.Courses.Name AS CourseName, dbo.Directions.Name AS DirectionName, dbo.Housings.Name AS Branch, dbo.Classes.Name AS Class, " +
                //         "(SELECT isnull(MAX(dbo.TemplateRates.Wage), 0) FROM dbo.Templates INNER JOIN dbo.TemplateRates ON dbo.Templates.Id = dbo.TemplateRates.TemplateId INNER JOIN " +
                //         "dbo.TemplateGroups ON dbo.Templates.Id = dbo.TemplateGroups.TemplateId " +
                //         "WHERE(dbo.TemplateGroups.GroupId = Groups.Id) AND (dbo.TemplateRates.CountStudents = (SELECT COUNT(Enrollments.GroupId) FROM Enrollments WHERE DateExclusion IS NULL AND GroupId = Groups.Id))) AS Wage, " +
                //         "(dbo.Lessons.DurationLesson * (SELECT isnull(MAX(dbo.TemplateRates.Wage),0) FROM dbo.Templates INNER JOIN dbo.TemplateRates ON dbo.Templates.Id = dbo.TemplateRates.TemplateId INNER JOIN " +
                //         "dbo.TemplateGroups ON dbo.Templates.Id = dbo.TemplateGroups.TemplateId " +
                //         "WHERE(dbo.TemplateGroups.GroupId = Groups.Id) AND(dbo.TemplateRates.CountStudents = (SELECT COUNT(Enrollments.GroupId) FROM Enrollments WHERE DateExclusion IS NULL AND GroupId = Groups.Id)))) AS Wages " +
                //         "FROM dbo.Groups INNER JOIN " +
                //         "dbo.Courses ON dbo.Groups.CourseId = dbo.Courses.Id INNER JOIN " +
                //         "dbo.Directions ON dbo.Courses.DirectionId = dbo.Directions.Id INNER JOIN " +
                //         "dbo.Classes ON dbo.Groups.ClassId = dbo.Classes.Id INNER JOIN " +
                //         "dbo.Housings ON dbo.Classes.HousingId = dbo.Housings.Id INNER JOIN " +
                //         "dbo.TemplateGroups ON dbo.Groups.Id = dbo.TemplateGroups.GroupId INNER JOIN " +
                //         "dbo.Templates ON dbo.TemplateGroups.TemplateId = dbo.Templates.Id INNER JOIN " +
                //         "dbo.TemplateRates ON dbo.Templates.Id = dbo.TemplateRates.TemplateId RIGHT OUTER JOIN " +
                //         "dbo.Lessons INNER JOIN " +
                //         "dbo.Workers ON dbo.Lessons.TeacherId = dbo.Workers.Id ON dbo.Classes.Id = dbo.Lessons.ClassId AND dbo.Groups.Id = dbo.Lessons.GroupId RIGHT OUTER JOIN " +
                //         "dbo.Enrollments ON dbo.Groups.Id = dbo.Enrollments.GroupId " +
                //         "GROUP BY dbo.Groups.Id, dbo.Directions.Id, dbo.Lessons.Date, dbo.Lessons.Number, dbo.Groups.Name, dbo.Courses.Name, dbo.Directions.Name, dbo.Workers.Lastname, dbo.Workers.Id, " +
                //         "dbo.Classes.Name, dbo.Housings.Name, dbo.Housings.Id, dbo.Lessons.DurationLesson, dbo.Workers.Firstname, dbo.Workers.Middlename " +
                //         "HAVING dbo.Lessons.[Date] >= '" + dateBegin.Date + "' AND dbo.Lessons.[Date] < '" + dateEnd.Date + "' " + teacherFilter +
                //         " ORDER BY dbo.Lessons.[Date] ASC";

                //var listLessons = db.Database.SqlQuery<ListLessons>(sql).ToList();

                listLessons = teacherId == null
                    ? (from lessons in db.Lessons
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
                       orderby lessons.Date ascending
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
                           Wage = db.TemplateRates.Where(a => a.TemplateId == db.TemplateGroups.FirstOrDefault(b => b.GroupId == groups.Id).TemplateId
                           && a.CountStudents == db.Enrollments.Where(c => c.GroupId == groups.Id && c.DateExclusion == null).Count()).Select(w => w.Wage).FirstOrDefault()

                       }).ToList()
                    : (from lessons in db.Lessons
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
                       where lessons.Date >= dateBegin && lessons.Date < dateEnd && teachers.Id == teacherId
                       orderby lessons.Date ascending
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
                           Wage = db.TemplateRates.Where(a => a.TemplateId == db.TemplateGroups.FirstOrDefault(b => b.GroupId == groups.Id).TemplateId
                           && a.CountStudents == db.Enrollments.Where(c => c.GroupId == groups.Id && c.DateExclusion == null).Count()).Select(w => w.Wage).FirstOrDefault()

                       }).ToList();
                dgvListLessons.DataSource = listLessons;
                Total(dgvListLessons);
                return listLessons;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void FilterRetention(DateTime dateBegin, int? teacherId)
        {
            dateEnd = dateBegin.AddMonths(1).Date;
            List<ListRetentions> lstRetentions;
            if (teacherId == null)
                lstRetentions = (from transactions in db.Retentions
                                 join workers in db.Workers on transactions.WorkerId equals workers.Id into outerWorker
                                 from workers in outerWorker.DefaultIfEmpty()
                                 join bases in db.Bases on transactions.BaseId equals bases.Id into outerBase
                                 from bases in outerBase.DefaultIfEmpty()
                                 join types in db.TypeOfTransactions on transactions.WorkerId equals types.Id into outerType
                                 from types in outerType.DefaultIfEmpty()
                                 where transactions.Month >= dateBegin && transactions.Month < dateEnd
                                 orderby transactions.Date descending
                                 select new ListRetentions
                                 {
                                     Id = transactions.Id,
                                     Date = transactions.Date,
                                     Month = transactions.Month,
                                     Teacher = workers.Lastname + " " + workers.Firstname.Substring(0, 1) + "." + workers.Middlename.Substring(0, 1) + ".",
                                     Name = bases.Name != null ? bases.Name : transactions.Name + " Кол:" + transactions.Count + " Час:" + transactions.Hours + " Ст:" + transactions.Wage,
                                     Type = transactions.TypeOfTransaction.Name,
                                     Value = transactions.Value
                                 }).ToList();
            else
                lstRetentions = (from transactions in db.Retentions
                                 join workers in db.Workers on transactions.WorkerId equals workers.Id into outerWorker
                                 from workers in outerWorker.DefaultIfEmpty()
                                 join bases in db.Bases on transactions.BaseId equals bases.Id into outerBase
                                 from bases in outerBase.DefaultIfEmpty()
                                 join types in db.TypeOfTransactions on transactions.WorkerId equals types.Id into outerType
                                 from types in outerType.DefaultIfEmpty()
                                 where transactions.Month >= dateBegin && transactions.Month < dateEnd && transactions.WorkerId == teacherId
                                 orderby transactions.Date descending
                                 select new ListRetentions
                                 {
                                     Id = transactions.Id,
                                     Date = transactions.Date,
                                     Month = transactions.Month,
                                     Teacher = workers.Lastname + " " + workers.Firstname.Substring(0, 1) + "." + workers.Middlename.Substring(0, 1) + ".",
                                     Name = bases.Name != null ? bases.Name : transactions.Name + " Кол:" + transactions.Count + " Час:" + transactions.Hours + " Ст:" + transactions.Wage,
                                     Type = transactions.TypeOfTransaction.Name,
                                     Value = transactions.Value
                                 }).ToList();

            dgvTransaction.DataSource = lstRetentions;
            TotalTransactions(dgvTransaction);
        }

        private void FilterWorkers(string department)
        {
            IQueryable<ListWorkers> workersList;
            if (department == "Все")
                workersList = from workers in db.Workers
                              join depart in db.Departments on workers.DepartmentId equals depart.Id
                              where workers.IsRemoved == false
                              orderby workers.Lastname, workers.Firstname, workers.Middlename
                              select new ListWorkers
                              {
                                  Id = workers.Id,
                                  Worker = workers.Lastname + " " + workers.Firstname.Substring(0, 1) + "." + workers.Middlename.Substring(0, 1) + ".",
                                  Department = depart.Name
                              };
            else
                workersList = from workers in db.Workers
                              join depart in db.Departments on workers.DepartmentId equals depart.Id
                              where workers.IsRemoved == false && depart.Name == department
                              orderby workers.Lastname, workers.Firstname, workers.Middlename
                              select new ListWorkers
                              {
                                  Id = workers.Id,
                                  Worker = workers.Lastname + " " + workers.Firstname.Substring(0, 1) + "." + workers.Middlename.Substring(0, 1) + ".",
                                  Department = depart.Name
                              };

            dgvListWorkers.DataSource = workersList.ToList();
        }

        private void btAddTemplate_Click(object sender, EventArgs e)
        {
            var t = new Template();
            if (checkBox1.Checked)
            {
                t.YearId = null;
            }
            else
            {
                t.YearId = (int)cbYears.SelectedValue;
            }

            var inputForm = new InputForm("Template", t);
            inputForm.ShowDialog();
            LoadTemplates();
            lbTemplates.SelectedIndex = lbTemplates.Items.Count - 1;
        }

        private void btRemoveTemplate_Click(object sender, EventArgs e)
        {
            int index = lbTemplates.SelectedIndex;
            if (index > -1)
            {
                int templateId = (int)lbTemplates.SelectedValue;
                var template = db.Templates.Find(templateId);
                if (template != null)
                {
                    db.Templates.Remove(template);
                    db.SaveChanges();
                    LoadTemplates();

                    if (index >= lbTemplates.Items.Count)
                    {
                        lbTemplates.SelectedIndex = lbTemplates.Items.Count - 1;
                    }
                    else
                    {
                        lbTemplates.SelectedIndex = index;
                    }
                }
                else MessageBox.Show("Ошибка удаления записи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btEditTemplate_Click(object sender, EventArgs e)
        {
            int index = lbTemplates.SelectedIndex;
            if (index > -1)
            {
                int templateId = (int)lbTemplates.SelectedValue;
                var template = db.Templates.Find(templateId);
                if (template != null)
                {
                    var inputForm = new InputForm("Template", template);
                    inputForm.ShowDialog();
                    LoadTemplates();
                    lbTemplates.SelectedIndex = index;
                }
                else MessageBox.Show("Ошибка редактирования записи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbYears_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LoadTemplates();
            LoadGroups();
            if (lbTemplates.Items.Count > 0)
                LoadTemplateRates((int)lbTemplates.SelectedValue);
            else dgvWages.DataSource = null;
            if (lbTemplates.Items.Count > 0)
                LoadTemplateGroups((int)lbTemplates.SelectedValue);
            else
            {
                dgvListGroups.DataSource = null;
                dgvListGroups.Columns.Clear();
                dgvListGroups.Rows.Clear();
            }
        }

        class ListGroup
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private void LoadGroups()
        {
            dgvAllGroups.DataSource = null;
            dgvAllGroups.Columns.Clear();
            dgvAllGroups.Rows.Clear();
            List<int> templatesId;
            List<int> groupsId;
            List<ListGroup> lstGroups;
            if (checkBox1.Checked)
            {
                templatesId = db.Templates.Where(a => a.YearId == null).Select(a => a.Id).ToList();
                groupsId = db.TemplateGroups.Where(a => templatesId.Contains(a.TemplateId)).Select(a => a.GroupId).ToList();
                lstGroups = db.Groups.Where(a => a.YearId == null && !groupsId.Contains(a.Id) && a.Activity.Name != "Закрытые").OrderBy(a => a.Name)
                    .Select(a => new ListGroup { Id = a.Id, Name = a.Name }).ToList();
            }
            else
            {
                int currentYearId = (int)cbYears.SelectedValue;
                templatesId = db.Templates.Where(a => a.YearId == currentYearId).Select(a => a.Id).ToList();
                groupsId = db.TemplateGroups.Where(a => templatesId.Contains(a.TemplateId)).Select(a => a.GroupId).ToList();
                lstGroups = db.Groups.Where(a => a.YearId == currentYearId && !groupsId.Contains(a.Id) && a.Activity.Name != "Закрытые").OrderBy(a => a.Name)
                    .Select(a => new ListGroup { Id = a.Id, Name = a.Name }).ToList();
            }

            dgvAllGroups.DataSource = lstGroups;
            dgvAllGroups.Columns["Id"].Visible = false;
            dgvAllGroups.Columns["Name"].HeaderText = "Группа";
            DataGridViewCheckBoxColumn check = new DataGridViewCheckBoxColumn();
            dgvAllGroups.Columns.Add(check);
            check.Name = "check";
            check.HeaderText = "";
            check.DisplayIndex = 0;
            check.Width = 40;
        }

        private void LoadTemplates()
        {
            int currentYearId;
            List<Template> lstTemplates;
            if (checkBox1.Checked)
            {
                lstTemplates = db.Templates.Where(a => a.YearId == null).OrderBy(a => a.Name).ToList();
            }
            else
            {
                currentYearId = (int)cbYears.SelectedValue;
                lstTemplates = db.Templates.Where(a => a.YearId == currentYearId).OrderBy(a => a.Name).ToList();
            }

            lbTemplates.DataSource = lstTemplates;
            lbTemplates.DisplayMember = "Name";
            lbTemplates.ValueMember = "Id";
        }

        private void btAddRow_Click(object sender, EventArgs e)
        {
            if (lbTemplates.SelectedIndex > -1)
            {
                var t = new TemplateRate();
                t.TemplateId = (int)lbTemplates.SelectedValue;
                t.CountStudents = (dgvWages.Rows.Count == 0) ? 5 : db.TemplateRates.Where(a => a.TemplateId == t.TemplateId).Max(a => a.CountStudents) + 1;
                var inputForm = new InputForm2(t);
                inputForm.ShowDialog();
                LoadTemplateRates((int)lbTemplates.SelectedValue);
                //lbTemplates.SelectedIndex = lbTemplates.Items.Count - 1;

                dgvWages.Rows[dgvWages.Rows.Count - 1].Selected = true;
                dgvWages.CurrentCell = dgvWages.Rows[dgvWages.Rows.Count - 1].Cells["Wage"];
            }
        }

        private void btRemoveRow_Click(object sender, EventArgs e)
        {
            if (dgvWages.CurrentCell != null)
            {
                int index = dgvWages.CurrentCell.RowIndex;
                int templateRateId = (int)dgvWages.Rows[index].Cells["Id"].Value;
                var templateRate = db.TemplateRates.Find(templateRateId);
                if (templateRate != null)
                {
                    db.TemplateRates.Remove(templateRate);
                    db.SaveChanges();
                    LoadTemplateRates((int)lbTemplates.SelectedValue);

                    if (index < dgvWages.Rows.Count)
                    {
                        dgvWages.Rows[index].Selected = true;
                        dgvWages.CurrentCell = dgvWages.Rows[index].Cells["Wage"];
                    }
                    else if (index == 0) { }
                    else
                    {
                        dgvWages.Rows[dgvWages.Rows.Count - 1].Selected = true;
                        dgvWages.CurrentCell = dgvWages.Rows[dgvWages.Rows.Count - 1].Cells["Wage"];
                    }
                }
                else MessageBox.Show("Ошибка удаления записи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTemplateRates(int templateId)
        {
            dgvWages.DataSource = null;
            dgvWages.Columns.Clear();
            dgvWages.Rows.Clear();
            var templateRates = db.TemplateRates.Where(a => a.TemplateId == templateId).OrderBy(a => a.CountStudents).ToList();
            if (templateRates.Count > 0)
            {
                dgvWages.DataSource = templateRates;
                dgvWages.Columns["Id"].Visible =
                    dgvWages.Columns["TemplateId"].Visible =
                    dgvWages.Columns["Template"].Visible = false;
                dgvWages.Columns["Wage"].HeaderText = "Оплата";
                dgvWages.Columns["CountStudents"].HeaderText = "Учащ.";
                dgvWages.Columns["CountStudents"].Width = 50;
            }
        }

        private void btEditRow_Click(object sender, EventArgs e)
        {
            if (dgvWages.CurrentCell != null)
            {
                int index = dgvWages.CurrentCell.RowIndex;
                int templateRateId = (int)dgvWages.Rows[index].Cells["Id"].Value;
                var templateRate = db.TemplateRates.Find(templateRateId);
                if (templateRate != null)
                {
                    var inputForm = new InputForm2(templateRate);
                    inputForm.ShowDialog();

                    LoadTemplateRates((int)lbTemplates.SelectedValue);

                    if (index < dgvWages.Rows.Count)
                    {
                        dgvWages.Rows[index].Selected = true;
                        dgvWages.CurrentCell = dgvWages.Rows[index].Cells["Wage"];
                    }
                    else if (index == 0) { }
                    else
                    {
                        dgvWages.Rows[dgvWages.Rows.Count - 1].Selected = true;
                        dgvWages.CurrentCell = dgvWages.Rows[dgvWages.Rows.Count - 1].Cells["Wage"];
                    }
                }
                else MessageBox.Show("Ошибка удаления записи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lbTemplates.SelectedIndex > -1)
            {
                foreach (DataGridViewRow row in dgvAllGroups.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["check"].Value))
                    {
                        var templateGroup = new TemplateGroup();
                        templateGroup.GroupId = (int)row.Cells["Id"].Value;
                        templateGroup.TemplateId = (int)lbTemplates.SelectedValue;
                        db.TemplateGroups.Add(templateGroup);
                    }
                }
                db.SaveChanges();
                LoadTemplateGroups((int)lbTemplates.SelectedValue);
                LoadGroups();
            }
        }

        private void lbTemplates_Click(object sender, EventArgs e)
        {
            if (lbTemplates.Items.Count != 0)
            {
                LoadTemplateRates((int)lbTemplates.SelectedValue);
                LoadTemplateGroups((int)lbTemplates.SelectedValue);
            }
        }

        private void LoadTemplateGroups(int templateId)
        {
            dgvListGroups.DataSource = null;
            dgvListGroups.Columns.Clear();
            dgvListGroups.Rows.Clear();
            var templateGroups = db.TemplateGroups.Where(a => a.TemplateId == templateId).Include(a => a.Group).Select(a => new { a.Id, a.Group.Name, }).ToList();
            if (templateGroups.Count > 0)
            {
                dgvListGroups.DataSource = templateGroups;
                dgvListGroups.Columns["Id"].Visible = false;
                dgvListGroups.Columns["Name"].HeaderText = "Группы";
                dgvListGroups.Columns["Name"].ReadOnly = true;
                DataGridViewCheckBoxColumn check = new DataGridViewCheckBoxColumn();
                dgvListGroups.Columns.Add(check);
                check.Name = "check";
                check.HeaderText = "";
                check.DisplayIndex = 0;
                check.Width = 40;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvListGroups.Rows.Count > 0 && lbTemplates.SelectedIndex > -1)
            {
                foreach (DataGridViewRow row in dgvListGroups.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["check"].Value))
                    {
                        var templateGroup = db.TemplateGroups.Find((int)row.Cells["Id"].Value);
                        db.TemplateGroups.Remove(templateGroup);
                    }
                }
                db.SaveChanges();
                LoadTemplateGroups((int)lbTemplates.SelectedValue);
                LoadGroups();
            }
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            dateStart = dtpDateBeginR.Value = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, 1);
            dateEnd = dtpStart.Value.AddMonths(1).AddDays(-1);
            if (!notFiltering)
                Filter(dateStart.Date, (int?)cbTeachers.SelectedValue);
            FilterRetention(dateStart.Date, (int?)cbTeachers.SelectedValue);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                cbYears.SelectedIndex = -1;
                cbYears.Enabled = false;

                LoadTemplates();
                LoadGroups();
                if (lbTemplates.Items.Count > 0)
                    LoadTemplateRates((int)lbTemplates.SelectedValue);
                else dgvWages.DataSource = null;
                if (lbTemplates.Items.Count > 0)
                    LoadTemplateGroups((int)lbTemplates.SelectedValue);
                else
                {
                    dgvListGroups.DataSource = null;
                    dgvListGroups.Columns.Clear();
                    dgvListGroups.Rows.Clear();
                }
            }
            else
            {
                cbYears.Enabled = true;
                //выбор последнего учебного года
                var maxYear = db.Years.Max(a => a.Id);
                cbYears.SelectedValue = maxYear;

                LoadTemplates();
                LoadGroups();
                if (lbTemplates.Items.Count > 0)
                    LoadTemplateRates((int)lbTemplates.SelectedValue);
                else dgvWages.DataSource = null;
                if (lbTemplates.Items.Count > 0)
                    LoadTemplateGroups((int)lbTemplates.SelectedValue);
                else
                {
                    dgvListGroups.DataSource = null;
                    dgvListGroups.Columns.Clear();
                    dgvListGroups.Rows.Clear();
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            var retentionForm = new RetentionForm(null);
            retentionForm.ShowDialog();
            FilterRetention(dateStart, (int?)cbTeacherR.SelectedValue);
        }

        private void dgvRetention_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvTransaction.CurrentCell != null)
            {
                var index = dgvTransaction.CurrentCell.RowIndex;
                var idRetention = (int)dgvTransaction["Id", index].Value;

                var retentionForm = new RetentionForm(idRetention);
                retentionForm.ShowDialog();
                FilterRetention(dateStart, (int?)cbTeacherR.SelectedValue);
            }
            else
                MessageBox.Show("Нет выбранных ячеек", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            if (dgvTransaction.CurrentCell != null)
            {
                var dr = MessageBox.Show("Вы уверены, что хотите удалить строку?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    var index = dgvTransaction.CurrentCell.RowIndex;
                    var idRetention = (int)dgvTransaction["Id", index].Value;
                    var retention = db.Retentions.Find(idRetention);
                    db.Retentions.Remove(retention);
                    int result = db.SaveChanges();
                    FilterRetention(dateStart, (int?)cbTeacherR.SelectedValue);
                }
            }
            else
                MessageBox.Show("Нет выбранных ячеек", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btClearR_Click(object sender, EventArgs e)
        {
            //установка периода на прошлый месяц
            dtpStart.Value = DateTime.Now.AddMonths(-1);
            dateStart = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, 1);

            cbTeacherR.SelectedIndex = -1;
            cbTeachers.SelectedIndex = -1;
            Filter(dateStart, null);
            FilterRetention(dateStart, null);
        }

        private void cbTeacherR_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FilterRetention(dateStart, (int?)cbTeacherR.SelectedValue);
        }

        private void dtpDateEndR_ValueChanged(object sender, EventArgs e)
        {
            FilterRetention(dateStart, (int?)cbTeacherR.SelectedValue);
        }

        private void dtpDateBeginR_ValueChanged(object sender, EventArgs e)
        {
            dateStart = dtpStart.Value = dtpMonth.Value = new DateTime(dtpDateBeginR.Value.Year, dtpDateBeginR.Value.Month, 1);
            dateEnd = dtpDateBeginR.Value.AddMonths(1).AddDays(-1);
            FilterRetention(dateStart, (int?)cbTeacherR.SelectedValue);
        }

        private void cbDepartments_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FilterWorkers(cbDepartments.Text);
        }

        private void btClean_Click(object sender, EventArgs e)
        {
            //установка периода на прошлый месяц
            dtpStart.Value = DateTime.Now.AddMonths(-1);
            dateStart = new DateTime(dtpStart.Value.Year, dtpStart.Value.Month, 1);
            dateEnd = dtpStart.Value.AddMonths(1).AddDays(-1);

            cbTeachers.SelectedIndex = -1;
            Filter(dateStart, null);
        }

        private void dgvListWorkers_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dateStart = dtpStart.Value = dtpDateBeginR.Value = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
            dateEnd = dateStart.AddMonths(1);

            int indexRow = dgvListWorkers.CurrentCell.RowIndex;
            int idWorker = Convert.ToInt32(dgvListWorkers["Id", indexRow].Value);

            GetTransactionsList(dateStart, idWorker);
        }

        private void GetTransactionsList(DateTime period, int idWorker)
        {
            decimal totalProfit = GetTransactions(period, idWorker, "Начисление");
            label12.Text = $"Всего начислено: {totalProfit.ToString("C", CultureInfo.CurrentCulture)}";

            decimal totalRetention = GetTransactions(period, idWorker, "Удержание");
            label15.Text = $"Всего удержано: {totalRetention.ToString("C", CultureInfo.CurrentCulture)}";

            decimal totalPays = GetTransactions(period, idWorker, "Выплата");
            label20.Text = $"Всего выплачено: {totalPays.ToString("C", CultureInfo.CurrentCulture)}";

            var report = new Report();
            decimal balance = report.Balance(period, idWorker);
            label13.Text = $"Долг предприятия на начало: {balance.ToString("C", CultureInfo.CurrentCulture)}";

            label14.Text = $"Долг предприятия на конец: {(totalProfit - totalRetention - totalPays + balance).ToString("C", CultureInfo.CurrentCulture)}";
        }

        private void btProfit_Click(object sender, EventArgs e)
        {
            var currentPeriod = dtpStart.Value;
            if (cbTeachers.SelectedIndex == -1)
            {
                var dr = MessageBox.Show($"Вы уверены, что хотите произвести начисления для всех преподавателей за период  \"{currentPeriod.ToString("MMM/yy")}\"?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    //получение списка ид преподавателей за данный период
                    var idTeachers = listLessons.Select(a => a.TeacherId).Distinct().ToList();
                    foreach (var item in idTeachers)
                    {
                        ProfitTeacherHours(item, dtpStart.Value);
                    }
                }
            }
            else
            {
                var dr = MessageBox.Show($"Вы уверены, что хотите произвести начисления для преподавателя \"{cbTeachers.Text}\" за период  \"{currentPeriod.ToString("MMM/yy")}\"?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    ProfitTeacherHours((int)cbTeachers.SelectedValue, dtpStart.Value);
                }
            }
        }

        private void ProfitTeacherHours(int idTeacher, DateTime period)
        {
            //получение крайних дат периода 
            var dateStart = new DateTime(period.Year, period.Month, 1);

            //проверка существуют ли начисления для преподавателей за данный период
            var checkProfitTeacher = db.Retentions.Where(a => a.WorkerId == idTeacher && a.Month >= dateStart && a.Month < dateEnd && a.Name != null).ToList();
            if (checkProfitTeacher.Count > 0)
            {
                var currentTeacher = db.Workers.FirstOrDefault(a => a.Id == idTeacher);
                var dr = MessageBox.Show($"Начисления для {currentTeacher.LastnameFM} за данный период уже внесены. Перезаписать начисления?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    db.Retentions.RemoveRange(checkProfitTeacher);
                }
                else return;
            }

            //получение списка проведенных занятий
            var lst = Filter(dateStart.Date, idTeacher);

            //итоговые суммы проведенных занятий
            List<ListTransactions> transactions = new List<ListTransactions>();
            transactions.AddRange((from l in lst
                                   group l by new { l.GroupName, l.Students, l.Wage } into g
                                   orderby g.Key.GroupName
                                   select new ListTransactions
                                   {
                                       Name = g.Key.GroupName,
                                       Period = dateStart,
                                       Count = g.Key.Students,
                                       Hours = g.Sum(a => a.DurationLesson),
                                       Wage = g.Key.Wage,
                                       Total = g.Sum(a => a.DurationLesson) * g.Key.Wage
                                   }).ToList());

            //добавление начислений для преподавателя
            foreach (var item in transactions)
            {
                db.Retentions.Add(new Retention
                {
                    WorkerId = idTeacher,
                    Date = DateTime.Now,
                    Month = dateStart,
                    BaseId = null,
                    TypeId = 2,
                    Name = $"Группа {item.Name}",
                    Count = item.Count,
                    Hours = item.Hours,
                    Wage = item.Wage,
                    Value = item.Total,
                });
            }
            db.SaveChangesAsync();
        }

        private void dtpMonth_ValueChanged(object sender, EventArgs e)
        {
            //определение периода начислений
            dateStart = dtpStart.Value = dtpDateBeginR.Value = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
            dateEnd = dateStart.AddMonths(1);
            if (dgvListWorkers.CurrentCell != null)
            {
                int indexRow = dgvListWorkers.CurrentCell.RowIndex;
                int idWorker = Convert.ToInt32(dgvListWorkers["Id", indexRow].Value);

                GetTransactionsList(dateStart, idWorker);
            }
        }

        private void экспортВExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] workersId = new int[1];
            if (dgvListWorkers.CurrentCell != null)
            {
                int indexRow = dgvListWorkers.CurrentCell.RowIndex;
                workersId[0] = Convert.ToInt32(dgvListWorkers["Id", indexRow].Value);
            }
            var report = new Report();
            report.PaySheet(workersId, dtpMonth.Value);
        }

        private void подробныйСписокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cbDepartments.Text == "Все")
            {
                MessageBox.Show("Не выбрано подразделение для печати расчетных листков", "Внимание", MessageBoxButtons.OK);
                return;
            }

            //получаем список сотрудников подразделения с начислениями за текущий период для печати расчетных листов
            dateStart = dtpStart.Value = dtpDateBeginR.Value = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
            dateEnd = dateStart.AddMonths(1);
            int idDepartment = (int)cbDepartments.SelectedValue;

            var lstWorkersAndBalance = (from workers in db.Workers
                                        where workers.DepartmentId == idDepartment
                                        select new ListWorkersAndBalance
                                        {
                                            WorkerId = workers.Id,
                                            Profit = db.Retentions.Where(a => a.Month < dateEnd && a.WorkerId == workers.Id
                                                && a.TypeId == 2).Select(a => a.Value).DefaultIfEmpty(0).Sum(),
                                            Retention = db.Retentions.Where(a => a.Month < dateEnd
                                                    && a.WorkerId == workers.Id && a.TypeId == 1).Select(a => a.Value).DefaultIfEmpty(0).Sum(),
                                            Payment = db.Retentions.Where(a => a.Month < dateEnd
                                                    && a.WorkerId == workers.Id && a.TypeId == 3).Select(a => a.Value).DefaultIfEmpty(0).Sum(),
                                            CurrentMonthTransactions = db.Retentions.Where(a => a.Month >= dateStart && a.Month < dateEnd && a.WorkerId == workers.Id).Count()
                                        }).ToList();
            var workersId = lstWorkersAndBalance.Where(a => a.Balance > 0 || a.CurrentMonthTransactions != 0).Select(a => a.WorkerId).Distinct().ToArray();

            var report = new Report();
            report.PaySheet(workersId, dateStart);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 3)
            {
                if (dgvListWorkers.CurrentCell != null)
                {                    
                    int indexRow = dgvListWorkers.CurrentCell.RowIndex;
                    int idWorker = Convert.ToInt32(dgvListWorkers["Id", indexRow].Value);

                    GetTransactionsList(dateStart, idWorker);
                }
            }
        }

        private void всеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //получаем список всех сотрудников с начислениями за текущий период для печати расчетных листов
            dateStart = dtpStart.Value = dtpDateBeginR.Value = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
            dateEnd = dateStart.AddMonths(1);

            var dateBegin = new DateTime(1900, 1, 1);
            var lstWorkersAndBalance = (from workers in db.Workers
                    select new ListWorkersAndBalance
                    {
                        WorkerId = workers.Id,
                        Profit = db.Retentions.Where(a => a.Month < dateEnd && a.WorkerId == workers.Id
                            && a.TypeId == 2).Select(a => a.Value).DefaultIfEmpty(0).Sum(),
                        Retention = db.Retentions.Where(a => a.Month < dateEnd
                                && a.WorkerId == workers.Id && a.TypeId == 1).Select(a => a.Value).DefaultIfEmpty(0).Sum(),
                        Payment = db.Retentions.Where(a => a.Month < dateEnd
                                && a.WorkerId == workers.Id && a.TypeId == 3).Select(a => a.Value).DefaultIfEmpty(0).Sum(),
                        CurrentMonthTransactions = db.Retentions.Where(a => a.Month >= dateStart && a.Month < dateEnd && a.WorkerId == workers.Id).Count()
                    }).ToList();
            var workersId = lstWorkersAndBalance.Where(a => a.Balance > 0 || a.CurrentMonthTransactions != 0).Select(a => a.WorkerId).Distinct().ToArray();

            var report = new Report();
            report.PaySheet(workersId, dateStart);
        }

        private void btCalculation_Click(object sender, EventArgs e)
        {
            if (dgvListLessons.Rows.Count != 0)
            {
                var exportLessons = new Report();
                exportLessons.ExportExcelLessons(dgvListLessons, CurrentSession.CurrentRole.Name);
            }
        }

        private decimal GetTransactions(DateTime date, int? worker, string type)
        {
            var transactions = new List<ListTransactions>();

            //получение операций
            if (worker != null)
            {
                transactions.AddRange(db.Retentions.Include(a => a.Base).Include(a => a.TypeOfTransaction).Where(a => a.Month >= dateStart && a.Month < dateEnd
                        && a.WorkerId == worker && a.TypeOfTransaction.Name == type).Select(a => new
                        ListTransactions
                        {
                            Name = a.BaseId != null ? a.Base.Name : a.Name,
                            Date = a.Date,
                            Period = dateStart,
                            Count = a.Count != null ? a.Count : null,
                            Hours = a.Hours != null ? a.Hours : null,
                            Wage = a.Wage != null ? a.Wage : null,
                            Total = a.Value
                        }).ToList());
            }
            else
            {
                transactions.AddRange(db.Retentions.Include(a => a.Base).Include(a => a.TypeOfTransaction).Where(a => a.Month >= dateStart && a.Month < dateEnd
                        && a.TypeOfTransaction.Name == type).Select(a => new
                        ListTransactions
                        {
                            Name = a.BaseId != null ? a.Base.Name : a.Name,
                            Date = a.Date,
                            Period = dateStart,
                            Count = a.Count != null ? a.Count : null,
                            Hours = a.Hours != null ? a.Hours : null,
                            Wage = a.Wage != null ? a.Wage : null,
                            Total = a.Value
                        }).ToList());
            }

            switch (type)
            {
                case "Удержание":
                    dgvRetentions.DataSource = transactions;
                    dgvRetentions.Columns["Name"].HeaderText = "Наименование";
                    dgvRetentions.Columns["Date"].Visible = false;
                    dgvRetentions.Columns["Period"].HeaderText = "Период";
                    dgvRetentions.Columns["Period"].DefaultCellStyle.Format = "MMM/yy";
                    dgvRetentions.Columns["Count"].Visible = false;
                    dgvRetentions.Columns["Hours"].Visible = false;
                    dgvRetentions.Columns["Wage"].Visible = false;
                    dgvRetentions.Columns["Total"].HeaderText = "Сумма";
                    dgvRetentions.Columns["Total"].DefaultCellStyle.Format = "0.00";
                    break;
                case "Начисление":
                    dgvProfits.DataSource = transactions;
                    dgvProfits.Columns["Name"].HeaderText = "Наименование";
                    dgvProfits.Columns["Date"].Visible = false;
                    dgvProfits.Columns["Period"].HeaderText = "Период";
                    dgvProfits.Columns["Period"].DefaultCellStyle.Format = "MMM/yy";
                    dgvProfits.Columns["Count"].HeaderText = "Кол-во";
                    dgvProfits.Columns["Hours"].HeaderText = "Часов";
                    dgvProfits.Columns["Wage"].HeaderText = "Ставка";
                    dgvProfits.Columns["Wage"].DefaultCellStyle.Format = "0.00";
                    dgvProfits.Columns["Total"].HeaderText = "Сумма";
                    dgvProfits.Columns["Total"].DefaultCellStyle.Format = "0.00";
                    break;
                case "Выплата":
                    dgvPays.DataSource = transactions;
                    dgvPays.Columns["Name"].HeaderText = "Наименование";
                    dgvPays.Columns["Period"].HeaderText = "Период";
                    dgvPays.Columns["Period"].DefaultCellStyle.Format = "MMM/yy";
                    dgvPays.Columns["Date"].HeaderText = "Дата";
                    dgvPays.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvPays.Columns["Count"].Visible = false;
                    dgvPays.Columns["Hours"].Visible = false;
                    dgvPays.Columns["Wage"].Visible = false;
                    dgvPays.Columns["Total"].HeaderText = "Сумма";
                    dgvPays.Columns["Total"].DefaultCellStyle.Format = "0.00";
                    break;
            }
            return transactions.Sum(a => a.Total);
        }

        //private decimal Balance(DateTime date, int? worker)
        //{
        //    dateStart = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
        //    //определение периода начислений
        //    //определяем дату первого начисления
        //    DateTime dateBegin;
        //    if (worker == null)
        //        dateBegin = db.Retentions.OrderBy(a => a.Month).Select(a => a.Month).FirstOrDefault();
        //    else
        //        dateBegin = db.Retentions.Where(a => a.WorkerId == worker).OrderBy(a => a.Month).Select(a => a.Month).FirstOrDefault();

        //    //получение суммы начислений за период до начала отчетного
        //    decimal profit;
        //    if (worker == null)
        //        profit = db.Retentions.Include(a => a.TypeOfTransaction).Where(a => a.Month >= dateBegin
        //        && a.Month < dateStart && a.TypeOfTransaction.Name == "Начисление").Select(a => a.Value).DefaultIfEmpty(0).Sum();
        //    else
        //        profit = db.Retentions.Include(a => a.TypeOfTransaction).Where(a => a.Month >= dateBegin
        //        && a.Month < dateStart && a.WorkerId == worker && a.TypeOfTransaction.Name == "Начисление").Select(a => a.Value).DefaultIfEmpty(0).Sum();

        //    //получение суммы удержаний за период до начала отчетного
        //    decimal retention;
        //    if (worker == null)
        //        retention = db.Retentions.Include(a => a.TypeOfTransaction).Where(a => a.Month >= dateBegin
        //        && a.Month < dateStart && a.TypeOfTransaction.Name == "Удержание").Select(a => a.Value).DefaultIfEmpty(0).Sum();
        //    else
        //        retention = db.Retentions.Include(a => a.TypeOfTransaction).Where(a => a.Month >= dateBegin && a.Month < dateStart
        //        && a.WorkerId == worker && a.TypeOfTransaction.Name == "Удержание").Select(a => a.Value).DefaultIfEmpty(0).Sum();

        //    //получение суммы выплат за период до начала отчетного
        //    decimal payment;
        //    if (worker == null)
        //        payment = db.Retentions.Include(a => a.TypeOfTransaction).Where(a => a.Month >= dateBegin
        //        && a.Month < dateStart && a.TypeOfTransaction.Name == "Выплата").Select(a => a.Value).DefaultIfEmpty(0).Sum();
        //    else
        //        payment = db.Retentions.Include(a => a.TypeOfTransaction).Where(a => a.Month >= dateBegin && a.Month < dateStart
        //        && a.WorkerId == worker && a.TypeOfTransaction.Name == "Выплата").Select(a => a.Value).DefaultIfEmpty(0).Sum();

        //    return profit - retention - payment;
        //}
    }
}
