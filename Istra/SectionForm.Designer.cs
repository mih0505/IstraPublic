namespace Istra
{
    partial class SectionForm
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
            this.tbName = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.cbLesson = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.tbDuration = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.cbCredit = new System.Windows.Forms.CheckBox();
            this.cbTypeGrade = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbName.Location = new System.Drawing.Point(12, 101);
            this.tbName.Margin = new System.Windows.Forms.Padding(4);
            this.tbName.Multiline = true;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(343, 89);
            this.tbName.TabIndex = 44;
            this.tbName.TextChanged += new System.EventHandler(this.IsDataChange);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(9, 79);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(141, 18);
            this.label25.TabIndex = 43;
            this.label25.Text = "Название раздела:";
            // 
            // cbLesson
            // 
            this.cbLesson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLesson.FormatString = "dd MMMM yyyy";
            this.cbLesson.FormattingEnabled = true;
            this.cbLesson.Location = new System.Drawing.Point(12, 292);
            this.cbLesson.Margin = new System.Windows.Forms.Padding(4);
            this.cbLesson.Name = "cbLesson";
            this.cbLesson.Size = new System.Drawing.Size(343, 26);
            this.cbLesson.TabIndex = 42;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(9, 270);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(138, 18);
            this.label23.TabIndex = 41;
            this.label23.Text = "Отчетное занятие:";
            // 
            // tbDuration
            // 
            this.tbDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbDuration.Location = new System.Drawing.Point(12, 227);
            this.tbDuration.Margin = new System.Windows.Forms.Padding(4);
            this.tbDuration.Name = "tbDuration";
            this.tbDuration.Size = new System.Drawing.Size(343, 24);
            this.tbDuration.TabIndex = 46;
            this.tbDuration.TextChanged += new System.EventHandler(this.IsDataChange);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 205);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 18);
            this.label1.TabIndex = 45;
            this.label1.Text = "Продолжительность:";
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCancel.Location = new System.Drawing.Point(281, 339);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(74, 33);
            this.btCancel.TabIndex = 47;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Enabled = false;
            this.btOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btOK.Location = new System.Drawing.Point(201, 339);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(74, 33);
            this.btOK.TabIndex = 48;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // cbCredit
            // 
            this.cbCredit.AutoSize = true;
            this.cbCredit.Location = new System.Drawing.Point(12, 12);
            this.cbCredit.Name = "cbCredit";
            this.cbCredit.Size = new System.Drawing.Size(68, 22);
            this.cbCredit.TabIndex = 49;
            this.cbCredit.Text = "Зачет";
            this.cbCredit.UseVisualStyleBackColor = true;
            // 
            // cbTypeGrade
            // 
            this.cbTypeGrade.AutoSize = true;
            this.cbTypeGrade.Checked = true;
            this.cbTypeGrade.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTypeGrade.Location = new System.Drawing.Point(12, 40);
            this.cbTypeGrade.Name = "cbTypeGrade";
            this.cbTypeGrade.Size = new System.Drawing.Size(313, 22);
            this.cbTypeGrade.TabIndex = 50;
            this.cbTypeGrade.Text = "Указывать оценку при печати протокола";
            this.cbTypeGrade.UseVisualStyleBackColor = true;
            // 
            // SectionForm
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(369, 384);
            this.Controls.Add(this.cbTypeGrade);
            this.Controls.Add(this.cbCredit);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.tbDuration);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.cbLesson);
            this.Controls.Add(this.label23);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Раздел";
            this.Load += new System.EventHandler(this.SectionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.ComboBox cbLesson;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tbDuration;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.CheckBox cbCredit;
        private System.Windows.Forms.CheckBox cbTypeGrade;
    }
}