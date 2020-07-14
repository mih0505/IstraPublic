using Istra.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Istra.Documents;
using System.Data.Entity;
using System.Text.RegularExpressions;
using Group = Istra.Entities.Group;

namespace Istra
{
    public partial class ListStudentsForm : Form
    {
        IstraContext db = new IstraContext();
        List<School> schools;

        IQueryable<ListStudent> listStudents;
        string sortColumn;
        SortOrder sortOrder;
        int currentRow = -1;


        public ListStudentsForm()
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

                //заполнение классов
                Dictionary<int, string> cl = new Dictionary<int, string>();
                for (int i = 1; i < 12; i++)
                    cl.Add(i, i.ToString());
                cbClasses.DataSource = new BindingSource(cl, null);
                cbClasses.DisplayMember = "Value";
                cbClasses.ValueMember = "Key";
                cbClasses.SelectedIndex = -1;

                //загрузка фильтра тип обучения 
                var type = db.Years.Where(a => a.IsRemoved == false).OrderBy(a => a.SortIndex).ToList();
                cbTypeGroup.DataSource = type;
                cbTypeGroup.DisplayMember = "Name";
                cbTypeGroup.ValueMember = "Id";
                cbTypeGroup.SelectedIndex = -1;

                //заполнение смен
                cbShifts.SelectedIndex = -1;

                //загрузка фильтра школ
                SelectStatus(null);

                //заполнение статусов слушателей
                var status = db.Statuses.Where(a => a.IsRemoved == false).ToList();
                cbStatuses.DataSource = status;
                cbStatuses.DisplayMember = "Name";
                cbStatuses.ValueMember = "Id";
                cbStatuses.SelectedIndex = -1;

                //заполнение статусов групп
                cbActivityGroup.DataSource = db.ActivityGroups.Where(a => a.IsRemoved == false).ToList();
                cbActivityGroup.DisplayMember = "Name";
                cbActivityGroup.ValueMember = "Id";

                //выбор активного статуса
                var currentActivity = db.ActivityGroups.FirstOrDefault(a => a.Name == "Активные");
                cbActivityGroup.SelectedValue = currentActivity.Id;

                //добавление программной сортировки
                foreach (DataGridViewColumn col in dgvStudents.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.Programmatic;
                }

                //загрузка списка слушателей
                Filter(null, null, null, null, null, null, null, null, cbActivityGroup.Text, sortColumn, sortOrder);

                //оформление таблицы
                dgvStudents.Columns["StudentId"].Visible = dgvStudents.Columns["GroupId"].Visible =
                    dgvStudents.Columns["SchoolId"].Visible = dgvStudents.Columns["EnrollId"].Visible =
                    dgvStudents.Columns["StatusId"].Visible = dgvStudents.Columns["ActivityId"].Visible =
                    dgvStudents.Columns["GroupActive"].Visible = dgvStudents.Columns["Activity"].Visible =
                    dgvStudents.Columns["PrivilegeId"].Visible = dgvStudents.Columns["YearId"].Visible = false;

                dgvStudents.Columns["Lastname"].HeaderText = "Фамилия";
                dgvStudents.Columns["Firstname"].HeaderText = "Имя";
                dgvStudents.Columns["Middlename"].HeaderText = "Отчество";
                dgvStudents.Columns["Status"].HeaderText = "Статус";
                dgvStudents.Columns["NameGroup"].HeaderText = "Группа";
                dgvStudents.Columns["Phone1"].HeaderText = "Тел1";
                dgvStudents.Columns["Phone2"].HeaderText = "Тел2";
                dgvStudents.Columns["ParentPhone"].HeaderText = "Тел родителя";
                dgvStudents.Columns["BirthDate"].HeaderText = "Дата рожд.";
                dgvStudents.Columns["School"].HeaderText = "Школа";
                dgvStudents.Columns["Class"].HeaderText = "Класс";
                dgvStudents.Columns["Shift"].HeaderText = "Смена";
                dgvStudents.Columns["Note"].HeaderText = "Примечание";
                dgvStudents.Columns["Sex"].HeaderText = "Пол";
                dgvStudents.Columns["DateEnrollment"].HeaderText = "Дата зачисления";
                dgvStudents.Columns["Saldo"].HeaderText = "Сальдо";
                dgvStudents.Columns["Begin"].HeaderText = "Дата начала";
                dgvStudents.Columns["Year"].HeaderText = "Форма\\Год";
                dgvStudents.Columns["AdditionalPays"].HeaderText = "Доп. платежи";

