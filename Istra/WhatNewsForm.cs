using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Istra
{
    public partial class WhatNewsForm : Form
    {
        public WhatNewsForm()
        {
            InitializeComponent();
        }

        private void WhatNewsForm_Load(object sender, EventArgs e)
        {
            try
            {
                string path = Directory.GetCurrentDirectory() + "\\" + "version.rtf";
                richTextBox1.LoadFile(path);                  
            }
            catch (Exception ex)
            {
                var m = new System.Diagnostics.StackTrace(false).GetFrame(0).GetMethod();
                string methodName = m.DeclaringType.ToString() + ";" + m.Name;
                CurrentSession.ReportError(methodName, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
