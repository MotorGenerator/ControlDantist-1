using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlDantist
{
    public partial class FormMessageAct : Form
    {
        /// <summary>
        /// Номер акта.
        /// </summary>
        public string НомерАкта { get; set; }


        public FormMessageAct()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormMessageAct_Load(object sender, EventArgs e)
        {
            this.label1.Text = "Иммется ранее оплаченный договор по акту - " + НомерАкта.Trim();
        }
    }
}
