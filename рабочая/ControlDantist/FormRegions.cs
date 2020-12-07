using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using ControlDantist.Classes;

namespace ControlDantist
{
    public partial class FormRegions : Form
    {
        public FormRegions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// id района.
        /// </summary>
        public int IdRegion { get; set; }

        private void FormRegions_Load(object sender, EventArgs e)
        {
            string sCon = ConfigurationSettings.AppSettings["connect"];

            string query = "select * from РайонОбласти";

            //DataTable tabRegion = ТаблицаБД.GetTableSQL(query,sCon,"RegionName");
            DataTable tabRegion = ТаблицаБД.GetTableSQL(query, "RegionName");

            this.comboBox1.DataSource = tabRegion;
            comboBox1.DisplayMember = "NameRegion";
            comboBox1.ValueMember = "idRegion";
            
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if(e.KeyChar == )
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.comboBox1.Items.Count > 0)
            //{
            //    string strIdRegion = this.comboBox1.SelectedValue.ToString();

            //    //this.Close();
            //}
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //if (this.comboBox1.Items.Count > 0)
            //{
            //    string strIdRegion = this.comboBox1.SelectedValue.ToString();

            //    MessageBox.Show("Выбрали");

            //    //this.Close();
            //}
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.comboBox1.Items.Count > 0)
            {
                string strIdRegion = this.comboBox1.SelectedValue.ToString();

                this.IdRegion = int.Parse(strIdRegion);

                // Вызывим событие нажатия по кнопке.
                this.button1.Click += new EventHandler(button1_Click);

                this.button1.PerformClick();

                //this.Close();
            }
        }

        void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
