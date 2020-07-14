namespace Istra
{
    partial class DiscountForm
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
            this.dgvSchedules = new System.Windows.Forms.DataGridView();
            this.btCalculate = new System.Windows.Forms.Button();
            this.tbDiscount = new System.Windows.Forms.TextBox();
            this.btClear = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.rbPercent = new System.Windows.Forms.RadioButton();
            this.rbCurrency = new System.Windows.Forms.RadioButton();
            this.btCancel = new System.Windows.Forms.Button();
            this.cbPrivilege = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chbCheckAll = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedules)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSchedules
            // 
            this.dgvSchedules.AllowUserToAddRows = false;
            this.dgvSchedules.AllowUserToDeleteRows = false;
            this.dgvSchedules.AllowUserToResizeRows = false;
            this.dgvSchedules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSchedules.Location = new System.Drawing.Point(13, 79);
            this.dgvSchedules.Margin = new System.Windows.Forms.Padding(4);
            this.dgvSchedules.Name = "dgvSchedules";
            this.dgvSchedules.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvSchedules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSchedules.Size = new System.Drawing.Size(468, 270);
            this.dgvSchedules.TabIndex = 0;
            // 
            // btCalculate
            // 
            this.btCalculate.Location = new System.Drawing.Point(501, 163);
            this.btCalculate.Margin = new System.Windows.Forms.Padding(4);
            this.btCalculate.Name = "btCalculate";
            this.btCalculate.Size = new System.Drawing.Size(117, 30);
            this.btCalculate.TabIndex = 1;
            this.btCalculate.Text = "Назначить";
            this.btCalculate.UseVisualStyleBackColor = true;
            this.btCalculate.Click += new System.EventHandler(this.btCalculate_Click);
            // 
            // tbDiscount
            // 
            this.tbDiscount.Location = new System.Drawing.Point(500, 79);
            this.tbDiscount.Name = "tbDiscount";
            this.tbDiscount.Size = new System.Drawing.Size(118, 23);
            this.tbDiscount.TabIndex = 2;
            this.tbDiscount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btClear
            // 
            this.btClear.Location = new System.Drawing.Point(501, 200);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(118, 30);
            this.btClear.TabIndex = 3;
            this.btClear.Text = "Снять";
            this.btClear.UseVisualStyleBackColor = true;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(501, 309);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(117, 30);
            this.btOk.TabIndex = 3;
            this.btOk.Text = "OK";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // rbPercent
            // 
            this.rbPercent.AutoSize = true;
            this.rbPercent.Checked = true;
            this.rbPercent.Location = new System.Drawing.Point(501, 108);
            this.rbPercent.Name = "rbPercent";
            this.rbPercent.Size = new System.Drawing.Size(38, 21);
            this.rbPercent.TabIndex = 4;
            this.rbPercent.TabStop = true;
            this.rbPercent.Text = "%";
            this.rbPercent.UseVisualStyleBackColor = true;
            // 
            // rbCurrency
            // 
            this.rbCurrency.AutoSize = true;
            this.rbCurrency.Location = new System.Drawing.Point(563, 108);
            this.rbCurrency.Name = "rbCurrency";
            this.rbCurrency.Size = new System.Drawing.Size(50, 21);
            this.rbCurrency.TabIndex = 5;
            this.rbCurrency.Text = "Руб";
            this.rbCurrency.UseVisualStyleBackColor = true;
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(501, 345);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(117, 30);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // cbPrivilege
            // 
            this.cbPrivilege.FormattingEnabled = true;
            this.cbPrivilege.Location = new System.Drawing.Point(13, 35);
            this.cbPrivilege.Name = "cbPrivilege";
            this.cbPrivilege.Size = new System.Drawing.Size(605, 25);
            this.cbPrivilege.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Основание предоставления скидки";
            // 
            // chbCheckAll
            // 
            this.chbCheckAll.AutoSize = true;
            this.chbCheckAll.Location = new System.Drawing.Point(502, 135);
            this.chbCheckAll.Name = "chbCheckAll";
            this.chbCheckAll.Size = new System.Drawing.Size(118, 21);
            this.chbCheckAll.TabIndex = 9;
            this.chbCheckAll.Text = "Выделить все";
            this.chbCheckAll.UseVisualStyleBackColor = true;
            this.chbCheckAll.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 358);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(308, 358);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "label2";
            // 
            // DiscountForm
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(631, 388);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chbCheckAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbPrivilege);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.rbCurrency);
            this.Controls.Add(this.rbPercent);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.btClear);
            this.Controls.Add(this.tbDiscount);
            this.Controls.Add(this.btCalculate);
            this.Controls.Add(this.dgvSchedules);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DiscountForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Скидка";
            this.Load += new System.EventHandler(this.DiscountForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedules)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSchedules;
        private System.Windows.Forms.Button btCalculate;
        private System.Windows.Forms.TextBox tbDiscount;
        private System.Windows.Forms.Button btClear;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.RadioButton rbPercent;
        private System.Windows.Forms.RadioButton rbCurrency;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.ComboBox cbPrivilege;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbCheckAll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}