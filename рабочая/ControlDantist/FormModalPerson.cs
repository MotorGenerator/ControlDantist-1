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
    public partial class FormModalPerson : Form
    {
        /// <summary>
        /// Хранит номер договора
        /// </summary>
        public string NumContract { get; set; }

        public FormModalPerson()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormModalPerson_Load(object sender, EventArgs e)
        {
            this.label1.Text = "Договор № " + this.NumContract.ToString().Trim() + " уже существует как прошедший проверку";
        }
    }
}
