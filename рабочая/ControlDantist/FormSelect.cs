using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Classes;

namespace ControlDantist
{
    public partial class FormSelect : Form
    {
        /// <summary>
        /// Хранит id терр органа
        /// </summary>
        public int IdTO { get; set; }

        public FormSelect()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void FormSelect_Load(object sender, EventArgs e)
        {
            string query = "select id_террОрган,НаименованиеТеррОргана from ТерриториальныйОрган";
            DataTable dt = ТаблицаБД.GetTableSQL(query, "ТерриториальныйОрган");

            this.comboBox1.DataSource = dt;
            this.comboBox1.DisplayMember = "НаименованиеТеррОргана";
            this.comboBox1.ValueMember = "id_террОрган";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.IdTO = Convert.ToInt32(this.comboBox1.SelectedValue);
        }
    }
}
