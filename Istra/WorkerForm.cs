using Istra.Entities;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace Istra
{
    public partial class WorkerForm : Form
    {
        public bool edit = false;//хранит состояние редактировались ли данные        
        IstraContext db = new IstraContext();
        Worker worker;
        public WorkerForm(int? WorkerId)
        {
            InitializeComponent();

            if (WorkerId == null)
            {
                worker = new Worker();
            }
            else
            {
                try
                {
                    worker = db.Workers.Find(WorkerId);
                }
                catch (Exception ex)
                {
                    var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                    string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                    CurrentSession.ReportError(methodName, ex.Message);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            try
            {
                var roles = db.Roles.Where(a=>a.Priority > CurrentSession.CurrentRole.Priority).OrderBy(a => a.Name).ToList();
                cbRoles.DataSource = roles;
                cbRoles.ValueMember = "Id";
                cbRoles.DisplayMember = "Name";

                var departments = db.Departments.OrderBy(a => a.Name).ToList();
                cbDepartment.DataSource = departments;
                cbDepartment.ValueMember = "Id";
                cbDepartment.DisplayMember = "Name";

                var posts = db.Posts.OrderBy(a => a.Name).ToList();
                cbPosts.DataSource = posts;
                cbPosts.ValueMember = "Id";
                cbPosts.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WorkerForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (worker.Id != 0)
                {
                    tbLastname.Text = worker.Lastname;
                    tbFirstname.Text = worker.Firstname;
                    tbMiddlename.Text = worker.Middlename;
                    tbLastnameEn.Text = worker.LastnameEn;
                    tbFirstnameEn.Text = worker.FirstnameEn;
                    tbMiddlenameEn.Text = worker.MiddlenameEn;
                    cbRoles.SelectedValue = worker.RoleId;
                    tbLogin.Text = worker.Login;
                    cbDepartment.SelectedValue = worker.DepartmentId;
                    chbAllAccessGroups.Checked = worker.AllAccessGroups;
                    if (worker.PostId == null)
                        cbPosts.SelectedIndex = -1;
                    else
                        cbPosts.SelectedValue = worker.PostId;
                }
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btLoginGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbFirstname.Text != "" && tbLastname.Text != "" && tbMiddlename.Text != "")
                    tbLogin.Text = tbLastname.Text + " " + tbFirstname.Text.Substring(0, 1) + "." + tbMiddlename.Text.Substring(0, 1) + ".";
                else MessageBox.Show("Заполните поля ФИО сотрудника", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbLastname_TextChanged(object sender, EventArgs e)
        {
            edit = true;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbLastname.Text != "" && tbFirstname.Text != "" && tbMiddlename.Text != "" && tbLogin.Text != "" && cbRoles.Text != "" && cbDepartment.Text != "")
                {
                    if (edit)
                    {
                        if (worker.Id == 0 && tbPassword.Text == "")
                        {
                            MessageBox.Show("При добавлении сотрудника, необходимо обязательно указать пароль!", "Внимание", MessageBoxButtons.OK);
                            return;
                        }

                        if (cbPosts.SelectedIndex == -1)
                        {
                            var dr = MessageBox.Show("Не указана должность сотрудника. Вы хотите продолжить сохранение?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.No)
                                return;
                        }

                        worker.Lastname = tbLastname.Text;
                        worker.Firstname = tbFirstname.Text;
                        worker.Middlename = tbMiddlename.Text;
                        worker.LastnameEn = (tbLastnameEn.Text != "") ? tbLastnameEn.Text : null;
                        worker.FirstnameEn = (tbFirstnameEn.Text != "") ? tbFirstnameEn.Text : null;
                        worker.MiddlenameEn = (tbMiddlenameEn.Text != "") ? tbMiddlenameEn.Text : null;
                        worker.RoleId = Convert.ToInt32(cbRoles.SelectedValue);
                        worker.Login = tbLogin.Text;
                        worker.DepartmentId = Convert.ToInt32(cbDepartment.SelectedValue);
                        worker.AllAccessGroups = chbAllAccessGroups.Checked;
                        if (cbPosts.SelectedIndex != -1)
                            worker.PostId = Convert.ToInt32(cbPosts.SelectedValue);
                        else
                            worker.PostId = null;

                        if (tbPassword.Text != "")
                            worker.Password = tbPassword.Text;
                        try
                        {
                            if (worker.Id == 0)
                                db.Workers.Add(worker);
                            else
                                db.Entry(worker).State = EntityState.Modified;

                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка сохранения данных сотрудника \r\n" + ex.Message, "Ошибка", MessageBoxButtons.OK);
                        }
                        finally
                        {
                            edit = false;
                            Close();
                        }

                    }
                }
                else MessageBox.Show("Не все поля заполнены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
