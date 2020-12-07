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
    public partial class FormСписокПолучателей : Form
    {
        public string НомерРеестра { get; set; }
        public int Год { get; set; }


        public FormСписокПолучателей()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            НомерРеестра = txtНомерРеестра.Text.Trim();
            Год = dtРеестр.Value.Year;
        }       
    }
}
