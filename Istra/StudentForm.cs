using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using Istra.Entities;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using Istra.Documents;

namespace Istra
{
    public partial class StudentForm : Form
    {
        IstraContext db = new IstraContext();
        public Student student;
        public List<ListPayments> listPays;
        public List<School> schools;

        List<Label> labelGroups = new List<Label>();
        List<DataGridView> dgvPays = new List<DataGridView>();
        List<TextBox> tbDiscounts = new List<TextBox>();
        List<Button> btPay = new List<Button>();
        List<CheckBox> chkbDiscount = new List<CheckBox>();
        ComboBox cbGroups;
        Button btEnroll;
        bool activity//true-обычное открытие формы, false-открытие из архива
            , showMessage; //отображение сообщения об удалении скидки


        public StudentForm(int id, bool activity)
        {
            InitializeComponent();
            try
            {
                if (id != 0)
                {
                    student = db.Students.Find(id);
                    this.Text = "Слушатель: " + student.Fullname();
                    this.activity = activity;
                }
                else
                {
                    student = new Student();
                    student.DateOfBirth = DateTime.Now.AddYears(-10);
                    student.PassportDate = DateTime.Now.AddYears(-5);
                    student.EntryDate = DateTime.Now;
                    this.activity = activity;

                    //combobox с группами
                    cbGroups = new ComboBox();
                    cbGroups.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbGroups.Location = new Point(20, 45);
                    panel1.Controls.Add(cbGroups);
                    //получение списка групп, в которые студент уже записан
                    List<int> en = db.Enrollments.Where(a => a.StudentId == student.Id && a.DateExclusion == null).Select(a => a.GroupId).Cast<int>().ToList();
                    cbGroups.DataSource = db.Groups.Include("Activity").Where(a => (a.Activity.Name == "Текущие" || a.Activity.Name == "В наборе") && !en.Contains(a.Id)).OrderBy(a => a.Name).ToList();
                    cbGroups.DisplayMember = "Name";
                    cbGroups.ValueMember = "Id";
                    cbGroups.SelectedIndex = -1;

                    //кнопка для записи в группу
                    btEnroll = new Button();
                    btEnroll.Text = "Записать";
                    btEnroll.Location = new Point(170, 45);
                    btEnroll.Size = new Size(100, 27);
                    btEnroll.Name = "btEnroll";

                    panel1.Controls.Add(btEnroll);
                    btEnroll.Click += EnrollmentStudent;
                }

                //заполнение статусов
                var status = db.Statuses.Where(a => a.IsRemoved == false).ToList();
                cbStatus.DataSource = status;
                cbStatus.DisplayMember = "Name";
                cbStatus.ValueMember = "Id";
                cbStatus.SelectedIndex = 3;
                SelectStatus(4);

                //загрузка городов
                var cities = db.Cities.Where(a => a.IsRemoved == false).OrderBy(a => a.Name).ToList();
                cbCity.DataSource = cities;
                cbCity.DisplayMember = "Name";
                cbCity.ValueMember = "Id";

                //автопоиск города            
                AutoCompleteStringCollection citiesList = new AutoCompleteStringCollection();
                for (int i = 0; i < cities.Count; i++)
                {
                    citiesList.Add(cities[i].Name);
                }
                cbCity.AutoCompleteSource = AutoCompleteSource.CustomSource;
                cbCity.AutoCompleteCustomSource = citiesList;
                cbCity.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                //автопоиск организации выдавшей паспорт
                AutoCompleteStringCollection passportList = new AutoCompleteStringCollection();
                passportList.Add("Отдел УФМС России по РБ в г. Стерлитамак");
                passportList.Add("УВД г. Стерлитамак РБ");
                passportList.Add("МВД по Республике Башкортостан");
                tbPassportIssuedBy.AutoCompleteSource = AutoCompleteSource.CustomSource;
                tbPassportIssuedBy.AutoCompleteCustomSource = passportList;
                tbPassportIssuedBy.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<GroupsEnrollStudent> lstGroupsEnroll(bool activ)
        {
            List<GroupsEnrollStudent> groupsEnroll;

            if (activ)
                groupsEnroll = (from enrolls in db.Enrollments
                                join groups in db.Groups on enrolls.GroupId equals groups.Id
                                join activity in db.ActivityGroups on groups.ActivityId equals activity.Id
                                join students in db.Students on enrolls.StudentId equals students.Id
                                join years in db.Years on groups.YearId equals years.Id
                                orderby activity.Name, groups.Name
                                where (activity.Name == "Текущие" || activity.Name == "В наборе") && students.Id == student.Id && enrolls.DateExclusion == null
                                select new GroupsEnrollStudent
                                {
                                    StudentId = students.Id,
                                    GroupId = groups.Id,
                                    EnrollId = enrolls.Id,
                                    NameGroup = groups.Name,
                                    ActivityGroup = groups.Activity.Name,
                                    Year = years.Name
                                }).ToList();
            else
                groupsEnroll = (from enrolls in db.Enrollments
                                join groups in db.Groups on enrolls.GroupId equals groups.Id
                                join activity in db.ActivityGroups on groups.ActivityId equals activity.Id
                                join students in db.Students on enrolls.StudentId equals students.Id
                                join years in db.Years on groups.YearId equals years.Id
                                orderby activity.Name, groups.Name
                                where (activity.Name == "Закрытые" && students.Id == student.Id && enrolls.DateExclusion == null)
                                || (students.Id == student.Id && enrolls.DateExclusion != null)
                                select new GroupsEnrollStudent
                                {
                                    StudentId = students.Id,
                                    GroupId = groups.Id,
                                    EnrollId = enrolls.Id,
                                    NameGroup = groups.Name,
                                    ActivityGroup = groups.Activity.Name,
                                    Year = years.Name
                                }).ToList();
            return groupsEnroll;
        }

        private void StudentForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (student.Id != 0)
                {
                    tbLastname.Text = student.Lastname;
                    tbFirsname.Text = student.Firstname;
                    tbMiddlename.Text = student.Middlename;
                    dtpBirthDate.Value = student.DateOfBirth;
                    mtbStudentPhone.Text = student.StudentPhone.ToString();
                    mtbStudentPhone2.Text = student.StudentPhone2.ToString();
                    cbStatus.SelectedValue = student.StatusId;
                    if (student.Sex == "м") rbMen.Checked = true; else rbWomen.Checked = true;

                    //загрузка и автопоиск уч. заведений
                    SelectStatus(student.StatusId);
                    //проверка не было ли удалено учебное заведение сохраненное за учащимся
                    if (student.SchoolId != null)
                        cbSchools.SelectedValue = student.SchoolId;
                    if (student.Class != null)
                        tbClass.Text = student.Class.ToString();
                    if (student.Shift != "")
                        cbShift.SelectedItem = student.Shift;

                    //выбираем город, если нет, то город по умолчанию
                    if (student.CityId != null)
                    {
                        cbCity.SelectedValue = student.CityId;
                        SelectCity(student.CityId);
                        if (student.StreetId != null)
                            cbStreet.SelectedValue = student.StreetId;
                        if (student.House != "")
                            tbHouse.Text = student.House;
                        if (student.Float != null)
                            tbFloat.Text = student.Float.ToString();
                    }
                    else
                    {
                        var defaultCity = db.Cities.FirstOrDefault(a => a.defaultCity == true);
                        if (defaultCity != null)
                        {
                            cbCity.SelectedValue = defaultCity.Id;
                            SelectCity(defaultCity.Id);
                        }
                    }

                    if (student.LastnameParent != null && student.LastnameParent != String.Empty)
                    {
                        chkbParentPay.Checked = true;
                        tbLastnameParent.Text = student.LastnameParent;
                        tbFirstnameParent.Text = student.FirstnameParent;
                        tbMiddlenameParent.Text = student.MiddlenameParent;
                        mtbPhoneParent.Text = student.ParentsPhone.ToString();
                    }
                    else chkbParentPay.Checked = false;

                    //chkbParentPay.Checked = (student.LastnameParent == null || student.LastnameParent == String.Empty) ? false : true;
                    if (student.PassportNumber != null)
                        mtbPassportNumber.Text = student.PassportNumber;
                    if (student.PassportDate != null)
                        mtbPassportDate.Text = Convert.ToDateTime(student.PassportDate).ToShortDateString();
                    else mtbPassportDate.Text = "";
                    if (student.PassportIssuedBy != null)
                        tbPassportIssuedBy.Text = student.PassportIssuedBy;

                    dtpEntryDate.Value = student.EntryDate;
                    rtbNote.Text = student.Note;

                    //получение списка активных групп, на которые записан слушатель
                    var groupsEnroll = lstGroupsEnroll(activity);

                    GeneratePaymentsTab(groupsEnroll);
                    double balance = Balance(groupsEnroll);
                    label22.Text = balance.ToString("C", CultureInfo.CurrentCulture);
                    if (balance >= 0) label22.ForeColor = Color.Green; else label22.ForeColor = Color.Red;
                    btSave.Enabled = btSaveAndClose.Enabled = false;
                }
                else
                {
                    cbCity.SelectedIndex = -1;
                    btSave.Enabled = btSaveAndClose.Enabled = true;
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

        public double Balance(List<GroupsEnrollStudent> ges)
        {
            try
            {
                //получение суммы платежей по активным группам
                double sumPays = 0;
                var listId = ges.Select(r => r.EnrollId).ToList();
                sumPays = db.Payments.Where(r => listId.Contains(r.EnrollmentId) && r.AdditionalPay == false && r.IsDeleted == false).Select(l => l.ValuePayment).DefaultIfEmpty(0).Sum();

                //получение суммы платежей по графику на текущую дату
                double sumSchs = 0;
                foreach (var g in ges)
                {
                    double sum = db.Schedules.Where(a => a.EnrollmentId == g.EnrollId && a.Source == 2 && DbFunctions.TruncateTime(a.DateBegin) <= DateTime.Now)//
                               .Sum(o => (double?)(o.Value - o.Discount)) ?? 0;
                    sumSchs += sum;
                }
                return sumPays - sumSchs;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return 0;
            }
        }

        private void GeneratePaymentsTab(List<GroupsEnrollStudent> ges)
        {
            try
            {
                int x = 10, y = 15, top = 0;
                int countEnroll = ges.Count;

                //построение интерфейса
                for (int i = 0; i < countEnroll; i++)
                {
                    //кнопка для отчисления и перевода из группы или даты обучения если отчислен
                    string dates = ""; //даты обучения в группе из которой отчислен
                    if (activity)
                    {
                        btPay.Add(new Button
                        {
                            Text = "Отчислить/Перевести",
                            Location = new Point(x + 230, y - 4 + (340 * i)),
                            Size = new Size(170, 27),
                            Name = "e" + ges[i].EnrollId.ToString()
                        });
                        panel1.Controls.Add(btPay[btPay.Count - 1]);
                        btPay[btPay.Count - 1].Click += DeleteStudentFromGroup;
                    }
                    else
                    {
                        //получение дат обучения в закрытой группе
                        var enroll = db.Enrollments.Find(ges[i].EnrollId);
                        dates = enroll.DateEnrollment.ToShortDateString() + " - ";
                        if (enroll.DateExclusion != null)
                        {
                            DateTime dateEnd = (DateTime)enroll.DateExclusion;
                            dates += dateEnd.ToShortDateString();
                        }
                        else
                        {
                            int id = ges[i].GroupId;
                            var lesson = db.Lessons.Where(a => a.GroupId == id).OrderByDescending(a => a.Date).FirstOrDefault();
                            if (lesson != null)
                                dates += lesson.Date.ToShortDateString();
                            else
                                dates = dates.Substring(0, dates.Length - 3);
                        }

                        if (ges[i].ActivityGroup == "Текущие" || ges[i].ActivityGroup == "В наборе")
                        //кнопка восстановления записи студента в группу
                        {
                            btPay.Add(new Button
                            {
                                Text = "Восстановить",
                                Location = new Point(x + 230, y - 4 + (340 * i)),
                                Size = new Size(170, 27),
                                Name = "r" + ges[i].EnrollId.ToString()
                            });
                            panel1.Controls.Add(btPay[btPay.Count - 1]);
                            btPay[btPay.Count - 1].Click += RecoveryStudentInGroup;
                        }
                    }

                    //label группы
                    labelGroups.Add(new Label
                    {
                        Text = (dates == "") ? ges[i].NameGroup + " (" + ges[i].Year + ")" : ges[i].NameGroup + " (" + ges[i].Year + ")     " + dates,
                        Location = new Point(x, y + (340 * i)),
                        Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold),
                        Width = 300
                    });
                    panel1.Controls.Add(labelGroups[labelGroups.Count - 1]);

                    //label заголовок таблицы платежей
                    labelGroups.Add(new Label
                    {
                        Text = "Оплаты",
                        Location = new Point(x, y + 22 + (340 * i)),
                        Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular)
                    });
                    panel1.Controls.Add(labelGroups[labelGroups.Count - 1]);

                    //label заголовок таблицы графика платежей
                    labelGroups.Add(new Label
                    {
                        Text = "График платежей",
                        Location = new Point(x, y + 22 + (340 * i)),
                        Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                        Size = new Size(126, 17)
                    });
                    panel3.Controls.Add(labelGroups[labelGroups.Count - 1]);

                    //таблица платежей
                    dgvPays.Add(new DataGridView
                    {
                        Enabled = (!activity) ? false : true,
                        Location = new Point(x, y + 45 + (340 * i)),
                        Size = new Size(panel1.Width - x - 2, 238),
                        Name = "p" + ges[i].EnrollId.ToString(),
                        Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top),
                        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                        RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing,
                        AllowUserToResizeRows = false,
                        ReadOnly = true,
                        MultiSelect = true,
                        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                        RowHeadersVisible = false
                    });
                    panel1.Controls.Add(dgvPays[dgvPays.Count - 1]);
                    dgvPays[dgvPays.Count - 1].MouseDoubleClick += EditPay;

                    //итоги
                    labelGroups.Add(new Label
                    {
                        Text = "Итого оплачено",
                        Location = new Point(x, y + 290 + (340 * i)),
                        Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
                    });
                    panel1.Controls.Add(labelGroups[labelGroups.Count - 1]);
                    labelGroups.Add(new Label
                    {
                        Location = new Point(x + 220, y + 290 + (340 * i)),
                        Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                        Name = "l" + ges[i].EnrollId.ToString()
                    });
                    panel1.Controls.Add(labelGroups[labelGroups.Count - 1]);

                    //заполнение платежей и их суммы
                    LoadPays(dgvPays[dgvPays.Count - 1], labelGroups[labelGroups.Count - 1], ges[i].EnrollId);

                    //кнопки добавления и удаления платежей
                    btPay.Add(new Button
                    {
                        Enabled = (!activity) ? false : true,
                        Image = Properties.Resources.plus,
                        Location = new Point(2, y + 45 + (340 * i)),
                        Size = new Size(43, 42),
                        Name = "a" + ges[i].EnrollId.ToString()
                    });
                    panel2.Controls.Add(btPay[btPay.Count - 1]);
                    btPay[btPay.Count - 1].Click += AddPay;

                    btPay.Add(new Button
                    {
                        Enabled = (!activity) ? false : true,
                        Image = Properties.Resources.trashcan_delete2,
                        Location = new Point(2, y + 90 + (340 * i)),
                        Size = new Size(43, 42),
                        Name = "d" + ges[i].EnrollId.ToString()
                    });
                    panel2.Controls.Add(btPay[btPay.Count - 1]);
                    btPay[btPay.Count - 1].Click += DeletePay;
                    btPay.Add(new Button
                    {
                        Enabled = (!activity) ? false : true,
                        BackgroundImage = Properties.Resources.printer,
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Location = new Point(2, y + 135 + (340 * i)),
                        Size = new Size(43, 42),
                        Name = "p" + ges[i].EnrollId.ToString()
                    });
                    panel2.Controls.Add(btPay[btPay.Count - 1]);
                    btPay[btPay.Count - 1].Click += PrintPay;

                    //таблица график платежей
                    dgvPays.Add(new DataGridView
                    {
                        Enabled = (!activity) ? false : true,
                        Location = new Point(x, y + 45 + (340 * i)),
                        Size = new Size(panel3.Width - x - 2, 238),
                        Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top),
                        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                        AllowUserToResizeRows = false,
                        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                        ReadOnly = true,
                        MultiSelect = false,
                        Name = "s" + ges[i].EnrollId.ToString(),
                        //RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing,
                        RowHeadersVisible = false
                    });
                    panel3.Controls.Add(dgvPays[dgvPays.Count - 1]);

                    //итоги                
                    labelGroups.Add(new Label
                    {
                        Text = "Итого начислено",
                        Location = new Point(x, y + 290 + (340 * i)),
                        Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
                    });
                    panel3.Controls.Add(labelGroups[labelGroups.Count - 1]);
                    labelGroups.Add(new Label
                    {
                        Location = new Point(x + 220, y + 290 + (340 * i)),
                        Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                        Name = "v" + ges[i].EnrollId.ToString()
                    });
                    panel3.Controls.Add(labelGroups[labelGroups.Count - 1]);

                    top = y + 290 + (340 * i);

                    //скидка                
                    chkbDiscount.Add(new CheckBox
                    {
                        Enabled = (!activity) ? false : true,
                        Text = "Скидка",
                        Location = new Point(2, y + 45 + (340 * i)),
                        Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                        Name = "c" + ges[i].EnrollId.ToString()
                    });
                    panel4.Controls.Add(chkbDiscount[chkbDiscount.Count - 1]);
                    chkbDiscount[chkbDiscount.Count - 1].CheckedChanged += checkBox_Checked;
                    chkbDiscount[chkbDiscount.Count - 1].MouseClick += checkBox_MouseClick;

                    btPay.Add(new Button
                    {
                        Text = "Рассчитать",
                        Location = new Point(2, y + 75 + (340 * i)),
                        Size = new Size(100, 36),
                        Enabled = activity,
                        Name = "b" + ges[i].EnrollId.ToString()
                    });
                    panel4.Controls.Add(btPay[btPay.Count - 1]);
                    LoadSchedule(dgvPays[dgvPays.Count - 1], labelGroups[labelGroups.Count - 1], chkbDiscount[chkbDiscount.Count - 1], btPay[btPay.Count - 1], ges[i]);
                    btPay[btPay.Count - 1].Click += button_Click;

                    //кнопки обновления графика платежей
                    btPay.Add(new Button
                    {
                        Enabled = (!activity) ? false : true,
                        //Image = Properties.Resources.icons8_synchronize_32,
                        Location = new Point(2, y + 115 + (340 * i)),
                        Size = new Size(100, 36),
                        Text = "Обновить",
                        Name = "u" + ges[i].EnrollId.ToString()
                    });
                    panel4.Controls.Add(btPay[btPay.Count - 1]);
                    btPay[btPay.Count - 1].Click += UpdateSchedule;

                    //кнопка печати договора и графика платежа
                    btPay.Add(new Button
                    {
                        Enabled = (!activity) ? false : true,
                        //BackgroundImage = Properties.Resources.printer,
                        //BackgroundImageLayout = ImageLayout.Stretch,
                        //Location = new Point(x + 785, y + (340 * i)),
                        Location = new Point(2, y + 155 + (340 * i)),
                        Text = "Договор",
                        Size = new Size(100, 36),
                        Name = "pd" + ges[i].EnrollId.ToString()
                    });
                    panel4.Controls.Add(btPay[btPay.Count - 1]);
                    btPay[btPay.Count - 1].Click += PrintContract;
                }

                //отображаем элементы для записи в группы, если форма открывается не из архива
                if (this.activity)
                {
                    //combobox с группами
                    cbGroups = new ComboBox();
                    cbGroups.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbGroups.Location = new Point(x, top + 45);
                    cbGroups.Size = new Size(170, 25);
                    panel1.Controls.Add(cbGroups);
                    //получение списка групп, в которые студент уже записан
                    List<int> en = db.Enrollments.Where(a => a.StudentId == student.Id && a.DateExclusion == null).Select(a => a.GroupId).Cast<int>().ToList();
                    cbGroups.DataSource = db.Groups.Include("Activity").OrderBy(a => a.Name).Where(a => (a.Activity.Name == "Текущие" || a.Activity.Name == "В наборе") && a.Course.Name != "Индивид."
                        && !en.Contains(a.Id)).Select(a => new { Id = a.Id, Name = a.Name + " | " + a.Year.Name }).ToList();
                    cbGroups.DisplayMember = "Name";
                    cbGroups.ValueMember = "Id";
                    cbGroups.SelectedIndex = -1;

                    //кнопка для записи в группу
                    btEnroll = new Button();
                    btEnroll.Text = "Записать";
                    btEnroll.Location = new Point(x + 180, top + 45);
                    btEnroll.Size = new Size(100, 27);
                    btEnroll.Name = "btEnroll";

                    panel1.Controls.Add(btEnroll);
                    btEnroll.Click += EnrollmentStudent;
                }

                //регулировка высоты вкладки "Платежи"
                tableLayoutPanel2.Height = 350 * countEnroll + 45;

            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //запись в группу
        private void EnrollmentStudent(object sender, EventArgs e)
        {
            try
            {
                if (cbGroups.SelectedIndex != -1)
                {
                    if (SaveStudentData())
                    {
                        var dr = MessageBox.Show("Вы уверены, что хотите записать учащегося в группу " + cbGroups.Text + "?", "Запись учащегося", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            //запись слушателя в группу
                            var en = new Enrollment();
                            en.DateEnrollment = DateTime.Now;
                            en.EnrollId = CurrentSession.CurrentUser.Id;
                            en.StudentId = student.Id;
                            en.GroupId = (int)cbGroups.SelectedValue;
                            db.Enrollments.Add(en);
                            db.SaveChanges();

                            //обновление вкладки с группами
                            panel1.Controls.Clear();
                            panel2.Controls.Clear();
                            panel3.Controls.Clear();
                            panel4.Controls.Clear();

                            //получение списка активных групп, на которые записан слушатель
                            var groupsEnroll = lstGroupsEnroll(true);

                            GeneratePaymentsTab(groupsEnroll);
                        }
                    }
                }
                else
                    MessageBox.Show("Не выбрана группа для записи", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //отчисление из группы
        public void DeleteStudentFromGroup(object sender, EventArgs e)
        {
            try
            {
                var dr = MessageBox.Show("Вы уверены что хотите отчислить или перевести учащегося?", "Отчисление", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    string nameTable = (sender as Button).Name.Substring(1);
                    int idEnroll = Convert.ToInt32(nameTable);

                    var fRemoveStudent = new RemoveStudentForm(idEnroll);
                    fRemoveStudent.ShowDialog();

                    //обновление вкладки с группами                    
                    panel1.Controls.Clear();
                    panel2.Controls.Clear();
                    panel3.Controls.Clear();
                    panel4.Controls.Clear();
                    //очистка списков с элементами
                    labelGroups.Clear();
                    dgvPays.Clear();
                    tbDiscounts.Clear();
                    btPay.Clear();
                    chkbDiscount.Clear();

                    //получение списка активных групп, на которые записан слушатель
                    var groupsEnroll = lstGroupsEnroll(true);

                    GeneratePaymentsTab(groupsEnroll);

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

        public void RecoveryStudentInGroup(object sender, EventArgs e)
        {
            try
            {
                var dr = MessageBox.Show("Вы уверены что хотите восстановить учащегося в группе?", "Отчисление", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    activity = true;//переключаем форму для отображения активных групп
                    string nameTable = (sender as Button).Name.Substring(1);
                    int idEnroll = Convert.ToInt32(nameTable);

                    //восстановление записи в группу
                    using (var context = new IstraContext())
                    {
                        var enroll = context.Enrollments.Find(idEnroll);
                        if (enroll != null)
                        {
                            enroll.DateExclusion = null;
                            enroll.ExclusionId = null;
                            enroll.MonthExclusionId = null;
                            context.Entry(enroll).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }

                    //обновление вкладки с группами
                    panel1.Controls.Clear();
                    panel2.Controls.Clear();
                    panel3.Controls.Clear();
                    panel4.Controls.Clear();

                    //очистка списков с элементами
                    labelGroups.Clear();
                    dgvPays.Clear();
                    tbDiscounts.Clear();
                    btPay.Clear();
                    chkbDiscount.Clear();

                    //получение списка активных групп, на которые записан слушатель
                    var groupsEnroll = lstGroupsEnroll(true);

                    GeneratePaymentsTab(groupsEnroll);

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

        //заполнение списка платежей
        public void LoadPays(DataGridView dgv, Label sum, int idEnroll)
        {
            double sumPay = 0;
            try
            {
                listPays = (from pays in db.Payments
                            where pays.EnrollmentId == idEnroll && pays.IsDeleted == false
                            join workers in db.Workers on pays.WorkerId equals workers.Id
                            join months in db.Months on pays.MonthId equals months.Id into g
                            from months in g.DefaultIfEmpty()
                            join types in db.TypePayments on pays.TypePaymentId equals types.Id
                            select new ListPayments
                            {
                                Id = pays.Id,
                                DatePayment = pays.DatePayment,
                                ValuePayment = pays.ValuePayment,
                                Month = months.ShortName,
                                Type = types.Shortname,
                                Worker = workers.Firstname.Substring(0, 1) + "." + workers.Middlename.Substring(0, 1) + ". "
                                 + workers.Lastname,
                                AdditionalPay = pays.AdditionalPay,
                                Note = pays.Note
                            }).ToList();

                dgv.DataSource = listPays;
                dgv.Columns["Id"].Visible = false;
                dgv.Columns["DatePayment"].HeaderText = "Дата";
                dgv.Columns["DatePayment"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgv.Columns["DatePayment"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv.Columns["Month"].HeaderText = "Мес.";
                dgv.Columns["Month"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv.Columns["Type"].HeaderText = "Вид";
                dgv.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv.Columns["ValuePayment"].HeaderText = "Сумма";
                dgv.Columns["ValuePayment"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv.Columns["Worker"].HeaderText = "Сотрудник";
                dgv.Columns["Worker"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv.Columns["AdditionalPay"].Visible = false;
                dgv.Columns["Note"].HeaderText = "Примечание";

                sumPay = (listPays.Count != 0) ? listPays.Where(a => a.AdditionalPay == false).Sum(a => a.ValuePayment) : 0;
                sum.Text = sumPay.ToString("C", CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //заполнение графика платежей
        public void LoadSchedule(DataGridView dgv, Label sum, CheckBox ch, Button bt, GroupsEnrollStudent groupEnroll)
        {
            double sumPay = 0;
            var listSchedules = new List<ListSchedules>();

            try
            {
                //var en = db.Enrollments.Find(groupEnroll);
                //if (en != null)
                //{
                //получение индивидуального графика платежей
                listSchedules = (from sched in db.Schedules
                                 where sched.EnrollmentId == groupEnroll.EnrollId && sched.Source == 2
                                 select new ListSchedules
                                 {
                                     Id = sched.Id,
                                     Discount = sched.Discount,
                                     Date = sched.DateBegin,
                                     Val = sched.Value - sched.Discount,
                                     Note = sched.Note
                                 }).ToList();
                //получение графика платежей для группы, если индивидуальный график не создан
                if (listSchedules.Count == 0)
                {
                    listSchedules = (from sched in db.Schedules
                                     where sched.GroupId == groupEnroll.GroupId && sched.Source == 2 && sched.EnrollmentId == null
                                     select new ListSchedules
                                     {
                                         Id = sched.Id,
                                         Discount = sched.Discount,
                                         Date = sched.DateBegin,
                                         Val = sched.Value - sched.Discount,
                                         Note = sched.Note
                                     }).ToList();

                    //создание индивидуального графика платежей (без скидки) на основе графика группы
                    if (listSchedules.Count > 0)
                    {
                        foreach (var a in listSchedules)
                        {
                            db.Schedules.Add(new Schedule
                            {
                                GroupId = groupEnroll.GroupId,
                                DateBegin = a.Date,
                                EnrollmentId = groupEnroll.EnrollId,
                                Source = 2,
                                Value = a.Val,
                                Discount = 0,
                                WorkerId = CurrentSession.CurrentUser.Id
                            });
                        }
                        db.SaveChanges();

                        //получение индивидуального графика платежей
                        listSchedules = (from sched in db.Schedules
                                         where sched.EnrollmentId == groupEnroll.EnrollId && sched.Source == 2
                                         select new ListSchedules
                                         {
                                             Id = sched.Id,
                                             Discount = sched.Discount,
                                             Date = sched.DateBegin,
                                             Val = sched.Value - sched.Discount,
                                             Note = sched.Note
                                         }).ToList();
                    }
                    //else MessageBox.Show("Не сформирован график платежей для группы. Без графика платежей не возможно рассчитать сальдо для обучающихся!", 
                    //    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //}

                if (listSchedules.Count > 0)
                {
                    if (activity) ch.Enabled = true; else ch.Enabled = false;
                    dgv.DataSource = listSchedules;
                    dgv.Columns["Id"].Visible = dgv.Columns["Discount"].Visible = false;
                    dgv.Columns["Date"].HeaderText = "Дата";
                    dgv.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgv.Columns["Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgv.Columns["Val"].HeaderText = "Сумма";
                    dgv.Columns["Val"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgv.Columns["Note"].HeaderText = "Основание";
                    dgv.Columns["Note"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    var en = db.Enrollments.Find(groupEnroll.EnrollId);
                    //активность скидки при открытии формы
                    if (activity)
                    {
                        if (en.PrivilegeId != null)
                        {
                            showMessage = false;
                            ch.Checked = bt.Enabled = true;
                        }
                        else { ch.Checked = false; }
                    }
                    else ch.Checked = bt.Enabled = false;

                    for (int i = 0; i < dgv.RowCount; i++)
                    {
                        sumPay += Convert.ToDouble(dgv.Rows[i].Cells["Val"].Value);
                    }
                    sum.Text = sumPay.ToString("C", CultureInfo.CurrentCulture);
                }
                else ch.Enabled = false;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadSchedule(DataGridView dgv, Label sum, CheckBox ch, Button bt, int enrollId, bool update)
        {
            double sumPay = 0;
            var listSchedules = new List<ListSchedules>();
            try
            {
                //обновление графика платежей
                if (update)
                {
                    //получение индивидуального графика платежей
                    List<int> listScheds = db.Schedules.Where(a => a.EnrollmentId == enrollId && a.Source == 2).Select(a => a.Id).Cast<int>().ToList();
                    var listRemove = db.Schedules.Where(a => listScheds.Contains(a.Id));
                    //удаление индивидуального графика платежей
                    db.Schedules.RemoveRange(listRemove);
                    db.SaveChanges();
                }

                var en = db.Enrollments.AsNoTracking().FirstOrDefault(a => a.Id == enrollId);
                if (en != null)
                {
                    //получение индивидуального графика платежей
                    listSchedules = (from sched in db.Schedules
                                     where sched.EnrollmentId == en.Id && sched.Source == 2
                                     select new ListSchedules
                                     {
                                         Id = sched.Id,
                                         Discount = sched.Discount,
                                         Date = sched.DateBegin,
                                         Val = sched.Value - sched.Discount,
                                         Note = sched.Note
                                     }).ToList();
                    //получение графика платежей для группы, если индивидуальный график не создан
                    if (listSchedules.Count == 0)
                    {
                        listSchedules = (from sched in db.Schedules
                                         where sched.GroupId == en.GroupId && sched.Source == 2 && sched.EnrollmentId == null
                                         select new ListSchedules
                                         {
                                             Id = sched.Id,
                                             Discount = sched.Discount,
                                             Date = sched.DateBegin,
                                             Val = sched.Value - sched.Discount,
                                             Note = sched.Note
                                         }).ToList();

                        //создание индивидуального графика платежей (без скидки) на основе графика группы
                        if (listSchedules.Count > 0)
                        {
                            foreach (var a in listSchedules)
                            {
                                db.Schedules.Add(new Schedule
                                {
                                    GroupId = en.GroupId,
                                    DateBegin = a.Date,
                                    EnrollmentId = en.Id,
                                    Source = 2,
                                    Value = a.Val,
                                    Discount = 0,
                                    WorkerId = CurrentSession.CurrentUser.Id
                                });
                            }
                            db.SaveChanges();

                            //получение индивидуального графика платежей
                            listSchedules = (from sched in db.Schedules
                                             where sched.EnrollmentId == en.Id && sched.Source == 2
                                             select new ListSchedules
                                             {
                                                 Id = sched.Id,
                                                 Discount = sched.Discount,
                                                 Date = sched.DateBegin,
                                                 Val = sched.Value - sched.Discount,
                                                 Note = sched.Note
                                             }).ToList();
                        }
                        //else MessageBox.Show("Не сформирован график платежей для группы. Без графика платежей не возможно рассчитать сальдо для обучающихся!", 
                        //    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                if (listSchedules.Count > 0)
                {
                    if (activity) ch.Enabled = true; else ch.Enabled = false;
                    dgv.DataSource = listSchedules;
                    dgv.Columns["Id"].Visible = dgv.Columns["Discount"].Visible = false;
                    dgv.Columns["Date"].HeaderText = "Дата";
                    dgv.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dgv.Columns["Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgv.Columns["Val"].HeaderText = "Сумма";
                    dgv.Columns["Val"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgv.Columns["Note"].HeaderText = "Основание";

                    if (en.PrivilegeId != null)
                    {
                        ch.Checked = bt.Enabled = true;
                    }
                    else { ch.Checked = false; }

                    for (int i = 0; i < dgv.RowCount; i++)
                    {
                        sumPay += Convert.ToDouble(dgv.Rows[i].Cells["Val"].Value);
                    }
                    sum.Text = sumPay.ToString("C", CultureInfo.CurrentCulture);
                }
                //else ch.Enabled = false;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //обновление графика платежей
        private void UpdateSchedule(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("Вы уверены, что хотите обновить график платежей учащегося? \r\n В этом случае все данные о скидках учащегося будут утрачены!",
                "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string nameTable = (sender as Button).Name.Substring(1);
                    int idEnroll = Convert.ToInt32(nameTable);

                    var dgv = dgvPays.Find(a => a.Name == "s" + idEnroll);
                    var summ = labelGroups.Find(a => a.Name == "v" + idEnroll);
                    var checkBox = chkbDiscount.Find(a => a.Name == "c" + idEnroll.ToString());
                    var bt = btPay.Find(a => a.Name == "b" + idEnroll);
                    showMessage = false;
                    checkBox.Checked = false;
                    LoadSchedule(dgv, summ, checkBox, bt, idEnroll, true);
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

        //печать договора и графика платежей
        private void PrintContract(object sender, EventArgs e)
        {
            try
            {
                string nameTable = (sender as Button).Name.Substring(2);
                int idEnroll = Convert.ToInt32(nameTable);
                var printContract = new PrintContract();
                var enroll = db.Enrollments.Find(idEnroll);
                if (enroll != null)
                {
                    printContract.Enroll = enroll;
                    printContract.Student = db.Students.Find(enroll.StudentId);
                    printContract.Group = db.Groups.Find(enroll.GroupId);
                    printContract.Year = db.Years.Find(printContract.Group.YearId);
                    printContract.Course = db.Courses.Find(printContract.Group.CourseId);
                    printContract.Direction = db.Directions.Find(printContract.Course.DirectionId);
                    printContract.Schedules = (from sched in db.Schedules
                                               where sched.EnrollmentId == enroll.Id && sched.Source == 2
                                               select new ListSchedules
                                               {
                                                   Id = sched.Id,
                                                   Discount = sched.Discount,
                                                   Date = sched.DateBegin,
                                                   Val = sched.Value - sched.Discount,
                                                   Note = sched.Note
                                               }).ToList();
                    printContract.Document = db.Documents.Find(printContract.Course.DocumentId);
                    printContract.City = db.Cities.Find(printContract.Student.CityId);
                }

                var fContract = new PrintContractForm(printContract);
                fContract.ShowDialog();

            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //добавление платежа в таблицу платежей
        private void AddPay(object sender, EventArgs e)
        {
            try
            {
                string nameTable = (sender as Button).Name.Substring(1);
                int idEnroll = Convert.ToInt32(nameTable);

                var fAddPay = new PayForm(idEnroll, CurrentSession.CurrentUser.Id, null);
                fAddPay.ShowDialog();
                var dgv = dgvPays.Find(a => a.Name == "p" + nameTable);
                var label = labelGroups.Find(a => a.Name == "l" + nameTable);
                LoadPays(dgv, label, idEnroll);

                //обновление сальдо
                var groupsEnroll = (from enrolls in db.Enrollments
                                    join groups in db.Groups on enrolls.GroupId equals groups.Id
                                    join activity in db.ActivityGroups on groups.ActivityId equals activity.Id
                                    join students in db.Students on enrolls.StudentId equals students.Id
                                    where activity.Name == "Текущие" && students.Id == student.Id
                                    select new GroupsEnrollStudent
                                    {
                                        StudentId = students.Id,
                                        GroupId = groups.Id,
                                        EnrollId = enrolls.Id,
                                        NameGroup = groups.Name
                                    }).ToList();

                double balance = Balance(groupsEnroll);
                label22.Text = balance.ToString("C", CultureInfo.CurrentCulture);
                if (balance >= 0) label22.ForeColor = Color.Green; else label22.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //редактирование платежа в таблице платежей
        private void EditPay(object sender, EventArgs e)
        {
            string nameTable = (sender as DataGridView).Name;
            int idEnroll = Convert.ToInt32(nameTable.Substring(1));
            var dgv = dgvPays.Find(a => a.Name == nameTable);
            if (dgv.Rows.Count > 0)
            {
                int idPay = Convert.ToInt32(dgv.Rows[dgv.CurrentCell.RowIndex].Cells["Id"].Value);
                var fAddPay = new PayForm(idEnroll, CurrentSession.CurrentUser.Id, idPay);
                fAddPay.ShowDialog();
                var label = labelGroups.Find(a => a.Name == "l" + nameTable.Substring(1));
                LoadPays(dgv, label, idEnroll);

                //обновление сальдо
                try
                {
                    var groupsEnroll = (from enrolls in db.Enrollments
                                        join groups in db.Groups on enrolls.GroupId equals groups.Id
                                        join activity in db.ActivityGroups on groups.ActivityId equals activity.Id
                                        join students in db.Students on enrolls.StudentId equals students.Id
                                        where activity.Name == "Текущие" && students.Id == student.Id
                                        select new GroupsEnrollStudent
                                        {
                                            StudentId = students.Id,
                                            GroupId = groups.Id,
                                            EnrollId = enrolls.Id,
                                            NameGroup = groups.Name
                                        }).ToList();

                    double balance = Balance(groupsEnroll);
                    label22.Text = balance.ToString("C", CultureInfo.CurrentCulture);
                    if (balance >= 0) label22.ForeColor = Color.Green; else label22.ForeColor = Color.Red;
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

        //удаление платежа из таблицы платежей
        private void DeletePay(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("Вы действительно хотите удалить платеж?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.Yes == dr)
            {
                string nameTable = (sender as Button).Name.Substring(1);
                int idEnroll = Convert.ToInt32(nameTable);
                var dgv = dgvPays.Find(a => a.Name == "p" + nameTable);
                if (dgv.Rows.Count > 0)
                {
                    int idPay = Convert.ToInt32(dgv.Rows[dgv.CurrentCell.RowIndex].Cells["Id"].Value);
                    try
                    {
                        var currentPay = db.Payments.Find(idPay);
                        if (currentPay != null)
                        {
                            currentPay.IsDeleted = true;
                            currentPay.RemovedWorkerId = CurrentSession.CurrentUser.Id;
                            db.Entry(currentPay).State = EntityState.Modified;

                            if (currentPay.AdditionalPay)
                            {
                                var value = currentPay.ValuePayment;
                                var enroll = db.Enrollments.FirstOrDefault(a => a.Id == idEnroll);
                                if (enroll != null)
                                {
                                    var a = enroll.AdditionalPays.LastIndexOf(value.ToString());
                                    enroll.AdditionalPays = enroll.AdditionalPays.Substring(0, a) + enroll.AdditionalPays.Substring(a + value.ToString().Length);
                                    enroll.AdditionalPays = enroll.AdditionalPays.Replace("  ", " ").Trim();
                                    db.Entry(enroll).State = EntityState.Modified;
                                }
                            }

                            db.SaveChanges();
                        }

                        var label = labelGroups.Find(a => a.Name == "l" + nameTable);
                        LoadPays(dgv, label, idEnroll);

                        //обновление сальдо
                        var groupsEnroll = (from enrolls in db.Enrollments
                                            join groups in db.Groups on enrolls.GroupId equals groups.Id
                                            join activity in db.ActivityGroups on groups.ActivityId equals activity.Id
                                            join students in db.Students on enrolls.StudentId equals students.Id
                                            where activity.Name == "Текущие" && students.Id == student.Id
                                            select new GroupsEnrollStudent
                                            {
                                                StudentId = students.Id,
                                                GroupId = groups.Id,
                                                EnrollId = enrolls.Id,
                                                NameGroup = groups.Name
                                            }).ToList();

                        double balance = Balance(groupsEnroll);
                        label22.Text = balance.ToString("C", CultureInfo.CurrentCulture);
                        if (balance >= 0) label22.ForeColor = Color.Green; else label22.ForeColor = Color.Red;
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

        private void PrintPay(object sender, EventArgs e)
        {
            try
            {
                var paymentsPrintForm = new PaymentsPrintForm();
                var dr = paymentsPrintForm.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    List<int> idPays = new List<int>();
                    string nameTable = (sender as Button).Name.Substring(1);
                    int idEnroll = Convert.ToInt32(nameTable);
                    var dgv = dgvPays.Find(a => a.Name == "p" + nameTable);

                    var selectedRows = dgv.SelectedRows.OfType<DataGridViewRow>().ToArray();

                    if (selectedRows.Length != 0)
                    {
                        foreach (var row in selectedRows)
                            idPays.Add(Convert.ToInt32(row.Cells["Id"].Value));

                        var listPays = db.Payments.Where(a => idPays.Contains(a.Id)).ToList();
                        if (listPays.Count != 0)
                        {
                            var receipt = new Report();
                            receipt.Receipts(listPays, CurrentSession.dateOrder, CurrentSession.namePaymaster, CurrentSession.enableDate);
                        }
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

        private void checkBox_Checked(object sender, EventArgs e)
        {
            string name = (sender as CheckBox).Name;
            int idEnroll = Convert.ToInt32(name.Substring(1));
            var checkBox = chkbDiscount.Find(a => a.Name == "c" + idEnroll.ToString());
            var bt = btPay.Find(a => a.Name == "b" + idEnroll);
            if (checkBox.Checked == false)
            {
                using (IstraContext context = new IstraContext())
                {
                    var enroll = context.Enrollments.FirstOrDefault(a => a.Id == idEnroll);
                    enroll.PrivilegeId = null;
                    context.Entry(enroll).State = EntityState.Modified;
                    context.SaveChanges();
                }
                bt.Enabled = false;
            }
            else bt.Enabled = true;
        }

        void CheckedBox_Discount(int id, CheckBox ch, Button bt, bool showMessage)
        {
            if (ch.Checked)
            {
                bt.Enabled = true;
            }
            else
            {
                using (IstraContext context = new IstraContext())
                {
                    if (showMessage)
                    {
                        var dr = MessageBox.Show("Вы действительно хотите удалить скидку?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (DialogResult.Yes == dr)
                        {
                            bt.Enabled = false;
                            //очищаем список скидок
                            var listSched = context.Schedules.Where(a => a.EnrollmentId == id).AsEnumerable().Select(a => { a.Discount = 0; return a; });
                            foreach (var sched in listSched)
                            {
                                context.Entry(sched).State = EntityState.Modified;
                            }

                            //очищаем льготы учащегося
                            var enroll = context.Enrollments.FirstOrDefault(a => a.Id == id);

                            if (enroll != null && enroll.PrivilegeId != null)
                            {
                                enroll.PrivilegeId = null;
                                context.Entry(enroll).State = EntityState.Modified;
                            }
                            context.SaveChanges();
                            var dgv = dgvPays.Find(a => a.Name == "s" + id);
                            var summ = labelGroups.Find(a => a.Name == "v" + id);

                            LoadSchedule(dgv, summ, ch, bt, id, false);
                        }
                        else
                            ch.Checked = true;
                    }
                    else
                    {
                        bt.Enabled = false;
                        //очищаем список скидок
                        var listSched = context.Schedules.Where(a => a.EnrollmentId == id).AsEnumerable().Select(a => { a.Discount = 0; return a; });
                        foreach (var sched in listSched)
                        {
                            context.Entry(sched).State = EntityState.Modified;
                        }

                        //очищаем льготы учащегося
                        var enroll = context.Enrollments.FirstOrDefault(a => a.Id == id);

                        if (enroll != null && enroll.PrivilegeId != null)
                        {
                            enroll.PrivilegeId = null;
                            context.Entry(enroll).State = EntityState.Modified;
                        }
                        context.SaveChanges();
                        var dgv = dgvPays.Find(a => a.Name == "s" + id);
                        var summ = labelGroups.Find(a => a.Name == "v" + id);

                        LoadSchedule(dgv, summ, ch, bt, id, false);
                    }
                }
            }
        }

        private void cbStatus_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int idStatus = Convert.ToInt32(cbStatus.SelectedValue);
            SelectStatus(idStatus);
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void SelectStatus(int idStatus)
        {
            try
            {
                //загрузка учебных заведений            
                if (cbStatus.Text == "Дошкольник" || cbStatus.Text == "Школьник")
                {
                    schools = db.Schools.Where(a => a.StatusId == idStatus && a.IsRemoved == false).OrderBy(a => a.Name).ToList();
                    tbClass.Text = String.Empty;
                    cbShift.SelectedIndex = -1;
                    cbSchools.Enabled = tbClass.Enabled = cbShift.Enabled = true;
                    cbSchools.DataSource = schools;
                    cbSchools.DisplayMember = "Name";
                    cbSchools.ValueMember = "Id";

                    //автопоиск учебных заведений            
                    AutoCompleteStringCollection schoolsList = new AutoCompleteStringCollection();
                    for (int i = 0; i < schools.Count; i++)
                    {
                        schoolsList.Add(schools[i].Name);
                    }
                    cbSchools.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    cbSchools.AutoCompleteCustomSource = schoolsList;
                    cbSchools.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    cbSchools.SelectedIndex = -1;
                }
                else
                {
                    cbSchools.Enabled = tbClass.Enabled = cbShift.Enabled = false;
                    tbClass.Text = "";
                    cbShift.SelectedIndex = -1;
                    cbSchools.DataSource = null;
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

        private void SelectCity(int? idCity)
        {
            try
            {
                //загрузка улиц
                if (idCity != null)
                {
                    var streets = db.Streets.Where(a => a.CityId == idCity && a.IsRemoved == false).OrderBy(a => a.Name).ToList();
                    cbStreet.DataSource = streets;
                    cbStreet.DisplayMember = "Name";
                    cbStreet.ValueMember = "Id";

                    //автопоиск улиц            
                    AutoCompleteStringCollection streetList = new AutoCompleteStringCollection();
                    for (int i = 0; i < streets.Count; i++)
                    {
                        streetList.Add(streets[i].Name);
                    }
                    cbStreet.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    cbStreet.AutoCompleteCustomSource = streetList;
                    cbStreet.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    cbStreet.Text = String.Empty;
                    cbStreet.SelectedIndex = -1;
                    btSave.Enabled = btSaveAndClose.Enabled = true;
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

        private void cbParentPay_CheckedChanged(object sender, EventArgs e)
        {
            tbLastnameParent.Enabled = tbFirstnameParent.Enabled =
                tbMiddlenameParent.Enabled = mtbPhoneParent.Enabled = chkbParentPay.Checked;
            tbLastnameParent.Text = tbFirstnameParent.Text =
                tbMiddlenameParent.Text = mtbPhoneParent.Text = String.Empty;
        }

        private void cbCity_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int idCity = Convert.ToInt32(cbCity.SelectedValue);
            SelectCity(idCity);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (mtbStudentPhone2.TextLength == 14)
            {
                button1.ImageIndex = 0;
                mtbStudentPhone2.Mask = "00-00-00";
            }
            else
            {
                button1.ImageIndex = 1;
                mtbStudentPhone2.Mask = "(999) 000-0000";
            }
        }

        private void StudentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (btSaveAndClose.Enabled)
                {
                    var dr = MessageBox.Show("Сохранить данные учащегося?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            SaveStudentData();
                            break;
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            break;
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

        private bool SaveStudentData()
        {
            try
            {
                //сохранение данных слушателя
                if (tbFirsname.Text == "" || student.StatusId == -1 || (mtbStudentPhone.Text == "(   )    -" && mtbPhoneParent.Text == "(   )    -"))
                {
                    MessageBox.Show("Для сохранения необходимо указать Имя и номер телефона учащегося или родителя", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                else
                {
                    student.Firstname = tbFirsname.Text;
                    mtbStudentPhone.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                    student.StudentPhone = mtbStudentPhone.Text;

                    student.StatusId = Convert.ToInt32(cbStatus.SelectedValue);
                    if (tbLastname.Text != "")
                        student.Lastname = tbLastname.Text;
                    if (tbMiddlename.Text != "")
                        student.Middlename = tbMiddlename.Text;
                    if (dtpBirthDate.Value.Date != DateTime.Now.Date)
                        student.DateOfBirth = dtpBirthDate.Value;

                    if (mtbStudentPhone2.Mask == "(999) 000-0000")
                    {
                        mtbStudentPhone2.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                        student.StudentPhone2 = mtbStudentPhone2.Text;
                    }
                    else
                    {
                        mtbStudentPhone2.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                        student.StudentPhone2 = mtbStudentPhone2.Text;
                    }

                    student.Sex = (rbMen.Checked) ? "м" : "ж";

                    if (cbSchools.Enabled && cbSchools.Text != "")
                    {
                        //добавление учебного заведения если его нет или простое сохранение
                        School currentSchool;
                        if (cbSchools.SelectedIndex == -1 && cbSchools.Text != String.Empty)
                        {
                            cbSchools.Text = cbSchools.Text.Substring(0, 1).ToUpper() + cbSchools.Text.Substring(1);
                            currentSchool = db.Schools.FirstOrDefault(a => a.Name == cbSchools.Text);
                            if (currentSchool == null)
                            {
                                var newSchool = new School() { Name = cbSchools.Text, StatusId = (int)cbStatus.SelectedValue };
                                db.Schools.Add(newSchool);
                                db.SaveChanges();
                            }
                            currentSchool = db.Schools.FirstOrDefault(a => a.Name == cbSchools.Text);
                            student.SchoolId = currentSchool.Id;
                        }
                        else student.SchoolId = Convert.ToInt32(cbSchools.SelectedValue);
                        if (tbClass.Text != "")
                            student.Class = Convert.ToInt32(tbClass.Text);
                        if (cbShift.Text != "")
                            student.Shift = cbShift.Text;
                    }
                    //сохранение адреса
                    //добавление города если его нет и сохранение
                    if (cbCity.Text != String.Empty)
                    {
                        City currentCity;
                        if (cbCity.SelectedIndex == -1 && cbCity.Text != String.Empty)
                        {
                            cbCity.Text = cbCity.Text.Substring(0, 1).ToUpper() + cbCity.Text.Substring(1);
                            currentCity = db.Cities.FirstOrDefault(a => a.Name == cbCity.Text && a.IsRemoved == false);
                            if (currentCity == null)
                            {
                                var newCity = new City() { Name = cbCity.Text, defaultCity = false };
                                db.Cities.Add(newCity);
                                db.SaveChanges();
                            }
                            currentCity = db.Cities.FirstOrDefault(a => a.Name == cbCity.Text && a.IsRemoved == false);
                            student.CityId = currentCity.Id;
                        }
                        else student.CityId = Convert.ToInt32(cbCity.SelectedValue);
                    }

                    //if (cbStreet.Text != String.Empty)
                    //{
                    //добавление улицы если ее нет и сохранение
                    Street currentStreet;
                    if (cbStreet.Text != String.Empty && cbStreet.SelectedIndex == -1)
                    {
                        cbStreet.Text = cbStreet.Text.Substring(0, 1).ToUpper() + cbStreet.Text.Substring(1);
                        currentStreet = db.Streets.FirstOrDefault(a => a.Name == cbStreet.Text && a.CityId == (int)student.CityId);
                        if (currentStreet == null)
                        {
                            var newStreet = new Street() { Name = cbStreet.Text, CityId = (int)student.CityId };
                            db.Streets.Add(newStreet);
                            db.SaveChanges();
                        }
                        currentStreet = db.Streets.FirstOrDefault(a => a.Name == cbStreet.Text && a.CityId == (int)student.CityId);
                        student.StreetId = currentStreet.Id;
                    }
                    else
                    {
                        int id = Convert.ToInt32(cbStreet.SelectedValue);
                        if (id == 0) student.StreetId = null; else student.StreetId = id;
                    }
                    student.House = tbHouse.Text;
                    if (tbFloat.Text != "") student.Float = Convert.ToInt32(tbFloat.Text); else student.Float = null;
                    //}

                    //сохранение данных родителей
                    if (chkbParentPay.Checked)
                    {
                        student.LastnameParent = tbLastnameParent.Text;
                        student.FirstnameParent = tbFirstnameParent.Text;
                        student.MiddlenameParent = tbMiddlenameParent.Text;
                        mtbPhoneParent.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                        student.ParentsPhone = mtbPhoneParent.Text;
                    }

                    //сохранение паспортных данных
                    if (mtbPassportNumber.Text != "")
                    {
                        student.PassportNumber = mtbPassportNumber.Text;
                    }
                    if (mtbPassportDate.Text != "  .  .")
                    {
                        student.PassportDate = Convert.ToDateTime(mtbPassportDate.Text);
                    }
                    else
                    {
                        student.PassportDate = null;
                    }
                    if (tbPassportIssuedBy.Text != "")
                    {
                        student.PassportIssuedBy = tbPassportIssuedBy.Text;
                    }


                    //дата заявления
                    student.EntryDate = dtpEntryDate.Value;

                    //примечание
                    student.Note = rtbNote.Text;

                    if (student.Id == 0)
                    {
                        student.WorkerId = CurrentSession.CurrentUser.Id;
                        student.DateAddBase = DateTime.Now;
                        db.Students.Add(student);
                    }
                    if (student.Id != 0)
                    {
                        db.Entry(student).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    btSave.Enabled = btSaveAndClose.Enabled = false;

                    //создание группы для индивидуальников
                    if (chbIndividualStudies.Checked)
                    {
                        var newGroup = new Group();
                        newGroup.Name = student.LastnameFM;
                        //поиск статусов индивидуального обучения
                        var year = db.Years.FirstOrDefault(a => a.Name == "Индив.");
                        newGroup.YearId = year.Id;
                        var activity = db.ActivityGroups.FirstOrDefault(a => a.Name == "Текущие");
                        var course = db.Courses.FirstOrDefault(a => a.Name == "Индивид.");
                        newGroup.CourseId = course.Id;
                        newGroup.ActivityId = activity.Id;
                        newGroup.Begin = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 18, 30, 0);
                        newGroup.Days = "пн,вт,ср,чт,пт,сб,вс";
                        newGroup.GroupCreatorId = CurrentSession.CurrentUser.Id;
                        newGroup.DurationLesson = 2;
                        newGroup.DurationCourse = 72;
                        newGroup.TeacherId = 40;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Нужно заменить поиском преподавателя!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        newGroup.UnvisibleLessons = 0;
                        newGroup.ClassId = 1;
                        db.Groups.Add(newGroup);
                        db.SaveChanges();

                        //запись на курс студента
                        var enroll = new Enrollment();
                        enroll.DateEnrollment = DateTime.Now;
                        enroll.GroupId = newGroup.Id;
                        enroll.StudentId = student.Id;
                        enroll.EnrollId = CurrentSession.CurrentUser.Id;
                        enroll.PrivilegeId = null;
                        db.Enrollments.Add(enroll);
                        db.SaveChangesAsync();
                    }
                    return true;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Неправильный формат строки. Проверьте введенные даты и остальные данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void tbLastname_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Length == 1)
                ((TextBox)sender).Text = ((TextBox)sender).Text.ToUpper();
            ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);

            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void cbStatus_SelectionChangeCommitted_1(object sender, EventArgs e)
        {
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void dtpBirthDate_ValueChanged(object sender, EventArgs e)
        {
            btSave.Enabled = btSaveAndClose.Enabled = true;
            //if ((DateTime.Now.Year - dtpBirthDate.Value.Year) >= 14 &&
            //    (DateTime.Now.Year - dtpBirthDate.Value.Year) < 20) dtpPassportDate.Value = dtpBirthDate.Value.AddYears(14);
            //else if ((DateTime.Now.Year - dtpBirthDate.Value.Year) >= 20 &&
            //    (DateTime.Now.Year - dtpBirthDate.Value.Year) < 45) dtpPassportDate.Value = dtpBirthDate.Value.AddYears(20);
            //else dtpPassportDate.Value = dtpBirthDate.Value.AddYears(45);
        }

        private void mtbStudentPhone_TextChanged(object sender, EventArgs e)
        {
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private string ValidationStudentData()
        {
            string result = "OK";
            if (tbLastname.Text == "") return tbLastname.Name;
            if (tbFirsname.Text == "") return tbFirsname.Name;
            if (tbMiddlename.Text == "") return tbMiddlename.Name;
            if (mtbStudentPhone.Text == "(   )    -") return mtbStudentPhone.Name;
            if (cbStatus.SelectedIndex == -1) return cbStatus.Name;
            if (cbSchools.Enabled)
            {
                if (cbSchools.SelectedIndex == -1 && cbSchools.Text == String.Empty) return cbSchools.Name;
                if (tbClass.Text == "") return tbClass.Name;
                if (cbShift.SelectedIndex == -1) return cbShift.Name;
            }

            if (cbCity.SelectedIndex == -1 && cbCity.Text == String.Empty) return cbCity.Name;
            if (cbStreet.SelectedIndex == -1 && cbStreet.Text == String.Empty) return cbStreet.Name;
            if (tbHouse.Text == "") return tbHouse.Name;
            if (tbFloat.Text == "") return tbFloat.Name;

            if (chkbParentPay.Checked)
            {
                if (tbLastnameParent.Text == "") return tbLastnameParent.Name;
                if (tbFirstnameParent.Text == "") return tbFirstnameParent.Name;
                if (tbMiddlenameParent.Text == "") return tbMiddlenameParent.Name;
                if (mtbPhoneParent.Text == "(   )    -") return mtbPhoneParent.Name;
            }

            if (mtbPassportNumber.Text == "") return mtbPassportNumber.Name;
            if (tbPassportIssuedBy.Text == "") return tbPassportIssuedBy.Name;

            return result;
        }

        private void btSaveAndClose_Click(object sender, EventArgs e)
        {
            SaveStudentData();
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rtbNote_TextChanged(object sender, EventArgs e)
        {
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }

        private void button_Click(object sender, EventArgs e)
        {
            string nameTable = (sender as Button).Name.Substring(1);
            int idEnroll = Convert.ToInt32(nameTable);

            var fDiscount = new DiscountForm(idEnroll);
            fDiscount.ShowDialog();
            //перегрузка графика платежей после закрытия окна скидки
            var dgv = dgvPays.Find(a => a.Name == "s" + idEnroll);
            var summ = labelGroups.Find(a => a.Name == "v" + idEnroll);
            var checkBox = chkbDiscount.Find(a => a.Name == "c" + idEnroll.ToString());
            var bt = btPay.Find(a => a.Name == "b" + idEnroll);
            showMessage = false;
            LoadSchedule(dgv, summ, checkBox, bt, idEnroll, false);
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            SaveStudentData();
            btSaveAndClose.Enabled = false;
        }

        private void StudentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void checkBox_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                string name = (sender as CheckBox).Name;
                int idEnroll = Convert.ToInt32(name.Substring(1));
                var checkBox = chkbDiscount.Find(a => a.Name == "c" + idEnroll.ToString());
                var bt = btPay.Find(a => a.Name == "b" + idEnroll);
                if (checkBox.Checked == false)
                {
                    showMessage = true;
                    CheckedBox_Discount(idEnroll, checkBox, bt, showMessage);
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

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void mtbPassportDate_TextChanged(object sender, EventArgs e)
        {
            btSave.Enabled = btSaveAndClose.Enabled = true;
        }
    }
}
