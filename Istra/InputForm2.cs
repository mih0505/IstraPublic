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
    public partial class InputForm2 : Form
    {
        IstraContext db = new IstraContext();
        TemplateRate templR;
        string mode;
        bool edit = false;
        public InputForm2(object entity)
        {
            templR = new TemplateRate();
            templR = entity as TemplateRate;           

            InitializeComponent();
        }

        private void InputForm_Load(object sender, EventArgs e)
        {
            textBox2.Text = templR.CountStudents.ToString();
            if (templR.Id != 0)
            {
                textBox1.Text = templR.Wage.ToString();
            }
            this.ActiveControl = textBox1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            templR.Wage = Convert.ToDecimal(textBox1.Text);
            templR.CountStudents = Convert.ToInt32(textBox2.Text);
            if (templR.Id == 0)
            {
                db.TemplateRates.Add(templR);
            }
            else
            {
                db.Entry(templR).State = System.Data.Entity.EntityState.Modified;
            }

            db.SaveChanges();
            this.Close();
        }
    }
}
