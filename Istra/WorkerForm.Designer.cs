namespace Istra
{
    partial class WorkerForm
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
            this.tbLastname = new System.Windows.Forms.TextBox();
            this.tbFirstname = new System.Windows.Forms.TextBox();
            this.tbMiddlename = new System.Windows.Forms.TextBox();
            this.tbLastnameEn = new System.Windows.Forms.TextBox();
            this.tbFirstnameEn = new System.Windows.Forms.TextBox();
            this.tbMiddlenameEn = new System.Windows.Forms.TextBox();
            this.cbRoles = new System.Windows.Forms.ComboBox();
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.btLoginGenerate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbDepartment = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbPosts = new System.Windows.Forms.ComboBox();
            this.chbAllAccessGroups = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(327, 462);
            this.btCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(100, 41);
            this.btCancel.TabIndex = 10;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.Location = new System.Drawing.Point(219, 462);
            this.btOK.Margin = new System.Windows.Forms.Padding(4);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(100, 41);
            this.btOK.TabIndex = 9;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // tbLastname
            // 
            this.tbLastname.Location = new System.Drawing.Point(181, 91);
            this.tbLastname.Name = "tbLastname";
            this.tbLastname.Size = new System.Drawing.Size(228, 23);
            this.tbLastname.TabIndex = 0;
            this.tbLastname.TextChanged += new System.EventHandler(this.tbLastname_TextChanged);
            // 
            // tbFirstname
            // 
            this.tbFirstname.Location = new System.Drawing.Point(181, 126);
            this.tbFirstname.Name = "tbFirstname";
            this.tbFirstname.Size = new System.Drawing.Size(228, 23);
            this.tbFirstname.TabIndex = 1;
            this.tbFirstname.TextChanged += new System.EventHandler(this.tbLastname_TextChanged);
            // 
            // tbMiddlename
            // 
            this.tbMiddlename.Location = new System.Drawing.Point(181, 161);
            this.tbMiddlename.Name = "tbMiddlename";
            this.tbMiddlename.Size = new System.Drawing.Size(228, 23);
            this.tbMiddlename.TabIndex = 2;
            this.tbMiddlename.TextChanged += new System.EventHandler(this.tbLastname_TextChanged);
            // 
            // tbLastnameEn
            // 
            this.tbLastnameEn.Location = new System.Drawing.Point(181, 313);
            this.tbLastnameEn.Name = "tbLastnameEn";
            this.tbLastnameEn.Size = new System.Drawing.Size(228, 23);
            this.tbLastnameEn.TabIndex = 6;
            this.tbLastnameEn.TextChanged += new System.EventHandler(this.tbLastname_TextChanged);
            // 
            // tbFirstnameEn
            // 
            this.tbFirstnameEn.Location = new System.Drawing.Point(181, 348);
            this.tbFirstnameEn.Name = "tbFirstnameEn";
            this.tbFirstnameEn.Size = new System.Drawing.Size(228, 23);
            this.tbFirstnameEn.TabIndex = 7;
            this.tbFirstnameEn.TextChanged += new System.EventHandler(this.tbLastname_TextChanged);
            // 
            // tbMiddlenameEn
            // 
            this.tbMiddlenameEn.Location = new System.Drawing.Point(181, 383);
            this.tbMiddlenameEn.Name = "tbMiddlenameEn";
            this.tbMiddlenameEn.Size = new System.Drawing.Size(228, 23);
            this.tbMiddlenameEn.TabIndex = 8;
            this.tbMiddlenameEn.TextChanged += new System.EventHandler(this.tbLastname_TextChanged);
            // 
            // cbRoles
            // 
            this.cbRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRoles.FormattingEnabled = true;
            this.cbRoles.Location = new System.Drawing.Point(181, 196);
            this.cbRoles.Name = "cbRoles";
            this.cbRoles.Size = new System.Drawing.Size(228, 25);
            this.cbRoles.TabIndex = 3;
            this.cbRoles.SelectionChangeCommitted += new System.EventHandler(this.tbLastname_TextChanged);
            // 
            // tbLogin
            // 
            this.tbLogin.Location = new System.Drawing.Point(181, 233);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(195, 23);
            this.tbLogin.TabIndex = 4;
            this.tbLogin.TextChanged += new System.EventHandler(this.tbLastname_TextChanged);
            // 
            // btLoginGenerate
            // 
            this.btLoginGenerate.Location = new System.Drawing.Point(378, 233);
            this.btLoginGenerate.Name = "btLoginGenerate";
            this.btLoginGenerate.Size = new System.Drawing.Size(31, 23);
            this.btLoginGenerate.TabIndex = 4;
            this.btLoginGenerate.Text = "+";
            this.btLoginGenerate.UseVisualStyleBackColor = true;
            this.btLoginGenerate.Click += new System.EventHandler(this.btLoginGenerate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Фамилия";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Имя";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Отчество";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Роль";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 236);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "Логин";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 316);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "Фамилия (En)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 351);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 17);
            this.label7.TabIndex = 5;
            this.label7.Text = "Имя (En)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 386);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 17);
            this.label8.TabIndex = 5;
            this.label8.Text = "Отчество (En)";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(181, 268);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(228, 23);
            this.tbPassword.TabIndex = 5;
            this.tbPassword.UseSystemPasswordChar = true;
            this.tbPassword.TextChanged += new System.EventHandler(this.tbLastname_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(33, 271);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 17);
            this.label9.TabIndex = 5;
            this.label9.Text = "Пароль";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(33, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 17);
            this.label10.TabIndex = 5;
            this.label10.Text = "Отдел";
            // 
            // cbDepartment
            // 
            this.cbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDepartment.FormattingEnabled = true;
            this.cbDepartment.Location = new System.Drawing.Point(181, 19);
            this.cbDepartment.Name = "cbDepartment";
            this.cbDepartment.Size = new System.Drawing.Size(228, 25);
            this.cbDepartment.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(33, 58);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 17);
            this.label11.TabIndex = 5;
            this.label11.Text = "Должность";
            // 
            // cbPosts
            // 
            this.cbPosts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPosts.FormattingEnabled = true;
            this.cbPosts.Location = new System.Drawing.Point(181, 55);
            this.cbPosts.Name = "cbPosts";
            this.cbPosts.Size = new System.Drawing.Size(228, 25);
            this.cbPosts.TabIndex = 11;
            // 
            // chbAllAccessGroups
            // 
            this.chbAllAccessGroups.AutoSize = true;
            this.chbAllAccessGroups.Location = new System.Drawing.Point(181, 412);
            this.chbAllAccessGroups.Name = "chbAllAccessGroups";
            this.chbAllAccessGroups.Size = new System.Drawing.Size(186, 21);
            this.chbAllAccessGroups.TabIndex = 12;
            this.chbAllAccessGroups.Text = "Доступ ко всем группам";
            this.chbAllAccessGroups.UseVisualStyleBackColor = true;
            // 
            // WorkerForm
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(440, 516);
            this.Controls.Add(this.chbAllAccessGroups);
            this.Controls.Add(this.cbPosts);
            this.Controls.Add(this.cbDepartment);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btLoginGenerate);
            this.Controls.Add(this.cbRoles);
            this.Controls.Add(this.tbMiddlenameEn);
            this.Controls.Add(this.tbFirstnameEn);
            this.Controls.Add(this.tbLogin);
            this.Controls.Add(this.tbLastnameEn);
            this.Controls.Add(this.tbMiddlename);
            this.Controls.Add(this.tbFirstname);
            this.Controls.Add(this.tbLastname);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WorkerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сотрудник";
            this.Load += new System.EventHandler(this.WorkerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.TextBox tbLastname;
        private System.Windows.Forms.TextBox tbFirstname;
        private System.Windows.Forms.TextBox tbMiddlename;
        private System.Windows.Forms.TextBox tbLastnameEn;
        private System.Windows.Forms.TextBox tbFirstnameEn;
        private System.Windows.Forms.TextBox tbMiddlenameEn;
        private System.Windows.Forms.ComboBox cbRoles;
        private System.Windows.Forms.TextBox tbLogin;
        private System.Windows.Forms.Button btLoginGenerate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbDepartment;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbPosts;
        private System.Windows.Forms.CheckBox chbAllAccessGroups;
    }
}