namespace Istra
{
    partial class SettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvDirectory = new System.Windows.Forms.DataGridView();
            this.lbCatigory = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btRemove = new System.Windows.Forms.Button();
            this.btAdd = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.btRemoveUser = new System.Windows.Forms.Button();
            this.btEditUser = new System.Windows.Forms.Button();
            this.btAddUser = new System.Windows.Forms.Button();
            this.tpPermission = new System.Windows.Forms.TabPage();
            this.ListLessons = new System.Windows.Forms.CheckBox();
            this.DeleteLesson = new System.Windows.Forms.CheckBox();
            this.AddLesson = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PrintDoc = new System.Windows.Forms.CheckBox();
            this.SchedulerPayGroup = new System.Windows.Forms.CheckBox();
            this.Journal = new System.Windows.Forms.CheckBox();
            this.ExclusionFromGroup = new System.Windows.Forms.CheckBox();
            this.FinanceReports = new System.Windows.Forms.CheckBox();
            this.EditGroup = new System.Windows.Forms.CheckBox();
            this.ArchiveStudents = new System.Windows.Forms.CheckBox();
            this.EditStudent = new System.Windows.Forms.CheckBox();
            this.Wages = new System.Windows.Forms.CheckBox();
            this.EnrollInGroup = new System.Windows.Forms.CheckBox();
            this.Directories = new System.Windows.Forms.CheckBox();
            this.PlanEnroll = new System.Windows.Forms.CheckBox();
            this.DetailsGroup = new System.Windows.Forms.CheckBox();
            this.DetailsStudent = new System.Windows.Forms.CheckBox();
            this.AccessGroups = new System.Windows.Forms.CheckBox();
            this.ManageRoles = new System.Windows.Forms.CheckBox();
            this.ManageUsers = new System.Windows.Forms.CheckBox();
            this.ListPayments = new System.Windows.Forms.CheckBox();
            this.ListGroups = new System.Windows.Forms.CheckBox();
            this.ListStudents = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbRoles = new System.Windows.Forms.ListBox();
            this.tpAccessGroups = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvWorkers = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvListGroupsWorker = new System.Windows.Forms.DataGridView();
            this.dgvGroups = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDirectory)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.tpPermission.SuspendLayout();
            this.tpAccessGroups.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListGroupsWorker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroups)).BeginInit();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tpPermission);
            this.tabControl1.Controls.Add(this.tpAccessGroups);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 4);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(847, 437);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabIndexChanged += new System.EventHandler(this.tabControl1_TabIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(839, 404);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Справочники";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvDirectory);
            this.panel2.Controls.Add(this.lbCatigory);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(780, 396);
            this.panel2.TabIndex = 3;
            // 
            // dgvDirectory
            // 
            this.dgvDirectory.AllowUserToResizeRows = false;
            this.dgvDirectory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDirectory.Location = new System.Drawing.Point(182, 0);
            this.dgvDirectory.MultiSelect = false;
            this.dgvDirectory.Name = "dgvDirectory";
            this.dgvDirectory.RowHeadersVisible = false;
            this.dgvDirectory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDirectory.Size = new System.Drawing.Size(598, 396);
            this.dgvDirectory.TabIndex = 1;
            this.dgvDirectory.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDirectory_CellValueChanged);
            // 
            // lbCatigory
            // 
            this.lbCatigory.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbCatigory.FormattingEnabled = true;
            this.lbCatigory.ItemHeight = 17;
            this.lbCatigory.Items.AddRange(new object[] {
            "Направления",
            "Курсы",
            "Учебные года",
            "Корпуса",
            "Классы",
            "Города",
            "Улицы",
            "Учебные заведения",
            "Виды документов",
            "Причины отчисления",
            "Основания скидок",
            "Роли",
            "Подразделения",
            "Должности"});
            this.lbCatigory.Location = new System.Drawing.Point(0, 0);
            this.lbCatigory.Name = "lbCatigory";
            this.lbCatigory.Size = new System.Drawing.Size(182, 396);
            this.lbCatigory.TabIndex = 0;
            this.lbCatigory.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btRemove);
            this.panel1.Controls.Add(this.btAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(784, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(51, 396);
            this.panel1.TabIndex = 2;
            // 
            // btRemove
            // 
            this.btRemove.Image = global::Istra.Properties.Resources.trashcan_delete2;
            this.btRemove.Location = new System.Drawing.Point(3, 3);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(45, 40);
            this.btRemove.TabIndex = 0;
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
            // 
            // btAdd
            // 
            this.btAdd.Image = global::Istra.Properties.Resources.plus;
            this.btAdd.Location = new System.Drawing.Point(3, 49);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(45, 40);
            this.btAdd.TabIndex = 0;
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Visible = false;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvUsers);
            this.tabPage2.Controls.Add(this.btRemoveUser);
            this.tabPage2.Controls.Add(this.btEditUser);
            this.tabPage2.Controls.Add(this.btAddUser);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(839, 404);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Пользователи";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToResizeRows = false;
            this.dgvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(7, 3);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(779, 394);
            this.dgvUsers.TabIndex = 6;
            this.dgvUsers.AllowUserToDeleteRowsChanged += new System.EventHandler(this.dgvUsers_AllowUserToDeleteRowsChanged);
            this.dgvUsers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellDoubleClick);
            // 
            // btRemoveUser
            // 
            this.btRemoveUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemoveUser.Image = global::Istra.Properties.Resources.trashcan_delete2;
            this.btRemoveUser.Location = new System.Drawing.Point(792, 93);
            this.btRemoveUser.Name = "btRemoveUser";
            this.btRemoveUser.Size = new System.Drawing.Size(40, 39);
            this.btRemoveUser.TabIndex = 2;
            this.btRemoveUser.UseVisualStyleBackColor = true;
            this.btRemoveUser.Click += new System.EventHandler(this.btRemoveUser_Click);
            // 
            // btEditUser
            // 
            this.btEditUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btEditUser.Image = global::Istra.Properties.Resources.edit__1_;
            this.btEditUser.Location = new System.Drawing.Point(792, 48);
            this.btEditUser.Name = "btEditUser";
            this.btEditUser.Size = new System.Drawing.Size(40, 39);
            this.btEditUser.TabIndex = 1;
            this.btEditUser.UseVisualStyleBackColor = true;
            this.btEditUser.Click += new System.EventHandler(this.btEditUser_Click);
            // 
            // btAddUser
            // 
            this.btAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddUser.Image = global::Istra.Properties.Resources.plus;
            this.btAddUser.Location = new System.Drawing.Point(792, 3);
            this.btAddUser.Name = "btAddUser";
            this.btAddUser.Size = new System.Drawing.Size(40, 39);
            this.btAddUser.TabIndex = 0;
            this.btAddUser.UseVisualStyleBackColor = true;
            this.btAddUser.Click += new System.EventHandler(this.btAddUser_Click);
            // 
            // tpPermission
            // 
            this.tpPermission.Controls.Add(this.ListLessons);
            this.tpPermission.Controls.Add(this.DeleteLesson);
            this.tpPermission.Controls.Add(this.AddLesson);
            this.tpPermission.Controls.Add(this.label5);
            this.tpPermission.Controls.Add(this.PrintDoc);
            this.tpPermission.Controls.Add(this.SchedulerPayGroup);
            this.tpPermission.Controls.Add(this.Journal);
            this.tpPermission.Controls.Add(this.ExclusionFromGroup);
            this.tpPermission.Controls.Add(this.FinanceReports);
            this.tpPermission.Controls.Add(this.EditGroup);
            this.tpPermission.Controls.Add(this.ArchiveStudents);
            this.tpPermission.Controls.Add(this.EditStudent);
            this.tpPermission.Controls.Add(this.Wages);
            this.tpPermission.Controls.Add(this.EnrollInGroup);
            this.tpPermission.Controls.Add(this.Directories);
            this.tpPermission.Controls.Add(this.PlanEnroll);
            this.tpPermission.Controls.Add(this.DetailsGroup);
            this.tpPermission.Controls.Add(this.DetailsStudent);
            this.tpPermission.Controls.Add(this.AccessGroups);
            this.tpPermission.Controls.Add(this.ManageRoles);
            this.tpPermission.Controls.Add(this.ManageUsers);
            this.tpPermission.Controls.Add(this.ListPayments);
            this.tpPermission.Controls.Add(this.ListGroups);
            this.tpPermission.Controls.Add(this.ListStudents);
            this.tpPermission.Controls.Add(this.label4);
            this.tpPermission.Controls.Add(this.label3);
            this.tpPermission.Controls.Add(this.label2);
            this.tpPermission.Controls.Add(this.label1);
            this.tpPermission.Controls.Add(this.lbRoles);
            this.tpPermission.Location = new System.Drawing.Point(4, 29);
            this.tpPermission.Name = "tpPermission";
            this.tpPermission.Padding = new System.Windows.Forms.Padding(3);
            this.tpPermission.Size = new System.Drawing.Size(839, 404);
            this.tpPermission.TabIndex = 2;
            this.tpPermission.Text = "Права";
            this.tpPermission.UseVisualStyleBackColor = true;
            // 
            // ListLessons
            // 
            this.ListLessons.AutoSize = true;
            this.ListLessons.Location = new System.Drawing.Point(584, 29);
            this.ListLessons.Name = "ListLessons";
            this.ListLessons.Size = new System.Drawing.Size(199, 21);
            this.ListLessons.TabIndex = 4;
            this.ListLessons.Text = "Просмотр списка занятий";
            this.ListLessons.UseVisualStyleBackColor = true;
            this.ListLessons.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // DeleteLesson
            // 
            this.DeleteLesson.AutoSize = true;
            this.DeleteLesson.Location = new System.Drawing.Point(584, 83);
            this.DeleteLesson.Name = "DeleteLesson";
            this.DeleteLesson.Size = new System.Drawing.Size(140, 21);
            this.DeleteLesson.TabIndex = 5;
            this.DeleteLesson.Text = "Удалять занятия";
            this.DeleteLesson.UseVisualStyleBackColor = true;
            this.DeleteLesson.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // AddLesson
            // 
            this.AddLesson.AutoSize = true;
            this.AddLesson.Location = new System.Drawing.Point(584, 56);
            this.AddLesson.Name = "AddLesson";
            this.AddLesson.Size = new System.Drawing.Size(157, 21);
            this.AddLesson.TabIndex = 6;
            this.AddLesson.Text = "Добавлять занятия";
            this.AddLesson.UseVisualStyleBackColor = true;
            this.AddLesson.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(566, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "Занятия";
            // 
            // PrintDoc
            // 
            this.PrintDoc.AutoSize = true;
            this.PrintDoc.Location = new System.Drawing.Point(255, 363);
            this.PrintDoc.Name = "PrintDoc";
            this.PrintDoc.Size = new System.Drawing.Size(156, 21);
            this.PrintDoc.TabIndex = 2;
            this.PrintDoc.Text = "Печать документов";
            this.PrintDoc.UseVisualStyleBackColor = true;
            this.PrintDoc.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // SchedulerPayGroup
            // 
            this.SchedulerPayGroup.AutoSize = true;
            this.SchedulerPayGroup.Location = new System.Drawing.Point(255, 336);
            this.SchedulerPayGroup.Name = "SchedulerPayGroup";
            this.SchedulerPayGroup.Size = new System.Drawing.Size(167, 21);
            this.SchedulerPayGroup.TabIndex = 2;
            this.SchedulerPayGroup.Text = "Начисления в группе";
            this.SchedulerPayGroup.UseVisualStyleBackColor = true;
            this.SchedulerPayGroup.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // Journal
            // 
            this.Journal.AutoSize = true;
            this.Journal.Location = new System.Drawing.Point(255, 254);
            this.Journal.Name = "Journal";
            this.Journal.Size = new System.Drawing.Size(155, 21);
            this.Journal.TabIndex = 2;
            this.Journal.Text = "Работа с журналом";
            this.Journal.UseVisualStyleBackColor = true;
            this.Journal.CheckedChanged += new System.EventHandler(this.Journal_CheckedChanged);
            // 
            // ExclusionFromGroup
            // 
            this.ExclusionFromGroup.AutoSize = true;
            this.ExclusionFromGroup.Location = new System.Drawing.Point(255, 308);
            this.ExclusionFromGroup.Name = "ExclusionFromGroup";
            this.ExclusionFromGroup.Size = new System.Drawing.Size(177, 21);
            this.ExclusionFromGroup.TabIndex = 2;
            this.ExclusionFromGroup.Text = "Отчисление из группы";
            this.ExclusionFromGroup.UseVisualStyleBackColor = true;
            this.ExclusionFromGroup.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // FinanceReports
            // 
            this.FinanceReports.AutoSize = true;
            this.FinanceReports.Location = new System.Drawing.Point(584, 197);
            this.FinanceReports.Name = "FinanceReports";
            this.FinanceReports.Size = new System.Drawing.Size(164, 21);
            this.FinanceReports.TabIndex = 2;
            this.FinanceReports.Text = "Финансовые отчеты";
            this.FinanceReports.UseVisualStyleBackColor = true;
            this.FinanceReports.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // EditGroup
            // 
            this.EditGroup.AutoSize = true;
            this.EditGroup.Location = new System.Drawing.Point(255, 228);
            this.EditGroup.Name = "EditGroup";
            this.EditGroup.Size = new System.Drawing.Size(239, 21);
            this.EditGroup.TabIndex = 2;
            this.EditGroup.Text = "Редактирование данных группы";
            this.EditGroup.UseVisualStyleBackColor = true;
            this.EditGroup.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // ArchiveStudents
            // 
            this.ArchiveStudents.AutoSize = true;
            this.ArchiveStudents.Location = new System.Drawing.Point(255, 110);
            this.ArchiveStudents.Name = "ArchiveStudents";
            this.ArchiveStudents.Size = new System.Drawing.Size(211, 21);
            this.ArchiveStudents.TabIndex = 2;
            this.ArchiveStudents.Text = "Работа с архивом учащихся";
            this.ArchiveStudents.UseVisualStyleBackColor = true;
            this.ArchiveStudents.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // EditStudent
            // 
            this.EditStudent.AutoSize = true;
            this.EditStudent.Location = new System.Drawing.Point(255, 83);
            this.EditStudent.Name = "EditStudent";
            this.EditStudent.Size = new System.Drawing.Size(263, 21);
            this.EditStudent.TabIndex = 2;
            this.EditStudent.Text = "Редактирование данных учащегося";
            this.EditStudent.UseVisualStyleBackColor = true;
            this.EditStudent.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // Wages
            // 
            this.Wages.AutoSize = true;
            this.Wages.Location = new System.Drawing.Point(584, 224);
            this.Wages.Name = "Wages";
            this.Wages.Size = new System.Drawing.Size(184, 21);
            this.Wages.TabIndex = 2;
            this.Wages.Text = "Начисление зар. платы";
            this.Wages.UseVisualStyleBackColor = true;
            this.Wages.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // EnrollInGroup
            // 
            this.EnrollInGroup.AutoSize = true;
            this.EnrollInGroup.Location = new System.Drawing.Point(255, 281);
            this.EnrollInGroup.Name = "EnrollInGroup";
            this.EnrollInGroup.Size = new System.Drawing.Size(165, 21);
            this.EnrollInGroup.TabIndex = 2;
            this.EnrollInGroup.Text = "Зачисление в группу";
            this.EnrollInGroup.UseVisualStyleBackColor = true;
            this.EnrollInGroup.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // Directories
            // 
            this.Directories.AutoSize = true;
            this.Directories.Location = new System.Drawing.Point(581, 281);
            this.Directories.Name = "Directories";
            this.Directories.Size = new System.Drawing.Size(114, 21);
            this.Directories.TabIndex = 2;
            this.Directories.Text = "Справочники";
            this.Directories.UseVisualStyleBackColor = true;
            this.Directories.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // PlanEnroll
            // 
            this.PlanEnroll.AutoSize = true;
            this.PlanEnroll.Location = new System.Drawing.Point(584, 170);
            this.PlanEnroll.Name = "PlanEnroll";
            this.PlanEnroll.Size = new System.Drawing.Size(233, 21);
            this.PlanEnroll.TabIndex = 2;
            this.PlanEnroll.Text = "Редактирование плана набора";
            this.PlanEnroll.UseVisualStyleBackColor = true;
            this.PlanEnroll.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // DetailsGroup
            // 
            this.DetailsGroup.AutoSize = true;
            this.DetailsGroup.Location = new System.Drawing.Point(255, 201);
            this.DetailsGroup.Name = "DetailsGroup";
            this.DetailsGroup.Size = new System.Drawing.Size(194, 21);
            this.DetailsGroup.TabIndex = 2;
            this.DetailsGroup.Text = "Просмотр данных группы";
            this.DetailsGroup.UseVisualStyleBackColor = true;
            this.DetailsGroup.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // DetailsStudent
            // 
            this.DetailsStudent.AutoSize = true;
            this.DetailsStudent.Location = new System.Drawing.Point(255, 56);
            this.DetailsStudent.Name = "DetailsStudent";
            this.DetailsStudent.Size = new System.Drawing.Size(231, 21);
            this.DetailsStudent.TabIndex = 2;
            this.DetailsStudent.Text = "Просмотр карточки учащегося";
            this.DetailsStudent.UseVisualStyleBackColor = true;
            this.DetailsStudent.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // AccessGroups
            // 
            this.AccessGroups.AutoSize = true;
            this.AccessGroups.Location = new System.Drawing.Point(581, 363);
            this.AccessGroups.Name = "AccessGroups";
            this.AccessGroups.Size = new System.Drawing.Size(240, 21);
            this.AccessGroups.TabIndex = 2;
            this.AccessGroups.Text = "Ограничение доступа к группам";
            this.AccessGroups.UseVisualStyleBackColor = true;
            this.AccessGroups.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // ManageRoles
            // 
            this.ManageRoles.AutoSize = true;
            this.ManageRoles.Location = new System.Drawing.Point(581, 336);
            this.ManageRoles.Name = "ManageRoles";
            this.ManageRoles.Size = new System.Drawing.Size(143, 21);
            this.ManageRoles.TabIndex = 2;
            this.ManageRoles.Text = "Назначение прав";
            this.ManageRoles.UseVisualStyleBackColor = true;
            this.ManageRoles.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // ManageUsers
            // 
            this.ManageUsers.AutoSize = true;
            this.ManageUsers.Location = new System.Drawing.Point(581, 308);
            this.ManageUsers.Name = "ManageUsers";
            this.ManageUsers.Size = new System.Drawing.Size(220, 21);
            this.ManageUsers.TabIndex = 2;
            this.ManageUsers.Text = "Управление пользователями";
            this.ManageUsers.UseVisualStyleBackColor = true;
            this.ManageUsers.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // ListPayments
            // 
            this.ListPayments.AutoSize = true;
            this.ListPayments.Location = new System.Drawing.Point(584, 143);
            this.ListPayments.Name = "ListPayments";
            this.ListPayments.Size = new System.Drawing.Size(209, 21);
            this.ListPayments.TabIndex = 2;
            this.ListPayments.Text = "Просмотр списка платежей";
            this.ListPayments.UseVisualStyleBackColor = true;
            this.ListPayments.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // ListGroups
            // 
            this.ListGroups.AutoSize = true;
            this.ListGroups.Location = new System.Drawing.Point(255, 174);
            this.ListGroups.Name = "ListGroups";
            this.ListGroups.Size = new System.Drawing.Size(181, 21);
            this.ListGroups.TabIndex = 2;
            this.ListGroups.Text = "Просмотр списка групп";
            this.ListGroups.UseVisualStyleBackColor = true;
            this.ListGroups.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // ListStudents
            // 
            this.ListStudents.AutoSize = true;
            this.ListStudents.Location = new System.Drawing.Point(255, 29);
            this.ListStudents.Name = "ListStudents";
            this.ListStudents.Size = new System.Drawing.Size(208, 21);
            this.ListStudents.TabIndex = 2;
            this.ListStudents.Text = "Просмотр списка учащихся";
            this.ListStudents.UseVisualStyleBackColor = true;
            this.ListStudents.CheckedChanged += new System.EventHandler(this.ListStudents_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(566, 261);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Администрирование";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(566, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Финансы";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(237, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Работа с группами";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(237, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Работа с учащимися";
            // 
            // lbRoles
            // 
            this.lbRoles.FormattingEnabled = true;
            this.lbRoles.ItemHeight = 17;
            this.lbRoles.Location = new System.Drawing.Point(6, 6);
            this.lbRoles.Name = "lbRoles";
            this.lbRoles.Size = new System.Drawing.Size(194, 395);
            this.lbRoles.TabIndex = 0;
            this.lbRoles.Click += new System.EventHandler(this.lbRoles_Click);
            this.lbRoles.SelectedIndexChanged += new System.EventHandler(this.lbRoles_SelectedIndexChanged);
            // 
            // tpAccessGroups
            // 
            this.tpAccessGroups.Controls.Add(this.tableLayoutPanel2);
            this.tpAccessGroups.Location = new System.Drawing.Point(4, 29);
            this.tpAccessGroups.Name = "tpAccessGroups";
            this.tpAccessGroups.Size = new System.Drawing.Size(839, 404);
            this.tpAccessGroups.TabIndex = 3;
            this.tpAccessGroups.Text = "Ограничение доступа к группам";
            this.tpAccessGroups.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.9946F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.0027F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.0027F));
            this.tableLayoutPanel2.Controls.Add(this.dgvWorkers, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.dgvListGroupsWorker, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.dgvGroups, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label7, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(839, 404);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // dgvWorkers
            // 
            this.dgvWorkers.AllowUserToResizeColumns = false;
            this.dgvWorkers.AllowUserToResizeRows = false;
            this.dgvWorkers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWorkers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWorkers.Location = new System.Drawing.Point(3, 23);
            this.dgvWorkers.MultiSelect = false;
            this.dgvWorkers.Name = "dgvWorkers";
            this.dgvWorkers.RowHeadersVisible = false;
            this.dgvWorkers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWorkers.Size = new System.Drawing.Size(352, 378);
            this.dgvWorkers.TabIndex = 0;
            this.dgvWorkers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWorkers_CellClick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(142, 17);
            this.label6.TabIndex = 4;
            this.label6.Text = "Список сотрудников";
            // 
            // dgvListGroupsWorker
            // 
            this.dgvListGroupsWorker.AllowUserToResizeColumns = false;
            this.dgvListGroupsWorker.AllowUserToResizeRows = false;
            this.dgvListGroupsWorker.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListGroupsWorker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvListGroupsWorker.Location = new System.Drawing.Point(361, 23);
            this.dgvListGroupsWorker.Name = "dgvListGroupsWorker";
            this.dgvListGroupsWorker.RowHeadersVisible = false;
            this.dgvListGroupsWorker.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListGroupsWorker.Size = new System.Drawing.Size(204, 378);
            this.dgvListGroupsWorker.TabIndex = 1;
            this.dgvListGroupsWorker.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvListGroupsWorker_DataBindingComplete);
            // 
            // dgvGroups
            // 
            this.dgvGroups.AllowUserToResizeColumns = false;
            this.dgvGroups.AllowUserToResizeRows = false;
            this.dgvGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGroups.Location = new System.Drawing.Point(631, 23);
            this.dgvGroups.Name = "dgvGroups";
            this.dgvGroups.RowHeadersVisible = false;
            this.dgvGroups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGroups.Size = new System.Drawing.Size(205, 378);
            this.dgvGroups.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.button2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(571, 23);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(54, 378);
            this.panel3.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 146);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "<--";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 175);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "-->";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(361, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(164, 20);
            this.label7.TabIndex = 4;
            this.label7.Text = "Группы доступные для сотрудника";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(631, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 17);
            this.label8.TabIndex = 4;
            this.label8.Text = "Список групп";
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(758, 3);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(88, 34);
            this.btCancel.TabIndex = 24;
            this.btCancel.Text = "Закрыть";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Enabled = false;
            this.btSave.Location = new System.Drawing.Point(664, 3);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(88, 34);
            this.btSave.TabIndex = 25;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(855, 498);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.btCancel);
            this.flowLayoutPanel1.Controls.Add(this.btSave);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 448);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(849, 47);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // SettingForm
            // 
            this.AcceptButton = this.btSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(855, 498);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDirectory)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.tpPermission.ResumeLayout(false);
            this.tpPermission.PerformLayout();
            this.tpAccessGroups.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListGroupsWorker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroups)).EndInit();
            this.panel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btRemoveUser;
        private System.Windows.Forms.Button btEditUser;
        private System.Windows.Forms.Button btAddUser;
        private System.Windows.Forms.DataGridView dgvDirectory;
        private System.Windows.Forms.ListBox lbCatigory;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.TabPage tpPermission;
        private System.Windows.Forms.CheckBox EditStudent;
        private System.Windows.Forms.CheckBox DetailsStudent;
        private System.Windows.Forms.CheckBox ListStudents;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbRoles;
        private System.Windows.Forms.CheckBox SchedulerPayGroup;
        private System.Windows.Forms.CheckBox Journal;
        private System.Windows.Forms.CheckBox ExclusionFromGroup;
        private System.Windows.Forms.CheckBox EditGroup;
        private System.Windows.Forms.CheckBox EnrollInGroup;
        private System.Windows.Forms.CheckBox DetailsGroup;
        private System.Windows.Forms.CheckBox ListGroups;
        private System.Windows.Forms.CheckBox FinanceReports;
        private System.Windows.Forms.CheckBox ArchiveStudents;
        private System.Windows.Forms.CheckBox Wages;
        private System.Windows.Forms.CheckBox PlanEnroll;
        private System.Windows.Forms.CheckBox ListPayments;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox PrintDoc;
        private System.Windows.Forms.CheckBox Directories;
        private System.Windows.Forms.CheckBox ManageRoles;
        private System.Windows.Forms.CheckBox ManageUsers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox ListLessons;
        private System.Windows.Forms.CheckBox DeleteLesson;
        private System.Windows.Forms.CheckBox AddLesson;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox AccessGroups;
        private System.Windows.Forms.TabPage tpAccessGroups;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView dgvWorkers;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvListGroupsWorker;
        private System.Windows.Forms.DataGridView dgvGroups;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}