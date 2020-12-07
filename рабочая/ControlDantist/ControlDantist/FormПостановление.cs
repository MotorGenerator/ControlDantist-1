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
    public partial class FormПостановление : Form
    {
        public FormПостановление()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.txtНомер.Text.Length != 0)
            {
                this.txtПостановление.Enabled = true;
            }
            else
            {
                this.txtПостановление.Enabled = false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (this.txtПостановление.Text.Length != 0)
            {
                this.dateTimePicker1.Enabled = true;
                this.btnStart.Enabled = true;
            }
            else
            {
                this.dateTimePicker1.Enabled = false;
                this.btnStart.Enabled = false;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //Выполним всё в одной транзакции
            StringBuilder builder = new StringBuilder();

            //Запишем название номер  и дату постановления на основании которого действуют нормы 
            string queryПостановление = "INSERT INTO ПостановлениеПравительства " +
                           "([Потстановление] " +
                           ",[Номер] " +
                           ",[ДатаПостановления]) " +
                           "VALUES " +
                           "('"+ this.txtПостановление.Text.Trim() +"' " +
                           ",'"+ this.txtНомер.Text.Trim() +"' " +
                           ",'"+ this.dateTimePicker3.Value.ToShortDateString() +"' ) ";

            //Добавим в builder
            builder.Append(queryПостановление);

            //Запишем начало действия данного постановления
            string queryTime = " INSERT INTO [ВременнойИнтервал] " +
                               "([ВремяНачало] " +
                               ",[ВремяОкончания] " +
                               ",[id_постановление]) " +
                               "VALUES " +
                               "('" + this.dateTimePicker1.Value.ToShortDateString() + "' " +
                               ",'0' " +
                               ",@@IDENTITY)";

            //Добавим в builder
            builder.Append(queryTime);

            string query = builder.ToString();

            SqlConnection con = new SqlConnection(ConnectDB.ConnectionString());
            SqlCommand com = new SqlCommand(query, con);
            
            con.Open();
            com.ExecuteNonQuery();
            con.Close();

            //Закроем форму
            this.Close();


        }

        private void FormПостановление_Load(object sender, EventArgs e)
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
    }
}