using Istra.Documents;
using Istra.Entities;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Istra;

namespace Istra
{
    public partial class ArchiveForm : Form
    {
        IstraContext db = new IstraContext();
        IQueryable<Archive> listStudents;
        public bool dateBegin = false, dateEnd = false;
        string sortColumn;
        SortOrder sortOrder;

        public ArchiveForm()
        {
            InitializeComponent();
            try
            {
                //загрузка фильтра групп
                var actGroups = db.Groups.ToList();
                cbGroups.DataSource = actGroups;
                cbGroups.DisplayMember = "Name";
                cbGroups.ValueMember = "Id";
                cbGroups.SelectedIndex = -1;

                //загрузка фильтра тип обучения                
                var type = db.Years.Where(a => a.IsRemoved == false).OrderBy(a => a.SortIndex).ToList();
                cbTypeGroup.DataSource = type;
                cbTypeGroup.DisplayMember = "Name";
                cbTypeGroup.ValueMember = "Id";
                cbTypeGroup.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                string methodName = ex.TargetSite + ";\r\n" + ex.StackTrace;                
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Filter(string lastname, int? idGroup, DateTime? dateEnrollment, DateTime? dateExclusion, int? typeId, string column, bool transfer, SortOrder? sortOrder)
        {
            listStudents = from enroll in db.Enrollments
                           join groups in db.Groups on enroll.GroupId equals groups.Id
                           join activity in db.ActivityGroups on groups.ActivityId equals activity.Id
                           join students in db.Students on enroll.StudentId equals students.Id
                           join schools in db.Schools on students.SchoolId equals schools.Id into outer2
                           from schools in outer2.DefaultIfEmpty()
                           join causes in db.Causes on enroll.CauseId equals causes.Id into outer1
                           from causes in outer1.DefaultIfEmpty()
                           join months in db.Months on enroll.MonthExclusionId equals months.Id into outer
                           from months in outer.DefaultIfEmpty()
                           join teachers in db.Workers on groups.TeacherId equals teachers.Id into outer3
                           from teachers in outer3.DefaultIfEmpty()
                           join years in db.Years on groups.YearId equals years.Id into outer4
                           from years in outer4.DefaultIfEmpty()
                           where enroll.Group.ActivityId == 1 || enroll.ExclusionId != null
                           select new Archive
                           {
                               StudentId = students.Id,
                               GroupId = groups.Id,
                               SchoolId = students.SchoolId,
                               EnrollId = enroll.Id,
                               YearId = years.Id,
                               ActivityId = activity.Id,
                               Lastname = students.Lastname,
                               Firstname = students.Firstname,
                               Middlename = students.Middlename,
                               NameGroup = groups.Name,
                               Year = years.Name,
                               Status = activity.Name,                               
                               Teacher = teachers.Lastname + " " + teachers.Firstname.Substring(0, 1) + "." + teachers.Middlename.Substring(0, 1) + ".",
                               DateEnrollment = enroll.DateEnrollment,
                               DateExclusion = enroll.DateExclusion,
                               Cause = causes.Name,
                               Transfer = enroll.Transfer,
                               Note = enroll.Note,
                               Phone1 = students.StudentPhone,
                               Phone2 = students.StudentPhone2,
                               ParentPhone = students.ParentsPhone,
                               Sex = students.Sex,
                               BirthDate = students.DateOfBirth,
                               School = schools.Name,
                               Class = students.Class
                           };

            if (typeId != null) listStudents = listStudents.Where(d => d.YearId == typeId);

            if (idGroup != null) listStudents = listStudents.Where(d => d.GroupId == idGroup);
            if (lastname != null && lastname != "") listStudents = listStudents.Where(e => e.Lastname.StartsWith(lastname));

            if (dateBegin) listStudents = listStudents.Where(a => a.DateExclusion.Value >= dtpDateBegin.Value.Date);
            if (dateEnd) listStudents = listStudents.Where(a => a.DateExclusion.Value <= dtpDateEnd.Value.Date);

            if (transfer) listStudents = listStudents.Where(a => a.Transfer == true);
            else listStudents = listStudents.Where(a => a.Transfer == false);

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
                    case "Teacher":
                        {
                            if (sortOrder == SortOrder.Ascending)
                            {
                                listStudents = listStudents.OrderBy(x => x.Teacher);
                            }
                            else
                            {
                                listStudents = listStudents.OrderByDescending(x => x.Teacher);
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
                    case "Transfer":
                        {
                            if (sortOrder == SortOrder.Ascending)
                            {
                                listStudents = listStudents.OrderBy(x => x.Transfer);
                            }
                            else
                            {
                                listStudents = listStudents.OrderByDescending(x => x.Transfer);
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
                    case "DateExclusion":
                        {
                            if (sortOrder == SortOrder.Ascending)
                            {
                                listStudents = listStudents.OrderBy(x => x.DateExclusion);
                            }
                            else
                            {
                                listStudents = listStudents.OrderByDescending(x => x.DateExclusion);
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
                    case "Cause":
                        {
                            if (sortOrder == SortOrder.Ascending)
                            {
                                listStudents = listStudents.OrderBy(x => x.Cause);
                            }
                            else
                            {
                                listStudents = listStudents.OrderByDescending(x => x.Cause);
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
                }
            }
            else
                listStudents = listStudents.OrderBy(f => f.Lastname);

            dgvStudents.DataSource = listStudents.ToList();

            if (sortOrder != SortOrder.None)
                dgvStudents.Columns[sortColumn].HeaderCell.SortGlyphDirection = (SortOrder)sortOrder;
        }

        private void ArchiveForm_Load(object sender, EventArgs e)
        {
            btStudentDelete.Visible = false;

            //права доступа
            if (CurrentSession.CurrentRole.Name == "Управляющий" || CurrentSession.CurrentRole.Name == "Администратор" || CurrentSession.CurrentRole.Name == "Старший секретарь")
            {
                btStudentDelete.Visible = true;
            }

            try
            {
                //загрузка списка слушателей
                Filter(null, null, null, null, null, sortColumn, chbTransfer.Checked, sortOrder);

                //оформление таблицы
                dgvStudents.Columns["StudentId"].Visible = dgvStudents.Columns["GroupId"].Visible =
                    dgvStudents.Columns["SchoolId"].Visible = dgvStudents.Columns["EnrollId"].Visible = 
                   dgvStudents.Columns["ActivityId"].Visible = dgvStudents.Columns["YearId"].Visible = false;

                dgvStudents.Columns["Lastname"].HeaderText = "Фамилия";
                dgvStudents.Columns["Firstname"].HeaderText = "Имя";
                dgvStudents.Columns["Middlename"].HeaderText = "Отчество";
                dgvStudents.Columns["Status"].HeaderText = "Статус";
                dgvStudents.Columns["NameGroup"].HeaderText = "Группа";
                dgvStudents.Columns["Teacher"].HeaderText = "Преподаватель";
                dgvStudents.Columns["Phone1"].HeaderText = "Тел1";
                dgvStudents.Columns["Phone2"].HeaderText = "Тел2";
                dgvStudents.Columns["ParentPhone"].HeaderText = "Тел родителя";
                dgvStudents.Columns["BirthDate"].HeaderText = "Дата рожд.";
                dgvStudents.Columns["School"].HeaderText = "Школа";
                dgvStudents.Columns["Class"].HeaderText = "Класс";
                dgvStudents.Columns["Transfer"].HeaderText = "Переведен";
                dgvStudents.Columns["Note"].HeaderText = "Примечание";
                dgvStudents.Columns["DateEnrollment"].HeaderText = "Дата зачисления";
                dgvStudents.Columns["DateExclusion"].HeaderText = "Дата отчисления";
                dgvStudents.Columns["Cause"].HeaderText = "Причина";
                dgvStudents.Columns["Sex"].HeaderText = "Пол";
                dgvStudents.Columns["Year"].HeaderText = "Форма\\Год";

                dgvStudents.Columns["NameGroup"].AutoSizeMode = dgvStudents.Columns["School"].AutoSizeMode =
                    dgvStudents.Columns["Class"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                dgvStudents.Columns["BirthDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvStudents.Columns["DateEnrollment"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvStudents.Columns["DateExclusion"].DefaultCellStyle.Format = "dd/MM/yyyy";

                dgvStudents.Columns["Phone1"].DefaultCellStyle.Format = "(###) ###-####";
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
            Filter(tbLastname.Text, (int?)cbGroups.SelectedValue, dtpDateBegin.Value, dtpDateEnd.Value, (int?)cbTypeGroup.SelectedValue, sortColumn, chbTransfer.Checked, sortOrder);
        }

        private void dtpDateEnd_ValueChanged(object sender, EventArgs e)
        {
            dateEnd = true;
            Filter(tbLastname.Text, (int?)cbGroups.SelectedValue, dtpDateBegin.Value, dtpDateEnd.Value, (int?)cbTypeGroup.SelectedValue, sortColumn, chbTransfer.Checked, sortOrder);
        }

        private void btCleanFilter_Click(object sender, EventArgs e)
        {
            dtpDateBegin.Value = dtpDateEnd.Value = DateTime.Now;
            dtpDateEnd.Enabled = dateBegin = dateEnd = false;
            cbTypeGroup.SelectedIndex = -1;
            chbTransfer.Checked = false;

            Filter(null, null, null, null, null, sortColumn, chbTransfer.Checked, sortOrder);
        }

        private void cbGroups_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter(tbLastname.Text, (int?)cbGroups.SelectedValue, dtpDateBegin.Value, dtpDateEnd.Value, (int?)cbTypeGroup.SelectedValue, sortColumn, chbTransfer.Checked, sortOrder);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var exportStudent = new Report();
            exportStudent.ExportExcelStudent(dgvStudents, false);
        }

        private void dgvStudents_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int currentRow = e.RowIndex;
            if (currentRow != -1)
            {
                var idStudent = Convert.ToInt32(dgvStudents.Rows[currentRow].Cells["StudentId"].Value);
                var studentForm = new StudentForm(idStudent, false);
                studentForm.MdiParent = this.MdiParent;
                studentForm.Show();
                                
                Filter(tbLastname.Text, (int?)cbGroups.SelectedValue, dtpDateBegin.Value, dtpDateEnd.Value, (int?)cbTypeGroup.SelectedValue, sortColumn, chbTransfer.Checked, sortOrder);

                if (currentRow != dgvStudents.Rows.Count)
                {
                    dgvStudents.Rows[currentRow].Selected = true;
                    dgvStudents.CurrentCell = dgvStudents.Rows[currentRow].Cells["Lastname"];
                }
                else
                {
                    dgvStudents.Rows[dgvStudents.Rows.Count - 1].Selected = true;
                    dgvStudents.CurrentCell = dgvStudents.Rows[currentRow].Cells["Lastname"];
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
            Filter(tbLastname.Text, (int?)cbGroups.SelectedValue, dtpDateBegin.Value, dtpDateEnd.Value, (int?)cbTypeGroup.SelectedValue, sortColumn, chbTransfer.Checked, sortOrder);
            grid.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sortOrder;
        }

        private void chbTransfer_CheckedChanged(object sender, EventArgs e)
        {
            Filter(tbLastname.Text, (int?)cbGroups.SelectedValue, dtpDateBegin.Value, dtpDateEnd.Value, (int?)cbTypeGroup.SelectedValue, sortColumn, chbTransfer.Checked, sortOrder);
        }

        private void bеStudentDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentCell != null)
            {
                var dr = MessageBox.Show("Вы уверены что хотите удалить выбранных учащихся? Удаленные учащиеся останутся в базе, но все данные о записи в группах восстановить будет нельзя!",
                    "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    int currentRow = dgvStudents.CurrentCell.RowIndex;
                    foreach (DataGridViewRow row in dgvStudents.SelectedRows)
                    {
                        int idEnroll = Convert.ToInt32(row.Cells["EnrollId"].Value);
                        //Удаление графика платежей
                        var schds = db.Schedules.Where(a => a.EnrollmentId == idEnroll);
                        db.Schedules.RemoveRange(schds);

                        //удаление платежей
                        var pays = db.Payments.Where(a => a.EnrollmentId == idEnroll);
                        db.Payments.RemoveRange(pays);

                        //удаление самих записей в группы
                        var enroll = db.Enrollments.Find(idEnroll);
                        db.Enrollments.Remove(enroll);
                    }
                    db.SaveChanges();
                    Filter(tbLastname.Text, (int?)cbGroups.SelectedValue, dtpDateBegin.Value, dtpDateEnd.Value, (int?)cbTypeGroup.SelectedValue, sortColumn, chbTransfer.Checked, sortOrder);


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
        }

        private void dtpDateEnrollment_ValueChanged(object sender, EventArgs e)
        {
            dtpDateEnd.Enabled = dateBegin = true;
            Filter(tbLastname.Text, (int?)cbGroups.SelectedValue, dtpDateBegin.Value, dtpDateEnd.Value, (int?)cbTypeGroup.SelectedValue, sortColumn, chbTransfer.Checked, sortOrder);
        }
    }
}
