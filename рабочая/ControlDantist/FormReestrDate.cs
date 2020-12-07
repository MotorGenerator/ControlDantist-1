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
    public partial class FormReestrDate : Form
    {
        public string НомерРеестра { get; set; }
        public DateTime ДатаРеестра { get; set; }
        public string НомерСчётФактуры { get; set; }
        public DateTime ДатаСчётФактуры { get; set; }

        public FormReestrDate()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            НомерРеестра = txtНомерРеестра.Text.Trim();
            ДатаРеестра = dtРеестр.Value.Date;
            НомерСчётФактуры = txtНомерСчётФактуры.Text.Trim();
            ДатаСчётФактуры = dtСчётФактуры.Value.Date;
        }
    }
}