                dgvStudents.Columns["Class"].Width =
                dgvStudents.Columns["Shift"].Width = dgvStudents.Columns["Sex"].Width = 50;

                dgvStudents.Columns["DateEnrollment"].DefaultCellStyle.Format =
                dgvStudents.Columns["BirthDate"].DefaultCellStyle.Format =
                dgvStudents.Columns["Begin"].DefaultCellStyle.Format = "dd/MM/yyyy";


                dgvStudents.Columns["Saldo"].DefaultCellStyle.Format = "0.00";
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
            cbGroups.SelectedIndex = -1;
            tbLastname.Text = String.Empty;
            tbPhoneNumber.Text = String.Empty;
            cbTypeGroup.SelectedIndex = -1;
            cbClasses.SelectedIndex = -1;
            cbShifts.SelectedIndex = -1;
            cbStatuses.SelectedIndex = -1;

            Filter(null, null, null, null, null, null, null, null, cbActivityGroup.Text, sortColumn, sortOrder);
            SelectStatus(null);
        }

        public void Filter(int? idGroup, int? idYear, string lastname, string phoneNumber, int? idStatus, int? idSchool, int? cl, object shift, string activeGroups, string column, SortOrder? sortOrder)
        {
            try
            {
                listStudents = from enroll in db.Enrollments
                               join groups in db.Groups on enroll.GroupId equals groups.Id
                               join activity in db.ActivityGroups on groups.ActivityId equals activity.Id
                               join students in db.Students on enroll.StudentId equals students.Id
                               join statuses in db.Statuses on students.StatusId equals statuses.Id
                               join years in db.Years on groups.YearId equals years.Id
                               where enroll.DateExclusion == null && enroll.ExclusionId == null
                               select new ListStudent
                               {
                                   StudentId = students.Id,
                                   GroupId = groups.Id,
                                   SchoolId = students.SchoolId,
                                   EnrollId = enroll.Id,
                                   ActivityId = activity.Id,
                                   YearId = years.Id,
                                   Activity = activity.Name,
                                   StatusId = students.StatusId,
                                   Lastname = students.Lastname,
                                   Firstname = students.Firstname,
                                   Middlename = students.Middlename,
                                   NameGroup = groups.Name,
                                   AdditionalPays = enroll.AdditionalPays,
                                   Year = years.Name,
                                   Begin = groups.Begin,
                                   Status = statuses.Name,
                                   Phone1 = students.StudentPhone,
                                   Phone2 = students.StudentPhone2,
                                   ParentPhone = students.ParentsPhone,
                                   Sex = students.Sex,
                                   BirthDate = students.DateOfBirth,
                                   School = students.School.Name,
                                   Class = students.Class,
                                   Shift = students.Shift,
                                   DateEnrollment = enroll.DateEnrollment,
                                   Saldo = (((from pays in db.Payments
                                              where pays.EnrollmentId == enroll.Id && pays.IsDeleted == false && pays.AdditionalPay == false
                                              select new { PaySum = pays.ValuePayment }).Sum(s => (double?)(s.PaySum)) ?? 0)
                                   -(from scheds in db.Schedules
                                     where scheds.Source == 2 && scheds.EnrollmentId == enroll.Id && DbFunctions.TruncateTime(scheds.DateBegin) <= DateTime.Now
                                     select new { SchedSum = scheds.Value - scheds.Discount }).Sum(s => (double?)(s.SchedSum)) ?? 0),
                                   Note = students.Note,
                               };

                if (idGroup != null) listStudents = listStudents.Where(d => d.GroupId == idGroup);
                if (lastname != null && lastname != "") listStudents = listStudents.Where(e => e.Lastname.StartsWith(lastname));
                if (phoneNumber != null && phoneNumber != "") listStudents = listStudents.Where(a => a.Phone1.IndexOf(phoneNumber) > -1 || a.Phone2.IndexOf(phoneNumber) > -1 || a.ParentPhone.IndexOf(phoneNumber) > -1);
                if (idYear != null) listStudents = listStudents.Where(e => e.YearId == idYear);
                if (idStatus != null) listStudents = listStudents.Where(e => e.StatusId == idStatus);
                if (idSchool != null) listStudents = listStudents.Where(f => f.SchoolId == idSchool);
                if (cl != null) listStudents = listStudents.Where(x => x.Class == cl);
                if (shift != null) listStudents = listStudents.Where(g => g.Shift == shift.ToString());

                if (activeGroups == "Активные")
                    listStudents = listStudents.Where(a => a.Activity != "Закрытые");
                else listStudents = listStudents.Where(a => a.Activity == activeGroups);

                //фильтрация по праву доступа к группам
                if (!CurrentSession.CurrentUser.AllAccessGroups)
                {
                    var listOpenGroups = db.AccessGroups.Where(a => a.WorkerId == CurrentSession.CurrentUser.Id).Select(a => a.GroupId).ToList();
                    listStudents = listStudents.Where(a => listOpenGroups.Contains(a.GroupId));
                }


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
                        case "Phone1":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.Phone1);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.Phone1);
                                }
                                break;
                            }
                        case "Phone2":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.Phone2);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.Phone2);
                                }
                                break;
                            }
                        case "ParentPhone":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.ParentPhone);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.ParentPhone);
                                }
                                break;
                            }
                        case "BirthDate":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.BirthDate);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.BirthDate);
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
                        case "Shift":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.Shift);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.Shift);
                                }
                                break;
                            }
                        case "Note":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.Note);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.Note);
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
                        case "DateEnrollment":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.DateEnrollment);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.DateEnrollment);
                                }
                                break;
                            }
                        case "Saldo":
                            {
                                if (sortOrder == SortOrder.Ascending)
                                {
                                    listStudents = listStudents.OrderBy(x => x.Saldo);
                                }
                                else
                                {
                                    listStudents = listStudents.OrderByDescending(x => x.Saldo);
                                }
                                break;
                            }
                    }
                }
                else
                    listStudents = listStudents.OrderBy(f => f.Lastname);

                dgvStudents.DataSource = listStudents.ToList();

                if (sortOrder != SortOrder.None)
                    dgvStudents.Columns[sortColumn].HeaderCell.SortGlyphDirection = (SortOrder)sortOrder;

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
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, tbPhoneNumber.Text, (int?)cbStatuses.SelectedValue, (int?)cbSchools.SelectedValue, (int?)cbClasses.SelectedValue,
                cbShifts.SelectedItem, cbActivityGroup.Text, sortColumn, sortOrder);
        }
        private void cbGroups_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, tbPhoneNumber.Text, (int?)cbStatuses.SelectedValue, (int?)cbSchools.SelectedValue, (int?)cbClasses.SelectedValue,
                cbShifts.SelectedItem, cbActivityGroup.Text, sortColumn, sortOrder);
        }

        private void cbClasses_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, tbPhoneNumber.Text, (int?)cbStatuses.SelectedValue, (int?)cbSchools.SelectedValue, (int?)cbClasses.SelectedValue,
                cbShifts.SelectedItem, cbActivityGroup.Text, sortColumn, sortOrder);
        }

        private void cbShifts_SelectionChangeCommitted(object sender, EventArgs e)
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


            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, tbPhoneNumber.Text, (int?)cbStatuses.SelectedValue, (int?)cbSchools.SelectedValue, (int?)cbClasses.SelectedValue,
                cbShifts.SelectedItem, cbActivityGroup.Text, sortColumn, sortOrder);
        }

        private void cbSchools_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, tbPhoneNumber.Text, (int?)cbStatuses.SelectedValue, (int?)cbSchools.SelectedValue, (int?)cbClasses.SelectedValue,
                cbShifts.SelectedItem, cbActivityGroup.Text, sortColumn, sortOrder);
        }

        private void dgvStudents_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            currentRow = e.RowIndex;
            if (currentRow != -1)
            {
                var idStudent = Convert.ToInt32(dgvStudents.Rows[currentRow].Cells["StudentId"].Value);
                var studentForm = new StudentForm(idStudent, true);
                studentForm.MdiParent = this.MdiParent;
                studentForm.Show();

                //Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, (int?)cbStatuses.SelectedValue, (int?)cbSchools.SelectedValue, (int?)cbClasses.SelectedValue,
                //cbShifts.SelectedItem, cbActivityGroup.Text, sortColumn, sortOrder);

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

        private void cbStatuses_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //фильтр слушателей
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, tbPhoneNumber.Text, (int?)cbStatuses.SelectedValue, (int?)cbSchools.SelectedValue, (int?)cbClasses.SelectedValue,
                cbShifts.SelectedItem, cbActivityGroup.Text, sortColumn, sortOrder);
            //фильтр образовательных учреждений
            var idStatus = Convert.ToInt32(cbStatuses.SelectedValue);
            SelectStatus(idStatus);
        }

        private void SelectStatus(int? idStatus)
        {
            try
            {
                if (idStatus != null)
                    schools = db.Schools.Where(a => a.StatusId == idStatus && a.IsRemoved == false).ToList();
                else
                    schools = db.Schools.Where(a => a.IsRemoved == false).ToList();
                if (schools.Count != 0)
                {
                    cbSchools.Enabled = cbClasses.Enabled = cbShifts.Enabled = true;
                    cbSchools.DataSource = schools;
                    cbSchools.DisplayMember = "Name";
                    cbSchools.ValueMember = "Id";
                    cbSchools.SelectedIndex = -1;
                }
                else
                {
                    cbSchools.Enabled = cbClasses.Enabled = cbShifts.Enabled = false;
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
            if (dgvStudents.Rows.Count != 0)
            {
                var exportStudent = new Report();
                exportStudent.ExportExcelStudent(dgvStudents, false);
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
                                     Address = students.Street.Name + ((!String.IsNullOrEmpty(students.House)) ? ", д." + students.House : "") + ((students.Float != null) ? ", кв." + students.Float : ""),
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
                                     SaldoNow = String.Empty,

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
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, tbPhoneNumber.Text, (int?)cbStatuses.SelectedValue, (int?)cbSchools.SelectedValue, (int?)cbClasses.SelectedValue,
                cbShifts.SelectedItem, cbActivityGroup.Text, sortColumn, sortOrder);
            grid.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
        }

        private void cbTypeGroup_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, tbPhoneNumber.Text, (int?)cbStatuses.SelectedValue, (int?)cbSchools.SelectedValue, (int?)cbClasses.SelectedValue,
                            cbShifts.SelectedItem, cbActivityGroup.Text, sortColumn, sortOrder);
        }

        private void ListStudentsForm_Activated(object sender, EventArgs e)
        {
            if (currentRow != -1)
            {
                Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, tbPhoneNumber.Text, (int?)cbStatuses.SelectedValue, (int?)cbSchools.SelectedValue, (int?)cbClasses.SelectedValue,
                cbShifts.SelectedItem, cbActivityGroup.Text, sortColumn, sortOrder);

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

        private void чисткаТелефоновToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string pattern = @"\D+";
            //var lstStudent = db.Students.ToList();
            //foreach (var student in lstStudent)
            //{
            //    var re = new Regex(pattern);
            //    //student.StudentPhone = re.Replace(student.StudentPhone, "");

            //    if (student.StudentPhone2 != null)
            //        student.StudentPhone2 = re.Replace(student.StudentPhone2, "");

            //    if (student.ParentsPhone != null)
            //        student.ParentsPhone = re.Replace(student.ParentsPhone, "");
            //    db.Entry(student).State = EntityState.Modified;
            //}
            //db.SaveChanges();
        }

        private void tbPhoneNumber_TextChanged(object sender, EventArgs e)
        {
            Filter((int?)cbGroups.SelectedValue, (int?)cbTypeGroup.SelectedValue, tbLastname.Text, tbPhoneNumber.Text, (int?)cbStatuses.SelectedValue, (int?)cbSchools.SelectedValue, (int?)cbClasses.SelectedValue,
                            cbShifts.SelectedItem, cbActivityGroup.Text, sortColumn, sortOrder);
        }
    }
}