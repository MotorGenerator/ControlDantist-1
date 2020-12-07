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
    public partial class FormHeadDepartment : Form
    {
        private int id_начОтдел = 0;
        public FormHeadDepartment()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Length != 0 && this.textBox2.Text.Length != 0)
            {
                string фио = this.textBox1.Text.Trim() + " " + this.textBox3.Text.Trim();

                string queryInsert = "INSERT INTO ДолжностьАналитичУправ " +
                                     "([должность] " +
                                     ",[фамилия] " +
                                     ",[инициалы] " +
                                     ",фио_инициалы ) " +
                                     "VALUES " +
                                    "('" + this.textBox2.Text.Trim() + "' " +
                                    ",'" + this.textBox1.Text.Trim() + "' " +
                                    ",'"+ this.textBox3.Text.Trim() +"'"  +
                                    ",'" + фио.Trim() + "' )";

                //выполним инструкцию
                ExecuteQuery.Execute(queryInsert);

                //Обновим содержимое DataGrid
                UpdateDataGrid();
            }
        }

        private void FormHeadDepartment_Load(object sender, EventArgs e)
        {
            UpdateDataGrid();

            //DataTable tabДолжность;

            //using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
            //{
            //    //Откроем соединение
            //    con.Open();

            //    //Выполним всё в единой транзакции
            //    SqlTransaction transact = con.BeginTransaction();

            //    string queryTable = "select * from ДолжностьАналитичУправ ";
            //    tabДолжность = ТаблицаБД.GetTableSQL(queryTable, "ДолжностьАналитичУправ", con, transact);
            //}

            //this.dataGridView1.DataSource = tabДолжность;
            //this.dataGridView1.Columns["id_должность"].Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Получим id начальника
                id_начОтдел = Convert.ToInt32(this.dataGridView1.CurrentRow.Cells["id_должность"].Value);
            }
            catch
            {

            }

            //отобразим выделенного начальника отдела
            DataGridViewRow rowCurrent = this.dataGridView1.CurrentRow;
            this.textBox1.Text = rowCurrent.Cells["фамилия"].Value.ToString().Trim();

            this.textBox3.Text = rowCurrent.Cells["инициалы"].Value.ToString().Trim();
            this.textBox2.Text = rowCurrent.Cells["должность"].Value.ToString().Trim();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string фио = this.textBox1.Text.Trim() + this.textBox3.Text.Trim();
            string updateQuery = "UPDATE ДолжностьАналитичУправ " +
                                "SET [должность] = '"+ this.textBox2.Text.Trim() +"' " +
                                ",[фамилия] = '"+ this.textBox1.Text.Trim() +"' " +
                                ",[инициалы] = '"+ this.textBox3.Text.Trim() +"' " +
                                ",фио_инициалы  = '"+ фио.Trim() +"' " +
                                "WHERE [id_должность] = "+ id_начОтдел +" ";

            //выполним инструкцию
            ExecuteQuery.Execute(updateQuery);

            //Обновим содержимое DataGrid
            UpdateDataGrid();

        }

        /// <summary>
        /// Обновляет содержимое DataGrid
        /// </summary>
        private void UpdateDataGrid()
        {
            //Обновим содержимое DataGridView
            this.dataGridView1.DataSource = "";

            DataTable tabДолжность;

            using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
            {
                //Откроем соединение
                con.Open();

                //Выполним всё в единой транзакции
                SqlTransaction transact = con.BeginTransaction();

                string queryTable = "select * from ДолжностьАналитичУправ ";
                tabДолжность = ТаблицаБД.GetTableSQL(queryTable, "ДолжностьАналитичУправ", con, transact);
            }

            this.dataGridView1.DataSource = tabДолжность;
            this.dataGridView1.Columns["id_должность"].Visible = false;
            this.dataGridView1.Columns["фио_инициалы"].Visible = false;

            this.textBox1.Text = "";
            this.textBox3.Text = "";
            this.textBox2.Text = "";
        }

        private void btnDelet_Click(object sender, EventArgs e)
        {
            StringBuilder build = new StringBuilder();

            string deleteQuery1 = "DELETE FROM [ДолжностьАналитичУправ] " +
                                 "WHERE id_должность = " + id_начОтдел + " ";
            build.Append(deleteQuery1);

            string deleteQuery2 = " delete FROM ПодписьДолжность " +
                                  "where id_должность = "+ id_начОтдел +" ";
            build.Append(deleteQuery2);

            //выполним инструкцию
            ExecuteQuery.Execute(build.ToString());

            //Обновим содержимое DataGrid
            UpdateDataGrid();
        }

      
    }
}