namespace Istra
{
    partial class LessonForm
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
            this.nudDurationLesson = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpDateLesson = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cbCurrentTeacher = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbCurrentHousing = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbCurrentClass = new System.Windows.Forms.ComboBox();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tbCurrentTopic = new System.Windows.Forms.TextBox();
            this.lbTopics = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudDurationLesson)).BeginInit();
            this.SuspendLayout();
            // 
            // nudDurationLesson
            // 
            this.nudDurationLesson.Location = new System.Drawing.Point(195, 87);
            this.nudDurationLesson.Margin = new System.Windows.Forms.Padding(4);
            this.nudDurationLesson.Name = "nudDurationLesson";
            this.nudDurationLesson.Size = new System.Drawing.Size(91, 23);
            this.nudDurationLesson.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Продолжительность";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Дата занятия";
            // 
            // dtpDateLesson
            // 
            this.dtpDateLesson.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateLesson.Location = new System.Drawing.Point(163, 42);
            this.dtpDateLesson.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.dtpDateLesson.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.dtpDateLesson.Name = "dtpDateLesson";
            this.dtpDateLesson.Size = new System.Drawing.Size(123, 23);
            this.dtpDateLesson.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Преподаватель";
            // 
            // cbCurrentTeacher
            // 
            this.cbCurrentTeacher.FormattingEnabled = true;
            this.cbCurrentTeacher.Location = new System.Drawing.Point(129, 133);
            this.cbCurrentTeacher.Name = "cbCurrentTeacher";
            this.cbCurrentTeacher.Size = new System.Drawing.Size(157, 25);
            this.cbCurrentTeacher.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 183);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Корпус";
            // 
            // cbCurrentHousing
            // 
            this.cbCurrentHousing.FormattingEnabled = true;
            this.cbCurrentHousing.Location = new System.Drawing.Point(129, 180);
            this.cbCurrentHousing.Name = "cbCurrentHousing";
            this.cbCurrentHousing.Size = new System.Drawing.Size(157, 25);
            this.cbCurrentHousing.TabIndex = 5;
            this.cbCurrentHousing.SelectionChangeCommitted += new System.EventHandler(this.cbCurrentHousing_SelectionChangeCommitted);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 232);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "Класс";
            // 
            // cbCurrentClass
            // 
            this.cbCurrentClass.FormattingEnabled = true;
            this.cbCurrentClass.Location = new System.Drawing.Point(129, 229);
            this.cbCurrentClass.Name = "cbCurrentClass";
            this.cbCurrentClass.Size = new System.Drawing.Size(157, 25);
            this.cbCurrentClass.TabIndex = 6;
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(600, 281);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 32);
            this.btOK.TabIndex = 4;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(681, 281);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 32);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(333, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "Тема";
            // 
            // tbCurrentTopic
            // 
            this.tbCurrentTopic.Location = new System.Drawing.Point(336, 42);
            this.tbCurrentTopic.Name = "tbCurrentTopic";
            this.tbCurrentTopic.Size = new System.Drawing.Size(420, 23);
            this.tbCurrentTopic.TabIndex = 0;
            this.tbCurrentTopic.TextChanged += new System.EventHandler(this.tbCurrentTopic_TextChanged);
            // 
            // lbTopics
            // 
            this.lbTopics.FormattingEnabled = true;
            this.lbTopics.ItemHeight = 17;
            this.lbTopics.Location = new System.Drawing.Point(336, 71);
            this.lbTopics.Name = "lbTopics";
            this.lbTopics.Size = new System.Drawing.Size(420, 191);
            this.lbTopics.TabIndex = 1;
            this.lbTopics.DoubleClick += new System.EventHandler(this.lbTopics_DoubleClick);
            // 
            // LessonForm
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(768, 330);
            this.Controls.Add(this.lbTopics);
            this.Controls.Add(this.tbCurrentTopic);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.cbCurrentClass);
            this.Controls.Add(this.cbCurrentHousing);
            this.Controls.Add(this.cbCurrentTeacher);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtpDateLesson);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudDurationLesson);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(784, 369);
            this.MinimizeBox = false;
            this.Name = "LessonForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Занятие №";
            this.Load += new System.EventHandler(this.LessonForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudDurationLesson)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudDurationLesson;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpDateLesson;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbCurrentTeacher;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbCurrentHousing;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbCurrentClass;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbCurrentTopic;
        private System.Windows.Forms.ListBox lbTopics;
    }
}