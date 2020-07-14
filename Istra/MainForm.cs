using Istra.Entities;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using System.Data.Entity;

namespace Istra
{
    public partial class MainForm : Form
    {
        IstraContext db = new IstraContext();

        public MainForm()
        {            
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text += CurrentSession.version;

            var fAuthForm = new AuthForm();
            var authResult = fAuthForm.ShowDialog();

            if (authResult == DialogResult.Cancel)
                Application.Exit();

            //распределение прав пользователя
            if (CurrentSession.CurrentRole != null)
            {
                int idRole = CurrentSession.CurrentRole.Id;
                var lstManageRole = db.ManageRoles.Include(a => a.Permission).Where(a => a.RoleId == idRole).ToList();

                //настройка прав доступа для текущей роли                                                            
                добавитьСлушателяToolStripMenuItem.Visible = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "EditStudent").Value;
                списокСлушателейToolStripMenuItem.Visible = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "ListStudents").Value;
                архивToolStripMenuItem.Visible = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "ArchiveStudents").Value;
                архивToolStripMenuItem.Visible = CurrentSession.CurrentUser.AllAccessGroups;
                добавитьГруппуToolStripMenuItem.Visible = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "EditGroup").Value;
                списокГруппToolStripMenuItem.Visible = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "ListGroups").Value;
                списокЗанятийToolStripMenuItem.Visible = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "ListLessons").Value;
                платежиToolStripMenuItem1.Visible = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "ListPayments").Value;
                возвратПлатежейToolStripMenuItem.Visible = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "ListPayments").Value;
                планПриемаВГруппыToolStripMenuItem.Visible = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "PlanEnroll").Value;
                отчетУчащиесяToolStripMenuItem.Visible = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "FinanceReports").Value;
                отчетГруппыToolStripMenuItem.Visible = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "FinanceReports").Value;
                заработнаяПлатаToolStripMenuItem.Visible = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "Wages").Value;


                //определение необходимости окна настроек
                bool s1 = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "Directories").Value;
                bool s2 = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "ManageUsers").Value;
                bool s3 = lstManageRole.FirstOrDefault(a => a.Permission.SystemName == "ManageRoles").Value;
                if(s1 || s2 || s3)
                    настройкаToolStripMenuItem.Visible = true;
                else
                    настройкаToolStripMenuItem.Visible = false;                                               
            }

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void добавитьСлушателяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var studentForm = new StudentForm(0, true);
            studentForm.MdiParent = this;
            //studentForm.addStudent = true;
            studentForm.Show();
            studentForm.Location = new Point(20, 10);
            studentForm.WindowState = FormWindowState.Maximized;
        }

        private void списокСлушателейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var studentsForm = new ListStudentsForm();
            studentsForm.MdiParent = this;
            studentsForm.Show();
            studentsForm.Location = new Point(20, 10);
            studentsForm.WindowState = FormWindowState.Maximized;
        }

        private void списокГруппToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var groupsForm = new ListGroupsForm();
            groupsForm.MdiParent = this;
            groupsForm.Show();
        }

        private void платежиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var paymentsForm = new ListPaymentsForm();
            paymentsForm.MdiParent = this;
            paymentsForm.WindowState = FormWindowState.Maximized;
            paymentsForm.Show();
        }

        private void настройкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingForm();
            settingsForm.ShowDialog();
        }

        private void добавитьГруппуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var groupForm = new GroupForm(null);
            groupForm.ShowDialog();
        }

        private void архивToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fArchine = new ArchiveForm();
            fArchine.MdiParent = this;
            fArchine.Show();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fAbout = new AboutForm();
            fAbout.ShowDialog();
        }

        private void чтоНовогоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fWhatNew = new WhatNewsForm();
            fWhatNew.ShowDialog();
        }

        private void отчетыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var financeForm = new FinancesStudentsForm();
            financeForm.MdiParent = this;
            financeForm.Show();
        }

        private void возвратПлатежейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lstReturnPayments = new ListReturnPaysForm();
            lstReturnPayments.MdiParent = this;
            lstReturnPayments.Show();
        }

        private void планПриемаВГруппыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var planGroups = new PlanForm();
            planGroups.MdiParent = this;
            planGroups.Show();
        }

        private void отчетГруппыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var financesGroups = new FinancesGroupsForm();
            financesGroups.MdiParent = this;
            financesGroups.Show();
        }

        private void списокЗанятийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lessonsForm = new ListLessonForm();
            lessonsForm.MdiParent = this;
            lessonsForm.Show();
        }

        private void заработнаяПлатаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var wagesForm = new WagesForm();
            wagesForm.MdiParent = this;
            wagesForm.Show();
        }
    }
}
