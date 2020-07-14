using Istra.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Istra
{
    public partial class SettingForm : Form
    {
        IstraContext db = new IstraContext();
        public List<Direction> directions;
        public int currentSelectedValueRole = -1;

        public SettingForm()
        {
            InitializeComponent();

            //загрузка списка ролей
            lbRoles.DataSource = db.Roles.Where(a => a.IsRemoved == false).OrderBy(a => a.Name).ToList();
            lbRoles.DisplayMember = "Name";
            lbRoles.ValueMember = "Id";

            //загрузка списка пользователей
            LoadUsers();

            //загрузка списка секретарей и кассиров
            LoadSecretaries();


            if (dgvWorkers.Rows.Count > 0)
            {
                //загрузка списка доступных групп для первого работника
                dgvWorkers.Rows[0].Selected = true;
                LoadSecretaryGroups((int)dgvWorkers.Rows[0].Cells["Id"].Value);

                //загрузка списка групп                        
                //dgvWorkers.CurrentCell = dgvWorkers.Rows[0].Cells["Lastname"];
                LoadGroups((int)dgvWorkers.Rows[0].Cells["Id"].Value);
            }

        }

        private void LoadGroups(int workerId)
        {
            var lstGroupsWorker = db.AccessGroups
                .Include(a => a.Group)
                .Where(a => a.WorkerId == workerId)
                .Select(a => a.Group.Id)
                .ToList();

            var lstGroups = db.Groups
                .Include(a => a.Activity)
                .Where(a => a.Activity.Name != "Закрытые" && !lstGroupsWorker.Contains(a.Id))
                .OrderBy(a => a.Name).Select(a => new { a.Id, a.Name })
                .ToList();

            dgvGroups.DataSource = lstGroups;
            dgvGroups.Columns["Id"].Visible = false;
            dgvGroups.Columns["Name"].HeaderText = "Название";
            dgvGroups.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void LoadSecretaryGroups(int secretaryId)
        {
            var lstGroups = db.AccessGroups
                .Include(a => a.Group)
                .Where(a => a.WorkerId == secretaryId)
                .OrderBy(a => a.Group.Name)
                .Select(a => new ListGroupsWorker { Id = a.Id, WorkerId = a.WorkerId, GroupId = a.Group.Id, Name = a.Group.Name })
                .ToList();

            dgvListGroupsWorker.DataSource = lstGroups;
        }

        private void LoadSecretaries()
        {
            var lstWorkers = db.Workers
                .Where(a => (a.Role.Name == "Кассир" || a.Role.Name == "Секретарь") && a.AllAccessGroups == false)
                .OrderBy(a => a.Lastname)
                .ThenBy(a => a.Lastname)
                .ThenBy(a => a.Firstname)
                .ThenBy(a => a.Middlename)
                .Select(a => new { a.Id, a.Lastname, a.Firstname, a.Middlename })
                .ToList();

            dgvWorkers.DataSource = lstWorkers;
            dgvWorkers.Columns["Id"].Visible = false;
            dgvWorkers.Columns["Lastname"].HeaderText = "Фамилия";
            dgvWorkers.Columns["Lastname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvWorkers.Columns["Firstname"].HeaderText = "Имя";
            dgvWorkers.Columns["Firstname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvWorkers.Columns["Middlename"].HeaderText = "Отчество";
            dgvWorkers.Columns["Middlename"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void LoadUsers()
        {
            try
            {
                var users = db.Workers.Where(a => a.IsRemoved != true).Select(a => new
                {
                    a.Id,
                    a.Lastname,
                    a.Firstname,
                    a.Middlename,
                    a.Login,
                    Role = a.Role.Name,
                    Department = a.Department.Name,
                    EnLastname = a.LastnameEn,
                    EnFirstname = a.FirstnameEn,
                    EnMiddlename = a.MiddlenameEn
                }).OrderBy(a => a.Lastname).ToList();

                dgvUsers.DataSource = users;
                dgvUsers.Columns["Id"].Visible = false;
                dgvUsers.Columns["Lastname"].HeaderText = "Фамилия";
                dgvUsers.Columns["Lastname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvUsers.Columns["Firstname"].HeaderText = "Имя";
                dgvUsers.Columns["Middlename"].HeaderText = "Отчество";
                dgvUsers.Columns["Middlename"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvUsers.Columns["Login"].HeaderText = "Логин";
                dgvUsers.Columns["Login"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvUsers.Columns["Role"].HeaderText = "Роль";
                dgvUsers.Columns["Role"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvUsers.Columns["Department"].HeaderText = "Подразделение";
                dgvUsers.Columns["Department"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvUsers.Columns["EnLastname"].HeaderText = "Фамилия (En)";
                dgvUsers.Columns["EnFirstname"].HeaderText = "Имя (En)";
                dgvUsers.Columns["EnMiddlename"].HeaderText = "Отчество (En)";
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            //инициализация параметров доступа, если их нет
            //var lstPermissions = db.Permissions.ToList();
            //if (lstPermissions.Count == 0)
            //{
            //    lstPermissions.Add(new Permission { Name = "Просмотр списка учащихся", SystemName = "ListStudents", });
            //    lstPermissions.Add(new Permission { Name = "Просмотр карточки учащегося", SystemName = "DetailsStudent" });
            //    lstPermissions.Add(new Permission { Name = "Редактирование данных учащегося", SystemName = "EditStudent" });
            //    lstPermissions.Add(new Permission { Name = "Работа с архивом учащихся", SystemName = "ArchiveStudents" });
            //    lstPermissions.Add(new Permission { Name = "Просмотр списка групп", SystemName = "ListGroups" });
            //    lstPermissions.Add(new Permission { Name = "Просмотр данных группы", SystemName = "DetailsGroup" });
            //    lstPermissions.Add(new Permission { Name = "Редактирование данных группы", SystemName = "EditGroup" });
            //    lstPermissions.Add(new Permission { Name = "Зачисление в группу", SystemName = "EnrollInGroup" });
            //    lstPermissions.Add(new Permission { Name = "Отчисление из группы", SystemName = "ExclusionFromGroup" });
            //    lstPermissions.Add(new Permission { Name = "Работа с журналом", SystemName = "Journal" });
            //    lstPermissions.Add(new Permission { Name = "Начисления в группе", SystemName = "SchedulerPayGroup" });
            //    lstPermissions.Add(new Permission { Name = "Печать документов", SystemName = "PrintDoc" });
            //    lstPermissions.Add(new Permission { Name = "Просмотр списка платежей", SystemName = "ListPayments" });
            //    lstPermissions.Add(new Permission { Name = "Редактирование плана набора", SystemName = "PlanEnroll" });
            //    lstPermissions.Add(new Permission { Name = "Финансовые отчеты", SystemName = "FinanceReports" });
            //    lstPermissions.Add(new Permission { Name = "Начисление зар. платы", SystemName = "Wages" });
            //    lstPermissions.Add(new Permission { Name = "Управление пользователями", SystemName = "ManageUsers" });
            //    lstPermissions.Add(new Permission { Name = "Справочники", SystemName = "Directories" });
            //    lstPermissions.Add(new Permission { Name = "Назначение прав", SystemName = "ManageRoles" });
            //    lstPermissions.Add(new Permission { Name = "Просмотр списка занятий", SystemName = "ListLessons" });
            //    lstPermissions.Add(new Permission { Name = "Добавлять занятия", SystemName = "AddLesson" });
            //    lstPermissions.Add(new Permission { Name = "Удалять занятия", SystemName = "DeleteLesson" });
            //    lstPermissions.Add(new Permission { Name = "Ограничение доступа к группам", SystemName = "AccessGroups" });
            //    db.Permissions.AddRange(lstPermissions);
            //    db.SaveChanges();
            //}



            //создание абсолютных прав для администратора
            var lstManageRole = db.ManageRoles.Include(a => a.Permission).ToList();
            if (lstManageRole.Count == 0)
            {
                var adminRole = db.Roles.FirstOrDefault(a => a.Name == "Администратор");
                CreatePermission(adminRole.Id, true);
            }

            //скрытие вкладок запрещеных для роли
            var sdf = !lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "ManageUsers").Value;
            if (!lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "ManageUsers" && a.RoleId == CurrentSession.CurrentRole.Id).Value)
                this.tabPage2.Parent = null;
            if (!lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "ManageRoles" && a.RoleId == CurrentSession.CurrentRole.Id).Value)
                this.tpPermission.Parent = null;
            if (!lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "AccessGroups" && a.RoleId == CurrentSession.CurrentRole.Id).Value)
                this.tpAccessGroups.Parent = null;

            //загрузка разрешений для первой роли в списке
            if (this.tpPermission.Parent != null)
            {
                if (lbRoles.SelectedIndex != -1)
                {
                    currentSelectedValueRole = (int)lbRoles.SelectedValue;
                    lstManageRole = db.ManageRoles.Where(a => a.RoleId == currentSelectedValueRole).ToList();
                    if (lstManageRole.Count == 0)
                        CreatePermission(currentSelectedValueRole, false);
                    else
                    {
                        LoadPermissionsRole(currentSelectedValueRole);
                    }

                    if (Journal.Checked)
                    {
                        AddLesson.Enabled = DeleteLesson.Enabled = true;
                        EnrollInGroup.Enabled = ExclusionFromGroup.Enabled = true;
                    }
                    else
                    {
                        AddLesson.Enabled = DeleteLesson.Enabled = false;
                        EnrollInGroup.Enabled = ExclusionFromGroup.Enabled = false;
                    }
                }
            }
            lbCatigory.SelectedIndex = 0;
        }

        private void CreatePermission(int idRole, bool value)
        {
            var lstPermissions = db.Permissions.ToList();

            var adminRole = db.Roles.FirstOrDefault(a => a.Id == idRole);
            foreach (var permis in lstPermissions)
                db.ManageRoles.Add(new ManageRole { PermissionId = permis.Id, RoleId = adminRole.Id, Value = value });
            db.SaveChanges();
        }

        private void LoadPermissionsRole(int idRole)
        {
            var lstManageRole = db.ManageRoles.Include(a => a.Permission).Where(a => a.RoleId == idRole).ToList();
            var checks = (from controls in tpPermission.Controls.OfType<CheckBox>()
                          select controls).ToList();

            foreach (var chk in checks)
            {
                var v = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == chk.Name);
                if (v == null)
                {
                    CreatePermission((int)lbRoles.SelectedValue, false);
                    db.SaveChanges();
                    chk.Checked = false;
                }
                else
                {
                    chk.Checked = v.Value;
                }
            }

            btSave.Enabled = false;
        }

        private void SavePermissions(int idRole)
        {
            try
            {
                var lstManageRole = db.ManageRoles.Include(a => a.Permission).Where(a => a.RoleId == idRole).ToList();
                var checks = (from controls in tpPermission.Controls.OfType<CheckBox>()
                              select controls).ToList();

                foreach (var chk in checks)
                {
                    var v = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == chk.Name);
                    v.Value = chk.Checked;
                    db.Entry(v).State = EntityState.Modified;
                }

                db.SaveChanges();
                btSave.Enabled = false;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btAddUser_Click(object sender, EventArgs e)
        {
            var fWorker = new WorkerForm(null);
            fWorker.ShowDialog();
            LoadUsers();
        }

        private void btEditUser_Click(object sender, EventArgs e)
        {
            int idWorker = Convert.ToInt32(dgvUsers.Rows[dgvUsers.CurrentCell.RowIndex].Cells["Id"].Value);
            var user = db.Workers.Include(a => a.Role).FirstOrDefault(a => a.Id == idWorker);
            if (user != null)
            {
                if (user.Role.Priority > CurrentSession.CurrentRole.Priority)
                {
                    var fWorker = new WorkerForm(idWorker);
                    fWorker.ShowDialog();
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Вы не можете редактировать данного пользователя", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btRemoveUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentCell.RowIndex != -1)
            {
                var dr = MessageBox.Show("Вы действительно хотите удалить сотрудника: \r\n"
                    + dgvUsers.Rows[dgvUsers.CurrentCell.RowIndex].Cells["Lastname"].Value + " "
                    + dgvUsers.Rows[dgvUsers.CurrentCell.RowIndex].Cells["Firstname"].Value + " "
                    + dgvUsers.Rows[dgvUsers.CurrentCell.RowIndex].Cells["Middlename"].Value + " ",
                    "Внимание", MessageBoxButtons.YesNo);

                int idWorker = Convert.ToInt32(dgvUsers.Rows[dgvUsers.CurrentCell.RowIndex].Cells["Id"].Value);
                var worker = db.Workers.Find(idWorker);
                if (worker != null)
                {
                    if (worker.Role.Priority >= CurrentSession.CurrentRole.Priority)
                    {
                        if (dr == DialogResult.Yes)
                        {
                            try
                            {
                                worker.IsRemoved = true;
                                db.Entry(worker).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Ошибка подключения к базе данных \r\n" + ex.Message, "Ошибка", MessageBoxButtons.OK);
                            }
                        }
                        LoadUsers();
                    }
                    else
                    {
                        MessageBox.Show("Вы не можете удалить данного пользователя", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void dgvUsers_AllowUserToDeleteRowsChanged(object sender, EventArgs e)
        {
            btRemoveUser.PerformClick();
        }

        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btEditUser.PerformClick();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            SaveData();
            if (currentSelectedValueRole != -1)
                SavePermissions(currentSelectedValueRole);
        }

        private void SaveData()
        {
            try
            {
                if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage1"])
                {
                    int a = db.SaveChanges();
                    SelectCatigory(lbCatigory.SelectedIndex);
                    btSave.Enabled = false;
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

        private void SelectCatigory(int indexCatigory)
        {
            try
            {
                string cat = lbCatigory.GetItemText(lbCatigory.Items[indexCatigory]);
                dgvDirectory.DataSource = null;
                dgvDirectory.Rows.Clear();
                dgvDirectory.Columns.Clear();

                switch (cat)
                {
                    case "Направления":
                        db.Directions.Where(a => a.IsRemoved == false).OrderBy(a => a.Name).Load();
                        dgvDirectory.DataSource = db.Directions.Local.ToBindingList();
                        dgvDirectory.Columns["Id"].Visible = dgvDirectory.Columns["IsRemoved"].Visible = false;
                        dgvDirectory.Columns["Name"].HeaderText = "Название направления";
                        dgvDirectory.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        dgvDirectory.Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;

                        if (dgvDirectory.Rows.Count != 0)
                        {
                            dgvDirectory.Rows[0].Cells["Name"].Selected = true;
                        }
                        break;

                    case "Курсы":
                        //создание колонок таблицы
                        var idCourse = new DataGridViewTextBoxColumn();
                        var nameCourse = new DataGridViewTextBoxColumn();
                        var templateCourse = new DataGridViewTextBoxColumn();
                        var durationCourseColumn = new DataGridViewTextBoxColumn();
                        var durationExpressColumn = new DataGridViewTextBoxColumn();
                        var noteCourse = new DataGridViewTextBoxColumn();
                        var directionIdCourse = new DataGridViewComboBoxColumn();
                        var documentIdCourse = new DataGridViewComboBoxColumn();

                        //настройка колонок 
                        // idColumn                    
                        idCourse.DataPropertyName = "Id";
                        idCourse.Name = "Id";
                        idCourse.Visible = false;
                        // nameColumn                    
                        nameCourse.DataPropertyName = "Name";
                        nameCourse.HeaderText = "Название курса";
                        nameCourse.Name = "Name";
                        nameCourse.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
                        nameCourse.SortMode = DataGridViewColumnSortMode.NotSortable;
                        // templateColumn                    
                        templateCourse.DataPropertyName = "Template";
                        templateCourse.HeaderText = "Краткое название";
                        templateCourse.Name = "Template";
                        templateCourse.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        templateCourse.SortMode = DataGridViewColumnSortMode.NotSortable;
                        // durationCourseColumn
                        durationCourseColumn.DataPropertyName = "DurationCourse";
                        durationCourseColumn.HeaderText = "Продолжительность курса, ч";
                        durationCourseColumn.Name = "DurationCourse";
                        durationCourseColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        durationCourseColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        // durationExpressColumn
                        durationExpressColumn.DataPropertyName = "DurationExpress";
                        durationExpressColumn.HeaderText = "Краткосрочный курс, ч";
                        durationExpressColumn.Name = "DurationExpress";
                        durationExpressColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        // noteColumn
                        noteCourse.DataPropertyName = "Note";
                        noteCourse.HeaderText = "Примечание";
                        noteCourse.Name = "Note";
                        noteCourse.SortMode = DataGridViewColumnSortMode.NotSortable;
                        // directionIdColumn
                        directionIdCourse.DataPropertyName = "DirectionId";
                        directionIdCourse.DataSource = db.Directions.Where(a => a.IsRemoved == false).ToList();
                        directionIdCourse.DisplayMember = "Name";
                        directionIdCourse.HeaderText = "Направление";
                        directionIdCourse.Name = "DirectionId";
                        directionIdCourse.Resizable = DataGridViewTriState.True;
                        directionIdCourse.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        directionIdCourse.ValueMember = "Id";
                        directionIdCourse.SortMode = DataGridViewColumnSortMode.NotSortable;

                        // DocumentIdColumn
                        documentIdCourse.DataPropertyName = "DocumentId";
                        documentIdCourse.DataSource = db.Documents.Where(a => a.IsRemoved == false).ToList();
                        documentIdCourse.DisplayMember = "Name";
                        documentIdCourse.HeaderText = "Документ";
                        documentIdCourse.Name = "DocumentId";
                        documentIdCourse.Resizable = DataGridViewTriState.True;
                        documentIdCourse.SortMode = DataGridViewColumnSortMode.NotSortable;
                        documentIdCourse.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        documentIdCourse.ValueMember = "Id";

                        //заполнение и настройка dgv
                        dgvDirectory.Columns.AddRange(new DataGridViewColumn[] {
                    idCourse, nameCourse, templateCourse, durationCourseColumn, durationExpressColumn,
                        noteCourse, directionIdCourse, documentIdCourse});
                        db.Courses.Where(a => a.IsRemoved == false).OrderBy(a => a.Name).Load();
                        dgvDirectory.DataSource = db.Courses.Local.ToBindingList();
                        dgvDirectory.Columns["Document"].Visible =
                        dgvDirectory.Columns["Direction"].Visible = dgvDirectory.Columns["IsRemoved"].Visible = false;

                        if (dgvDirectory.Rows.Count != 0)
                        {
                            dgvDirectory.Rows[0].Cells["Name"].Selected = true;
                        }
                        break;


                    case "Учебные года":
                        db.Years.Where(a => a.IsRemoved == false).OrderBy(a => a.Name).Load();
                        dgvDirectory.DataSource = db.Years.Local.ToBindingList();
                        dgvDirectory.Columns["Id"].Visible = dgvDirectory.Columns["IsRemoved"].Visible = false;
                        dgvDirectory.Columns["Name"].HeaderText = "Учебный год";
                        dgvDirectory.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        dgvDirectory.Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        if (dgvDirectory.Rows.Count != 0)
                        {
                            dgvDirectory.Rows[0].Cells["Name"].Selected = true;
                        }
                        break;


                    case "Корпуса":
                        db.Housings.Where(a => a.IsRemoved == false).OrderBy(a => a.Name).Load();
                        dgvDirectory.DataSource = db.Housings.Local.ToBindingList();
                        dgvDirectory.Columns["Id"].Visible = dgvDirectory.Columns["IsRemoved"].Visible = false;
                        dgvDirectory.Columns["Name"].HeaderText = "Название направления";
                        dgvDirectory.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        dgvDirectory.Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        if (dgvDirectory.Rows.Count != 0)
                        {
                            dgvDirectory.Rows[0].Cells["Name"].Selected = true;
                        }
                        break;


                    case "Классы":
                        //создание колонок таблицы
                        var idClass = new DataGridViewTextBoxColumn();
                        var nameClass = new DataGridViewTextBoxColumn();
                        var housingIdColumn = new DataGridViewComboBoxColumn();

                        //настройка колонок 
                        // idClass                    
                        idClass.DataPropertyName = "Id";
                        idClass.Name = "Id";
                        idClass.Visible = false;
                        // nameClass                    
                        nameClass.DataPropertyName = "Name";
                        nameClass.HeaderText = "Название класса";
                        nameClass.Name = "Name";
                        nameClass.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        nameClass.SortMode = DataGridViewColumnSortMode.NotSortable;
                        // HousingIdColumn
                        housingIdColumn.DataPropertyName = "HousingId";
                        housingIdColumn.DataSource = db.Housings.Where(a => a.IsRemoved == false).ToList();
                        housingIdColumn.DisplayMember = "Name";
                        housingIdColumn.HeaderText = "Корпус";
                        housingIdColumn.Name = "HousingId";
                        housingIdColumn.Resizable = DataGridViewTriState.True;
                        housingIdColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        housingIdColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        housingIdColumn.ValueMember = "Id";
                        //заполнение и настройка dgv
                        dgvDirectory.Columns.AddRange(new DataGridViewColumn[] {
                    idClass, nameClass, housingIdColumn });

                        db.Classes.Where(a => a.IsRemoved == false).OrderBy(a => a.Name).Load();
                        dgvDirectory.DataSource = db.Classes.Local.ToBindingList();
                        dgvDirectory.Columns["Housing"].Visible = dgvDirectory.Columns["IsRemoved"].Visible = false;

                        if (dgvDirectory.Rows.Count != 0)
                        {
                            dgvDirectory.Rows[0].Cells["Name"].Selected = true;
                        }
                        break;


                    case "Города":
                        db.Cities.Where(a => a.IsRemoved == false).OrderBy(a => a.Name).Load();
                        dgvDirectory.DataSource = db.Cities.Local.ToBindingList();
                        dgvDirectory.Columns["Id"].Visible = dgvDirectory.Columns["IsRemoved"].Visible = false;
                        dgvDirectory.Columns["Name"].HeaderText = "Название города";
                        dgvDirectory.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dgvDirectory.Columns["defaultCity"].HeaderText = "Город по умолчанию";
                        dgvDirectory.Columns["defaultCity"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        dgvDirectory.Columns["Name"].SortMode = dgvDirectory.Columns["defaultCity"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        if (dgvDirectory.Rows.Count != 0)
                        {
                            dgvDirectory.Rows[0].Cells["Name"].Selected = true;
                        }
                        break;


                    case "Улицы":
                        //создание колонок таблицы
                        var idStreet = new DataGridViewTextBoxColumn();
                        var nameStreet = new DataGridViewTextBoxColumn();
                        var cityIdColumn = new DataGridViewComboBoxColumn();

                        //настройка колонок 
                        // idStreet                    
                        idStreet.DataPropertyName = "Id";
                        idStreet.Name = "Id";
                        idStreet.Visible = false;
                        // name                  
                        nameStreet.DataPropertyName = "Name";
                        nameStreet.HeaderText = "Название улицы";
                        nameStreet.Name = "Name";
                        nameStreet.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        nameStreet.SortMode = DataGridViewColumnSortMode.NotSortable;
                        // StreetIdColumn
                        cityIdColumn.DataPropertyName = "CityId";
                        cityIdColumn.DataSource = db.Cities.Where(a => a.IsRemoved == false).ToList();
                        cityIdColumn.DisplayMember = "Name";
                        cityIdColumn.HeaderText = "Город";
                        cityIdColumn.Name = "CityId";
                        cityIdColumn.Resizable = DataGridViewTriState.True;
                        cityIdColumn.SortMode = DataGridViewColumnSortMode.Automatic;
                        cityIdColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        cityIdColumn.ValueMember = "Id";
                        cityIdColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        //заполнение и настройка dgv
                        dgvDirectory.Columns.AddRange(new DataGridViewColumn[] {
                    idStreet, nameStreet, cityIdColumn });
                        db.Streets.Where(a => a.IsRemoved == false).OrderBy(a => a.Name).Load();
                        dgvDirectory.DataSource = db.Streets.Local.ToBindingList();
                        dgvDirectory.Columns["City"].Visible = dgvDirectory.Columns["IsRemoved"].Visible = false;

                        if (dgvDirectory.Rows.Count != 0)
                        {
                            dgvDirectory.Rows[0].Cells["Name"].Selected = true;
                        }
                        break;


                    case "Учебные заведения":
                        //создание колонок таблицы
                        var idSchool = new DataGridViewTextBoxColumn();
                        var nameSchool = new DataGridViewTextBoxColumn();
                        var levelIdColumn = new DataGridViewComboBoxColumn();

                        //настройка колонок 
                        // idSchool                    
                        idSchool.DataPropertyName = "Id";
                        idSchool.Name = "Id";
                        idSchool.Visible = false;
                        // nameSchool                  
                        nameSchool.DataPropertyName = "Name";
                        nameSchool.HeaderText = "Название учреждения";
                        nameSchool.Name = "Name";
                        nameSchool.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        nameSchool.SortMode = DataGridViewColumnSortMode.NotSortable;
                        // levelIdColumn
                        levelIdColumn.DataPropertyName = "StatusId";
                        levelIdColumn.DataSource = db.Statuses.Where(a => a.IsRemoved == false).ToList();
                        levelIdColumn.DisplayMember = "Name";
                        levelIdColumn.HeaderText = "Отношение к статусу обучающегося";
                        levelIdColumn.Name = "StatusId";
                        levelIdColumn.Resizable = DataGridViewTriState.True;
                        levelIdColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        levelIdColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        levelIdColumn.ValueMember = "Id";
                        //заполнение и настройка dgv
                        dgvDirectory.Columns.AddRange(new DataGridViewColumn[] {
                    idSchool, nameSchool, levelIdColumn });
                        db.Schools.Where(a => a.IsRemoved == false).OrderBy(a => a.StatusId).ThenBy(a => a.Name).Load();
                        dgvDirectory.DataSource = db.Schools.Local.ToBindingList();
                        dgvDirectory.Columns["Status"].Visible = dgvDirectory.Columns["IsRemoved"].Visible = false;

                        if (dgvDirectory.Rows.Count != 0)
                        {
                            dgvDirectory.Rows[0].Cells["Name"].Selected = true;
                        }
                        break;


                    //case "Статусы групп":
                    //    dgvDirectory.DataSource = db.ActivityGroups.Where(a => a.IsRemoved == false).ToList();
                    //    dgvDirectory.Columns["Id"].Visible = dgvDirectory.Columns["IsRemoved"].Visible = false;
                    //    dgvDirectory.Columns["Name"].HeaderText = "Статус группы";
                    //    dgvDirectory.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    //    break;


                    //case "Статусы обучающихся":
                    //    dgvDirectory.DataSource = db.Statuses.Where(a => a.IsRemoved == false).ToList();
                    //    dgvDirectory.Columns["Id"].Visible = dgvDirectory.Columns["IsRemoved"].Visible = false;
                    //    dgvDirectory.Columns["Name"].HeaderText = "Статус обучающегося";
                    //    dgvDirectory.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    //    break;


                    case "Виды документов":
                        db.Documents.Where(a => a.IsRemoved == false).OrderBy(a => a.Name).Load();
                        dgvDirectory.DataSource = db.Documents.Local.ToBindingList();
                        dgvDirectory.Columns["Id"].Visible = dgvDirectory.Columns["IsRemoved"].Visible = false;
                        dgvDirectory.Columns["Name"].HeaderText = "Документы";
                        dgvDirectory.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dgvDirectory.Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        if (dgvDirectory.Rows.Count != 0)
                        {
                            dgvDirectory.Rows[0].Cells["Name"].Selected = true;
                        }
                        break;

                    case "Подразделения":
                        db.Departments.Where(a => a.IsRemoved == false).OrderBy(a => a.Name).Load();
                        dgvDirectory.DataSource = db.Departments.Local.ToBindingList();

                        dgvDirectory.Columns["Id"].Visible = dgvDirectory.Columns["IsRemoved"].Visible = false;
                        dgvDirectory.Columns["Name"].HeaderText = "Подразделения";
                        dgvDirectory.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dgvDirectory.Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        if (dgvDirectory.Rows.Count != 0)
                        {
                            dgvDirectory.Rows[0].Cells["Name"].Selected = true;
                        }
                        break;

                    case "Должности":
                        db.Posts.OrderBy(a => a.Name).Load();
                        dgvDirectory.DataSource = db.Posts.Local.ToBindingList();

                        dgvDirectory.Columns["Id"].Visible = false;
                        dgvDirectory.Columns["Name"].HeaderText = "Должности";
                        dgvDirectory.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dgvDirectory.Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        if (dgvDirectory.Rows.Count != 0)
                        {
                            dgvDirectory.Rows[0].Cells["Name"].Selected = true;
                        }
                        break;

                    case "Причины отчисления":
                        db.Causes.Where(a => a.IsRemoved == false).OrderBy(a => a.Name).Load();
                        dgvDirectory.DataSource = db.Causes.Local.ToBindingList();
                        dgvDirectory.Columns["Id"].Visible = dgvDirectory.Columns["IsRemoved"].Visible = false;
                        dgvDirectory.Columns["Name"].HeaderText = "Причина";
                        dgvDirectory.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dgvDirectory.Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        if (dgvDirectory.Rows.Count != 0)
                        {
                            dgvDirectory.Rows[0].Cells["Name"].Selected = true;
                        }
                        break;

                    case "Основания скидок":
                        db.Privileges.OrderBy(a => a.Name).Load();
                        dgvDirectory.DataSource = db.Privileges.Local.ToBindingList();

                        dgvDirectory.Columns["Id"].Visible = false;
                        dgvDirectory.Columns["Name"].HeaderText = "Основание";
                        dgvDirectory.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dgvDirectory.Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        if (dgvDirectory.Rows.Count > 1)
                        {
                            dgvDirectory.Rows[0].Cells["Name"].Selected = true;
                        }
                        break;

                    case "Роли":
                        db.Roles.Where(a => a.IsRemoved == false).OrderBy(a => a.Name).Load();
                        dgvDirectory.DataSource = db.Roles.Local.ToBindingList();

                        dgvDirectory.Columns["Id"].Visible = dgvDirectory.Columns["IsRemoved"].Visible = false;
                        dgvDirectory.Columns["Name"].HeaderText = "Роль";
                        dgvDirectory.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dgvDirectory.Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;

                        if (CurrentSession.CurrentRole.Name == "Администратор")
                        {
                            dgvDirectory.Columns["Priority"].HeaderText = "Вес роли";
                            dgvDirectory.Columns["Priority"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            dgvDirectory.Columns["Priority"].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        else
                        {
                            dgvDirectory.Columns["Priority"].Visible = false;
                        }

                        if (dgvDirectory.Rows.Count > 1)
                        {
                            dgvDirectory.Rows[0].Cells["Name"].Selected = true;
                        }
                        break;
                }
                btSave.Enabled = false;
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = lbCatigory.SelectedIndex;
            SelectCatigory(ind);
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDirectory.DataSource != null && dgvDirectory.CurrentCell.RowIndex != -1)
                {
                    int indCatigory = lbCatigory.SelectedIndex;
                    string cat = lbCatigory.GetItemText(lbCatigory.Items[indCatigory]);

                    int indObject = dgvDirectory.CurrentCell.RowIndex;
                    int id = Convert.ToInt32(dgvDirectory.Rows[indObject].Cells["Id"].Value);

                    switch (cat)
                    {
                        case "Направления":
                            var currentDirection = db.Directions.Find(id);
                            var courses = db.Courses.Where(a => a.DirectionId == currentDirection.Id && a.IsRemoved == false).ToList();
                            if (courses.Count == 0)
                            {
                                if (MessageBox.Show("Вы действительно хотите удалить выбранное направление?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    currentDirection.IsRemoved = true;
                                    db.Entry(currentDirection).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                if (MessageBox.Show("По выбранному направлению имеется несколько курсов.\r\n Удаление направления приведет к удалению этих курсов!\r\nВы действительно хотите удалить выбранное направление?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    //удаление направления
                                    currentDirection.IsRemoved = true;
                                    db.Entry(currentDirection).State = EntityState.Modified;
                                    //удаление курсов
                                    foreach (var course in courses)
                                    {
                                        course.IsRemoved = true;
                                        db.Entry(course).State = EntityState.Modified;
                                    }
                                }
                            }
                            break;

                        case "Курсы":
                            if (MessageBox.Show("Вы действительно хотите удалить выбранный курс?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                //удаление курса
                                var currentCourse = db.Courses.Find(id);
                                currentCourse.IsRemoved = true;
                                db.Entry(currentCourse).State = EntityState.Modified;
                            }
                            break;

                        case "Учебные года":
                            var currentYear = db.Years.Find(id);
                            var groups = db.Groups.Where(a => a.YearId == currentYear.Id).ToList();
                            if (groups.Count == 0)
                            {
                                if (MessageBox.Show("Вы действительно хотите удалить выбранный учебный год?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    //удаление учебного года
                                    currentYear.IsRemoved = true;
                                    db.Entry(currentYear).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                if (MessageBox.Show("С выбранным учебным годом связано несколько групп.\r\n Удаление учебного года переведет данные группы в разряд краткосрочных и для них повторно нужно будет выбрать учебный год!\r\nВы действительно хотите удалить выбранный учебный год?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    //удаление учебного года
                                    currentYear.IsRemoved = true;
                                    db.Entry(currentYear).State = EntityState.Modified;
                                    //удаление учебного года из групп
                                    foreach (var group in groups)
                                    {
                                        group.YearId = null;
                                        db.Entry(group).State = EntityState.Modified;
                                    }
                                }
                            }
                            break;

                        case "Корпуса":
                            var currentHousing = db.Housings.Find(id);
                            var classes = db.Classes.Where(a => a.HousingId == currentHousing.Id && a.IsRemoved == false).ToList();
                            if (classes.Count == 0)
                            {
                                if (MessageBox.Show("Вы действительно хотите удалить выбранный корпус?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    //удаление учебного года
                                    currentHousing.IsRemoved = true;
                                    db.Entry(currentHousing).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                if (MessageBox.Show("За удаляемым корпусом закреплено несколько классов.\r\n Удаление корпуса приведет к удалению классов находящихся в этом корпусе!\r\nВы действительно хотите удалить выбранный корпус?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    //удаление корпуса
                                    currentHousing.IsRemoved = true;
                                    db.Entry(currentHousing).State = EntityState.Modified;
                                    //удаление классов корпуса
                                    foreach (var @class in classes)
                                    {
                                        @class.IsRemoved = true;
                                        db.Entry(@class).State = EntityState.Modified;
                                    }
                                }
                            }
                            break;

                        case "Классы":
                            //удаление класса
                            if (MessageBox.Show("Вы действительно хотите удалить выбранный класс?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                var currentClass = db.Classes.Find(id);
                                currentClass.IsRemoved = true;
                                db.Entry(currentClass).State = EntityState.Modified;
                            }
                            break;

                        case "Города":
                            var currentCity = db.Cities.Find(id);
                            var streets = db.Streets.Where(a => a.CityId == currentCity.Id && a.IsRemoved == false).ToList();
                            if (streets.Count == 0)
                            {
                                if (MessageBox.Show("Удаление города приведет к очистке этой информации из данных учащихся. \r\nВы действительно хотите удалить выбранный город?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    //удаление города
                                    currentCity.IsRemoved = true;
                                    db.Entry(currentCity).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                if (MessageBox.Show("Удаление города приведет к очистке этой информации из данных учащихся. \r\nЗа удаляемым городом закреплено несколько улиц.\r\n Удаление города приведет к удалению улиц связанных с данным городом!\r\nВы действительно хотите удалить выбранный город?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    //удаление города
                                    currentCity.IsRemoved = true;
                                    db.Entry(currentCity).State = EntityState.Modified;
                                    //удаление улиц
                                    foreach (var street in streets)
                                    {
                                        street.IsRemoved = true;
                                        db.Entry(street).State = EntityState.Modified;
                                    }
                                }
                            }
                            break;

                        case "Улицы":
                            //удаление улицы
                            if (MessageBox.Show("Удаление улицы приведет к очистке этой информации из данных учащихся. \r\nВы действительно хотите удалить выбранную улицу?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                var currentStreet = db.Streets.Find(id);
                                currentStreet.IsRemoved = true;
                                db.Entry(currentStreet).State = EntityState.Modified;
                            }
                            break;

                        case "Учебные заведения":
                            //удаление учебного заведения
                            if (MessageBox.Show("Удаление учебного заведения приведет к очистке этой информации из данных учащихся. \r\nВы действительно хотите удалить выбранное учебное заведение?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                var currentSchool = db.Schools.Find(id);
                                currentSchool.IsRemoved = true;
                                db.Entry(currentSchool).State = EntityState.Modified;
                            }
                            break;

                        case "Виды документов":
                            //удаление вида документа
                            if (MessageBox.Show("Вы действительно хотите удалить выбранный вид документ?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                var currentDoc = db.Documents.Find(id);
                                currentDoc.IsRemoved = true;
                                db.Entry(currentDoc).State = EntityState.Modified;
                            }
                            break;

                        case "Причины отчисления":
                            //удаление причины
                            if (MessageBox.Show("Вы действительно хотите удалить выбранную причину отчисления?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                var currentCause = db.Causes.Find(id);
                                currentCause.IsRemoved = true;
                                db.Entry(currentCause).State = EntityState.Modified;
                            }
                            break;

                        case "Основания скидок":
                            //удаление причины
                            if (MessageBox.Show("Вы действительно хотите удалить основание скидки? Удаление основания приведет к удалению всех начисленных скидок учащимся.", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                var priv = db.Privileges.Find(id);
                                var enrolls = db.Enrollments.Where(a => a.PrivilegeId == id).ToList();
                                string f = string.Join(",", enrolls.Select(b => b.Id.ToString()));
                                int numberOfRowUpdated = db.Database.ExecuteSqlCommand("UPDATE Schedules SET Discount=0 WHERE EnrollmentId IN (" + f + ")");
                                db.Privileges.Remove(priv);
                            }
                            break;

                        case "Роли":
                            var currentRole = db.Roles.Find(id);
                            var workers = db.Workers.Where(a => a.RoleId == currentRole.Id && a.IsRemoved == false).ToList();
                            if (workers.Count == 0)
                            {
                                if (MessageBox.Show("Вы действительно хотите удалить роль?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    //удаление города
                                    currentRole.IsRemoved = true;
                                    db.Entry(currentRole).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Удаляемая роль назначена одному или нескольким сотрудникам. \r\nУдалите этих сотрудников или смените их роль перед удаленим роли!",
                                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            break;

                        case "Подразделения":
                            //удаление отдела                            
                            if (MessageBox.Show("Вы действительно хотите удалить выбранное подразделения?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                var currentDepartment = db.Departments.Find(id);
                                if (currentDepartment.Name != "Все")
                                {
                                    currentDepartment.IsRemoved = true;
                                    db.Entry(currentDepartment).State = EntityState.Modified;
                                }
                                else MessageBox.Show("Данное подразделение удалить невозможно", "Внимание", MessageBoxButtons.OK);
                            }
                            break;

                        case "Должности":
                            //удаление должности
                            if (MessageBox.Show("Вы действительно хотите удалить выбранную должность?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                var currentPost = db.Posts.Find(id);
                                db.Posts.Remove(currentPost);
                            }
                            break;
                    }
                    db.SaveChanges();
                    SelectCatigory(indCatigory);

                    if (indObject != dgvDirectory.Rows.Count)
                    {
                        if (dgvDirectory.Rows.Count != 0)
                        {
                            dgvDirectory.Rows[indObject].Selected = true;
                            dgvDirectory.CurrentCell = dgvDirectory.Rows[indObject].Cells["Name"];
                        }
                    }
                    else
                    {
                        dgvDirectory.Rows[dgvDirectory.Rows.Count - 1].Selected = true;
                        dgvDirectory.CurrentCell = dgvDirectory.Rows[indObject].Cells["Name"];
                    }
                }
                else
                {
                    MessageBox.Show("Не выбран объект для удаления", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void dgvDirectory_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            btSave.Enabled = true;
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            if (dgvDirectory.AllowUserToAddRows == false)
                dgvDirectory.AllowUserToAddRows = true;
            else
                dgvDirectory.AllowUserToAddRows = false;
        }

        private void lbRoles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lbRoles_Click(object sender, EventArgs e)
        {
            if (lbRoles.SelectedValue != null)
            {
                //сохраняем права предыдущей роли
                if (btSave.Enabled && currentSelectedValueRole != -1) SavePermissions(currentSelectedValueRole);
                //получаем id выбранной роли, для сохранения настроек при смене роли
                currentSelectedValueRole = (int)lbRoles.SelectedValue;

                var lstManageRole = db.ManageRoles.Where(a => a.RoleId == currentSelectedValueRole).ToList();
                if (lstManageRole.Count == 0)
                {
                    CreatePermission(currentSelectedValueRole, false);
                    LoadPermissionsRole(currentSelectedValueRole);
                }
                else
                    LoadPermissionsRole(currentSelectedValueRole);
            }
        }

        private void ListStudents_CheckedChanged(object sender, EventArgs e)
        {
            btSave.Enabled = true;
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            if (lbRoles.SelectedValue != null)
            {
                var role = (int)lbRoles.SelectedValue;
                if (btSave.Enabled && currentSelectedValueRole != -1) SavePermissions(currentSelectedValueRole);
            }
        }

        private void SettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btSave.Enabled)
            {
                if (currentSelectedValueRole != -1)
                    SavePermissions(currentSelectedValueRole);
                SaveData();
            }
        }

        private void Journal_CheckedChanged(object sender, EventArgs e)
        {
            btSave.Enabled = true;
            if (Journal.Checked)
            {
                AddLesson.Enabled = DeleteLesson.Enabled = true;
                EnrollInGroup.Enabled = ExclusionFromGroup.Enabled = true;
            }
            else
            {
                AddLesson.Enabled = DeleteLesson.Enabled = false;
                EnrollInGroup.Enabled = ExclusionFromGroup.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvGroups.CurrentCell != null)
            {
                var rows = dgvGroups.SelectedRows
                    .OfType<DataGridViewRow>()                    
                    .ToArray();

                foreach (var row in rows)
                {
                    //сохранение списка доступных групп
                    db.AccessGroups.Add(new AccessGroups
                    {
                        WorkerId = (int)dgvWorkers.Rows[dgvWorkers.CurrentCell.RowIndex].Cells["Id"].Value,
                        GroupId = (int)row.Cells["Id"].Value
                    });
                }
                db.SaveChanges();

                //обновление списков групп
                var id = (int)dgvWorkers.Rows[dgvWorkers.CurrentCell.RowIndex].Cells["Id"].Value;
                LoadGroups(id);
                LoadSecretaryGroups(id);
            }
        }

        private void dgvListGroupsWorker_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvListGroupsWorker.Columns["GroupId"].Visible = 
                dgvListGroupsWorker.Columns["WorkerId"].Visible =
                dgvListGroupsWorker.Columns["Id"].Visible = false;
            dgvListGroupsWorker.Columns["Name"].HeaderText = "Название";
            dgvListGroupsWorker.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void dgvWorkers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var id = (int)dgvWorkers.Rows[dgvWorkers.CurrentCell.RowIndex].Cells["Id"].Value;
            LoadGroups(id);
            LoadSecretaryGroups(id);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvListGroupsWorker.CurrentCell != null)
            {
                var rows = dgvListGroupsWorker.SelectedRows
                    .OfType<DataGridViewRow>()
                    .ToArray();

                foreach (var row in rows)
                {
                    //сохранение списка доступных групп
                    int id = (int)row.Cells["Id"].Value;
                    var a = db.AccessGroups.Find(id);
                    if (a != null)
                    {
                        db.AccessGroups.Remove(a);
                    }
                }
                db.SaveChanges();

                //обновление списков групп
                var workerId = (int)dgvWorkers.Rows[dgvWorkers.CurrentCell.RowIndex].Cells["Id"].Value;
                LoadGroups(workerId);
                LoadSecretaryGroups(workerId);
            }
        }
    }
}
//Что делать с доступом к архиву студентов
//что делать с возможностью записи студента в группу (в принципе, все отчисления будут видны в архиве)