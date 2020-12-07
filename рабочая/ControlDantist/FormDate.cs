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
    public partial class FormDate : Form
    {
        public string Date { get; set; }
        public FormDate()
        {
            InitializeComponent();
        }

        private void dtnOpen_Click(object sender, EventArgs e)
        {
            this.Date = this.dateTimePicker1.Value.ToShortDateString();
        }
    }
}
