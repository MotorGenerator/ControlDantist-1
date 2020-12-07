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
    public partial class FormSelectHeadDepartment : Form
    {
        //Таблица содержит id по выбранному на текущий момент начальнику управления автоматизации
        private DataTable tabCurrentSelect;


        public FormSelectHeadDepartment()
        {
            InitializeComponent();
        }

        private void FormSelectHeadDepartment_Load(object sender, EventArgs e)
        {

             //конкретный 
            DataTable tabIdCurrent;
            string item1 = string.Empty;

            //Содержит таблицу всех записанных в БД начальников управления автоматизации 
            DataTable tabSelect;
            string query = "select * from [ДолжностьАналитичУправ] ";

            using(SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
            {
                con.Open();
                SqlTransaction transact = con.BeginTransaction();
                tabSelect = ТаблицаБД.GetTableSQL(query, "ДолжностьАналитичУправ", con, transact);

                //Узнаем кто из начальнико выбран на текущий момент
                string stringSelect = "select id_должность from ПодписьДолжность";
                tabIdCurrent = ТаблицаБД.GetTableSQL(stringSelect, "ДолжностьАналитичУправ1", con, transact);

                if (tabIdCurrent.Rows.Count != 0)
                {
                    string queryCurrent = "select * from [ДолжностьАналитичУправ] where id_должность = " + Convert.ToInt32(tabIdCurrent.Rows[0]["id_должность"]) + " ";
                    tabCurrentSelect = ТаблицаБД.GetTableSQL(queryCurrent, "ДолжностьАналитичУправ2", con, transact);
                    //item1 = tabCurrentSelect.Rows[0]["фамилия"].ToString().Trim() +" " + tabCurrentSelect.Rows[0]["инициалы"].ToString().Trim();
                    item1 = tabCurrentSelect.Rows[0]["фио_инициалы"].ToString().Trim(); 

                }
            }

            foreach (DataRow row in tabSelect.Rows)
            {
                string item = row["фио_инициалы"].ToString().Trim();
                this.comboBox1.Items.Add(item);
            }


            this.comboBox1.Text = item1;

            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Зароем форму
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string stringUpdate = "declare @id int " +
                                  "select @id = id_должность from dbo.ДолжностьАналитичУправ where фио_инициалы = '" + this.comboBox1.Text.Trim() + "' " +
                                  "declare @count int " +
                                  "select @count = COUNT(*) from ПодписьДолжность " +
                                  "if(@count = 0) " +
                                  "begin " +
                                  "INSERT INTO ПодписьДолжность " +
                                  "([id_должность]) " +
                                  "VALUES " +
                                  "(@id) " +
                                  "end " +
                                  "else " +
                                  "begin " +
                                  "declare @iid int " +
                                  "select @iid = id_должность from ПодписьДолжность " +
                                  "UPDATE ПодписьДолжность " +
                                  "SET [id_должность] = @id " +
                                  "WHERE id_должность = @iid " +
                                  "end ";

           ExecuteQuery.Execute(stringUpdate);
           this.Close();
        }
    }
}