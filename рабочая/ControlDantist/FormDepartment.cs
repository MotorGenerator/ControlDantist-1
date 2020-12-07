using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using ControlDantist.Classes;

namespace ControlDantist
{
    public partial class FormDepartment : Form
    {
        private int id = 0;
        public FormDepartment()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            Патаметры parm = new Патаметры();
            parm.НаименованиеТеррОргана = this.textBox1.Text.Trim();

            if (parm.НаименованиеТеррОргана != string.Empty)
            {
                string insertQuery = "INSERT INTO ТерриториальныйОрган " +
                                     "([НаименованиеТеррОргана] " +
                                     ",[ФИО] " +
                                     ",[Обращение] ) " +
                                     "VALUES " +
                                     "('" + parm.НаименованиеТеррОргана + "' " +
                                     ",'"+ this.textBox2.Text.Trim() +"' " +
                                     ",'"+ this.textBox3.Text +"' )";

                ExecuteQuery.Execute(insertQuery);
                LoadUpdate();

                this.textBox1.Text = "";
                this.textBox2.Text = "";
                this.textBox3.Text = "";
            }
            else
            {
                MessageBox.Show("Введите наименование терр органа");
            }
        }

        private void LoadUpdate()
        {
            string query = "select id_террОрган,НаименованиеТеррОргана,ФИО,Обращение from ТерриториальныйОрган";
            DataTable dt = ТаблицаБД.GetTableSQL(query, "ТерриториальныйОрган");

            this.dataGridView1.DataSource = dt;

            this.dataGridView1.Columns["id_террОрган"].Visible = false;
            this.dataGridView1.Columns["НаименованиеТеррОргана"].HeaderText = "Наименование ТО";
            this.dataGridView1.Columns["НаименованиеТеррОргана"].Visible = true;

            this.dataGridView1.Columns["ФИО"].Visible = false;
            this.dataGridView1.Columns["Обращение"].Visible = false;
            

        }

        struct Патаметры
        {
            private string наименовТеррОргана;

            /// <summary>
            /// Название территориального органа
            /// </summary>
            public string НаименованиеТеррОргана
            {
                get
                {
                    return наименовТеррОргана;
                }
                set
                {
                    наименовТеррОргана = value;
                }
            }

            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormDepartment_Load(object sender, EventArgs e)
        {
            LoadUpdate();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Патаметры parm = new Патаметры();
            parm.НаименованиеТеррОргана = this.textBox1.Text.Trim();

            string updateQuery = "UPDATE ТерриториальныйОрган " +
                                 "SET [НаименованиеТеррОргана] = '" + parm.НаименованиеТеррОргана + "' " +
                                 ",[ФИО] = '"+ this.textBox2.Text.Trim() +"' " +
                                 ",[Обращение] = '"+ this.textBox3.Text.Trim() +"' " +
                                 "WHERE id_террОрган = "+ id +" ";

            ExecuteQuery.Execute(updateQuery);
            LoadUpdate();

            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            id = 0;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Получим id Терр органа
            id = Convert.ToInt32(this.dataGridView1.CurrentRow.Cells["id_террОрган"].Value);

            //Получим название Терр органа
            string name = this.dataGridView1.CurrentRow.Cells["НаименованиеТеррОргана"].Value.ToString();
            this.textBox1.Text = name.Trim();

            //Получаем ФИО
            string fio = this.dataGridView1.CurrentRow.Cells["ФИО"].Value.ToString();
            this.textBox2.Text = fio.Trim();

            //Получаем Обращение
            string обращение = this.dataGridView1.CurrentRow.Cells["Обращение"].Value.ToString();
            this.textBox3.Text = обращение.Trim();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string deleteQuery = "delete from ТерриториальныйОрган " +
                                 "where id_террОрган = "+ id +" ";

            ExecuteQuery.Execute(deleteQuery);
            LoadUpdate();

            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            id = 0;
        }
    }
}