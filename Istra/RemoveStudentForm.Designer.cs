namespace Istra
{
    partial class RemoveStudentForm
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
            this.cbList = new System.Windows.Forms.ComboBox();
            this.rbExclude = new System.Windows.Forms.RadioButton();
            this.rbTransfer = new System.Windows.Forms.RadioButton();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.cbGroups = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbMonths = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chbSaveGrades = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbList
            // 
            this.cbList.FormattingEnabled = true;
            this.cbList.Location = new System.Drawing.Point(16, 142);
            this.cbList.Margin = new System.Windows.Forms.Padding(4);
            this.cbList.Name = "cbList";
            this.cbList.Size = new System.Drawing.Size(431, 25);
            this.cbList.TabIndex = 0;
            // 
            // rbExclude
            // 
            this.rbExclude.AutoSize = true;
            this.rbExclude.Checked = true;
            this.rbExclude.Location = new System.Drawing.Point(16, 87);
            this.rbExclude.Name = "rbExclude";
            this.rbExclude.Size = new System.Drawing.Size(197, 21);
            this.rbExclude.TabIndex = 1;
            this.rbExclude.TabStop = true;
            this.rbExclude.Text = "Отчислить обучающегося";
            this.rbExclude.UseVisualStyleBackColor = true;
            this.rbExclude.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // rbTransfer
            // 
            this.rbTransfer.AutoSize = true;
            this.rbTransfer.Location = new System.Drawing.Point(16, 12);
            this.rbTransfer.Name = "rbTransfer";
            this.rbTransfer.Size = new System.Drawing.Size(204, 21);
            this.rbTransfer.TabIndex = 1;
            this.rbTransfer.Text = "Перевести в другую группу";
            this.rbTransfer.UseVisualStyleBackColor = true;
            this.rbTransfer.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(296, 219);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 35);
            this.btOK.TabIndex = 2;
            this.btOK.Text = "ОК";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(377, 219);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 35);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // cbGroups
            // 
            this.cbGroups.Enabled = false;
            this.cbGroups.FormattingEnabled = true;
            this.cbGroups.Location = new System.Drawing.Point(226, 11);
            this.cbGroups.Name = "cbGroups";
            this.cbGroups.Size = new System.Drawing.Size(218, 25);
            this.cbGroups.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Причина отчисления";
            // 
            // cbMonths
            // 
            this.cbMonths.FormattingEnabled = true;
            this.cbMonths.Location = new System.Drawing.Point(151, 184);
            this.cbMonths.Name = "cbMonths";
            this.cbMonths.Size = new System.Drawing.Size(121, 25);
            this.cbMonths.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Месяц отчисления";
            // 
            // chbSaveGrades
            // 
            this.chbSaveGrades.AutoSize = true;
            this.chbSaveGrades.Checked = true;
            this.chbSaveGrades.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbSaveGrades.Enabled = false;
            this.chbSaveGrades.Location = new System.Drawing.Point(224, 49);
            this.chbSaveGrades.Name = "chbSaveGrades";
            this.chbSaveGrades.Size = new System.Drawing.Size(147, 21);
            this.chbSaveGrades.TabIndex = 9;
            this.chbSaveGrades.Text = "Сохранить оценки";
            this.chbSaveGrades.UseVisualStyleBackColor = true;
            // 
            // RemoveStudentForm
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(459, 266);
            this.Controls.Add(this.chbSaveGrades);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbMonths);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbGroups);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.rbTransfer);
            this.Controls.Add(this.rbExclude);
            this.Controls.Add(this.cbList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RemoveStudentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Отчисление/перевод учащегося из группы";
            this.Load += new System.EventHandler(this.RemoveStudentForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbList;
        private System.Windows.Forms.RadioButton rbExclude;
        private System.Windows.Forms.RadioButton rbTransfer;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.ComboBox cbGroups;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbMonths;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chbSaveGrades;
    }
}