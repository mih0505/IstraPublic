namespace Istra
{
    partial class PrintContractForm
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
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.rbNothing = new System.Windows.Forms.RadioButton();
            this.rbWithoutRecvisits = new System.Windows.Forms.RadioButton();
            this.rbFullFill = new System.Windows.Forms.RadioButton();
            this.chbContract = new System.Windows.Forms.CheckBox();
            this.rbCHOU = new System.Windows.Forms.RadioButton();
            this.rbIP = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbShedule = new System.Windows.Forms.CheckBox();
            this.cbAgree = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btOK.Location = new System.Drawing.Point(126, 199);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 32);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCancel.Location = new System.Drawing.Point(207, 199);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(79, 32);
            this.btCancel.TabIndex = 0;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // rbNothing
            // 
            this.rbNothing.AutoSize = true;
            this.rbNothing.Location = new System.Drawing.Point(3, 8);
            this.rbNothing.Name = "rbNothing";
            this.rbNothing.Size = new System.Drawing.Size(61, 17);
            this.rbNothing.TabIndex = 2;
            this.rbNothing.Text = "Пустой";
            this.rbNothing.UseVisualStyleBackColor = true;
            // 
            // rbWithoutRecvisits
            // 
            this.rbWithoutRecvisits.AutoSize = true;
            this.rbWithoutRecvisits.Location = new System.Drawing.Point(3, 31);
            this.rbWithoutRecvisits.Name = "rbWithoutRecvisits";
            this.rbWithoutRecvisits.Size = new System.Drawing.Size(106, 17);
            this.rbWithoutRecvisits.TabIndex = 2;
            this.rbWithoutRecvisits.Text = "Без реквизитов";
            this.rbWithoutRecvisits.UseVisualStyleBackColor = true;
            // 
            // rbFullFill
            // 
            this.rbFullFill.AutoSize = true;
            this.rbFullFill.Checked = true;
            this.rbFullFill.Location = new System.Drawing.Point(3, 54);
            this.rbFullFill.Name = "rbFullFill";
            this.rbFullFill.Size = new System.Drawing.Size(88, 17);
            this.rbFullFill.TabIndex = 2;
            this.rbFullFill.TabStop = true;
            this.rbFullFill.Text = "Заполненый";
            this.rbFullFill.UseVisualStyleBackColor = true;
            // 
            // chbContract
            // 
            this.chbContract.AutoSize = true;
            this.chbContract.Checked = true;
            this.chbContract.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbContract.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chbContract.Location = new System.Drawing.Point(12, 12);
            this.chbContract.Name = "chbContract";
            this.chbContract.Size = new System.Drawing.Size(143, 21);
            this.chbContract.TabIndex = 4;
            this.chbContract.Text = "Печать договора:";
            this.chbContract.UseVisualStyleBackColor = true;
            // 
            // rbCHOU
            // 
            this.rbCHOU.AutoSize = true;
            this.rbCHOU.Checked = true;
            this.rbCHOU.Location = new System.Drawing.Point(21, 47);
            this.rbCHOU.Name = "rbCHOU";
            this.rbCHOU.Size = new System.Drawing.Size(49, 17);
            this.rbCHOU.TabIndex = 5;
            this.rbCHOU.TabStop = true;
            this.rbCHOU.Text = "ЧОУ";
            this.rbCHOU.UseVisualStyleBackColor = true;
            this.rbCHOU.CheckedChanged += new System.EventHandler(this.rbCHOU_CheckedChanged);
            // 
            // rbIP
            // 
            this.rbIP.AutoSize = true;
            this.rbIP.Location = new System.Drawing.Point(21, 70);
            this.rbIP.Name = "rbIP";
            this.rbIP.Size = new System.Drawing.Size(41, 17);
            this.rbIP.TabIndex = 5;
            this.rbIP.Text = "ИП";
            this.rbIP.UseVisualStyleBackColor = true;
            this.rbIP.CheckedChanged += new System.EventHandler(this.rbIP_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbNothing);
            this.panel1.Controls.Add(this.rbWithoutRecvisits);
            this.panel1.Controls.Add(this.rbFullFill);
            this.panel1.Location = new System.Drawing.Point(127, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(111, 78);
            this.panel1.TabIndex = 6;
            // 
            // cbShedule
            // 
            this.cbShedule.AutoSize = true;
            this.cbShedule.Checked = true;
            this.cbShedule.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShedule.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbShedule.Location = new System.Drawing.Point(12, 126);
            this.cbShedule.Name = "cbShedule";
            this.cbShedule.Size = new System.Drawing.Size(194, 21);
            this.cbShedule.TabIndex = 4;
            this.cbShedule.Text = "Печать график платежей";
            this.cbShedule.UseVisualStyleBackColor = true;
            // 
            // cbAgree
            // 
            this.cbAgree.AutoSize = true;
            this.cbAgree.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbAgree.Location = new System.Drawing.Point(12, 153);
            this.cbAgree.Name = "cbAgree";
            this.cbAgree.Size = new System.Drawing.Size(138, 21);
            this.cbAgree.TabIndex = 4;
            this.cbAgree.Text = "Печать согласия";
            this.cbAgree.UseVisualStyleBackColor = true;
            // 
            // PrintContractForm
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(298, 243);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.rbIP);
            this.Controls.Add(this.rbCHOU);
            this.Controls.Add(this.cbAgree);
            this.Controls.Add(this.cbShedule);
            this.Controls.Add(this.chbContract);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PrintContractForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PrintContract";
            this.Load += new System.EventHandler(this.PrintContractForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.RadioButton rbNothing;
        private System.Windows.Forms.RadioButton rbWithoutRecvisits;
        private System.Windows.Forms.RadioButton rbFullFill;
        private System.Windows.Forms.CheckBox chbContract;
        private System.Windows.Forms.RadioButton rbCHOU;
        private System.Windows.Forms.RadioButton rbIP;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cbShedule;
        private System.Windows.Forms.CheckBox cbAgree;
    }
}