namespace Istra
{
    partial class ListStudentsForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbTypeGroup = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.экспортВExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.подробныйСписокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbStatuses = new System.Windows.Forms.ComboBox();
            this.cbSchools = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbActivityGroup = new System.Windows.Forms.ComboBox();
            this.cbShifts = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbClasses = new System.Windows.Forms.ComboBox();
            this.btCleanFilter = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLastname = new System.Windows.Forms.TextBox();
            this.cbGroups = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvStudents = new System.Windows.Forms.DataGridView();
            this.tbPhoneNumber = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbPhoneNumber);
            this.panel1.Controls.Add(this.cbTypeGroup);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.cbStatuses);
            this.panel1.Controls.Add(this.cbSchools);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cbActivityGroup);
            this.panel1.Controls.Add(this.cbShifts);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbClasses);
            this.panel1.Controls.Add(this.btCleanFilter);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tbLastname);
            this.panel1.Controls.Add(this.cbGroups);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1054, 68);
            this.panel1.TabIndex = 0;
            // 
            // cbTypeGroup
            // 
            this.cbTypeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTypeGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbTypeGroup.FormattingEnabled = true;
            this.cbTypeGroup.Items.AddRange(new object[] {
            "Учебный год",
            "Экспресс",
            "Индивидуально"});
            this.cbTypeGroup.Location = new System.Drawing.Point(381, 31);
            this.cbTypeGroup.Name = "cbTypeGroup";
            this.cbTypeGroup.Size = new System.Drawing.Size(103, 25);
            this.cbTypeGroup.TabIndex = 13;
            this.cbTypeGroup.SelectionChangeCommitted += new System.EventHandler(this.cbTypeGroup_SelectionChangeCommitted);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(378, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Форма\\Год";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(975, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(79, 39);
            this.toolStrip1.TabIndex = 11;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.экспортВExcelToolStripMenuItem,
            this.подробныйСписокToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::Istra.Properties.Resources.Excel;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(45, 36);
            // 
            // экспортВExcelToolStripMenuItem
            // 
            this.экспортВExcelToolStripMenuItem.Name = "экспортВExcelToolStripMenuItem";
            this.экспортВExcelToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.экспортВExcelToolStripMenuItem.Text = "Краткий список";
            this.экспортВExcelToolStripMenuItem.Click += new System.EventHandler(this.btExportExcel_Click);
            // 
            // подробныйСписокToolStripMenuItem
            // 
            this.подробныйСписокToolStripMenuItem.Name = "подробныйСписокToolStripMenuItem";
            this.подробныйСписокToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.подробныйСписокToolStripMenuItem.Text = "Подробный список";
            this.подробныйСписокToolStripMenuItem.Click += new System.EventHandler(this.подробныйСписокToolStripMenuItem_Click);
            // 
            // cbStatuses
            // 
            this.cbStatuses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatuses.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbStatuses.FormattingEnabled = true;
            this.cbStatuses.Location = new System.Drawing.Point(490, 31);
            this.cbStatuses.Name = "cbStatuses";
            this.cbStatuses.Size = new System.Drawing.Size(95, 25);
            this.cbStatuses.TabIndex = 3;
            this.cbStatuses.SelectionChangeCommitted += new System.EventHandler(this.cbStatuses_SelectionChangeCommitted);
            // 
            // cbSchools
            // 
            this.cbSchools.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSchools.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSchools.FormattingEnabled = true;
            this.cbSchools.Location = new System.Drawing.Point(591, 31);
            this.cbSchools.Name = "cbSchools";
            this.cbSchools.Size = new System.Drawing.Size(102, 25);
            this.cbSchools.TabIndex = 4;
            this.cbSchools.SelectionChangeCommitted += new System.EventHandler(this.cbSchools_SelectionChangeCommitted);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(802, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Статус группы";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(754, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Смена";
            // 
            // cbActivityGroup
            // 
            this.cbActivityGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbActivityGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbActivityGroup.FormattingEnabled = true;
            this.cbActivityGroup.Location = new System.Drawing.Point(799, 31);
            this.cbActivityGroup.Name = "cbActivityGroup";
            this.cbActivityGroup.Size = new System.Drawing.Size(97, 25);
            this.cbActivityGroup.TabIndex = 7;
            this.cbActivityGroup.SelectionChangeCommitted += new System.EventHandler(this.cbShifts_SelectionChangeCommitted);
            // 
            // cbShifts
            // 
            this.cbShifts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShifts.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbShifts.FormattingEnabled = true;
            this.cbShifts.Items.AddRange(new object[] {
            "I",
            "II"});
            this.cbShifts.Location = new System.Drawing.Point(757, 31);
            this.cbShifts.Name = "cbShifts";
            this.cbShifts.Size = new System.Drawing.Size(37, 25);
            this.cbShifts.TabIndex = 6;
            this.cbShifts.SelectionChangeCommitted += new System.EventHandler(this.cbShifts_SelectionChangeCommitted);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(588, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Школа";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(696, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Класс";
            // 
            // cbClasses
            // 
            this.cbClasses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClasses.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbClasses.FormattingEnabled = true;
            this.cbClasses.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11"});
            this.cbClasses.Location = new System.Drawing.Point(699, 31);
            this.cbClasses.Name = "cbClasses";
            this.cbClasses.Size = new System.Drawing.Size(53, 25);
            this.cbClasses.TabIndex = 5;
            this.cbClasses.SelectionChangeCommitted += new System.EventHandler(this.cbClasses_SelectionChangeCommitted);
            // 
            // btCleanFilter
            // 
            this.btCleanFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCleanFilter.Location = new System.Drawing.Point(903, 28);
            this.btCleanFilter.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.btCleanFilter.Name = "btCleanFilter";
            this.btCleanFilter.Size = new System.Drawing.Size(82, 30);
            this.btCleanFilter.TabIndex = 8;
            this.btCleanFilter.Text = "Очистить";
            this.btCleanFilter.UseVisualStyleBackColor = true;
            this.btCleanFilter.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(487, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Статус слушателя";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(280, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Группа";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(159, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Телефон";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Фамилия";
            // 
            // tbLastname
            // 
            this.tbLastname.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbLastname.Location = new System.Drawing.Point(10, 32);
            this.tbLastname.Name = "tbLastname";
            this.tbLastname.Size = new System.Drawing.Size(144, 23);
            this.tbLastname.TabIndex = 1;
            this.tbLastname.TextChanged += new System.EventHandler(this.tbLastname_TextChanged);
            // 
            // cbGroups
            // 
            this.cbGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbGroups.FormattingEnabled = true;
            this.cbGroups.Location = new System.Drawing.Point(283, 31);
            this.cbGroups.Name = "cbGroups";
            this.cbGroups.Size = new System.Drawing.Size(92, 25);
            this.cbGroups.TabIndex = 2;
            this.cbGroups.SelectionChangeCommitted += new System.EventHandler(this.cbGroups_SelectionChangeCommitted);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvStudents);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 68);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1054, 396);
            this.panel2.TabIndex = 1;
            // 
            // dgvStudents
            // 
            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.AllowUserToDeleteRows = false;
            this.dgvStudents.AllowUserToResizeRows = false;
            this.dgvStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStudents.Location = new System.Drawing.Point(0, 0);
            this.dgvStudents.MultiSelect = false;
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.ReadOnly = true;
            this.dgvStudents.RowHeadersVisible = false;
            this.dgvStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudents.Size = new System.Drawing.Size(1054, 396);
            this.dgvStudents.TabIndex = 0;
            this.dgvStudents.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvStudents_CellMouseDoubleClick);
            this.dgvStudents.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvStudents_ColumnHeaderMouseClick);
            // 
            // tbPhoneNumber
            // 
            this.tbPhoneNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbPhoneNumber.Location = new System.Drawing.Point(160, 32);
            this.tbPhoneNumber.Name = "tbPhoneNumber";
            this.tbPhoneNumber.Size = new System.Drawing.Size(117, 23);
            this.tbPhoneNumber.TabIndex = 14;
            this.tbPhoneNumber.TextChanged += new System.EventHandler(this.tbLastname_TextChanged);
            // 
            // ListStudentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1054, 464);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(1070, 390);
            this.Name = "ListStudentsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Список учащихся";
            this.Activated += new System.EventHandler(this.ListStudentsForm_Activated);
            this.Load += new System.EventHandler(this.StudentsForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvStudents;
        private System.Windows.Forms.TextBox tbLastname;
        private System.Windows.Forms.ComboBox cbGroups;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btCleanFilter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbShifts;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbClasses;
        private System.Windows.Forms.ComboBox cbSchools;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbStatuses;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbActivityGroup;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem экспортВExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem подробныйСписокToolStripMenuItem;
        private System.Windows.Forms.ComboBox cbTypeGroup;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbPhoneNumber;
    }
}