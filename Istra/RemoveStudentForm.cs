using Istra.Entities;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Istra
{
    public partial class RemoveStudentForm : Form
    {
        IstraContext db = new IstraContext();
        public Enrollment enroll;
        public RemoveStudentForm(int enrollId)
        {
            InitializeComponent();

            try
            {
                enroll = db.Enrollments.Find(enrollId);

                var causes = db.Causes.OrderBy(a => a.Name).ToList();
                cbList.DataSource = causes;
                cbList.DisplayMember = "Name";
                cbList.ValueMember = "Id";

                var months = db.Months.ToList();
                cbMonths.DataSource = months;
                cbMonths.DisplayMember = "Name";
                cbMonths.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveStudentForm_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbGroups.DataSource = null;
                cbGroups.Items.Clear();
                cbGroups.Enabled = chbSaveGrades.Enabled = false;
                cbList.Enabled = cbMonths.Enabled = true;

                var causes = db.Causes.OrderBy(a => a.Name).ToList();
                cbList.DataSource = causes;
                cbList.DisplayMember = "Name";
                cbList.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                cbList.DataSource = null;
                cbList.Items.Clear();
                cbMonths.SelectedIndex = -1;
                cbMonths.Enabled = cbList.Enabled = false;
                cbGroups.Enabled = chbSaveGrades.Enabled = true;

                var enrollsStudent = db.Enrollments.Where(a => a.StudentId == enroll.StudentId && a.DateExclusion == null).Select(a => a.GroupId).ToList();
                var groups = db.Groups.Include("Activity").OrderBy(a => a.Name).Where(a => (a.Activity.Name == "Текущие" || a.Activity.Name == "В наборе") && a.Course.Name != "Индивид."
                        && !enrollsStudent.Contains(a.Id)).Select(a => new { Id = a.Id, Name = a.Name + " | " + a.Year.Name }).ToList();
                cbGroups.DataSource = groups;
                cbGroups.DisplayMember = "Name";
                cbGroups.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbExclude.Checked)
                {
                    //добавление причины отчисления если ее нет и сохранение
                    if (cbList.Text != String.Empty)
                    {
                        Cause currentCause;
                        if (cbList.SelectedIndex == -1 && cbList.Text != String.Empty)
                        {
                            cbList.Text = cbList.Text.Substring(0, 1).ToUpper() + cbList.Text.Substring(1);
                            currentCause = db.Causes.FirstOrDefault(a => a.Name == cbList.Text && a.IsRemoved == false);
                            if (currentCause == null)
                            {
                                var newCause = new Cause() { Name = cbList.Text };
                                db.Causes.Add(newCause);
                                db.SaveChanges();
                            }
                            currentCause = db.Causes.FirstOrDefault(a => a.Name == cbList.Text && a.IsRemoved == false);
                            enroll.CauseId = currentCause.Id;
                        }
                        else enroll.CauseId = Convert.ToInt32(cbList.SelectedValue);

                        enroll.ExclusionId = CurrentSession.CurrentUser.Id;
                        enroll.DateExclusion = DateTime.Now;
                        enroll.MonthExclusionId = Convert.ToInt32(cbMonths.SelectedValue);
                        enroll.Note = "Отчислен";
                        db.Entry(enroll).State = System.Data.Entity.EntityState.Modified;
                        int i = db.SaveChanges();

                        Close();
                    }
                    else
                        MessageBox.Show("Не указана причина отчисления", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //добавление записи о старой группе
                    var oldEnrollment = new Enrollment
                    {
                        DateEnrollment = enroll.DateEnrollment,
                        Transfer = true,
                        DateExclusion = DateTime.Now,
                        ExclusionId = CurrentSession.CurrentUser.Id,
                        Note = string.Format("Переведен в группу {0}", cbGroups.Text),
                        StudentId = enroll.StudentId,
                        GroupId = enroll.GroupId,
                        EnrollId = enroll.EnrollId,
                        NumberDocument = enroll.NumberDocument,
                        PrivilegeId = enroll.PrivilegeId
                    };

                    db.Enrollments.Add(oldEnrollment);

                    //обновление текущей записи о группе (переписываем данные о группе при переводе, чтобы платежи сохранились)
                    var oldGroup = db.Groups.Find(enroll.GroupId);
                    enroll.Note = string.Format("Переведен из группы {0}", oldGroup.Name);
                    //enroll.DateEnrollment = DateTime.Now;
                    enroll.EnrollId = CurrentSession.CurrentUser.Id;
                    enroll.GroupId = Convert.ToInt32(cbGroups.SelectedValue);
                    enroll.ExclusionId = null;
                    enroll.DateExclusion = null;
                    enroll.PrivilegeId = null;
                    enroll.NumberDocument = null;
                    db.Entry(enroll).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    //обновление занятий и оценок при переводе
                    if (chbSaveGrades.Checked)
                    {
                        //Получение списка занятий из новой группы
                        var lessonsNew = db.Lessons.Where(a => a.GroupId == enroll.GroupId).OrderBy(a => a.Number).ToList();
                        //получение оценок из старой группы
                        var gradesOld = db.Studies.Include("Lesson").Where(a => a.GroupId == oldEnrollment.GroupId && a.StudentId == enroll.StudentId)
                            .OrderBy(a => a.Lesson.Number).ToList();
                        //замена занятий в списке оценок
                        for (var j = 0; j < lessonsNew.Count; j++)
                            for (var i = 0; i < gradesOld.Count; i++)
                            {
                                if (gradesOld[i].Lesson.Number == lessonsNew[j].Number)
                                {
                                    gradesOld[i].LessonId = lessonsNew[j].Id;
                                    gradesOld[i].GroupId = Convert.ToInt32(enroll.GroupId);
                                    break;
                                }
                            }
                    }
                    //////////удаление старого и добавление нового графика платежей///////////
                    //получение старого графика
                    var scheds = db.Schedules.Where(a => a.EnrollmentId == enroll.Id).ToList();
                    //перезапись id записи в группу для старого графика платежей
                    foreach (var s in scheds)
                    {
                        s.EnrollmentId = oldEnrollment.Id;
                        db.Entry(s).State = System.Data.Entity.EntityState.Modified;
                    }
                    //добавление нового графика происходит в форме студента при записи в группу                    
                    db.SaveChanges();
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
    }
}
