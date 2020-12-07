using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ControlDantist
{
    public partial class FormControlContract : Form
    {
        private List<string> list;

        /// <summary>
        /// Список для хранения договоров которые не записались в БД
        /// </summary>
        public List<string> List
        {
            get
            {
                return list;
            }
            set
            {
                list = value;
            }
        }

        public FormControlContract()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormControlContract_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = List;
        }
    }
}