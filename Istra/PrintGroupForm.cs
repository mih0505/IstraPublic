using Istra.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Istra
{
    public partial class PrintGroupForm : Form
    {
        IstraContext db = new IstraContext();
        private List<CheckBox> chkbPrintListAlbum, chkbPrintListBook;
        public int countDisableCheckBox = 0;
        int id;
        bool countStudents;
        Group group;
        DataGridView dgvJournal;
        public PrintGroupForm(int idGroup, bool count, DataGridView dgv)
        {

            InitializeComponent();
            countStudents = count;
            id = idGroup;
            dgvJournal = dgv;
            chkbPrintListAlbum = new List<CheckBox>();
            chkbPrintListBook = new List<CheckBox>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var Control in Controls)
            {
                if (ReferenceEquals(new CheckBox().GetType(), Control.GetType()))
                {
                    if ((Control as CheckBox).Checked)
                    {
                        if ((Control as CheckBox) == chkbJournal)
                        {
                            chkbPrintListAlbum.Add(Control as CheckBox);
                        }
                        else
                        {
                            chkbPrintListBook.Add(Control as CheckBox);
                        }
                    }
                }
            }
            try
            {
                PrintDocument document = new PrintDocument();
                //PrintPreviewDialog documentPreview = new PrintPreviewDialog();
                document.DefaultPageSettings.PaperSize.RawKind = (int)PaperKind.A4;
                document.DefaultPageSettings.Landscape = chkbPrintListAlbum.Count != 0;
                document.DocumentName = "Документация";
                //documentPreview.Document = document;
                document.PrintPage += new PrintPageEventHandler(Print);

                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = document;
                if (printDialog.ShowDialog() == DialogResult.OK) document.Print();


                Close();
                //documentPreview.ShowDialog();
            }
            catch (Exception ex)
            {
                var m = new StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Print(object sender, PrintPageEventArgs e)
        {
            if (chkbPrintListAlbum.Count != 0)
            {
                switch (chkbPrintListAlbum[0].Name)
                {
                    case "chkbJournal":
                        PrintJournal(sender, e);
                        break;
                }

                chkbPrintListAlbum.Remove(chkbPrintListAlbum[0]);
                e.PageSettings.Landscape = chkbPrintListAlbum.Count != 0;
                e.HasMorePages = chkbPrintListAlbum.Count != 0 || chkbPrintListBook.Count != 0;
                return;
            }

            if (chkbPrintListBook.Count != 0)
            {
                switch (chkbPrintListBook[0].Name)
                {
                    case "chkbListTopic":
                        PrintListTopic(sender, e);
                        break;

                    case "chkbContacts":
                        PrintContacts(sender, e);
                        break;

                    case "chkbStudents":
                        PrintStudents(sender, e);
                        break;
                }

                chkbPrintListBook.Remove(chkbPrintListBook[0]);
                e.HasMorePages = chkbPrintListAlbum.Count != 0 || chkbPrintListBook.Count != 0;
                return;
            }
        }

        private void PrintGroupForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Определение доступной информации для печати
                // Запрещать отправлять журнал на печать, если учащихся в журнале не существует
                if (!countStudents)
                {
                    chkbJournal.Enabled = chkbJournal.Checked = false;
                    countDisableCheckBox++;
                }

                if (CurrentSession.CurrentRole.Name == "Преподаватель")
                {
                    chkbContacts.Enabled = false;
                }

                var lessonsList = db.Lessons.Include("Topic").Where(Lesson => (id == Lesson.GroupId)).OrderBy(Lesson => (Lesson.Date)).ToList();
                // Запрещать отправлять список тем на печать, если их не существует
                if (!countStudents)
                {
                    chkbListTopic.Enabled = chkbListTopic.Checked = false;
                    countDisableCheckBox++;
                }

                var contactsList = db.Enrollments.Include("Student").Where(Enrollment => (id == Enrollment.GroupId && Enrollment.DateExclusion == null))
                    .OrderBy(Enrollment => (Enrollment.Student.Lastname)).ToList();
                // Запрещать отправлять контактные данные на печать, если их не существует
                if (!countStudents)
                {
                    chkbContacts.Enabled = chkbContacts.Checked = false;
                    countDisableCheckBox++;
                }

                var studentList = db.Enrollments.Include("Student").Include("School").Where(a => (a.GroupId == id && a.DateExclusion == null))
                    .Select(Enrollment => (Enrollment.Student)).OrderBy(Student => (Student.Lastname)).ToList();
                // Запрещать отправлять информацию об учащихся на печать, если их не существует
                if (!countStudents)
                {
                    chkbStudents.Enabled = chkbStudents.Checked = false;
                    countDisableCheckBox++;
                }

                if (countDisableCheckBox == 4) button1.Enabled = false;
            }
            catch (Exception ex)
            {
                var m = new StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void PrintStudents(object sender, PrintPageEventArgs e)
        {
            try
            {
                var font = new Font("Arial", 10, FontStyle.Bold);
                var font1 = new Font("Arial", 10, FontStyle.Regular);

                // Выравнивание по горизонтали и вертикали
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                // Выравнивание только по вертикали
                StringFormat stringFormat1 = new StringFormat();
                stringFormat1.LineAlignment = StringAlignment.Center;

                // Выравнивание по горизонтали влево и по вертикали по центру
                StringFormat stringFormat2 = new StringFormat();
                stringFormat2.Alignment = StringAlignment.Near;
                stringFormat2.LineAlignment = StringAlignment.Center;

                group = db.Groups.FirstOrDefault(a => a.Id == id);
                //заполнение данных о группе
                var teacher = db.Workers.Find(group.TeacherId);
                string teach = "Преподаватель: " + teacher.LastnameFM;
                string time = "Расписание: " + group.Days + ";   " +
                    group.Begin.ToShortTimeString() + "-" + group.EndTimeLesson;

                var groupDirection = db.Courses.FirstOrDefault(Course => (Course.Id == group.CourseId)).Name;
                e.Graphics.DrawString("Курс: " + groupDirection, font, Brushes.Black, new Point(50, 50));
                e.Graphics.DrawString(group.Name + "          " + teach + "     " + time,
                    font, Brushes.Black, new Point(50, 70));

                // Прорисовка шапки контактов
                Pen blackPen = new Pen(Color.Black, 1);
                RectangleF[] rectsHeaderTable =
                {
                new RectangleF(30, 90, 30, 60),
                new RectangleF(60, 90, 330, 60),
                new RectangleF(390, 90, 130, 60),
                new RectangleF(520, 90, 130, 60),
                new RectangleF(650, 90, 70, 60)
            };

                var studentList = db.Enrollments.Where(a => (a.GroupId == group.Id && a.DateExclusion == null)).Include("Student")
                    .Select(Enrollment => (Enrollment.Student)).Include("School").OrderBy(Student => (Student.Lastname)).ToList();

                // Нумерация данных
                int studentsCount = studentList.Count();
                RectangleF[] rectsNums = new RectangleF[studentsCount * 2];
                int x = 30, y = 130;
                for (int i = 0; i < studentsCount; i++)
                {
                    rectsNums[i] = new RectangleF(x, y + (20 * (i + 1)), 30, 20);
                    e.Graphics.DrawString((i + 1).ToString(), font1, Brushes.Black, rectsNums[i], stringFormat);
                }

                // Прорисовка имен
                RectangleF[] rectsNames = new RectangleF[studentsCount * 2];
                x = 60; y = 130;
                for (int i = 0; i < studentsCount; i++)
                {
                    rectsNames[i] = new RectangleF(x, y + (20 * (i + 1)), 330, 20);
                    e.Graphics.DrawString(studentList[i].Fullname(), font1, Brushes.Black, rectsNames[i], stringFormat1);
                }

                // Прорисовка даты рождения
                RectangleF[] rectsBirthdays = new RectangleF[studentsCount * 2];
                x = 390; y = 130;
                for (int i = 0; i < studentsCount; i++)
                {
                    rectsBirthdays[i] = new RectangleF(x, y + (20 * (i + 1)), 130, 20);
                    e.Graphics.DrawString(studentList[i].DateOfBirth.ToShortDateString(), font1, Brushes.Black, rectsBirthdays[i], stringFormat);
                }

                // Прорисовка школы
                RectangleF[] rectsSchools = new RectangleF[studentsCount * 2];
                x = 520; y = 130;
                for (int i = 0; i < studentsCount; i++)
                {
                    string schoolName = (studentList[i].School == null) ? String.Empty : studentList[i].School.Name;
                    rectsSchools[i] = new RectangleF(x, y + (20 * (i + 1)), 130, 20);
                    e.Graphics.DrawString(schoolName, font1, Brushes.Black, rectsSchools[i], stringFormat);
                }

                // Прорисовка класса
                RectangleF[] rectsGrades = new RectangleF[studentsCount * 2];
                x = 650; y = 130;
                for (int i = 0; i < studentsCount; i++)
                {
                    string studentGrade = (studentList[i].Class == null) ? String.Empty : studentList[i].Class.Value.ToString();
                    rectsGrades[i] = new RectangleF(x, y + (20 * (i + 1)), 70, 20);
                    e.Graphics.DrawString(studentGrade, font1, Brushes.Black, rectsGrades[i], stringFormat);
                }

                // Вывод на экран
                e.Graphics.DrawString("№", font, Brushes.Black, rectsHeaderTable[0], stringFormat);
                e.Graphics.DrawString("Фамилия, имя и отчество", font, Brushes.Black, rectsHeaderTable[1], stringFormat);
                e.Graphics.DrawString("Дата рождения", font, Brushes.Black, rectsHeaderTable[2], stringFormat);
                e.Graphics.DrawString("Школа", font, Brushes.Black, rectsHeaderTable[3], stringFormat);
                e.Graphics.DrawString("Класс", font, Brushes.Black, rectsHeaderTable[4], stringFormat);
                e.Graphics.DrawRectangles(blackPen, rectsHeaderTable);
                e.Graphics.DrawRectangles(blackPen, rectsNums);
                e.Graphics.DrawRectangles(blackPen, rectsNames);
                e.Graphics.DrawRectangles(blackPen, rectsBirthdays);
                e.Graphics.DrawRectangles(blackPen, rectsSchools);
                e.Graphics.DrawRectangles(blackPen, rectsGrades);
            }
            catch (Exception ex)
            {
                var m = new StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void PrintContacts(object sender, PrintPageEventArgs e)
        {
            try
            {
                var font = new Font("Arial", 10, FontStyle.Bold);
                var font1 = new Font("Arial", 10, FontStyle.Regular);

                // Выравнивание по горизонтали и вертикали
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                // Выравнивание только по вертикали
                StringFormat stringFormat1 = new StringFormat();
                stringFormat1.LineAlignment = StringAlignment.Center;

                // Выравнивание по горизонтали влево и по вертикали по центру
                StringFormat stringFormat2 = new StringFormat();
                stringFormat2.Alignment = StringAlignment.Near;
                stringFormat2.LineAlignment = StringAlignment.Center;

                //заполнение данных о группе
                group = db.Groups.FirstOrDefault(a => a.Id == id);
                var teacher = db.Workers.Find(group.TeacherId);
                string teach = "Преподаватель: " + teacher.LastnameFM;
                string time = "Расписание: " + group.Days + ";   " +
                    group.Begin.ToShortTimeString() + "-" + group.EndTimeLesson;
                e.Graphics.DrawString(group.Name + "             " + teach + "     " + time,
                    font, Brushes.Black, new Point(50, 70));

                // Прорисовка шапки контактов
                Pen blackPen = new Pen(Color.Black, 1);
                RectangleF[] rectsHeaderTable =
                {
                new RectangleF(30, 90, 30, 60),
                new RectangleF(60, 90, 290, 60),
                new RectangleF(350, 90, 130, 60),
                new RectangleF(480, 90, 130, 60),
                new RectangleF(610, 90, 130, 60)
            };

                var contactList = db.Enrollments.Where(Enrollment => (Enrollment.GroupId == group.Id && Enrollment.DateExclusion == null)).Include("Student").OrderBy(Enrollment => (Enrollment.Student.Lastname)).ToList();

                // Нумерация данных
                int studentsCount = contactList.Count();
                RectangleF[] rectsNums = new RectangleF[studentsCount * 2];
                int x = 30, y = 130;
                for (int i = 0; i < studentsCount; i++)
                {
                    rectsNums[i] = new RectangleF(x, y + (20 * (i + 1)), 30, 20);
                    e.Graphics.DrawString((i + 1).ToString(), font1, Brushes.Black, rectsNums[i], stringFormat);
                }

                // Прорисовка имен
                RectangleF[] rectsNames = new RectangleF[studentsCount * 2];
                x = 60; y = 130;
                for (int i = 0; i < studentsCount; i++)
                {
                    rectsNames[i] = new RectangleF(x, y + (20 * (i + 1)), 290, 20);
                    e.Graphics.DrawString(contactList[i].Student.Fullname(), font1, Brushes.Black, rectsNames[i], stringFormat1);
                }

                // Прорисовка мобильного телефона
                RectangleF[] rectsPhones = new RectangleF[studentsCount * 2];
                x = 350; y = 130;
                for (int i = 0; i < studentsCount; i++)
                {
                    rectsPhones[i] = new RectangleF(x, y + (20 * (i + 1)), 130, 20);
                    e.Graphics.DrawString(String.Format("{0:(###) ###-##-##}", contactList[i].Student.StudentPhone), font1, Brushes.Black, rectsPhones[i], stringFormat);
                }

                // Прорисовка дополнительного телефона
                RectangleF[] rectsExPhones = new RectangleF[studentsCount * 2];
                x = 480; y = 130;
                for (int i = 0; i < studentsCount; i++)
                {
                    rectsExPhones[i] = new RectangleF(x, y + (20 * (i + 1)), 130, 20);
                    e.Graphics.DrawString(String.Format("{0:+# (###) ###-##-##}", contactList[i].Student.StudentPhone2), font1, Brushes.Black, rectsExPhones[i], stringFormat);
                }

                // Прорисовка телефона родителя
                RectangleF[] rectsParentsPhone = new RectangleF[studentsCount * 2];
                x = 610; y = 130;
                for (int i = 0; i < studentsCount; i++)
                {
                    rectsParentsPhone[i] = new RectangleF(x, y + (20 * (i + 1)), 130, 20);
                    e.Graphics.DrawString(String.Format("{0:+# (###) ###-##-##}", contactList[i].Student.ParentsPhone), font1, Brushes.Black, rectsParentsPhone[i], stringFormat);
                }

                // Вывод на экран
                e.Graphics.DrawString("№", font, Brushes.Black, rectsHeaderTable[0], stringFormat);
                e.Graphics.DrawString("Фамилия, имя и отчество", font, Brushes.Black, rectsHeaderTable[1], stringFormat);
                e.Graphics.DrawString("Моб. телефон", font, Brushes.Black, rectsHeaderTable[2], stringFormat);
                e.Graphics.DrawString("Доп. телефон", font, Brushes.Black, rectsHeaderTable[3], stringFormat);
                e.Graphics.DrawString("Тел. родителя", font, Brushes.Black, rectsHeaderTable[4], stringFormat);
                e.Graphics.DrawRectangles(blackPen, rectsHeaderTable);
                e.Graphics.DrawRectangles(blackPen, rectsNums);
                e.Graphics.DrawRectangles(blackPen, rectsNames);
                e.Graphics.DrawRectangles(blackPen, rectsPhones);
                e.Graphics.DrawRectangles(blackPen, rectsExPhones);
                e.Graphics.DrawRectangles(blackPen, rectsParentsPhone);
            }
            catch (Exception ex)
            {
                var m = new StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void PrintListTopic(object sender, PrintPageEventArgs e)
        {
            try
            {
                var font = new Font("Arial", 10, FontStyle.Bold);
                var font1 = new Font("Arial", 10, FontStyle.Regular);

                // Выравнивание по горизонтали и вертикали
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                // Выравнивание только по вертикали
                StringFormat stringFormat1 = new StringFormat();
                stringFormat1.LineAlignment = StringAlignment.Center;

                // Выравнивание по горизонтали влево и по вертикали по центру
                StringFormat stringFormat2 = new StringFormat();
                stringFormat2.Alignment = StringAlignment.Near;
                stringFormat2.LineAlignment = StringAlignment.Center;

                //заполнение данных о группе
                group = db.Groups.FirstOrDefault(a => a.Id == id);
                var teacher = db.Workers.Find(group.TeacherId);
                string teach = "Преподаватель: " + teacher.LastnameFM;
                string time = "Расписание: " + group.Days + ";   " +
                    group.Begin.ToShortTimeString() + "-" + group.EndTimeLesson;
                e.Graphics.DrawString(group.Name + "             " + teach + "     " + time,
                    font, Brushes.Black, new Point(50, 20));

                // Прорисовка шапки списка тем
                Pen blackPen = new Pen(Color.Black, 1);
                RectangleF[] rectsHeaderTable =
                {
                new RectangleF(30, 40, 30, 30),
                new RectangleF(60, 40, 120, 30),
                new RectangleF(180, 40, 50, 30),
                new RectangleF(230, 40, 435, 30),
                new RectangleF(665, 40, 120, 30)
            };

                //получение списка тем
                var lessonsList = db.Lessons.Include("Topic").Where(Lesson => (group.Id == Lesson.GroupId)).OrderBy(Lesson => (Lesson.Date)).ToList();
                //получение количества скрытых занятий в журнале
                var unvisibleLesson = (int)group.UnvisibleLessons;
                // Нумерация тем
                int lessonsCount = 36;// group.DurationCourse / group.DurationLesson;
                RectangleF[] rectsNums = new RectangleF[lessonsCount * 2];
                int x = 30, y = 40;
                for (int i = 0; i < lessonsCount; i++)
                {
                    rectsNums[i] = new RectangleF(x, y + (30 * (i + 1)), 30, 30);
                    if(i < (lessonsList.Count - unvisibleLesson))
                        e.Graphics.DrawString(lessonsList[i+unvisibleLesson].Number.ToString(), font1, Brushes.Black, rectsNums[i], stringFormat);
                }

                // Прорисовка дат
                RectangleF[] rectsDates = new RectangleF[lessonsCount * 2];
                x = 60; y = 40;
                for (int i = 0; i < lessonsCount; i++)
                {
                    rectsDates[i] = new RectangleF(x, y + (30 * (i + 1)), 120, 30);
                    try
                    {
                        e.Graphics.DrawString(lessonsList[i + unvisibleLesson].Date.ToShortDateString(), font1, Brushes.Black, rectsDates[i], stringFormat);
                    }
                    catch { continue; }
                }

                // Прорисовка продолжительности занятий
                RectangleF[] rectsLength = new RectangleF[lessonsCount * 2];
                x = 180; y = 40;
                for (int i = 0; i < lessonsCount; i++)
                {
                    rectsLength[i] = new RectangleF(x, y + (30 * (i + 1)), 50, 30);
                    try
                    {
                        //string lessonLength = lessonsList[i + unvisibleLesson].DurationLesson.ToString();
                        e.Graphics.DrawString(lessonsList[i + unvisibleLesson].DurationLesson.ToString(), font1, Brushes.Black, rectsLength[i], stringFormat);
                    }
                    catch { continue; }
                }

                // Прорисовка тем
                RectangleF[] rectsTopics = new RectangleF[lessonsCount * 2];
                x = 230; y = 40;
                for (int i = 0; i < lessonsCount; i++)
                {
                    rectsTopics[i] = new RectangleF(x, y + (30 * (i + 1)), 435, 30);
                    try
                    {
                        string lessonName = (lessonsList[i + unvisibleLesson].Topic == null) ? String.Empty : lessonsList[i + unvisibleLesson].Topic.Name;
                        e.Graphics.DrawString(lessonName, font1, Brushes.Black, rectsTopics[i], stringFormat2);
                    }
                    catch { continue; }
                }

                // Прорисовка подписи
                RectangleF[] rectsSigns = new RectangleF[lessonsCount * 2];
                x = 665; y = 40;
                for (int i = 0; i < lessonsCount; i++)
                    rectsSigns[i] = new RectangleF(x, y + (30 * (i + 1)), 120, 30);

                // Вывод на экран
                e.Graphics.DrawString("№", font, Brushes.Black, rectsHeaderTable[0], stringFormat);
                e.Graphics.DrawString("Дата", font, Brushes.Black, rectsHeaderTable[1], stringFormat);
                e.Graphics.DrawString("Часы", font, Brushes.Black, rectsHeaderTable[2], stringFormat);
                e.Graphics.DrawString("Тема", font, Brushes.Black, rectsHeaderTable[3], stringFormat);
                e.Graphics.DrawString("Подпись", font, Brushes.Black, rectsHeaderTable[4], stringFormat);
                e.Graphics.DrawRectangles(blackPen, rectsHeaderTable);
                e.Graphics.DrawRectangles(blackPen, rectsNums);
                e.Graphics.DrawRectangles(blackPen, rectsDates);
                e.Graphics.DrawRectangles(blackPen, rectsLength);
                e.Graphics.DrawRectangles(blackPen, rectsTopics);
                e.Graphics.DrawRectangles(blackPen, rectsSigns);
            }
            catch (Exception ex)
            {
                var m = new StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkbJournal_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void PrintJournal(object sender, PrintPageEventArgs e)
        {
            try
            {
                var font = new Font("Arial", 10, FontStyle.Bold);
                var font1 = new Font("Arial", 10, FontStyle.Regular);

                // Выравнивание по центрам
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                // Выравнивание по центру вертикали
                StringFormat stringFormat1 = new StringFormat();
                stringFormat1.LineAlignment = StringAlignment.Center;

                //заполнение данных о группе
                group = db.Groups.FirstOrDefault(a => a.Id == id);
                var teacher = db.Workers.Find(group.TeacherId);
                string teach = "Преподаватель: " + teacher.LastnameFM;
                string time = "Расписание: " + group.Days + ";   " +
                    group.Begin.ToShortTimeString() + "-" + group.EndTimeLesson;
                e.Graphics.DrawString(group.Name + "             " + teach + "     " + time,
                    font, Brushes.Black, new Point(50, 70));

                // Прорисовка шапки журнала
                Pen blackPen = new Pen(Color.Black, 1);
                RectangleF[] rectsHeaderTable =
                {
                new RectangleF(20, 90, 25, 75),
                new RectangleF(45, 90, 270, 75)
                };
                // Даты занятий
                int countColumns = 40;//dgvJournal.Columns.Count;
                RectangleF[] rectsLessonsHeader = new RectangleF[countColumns * 2];
                int x = 270; int y = 90;
                int delta = 0;//смещение на количество скрытых занятий
                for (int i = 4; i < countColumns + delta; i++)
                {
                    if (i < countColumns + delta && i < dgvJournal.Columns.Count)
                        if (dgvJournal.Columns[i].Visible != false)
                        {
                            rectsLessonsHeader[i - 3 - delta] = new RectangleF(x + (22 * (i - 2 - delta)), y, 22, 55);
                            rectsLessonsHeader[i - 3 - delta + countColumns - 3] = new RectangleF(x + (22 * (i - 2 - delta)), y + 55, 22, 20);
                            if (dgvJournal.Columns[i].HeaderText.Contains("\r\n"))
                            {
                                string date = dgvJournal.Columns[i].HeaderText.Substring(0, dgvJournal.Columns[i].HeaderText.LastIndexOf("\r\n"));
                                e.Graphics.DrawString(date, font1, Brushes.Black, rectsLessonsHeader[i - 3 - delta], stringFormat);

                                var l = dgvJournal.Columns[i].Name;
                                e.Graphics.DrawString(l, font1, Brushes.Black, rectsLessonsHeader[i - 3 - delta + countColumns - 3], stringFormat);
                            }
                        }
                        else delta++;
                    else
                    {//прорисовка пустых ячеек
                        rectsLessonsHeader[i - 3 - delta] = new RectangleF(x + (22 * (i - 2 - delta)), y, 22, 55);
                        //e.Graphics.DrawString("", font1, Brushes.Black, rectsLessonsHeader[i - 3], stringFormat);

                        rectsLessonsHeader[i - 3 - delta + countColumns-3] = new RectangleF(x + (22 * (i - 2 - delta)), y + 55, 22, 20);
                        //e.Graphics.DrawString("", font1, Brushes.Black, rectsLessonsHeader[i - 3 + countColumns-3], stringFormat);
                    }
                }

                // Нумерация занятий
                //delta = 0;
                //for (int i = 4; i < countColumns + delta; i++)
                //{
                //    if (i < countColumns)
                //        if (dgvJournal.Columns[i].Visible != false)
                //        {
                //            rectsLessonsHeader[i - 3 + countColumns] = new RectangleF(x + (22 * (i - 2 - delta)), y + 55, 22, 20);
                //            if (dgvJournal.Columns[i].HeaderText.Contains("\r\n"))
                //            {
                //                var l = dgvJournal.Columns[i].Name;
                //                e.Graphics.DrawString(l, font1, Brushes.Black, rectsLessonsHeader[i - 3 + countColumns], stringFormat);
                //            }
                //        }
                //        else delta++;
                //    else
                //    {
                //        rectsLessonsHeader[i - 3 + countColumns] = new RectangleF(x + (22 * (i - 2 - delta)), y + 55, 22, 20);
                //        e.Graphics.DrawString("", font1, Brushes.Black, rectsLessonsHeader[i - 3 + countColumns], stringFormat);
                //    }
                //}

                // Список слушателей
                int countRows = dgvJournal.Rows.Count;
                RectangleF[] rectsStudents = new RectangleF[countRows * 2];
                x = 20; y = 145;
                for (int i = 0; i < countRows; i++)
                {
                    rectsStudents[i] = new RectangleF(x, y + (20 * (i + 1)), 25, 20);
                    e.Graphics.DrawString((i + 1).ToString(), font1, Brushes.Black, rectsStudents[i], stringFormat);

                    rectsStudents[countRows * 2 - (i + 1)] = new RectangleF(x + 25, y + (20 * (i + 1)), 270, 20);
                    e.Graphics.DrawString(dgvJournal.Rows[i].Cells["Students"].Value.ToString(), font1, Brushes.Black, rectsStudents[countRows * 2 - (i + 1)], stringFormat1);
                }

                // Ячейки для оценок
                RectangleF[] rectsGrades = new RectangleF[countRows * (countColumns - 4)];
                x = 270; y = 165;
                int countRect = 0;
                for (int i = 0; i < countRows; i++)
                {
                    delta = 0;
                    for (int j = 4; j < countColumns + delta; j++)
                    {
                        if (j < countColumns + delta && j < dgvJournal.Columns.Count)
                            if (dgvJournal.Columns[j].Visible != false)
                            {
                                rectsGrades[countRect] = new RectangleF(x + (22 * (j - 2 - delta)), y, 22, 20);
                                e.Graphics.DrawString(dgvJournal.Rows[i].Cells[j].Value.ToString(), font1, Brushes.Black, rectsGrades[countRect], stringFormat);
                                countRect++;
                            }
                            else delta++;
                        else
                        {
                            rectsGrades[countRect] = new RectangleF(x + (22 * (j - 2 - delta)), y, 22, 20);
                            //e.Graphics.DrawString("", font1, Brushes.Black, rectsGrades[countRect], stringFormat);
                            countRect++;
                        }
                    }
                    x = 270; y = y + (20);
                }

                // Вывод на экран
                e.Graphics.DrawString("№", font, Brushes.Black, rectsHeaderTable[0], stringFormat);
                e.Graphics.DrawString("Фамилия, имя и отчество", font, Brushes.Black, rectsHeaderTable[1], stringFormat);
                e.Graphics.DrawRectangles(blackPen, rectsHeaderTable);
                e.Graphics.DrawRectangles(blackPen, rectsLessonsHeader);
                e.Graphics.DrawRectangles(blackPen, rectsStudents);
                e.Graphics.DrawRectangles(blackPen, rectsGrades);
            }
            catch (Exception ex)
            {
                var m = new StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
