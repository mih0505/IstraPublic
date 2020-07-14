using Istra.Entities;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace Istra
{
    public partial class LessonForm : Form
    {
        public bool addLesson = false;

        IstraContext db = new IstraContext();
        public Group group;
        public Lesson lesson;
        public int idLesson { get; set; }
        public DateTime dateLesson { get; set; }

        public LessonForm(Group gr, Lesson les)  //true-new lesson, false-edit lesson
        {
            InitializeComponent();

            group = gr;
            lesson = les;
            try
            {
                if (lesson == null)
                {
                    this.Text = "Добавить занятие";
                    addLesson = true;
                }
                else
                {
                    this.Text = "Занятие № " + lesson.Number;
                }

                //заполнение корпусов
                var housings = db.Housings.ToList();
                cbCurrentHousing.DataSource = housings;
                cbCurrentHousing.DisplayMember = "Name";
                cbCurrentHousing.ValueMember = "Id";

                //заполнение  преподавателей
                var teachers = db.Workers.Where(a => a.Role.Name == "Преподаватель").ToList();
                cbCurrentTeacher.DataSource = teachers;
                cbCurrentTeacher.DisplayMember = "LastnameFM";
                cbCurrentTeacher.ValueMember = "Id";

                //заполнение списка тем по курсу
                FilterTopics(null);
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterTopics(string topic)
        {
            try
            {
                if (topic != null && topic != String.Empty)
                {
                    var topicList = db.Topics.Where(a => a.Name.Contains(topic) && a.CourseId == group.CourseId).OrderBy(a => a.Name).ToList();
                    lbTopics.DataSource = topicList;
                    lbTopics.DisplayMember = "Name";
                    lbTopics.ValueMember = "Id";
                }
                else
                {
                    var topicList = db.Topics.Where(a => a.CourseId == group.CourseId).OrderBy(a => a.Name).ToList();
                    lbTopics.DataSource = topicList;
                    lbTopics.DisplayMember = "Name";
                    lbTopics.ValueMember = "Id";
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
                    cbCurrentClass.DataSource = classes;
                    cbCurrentClass.DisplayMember = "Name";
                    cbCurrentClass.ValueMember = "Id";
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

        private void LessonForm_Load(object sender, EventArgs e)
        {
            try
            {
                //заполнение данных о занятии по умолчанию, для нового занятия
                if (group != null && lesson == null)
                {
                    nudDurationLesson.Value = group.DurationLesson;
                    var currentHousing = db.Classes.FirstOrDefault(a => a.Id == group.ClassId);
                    cbCurrentHousing.SelectedValue = currentHousing.HousingId;
                    SelectHousing(currentHousing.HousingId);
                    cbCurrentClass.SelectedValue = group.ClassId;
                    cbCurrentTeacher.SelectedValue = group.TeacherId;
                }

                //заполнение данных о занятии для редактирования
                if (group != null && lesson != null)
                {
                    dtpDateLesson.Value = lesson.Date;
                    nudDurationLesson.Value = lesson.DurationLesson;

                    var currentHousing = db.Classes.FirstOrDefault(a => a.Id == lesson.ClassId);
                    cbCurrentHousing.SelectedValue = currentHousing.HousingId;
                    SelectHousing(currentHousing.HousingId);

                    cbCurrentClass.SelectedValue = lesson.ClassId;
                    cbCurrentTeacher.SelectedValue = lesson.TeacherId;

                    if (lesson.TopicId != null)
                    {
                        var currentTopic = db.Topics.FirstOrDefault(a => a.Id == lesson.TopicId);
                        tbCurrentTopic.Text = currentTopic.Name;
                    }
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

        private void cbCurrentHousing_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SelectHousing((int)cbCurrentHousing.SelectedValue);
        }

        private void tbCurrentTopic_TextChanged(object sender, EventArgs e)
        {
            FilterTopics(tbCurrentTopic.Text);
        }

        private void lbTopics_DoubleClick(object sender, EventArgs e)
        {
            if (lbTopics.Items.Count != 0 && lbTopics.SelectedIndex != -1)
            {
                tbCurrentTopic.Text = lbTopics.GetItemText(lbTopics.SelectedItem);
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (addLesson)
                {
                    lesson = new Lesson();
                    lesson.Number = (db.Lessons.Where(a => a.GroupId == group.Id).Count() != 0) ? db.Lessons.Where(a => a.GroupId == group.Id).Max(a => a.Number) + 1 : 1;
                }
                lesson.Date = dtpDateLesson.Value;
                lesson.ClassId = Convert.ToInt32(cbCurrentClass.SelectedValue);
                lesson.DurationLesson = Convert.ToByte(nudDurationLesson.Value);
                lesson.GroupId = group.Id;
                lesson.TeacherId = Convert.ToInt32(cbCurrentTeacher.SelectedValue);
                
                //сохранение темы
                if (lbTopics.Items.Count == 0 && tbCurrentTopic.Text != String.Empty)
                {
                    var currentTopic = new Topic { Name = tbCurrentTopic.Text, CourseId = group.CourseId };
                    db.Topics.Add(currentTopic);
                    db.SaveChanges();
                    lesson.TopicId = currentTopic.Id;
                }
                else if (tbCurrentTopic.Text == String.Empty)
                {
                    lesson.TopicId = null;
                }
                else
                {
                    lbTopics.SelectedIndex = 0;
                    lesson.TopicId = Convert.ToInt32(lbTopics.SelectedValue);
                }

                if (addLesson)
                {
                    db.Lessons.Add(lesson);
                }
                else
                {
                    db.Entry(lesson).State = EntityState.Modified;
                }
                db.SaveChanges();

                this.idLesson = lesson.Number;
                this.dateLesson = lesson.Date;
                this.DialogResult = DialogResult.OK;
                this.Close();
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
