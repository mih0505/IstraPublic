namespace Istra
{
    partial class ListReturnPaysForm
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
            this.chbSelecting = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbGroup = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbLastname = new System.Windows.Forms.TextBox();
            this.cbWorkers = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpBegin = new System.Windows.Forms.DateTimePicker();
            this.btCleanFilter = new System.Windows.Forms.Button();
            this.dgvPayments = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayments)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.chbSelecting);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.tbGroup);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.tbLastname);
            this.panel1.Controls.Add(this.cbWorkers);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtpEnd);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtpBegin);
            this.panel1.Controls.Add(this.btCleanFilter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1070, 68);
            this.panel1.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(991, 17);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(79, 39);
            this.toolStrip1.TabIndex = 23;
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
            this.краткийСписокToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.краткийСписокToolStripMenuItem.Text = "Список возвратов";
            this.краткийСписокToolStripMenuItem.Click += new System.EventHandler(this.краткийСписокToolStripMenuItem_Click);
            // 
            // подробныйСписокToolStripMenuItem
            // 
            this.подробныйСписокToolStripMenuItem.Name = "подробныйСписокToolStripMenuItem";
            this.подробныйСписокToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.подробныйСписокToolStripMenuItem.Text = "Квитанции";
            this.подробныйСписокToolStripMenuItem.Visible = false;
            this.подробныйСписокToolStripMenuItem.Click += new System.EventHandler(this.подробныйСписокToolStripMenuItem_Click_1);
            // 
            // chbSelecting
            // 
            this.chbSelecting.AutoSize = true;
            this.chbSelecting.Location = new System.Drawing.Point(27, 37);
            this.chbSelecting.Name = "chbSelecting";
            this.chbSelecting.Size = new System.Drawing.Size(15, 14);
            this.chbSelecting.TabIndex = 22;
            this.chbSelecting.UseVisualStyleBackColor = true;
            this.chbSelecting.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(215, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Группа учащегося";
            // 
            // tbGroup
            // 
            this.tbGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbGroup.Location = new System.Drawing.Point(218, 31);
            this.tbGroup.Name = "tbGroup";
            this.tbGroup.Size = new System.Drawing.Size(125, 24);
            this.tbGroup.TabIndex = 20;
            this.tbGroup.TextChanged += new System.EventHandler(this.tbGroup_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(68, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Фамилия учащегося";
            // 
            // tbLastname
            // 
            this.tbLastname.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbLastname.Location = new System.Drawing.Point(71, 30);
            this.tbLastname.Name = "tbLastname";
            this.tbLastname.Size = new System.Drawing.Size(125, 24);
            this.tbLastname.TabIndex = 18;
            this.tbLastname.TextChanged += new System.EventHandler(this.tbLastname_TextChanged);
            // 
            // cbWorkers
            // 
            this.cbWorkers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbWorkers.FormattingEnabled = true;
            this.cbWorkers.Location = new System.Drawing.Point(705, 31);
            this.cbWorkers.Name = "cbWorkers";
            this.cbWorkers.Size = new System.Drawing.Size(174, 25);
            this.cbWorkers.TabIndex = 17;
            this.cbWorkers.SelectionChangeCommitted += new System.EventHandler(this.cbWorkers_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(702, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Работник, принявший платеж";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(529, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Дата по";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Enabled = false;
            this.dtpEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnd.Location = new System.Drawing.Point(532, 31);
            this.dtpEnd.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.dtpEnd.MinDate = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(149, 24);
            this.dtpEnd.TabIndex = 14;
            this.dtpEnd.Value = new System.DateTime(2018, 2, 26, 1, 6, 0, 0);
            this.dtpEnd.ValueChanged += new System.EventHandler(this.dtpEnd_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(359, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Дата с";
            // 
            // dtpBegin
            // 
            this.dtpBegin.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpBegin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBegin.Location = new System.Drawing.Point(362, 31);
            this.dtpBegin.Margin = new System.Windows.Forms.Padding(4);
            this.dtpBegin.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.dtpBegin.MinDate = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            this.dtpBegin.Name = "dtpBegin";
            this.dtpBegin.Size = new System.Drawing.Size(149, 24);
            this.dtpBegin.TabIndex = 12;
            this.dtpBegin.Value = new System.DateTime(2018, 2, 26, 1, 6, 0, 0);
            this.dtpBegin.ValueChanged += new System.EventHandler(this.dtpBegin_ValueChanged);
            // 
            // btCleanFilter
            // 
            this.btCleanFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCleanFilter.Location = new System.Drawing.Point(885, 27);
            this.btCleanFilter.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.btCleanFilter.Name = "btCleanFilter";
            this.btCleanFilter.Size = new System.Drawing.Size(82, 30);
            this.btCleanFilter.TabIndex = 3;
            this.btCleanFilter.Text = "Очистить";
            this.btCleanFilter.UseVisualStyleBackColor = true;
            this.btCleanFilter.Click += new System.EventHandler(this.btCleanFilter_Click);
            // 
            // dgvPayments
            // 
            this.dgvPayments.AllowUserToAddRows = false;
            this.dgvPayments.AllowUserToDeleteRows = false;
            this.dgvPayments.AllowUserToResizeRows = false;
            this.dgvPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPayments.Location = new System.Drawing.Point(0, 68);
            this.dgvPayments.MultiSelect = false;
            this.dgvPayments.Name = "dgvPayments";
            this.dgvPayments.RowHeadersVisible = false;
            this.dgvPayments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPayments.Size = new System.Drawing.Size(1070, 330);
            this.dgvPayments.TabIndex = 2;
            // 
            // ListReturnPaysForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 398);
            this.Controls.Add(this.dgvPayments);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(1086, 436);
            this.Name = "ListReturnPaysForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Возврат платежей";
            this.Load += new System.EventHandler(this.ListPaymentsForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btCleanFilter;
        private System.Windows.Forms.ComboBox cbWorkers;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpBegin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbLastname;
        public System.Windows.Forms.DataGridView dgvPayments;
        private System.Windows.Forms.CheckBox chbSelecting;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem краткийСписокToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem подробныйСписокToolStripMenuItem;
    }
}