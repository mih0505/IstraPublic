namespace Istra
{
    partial class ListGroupsForm
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.краткийСписокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.подробныйСписокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbActivity = new System.Windows.Forms.ComboBox();
            this.cbTeachers = new System.Windows.Forms.ComboBox();
            this.btCleanFilter = new System.Windows.Forms.Button();
            this.cbTypeGroup = new System.Windows.Forms.ComboBox();
            this.tbNameGroup = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDirectionOfTraining = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvListGroups = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListGroups)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.cbActivity);
            this.panel1.Controls.Add(this.cbTeachers);
            this.panel1.Controls.Add(this.btCleanFilter);
            this.panel1.Controls.Add(this.cbTypeGroup);
            this.panel1.Controls.Add(this.tbNameGroup);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbDirectionOfTraining);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(923, 70);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(844, 20);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(79, 39);
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.краткийСписокToolStripMenuItem,
            this.подробныйСписокToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::Istra.Properties.Resources.Excel;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(45, 36);
            // 
            // краткийСписокToolStripMenuItem
            // 
            this.краткийСписокToolStripMenuItem.Name = "краткийСписокToolStripMenuItem";
            this.краткийСписокToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.краткийСписокToolStripMenuItem.Text = "Краткий список";
            this.краткийСписокToolStripMenuItem.Click += new System.EventHandler(this.btExportExcel_Click);
            // 
            // подробныйСписокToolStripMenuItem
            // 
            this.подробныйСписокToolStripMenuItem.Name = "подробныйСписокToolStripMenuItem";
            this.подробныйСписокToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.подробныйСписокToolStripMenuItem.Text = "Подробный список";
            this.подробныйСписокToolStripMenuItem.Click += new System.EventHandler(this.подробныйСписокToolStripMenuItem_Click);
            // 
            // cbActivity
            // 
            this.cbActivity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbActivity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbActivity.FormattingEnabled = true;
            this.cbActivity.Location = new System.Drawing.Point(617, 34);
            this.cbActivity.Name = "cbActivity";
            this.cbActivity.Size = new System.Drawing.Size(107, 25);
            this.cbActivity.TabIndex = 13;
            this.cbActivity.SelectionChangeCommitted += new System.EventHandler(this.cbTeachers_SelectionChangeCommitted);
            // 
            // cbTeachers
            // 
            this.cbTeachers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTeachers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbTeachers.FormattingEnabled = true;
            this.cbTeachers.Location = new System.Drawing.Point(446, 34);
            this.cbTeachers.Name = "cbTeachers";
            this.cbTeachers.Size = new System.Drawing.Size(160, 25);
            this.cbTeachers.TabIndex = 13;
            this.cbTeachers.SelectionChangeCommitted += new System.EventHandler(this.cbTeachers_SelectionChangeCommitted);
            // 
            // btCleanFilter
            // 
            this.btCleanFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCleanFilter.Location = new System.Drawing.Point(738, 31);
            this.btCleanFilter.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.btCleanFilter.Name = "btCleanFilter";
            this.btCleanFilter.Size = new System.Drawing.Size(82, 30);
            this.btCleanFilter.TabIndex = 9;
            this.btCleanFilter.Text = "Очистить";
            this.btCleanFilter.UseVisualStyleBackColor = true;
            this.btCleanFilter.Click += new System.EventHandler(this.btCleanFilter_Click);
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
            this.cbTypeGroup.Location = new System.Drawing.Point(315, 34);
            this.cbTypeGroup.Name = "cbTypeGroup";
            this.cbTypeGroup.Size = new System.Drawing.Size(121, 25);
            this.cbTypeGroup.TabIndex = 5;
            this.cbTypeGroup.SelectionChangeCommitted += new System.EventHandler(this.cbTypeGroup_SelectionChangeCommitted);
            // 
            // tbNameGroup
            // 
            this.tbNameGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNameGroup.Location = new System.Drawing.Point(178, 34);
            this.tbNameGroup.Name = "tbNameGroup";
            this.tbNameGroup.Size = new System.Drawing.Size(128, 23);
            this.tbNameGroup.TabIndex = 4;
            this.tbNameGroup.TextChanged += new System.EventHandler(this.tbNameGroup_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(614, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Статус";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(443, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Преподаватель";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(312, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Вид обучения";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Название";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Направление";
            // 
            // cbDirectionOfTraining
            // 
            this.cbDirectionOfTraining.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirectionOfTraining.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbDirectionOfTraining.FormattingEnabled = true;
            this.cbDirectionOfTraining.Location = new System.Drawing.Point(15, 34);
            this.cbDirectionOfTraining.Name = "cbDirectionOfTraining";
            this.cbDirectionOfTraining.Size = new System.Drawing.Size(154, 25);
            this.cbDirectionOfTraining.TabIndex = 0;
            this.cbDirectionOfTraining.SelectionChangeCommitted += new System.EventHandler(this.cbDirectionOfTraining_SelectionChangeCommitted);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvListGroups);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(923, 360);
            this.panel2.TabIndex = 1;
            // 
            // dgvListGroups
            // 
            this.dgvListGroups.AllowUserToAddRows = false;
            this.dgvListGroups.AllowUserToDeleteRows = false;
            this.dgvListGroups.AllowUserToResizeRows = false;
            this.dgvListGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvListGroups.Location = new System.Drawing.Point(0, 0);
            this.dgvListGroups.MultiSelect = false;
            this.dgvListGroups.Name = "dgvListGroups";
            this.dgvListGroups.ReadOnly = true;
            this.dgvListGroups.RowHeadersVisible = false;
            this.dgvListGroups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListGroups.Size = new System.Drawing.Size(923, 360);
            this.dgvListGroups.TabIndex = 0;
            this.dgvListGroups.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvListGroups_CellMouseDoubleClick);
            this.dgvListGroups.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvListGroups_ColumnHeaderMouseClick);
            // 
            // ListGroupsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 430);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ListGroupsForm";
            this.Text = "Список групп";
            this.Load += new System.EventHandler(this.ListGroupsForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListGroups)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvListGroups;
        private System.Windows.Forms.ComboBox cbDirectionOfTraining;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbTypeGroup;
        private System.Windows.Forms.TextBox tbNameGroup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btCleanFilter;
        private System.Windows.Forms.ComboBox cbTeachers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbActivity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem краткийСписокToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem подробныйСписокToolStripMenuItem;
    }
}