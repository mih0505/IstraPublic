using Istra.Documents;
using Istra.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;


namespace Istra
{
    public partial class GroupForm : Form
    {
        private struct InvalidStudent
        {
            public Student Student { get; set; }
            public List<Lesson> Lessons { get; set; }
        }

        public bool generateNewScheduler = false;
        List<CheckBox> week;//список chekbox'ов для дней недели
        public List<ListPeriods> listPeriod;
        IstraContext db = new IstraContext();
        public Group group;
        int countLessonLearned;//количество проведенных занятий
        private Enrollment lastSelectedEnrollment;
        private bool isDocumentNumberNotSaved, isFirstNumberDocumentNotSaved;

        public GroupForm(Group gr)
        {
            InitializeComponent();

            if (gr != null)
            {
                group = gr;
                this.Text = "Группа: " + gr.Name;
            }
            else
            {
                group = new Group();
                this.Text = "Создать группу";
            }
            try
            {
                //заполнение направлений
                var directions = db.Directions.ToList();
                cbDirection.DataSource = directions;
                cbDirection.DisplayMember = "Name";
                cbDirection.ValueMember = "Id";
                cbDirection.SelectedIndex = -1;

                //заполнение списка учебного года
                var years = db.Years.OrderBy(a => a.SortIndex).ToList();
                cbYear.DataSource = years;
                cbYear.DisplayMember = "Name";
                cbYear.ValueMember = "Id";
                //выбор последнего учебного года
                var maxYear = db.Years.Max(a => a.Id);
                cbYear.SelectedValue = maxYear;

                //заполнение корпусов
                var housings = db.Housings.ToList();
                cbHousing.DataSource = housings;
                cbHousing.DisplayMember = "Name";
                cbHousing.ValueMember = "Id";
                cbHousing.SelectedIndex = -1;

                //заполнение  преподавателей
                var teachers = db.Workers
                    .Where(a => a.Role.Name == "Преподаватель" && a.IsRemoved == false)
                    .OrderBy(a => a.Lastname)
                    .ThenBy(a => a.Firstname)                    
                    .ToList();
                cbTeacher.DataSource = teachers;
                cbTeacher.DisplayMember = "LastnameFM";
                cbTeacher.ValueMember = "Id";
                cbTeacher.SelectedValue = 40;

                //заполнение статусов групп
                cbActivity.DataSource = db.ActivityGroups.Where(a => a.Name != "Активные").ToList();
                cbActivity.DisplayMember = "Name";
                cbActivity.ValueMember = "Id";
                cbActivity.SelectedIndex = 2;

                //создание списка checkbox'ов для дней недели
                week = new List<CheckBox>();
                week.Add(chkbMon);
                week.Add(chkbTue);
                week.Add(chkbWed);
                week.Add(chkbThu);
                week.Add(chkbFri);
                week.Add(chkbSat);
                week.Add(chkbSun);

                //заполнение типов документов
                var types = db.Documents;
                cbDocType.DataSource = types.ToList();
                cbDocType.DisplayMember = "Name";
                cbDocType.ValueMember = "Id";
                cbDocType.SelectedIndex = -1;

                tbUnvisibleLessons.Text = group.UnvisibleLessons.ToString();

                //настройка таблицы с графиком платежей
                dgvPayments.Columns["DatePay"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            catch (Exception ex)
            {
                var m = new StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectDirection(int? directionId)
        {
            try
            {
                if (directionId != null)
                {
                    var courses = db.Courses.Where(a => a.DirectionId == directionId).ToList();
                    cbCourse.DataSource = courses;
                    cbCourse.DisplayMember = "Name";
                    cbCourse.ValueMember = "Id";
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

        private void SelectHousing(int? housingId)
        {
            try
            {
                if (housingId != null)
                {
                    var classes = db.Classes.Where(a => a.HousingId == housingId).ToList();
                    cbClass.DataSource = classes;
                    cbClass.DisplayMember = "Name";
                    cbClass.ValueMember = "Id";
                }
            }
            catch (Exception ex)
            {
                var m = new StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GroupForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (group.Id != 0)
                {
                    CurrentSession.GroupId = group.Id;

                    //загрузка данных
                    var currentDirection = db.Courses.FirstOrDefault(a => a.Id == group.CourseId);
                    cbDirection.SelectedValue = currentDirection.DirectionId;
                    SelectDirection(currentDirection.DirectionId);
                    cbCourse.SelectedValue = group.CourseId;

                    tbName.Text = group.Name;
                    if (group.YearId != null)
                        cbYear.SelectedValue = group.YearId;

                    nudDurationLesson.Value = group.DurationLesson;
                    nudDurationCourse.Value = group.DurationCourse;

                    if (group.DurationLesson != 0)
                        nudCountLessons.Value = Convert.ToInt32(group.DurationCourse / group.DurationLesson);
                    else nudCountLessons.Value = 0;

                    dtpDateBegin.Value = group.Begin;
                    cbBeginHours.SelectedItem = (group.Begin.Hour < 10) ? "0" + group.Begin.Hour.ToString() : group.Begin.Hour.ToString();


                    if (group.Begin.Minute == 0)
                        cbBeginMinutes.SelectedItem = "00";
                    else
                        cbBeginMinutes.SelectedItem = group.Begin.Minute.ToString();

                    var currentHousing = db.Classes.FirstOrDefault(a => a.Id == group.ClassId);
                    if (currentHousing != null)
                    {
                        cbHousing.SelectedValue = currentHousing.HousingId;
                        SelectHousing(currentHousing.HousingId);
                    }
                    else
                    {
                        cbHousing.SelectedIndex = -1;
                    }

                    cbClass.SelectedValue = group.ClassId;
                    cbTeacher.SelectedValue = group.TeacherId;
                    //заполнение дней недели
                    string[] days = group.Days.Split(new Char[] { ',' });
                    foreach (var day in days)
                    {
                        foreach (var chkb in week)
                        {
                            if (day == chkb.Text) chkb.Checked = true;
                        }
                    }
                    //два преподавателя
                    chbTwoTeachers.Checked = group.TwoTeachers;

                    //заполнение статуса группы
                    cbActivity.SelectedValue = group.ActivityId;

                    //заполнение примечания
                    rtbNote.Text = group.Note;

                    //------заполнение журнала--------------
                    tbUnvisibleLessons.Text = group.UnvisibleLessons.ToString();
                    GenerateJournal(group);
                    UnvisiblingLessons((int)group.UnvisibleLessons);

                    //-----заполнение вкладки платежи--------                  
                    LoadGeneratePayments(group.Id);
                    //заполнение графика
                    int i = 1;
                    var listSched = db.Schedules.Where(a => a.GroupId == group.Id && a.Source == 2 && a.EnrollmentId == null)
                        .Select(a => new { Id = a.Id, DatePay = a.DateBegin, Pay = a.Value }).OrderBy(a => a.DatePay).ToList();
                    double summ = 0;
                    if (listSched.Count > 0)
                    {
                        foreach (var a in listSched)
                        {
                            dgvPayments.Rows.Add(a.Id, i, a.DatePay, a.Pay);
                            summ += a.Pay;
                            i++;
                        }
                        label21.Text = summ.ToString("C", CultureInfo.CurrentCulture);
                    }
                    //вычисление сальдо
                    //получение записей (студентов) для группы
                    var ges = db.Enrollments.Where(a => a.GroupId == group.Id && a.DateExclusion == null).ToList();
                    double balance = Balance(ges, group.Id);
                    label22.Text = balance.ToString("C", CultureInfo.CurrentCulture);
                    if (balance >= 0) label22.ForeColor = Color.Green; else label22.ForeColor = Color.Red;

                    //-----заполнение вкладки Свидетельство об обучении--------  
                    LoadSectionsList();

                    var selectedType = db.Documents.ToList().FirstOrDefault(Document => (group.DocumentId == Document.Id));
                    cbDocType.SelectedItem = selectedType;

                    var students = db.Enrollments.Include("Student").Where(Enrollment => (Enrollment.GroupId == group.Id)).Select(Enrollment => (Enrollment.Student)).OrderBy(Student => (Student.Lastname));
                    cbStudents.DataSource = students.ToList();
                    cbStudents.DisplayMember = "LastnameFM";
                    cbStudents.ValueMember = "Id";

                    tbProtocolNo.Text = group.NumberProtocol;
                    tbMaxPoint.Text = group.TotalPoint.ToString();

                    //---------------------------------------------------------------------------
                    //------------------распределение прав пользователя--------------------------
                    //---------------------------------------------------------------------------
                    int idRole = CurrentSession.CurrentRole.Id;
                    var lstManageRole = db.ManageRoles.Include(a => a.Permission).Where(a => a.RoleId == idRole).ToList();

                    if (!lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "SchedulerPayGroup").Value)
                        this.tabPage3.Parent = null;
                    if (!lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "PrintDoc").Value)
                        this.tabPage4.Parent = null;

                    //блокировка контролов на вкладке 
                    if (!lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "EditGroup").Value)
                    {
                        var dg = !lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "DetailsGroup").Value;
                        var cbs = (from controls in tabPage1.Controls.OfType<ComboBox>()
                                   select controls).ToList();

                        foreach (var cb in cbs)
                            cb.Enabled = dg;
                        foreach (var chkb in week)
                            chkb.Enabled = dg;

                        tbName.Enabled = chbTwoTeachers.Enabled = nudDurationLesson.Enabled = nudCountLessons.Enabled = nudDurationCourse.Enabled =
                            label22.Visible = label17.Visible = dtpDateBegin.Enabled = rtbNote.Enabled = dg;
                    }
                    //доступ к кнопкам в журнале
                    if (lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "Journal").Value)
                    {
                        sbAddStudent.Enabled = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "EnrollInGroup").Value;
                        tsbDeleteStudent.Enabled = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "ExclusionFromGroup").Value;
                        tsbAddLesson.Enabled = tsbEditLesson.Enabled = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "AddLesson").Value;
                        tsbDeleteLesson.Enabled = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "DeleteLesson").Value;
                    }
                    else
                    {
                        if (!lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "Journal").Value)
                            this.tabPage2.Parent = null;
                    }




