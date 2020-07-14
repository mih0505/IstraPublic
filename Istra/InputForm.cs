using Istra.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Istra
{
    public partial class InputForm : Form
    {
        IstraContext db = new IstraContext();
        Template templ;
        TemplateRate templR;
        string mode;
        bool edit = false;
        public InputForm(string mode, object entity)
        {
            templ = new Template();
            templ = entity as Template;

            InitializeComponent();
        }

        private void InputForm_Load(object sender, EventArgs e)
        {
            if (templ.Id != 0)
                textBox1.Text = templ.Name;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            templ.Name = textBox1.Text;
            if (templ.Id == 0)
            {
                db.Templates.Add(templ);
            }
            else
            {
                db.Entry(templ).State = System.Data.Entity.EntityState.Modified;
            }

            db.SaveChanges();
            this.Close();
        }
    }
}
