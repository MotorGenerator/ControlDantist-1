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
    public partial class FormMessage : Form
    {
        public string ErrorMessage { get; set; }
        public FormMessage()
        {
            InitializeComponent();
        }

        public FormMessage(string errorText)
            : this()
        {
            if (errorText != null)
            {
                this.label1.Text = errorText;
            }
           
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }
    }
}
