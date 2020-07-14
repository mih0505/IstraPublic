using Istra.Entities;
using Microsoft.Win32;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace Istra
{
    public partial class AuthForm : Form
    {
        const string userRoot = "HKEY_CURRENT_USER\\SOFTWARE";
        const string subkey = "IstraCRM";
        const string keyName = userRoot + "\\" + subkey;
        bool exit = false;
        IstraContext db = new IstraContext();
        public AuthForm()
        {
            InitializeComponent();
        }

        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            exit = true;
            Application.Exit();            
        }

        private void AuthForm_Load(object sender, EventArgs e)
        {
            try
            {
                cbHousing.DataSource = db.Housings.Where(a => a.IsRemoved == false).OrderBy(a => a.Name).ToList();
                cbHousing.DisplayMember = "Name";
                cbHousing.ValueMember = "Id";

                cbLogin.DataSource = db.Workers.Where(a => a.IsRemoved != true).OrderBy(a => a.Lastname).ToList();
                cbLogin.DisplayMember = "Login";
                cbLogin.ValueMember = "Id";

                //выбор значений по умолчанию
                cbHousing.SelectedValue = Convert.ToInt32(Registry.GetValue(keyName, "Branch", -1));
                cbLogin.SelectedValue = Convert.ToInt32(Registry.GetValue(keyName, "User", -1));

                btnOK.Focus();
            }
            catch (Exception ex)
            {
                string methodName = ex.TargetSite + ";\r\n" + ex.StackTrace;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                var login = db.Workers.Count(a => a.Login == cbLogin.Text && a.Password == tbPassword.Text);
                if (login == 1)
                {
                    var r = db.Roles.ToList();
                    CurrentSession.CurrentUser = db.Workers.FirstOrDefault(a => a.Login == cbLogin.Text);
                    CurrentSession.CurrentRole = db.Roles.Find(CurrentSession.CurrentUser.RoleId);
                    int idHousing = Convert.ToInt32(cbHousing.SelectedValue);
                    CurrentSession.CurrentHousing = db.Housings.FirstOrDefault(a => a.Id == idHousing);
                    CurrentSession.TimeRun = DateTime.Now;
                    //запись данных последнего входа в реестр
                    Registry.SetValue(keyName, "Branch", cbHousing.SelectedValue, RegistryValueKind.DWord);
                    Registry.SetValue(keyName, "User", cbLogin.SelectedValue, RegistryValueKind.DWord);

                    Close();
                }
                else
                {
                    tbPassword.Text = "";
                    label2.Text = "Ошибка! Вход не выполнен";                    
                }
            }
            catch (Exception ex)
            {
                string methodName = ex.TargetSite + ";\r\n" + ex.StackTrace;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AuthForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(label2.Text == "Ошибка! Вход не выполнен" && !exit)
                e.Cancel = true;
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            label2.Text = "";
        }
    }
}
