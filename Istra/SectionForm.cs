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
    public partial class SectionForm : Form
    {
        private IstraContext db = new IstraContext();
        private Group group;
        private int sectionIndex;

        public Section Section { get; set; }

        public SectionForm(Group group)
        {
            InitializeComponent();

            this.group = group;
        }

        public SectionForm(Group group, Section section, int sectionIndex)
        {
            InitializeComponent();

            this.group = group;
            Section = section;
            this.sectionIndex = ++sectionIndex;
        }

        private void SectionForm_Load(object sender, EventArgs e)
        {
            try
            {
                Text = "Добавление нового раздела";
                var lessonsDates = db.Lessons.Where(Lesson => (group.Id == Lesson.GroupId)).ToList();
                cbLesson.DataSource = lessonsDates;
                cbLesson.DisplayMember = "Date";
                cbLesson.ValueMember = "Id";

                if (Section == null)
                    return;

                Text = $"Редактирование раздела №{sectionIndex} за {Section.Lesson.Date.ToShortDateString()}";
                cbCredit.Checked = Section.IsCredit;
                cbTypeGrade.Checked = Section.IsTypeGrade;
                tbName.Text = Section.Name;
                tbDuration.Text = Section.Duration.ToString();
                cbLesson.SelectedItem = lessonsDates.FirstOrDefault(Lesson => (Section.Lesson.Date == Lesson.Date));
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
                if (!int.TryParse(tbDuration.Text, out int duration))
                {
                    MessageBox.Show(this, "Значение в поле продолжительности раздела указано неверно\nЗаполните его и повторите попытку", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                var lesson = db.Lessons.Where(Lesson => (group.Id == Lesson.GroupId)).ToList().FirstOrDefault(Lesson => ((cbLesson.SelectedItem as Lesson).Date == Lesson.Date));

                if (lesson == null)
                {
                    MessageBox.Show(this, "Выбранного урока не существует в базе данных\nСоздайте урок и повторите попытку снова", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                Section = new Section
                {
                    IsCredit = cbCredit.Checked,
                    IsTypeGrade = cbTypeGrade.Checked,
                    Name = tbName.Text,
                    Duration = duration,
                    CourseId = group.CourseId,
                    GroupId = group.Id,
                    LessonId = lesson.Id
                };

                DialogResult = DialogResult.OK;

                Close();
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void IsDataChange(object sender, EventArgs e)
        {
            btOK.Enabled = (tbName.Text != String.Empty) && (tbDuration.Text != String.Empty);
        }
    }
}
