namespace Istra
{
    partial class RetentionForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cbTeachers = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.rbProfit = new System.Windows.Forms.RadioButton();
            this.rbRetention = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbBase = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.rbPay = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpMonth = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(189, 271);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(297, 271);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 30);
            this.button2.TabIndex = 1;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cbTeachers
            // 
            this.cbTeachers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTeachers.FormattingEnabled = true;
            this.cbTeachers.Location = new System.Drawing.Point(156, 94);
            this.cbTeachers.Margin = new System.Windows.Forms.Padding(4);
            this.cbTeachers.Name = "cbTeachers";
            this.cbTeachers.Size = new System.Drawing.Size(234, 25);
            this.cbTeachers.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Сотрудник";
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(156, 19);
            this.dtpDate.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.dtpDate.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(160, 23);
            this.dtpDate.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Дата операции";
            // 
            // rbProfit
            // 
            this.rbProfit.AutoSize = true;
            this.rbProfit.Location = new System.Drawing.Point(233, 135);
            this.rbProfit.Name = "rbProfit";
            this.rbProfit.Size = new System.Drawing.Size(79, 21);
            this.rbProfit.TabIndex = 5;
            this.rbProfit.Text = "Начисл.";
            this.rbProfit.UseVisualStyleBackColor = true;
            // 
            // rbRetention
            // 
            this.rbRetention.AutoSize = true;
            this.rbRetention.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rbRetention.Checked = true;
            this.rbRetention.Location = new System.Drawing.Point(156, 135);
            this.rbRetention.Name = "rbRetention";
            this.rbRetention.Size = new System.Drawing.Size(72, 21);
            this.rbRetention.TabIndex = 5;
            this.rbRetention.TabStop = true;
            this.rbRetention.Text = "Удерж.";
            this.rbRetention.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Операция";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Основание";
            // 
            // cbBase
            // 
            this.cbBase.FormattingEnabled = true;
            this.cbBase.Location = new System.Drawing.Point(156, 172);
            this.cbBase.Margin = new System.Windows.Forms.Padding(4);
            this.cbBase.Name = "cbBase";
            this.cbBase.Size = new System.Drawing.Size(234, 25);
            this.cbBase.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "Сумма";
            // 
            // tbValue
            // 
            this.tbValue.Location = new System.Drawing.Point(156, 215);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(160, 23);
            this.tbValue.TabIndex = 6;
            this.tbValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(322, 218);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 17);
            this.label6.TabIndex = 7;
            this.label6.Text = "руб.";
            // 
            // rbPay
            // 
            this.rbPay.AutoSize = true;
            this.rbPay.Location = new System.Drawing.Point(313, 135);
            this.rbPay.Name = "rbPay";
            this.rbPay.Size = new System.Drawing.Size(84, 21);
            this.rbPay.TabIndex = 5;
            this.rbPay.Text = "Выплата";
            this.rbPay.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 17);
            this.label7.TabIndex = 3;
            this.label7.Text = "Месяц";
            // 
            // dtpMonth
            // 
            this.dtpMonth.CustomFormat = "MMMM yyyy";
            this.dtpMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMonth.Location = new System.Drawing.Point(156, 57);
            this.dtpMonth.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.dtpMonth.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.dtpMonth.Name = "dtpMonth";
            this.dtpMonth.Size = new System.Drawing.Size(160, 23);
            this.dtpMonth.TabIndex = 8;
            // 
            // RetentionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 313);
            this.Controls.Add(this.dtpMonth);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbValue);
            this.Controls.Add(this.rbRetention);
            this.Controls.Add(this.rbPay);
            this.Controls.Add(this.rbProfit);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbBase);
            this.Controls.Add(this.cbTeachers);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RetentionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Начисления/Удержания";
            this.Load += new System.EventHandler(this.RetentionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox cbTeachers;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbProfit;
        private System.Windows.Forms.RadioButton rbRetention;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbBase;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton rbPay;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpMonth;
    }
}