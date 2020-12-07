using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Classes;

namespace ControlDantist
{
    public partial class FormSelectПостановление : Form
    {
        private string _постановление;
        private int _id;

        /// <summary>
        /// Хранит название выбранного постановления
        /// </summary>
        public int ID_Поствановления
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        /// <summary>
        /// Хранит название выбранного постановления
        /// </summary>
        public string Постановление
        {
            get
            {
                return _постановление;
            }
            set
            {
                _постановление = value;
            }
        }

        public FormSelectПостановление()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Потстановление,Номер,ДатаПостановления
            string постановление = this.dataGridView1.CurrentRow.Cells["Потстановление"].Value.ToString().Trim();
            string номер = this.dataGridView1.CurrentRow.Cells["Номер"].Value.ToString().Trim();
            string дата = Convert.ToDateTime(this.dataGridView1.CurrentRow.Cells["ДатаПостановления"].Value).ToShortDateString().Trim();

            this.Постановление = постановление + " № " + номер + " от " + дата;
            //this.Close();


        }

        private void FormSelectПостановление_Load(object sender, EventArgs e)
        {
            string queryLoad = "select * from dbo.ПостановлениеПравительства";

            SqlConnection con = new SqlConnection(ConnectDB.ConnectionString());
            SqlDataAdapter da = new SqlDataAdapter(queryLoad, con);

            con.Open();
            DataTable tab = new DataTable("Постановление");

            da.Fill(tab);
            con.Close();

            this.dataGridView1.DataSource = tab;
            this.dataGridView1.Columns["id_постановление"].Visible = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Потстановление,Номер,ДатаПостановления
            string постановление = this.dataGridView1.CurrentRow.Cells["Потстановление"].Value.ToString().Trim();
            string номер = this.dataGridView1.CurrentRow.Cells["Номер"].Value.ToString().Trim();
            string дата = Convert.ToDateTime(this.dataGridView1.CurrentRow.Cells["ДатаПостановления"].Value).ToShortDateString().Trim();

            this.Постановление = постановление + " № " + номер + " от " + дата;
            this.ID_Поствановления = Convert.ToInt32(this.dataGridView1.CurrentRow.Cells["id_постановление"].Value);

        }
    }
}