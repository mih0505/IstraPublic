using Istra.Documents;
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
    public partial class PrintContractForm : Form
    {
        public PrintContract pc;
        public PrintContractForm(PrintContract data)
        {
            pc = data;
            InitializeComponent();
        }

        private void PrintContractForm_Load(object sender, EventArgs e)
        {
            //if(pc.Course.Name == "ЛДП")

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var report = new Report();
            if (chbContract.Checked)
            {
                if (rbCHOU.Checked)
                {
                    string contract = (pc.Course.Name != "ЛДП") ? "CHOU" : "camp";

                    if (rbFullFill.Checked)
                        report.ExportContract(pc, "fullFill", contract, cbAgree.Checked, cbShedule.Checked);
                    else if (rbWithoutRecvisits.Checked)
                        report.ExportContract(pc, "withoutRecvisits", contract, cbAgree.Checked, cbShedule.Checked);
                    else
                        report.ExportContract(pc, "nothing", contract, cbAgree.Checked, cbShedule.Checked);
                }
                else
                {
                    if (rbFullFill.Checked)
                        report.ExportContract(pc, "fullFill", "IP", cbAgree.Checked, cbShedule.Checked);
                    else if (rbWithoutRecvisits.Checked)
                        report.ExportContract(pc, "withoutRecvisits", "IP", cbAgree.Checked, cbShedule.Checked);
                    else
                        report.ExportContract(pc, "nothing", "IP", cbAgree.Checked, cbShedule.Checked);
                }
            }
            this.Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbIP_CheckedChanged(object sender, EventArgs e)
        {
            cbShedule.Enabled = false;
        }

        private void rbCHOU_CheckedChanged(object sender, EventArgs e)
        {
            cbShedule.Enabled = true;
        }
    }
}
