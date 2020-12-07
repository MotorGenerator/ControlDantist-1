using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Classes;
using System.Data;
using System.Data.SqlClient;
using ControlDantist.ClassValidRegions;
using ControlDantist.Repository;
using System.Configuration;
using ControlDantist.ValidateMedicalServices;
using System.Net;
using System.Net.NetworkInformation;
using ControlDantist.Find;
using ControlDantist.DisplayRegistr;

namespace ControlDantist
{
    public partial class FindPersonESRN : Form
    {
        public FindPersonESRN()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Строки подключения к БД АИС ЭСРН.
            ConfigLibrary.Config config = new ConfigLibrary.Config();

            //Пока закоментирован.
            //Получаем словарь со строками подключения к АИС ЭСРН.
            Dictionary<string, string> pullConnect = config.Select();

            // Список с результатами поиска.
            List<DisplayPerson> list = new List<DisplayPerson>();


            // Пройдемся по районам.
            foreach (var scon in pullConnect)
            {
                string query = string.Empty;

                if (this.textBox1.Text.Length > 0 && this.textBox2.Text.Length > 0 && this.textBox3.Text.Length > 0)
                {
                    IFindPerson findPerson = new FindPersonESRN_FIO(this.textBox1.Text.Trim(), this.textBox2.Text.Trim(), this.textBox3.Text.Trim());
                    query = findPerson.Query();
                }
                else if(this.textBox1.Text.Length > 0 && this.textBox2.Text.Length > 0)
                {
                    IFindPerson findPerson = new FindPerson_FI(this.textBox1.Text.Trim(), this.textBox2.Text.Trim());
                    query = findPerson.Query();
                }

                if(scon.Key == "Энгельский")
                {
                    var test = "";
                }

                using (SqlConnection con = new SqlConnection(scon.Value))
                {
                    try
                    {
                        con.Open();

                        // Таблица с услугами которые совпали с сервером и с договором.
                        DataTable tServ = ТаблицаБД.GetTableSQL(query, "ФИО", con);

                        foreach (DataRow row in tServ.Rows)
                        {
                            DisplayPerson dp = new DisplayPerson();
                            dp.Фамилия = row["Фамилия"].ToString().Trim();
                            dp.Имя = row["Имя"].ToString().Trim();
                            dp.Отчество = row["Отчество"].ToString().Trim();
                            dp.ДатаРождения = Convert.ToDateTime(row["ДР"]).ToShortDateString().Trim();
                            dp.Район = scon.Key;
                            list.Add(dp);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Подключение к району - " + scon.Key + " отсутствует");
                    }
                }

            }

            // Выведим результат.
            this.dataGridView1.DataSource = list;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
