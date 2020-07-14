namespace Istra
{
    partial class PrintGroupForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.chkbJournal = new System.Windows.Forms.CheckBox();
            this.chkbStudents = new System.Windows.Forms.CheckBox();
            this.chkbContacts = new System.Windows.Forms.CheckBox();
            this.chkbExam = new System.Windows.Forms.CheckBox();
            this.chkbDocument = new System.Windows.Forms.CheckBox();
            this.chkbListTopic = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Какую информацию напечатать?";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(167, 224);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 32);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(248, 224);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 32);
            this.button2.TabIndex = 1;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chkbJournal
            // 
            this.chkbJournal.AutoSize = true;
            this.chkbJournal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkbJournal.Location = new System.Drawing.Point(36, 48);
            this.chkbJournal.Name = "chkbJournal";
            this.chkbJournal.Size = new System.Drawing.Size(175, 21);
            this.chkbJournal.TabIndex = 2;
            this.chkbJournal.Text = "Журнал успеваемости";
            this.chkbJournal.UseVisualStyleBackColor = true;
            this.chkbJournal.CheckedChanged += new System.EventHandler(this.chkbJournal_CheckedChanged);
            // 
            // chkbStudents
            // 
            this.chkbStudents.AutoSize = true;
            this.chkbStudents.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkbStudents.Location = new System.Drawing.Point(36, 102);
            this.chkbStudents.Name = "chkbStudents";
            this.chkbStudents.Size = new System.Drawing.Size(200, 21);
            this.chkbStudents.TabIndex = 2;
            this.chkbStudents.Text = "Информация об учащихся";
            this.chkbStudents.UseVisualStyleBackColor = true;
            // 
            // chkbContacts
            // 
            this.chkbContacts.AutoSize = true;
            this.chkbContacts.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkbContacts.Location = new System.Drawing.Point(36, 129);
            this.chkbContacts.Name = "chkbContacts";
            this.chkbContacts.Size = new System.Drawing.Size(161, 21);
            this.chkbContacts.TabIndex = 2;
            this.chkbContacts.Text = "Контактные данные";
            this.chkbContacts.UseVisualStyleBackColor = true;
            // 
            // chkbExam
            // 
            this.chkbExam.AutoSize = true;
            this.chkbExam.Enabled = false;
            this.chkbExam.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkbExam.Location = new System.Drawing.Point(36, 156);
            this.chkbExam.Name = "chkbExam";
            this.chkbExam.Size = new System.Drawing.Size(157, 21);
            this.chkbExam.TabIndex = 2;
            this.chkbExam.Text = "Протокол экзамена";
            this.chkbExam.UseVisualStyleBackColor = true;
            this.chkbExam.Visible = false;
            // 
            // chkbDocument
            // 
            this.chkbDocument.AutoSize = true;
            this.chkbDocument.Enabled = false;
            this.chkbDocument.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkbDocument.Location = new System.Drawing.Point(36, 183);
            this.chkbDocument.Name = "chkbDocument";
            this.chkbDocument.Size = new System.Drawing.Size(189, 21);
            this.chkbDocument.TabIndex = 2;
            this.chkbDocument.Text = "Документы об обучении";
            this.chkbDocument.UseVisualStyleBackColor = true;
            this.chkbDocument.Visible = false;
            // 
            // chkbListTopic
            // 
            this.chkbListTopic.AutoSize = true;
            this.chkbListTopic.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkbListTopic.Location = new System.Drawing.Point(36, 75);
            this.chkbListTopic.Name = "chkbListTopic";
            this.chkbListTopic.Size = new System.Drawing.Size(102, 21);
            this.chkbListTopic.TabIndex = 2;
            this.chkbListTopic.Text = "Список тем";
            this.chkbListTopic.UseVisualStyleBackColor = true;
            // 
            // PrintGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(339, 268);
            this.Controls.Add(this.chkbDocument);
            this.Controls.Add(this.chkbExam);
            this.Controls.Add(this.chkbContacts);
            this.Controls.Add(this.chkbListTopic);
            this.Controls.Add(this.chkbStudents);
            this.Controls.Add(this.chkbJournal);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PrintGroupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Печать";
            this.Load += new System.EventHandler(this.PrintGroupForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox chkbJournal;
        private System.Windows.Forms.CheckBox chkbStudents;
        private System.Windows.Forms.CheckBox chkbContacts;
        private System.Windows.Forms.CheckBox chkbExam;
        private System.Windows.Forms.CheckBox chkbDocument;
        private System.Windows.Forms.CheckBox chkbListTopic;
    }
}