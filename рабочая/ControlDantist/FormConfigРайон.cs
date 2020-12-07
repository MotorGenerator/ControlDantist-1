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
    public partial class FormConfigРайон : Form
    {
        //Список хранит перечень id наименований районов
        private List<int> listId = new List<int>();

        public FormConfigРайон()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void FormConfigРайон_Load(object sender, EventArgs e)
        {

            LoadForm();
        }

        private void dtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            FormSelect fs = new FormSelect();
            fs.ShowDialog();

            if (fs.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                //получим id территориального органа
                int id_TO = fs.IdTO;

                //выполним обновление таблицы БД 
                string queryUpdate = "";

                int iCount = listId.Count;

                int countItem = 1;

                //сформируем список id
                StringBuilder build = new StringBuilder();
                foreach (int id in listId)
                {
                    if (countItem != iCount)
                    {
                        string sid = id.ToString() + ",";
                        build.Append(sid);
                    }
                    else
                    {
                        build.Append(id.ToString());
                    }

                    countItem++;
                }

                  string sTest = build.ToString();
                  string update = "UPDATE [НаименованиеРайона] " +
                                    "SET [id_террОргана] = " + id_TO + " " +
                                    "WHERE id_район in (" + sTest + ") ";

                  ExecuteQuery.Execute(update);
                  LoadForm();


                //Обнуляем список
                  listId.Clear();
            }


            //StringBuilder builder = new StringBuilder();

            //foreach (DataGridViewRow row in this.dataGridView1.Rows)
            //{
            //    if (Convert.ToBoolean(row.Cells["Flag"].Value) == true)
            //    {
            //        //получим id_район
            //        int id_район = Convert.ToInt32(row.Cells["Id"].Value);

            //        //получим id района терр органа
            //        int id_районТО = Convert.ToInt32(row.Cells["НаименованиеТО"].Value);

            //        string update = "UPDATE [НаименованиеРайона] " +
            //                        "SET [id_террОргана] = "+ id_районТО +" " +
            //                        "WHERE id_район = "+ id_район +" ";
                    
            //        builder.Append(update);

            //    }
            //}

            //Выполним всё в единой транзакции
           // ExecuteQuery.Execute(builder.ToString());
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Получим id района
            int id_район = Convert.ToInt32(this.dataGridView1.CurrentRow.Cells["Id"].Value);

            //Создадим список id_районов
            listId.Add(id_район);

        }


        /// <summary>
        /// Загружает в форму данные
        /// </summary>
        private void LoadForm()
        {
            string queryLoad = "select id_район,РайонОбласти,id_террОргана from НаименованиеРайона";
            DataTable dtРайон = ТаблицаБД.GetTableSQL(queryLoad, "НаименованиеРайона");

            List<НаименованиеРайона> list = new List<НаименованиеРайона>();

            foreach (DataRow row in dtРайон.Rows)
            {
                НаименованиеРайона район = new НаименованиеРайона();
                район.Id = Convert.ToInt32(row["id_район"]);
                район.Наименование = row["РайонОбласти"].ToString().Trim();

                if (row["id_террОргана"] != DBNull.Value)
                {
                    район.Flag = true;
                    район.IdТеррОргана = Convert.ToInt32(row["id_террОргана"]);

                    string to = "select НаименованиеТеррОргана from dbo.ТерриториальныйОрган " +
                                "where id_террОрган = "+ район.IdТеррОргана +" ";

                    string nameТеррОрган = ТаблицаБД.GetTableSQL(to, "ТерриториальныйОрган").Rows[0]["НаименованиеТеррОргана"].ToString();
                    район.НазваниеТеррОргана = nameТеррОрган;
                }
                else
                {
                    район.Flag = false;
                }

                list.Add(район);
            }

            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = list;

            this.dataGridView1.Columns["Id"].Visible = false;
            //this.dataGridView1.Columns["Id"].DisplayIndex = ;

            this.dataGridView1.Columns["Наименование"].Visible = true;
            this.dataGridView1.Columns["Наименование"].HeaderText = "Район";
            this.dataGridView1.Columns["Наименование"].DisplayIndex = 1;

            this.dataGridView1.Columns["Flag"].Visible = true;
            this.dataGridView1.Columns["Flag"].HeaderText = "Соответствие";
            this.dataGridView1.Columns["Flag"].DisplayIndex = 2;

            this.dataGridView1.Columns["IdТеррОргана"].Visible = false;
            //this.dataGridView1.Columns["IdТеррОргана"].HeaderText = "Соответствие";
            this.dataGridView1.Columns["IdТеррОргана"].DisplayIndex = 3;
        }

  
    }
}