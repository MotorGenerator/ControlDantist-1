using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Reports;

namespace ControlDantist
{
    public partial class FormStatistic : Form
    {
        public FormStatistic()
        {
            InitializeComponent();
        }

        private void FormStatistic_Load(object sender, EventArgs e)
        {
            this.label1.Text = DateTime.Today.Year.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            PrintReportStatistic printReportStatistic = new PrintReportStatistic("01.01." + this.label1.Text);
            printReportStatistic.Print();

            return;
        }
    }
}
