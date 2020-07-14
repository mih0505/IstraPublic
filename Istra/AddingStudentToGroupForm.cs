using Istra.Entities;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Istra
{
    public partial class AddingStudentToGroupForm : Form
    {
        IstraContext db = new IstraContext();
        IQueryable<AddingStudents> listStudents;
        int groupId;
        public AddingStudentToGroupForm(int idGroup)
        {
            groupId = idGroup;
            InitializeComponent();
        }

        private void AddingStudentToGroupForm_Load(object sender, EventArgs e)
        {
            try
            {
                //загрузка списка слушателей
                Filter(null);

                //оформление таблицы
                dgvListStudents.Columns["StudentId"].Visible = dgvListStudents.Columns["SchoolId"].Visible =
                    dgvListStudents.Columns["StatusId"].Visible = false;

                dgvListStudents.Columns["Lastname"].HeaderText = "Фамилия";
                dgvListStudents.Columns["Firstname"].HeaderText = "Имя";
                dgvListStudents.Columns["Middlename"].HeaderText = "Отчество";
                dgvListStudents.Columns["Status"].HeaderText = "Статус";
                dgvListStudents.Columns["Phone1"].HeaderText = "Тел1";
                dgvListStudents.Columns["Phone2"].HeaderText = "Тел2";
                dgvListStudents.Columns["BirthDate"].HeaderText = "Дата рожд.";
                dgvListStudents.Columns["School"].HeaderText = "Школа";
                dgvListStudents.Columns["Class"].HeaderText = "Класс";
                dgvListStudents.Columns["Shift"].HeaderText = "Смена";
                dgvListStudents.Columns["Note"].HeaderText = "Примечание";
                dgvListStudents.Columns["Sex"].HeaderText = "Пол";

                dgvListStudents.Columns["School"].AutoSizeMode = dgvListStudents.Columns["Class"].AutoSizeMode =
                    dgvListStudents.Columns["Shift"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                dgvListStudents.Columns["BirthDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvListStudents.Columns["Phone1"].DefaultCellStyle.Format = "(###) ###-####";

                DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                dgvListStudents.Columns.Add(btn);
                btn.HeaderText = "Добавление";
                btn.Text = "Добавить";
                btn.Name = "btn";
                btn.UseColumnTextForButtonValue = true;
            }
            catch (Exception ex)
            {
                string methodName = ex.TargetSite + ";\r\n" + ex.StackTrace;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Filter(string lastname)
        {
            try
            {
                var listExclude = db.Enrollments.Where(a => a.GroupId == groupId && a.DateExclusion == null).Select(a => a.StudentId).ToList();

                listStudents = from students in db.Students
                               join statuses in db.Statuses on students.StatusId equals statuses.Id
                               join schools in db.Schools on students.SchoolId equals schools.Id into school
                               from years in school.DefaultIfEmpty()
                               select new AddingStudents
                               {
                                   StudentId = students.Id,
                                   SchoolId = students.SchoolId,
                                   StatusId = students.StatusId,
                                   Lastname = students.Lastname,
                                   Firstname = students.Firstname,
                                   Middlename = students.Middlename,
                                   Status = statuses.Name,
                                   Phone1 = students.StudentPhone,
                                   Phone2 = students.StudentPhone2,
                                   Sex = students.Sex,
                                   BirthDate = students.DateOfBirth,
                                   School = students.School.Name,
                                   Class = students.Class,
                                   Shift = students.Shift,
                                   Note = students.Note
                               };

                if (lastname != null && lastname != "") listStudents = listStudents.Where(e => e.Lastname.StartsWith(lastname));
                dgvListStudents.DataSource = listStudents.Where(a => !listExclude.Contains(a.StudentId)).OrderBy(f => f.Lastname).ToList();

                
            }
            catch (Exception ex)
            {
                string methodName = ex.TargetSite + ";\r\n" + ex.StackTrace;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbLastname_TextChanged(object sender, EventArgs e)
        {
            Filter(tbLastname.Text);
        }

        private void dgvListStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvListStudents.Columns["btn"].Index)
                {
                    //определяем не записан ли уже студент в группу
                    int idStudent = Convert.ToInt32(dgvListStudents.Rows[e.RowIndex].Cells["StudentId"].Value);
                    var validatingStudent = db.Enrollments.FirstOrDefault(a => a.GroupId == groupId && a.StudentId == idStudent);
                    if (validatingStudent == null)
                    {
                        db.Enrollments.Add(new Enrollment { StudentId = idStudent, GroupId = groupId, DateEnrollment = DateTime.Now, EnrollId = CurrentSession.CurrentUser.Id });
                        db.SaveChanges();
                    }
                    else
                        MessageBox.Show("Выбранный учащийся уже записан в эту группу", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //обновляем список, очищаем textbox
                    tbLastname.Text = "";
                    Filter(null);
                }
            }
            catch (Exception ex)
            {
                string methodName = ex.TargetSite + ";\r\n" + ex.StackTrace;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvListStudents_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //определяем не записан ли студент уже в эту группу
                int idStudent = Convert.ToInt32(dgvListStudents.Rows[e.RowIndex].Cells["StudentId"].Value);
                var validatingStudent = db.Enrollments.FirstOrDefault(a => a.GroupId == groupId && a.StudentId == idStudent);
                if (validatingStudent == null)
                {
                    db.Enrollments.Add(new Enrollment { StudentId = idStudent, GroupId = groupId, DateEnrollment = DateTime.Now, EnrollId = CurrentSession.CurrentUser.Id });
                    db.SaveChanges();
                }
                else
                {
                    if (validatingStudent.DateExclusion != null)
                    {
                        validatingStudent.DateExclusion = null;
                        validatingStudent.MonthExclusionId = null;
                        validatingStudent.CauseId = null;
                        validatingStudent.ExclusionId = null;

                        db.Entry(validatingStudent).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                //    MessageBox.Show("Выбранный учащийся уже записан в эту группу", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //обновляем список, очищаем textbox
                tbLastname.Text = "";
                Filter(null);
            }
            catch (Exception ex)
            {
                string methodName = ex.TargetSite + ";\r\n" + ex.StackTrace;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
