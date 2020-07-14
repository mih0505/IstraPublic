namespace Istra
{
    partial class PayForm
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
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.tbPay = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbMonth = new System.Windows.Forms.ComboBox();
            this.nudYear = new System.Windows.Forms.NumericUpDown();
            this.dtpDatePay = new System.Windows.Forms.DateTimePicker();
            this.cbTypePayment = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbNotePay = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chbAdditionalPay = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudYear)).BeginInit();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(172, 314);
            this.btCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(89, 33);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(75, 314);
            this.btOK.Margin = new System.Windows.Forms.Padding(4);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(89, 33);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "Сохранить";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // tbPay
            // 
            this.tbPay.Location = new System.Drawing.Point(12, 169);
            this.tbPay.Name = "tbPay";
            this.tbPay.Size = new System.Drawing.Size(246, 23);
            this.tbPay.TabIndex = 0;
            this.tbPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Сумма платежа";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Месяц";
            // 
            // cbMonth
            // 
            this.cbMonth.FormattingEnabled = true;
            this.cbMonth.Location = new System.Drawing.Point(14, 63);
            this.cbMonth.Name = "cbMonth";
            this.cbMonth.Size = new System.Drawing.Size(138, 25);
            this.cbMonth.TabIndex = 3;
            // 
            // nudYear
            // 
            this.nudYear.Location = new System.Drawing.Point(158, 63);
            this.nudYear.Maximum = new decimal(new int[] {
            2020,
            0,
            0,
            0});
            this.nudYear.Minimum = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            this.nudYear.Name = "nudYear";
            this.nudYear.Size = new System.Drawing.Size(100, 23);
            this.nudYear.TabIndex = 4;
            this.nudYear.Value = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            // 
            // dtpDatePay
            // 
            this.dtpDatePay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDatePay.Location = new System.Drawing.Point(15, 12);
            this.dtpDatePay.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            this.dtpDatePay.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.dtpDatePay.Name = "dtpDatePay";
            this.dtpDatePay.Size = new System.Drawing.Size(243, 23);
            this.dtpDatePay.TabIndex = 5;
            // 
            // cbTypePayment
            // 
            this.cbTypePayment.FormattingEnabled = true;
            this.cbTypePayment.Location = new System.Drawing.Point(12, 116);
            this.cbTypePayment.Name = "cbTypePayment";
            this.cbTypePayment.Size = new System.Drawing.Size(246, 25);
            this.cbTypePayment.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Вид платежа";
            // 
            // tbNotePay
            // 
            this.tbNotePay.Location = new System.Drawing.Point(9, 249);
            this.tbNotePay.Multiline = true;
            this.tbNotePay.Name = "tbNotePay";
            this.tbNotePay.Size = new System.Drawing.Size(246, 55);
            this.tbNotePay.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 229);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Примечание";
            // 
            // chbAdditionalPay
            // 
            this.chbAdditionalPay.AutoSize = true;
            this.chbAdditionalPay.Location = new System.Drawing.Point(12, 198);
            this.chbAdditionalPay.Name = "chbAdditionalPay";
            this.chbAdditionalPay.Size = new System.Drawing.Size(261, 21);
            this.chbAdditionalPay.TabIndex = 8;
            this.chbAdditionalPay.Text = "Не учитывать в основных платежах";
            this.chbAdditionalPay.UseVisualStyleBackColor = true;
            // 
            // PayForm
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(274, 360);
            this.Controls.Add(this.chbAdditionalPay);
            this.Controls.Add(this.tbNotePay);
            this.Controls.Add(this.cbTypePayment);
            this.Controls.Add(this.dtpDatePay);
            this.Controls.Add(this.nudYear);
            this.Controls.Add(this.cbMonth);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPay);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Платежи";
            this.Load += new System.EventHandler(this.PayForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudYear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.TextBox tbPay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbMonth;
        private System.Windows.Forms.NumericUpDown nudYear;
        private System.Windows.Forms.DateTimePicker dtpDatePay;
        private System.Windows.Forms.ComboBox cbTypePayment;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbNotePay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chbAdditionalPay;
    }
}