                    btSave.Enabled = btSaveAndClose.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                var m = new StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private double Balance(List<Enrollment> ges, int id)
        {
            double sum = 0, sumPays = 0;
            try
            {
                //получение суммы платежей по активным группам                
                var listId = ges.Select(r => r.Id).ToList();
                sumPays = db.Payments.Where(r => listId.Contains(r.EnrollmentId) && r.IsDeleted == false && r.AdditionalPay == false).Select(l => l.ValuePayment).DefaultIfEmpty(0).Sum();

                //получение суммы платежей по графику на текущую дату
                sum = db.Schedules.Where(a => listId.Contains((int)a.EnrollmentId) && a.GroupId == id && a.Source == 2 && a.EnrollmentId != null && a.DateBegin <= DateTime.Now)
                               .Sum(o => (double?)(o.Value - o.Discount)) ?? 0;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sumPays - sum;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chkbYear_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkbYear.Checked)
            //    cbYear.Enabled = false;
            //else
            //    cbYear.Enabled = true;
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void nudDurationLesson_ValueChanged(object sender, EventArgs e)
        {
            if (nudCountLessons.Value != 0)
            {
                nudDurationCourse.Value = nudDurationLesson.Value * nudCountLessons.Value;
                btSave.Enabled = btSaveAndClose.Enabled = true;
            }
        }

        private void nudCountLessons_ValueChanged(object sender, EventArgs e)
        {
            if (nudDurationLesson.Value != 0)
            {
                nudDurationCourse.Value = nudDurationLesson.Value * nudCountLessons.Value;
                btSave.Enabled = btSaveAndClose.Enabled = true;
            }
        }

        private void cbDirection_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SelectDirection((int)cbDirection.SelectedValue);
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void cbHousing_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SelectHousing((int)cbHousing.SelectedValue);
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void btSaveAndClose_Click(object sender, EventArgs e)
        {
            SaveGroup();
            SaveSchedulerGroup();
            SaveJournal();
            btSave.Enabled = btSaveAndClose.Enabled = false;
            Close();
        }

        private void UnvisiblingLessons(int count)
        {
            int k = 4;//незатрагиваемые столбцы
            for (int i = k; i < dgvJournal.Columns.Count; i++)
            {
                if ((i - k) < count)
                    dgvJournal.Columns[i].Visible = false;
                else
                    dgvJournal.Columns[i].Visible = true;
            }
            dgvJournal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvJournal.ColumnHeadersHeight = 80;

        }

        private bool SaveGroup()
        {
            //сохранение на вкладке общая
            if (cbDirection.SelectedIndex == -1 || cbCourse.SelectedIndex == -1 || tbName.Text == ""
                || cbYear.SelectedIndex == -1 || cbActivity.SelectedIndex == -1)
            {
                MessageBox.Show("Для сохранения должны быть заполнены следующие поля: \r\n Направление;\r\n Курс\r\n Название группы\r\n" +
                      " Учебный год\r\n Статус группы", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                //обязательные поля
                group.Name = tbName.Text;
                group.ActivityId = Convert.ToInt32(cbActivity.SelectedValue);

                group.CourseId = Convert.ToInt32(cbCourse.SelectedValue);

                group.YearId = Convert.ToInt32(cbYear.SelectedValue);

                group.GroupCreatorId = CurrentSession.CurrentUser.Id;
                group.TeacherId = Convert.ToInt32(cbTeacher.SelectedValue);

                //сохранение не обязательных полей
                if (cbBeginHours.SelectedIndex != -1 && cbBeginMinutes.SelectedIndex != -1)
                {
                    group.Begin = new DateTime(dtpDateBegin.Value.Year, dtpDateBegin.Value.Month, dtpDateBegin.Value.Day,
                        Convert.ToInt32(cbBeginHours.Text), Convert.ToInt32(cbBeginMinutes.Text), 0);
                }
                else group.Begin = DateTime.Now.Date;

                group.TwoTeachers = chbTwoTeachers.Checked;
                if (cbClass.SelectedIndex != -1)
                    group.ClassId = Convert.ToInt32(cbClass.SelectedValue);

                //сохранение данных о докуметах
                group.TotalPoint = (tbMaxPoint.Text != String.Empty) ? (int?)Convert.ToInt32(tbMaxPoint.Text) : null;
                group.DocumentId = Convert.ToInt32(cbDocType.SelectedValue);

                if (tbProtocolNo.Text != String.Empty)
                    group.NumberProtocol = tbProtocolNo.Text;

                if (tbDocNo.Text != String.Empty)
                {
                    lastSelectedEnrollment.NumberDocument = tbDocNo.Text;
                    db.Entry(lastSelectedEnrollment).State = EntityState.Modified;
                    isFirstNumberDocumentNotSaved = isDocumentNumberNotSaved = false;
                }

                group.Days = String.Empty;
                foreach (var chkb in week)
                {
                    if (chkb.Checked)
                        if (group.Days != String.Empty)
                            group.Days += "," + chkb.Text;
                        else group.Days = chkb.Text;
                }

                group.DurationCourse = Convert.ToInt32(nudDurationCourse.Value);
                group.DurationLesson = Convert.ToByte(nudDurationLesson.Value);

                //сохранение примечания                
                group.Note = rtbNote.Text;


                //сохранение количества скрываемых занятий
                if (tbUnvisibleLessons.Text != "")
                {
                    if (group.UnvisibleLessons != Convert.ToInt32(tbUnvisibleLessons.Text))
                    {
                        group.UnvisibleLessons = Convert.ToInt32(tbUnvisibleLessons.Text);
                        UnvisiblingLessons((int)group.UnvisibleLessons);
                    }
                }
                else group.UnvisibleLessons = 0;

                try
                {
                    if (group.Id == 0)
                    {
                        db.Groups.Add(group);
                    }
                    if (group.Id != 0)
                    {
                        db.Entry(group).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    CurrentSession.GroupId = group.Id;

                    //добавление группы в список доступных, для сотрудников с ограничением просмотра групп
                    db.AccessGroups.Add(new AccessGroups
                    {
                        WorkerId = CurrentSession.CurrentUser.Id,
                        GroupId = group.Id
                    });

                    //сохранение на вкладке платежи
                    if (radioButton2.Checked)
                    {
                        var newSource = new Schedule
                        {
                            Source = 1,
                            DateBegin = dtpBeginPay.Value,
                            CountPayments = (int)nudCountPayments.Value,
                            Value = Convert.ToInt32(tbValueCourse.Text),
                            GroupId = group.Id
                        };

                        //сохранение периодов (1 способ) происходит автоматически, здесь сохранение при выборе способа 2
                        //проверка на существование записей в таблице графика платежей
                        var listPayments = db.Schedules.Where(a => a.Source == 1 && a.GroupId == group.Id).ToList();
                        if (listPayments.Count > 0)
                        {
                            if (listPayments.Count > 1)
                            {
                                //удаление периодов платежей и сохранение записи способа 2
                                db.Schedules.RemoveRange(listPayments);
                                db.Schedules.Add(newSource);
                            }
                            else
                            {
                                //проверка содержимого записи и обновление в случае необходимости
                                listPayments[0].DateBegin = newSource.DateBegin;
                                listPayments[0].DateEnd = null;
                                listPayments[0].CountPayments = newSource.CountPayments;
                                listPayments[0].Value = newSource.Value;
                                db.Entry(listPayments[0]).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            //добавление новой записи в таблицу
                            db.Schedules.Add(newSource);
                        }
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
                return true;
            }
        }

        private void SaveSchedulerGroup()
        {
            var listSched = new List<Schedule>();

            //если был сгенерирован новый график
            if (generateNewScheduler && dgvPayments.Rows.Count > 0)
            {
                try
                {
                    //проверка существования графика платежей созданного ранее
                    listSched = db.Schedules.Where(a => a.GroupId == group.Id && a.Source == 2 && a.EnrollmentId == null).ToList();
                    //удаление если существовал
                    if (listSched.Count > 0)
                    {
                        db.Schedules.RemoveRange(listSched);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    var m = new StackTrace(false).GetFrame(0).GetMethod();
                    string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                    CurrentSession.ReportError(methodName, ex.Message);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                generateNewScheduler = false;

                //создание нового графика
                listSched.Clear();
                for (int i = 0; i < dgvPayments.Rows.Count; i++)
                {
                    listSched.Add(new Schedule
                    {
                        GroupId = group.Id,
                        DateBegin = Convert.ToDateTime(dgvPayments.Rows[i].Cells["DatePay"].Value),
                        Source = 2,
                        Discount = 0,
                        Value = Convert.ToDouble(dgvPayments.Rows[i].Cells["Pay"].Value),
                        WorkerId = CurrentSession.CurrentUser.Id
                    });
                }
                try
                {
                    db.Schedules.AddRange(listSched);
                    db.SaveChanges();
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

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dgvPeriod.Enabled = btAddPeriod.Enabled = btEditPeriod.Enabled =
                btDeletePeriod.Enabled = false;
            tbValueCourse.Enabled = nudCountPayments.Enabled = dtpBeginPay.Enabled = true;
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dgvPeriod.Enabled = btAddPeriod.Enabled = btEditPeriod.Enabled =
                btDeletePeriod.Enabled = true;
            tbValueCourse.Enabled = nudCountPayments.Enabled = dtpBeginPay.Enabled = false;
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void LoadSectionsList()
        {
            try
            {
                dgvSections.DataSource = null;

                var sections = from section in db.Sections
                               join lesson in db.Lessons on section.LessonId equals lesson.Id
                               where (@group.Id == section.GroupId)
                               select new ListSections()
                               {
                                   Id = section.Id,
                                   Name = section.Name,
                                   Duration = section.Duration,
                                   Date = lesson.Date,
                                   IsCredit = section.IsCredit,
                                   IsTypeGrade = section.IsTypeGrade,
                                   CourseId = @group.CourseId,
                                   LessonId = lesson.Id,
                                   GroupId = section.GroupId
                               };

                dgvSections.DataSource = sections.OrderBy(Section => (Section.Id)).ToList();

                dgvSections.Columns["Id"].Visible
                    = dgvSections.Columns["GroupId"].Visible
                    = dgvSections.Columns["LessonId"].Visible
                    = dgvSections.Columns["Date"].Visible
                    = dgvSections.Columns["CourseId"].Visible
                    = dgvSections.Columns["IsCredit"].Visible
                    = dgvSections.Columns["IsTypeGrade"].Visible = false;

                dgvSections.Columns["Name"].HeaderText = "Название раздела";
                dgvSections.Columns["Duration"].HeaderText = "Объем";
                dgvSections.Columns["ShortDate"].HeaderText = "Дата";
                dgvSections.Columns["Credit"].HeaderText = "Зачет";
                dgvSections.Columns["TypeGrade"].HeaderText = "Оценка в протокол";

                label33.Text = $"кол-во: {sections.Count()}";
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadGeneratePayments(int groupId)
        {
            try
            {
                dgvPeriod.DataSource = null;
                dgvPeriod.Columns.Clear();
                dgvPeriod.Rows.Clear();

                listPeriod = (from schedule in db.Schedules
                              where schedule.Source == 1 &&
                              schedule.GroupId == groupId
                              select new ListPeriods
                              {
                                  Id = schedule.Id,
                                  DateBegin = schedule.DateBegin,
                                  DateEnd = schedule.DateEnd,
                                  Value = schedule.Value,
                                  CountPay = schedule.CountPayments
                              }).OrderBy(a => a.DateBegin).ToList();
                if (listPeriod.Count == 1 && listPeriod[0].DateEnd == null)
                {
                    radioButton2.Checked = true;
                    tbValueCourse.Text = listPeriod[0].Value.ToString();
                    nudCountPayments.Value = (int)listPeriod[0].CountPay;
                    dtpBeginPay.Value = listPeriod[0].DateBegin;
                }
                else
                {
                    radioButton1.Checked = true;
                    dgvPeriod.DataSource = listPeriod;
                    dgvPeriod.Columns["Id"].Visible = false;
                    dgvPeriod.Columns["DateBegin"].HeaderText = "Начало";
                    dgvPeriod.Columns["DateBegin"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvPeriod.Columns["DateEnd"].HeaderText = "Конец";
                    dgvPeriod.Columns["DateEnd"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgvPeriod.Columns["Value"].HeaderText = "Платеж, мес";
                    dgvPeriod.Columns["Value"].Width = 130;
                    dgvPeriod.Columns["CountPay"].Visible = false;
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

        private void btAddPeriod_Click(object sender, EventArgs e)
        {
            bool result = SaveGroup();
            if (result)
            {
                try
                {

                    //удаление данных о источнике платежей (способ 2) если он был создан

                    var p = db.Schedules.FirstOrDefault(a => a.GroupId == CurrentSession.GroupId && a.Source == 1 && a.DateEnd == null);
                    if (p != null)
                    {
                        db.Schedules.Remove(p);
                        db.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    var m = new StackTrace(false).GetFrame(0).GetMethod();
                    string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                    CurrentSession.ReportError(methodName, ex.Message);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //получение последней даты окончания платежей, для подстановки в форму добавления            
                var formGeneratePay = (dgvPeriod.Rows.Count == 0) ? new GeneratePayForm(null, null) :
                    new GeneratePayForm(null, listPeriod[listPeriod.Count - 1].DateEnd.Value);
                formGeneratePay.ShowDialog();
                //заполнение таблицы периодов
                LoadGeneratePayments(CurrentSession.GroupId);

                if (dgvPeriod.Rows.Count != 0)
                {
                    dgvPeriod.CurrentCell = dgvPeriod["DateBegin", dgvPeriod.Rows.Count - 1];
                    dgvPeriod["DateBegin", dgvPeriod.Rows.Count - 1].Selected = true;
                }
            }
        }

        private void cbCourse_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var currentCourse = db.Courses.Find(cbCourse.SelectedValue);
            if (currentCourse != null)
                cbDocType.SelectedValue = currentCourse.DocumentId;
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void btEditPeriod_Click(object sender, EventArgs e)
        {
            if (dgvPeriod.CurrentCell != null)
            {
                int index = dgvPeriod.CurrentCell.RowIndex;
                if (index != -1)
                {
                    int idRow = Convert.ToInt32(dgvPeriod.Rows[dgvPeriod.CurrentCell.RowIndex].Cells["Id"].Value);
                    var currentPeriod = db.Schedules.Find(idRow);

                    var formGeneratePay = new GeneratePayForm(currentPeriod, null);
                    formGeneratePay.ShowDialog();
                    LoadGeneratePayments(CurrentSession.GroupId);
                    if (dgvPeriod.Rows.Count != 0)
                    {
                        dgvPeriod.CurrentCell = dgvPeriod["DateBegin", index];
                        dgvPeriod["DateBegin", index].Selected = true;
                    }
                }
                else MessageBox.Show("Выберите нужный период для редактирования", "Внимание", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private void btDeletePeriod_Click(object sender, EventArgs e)
        {
            if (dgvPeriod.CurrentCell != null)
            {
                int index = dgvPeriod.CurrentCell.RowIndex;
                if (index != -1)
                {
                    int idRow = Convert.ToInt32(dgvPeriod.Rows[dgvPeriod.CurrentCell.RowIndex].Cells["Id"].Value);
                    var currentPeriod = db.Schedules.Find(idRow);

                    var dr = MessageBox.Show("Вы действительно хотите удалить запись: \r\n \r\n" +
                        String.Format("Начало платежей: {0}\r\n Конец платежей: {1}\r\n Сумма: {2}", currentPeriod.DateBegin.ToShortDateString(),
                        Convert.ToDateTime(currentPeriod.DateEnd).ToShortDateString(), currentPeriod.Value), "Удаление периода платежей", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        try
                        {
                            db.Schedules.Remove(currentPeriod);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                            string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                            CurrentSession.ReportError(methodName, ex.Message);
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    LoadGeneratePayments(CurrentSession.GroupId);
                    if (dgvPeriod.Rows.Count != 0)
                    {
                        if (index <= dgvPeriod.Rows.Count - 1)
                        {
                            dgvPeriod.CurrentCell = dgvPeriod["DateBegin", index];
                            SelectRow(dgvPeriod, index, "DateBegin");
                        }
                        else
                        {
                            dgvPeriod.CurrentCell = dgvPeriod["DateBegin", dgvPeriod.Rows.Count - 1];
                            SelectRow(dgvPeriod, dgvPeriod.Rows.Count - 1, "DateBegin");
                        }
                    }

                }
                else MessageBox.Show("Выберите нужный период для редактирования", "Внимание", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private void SelectRow(DataGridView dgv, int currentIndexRow, string columnName)
        {
            if (dgv.Rows.Count != 0)
            {
                if (currentIndexRow < dgv.Rows.Count)
                {
                    dgv.CurrentCell = dgv[columnName, currentIndexRow];
                    dgv[columnName, currentIndexRow].Selected = true;
                }
                else
                {
                    dgv.CurrentCell = dgv[columnName, dgv.Rows.Count - 1];
                    dgv[columnName, dgv.Rows.Count - 1].Selected = true;
                }
            }
        }

        private void tbValueCourse_TextChanged(object sender, EventArgs e)
        {
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void tbValueCourse_KeyPress(object sender, KeyPressEventArgs e)
        {
            char с = e.KeyChar;
            if ((с < '0' || с > '9') && с != '\b')
            {
                e.Handled = true;
            }
        }

        private void GroupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btSaveAndClose.Enabled)
            {
                var dr = MessageBox.Show("Сохранить данные о группе?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    SaveGroup();
                    SaveSchedulerGroup();
                    SaveJournal();
                    btSave.Enabled = btSaveAndClose.Enabled = false;
                }
                if (dr == DialogResult.Cancel) e.Cancel = true;
            }
        }

        private void btGeneratePeriod_Click(object sender, EventArgs e)
        {
            if (dgvPayments.Rows.Count == 0)
            {
                //генерация графика платежей если его еще нет
                generateNewScheduler = true;
                if (!GeneratePayments()) MessageBox.Show("Не задан период платежей", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var studentsList = db.Enrollments.Where(a => a.GroupId == group.Id && a.DateExclusion == null).ToList();
                if (studentsList.Count > 0)
                {
                    var dr = MessageBox.Show("В данную группу уже записаны учащиеся! Обновить график платежей для учащихся записанных в группу?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        //генерация графика платежей для группы
                        generateNewScheduler = true;
                        if (!GeneratePayments())
                        {
                            MessageBox.Show("Не задан период платежей", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            //генерация графика платежей для учащихся записанных в группу
                            foreach (var enroll in studentsList)
                            {
                                var schedule = db.Schedules.Where(a => a.GroupId == group.Id && a.Source == 2 && a.EnrollmentId == enroll.Id).OrderBy(a => a.DateBegin).ToList();
                                if (schedule.Count == 0)
                                {
                                    foreach (DataGridViewRow a in dgvPayments.Rows)
                                    {
                                        db.Schedules.Add(new Schedule
                                        {
                                            GroupId = group.Id,
                                            DateBegin = Convert.ToDateTime(a.Cells["DatePay"].Value),
                                            EnrollmentId = enroll.Id,
                                            Source = 2,
                                            Value = Convert.ToDouble(a.Cells["Pay"].Value),
                                            Discount = 0,
                                            WorkerId = CurrentSession.CurrentUser.Id
                                        });
                                    }
                                    int c = db.SaveChanges();
                                }
                                else if (schedule.Count > 0 && schedule.Count == dgvPayments.Rows.Count)
                                {
                                    int i = 0;
                                    foreach (DataGridViewRow a in dgvPayments.Rows)
                                    {
                                        schedule[i].DateBegin = Convert.ToDateTime(a.Cells["DatePay"].Value);
                                        schedule[i].Value = Convert.ToDouble(a.Cells["Pay"].Value);
                                        schedule[i].WorkerId = CurrentSession.CurrentUser.Id;
                                        schedule[i].Discount = schedule[i].Discount;
                                        db.Entry(schedule[i]).State = EntityState.Modified;
                                        i++;
                                    }
                                    int c = db.SaveChanges();
                                }
                                else if (schedule.Count > 0 && schedule.Count < dgvPayments.Rows.Count)
                                {
                                    int i = 0;
                                    foreach (DataGridViewRow a in dgvPayments.Rows)
                                    {
                                        if (i < schedule.Count)
                                        {
                                            schedule[i].DateBegin = Convert.ToDateTime(a.Cells["DatePay"].Value);
                                            schedule[i].Value = Convert.ToDouble(a.Cells["Pay"].Value);
                                            schedule[i].Discount = schedule[i].Discount;
                                            schedule[i].WorkerId = CurrentSession.CurrentUser.Id;

                                            db.Entry(schedule[i]).State = EntityState.Modified;
                                        }
                                        else
                                        {
                                            db.Schedules.Add(new Schedule
                                            {
                                                GroupId = group.Id,
                                                DateBegin = Convert.ToDateTime(a.Cells["DatePay"].Value),
                                                EnrollmentId = enroll.Id,
                                                Source = 2,
                                                Value = Convert.ToDouble(a.Cells["Pay"].Value),
                                                Discount = 0,
                                                WorkerId = CurrentSession.CurrentUser.Id
                                            });
                                        }
                                        i++;
                                    }
                                    int c = db.SaveChanges();
                                }
                                else
                                {
                                    int i = 0;
                                    foreach (var sch in schedule)
                                    {
                                        if (i < dgvPayments.Rows.Count)
                                        {
                                            sch.DateBegin = Convert.ToDateTime(dgvPayments.Rows[i].Cells["DatePay"].Value);
                                            sch.Value = Convert.ToDouble(dgvPayments.Rows[i].Cells["Pay"].Value);
                                            sch.WorkerId = CurrentSession.CurrentUser.Id;

                                            db.Entry(sch).State = EntityState.Modified;
                                        }
                                        else
                                        {
                                            db.Schedules.Remove(sch);
                                        }
                                        i++;
                                    }
                                    int c = db.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        //генерация графика платежей только для группы, так как студентов отменили
                        generateNewScheduler = true;
                        if (!GeneratePayments()) MessageBox.Show("Не задан период платежей", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    //генерация графика платежей только для группы, так как в ней никто не числится
                    generateNewScheduler = true;
                    if (!GeneratePayments()) MessageBox.Show("Не задан период платежей", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                SaveSchedulerGroup();
                btSave.Enabled = btSaveAndClose.Enabled = false;
            }
        }

        public bool GeneratePayments()
        {
            bool result = true;
            btSave.Enabled = btSaveAndClose.Enabled = true;
            dgvPayments.Rows.Clear();
            int i = 1;
            if (radioButton1.Checked)
            {
                if (listPeriod == null)
                {
                    result = false;
                }
                else if (listPeriod.Count > 0)
                {
                    for (var a = 0; a < listPeriod.Count; a++)
                    {
                        var date = listPeriod[a].DateBegin;
                        while (date < listPeriod[a].DateEnd)
                        {
                            dgvPayments.Rows.Add(0, i++, date, listPeriod[a].Value);
                            date = date.AddMonths(1);
                        }
                    }
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                if (tbValueCourse.Text != "" && nudCountPayments.Value != 0)
                {
                    int a = (int)nudCountPayments.Value;
                    var date = dtpBeginPay.Value;
                    double p = Convert.ToDouble(tbValueCourse.Text) % a;
                    int price = Convert.ToInt32(Convert.ToDouble(tbValueCourse.Text) / a);
                    for (i = 1; i <= a; i++)
                    {
                        if (i != a)
                            dgvPayments.Rows.Add(0, i, date, price);
                        else dgvPayments.Rows.Add(0, i, date, price + p);
                        date = date.AddMonths(1);
                    }
                }
                else
                {
                    result = false;
                }
            }

            //расчет итоговой суммы по графику платежей
            if (result)
            {
                double sum = 0;
                for (i = 0; i < dgvPayments.Rows.Count; i++)
                {
                    sum += Convert.ToDouble(dgvPayments.Rows[i].Cells["Pay"].Value);
                }
                label21.Text = sum.ToString("C", CultureInfo.CurrentCulture);
            }
            return result;
        }

        //-------------------------------------//
        //        заполнение журнала           //
        //-------------------------------------//
        DataTable dtJournal = new DataTable();
        //DataSet dsJournal = new DataSet();
        private void GenerateJournal(Group group)
        {
            dtJournal.Rows.Clear();
            dtJournal.Columns.Clear();
            var currentGroup = group;
            if (currentGroup != null)
            {
                //заполнение данных о группе
                var teacher = db.Workers.Find(currentGroup.TeacherId);
                tslTeacher.Text = "Преподаватель: " + teacher.LastnameFM;
                tslTimetable.Text = "Расписание: " + currentGroup.Days + ";   " +
                    currentGroup.Begin.ToShortTimeString() + "-" + currentGroup.EndTimeLesson;

                //получение проведенных занятий и создание столбцов
                List<Lesson> listLessons;// = db.Lessons.AsNoTracking().Where(a => a.GroupId == currentGroup.Id).OrderBy(a => a.Date).ToList();
                using (IstraContext context = new IstraContext())
                {
                    listLessons = context.Lessons.Where(a => a.GroupId == currentGroup.Id).OrderBy(a => a.Date).ToList();

                    countLessonLearned = listLessons.Count;
                    var recoveryNumberLesson = false;
                    if (countLessonLearned > 0)
                        for (int k = 0; k < countLessonLearned; k++)
                        {
                            if (listLessons[k].Number != k + 1)//для восстановления правильной нумерации занятий
                            {
                                listLessons[k].Number = k + 1;
                                context.Entry(listLessons[k]).State = EntityState.Modified;
                                recoveryNumberLesson = true;
                            }
                            //dtJournal.Columns.Add(listLessons[k].Number.ToString(), typeof(string));//вместо Number было Id
                        }
                    if (recoveryNumberLesson) context.SaveChanges();//сохранение после восстановления правильной последовательности нумерации занятий
                }

                int countLessons = 0;
                if (currentGroup.DurationLesson != 0)
                {
                    countLessons = currentGroup.DurationCourse / currentGroup.DurationLesson;
                    tslCountLesson.Text = "Занятий: " + countLessons.ToString();
                }
                else
                {
                    tslCountLesson.Text = "Занятий: 0";
                }

                //новый журнал
                var connectionString = ConfigurationManager.ConnectionStrings["IstraDb"].ConnectionString;

                string sql = "SELECT TOP (100) PERCENT [dbo].[Enrollments].[StudentId], [dbo].[Enrollments].[Id] AS EnrollId, row_number() over(ORDER BY Lastname, Firstname, Middlename) Number, CONCAT( [dbo].[Students].[Lastname], ' ', [dbo].[Students].[Firstname], ' ', [dbo].[Students].[Middlename])  AS Students, ";
                for (int n = 0; n < countLessons; n++)
                    sql += "MAX(CASE Number WHEN " + (n + 1) + " THEN Grade ELSE NULL END) AS [" + (n + 1).ToString() + "], ";
                sql = sql.Substring(0, sql.Length - 2) +
                " FROM [Lessons] RIGHT OUTER JOIN " +
                         "[dbo].[Groups] ON [dbo].[Lessons].[GroupId] = [dbo].[Groups].[Id] INNER JOIN " +
                         "[dbo].[Enrollments] INNER JOIN " +
                         "[dbo].[Students] ON [dbo].[Enrollments].[StudentId] = [dbo].[Students].[Id] ON [dbo].[Groups].[Id] = [dbo].[Enrollments].[GroupId] LEFT OUTER JOIN " +
                         "[dbo].[Studies] ON [dbo].[Students].[Id] = [dbo].[Studies].[StudentId] AND [dbo].[Lessons].[Id] = [dbo].[Studies].[LessonId] " +
                "WHERE [dbo].[Enrollments].[DateExclusion] IS NULL AND [Groups].[Id] = " + currentGroup.Id + " " +
                "GROUP BY [dbo].[Enrollments].[Id], [dbo].[Enrollments].[StudentId], [dbo].[Students].[Lastname], [dbo].[Students].[Firstname], [dbo].[Students].[Middlename] ";

                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(sql, connection);
                    adapter.Fill(dtJournal);
                    dgvJournal.DataSource = dtJournal;
                }


                //оформление datagridview
                dgvJournal.Columns["StudentId"].Visible = dgvJournal.Columns["EnrollId"].Visible = false;
                dgvJournal.Columns["StudentId"].Frozen = dgvJournal.Columns["EnrollId"].Frozen = true;
                dgvJournal.Columns["Number"].Width = 30;
                dgvJournal.Columns["Number"].Frozen = true;
                dgvJournal.Columns["Number"].ReadOnly = true;
                dgvJournal.Columns["Number"].HeaderText = "№";
                dgvJournal.Columns["Number"].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvJournal.Columns["Students"].Width = 230;
                dgvJournal.Columns["Students"].Frozen = true;
                dgvJournal.Columns["Students"].ReadOnly = true;
                dgvJournal.Columns["Students"].HeaderText = "ФИО";
                dgvJournal.Columns["Students"].SortMode = DataGridViewColumnSortMode.NotSortable;

                //заполнение даты для проведенных занятий
                if (countLessonLearned > 0)
                    for (int i = 0; i < countLessonLearned; i++)
                    {
                        dgvJournal.Columns[i + 4].HeaderText = listLessons[i].Date.ToString("dd") + "\r\n"
                            + listLessons[i].Date.ToString("MM") + "\r\n" + listLessons[i].Date.ToString("yy") +
                            "\r\n" + listLessons[i].Number.ToString();
                        dgvJournal.Columns[i + 4].Width = 30;
                        dgvJournal.Columns[i + 4].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dgvJournal.Columns[i + 4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                //нумерация оставшихся столбцов
                if (listLessons.Count < countLessons)
                    for (int j = countLessonLearned + 4; j < countLessons + 4; j++)
                    {
                        dgvJournal.Columns[j].Width = 30;
                        dgvJournal.Columns[j].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dgvJournal.Columns[j].HeaderText = (j - 3).ToString();
                    }
            }
            else MessageBox.Show("Группа не найдена", "Ошибка", MessageBoxButtons.OK);
        }

        private void tsbPrint_Click(object sender, EventArgs e)
        {
            //завершить редактирование ячеек для фиксации изменений в журнале
            dgvJournal.EndEdit();
            int countRows = dgvJournal.RowCount;
            bool countStudents = (countRows > 0) ? true : false;
            var fPrintGroupForm = new PrintGroupForm(group.Id, countStudents, dgvJournal);

            fPrintGroupForm.ShowDialog();
        }

        private void tsbSummary_Click(object sender, EventArgs e)
        {
            // Экспорт журнала в excel
            var journal = new Report();
            journal.ExportJournal(dgvJournal);
        }


        private void tsbAddLesson_Click(object sender, EventArgs e)
        {
            if (dgvJournal.Rows.Count > 0 && nudCountLessons.Value != 0)
            {
                if (nudDurationLesson.Value != 0 && cbHousing.SelectedIndex != -1 && cbClass.SelectedIndex != -1)
                {
                    using (var fLesson = new LessonForm(group, null))
                    {
                        var result = fLesson.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            dtJournal.Columns[countLessonLearned + 4].ColumnName = fLesson.idLesson.ToString();
                            dgvJournal.Columns[countLessonLearned + 4].HeaderText = fLesson.dateLesson.ToString("dd") + "\r\n"
                    + fLesson.dateLesson.ToString("MM") + "\r\n" + fLesson.dateLesson.ToString("yy") + "\r\n" + fLesson.idLesson.ToString();
                            dgvJournal.Columns[countLessonLearned + 4].Width = 30;
                            dgvJournal.Columns[countLessonLearned + 4].SortMode = DataGridViewColumnSortMode.NotSortable;
                            dgvJournal.Columns[countLessonLearned + 4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                        countLessonLearned++;
                        btSave.Enabled = btSaveAndClose.Enabled = false;
                    }
                }
                else
                {
                    MessageBox.Show("Добавление занятия не возможно. Отсутствуют данные о проведении занятий в группе: корпус и/или класс", "Добавление занятия", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else MessageBox.Show("Добавление занятия не возможно. Отсутствуют студенты в группе или не определено количество занятий для группы.", "Добавление занятия", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsbEditLesson_Click(object sender, EventArgs e)
        {
            if (dgvJournal.CurrentCell != null)
                EditLesson(dgvJournal.CurrentCell.ColumnIndex);
        }

        private void EditLesson(int columnIndex)
        {
            try
            {
                if (columnIndex > 2)
                {
                    if (dgvJournal.Columns[columnIndex].HeaderText.Contains("\r\n"))
                    {
                        int numberLesson = Convert.ToInt32(dtJournal.Columns[columnIndex].ColumnName);
                        Lesson currentLesson;
                        using (IstraContext context = new IstraContext())
                        {
                            currentLesson = context.Lessons.FirstOrDefault(a => a.Number == numberLesson && a.GroupId == group.Id);
                        }
                        if (currentLesson != null)
                        {
                            using (var fLesson = new LessonForm(group, currentLesson))
                            {
                                var result = fLesson.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    dtJournal.Columns[columnIndex].ColumnName = fLesson.idLesson.ToString();
                                    dgvJournal.Columns[columnIndex].HeaderText = fLesson.dateLesson.ToString("dd") + "\r\n"
                            + fLesson.dateLesson.ToString("MM") + "\r\n" + fLesson.dateLesson.ToString("yy") + "\r\n" + fLesson.idLesson.ToString();
                                    dgvJournal.Columns[columnIndex].Width = 30;
                                    dgvJournal.Columns[columnIndex].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    dgvJournal.Columns[columnIndex].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                }
                            }
                        }
                    }
                    else
                    {
                        tsbAddLesson.PerformClick();
                    }
                    btSave.Enabled = btSaveAndClose.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных \r\n" + ex.Message, "Ошибка", MessageBoxButtons.OK);
            }
        }

        private void dgvJournal_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EditLesson(e.ColumnIndex);
        }

        private List<Study> editedStudies = new List<Study>();

        private void SaveJournal()
        {
            try
            {
                // Проходимся по списку ожидающих редактирование записей
                foreach (var study in editedStudies)
                {
                    // Получаем запись из БД, идентичную записи, которая ожидает редактирования
                    var databaseStudy = db.Studies.Include("Lesson").Where(Study => (study.GroupId == Study.GroupId)).ToList().FirstOrDefault(Study => (study.StudentId == Study.StudentId && study.LessonId == Study.LessonId));
                    //var GroupStudy = db.Studies.Where(a => a.GroupId == study.GroupId).ToList();
                    // Если запись не найдена, добавляем новую в БД
                    if (databaseStudy == null)
                    {
                        db.Studies.Add(study);

                        continue;
                    }

                    // Если за урок не стоит оценка, удаляем запись из базы данных
                    if (study.Grade == String.Empty)
                    {
                        db.Studies.Remove(databaseStudy);

                        continue;
                    }

                    // В остальных случаях редактируем запись из базы данных в соответствии с изменениями
                    databaseStudy.LessonId = study.LessonId;
                    databaseStudy.StudentId = study.StudentId;
                    databaseStudy.GroupId = study.GroupId;
                    databaseStudy.Grade = study.Grade;
                    db.Entry(databaseStudy).State = EntityState.Modified;
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                var m = new StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Очищаем список
            editedStudies.Clear();
        }

        private void dgvJournal_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                // Получаем информацию об оценке
                var numberLesson = Convert.ToInt32(dtJournal.Columns[e.ColumnIndex].ColumnName);
                var lesson = db.Lessons.FirstOrDefault(a => a.GroupId == group.Id && a.Number == numberLesson);
                if (lesson != null)
                {
                    int lessonId = lesson.Id;
                    var studentId = Convert.ToInt32(dtJournal.Rows[e.RowIndex]["StudentId"]);
                    var groupId = group.Id;
                    var grade = dgvJournal.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                    // Ищем в глобальной переменной запись, если она существует
                    var study = editedStudies.FirstOrDefault(Study => (Study.GroupId == groupId && Study.StudentId == studentId && Study.LessonId == lessonId));

                    // Если запись существует, подправляем оценку, иначе создаем новую запись об оценке
                    if (study == null)
                    {
                        editedStudies.Add(new Study
                        {
                            LessonId = lessonId,
                            StudentId = studentId,
                            GroupId = group.Id,
                            Grade = grade
                        });
                    }
                    else
                    {
                        study.Grade = grade;
                    }
                }
            }
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void tsbDeleteLesson_Click(object sender, EventArgs e)
        {
            SaveJournal();
            if (dgvJournal.CurrentCell != null)
            {
                string dateLesson = dgvJournal.Columns[dgvJournal.CurrentCell.ColumnIndex].HeaderText.Replace("\r\n", ".");
                string numberLesson = dgvJournal.Columns[dgvJournal.CurrentCell.ColumnIndex].Name;
                var dr = MessageBox.Show("Вы действительно хотите удалить занятие №" + numberLesson + " за " + dateLesson
                    + "?\r\n Занятие будет удалено со всеми оценками учащихся!", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    Lesson currentLesson;
                    int num = Convert.ToInt32(numberLesson);
                    //получение удаляемого урока
                    using (IstraContext context = new IstraContext())
                    {
                        currentLesson = context.Lessons.FirstOrDefault(a => a.Number == num && a.GroupId == group.Id);

                        context.Lessons.Attach(currentLesson);
                        context.Entry(currentLesson).State = EntityState.Deleted;
                        context.SaveChanges();
                    }
                    //исправление нумерации занятий после удаления
                    //var lessons = db.Lessons.Where(a => a.GroupId == group.Id).OrderBy(a => a.Number);
                    //int i = 1;
                    //foreach (var l in lessons)
                    //{
                    //    l.Number = i;
                    //    db.Entry(l).State = EntityState.Modified;
                    //}
                    //db.SaveChanges();

                    GenerateJournal(group);
                    UnvisiblingLessons((int)group.UnvisibleLessons);
                    btSave.Enabled = btSaveAndClose.Enabled = false;
                }
            }
        }

        private void tsbEditStudent_Click(object sender, EventArgs e)
        {
            int idStudent = Convert.ToInt32(dgvJournal.Rows[dgvJournal.CurrentCell.RowIndex].Cells["StudentId"].Value);
            var fStudent = new StudentForm(idStudent, true);
            fStudent.ShowDialog();

        }

        private void rbCurrentStudent_CheckedChanged(object sender, EventArgs e)
        {
            if (isFirstNumberDocumentNotSaved)
            {
                var dialogResult = MessageBox.Show(this, $"Изменения в номере документа учеников не были сохранены\nВы хотите сохранить данные?", "Информация об учащихся не сохранена", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.No)
                {
                    Enrollment enrollment;

                    foreach (var student in cbStudents.DataSource as List<Student>)
                    {
                        enrollment = db.Enrollments.Where(Enrollment => (group.Id == Enrollment.GroupId)).ToList().FirstOrDefault(Enrollment => (student?.Id == Enrollment.StudentId));
                        db.Entry(enrollment).State = EntityState.Unchanged;
                    }

                    enrollment = db.Enrollments.Where(Enrollment => (group.Id == Enrollment.GroupId)).ToList().FirstOrDefault(Enrollment => ((cbStudents.SelectedItem as Student)?.Id == Enrollment.StudentId));
                    tbDocNo.Text = enrollment.NumberDocument;
                    tbFirstNo.Text = String.Empty;
                }
                else
                {
                    db.SaveChanges();
                }

                btSave.Enabled = btSaveAndClose.Enabled = false;
            }

            isFirstNumberDocumentNotSaved = isDocumentNumberNotSaved = false;

            label27.Enabled = tbFirstNo.Enabled = false;
            label26.Enabled = tbDocNo.Enabled = true;
            label32.Enabled = cbStudents.Enabled = true;
            btCreateDoc.Text = "Сформировать документ";
            btOpenDocument.Enabled = IsDocumentExists();
        }

        private void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            label27.Enabled = tbFirstNo.Enabled = true;
            label26.Enabled = tbDocNo.Enabled = false;
            label32.Enabled = cbStudents.Enabled = false;
            btCreateDoc.Text = "Сформировать документы";
            btOpenDocument.Enabled = false;
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var fSectionForm = new SectionForm(group);
                var dialogResult = fSectionForm.ShowDialog();

                if (dialogResult == DialogResult.Cancel)
                    return;

                db.Sections.Add(fSectionForm.Section);

                db.SaveChanges();

                LoadSectionsList();

                dgvSections.SelectedRows[0].Selected = false;
                dgvSections.Rows[dgvSections.Rows.Count - 1].Selected = true;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (int)dgvSections.SelectedRows[0].Cells["Id"].Value;
                var index = dgvSections.SelectedRows[0].Index;
                var section = db.Sections.Find(id);
                var fSectionForm = new SectionForm(group, section, index);
                var dialogResult = fSectionForm.ShowDialog();

                if (dialogResult == DialogResult.Cancel)
                    return;

                // Обновление выбранного пользователем раздела
                section.Name = fSectionForm.Section.Name;
                section.IsCredit = fSectionForm.Section.IsCredit;
                section.IsTypeGrade = fSectionForm.Section.IsTypeGrade;
                section.Duration = fSectionForm.Section.Duration;
                section.LessonId = fSectionForm.Section.LessonId;
                section.CourseId = fSectionForm.Section.CourseId;
                section.GroupId = fSectionForm.Section.GroupId;

                db.Entry(section).State = EntityState.Modified;

                db.SaveChanges();

                LoadSectionsList();

                dgvSections.SelectedRows[0].Selected = false;
                dgvSections.Rows[index].Selected = true;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var section = db.Sections.Include("Lesson").Where(Section => (Section.GroupId == group.Id)).ToList().FirstOrDefault(Section => ((int)dgvSections.SelectedRows[0].Cells["Id"].Value == Section.Id));
                var dialogResult = MessageBox.Show(this, $"Вы действительно хотите удалить раздел \"{section.Name}\"?", "Удаление раздела", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Cancel)
                    return;

                db.Sections.Remove(section);

                db.SaveChanges();

                LoadSectionsList();
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSections_DataSourceChanged(object sender, EventArgs e)
        {
            if (dgvSections.Rows.Count > 0)
                btDelete.Enabled = btEdit.Enabled = true;
            else btDelete.Enabled = btEdit.Enabled = false;
        }

        private void tbMaxPoint_TextChanged(object sender, EventArgs e)
        {
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void btCreateProtocol_Click(object sender, EventArgs e)
        {
            try
            {
                #region Если какие то данные не сохранены, прекратить формирование протокола
                if (btSaveAndClose.Enabled)
                {
                    MessageBox.Show(this, "Какие то данные не были сохранены в базу данных\nСохраните их и повторите операцию снова", "Ошибка формирования протокола", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                #endregion

                #region Получение необходимых для работы метода данных
                List<Student> students;
                List<Section> sections;

                try
                {
                    students = db.Enrollments.Include("Student").Where(Enrollment => (Enrollment.GroupId == group.Id) && (Enrollment.DateExclusion == null)).Select(Enrollment => (Enrollment.Student)).OrderBy(Student => (Student.Lastname)).ToList();
                    sections = db.Sections.Include("Lesson").Where(Section => (Section.GroupId == group.Id)).OrderBy(Section => (Section.Id)).ToList();
                }

                catch (Exception)
                {
                    MessageBox.Show(this, "Прежде чем начать выбранную операцию, дождитесь окончания другой", "Ошибка формирования протокола", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                var course = cbCourse.SelectedItem as Course;
                var teacher = cbTeacher.SelectedItem as Worker;
                #endregion

                #region Проверка на наличие номера протокола
                if (tbProtocolNo.Text == String.Empty)
                {
                    MessageBox.Show(this, "Номер протокола не указан\nЧтобы сформировать документ необходимо указать номер протокола", "Ошибка формирования протокола", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                #endregion

                #region Проверка на наличие максимального балла, если он требуется
                if (tbMaxPoint.Enabled && tbMaxPoint.Text == String.Empty)
                {
                    MessageBox.Show(this, "Максимальный балл не указан\nЧтобы сформировать документ необходимо указать максимальный балл", "Ошибка формирования протокола", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                #endregion

                #region Проверка на пустоту таблицы разделов
                if ((cbDocType.SelectedItem as Document).Name != "Сертификат")
                {
                    if (sections.Count == 0)
                    {
                        MessageBox.Show(this, "Таблица разделов не заполнена\nЧтобы сформировать протокол необходимо заполнить разделы", "Ошибка формирования протокола", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                }
                #endregion

                #region Переменная для хранения результата диалоговых окон
                DialogResult dialogResult;
                #endregion

                #region Переменная для хранения пути к протоколу
                var fileName = String.Empty;
                #endregion

                #region Проверка на существование протокола у группы
                if (IsProtocolExists())
                {
                    dialogResult = MessageBox.Show(this, $"Протокол №{tbProtocolNo.Text} у группы \"{group.Name}\" уже существует\nВы уверены, что хотите перезаписать его?", "Сообщение о перезаписи протокола", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dialogResult == DialogResult.No)
                        return;
                }
                #endregion

                #region Формирование протокола для выбранного ученика
                if (rbCurrentStudent.Checked)
                {
                    var student = cbStudents.SelectedItem as Student;

                    #region Проверка на наличие номера документа у выбранного студента
                    var enrollment = db.Enrollments.Where(Enrollment => (group.Id == Enrollment.GroupId)).ToList().FirstOrDefault(Enrollment => (student?.Id == Enrollment.StudentId));

                    if (enrollment.NumberDocument == String.Empty || enrollment.NumberDocument == null)
                    {
                        MessageBox.Show(this, $"У ученика {student?.LastnameFM} не указан номер документа\nЧтобы сформировать документ необходимо его указать", "Ошибка формирования протокола", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                    #endregion

                    #region Если выбранному ученику не поставили оценку за дату, указанную в разделах, прекратить выполнение печати
                    if ((cbDocType.SelectedItem as Document).Name != "Сертификат")
                    {
                        var invalidStudent = new InvalidStudent
                        {
                            Student = student,
                            Lessons = new List<Lesson>()
                        };

                        foreach (var section in sections)
                        {
                            var lesson = db.Lessons.Where(Lesson => (group.Id == Lesson.GroupId)).ToList().FirstOrDefault(Lesson => (section.LessonId == Lesson.Id));

                            var study = db.Studies.ToList().FirstOrDefault(Study => (student?.Id == Study.StudentId && lesson.Id == Study.LessonId));

                            if (study == null || !int.TryParse(study.Grade, out int valid))
                            {
                                invalidStudent.Lessons.Add(lesson);
                            }
                        }

                        if (invalidStudent.Lessons.Count != 0)
                        {
                            var output = String.Empty;

                            foreach (var lesson in invalidStudent.Lessons)
                            {
                                if (output == String.Empty)
                                {
                                    output += $"{lesson.Date.ToShortDateString()}";
                                }
                                else
                                {
                                    output += $", {lesson.Date.ToShortDateString()}";
                                }
                            }

                            MessageBox.Show(this, $"У ученика {student?.LastnameFM} не выставлены оценки за {output}\nПроставьте оценки в журнал, чтобы сформировать документ", "Ошибка формирования протокола", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            return;
                        }
                    }
                    #endregion

                    #region Создаем и получаем полное имя директории
                    var directory = Directory.CreateDirectory($"Документы\\Группа {group.Name} ({GetYear()})").FullName;
                    #endregion

                    #region Если документ - это свидетельство или удостоверение, печатаем протокол удостоверения
                    if ((cbDocType.SelectedItem as Document).Name == "Свидетельство" || (cbDocType.SelectedItem as Document).Name == "Удостоверение")
                    {
                        // Формирование регистрационной книги для группы
                        var studyRegBook = new StudyRegBook()
                        {
                            CourseName = course.Name,
                            ProtocolDate = dtpProtocol.Value,
                            ProtocolNumber = tbProtocolNo.Text,
                            Students = new List<Student>() { student }
                        };

                        if ((cbDocType.SelectedItem as Document).Name == "Свидетельство")
                            studyRegBook.DocumentType = "свидетельства";

                        if ((cbDocType.SelectedItem as Document).Name == "Удостоверение")
                            studyRegBook.DocumentType = "удостоверения";

                        var printRegBook = new PtRegBook(studyRegBook, group);
                        fileName = $"{directory}\\Регистрационная книга.docx";
                        printRegBook.CreatePackage(fileName);

                        // Формирование протокола свидетельства для группы
                        var printProtocolDocument = new PtWord(new StudyProtocol()
                        {
                            GroupName = group.Name,
                            CourseName = course.Name,
                            ProtocolNumber = tbProtocolNo.Text,
                            TeacherName = teacher.Fullname,
                            ProtocolDate = dtpProtocol.Value,
                            Students = new List<Student>() { student },
                            Sections = sections
                        }, group);

                        fileName = $"{directory}\\Протокол №{tbProtocolNo.Text.Split('/')[0]}.docx";
                        printProtocolDocument.CreatePackage(fileName);
                    }
                    #endregion

                    #region Если документ - это сертификат, печатаем протокол сертификата
                    if ((cbDocType.SelectedItem as Document).Name == "Сертификат")
                    {
                        // Формирование регистрационной книги для группы
                        var printRegBook = new PtRegBook(new StudyRegBook()
                        {
                            CourseName = course.Name,
                            ProtocolDate = dtpProtocol.Value,
                            DocumentType = "сертификаты",
                            ProtocolNumber = tbProtocolNo.Text,
                            Students = new List<Student>() { student }
                        }, group);

                        fileName = $"{directory}\\Регистрационная книга.docx";
                        printRegBook.CreatePackage(fileName);

                        // Формирование протокола сертификата для группы
                        var printProtocolDocument = new PtEnglishWord(new StudyProtocolEnglish()
                        {
                            GroupName = group.Name,
                            CourseName = course.Name,
                            Level = course.Name,
                            MaxGrade = tbMaxPoint.Text,
                            ProtocolNumber = tbProtocolNo.Text,
                            ProtocolDate = dtpProtocol.Value,
                            TeacherName = teacher.Fullname,
                            Students = new List<Student>() { student }
                        }, group);

                        fileName = $"{directory}\\Протокол №{tbProtocolNo.Text.Split('/')[0]}.docx";
                        printProtocolDocument.CreatePackage(fileName);
                    }
                    #endregion

                    #region Вывод результата печати
                    dialogResult = MessageBox.Show(this, $"Протокол №{tbProtocolNo.Text} для учащегося {student.LastnameFM} успешно сформирован\nОткрыть протокол?", "Сообщение об успешном формировании протокола", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dialogResult == DialogResult.Yes)
                    {
                        try
                        {
                            Process.Start(fileName);
                        }

                        catch (Exception error)
                        {
                            MessageBox.Show(this, $"При открытии файла с протоколом произошла ошибка:\n{error.Message}", "Ошибка открытия протокола", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    #endregion
                }
                #endregion

                #region Формирование протоколов для группы
                if (rbAll.Checked)
                {
                    #region Проверка на наличие номера документа у всех студентов
                    var output = String.Empty;

                    foreach (var student in students)
                    {
                        var enrollment = db.Enrollments.Where(Lesson => (group.Id == Lesson.GroupId)).ToList().FirstOrDefault(Enrollment => (student?.Id == Enrollment.StudentId));

                        if (enrollment.NumberDocument == String.Empty || enrollment.NumberDocument == null)
                            output += $"\n• {student?.LastnameFM}";
                    }

                    if (output != String.Empty)
                    {
                        MessageBox.Show(this, $"У следующих учеников не указан номер документа: {output}\nЧтобы сформировать документ необходимо указать номера документов", "Ошибка формирования протокола", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                    #endregion

                    #region Если какой то в определенном разделе есть дата, за которую какому то ученику не выставили оценку, прекратить выполнение печати
                    if ((cbDocType.SelectedItem as Document).Name != "Сертификат")
                    {
                        var invalidStudents = new List<InvalidStudent>();

                        foreach (var student in students)
                        {
                            var invalidStudent = new InvalidStudent
                            {
                                Student = student,
                                Lessons = new List<Lesson>()
                            };

                            foreach (var section in sections)
                            {
                                var lesson = db.Lessons.Where(Lesson => (group.Id == Lesson.GroupId)).ToList().FirstOrDefault(Lesson => (section.LessonId == Lesson.Id));

                                var study = db.Studies.ToList().FirstOrDefault(Study => (student?.Id == Study.StudentId && lesson.Id == Study.LessonId));

                                if (study == null || !int.TryParse(study.Grade, out int valid))
                                {
                                    invalidStudent.Lessons.Add(lesson);
                                }
                            }

                            if (invalidStudent.Lessons.Count != 0)
                            {
                                invalidStudents.Add(invalidStudent);
                            }
                        }

                        if (invalidStudents.Count != 0)
                        {
                            output = String.Empty;

                            foreach (var invalidStudent in invalidStudents)
                            {
                                var dates = String.Empty;

                                foreach (var lesson in invalidStudent.Lessons)
                                {
                                    if (dates == String.Empty)
                                    {
                                        dates += $"{lesson.Date.ToShortDateString()}";
                                    }
                                    else
                                    {
                                        dates += $", {lesson.Date.ToShortDateString()}";
                                    }
                                }

                                output += $"\n• {invalidStudent.Student.Fullname()} за {dates}";
                            }

                            MessageBox.Show(this, $"У следующих учеников не выставлена оценка: {output}\nПроставьте оценки в журнал, чтобы сформировать документ", "Ошибка формирования протокола", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            return;
                        }
                    }
                    #endregion

                    #region Создаем и получаем полное имя директории
                    var directory = Directory.CreateDirectory($"Документы\\Группа {group.Name} ({GetYear()})").FullName;
                    #endregion

                    #region Если документ - это свидетельство или удостоверение, печатаем протокол удостоверения
                    if ((cbDocType.SelectedItem as Document).Name == "Свидетельство" || (cbDocType.SelectedItem as Document).Name == "Удостоверение"
                        || (cbDocType.SelectedItem as Document).Name == "Диплом")
                    {
                        // Формирование регистрационной книги для группы
                        var studyRegBook = new StudyRegBook()
                        {
                            CourseName = course.Name,
                            ProtocolDate = dtpProtocol.Value,
                            ProtocolNumber = tbProtocolNo.Text,
                            Students = students
                        };

                        if ((cbDocType.SelectedItem as Document).Name == "Свидетельство")
                            studyRegBook.DocumentType = "свидетельства";

                        if ((cbDocType.SelectedItem as Document).Name == "Удостоверение")
                            studyRegBook.DocumentType = "удостоверения";

                        if ((cbDocType.SelectedItem as Document).Name == "Диплом")
                            studyRegBook.DocumentType = "диплом";

                        var printRegBook = new PtRegBook(studyRegBook, group);
                        fileName = $"{directory}\\Регистрационная книга.docx";
                        printRegBook.CreatePackage(fileName);

                        // Формирование протокола свидетельства для группы
                        var printProtocolDocument = new PtWord(new StudyProtocol()
                        {
                            GroupName = group.Name,
                            CourseName = course.Name,
                            ProtocolNumber = tbProtocolNo.Text,
                            TeacherName = teacher.Fullname,
                            ProtocolDate = dtpProtocol.Value,
                            Students = students,
                            Sections = sections
                        }, group);

                        fileName = $"{directory}\\Протокол №{tbProtocolNo.Text.Split('/')[0]}.docx";
                        printProtocolDocument.CreatePackage(fileName);
                    }
                    #endregion

                    #region Если документ - это сертификат, печатаем сертификат
                    if ((cbDocType.SelectedItem as Document).Name == "Сертификат")
                    {
                        // Формирование регистрационной книги для группы
                        var printRegBook = new PtRegBook(new StudyRegBook()
                        {
                            CourseName = course.Name,
                            ProtocolDate = dtpProtocol.Value,
                            DocumentType = "сертификаты",
                            ProtocolNumber = tbProtocolNo.Text,
                            Students = students
                        }, group);

                        fileName = $"{directory}\\Регистрационная книга.docx";
                        printRegBook.CreatePackage(fileName);

                        // Формирование протокола сертификата для группы
                        var printProtocolDocument = new PtEnglishWord(new StudyProtocolEnglish()
                        {
                            GroupName = group.Name,
                            CourseName = course.Name,
                            Level = course.Name,
                            MaxGrade = tbMaxPoint.Text,
                            ProtocolNumber = tbProtocolNo.Text,
                            ProtocolDate = dtpProtocol.Value,
                            TeacherName = teacher.Fullname,
                            Students = students
                        }, group);

                        fileName = $"{directory}\\Протокол №{tbProtocolNo.Text.Split('/')[0]}.docx";
                        printProtocolDocument.CreatePackage(fileName);
                    }
                    #endregion

                    #region Вывод результата печати
                    dialogResult = MessageBox.Show(this, $"Протокол №{tbProtocolNo.Text} для группы \"{group.Name}\" успешно сформирован\nОткрыть протокол?", "Сообщение об успешном формировании протокола", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dialogResult == DialogResult.Yes)
                    {
                        try
                        {
                            Process.Start(fileName);
                        }

                        catch (Exception error)
                        {
                            MessageBox.Show(this, $"При открытии файла с протоколом произошла ошибка:\n{error.Message}", "Ошибка открытия протокола", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    #endregion
                }
                #endregion

                #region Проверка на существование файлов и включение/выключение кнопок их открытия
                CheckFileExists();
                #endregion
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btCreateDoc_Click(object sender, EventArgs e)
        {
            try
            {
                #region Если какие то данные не сохранены, прекратить формирование документа
                if (btSaveAndClose.Enabled)
                {
                    MessageBox.Show(this, "Какие то данные не были сохранены в базу данных\nСохраните их и повторите операцию снова", "Ошибка формирования документа", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                #endregion

                #region Получение необходимых для работы метода данных
                List<Student> students;
                List<Section> sections;

                try
                {
                    students = db.Enrollments.Include("Student").Where(Enrollment => (Enrollment.GroupId == group.Id) && (Enrollment.DateExclusion == null)).Select(Enrollment => (Enrollment.Student)).OrderBy(Student => (Student.Lastname)).ToList();
                    sections = db.Sections.Include("Lesson").Where(Section => (Section.GroupId == group.Id)).OrderBy(Section => (Section.Id)).ToList();
                }

                catch (Exception)
                {
                    MessageBox.Show(this, "Прежде чем начать выбранную операцию, дождитесь окончания другой", "Ошибка формирования документа", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                var course = cbCourse.SelectedItem as Course;
                var teacher = cbTeacher.SelectedItem as Worker;
                #endregion

                #region Проверка на наличие номера протокола
                if (tbProtocolNo.Text == String.Empty)
                {
                    MessageBox.Show(this, "Номер протокола не указан\nЧтобы сформировать документ необходимо указать номер протокола", "Ошибка формирования документа", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                #endregion

                #region Проверка на наличие максимального балла, если он требуется
                if (tbMaxPoint.Enabled && tbMaxPoint.Text == String.Empty)
                {
                    MessageBox.Show(this, "Максимальный балл не указан\nЧтобы сформировать документ необходимо указать максимальный балл", "Ошибка формирования документа", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                #endregion

                #region Проверка на пустоту таблицы разделов
                if (sections.Count == 0)
                {
                    MessageBox.Show(this, "Таблица разделов не заполнена\nЧтобы сформировать документ необходимо заполнить разделы", "Ошибка формирования документа", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                #endregion

                #region Переменная для хранения результата диалоговых окон
                DialogResult dialogResult;
                #endregion

                #region Переменная для хранения пути к документу
                var fileName = String.Empty;
                #endregion

                #region Формирования документов для выбранного ученика
                if (rbCurrentStudent.Checked)
                {
                    var student = cbStudents.SelectedItem as Student;

                    #region Проверка на существования документа у студента
                    if (IsDocumentExists())
                    {
                        dialogResult = MessageBox.Show(this, $"Документ для учащегося {student?.LastnameFM} уже существует\nВы уверены, что хотите перезаписать его?", "Сообщение о перезаписи документа", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dialogResult == DialogResult.No)
                            return;
                    }
                    #endregion

                    #region Проверка на наличие номера документа у выбранного студента
                    var enrollment = db.Enrollments.Where(Enrollment => (group.Id == Enrollment.GroupId)).ToList().FirstOrDefault(Enrollment => (student?.Id == Enrollment.StudentId));

                    if (enrollment.NumberDocument == String.Empty || enrollment.NumberDocument == null)
                    {
                        MessageBox.Show(this, $"У ученика {student?.LastnameFM} не указан номер документа\nЧтобы сформировать документ необходимо его указать", "Ошибка формирования документа", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                    #endregion

                    #region Если выбранному ученику не поставили оценку за дату, указанную в разделах, прекратить выполнение печати
                    var invalidStudent = new InvalidStudent
                    {
                        Student = student,
                        Lessons = new List<Lesson>()
                    };

                    foreach (var section in sections)
                    {
                        var lesson = db.Lessons.Where(Lesson => (group.Id == Lesson.GroupId)).ToList().FirstOrDefault(Lesson => (section.LessonId == Lesson.Id));

                        var study = db.Studies.ToList().FirstOrDefault(Study => (student?.Id == Study.StudentId && lesson.Id == Study.LessonId));

                        if (study == null || !int.TryParse(study.Grade, out int valid))
                        {
                            invalidStudent.Lessons.Add(lesson);
                        }
                    }

                    if (invalidStudent.Lessons.Count != 0)
                    {
                        var output = String.Empty;

                        foreach (var lesson in invalidStudent.Lessons)
                        {
                            if (output == String.Empty)
                            {
                                output += $"{lesson.Date.ToShortDateString()}";
                            }
                            else
                            {
                                output += $", {lesson.Date.ToShortDateString()}";
                            }
                        }

                        MessageBox.Show(this, $"У ученика {student?.LastnameFM} не выставлены оценки за {output}\nПроставьте оценки в журнал, чтобы сформировать документ", "Ошибка формирования документа", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                    #endregion

                    #region Создаем и получаем полное имя директории
                    var directory = Directory.CreateDirectory($"Документы\\Группа {group.Name} ({GetYear()})").FullName;
                    #endregion

                    #region Если документ - это свидетельство, печатаем свидетельство
                    if ((cbDocType.SelectedItem as Document).Name == "Свидетельство")
                    {
                        // Формирование документа для конкретного ученика
                        var printStudyDocument = new DtWord(new StudyDocument()
                        {
                            StudentId = student.Id,
                            StudentName = student.Fullname(),
                            StudentBirthday = student.DateOfBirth,
                            TeacherName = teacher.LastnameFM,
                            DocumentNumber = tbDocNo.Text,
                            ProtocolNumber = tbProtocolNo.Text,
                            CourseName = course.Name,
                            CourseDuration = Convert.ToInt32(nudDurationCourse.Value),
                            CourseBegin = dtpDateBegin.Value,
                            CourseEnd = dtpDoc.Value,
                            DocumentDate = dtpDoc.Value,
                            ProtocolDate = dtpProtocol.Value,
                            IssueDate = dtpDoc.Value,
                            Sections = sections
                        }, group);

                        fileName = $"{directory}\\{student?.Fullname()}.docx";
                        printStudyDocument.CreatePackage(fileName);
                    }
                    #endregion

                    #region Если документ - это сертификат, печатаем сертификат
                    if ((cbDocType.SelectedItem as Document).Name == "Сертификат")
                    {
                        //var grade = db.Studies.Where(Study => (group.Id == Study.GroupId && student.Id == Study.StudentId)).Sum(x => (long.Parse(x.Grade)));

                        // Формирование документа ученика
                        var printStudyDocument = new DtEnglishWord(new StudyDocumentEnglish()
                        {
                            MaxGrade = tbMaxPoint.Text,
                            Grade = "____",
                            CourseDuration = Convert.ToInt32(nudDurationCourse.Value),
                            Level = course.Name,
                            Student = student,
                            Teacher = teacher,
                            CourseBegin = dtpDateBegin.Value,
                            CourseEnd = dtpDoc.Value,
                        }, group);

                        fileName = $"{directory}\\{student?.Fullname()}.docx";
                        printStudyDocument.CreatePackage(fileName);
                    }
                    #endregion

                    #region Если документ - это удостоверение, печатаем удостоверение
                    if ((cbDocType.SelectedItem as Document).Name == "Удостоверение")
                    {
                        var printStudyDocument = new DtTickWord(new StudyDocument()
                        {
                            StudentId = student.Id,
                            StudentName = student.Fullname(),
                            StudentBirthday = student.DateOfBirth,
                            TeacherName = teacher.LastnameFM,
                            DocumentNumber = tbDocNo.Text,
                            ProtocolNumber = tbProtocolNo.Text,
                            CourseName = course.Name,
                            CourseDuration = Convert.ToInt32(nudDurationCourse.Value),
                            CourseBegin = dtpDateBegin.Value,
                            CourseEnd = dtpDoc.Value,
                            DocumentDate = dtpDoc.Value,
                            ProtocolDate = dtpProtocol.Value,
                            IssueDate = dtpDoc.Value,
                            Sections = sections
                        }, group);

                        fileName = $"{directory}\\{student?.Fullname()}.docx";
                        printStudyDocument.CreatePackage(fileName);
                    }
                    #endregion

                    #region Вывод результата печати
                    dialogResult = MessageBox.Show(this, $"Документ для {student?.LastnameFM} успешно сформирован\nОткрыть документ?", "Сообщение об успешном формировании документа", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dialogResult == DialogResult.Yes)
                    {
                        try
                        {
                            Process.Start(fileName);
                        }

                        catch (Exception error)
                        {
                            MessageBox.Show(this, $"При открытии файла с документом произошла ошибка:\n{error.Message}", "Ошибка открытия документа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    #endregion
                }
                #endregion

                #region Формирование документов для группы
                if (rbAll.Checked)
                {
                    #region Проверка на существования документов у группы
                    if (IsDirectoryExists())
                    {
                        dialogResult = MessageBox.Show(this, "Документы для группы уже существуют\nВы уверены, что хотите перезаписать их?", "Сообщение о перезаписи документов", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dialogResult == DialogResult.No)
                            return;
                    }
                    #endregion

                    #region Проверка на наличие номера документа у всех студентов
                    var output = String.Empty;

                    foreach (var student in students)
                    {
                        var enrollment = db.Enrollments.Where(Lesson => (group.Id == Lesson.GroupId)).ToList().FirstOrDefault(Enrollment => (student?.Id == Enrollment.StudentId));

                        if (enrollment.NumberDocument == String.Empty || enrollment.NumberDocument == null)
                            output += $"\n• {student?.LastnameFM}";
                    }

                    if (output != String.Empty)
                    {
                        MessageBox.Show(this, $"У следующих учеников не указан номер документа: {output}\nЧтобы сформировать документ необходимо указать номера документов", "Ошибка формирования документа", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                    #endregion

                    #region Если какой то в определенном разделе есть дата, за которую какому то ученику не выставили оценку, прекратить выполнение печати
                    var invalidStudents = new List<InvalidStudent>();

                    foreach (var student in students)
                    {

                        var invalidStudent = new InvalidStudent
                        {
                            Student = student,
                            Lessons = new List<Lesson>()
                        };

                        foreach (var section in sections)
                        {
                            var lesson = db.Lessons.Where(Lesson => (group.Id == Lesson.GroupId)).ToList().FirstOrDefault(Lesson => (section.LessonId == Lesson.Id));

                            var study = db.Studies.ToList().FirstOrDefault(Study => (student?.Id == Study.StudentId && lesson.Id == Study.LessonId));

                            if (study == null || !int.TryParse(study.Grade, out int valid))
                            {
                                invalidStudent.Lessons.Add(lesson);
                            }
                        }

                        if (invalidStudent.Lessons.Count != 0)
                        {
                            invalidStudents.Add(invalidStudent);
                        }
                    }

                    if (invalidStudents.Count != 0)
                    {
                        output = String.Empty;

                        foreach (var invalidStudent in invalidStudents)
                        {
                            var dates = String.Empty;

                            foreach (var lesson in invalidStudent.Lessons)
                            {
                                if (dates == String.Empty)
                                {
                                    dates += $"{lesson.Date.ToShortDateString()}";
                                }
                                else
                                {
                                    dates += $", {lesson.Date.ToShortDateString()}";
                                }
                            }

                            output += $"\n• {invalidStudent.Student.Fullname()} за {dates}";
                        }

                        MessageBox.Show(this, $"У следующих учеников не выставлена оценка: {output}\nПроставьте оценки в журнал, чтобы сформировать документ", "Ошибка формирования документа", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                    #endregion

                    #region Создаем и получаем полное имя директории
                    var directory = Directory.CreateDirectory($"Документы\\Группа {group.Name} ({GetYear()})").FullName;
                    #endregion

                    #region Если документ - это свидетельство, печатаем свидетельство
                    if ((cbDocType.SelectedItem as Document).Name == "Свидетельство")
                    {
                        // Формирование документа для всех учеников
                        foreach (var student in students)
                        {
                            var enrollment = db.Enrollments.Where(Lesson => (group.Id == Lesson.GroupId)).ToList().FirstOrDefault(Enrollment => (student?.Id == Enrollment.StudentId));

                            var printStudyDocument = new DtWord(new StudyDocument()
                            {
                                StudentId = student.Id,
                                StudentName = student.Fullname(),
                                StudentBirthday = student.DateOfBirth,
                                TeacherName = teacher.LastnameFM,
                                DocumentNumber = enrollment.NumberDocument,
                                ProtocolNumber = tbProtocolNo.Text,
                                CourseName = course.Name,
                                CourseDuration = Convert.ToInt32(nudDurationCourse.Value),
                                CourseBegin = dtpDateBegin.Value,
                                CourseEnd = dtpDoc.Value,
                                DocumentDate = dtpDoc.Value,
                                ProtocolDate = dtpProtocol.Value,
                                IssueDate = dtpDoc.Value,
                                Sections = sections
                            }, group);

                            fileName = $"{directory}\\{student?.Fullname()}.docx";
                            printStudyDocument.CreatePackage(fileName);
                        }
                    }
                    #endregion

                    #region Если документ - это сертификат, печатаем сертификат
                    if ((cbDocType.SelectedItem as Document).Name == "Сертификат")
                    {
                        // Формирование документов для всех учеников
                        foreach (var student in students)
                        {
                            var printStudyDocument = new DtEnglishWord(new StudyDocumentEnglish()
                            {
                                MaxGrade = tbMaxPoint.Text,
                                Grade = "____",
                                CourseDuration = Convert.ToInt32(nudDurationCourse.Value),
                                Level = course.Name,
                                Student = student,
                                Teacher = teacher,
                                CourseBegin = dtpDateBegin.Value,
                                CourseEnd = dtpDoc.Value,
                            }, group);

                            fileName = $"{directory}\\{student?.Fullname()}.docx";
                            printStudyDocument.CreatePackage(fileName);
                        }
                    }
                    #endregion

                    #region Если документ - это удостоверение, печатаем удостоверение
                    if ((cbDocType.SelectedItem as Document).Name == "Удостоверение")
                    {
                        // Формирование документов для всех учеников
                        foreach (var student in students)
                        {
                            var enrollment = db.Enrollments.Where(Lesson => (group.Id == Lesson.GroupId)).ToList().FirstOrDefault(Enrollment => (student?.Id == Enrollment.StudentId));

                            var printStudyDocument = new DtTickWord(new StudyDocument()
                            {
                                StudentId = student.Id,
                                StudentName = student.Fullname(),
                                StudentBirthday = student.DateOfBirth,
                                TeacherName = teacher.LastnameFM,
                                DocumentNumber = enrollment.NumberDocument,
                                ProtocolNumber = tbProtocolNo.Text,
                                CourseName = course.Name,
                                CourseDuration = Convert.ToInt32(nudDurationCourse.Value),
                                CourseBegin = dtpDateBegin.Value,
                                CourseEnd = dtpDoc.Value,
                                DocumentDate = dtpDoc.Value,
                                ProtocolDate = dtpProtocol.Value,
                                IssueDate = dtpDoc.Value,
                                Sections = sections
                            }, group);

                            fileName = $"{directory}\\{student?.Fullname()}.docx";
                            printStudyDocument.CreatePackage(fileName);
                        }
                    }
                    #endregion

                    #region Вывод результатов печати
                    dialogResult = MessageBox.Show(this, $"Документы для всей группы успешно сформированы\nОткрыть папку с документами?", "Сообщение об успешном формировании документа", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dialogResult == DialogResult.Yes)
                    {
                        try
                        {
                            Process.Start(directory);
                        }

                        catch (Exception error)
                        {
                            MessageBox.Show(this, $"При открытии папки с документами произошла ошибка:\n{error.Message}", "Ошибка открытия папки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    #endregion
                }
                #endregion

                #region Проверка на существование файлов и включение/выключение кнопок их открытия
                CheckFileExists();
                #endregion
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckFileExists()
        {
            btOpenDocuments.Enabled = IsDirectoryExists();

            btOpenDocument.Enabled = IsDocumentExists() && rbCurrentStudent.Checked;

            btOpenProtocol.Enabled = IsProtocolExists();
        }

        private bool IsDirectoryExists()
        {
            var directory = new DirectoryInfo($"Документы\\Группа {group.Name} ({GetYear()})");

            return directory.Exists;
        }

        private bool IsDocumentExists()
        {
            var student = cbStudents.SelectedItem as Student;
            var file = new FileInfo($"Документы\\Группа {group.Name} ({GetYear()})\\{student?.Fullname()}.docx");

            return file.Exists;
        }

        private bool IsProtocolExists()
        {
            var file = new FileInfo($"Документы\\Группа {group.Name} ({GetYear()})\\Протокол №{tbProtocolNo.Text.Split('/')[0]}.docx");

            return file.Exists;
        }

        private string GetYear()
        {
            var year = db.Years.ToList().FirstOrDefault(Year => (group.YearId == Year.Id));

            if (year != null)
                return "У-" + dtpDoc.Value.Year.ToString().Substring(2);
            else
                return dtpDoc.Value.Year.ToString().Substring(2);
        }

        private void tbProtocolNo_TextChanged(object sender, EventArgs e)
        {
            btOpenProtocol.Enabled = IsProtocolExists();
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void tbFirstNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Enrollment enrollment;

                if (!int.TryParse(tbFirstNo.Text, out int value))
                    return;

                if (cbStudents.DataSource as List<Student> == null)
                    return;

                foreach (var student in cbStudents.DataSource as List<Student>)
                {
                    enrollment = db.Enrollments.Where(Enrollment => (group.Id == Enrollment.GroupId)).ToList().FirstOrDefault(Enrollment => (student?.Id == Enrollment.StudentId));

                    enrollment.NumberDocument = $"{value++}/{GetYear()}";
                }

                enrollment = db.Enrollments.Where(Enrollment => (group.Id == Enrollment.GroupId)).ToList().FirstOrDefault(Enrollment => ((cbStudents.SelectedItem as Student)?.Id == Enrollment.StudentId));
                tbDocNo.Text = enrollment.NumberDocument;

                isDocumentNumberNotSaved = false;
                btSave.Enabled = isFirstNumberDocumentNotSaved = btSaveAndClose.Enabled = true;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbDocNo_TextChanged(object sender, EventArgs e)
        {
            btSave.Enabled = isDocumentNumberNotSaved = btSaveAndClose.Enabled = true;
        }

        private void cbStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbStudents.DataSource == null)
                    return;

                if (isDocumentNumberNotSaved)
                {
                    var dialogResult = MessageBox.Show(this, $"Изменения для ученика {lastSelectedEnrollment.Student.LastnameFM} не применены\nВы хотите сохранить данные?", "Информация об учащемся не сохранена", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.Yes)
                    {
                        lastSelectedEnrollment.NumberDocument = tbDocNo.Text;
                        db.Entry(lastSelectedEnrollment).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    btSave.Enabled = isDocumentNumberNotSaved = btSaveAndClose.Enabled = false;
                }

                var student = cbStudents.SelectedItem as Student;
                var enrollment = db.Enrollments.Include("Student").Where(Enrollment => (group.Id == Enrollment.GroupId)).ToList().FirstOrDefault(Enrollment => (student?.Id == Enrollment.StudentId));
                lastSelectedEnrollment = enrollment;
                tbDocNo.Text = enrollment.NumberDocument;
                btSave.Enabled = isDocumentNumberNotSaved = btSaveAndClose.Enabled = false;

                CheckFileExists();
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btOpenDocuments_Click(object sender, EventArgs e)
        {
            var directory = new DirectoryInfo($"Документы\\Группа {group.Name} ({GetYear()})");

            try
            {
                Process.Start(directory.FullName);
            }

            catch (Exception error)
            {
                MessageBox.Show(this, $"При открытии папки с документами произошла ошибка:\n{error.Message}", "Ошибка открытия папки", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            CheckFileExists();
        }

        private void btOpenDocument_Click(object sender, EventArgs e)
        {
            try
            {
                var student = cbStudents.SelectedItem as Student;

                var file = new FileInfo($"Документы\\Группа {group.Name} ({GetYear()})\\{student?.Fullname()}.docx");

                try
                {
                    Process.Start(file.FullName);
                }

                catch (Exception error)
                {
                    MessageBox.Show(this, $"При открытии файла с документом произошла ошибка:\n{error.Message}", "Ошибка открытия документа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                CheckFileExists();
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btOpenProtocol_Click(object sender, EventArgs e)
        {
            try
            {
                var file = new FileInfo($"Документы\\Группа {group.Name} ({GetYear()})\\Протокол №{tbProtocolNo.Text.Split('/')[0]}.docx");

                try
                {
                    Process.Start(file.FullName);
                }

                catch (Exception error)
                {
                    MessageBox.Show(this, $"При открытии файла с протоколом произошла ошибка:\n{error.Message}", "Ошибка открытия протокола", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                CheckFileExists();
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtpDoc_ValueChanged(object sender, EventArgs e)
        {
            CheckFileExists();
        }

        private void sbAddStudent_Click(object sender, EventArgs e)
        {
            bool result = SaveGroup();
            if (result)
            {
                SaveJournal();
                var addingStudent = new AddingStudentToGroupForm(group.Id);
                addingStudent.ShowDialog();
                GenerateJournal(group);
                UnvisiblingLessons((int)group.UnvisibleLessons);
                btSave.Enabled = btSaveAndClose.Enabled = false;
            }
        }

        private void chbTwoTeachers_CheckedChanged(object sender, EventArgs e)
        {
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void tsbDeleteStudent_Click_1(object sender, EventArgs e)
        {
            if (dgvJournal.CurrentCell != null)
            {
                SaveJournal();
                int idStudent = Convert.ToInt32(dgvJournal.Rows[dgvJournal.CurrentCell.RowIndex].Cells["StudentId"].Value);
                int enrollId = Convert.ToInt32(dgvJournal.Rows[dgvJournal.CurrentCell.RowIndex].Cells["EnrollId"].Value);

                var fRemoveStudent = new RemoveStudentForm(enrollId);
                fRemoveStudent.ShowDialog();

                GenerateJournal(group);
                UnvisiblingLessons((int)group.UnvisibleLessons);
                btSave.Enabled = btSaveAndClose.Enabled = true;
            }
        }

        private void dgvJournal_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvJournal.CurrentCell != null && e.RowIndex != -1 && CurrentSession.CurrentRole.Name != "Преподаватель")
            {
                int idStudent = Convert.ToInt32(dgvJournal.Rows[dgvJournal.CurrentCell.RowIndex].Cells["StudentId"].Value);
                var studentForm = new StudentForm(idStudent, true);
                studentForm.ShowDialog();
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            SaveGroup();
            SaveSchedulerGroup();
            SaveJournal();
            btSave.Enabled = btSaveAndClose.Enabled = false;
        }

        private void dgvPeriod_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btEditPeriod.PerformClick();
        }

        private void rtbNote_TextChanged(object sender, EventArgs e)
        {
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void tbUnvisibleLessons_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void tbUnvisibleLessons_TextChanged(object sender, EventArgs e)
        {
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void dgvJournal_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                // Получаем информацию об оценке
                var numberLesson = Convert.ToInt32(dtJournal.Columns[e.ColumnIndex].ColumnName);
                var lesson = db.Lessons.FirstOrDefault(a => a.GroupId == group.Id && a.Number == numberLesson);
                if (lesson == null)
                {
                    dgvJournal.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                    MessageBox.Show("Оценка не может быть выставлена за занятие, которое еще не создано!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void cbDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cbDocType.SelectedItem as Document)?.Name == "Сертификат")
                label25.Enabled = tbMaxPoint.Enabled = true;
            else
                label25.Enabled = tbMaxPoint.Enabled = false;

            btSave.Enabled = btSaveAndClose.Enabled = true;
        }
    }
}