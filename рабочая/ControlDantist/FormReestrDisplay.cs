using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Classes;
using System.Linq;
using ControlDantist.Reports;
using DantistLibrary;

namespace ControlDantist
{
    public partial class FormReestrDisplay : Form
    {

        public bool FlagLetter { get; set; }
        public string Date { get; set; }

        public FormReestrDisplay()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Заполняет список с годами реализации.
        private List<int> LoadYear()
        {
            // Узнаем текущий год.
            int year = DateTime.Today.Year;

            List<int> listYear = new List<int>();

            do
            {
                listYear.Add(year);

                year--;
            }
            while (year >= 2013);

            return listYear;
        }

        private void FormReestrDisplay_Load(object sender, EventArgs e)
        {
            this.label4.Visible = false;

           // this.listYear.DataSource = LoadYear();

            if (this.FlagLetter == true)
            {
                this.dtnWrite.Enabled = false;
                this.dateTimePicker1.Visible = false;
                this.label2.Visible = false;

                LoadUpdate();
            }
            else
            {
                this.btnPoust.Visible = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Обновим запись
            //LoadUpdate();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           //LoadUpdate();
        }

        private void dtnWrite_Click(object sender, EventArgs e)
        {
            //Запишем кто вносил изменения
            string logWrite = MyAplicationIdentity.GetUses();

            //Текущий день
            DateTime dt = DateTime.Today;

            //Выполним запрос в единой транзакции
            StringBuilder builder = new StringBuilder();
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                int id = Convert.ToInt32(row.Cells["id_акт"].Value);

                string query = "UPDATE АктВыполненныхРабот " +
                               "SET [ДатаОплаты] = '" + this.dateTimePicker1.Value.ToShortDateString().Trim() + "' " +
                               ",[logWrite] = '" + logWrite.Trim() +"' " +
                               ",[logDate] = '"+ dt.ToShortDateString() +"' " +
                               "WHERE id_акт = "+ id +" ";

                builder.Append(query);
            }

            //Выполним запрос на обновление таблицы
            ExecuteQuery.Execute(builder.ToString());
            //this.Close();

            LoadUpdate();
        }

        /// <summary>
        /// Загружает обновление
        /// </summary>
        private void LoadUpdate()
        {
            string num_счётФактура;
            string unmReestr;

            // Флаг счет фактуры.
            bool flagCF = false;

            // Флаг реестра.
            bool flagReestr = false;

            // Флаг указывает на поиск и по номеру реестра и по номеру счёт-фактуры.
            bool flagFullFind = false;

            if (this.textBox1.Text.Trim() != "")
            {
                unmReestr = this.textBox1.Text.Trim();

                flagReestr = true;
            }
            else
            {
                unmReestr = "";
            }

            if (this.textBox2.Text.Trim() != "")
            {
                num_счётФактура = this.textBox2.Text.Trim();

                flagCF = true;
            }
            else
            {
                num_счётФактура = "";
            }


            string query = string.Empty;

            if (this.checkBox1.Checked == true)
            {
                flagFullFind = true;
            }

            if (this.FlagLetter == false)
            {

                if (flagFullFind == false)
                {
                    // поиск реестра по номеру и по дате
                    query = "SELECT АктВыполненныхРабот.id_акт,АктВыполненныхРабот.НомерАкта,АктВыполненныхРабот.НомерРеестра,АктВыполненныхРабот.НомерСчётФактуры,Договор.id_договор, Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, Льготник.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория, sum(УслугиПоДоговору.Сумма) as 'Сумма',Договор.ДатаЗаписиДоговора,АктВыполненныхРабот.ДатаОплаты,АктВыполненныхРабот.logWrite,АктВыполненныхРабот.logDate,dbo.Льготник.id_льготник,dbo.Льготник.id_район,dbo.Льготник.id_насПункт,Льготник.улица,Льготник.НомерДома,Льготник.корпус,Льготник.НомерКвартиры  FROM Договор " +
                                   "INNER JOIN Льготник ON  " +
                                   "dbo.Договор.id_льготник = dbo.Льготник.id_льготник  " +
                                   "INNER JOIN dbo.ЛьготнаяКатегория ON " +
                                   "dbo.Льготник.id_льготнойКатегории = dbo.ЛьготнаяКатегория.id_льготнойКатегории  " +
                                   "INNER JOIN dbo.УслугиПоДоговору ON  " +
                                   "dbo.Договор.id_договор = dbo.УслугиПоДоговору.id_договор  " +
                                   "INNER JOIN АктВыполненныхРабот " +
                                   "ON dbo.Договор.id_договор = АктВыполненныхРабот.id_договор " +
                                   "where Договор.ФлагПроверки = 'True' and (АктВыполненныхРабот.ДатаСчётФактуры >= '" + Время.Дата(this.dateTimePicker1.Value.ToShortDateString()) + "' and АктВыполненныхРабот.ДатаСчётФактуры <= '" + Время.Дата(this.dateTimePicker2.Value.ToShortDateString()) + "' ) " + 
                                   //" (АктВыполненныхРабот.НомерРеестра = '" + unmReestr + "') " +
                                   "and ((АктВыполненныхРабот.НомерРеестра = '" + unmReestr + "') or (АктВыполненныхРабот.НомерСчётФактуры = '" + num_счётФактура + "')) " +
                                   "Group by АктВыполненныхРабот.id_акт,Договор.id_договор,Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, Льготник.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория,Договор.ДатаЗаписиДоговора,АктВыполненныхРабот.НомерАкта,АктВыполненныхРабот.НомерРеестра,АктВыполненныхРабот.НомерСчётФактуры,АктВыполненныхРабот.ДатаОплаты,АктВыполненныхРабот.logWrite,АктВыполненныхРабот.logDate,dbo.Льготник.id_льготник,dbo.Льготник.id_район,dbo.Льготник.id_насПункт,Льготник.улица,Льготник.НомерДома,Льготник.корпус,Льготник.НомерКвартиры ";
                }
                else 
                {
                    query = "";
                }

            }

            if (this.FlagLetter == true)
            {
                query = "SELECT АктВыполненныхРабот.id_акт,АктВыполненныхРабот.НомерАкта,АктВыполненныхРабот.НомерРеестра,АктВыполненныхРабот.НомерСчётФактуры,Договор.id_договор, Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, Льготник.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория, sum(УслугиПоДоговору.Сумма) as 'Сумма',Договор.ДатаЗаписиДоговора,АктВыполненныхРабот.ДатаОплаты,АктВыполненныхРабот.logWrite,АктВыполненныхРабот.logDate,dbo.Льготник.id_льготник,dbo.Льготник.id_район,dbo.Льготник.id_насПункт,Льготник.улица,Льготник.НомерДома,Льготник.корпус,Льготник.НомерКвартиры  FROM Договор " +
                         "INNER JOIN Льготник ON  " +
                         "dbo.Договор.id_льготник = dbo.Льготник.id_льготник  " +
                         "INNER JOIN dbo.ЛьготнаяКатегория ON " +
                         "dbo.Льготник.id_льготнойКатегории = dbo.ЛьготнаяКатегория.id_льготнойКатегории  " +
                         "INNER JOIN dbo.УслугиПоДоговору ON  " +
                         "dbo.Договор.id_договор = dbo.УслугиПоДоговору.id_договор  " +
                         "INNER JOIN АктВыполненныхРабот " +
                         "ON dbo.Договор.id_договор = АктВыполненныхРабот.id_договор " +
                         ////"INNER JOIN НаселённыйПункт ON " +
                         ////"dbo.Льготник.id_насПункт = НаселённыйПункт.id_насПункт " +
                         //// "INNER JOIN НаименованиеРайона ON " +
                         //// "dbo.Льготник.id_район = НаименованиеРайона.id_район " +
                         //"where Договор.ФлагПроверки = 'True' and ((АктВыполненныхРабот.НомерРеестра = '" + unmReestr + "') or (АктВыполненныхРабот.НомерСчётФактуры = '" + num_счётФактура + "')) " +
                         "where Договор.ФлагПроверки = 'True' and АктВыполненныхРабот.ДатаОплаты = '"+ this.Date +"' " +
                         "Group by АктВыполненныхРабот.id_акт,Договор.id_договор,Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, Льготник.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория,Договор.ДатаЗаписиДоговора,АктВыполненныхРабот.НомерАкта,АктВыполненныхРабот.НомерРеестра,АктВыполненныхРабот.НомерСчётФактуры,АктВыполненныхРабот.ДатаОплаты,АктВыполненныхРабот.logWrite,АктВыполненныхРабот.logDate,dbo.Льготник.id_льготник,dbo.Льготник.id_район,dbo.Льготник.id_насПункт,Льготник.улица,Льготник.НомерДома,Льготник.корпус,Льготник.НомерКвартиры ";

            }

            using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
            {
                con.Open();
                SqlTransaction transact = con.BeginTransaction();

                //DataTable dtContract = ТаблицаБД.GetTableSQL(query, "ТаблицаДоговоров", con, transact);

                string iTestQuery = query;

                SqlCommand com = new SqlCommand(query, con);
                com.Transaction = transact;


                DataTable tab = new DataTable();
                DataColumn col0 = new DataColumn("id_акт", typeof(int));
                tab.Columns.Add(col0);
                DataColumn col1 = new DataColumn("НомерАкта", typeof(string));
                tab.Columns.Add(col1);
                DataColumn col2 = new DataColumn("НомерРеестра", typeof(string));
                tab.Columns.Add(col2);
                DataColumn col3 = new DataColumn("НомерСчётФактуры", typeof(string));
                tab.Columns.Add(col3);
                DataColumn col4 = new DataColumn("id_договор", typeof(int));
                tab.Columns.Add(col4);
                DataColumn col5 = new DataColumn("НомерДоговора", typeof(string));
                tab.Columns.Add(col5);
                DataColumn col6 = new DataColumn("Фамилия", typeof(string));
                tab.Columns.Add(col6);
                DataColumn col7 = new DataColumn("Имя", typeof(string));
                tab.Columns.Add(col7);
                DataColumn col8 = new DataColumn("Отчество", typeof(string));
                tab.Columns.Add(col8);
                DataColumn col9 = new DataColumn("ЛьготнаяКатегория", typeof(string));
                tab.Columns.Add(col9);
                DataColumn col10 = new DataColumn("Сумма", typeof(string));
                tab.Columns.Add(col10);
                DataColumn col11 = new DataColumn("ДатаОплаты", typeof(string));
                tab.Columns.Add(col11);

                if (this.FlagLetter == false)
                {
                    DataColumn col12 = new DataColumn("logWrite", typeof(string));
                    tab.Columns.Add(col12);
                    DataColumn col13 = new DataColumn("logDate", typeof(string));
                    tab.Columns.Add(col13);
                }

                //Наименование населённого пункта
                DataColumn col14 = new DataColumn("Наименование", typeof(string));
                tab.Columns.Add(col14);

                DataColumn col15 = new DataColumn("Адрес", typeof(string));
                tab.Columns.Add(col15);

                DataColumn col16 = new DataColumn("Район", typeof(string));
                tab.Columns.Add(col16);


                int iCountD = 0;

                SqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    DataRow row = tab.NewRow();
                    row["id_акт"] = read["id_акт"].ToString().Trim();

                    row["id_договор"] = read["id_договор"].ToString().Trim();
                    row["НомерАкта"] = read["НомерАкта"].ToString().Trim();
                    row["НомерРеестра"] = read["НомерРеестра"].ToString().Trim();

                    row["НомерСчётФактуры"] = read["НомерСчётФактуры"].ToString().Trim();
                    row["НомерДоговора"] = read["НомерДоговора"].ToString().Trim();
                    row["Фамилия"] = read["Фамилия"].ToString().Trim();

                    row["Имя"] = read["Имя"].ToString().Trim();
                    row["Отчество"] = read["Отчество"].ToString().Trim();
                    row["ЛьготнаяКатегория"] = read["ЛьготнаяКатегория"].ToString().Trim();

                    row["Сумма"] = Convert.ToDecimal(read["Сумма"]).ToString("c").Trim();
                    if (read["ДатаОплаты"] != DBNull.Value)
                    {
                        row["ДатаОплаты"] = read["ДатаОплаты"].ToString().Trim();
                    }

                    if (this.FlagLetter == false)
                    {
                        row["logWrite"] = read["logWrite"].ToString().Trim();
                        row["logDate"] = read["logDate"].ToString().Trim();
                    }

                    int id_район = 0;
                    int id_насПункт = 0;

                    if (read["id_район"] != DBNull.Value)
                    {
                        id_район = Convert.ToInt32(read["id_район"]);

                        string queryR = "SELECT РайонОбласти FROM НаименованиеРайона where id_район = "+ id_район +" ";

                        DataTable tabРайон = ТаблицаБД.GetTableSQL(queryR, "НазваниеРайона");
                        if (tabРайон.Rows.Count != 0)
                        {
                            string район = ТаблицаБД.GetTableSQL(queryR, "НазваниеРайона").Rows[0][0].ToString().Trim();
                            row["Район"] = район;
                        }
                        

                    }

                    if (read["id_насПункт"] != DBNull.Value)
                    {
                        id_насПункт = Convert.ToInt32(read["id_насПункт"]);

                        string queryA = "SELECT Наименование FROM НаселённыйПункт where id_насПункт = " + id_насПункт + " ";
                        DataTable tabГород = ТаблицаБД.GetTableSQL(queryA, "НаселённыйПункт");

                        if (tabГород.Rows.Count != 0)
                        {
                            string город = ТаблицаБД.GetTableSQL(queryA, "НаселённыйПункт").Rows[0][0].ToString().Trim();
                            row["Наименование"] = город;
                        }

                    }




                    //Получим данные об адресе проживания льготника


                    //row["Наименование"] = read["Наименование"].ToString().Trim();

                    ////,Льготник.улица,Льготник.НомерДома,Льготник.корпус,Льготник.НомерКвартиры
                    StringBuilder buldAdr = new StringBuilder();
                    buldAdr.Append(read["улица"].ToString().Trim() + " ");

                    buldAdr.Append(read["НомерДома"].ToString().Trim() + " ");
                    //buldAdr.Append("корп. " + read["корпус"].ToString().Trim() + " ");

                    buldAdr.Append(read["корпус"].ToString().Trim() + " ");
                    buldAdr.Append(read["НомерКвартиры"].ToString().Trim());

                    row["Адрес"] = buldAdr.ToString().Trim();

                    //// район области
                    //row["Район"] = read["РайонОбласти"].ToString().Trim();

                    ////if (read["ДатаОплаты"] == "1900-01-01")
                    ////{
                    ////    row["ДатаОплаты"] = ""; 
                    ////}
                    ////else
                    ////{
                    ////    row["ДатаОплаты"] = Convert.ToDateTime(read["ДатаОплаты"]).ToShortDateString().Trim();
                    ////}

                    ////if (read["logWrite"] != DBNull.Value)
                    ////{
                    ////    row["КтоЗаписал"] = read["logWrite"].ToString().Trim();
                    ////}
                    tab.Rows.Add(row);

                    iCountD++;
                }




                this.dataGridView1.DataSource = tab;

                this.dataGridView1.Columns["id_акт"].Width = 100;
                this.dataGridView1.Columns["id_акт"].Visible = false;

                this.dataGridView1.Columns["id_договор"].Width = 100;
                this.dataGridView1.Columns["id_договор"].Visible = false;

                this.dataGridView1.Columns["НомерАкта"].Width = 100;
                this.dataGridView1.Columns["НомерАкта"].DisplayIndex = 1;
                this.dataGridView1.Columns["НомерАкта"].HeaderText = "№ Акта";

                this.dataGridView1.Columns["НомерРеестра"].Width = 100;
                this.dataGridView1.Columns["НомерРеестра"].DisplayIndex = 2;
                this.dataGridView1.Columns["НомерРеестра"].HeaderText = "№ Реестра";

                this.dataGridView1.Columns["НомерСчётФактуры"].Width = 100;
                this.dataGridView1.Columns["НомерСчётФактуры"].DisplayIndex = 3;
                this.dataGridView1.Columns["НомерСчётФактуры"].HeaderText = "№ Счёт-фактуры";

                this.dataGridView1.Columns["НомерДоговора"].Width = 100;
                this.dataGridView1.Columns["НомерДоговора"].DisplayIndex = 4;
                this.dataGridView1.Columns["НомерДоговора"].HeaderText = "№ Договора";

                this.dataGridView1.Columns["Фамилия"].Width = 200;
                this.dataGridView1.Columns["Фамилия"].DisplayIndex = 5;

                this.dataGridView1.Columns["Имя"].Width = 200;
                this.dataGridView1.Columns["Имя"].DisplayIndex = 6;

                this.dataGridView1.Columns["Отчество"].Width = 200;
                this.dataGridView1.Columns["Отчество"].DisplayIndex = 7;

                this.dataGridView1.Columns["ЛьготнаяКатегория"].Width = 300;
                this.dataGridView1.Columns["ЛьготнаяКатегория"].DisplayIndex = 8;

                this.dataGridView1.Columns["Сумма"].Width = 200;
                this.dataGridView1.Columns["Сумма"].DisplayIndex = 9;

                this.dataGridView1.Columns["ДатаОплаты"].Width = 70;
                this.dataGridView1.Columns["ДатаОплаты"].DisplayIndex = 10;

                if (this.FlagLetter == false)
                {
                    this.dataGridView1.Columns["logWrite"].Width = 70;
                    this.dataGridView1.Columns["logWrite"].DisplayIndex = 11;
                    this.dataGridView1.Columns["logWrite"].HeaderText = "Кто записал";

                    this.dataGridView1.Columns["logDate"].Width = 70;
                    this.dataGridView1.Columns["logDate"].DisplayIndex = 12;
                    this.dataGridView1.Columns["logDate"].HeaderText = "Дата записи";
                

                    //Проверим если в поле logDate указано время то кнопку сохранения записи сделаем не активной
                    foreach (DataGridViewRow row in this.dataGridView1.Rows)
                    {
                        if (row.Cells["logDate"].Value != "")
                        {
                            this.dtnWrite.Enabled = true;
                        }
                        else
                        {
                            this.dtnWrite.Enabled = false;
                        }
                    }

                }

                //Наименование населённого пункта
                this.dataGridView1.Columns["Наименование"].Width = 70;
                this.dataGridView1.Columns["Наименование"].DisplayIndex = 13;
                this.dataGridView1.Columns["Наименование"].HeaderText = "Населённый пункт";

                // Адрес проживания льготника
                this.dataGridView1.Columns["Адрес"].Width = 70;
                this.dataGridView1.Columns["Адрес"].DisplayIndex = 14;
                this.dataGridView1.Columns["Адрес"].HeaderText = "Адрес";

                // Район области в котором проживает льготник
                this.dataGridView1.Columns["Район"].Width = 70;
                this.dataGridView1.Columns["Район"].DisplayIndex = 15;
                this.dataGridView1.Columns["Район"].HeaderText = "Район";

                //Выведим количество договоров на экран
                this.label4.Visible = true;
                this.label4.Text = String.Format("Количество договоров = {0}",iCountD);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //if (this.checkBox1.Checked == true)
            //{
            //    LoadUpdate(this.dateTimePicker1.Value.ToShortDateString());
            //}
            
        }

        private void LoadUpdate(string date)
        {
            string num_счётФактура;
            string unmReestr;

            if (this.textBox1.Text.Trim() != "")
            {
                unmReestr = this.textBox1.Text.Trim();
            }
            else
            {
                unmReestr = "";
            }

            if (this.textBox2.Text.Trim() != "")
            {
                num_счётФактура = this.textBox2.Text.Trim();
            }
            else
            {
                num_счётФактура = "";
            }

            //поиск реестра по номеру и по дате
            string query = "SELECT АктВыполненныхРабот.id_акт,АктВыполненныхРабот.НомерАкта,АктВыполненныхРабот.НомерРеестра,АктВыполненныхРабот.НомерСчётФактуры,Договор.id_договор, Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, Льготник.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория, sum(УслугиПоДоговору.Сумма) as 'Сумма',Договор.ДатаЗаписиДоговора,АктВыполненныхРабот.ДатаОплаты,АктВыполненныхРабот.logWrite,АктВыполненныхРабот.logDate  FROM Договор " +
                            "INNER JOIN Льготник ON  " +
                            "dbo.Договор.id_льготник = dbo.Льготник.id_льготник  " +
                            "INNER JOIN dbo.ЛьготнаяКатегория ON " +
                            "dbo.Льготник.id_льготнойКатегории = dbo.ЛьготнаяКатегория.id_льготнойКатегории  " +
                            "INNER JOIN dbo.УслугиПоДоговору ON  " +
                            "dbo.Договор.id_договор = dbo.УслугиПоДоговору.id_договор  " +
                            "INNER JOIN АктВыполненныхРабот " +
                            "ON dbo.Договор.id_договор = АктВыполненныхРабот.id_договор " +
                            "where Договор.ФлагПроверки = 'True' and Договор.ДатаЗаписиДоговора = '" + date + "' " + //((АктВыполненныхРабот.НомерРеестра = '" + unmReestr + "') or (АктВыполненныхРабот.НомерСчётФактуры = '" + num_счётФактура + "')) " +
                            "Group by АктВыполненныхРабот.id_акт,Договор.id_договор,Договор.НомерДоговора, Льготник.Фамилия, Льготник.Имя, Льготник.Отчество, ЛьготнаяКатегория.ЛьготнаяКатегория,Договор.ДатаЗаписиДоговора,АктВыполненныхРабот.НомерАкта,АктВыполненныхРабот.НомерРеестра,АктВыполненныхРабот.НомерСчётФактуры,АктВыполненныхРабот.ДатаОплаты,АктВыполненныхРабот.logWrite,АктВыполненныхРабот.logDate  ";

            //DataTable dtContract = ТаблицаБД.GetTableSQL(query, "РеестрДоговоров");


            using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
            {
                con.Open();
                SqlTransaction transact = con.BeginTransaction();

                //DataTable dtContract = ТаблицаБД.GetTableSQL(query, "ТаблицаДоговоров", con, transact);
                SqlCommand com = new SqlCommand(query, con);
                com.Transaction = transact;


                DataTable tab = new DataTable();
                DataColumn col0 = new DataColumn("id_акт", typeof(int));
                tab.Columns.Add(col0);
                DataColumn col1 = new DataColumn("НомерАкта", typeof(string));
                tab.Columns.Add(col1);
                DataColumn col2 = new DataColumn("НомерРеестра", typeof(string));
                tab.Columns.Add(col2);
                DataColumn col3 = new DataColumn("НомерСчётФактуры", typeof(string));
                tab.Columns.Add(col3);
                DataColumn col4 = new DataColumn("id_договор", typeof(int));
                tab.Columns.Add(col4);
                DataColumn col5 = new DataColumn("НомерДоговора", typeof(string));
                tab.Columns.Add(col5);
                DataColumn col6 = new DataColumn("Фамилия", typeof(string));
                tab.Columns.Add(col6);
                DataColumn col7 = new DataColumn("Имя", typeof(string));
                tab.Columns.Add(col7);
                DataColumn col8 = new DataColumn("Отчество", typeof(string));
                tab.Columns.Add(col8);
                DataColumn col9 = new DataColumn("ЛьготнаяКатегория", typeof(string));
                tab.Columns.Add(col9);
                DataColumn col10 = new DataColumn("Сумма", typeof(string));
                tab.Columns.Add(col10);
                DataColumn col11 = new DataColumn("ДатаОплаты", typeof(string));
                tab.Columns.Add(col11);
                DataColumn col12 = new DataColumn("logWrite", typeof(string));
                tab.Columns.Add(col12);
                DataColumn col13 = new DataColumn("logDate", typeof(string));
                tab.Columns.Add(col13);


                SqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    DataRow row = tab.NewRow();
                    row["id_акт"] = read["id_акт"].ToString().Trim();

                    row["id_договор"] = read["id_договор"].ToString().Trim();
                    row["НомерАкта"] = read["НомерАкта"].ToString().Trim();
                    row["НомерРеестра"] = read["НомерРеестра"].ToString().Trim();

                    row["НомерСчётФактуры"] = read["НомерСчётФактуры"].ToString().Trim();
                    row["НомерДоговора"] = read["НомерДоговора"].ToString().Trim();
                    row["Фамилия"] = read["Фамилия"].ToString().Trim();

                    row["Имя"] = read["Имя"].ToString().Trim();
                    row["Отчество"] = read["Отчество"].ToString().Trim();
                    row["ЛьготнаяКатегория"] = read["ЛьготнаяКатегория"].ToString().Trim();

                    row["Сумма"] = Convert.ToDecimal(read["Сумма"]).ToString("c").Trim();
                    if (read["ДатаОплаты"] != DBNull.Value)
                    {
                        row["ДатаОплаты"] = read["ДатаОплаты"].ToString().Trim();
                    }

                    row["logWrite"] = read["logWrite"].ToString().Trim();
                    row["logDate"] = read["logDate"].ToString().Trim();

                    //if (read["ДатаОплаты"] == "1900-01-01")
                    //{
                    //    row["ДатаОплаты"] = ""; 
                    //}
                    //else
                    //{
                    //    row["ДатаОплаты"] = Convert.ToDateTime(read["ДатаОплаты"]).ToShortDateString().Trim();
                    //}

                    //if (read["logWrite"] != DBNull.Value)
                    //{
                    //    row["КтоЗаписал"] = read["logWrite"].ToString().Trim();
                    //}
                    tab.Rows.Add(row);
                }


                this.dataGridView1.DataSource = tab;

                this.dataGridView1.Columns["id_акт"].Width = 100;
                this.dataGridView1.Columns["id_акт"].Visible = false;

                this.dataGridView1.Columns["id_договор"].Width = 100;
                this.dataGridView1.Columns["id_договор"].Visible = false;

                this.dataGridView1.Columns["НомерАкта"].Width = 100;
                this.dataGridView1.Columns["НомерАкта"].DisplayIndex = 1;
                this.dataGridView1.Columns["НомерАкта"].HeaderText = "№ Акта";

                this.dataGridView1.Columns["НомерРеестра"].Width = 100;
                this.dataGridView1.Columns["НомерРеестра"].DisplayIndex = 2;
                this.dataGridView1.Columns["НомерРеестра"].HeaderText = "№ Реестра";

                this.dataGridView1.Columns["НомерСчётФактуры"].Width = 100;
                this.dataGridView1.Columns["НомерСчётФактуры"].DisplayIndex = 3;
                this.dataGridView1.Columns["НомерСчётФактуры"].HeaderText = "№ Счёт-фактуры";

                this.dataGridView1.Columns["НомерДоговора"].Width = 100;
                this.dataGridView1.Columns["НомерДоговора"].DisplayIndex = 4;
                this.dataGridView1.Columns["НомерДоговора"].HeaderText = "№ Договора";

                this.dataGridView1.Columns["Фамилия"].Width = 200;
                this.dataGridView1.Columns["Фамилия"].DisplayIndex = 5;

                this.dataGridView1.Columns["Имя"].Width = 200;
                this.dataGridView1.Columns["Имя"].DisplayIndex = 6;

                this.dataGridView1.Columns["Отчество"].Width = 200;
                this.dataGridView1.Columns["Отчество"].DisplayIndex = 7;

                this.dataGridView1.Columns["ЛьготнаяКатегория"].Width = 300;
                this.dataGridView1.Columns["ЛьготнаяКатегория"].DisplayIndex = 8;

                this.dataGridView1.Columns["Сумма"].Width = 200;
                this.dataGridView1.Columns["Сумма"].DisplayIndex = 9;

                this.dataGridView1.Columns["ДатаОплаты"].Width = 70;
                this.dataGridView1.Columns["ДатаОплаты"].DisplayIndex = 10;

                this.dataGridView1.Columns["logWrite"].Width = 70;
                this.dataGridView1.Columns["logWrite"].DisplayIndex = 11;
                this.dataGridView1.Columns["logWrite"].HeaderText = "Кто записал";

                this.dataGridView1.Columns["logDate"].Width = 70;
                this.dataGridView1.Columns["logDate"].DisplayIndex = 12;
                this.dataGridView1.Columns["logDate"].HeaderText = "Дата записи";


                ////Проверим если в поле logDate указано время то кнопку сохранения записи сделаем не активной
                //foreach (DataGridViewRow row in this.dataGridView1.Rows)
                //{
                //    if (row.Cells["logDate"].Value == "")
                //    {
                //        this.dtnWrite.Enabled = false;
                //    }
                //}


            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.checkBox1.Checked == true)
            //{
            //    dtnWrite.Enabled = false;
            //}
            //else
            //{
            //    dtnWrite.Enabled = true;
            //}
        }

        private void btnPoust_Click(object sender, EventArgs e)
        {
            List<ControlDantist.Classes.Letter> list = new List<ControlDantist.Classes.Letter>();

            using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
            {
                con.Open();
                SqlTransaction transact = con.BeginTransaction();

                //количество строк
                int countRow = this.dataGridView1.Rows.Count;

                //счётчик строк
                int cRow = 1;

                StringBuilder sTest = new StringBuilder();

                //Пойдёмся по компоненту DataGridView
                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    ControlDantist.Classes.Letter let = new ControlDantist.Classes.Letter();

                    let.IdАкт = Convert.ToInt32(row.Cells["id_акт"].Value);

                    if (countRow != cRow)
                    {
                        //Найдём наименование комитета, ФИО руководителя и обращение (Уважаемый Леонид Ильич)
                        string queryIdРайон = "select * from dbo.ТерриториальныйОрган " +
                                              "where id_террОрган in ( " +
                                              "select id_террОргана from dbo.НаименованиеРайона " +
                                              "where id_район in ( " +
                                              "select id_район from dbo.Льготник " +
                                              "where id_льготник in ( " +
                                              "select id_льготник from dbo.Договор " +
                                              "where id_договор in ( " +
                                              "select id_договор from dbo.АктВыполненныхРабот " +
                                              "where id_акт = " + Convert.ToInt32(row.Cells["id_акт"].Value) + ")))) ";

                        sTest.Append(queryIdРайон);

                        DataTable tabРайон = ТаблицаБД.GetTableSQL(queryIdРайон, "ТеррОрган", con, transact);

                        if (tabРайон.Rows.Count != 0)
                        {
                            //Запишем наименование ограна
                            let.Комитет = tabРайон.Rows[0]["НаименованиеТеррОргана"].ToString().Trim();

                            //Запишем ФИо начальника
                            let.Начальник = tabРайон.Rows[0]["ФИО"].ToString().Trim();

                            //Запишем обращение
                            let.Обращение = tabРайон.Rows[0]["Обращение"].ToString().Trim();
                        }
                        else
                        {
                            //Запишем наименование ограна
                            let.Комитет = "ГКО СО " + " КСПН по Саратовскому району "+" ";

                            //Запишем ФИо начальника
                            let.Начальник = "Е.И Бакаеву";

                            //Запишем обращение
                            let.Обращение = "Уважаемый Евгений Иванович!";

                        }

                       

                        list.Add(let);

                    }

                    cRow++;

                }

                foreach(ControlDantist.Classes.Letter ln in list)
                {
                    string query = "SELECT * FROM [View_4] where id_акт = "+ ln.IdАкт +" ";
                    DataTable tabАкт = ТаблицаБД.GetTableSQL(query,"View");

                    foreach(DataRow r in tabАкт.Rows)
                    {
                        string фио = r["Фамилия"].ToString().Trim() + " " + r["Имя"].ToString().Trim() + " " + r["Отчество"].ToString().Trim();
                        ln.Фио = фио;

                        string корп = string.Empty;
                        if (r["корпус"].ToString().Trim() != "")
                        {
                            корп = "корп. " + r["корпус"].ToString().Trim();
                        }

                        string кв = string.Empty;
                        if (r["корпус"].ToString().Trim() != "")
                        {
                            кв = " кв. " + r["НомерКвартиры"].ToString().Trim();
                        }


                        string адрес = "ул. " + r["улица"].ToString().Trim() + "  д. " + r["НомерДома"].ToString().Trim() + " " + корп + " " + кв;
                        ln.Адрес = адрес;

                        string numDoc = r["СерияДокумента"].ToString().Trim() + " " + r["НомерДокумента"].ToString().Trim();
                        ln.НомерДокумента = numDoc;

                        string summ = Convert.ToDecimal(r["СуммаАктаВыполненныхРабот"]).ToString("c");
                        ln.CуммаАкта = summ;
                    }

                }


                List<ControlDantist.Classes.Letter> iList = list;

                //Пройдемся по таблице с адресами комитета

                string queryTO = "SELECT НаименованиеТеррОргана  FROM ТерриториальныйОрган";
                DataTable tabТеррОрган = ТаблицаБД.GetTableSQL(queryTO, "ТерриториальныйОрган");

                foreach (DataRow row in tabТеррОрган.Rows)
                {
                    string названиеТеррОргана = row["НаименованиеТеррОргана"].ToString();

                    IEnumerable<ControlDantist.Classes.Letter> listLetter = list.Where(lst => lst.Комитет == названиеТеррОргана).Select(lst => lst);
                    foreach (ControlDantist.Classes.Letter letPrint in listLetter)
                    {

                    }
                }



            }
        


            ////Пойдёмся по компоненту DataGridView
            //foreach (DataGridViewRow row in this.dataGridView1.Rows)
            //{

            //    int i = Convert.ToInt32(row.Cells["id_акт"].Value);

            //    string query = "SELECT     ТерриториальныйОрган_1.НаименованиеТеррОргана AS 'Терр орагн', derivedtbl_1.ФИО, derivedtbl_3.Обращение, derivedtbl_2.Фамилия, " +
            //                   "derivedtbl_2.Имя, derivedtbl_2.Отчество, derivedtbl_2.улица, derivedtbl_2.НомерДома, derivedtbl_2.корпус, derivedtbl_2.НомерКвартиры, " +
            //                   "derivedtbl_2.СерияДокумента, derivedtbl_2.НомерДокумента, derivedtbl_2.ДатаВыдачиДокумента, " +
            //                   "derivedtbl_4.СуммаАктаВыполненныхРабот " +
            //                   "FROM         (SELECT     НаименованиеТеррОргана " +
            //                   "FROM          dbo.ТерриториальныйОрган AS ТерриториальныйОрган " +
            //                   "WHERE      (id_террОрган IN " +
            //                                      "(SELECT     id_террОргана " +
            //                                        "FROM          dbo.НаименованиеРайона " +
            //                                        "WHERE      (id_район = " +
            //                                                                   "(SELECT     id_район " +
            //                                                                     "FROM          dbo.Льготник " +
            //                                                                     "WHERE      (id_льготник = " +
            //                                                                                                "(SELECT     id_льготник " +
            //                                                                                                  "FROM          dbo.Договор " +
            //                                                                                                  "WHERE      (id_договор = " +
            //                                                                                                                             "(SELECT     id_договор " +
            //                                                                                                                               "FROM          dbo.АктВыполненныхРабот " +
            //                                                                                                                               "WHERE      (id_акт = " + Convert.ToInt32(row.Cells["id_акт"].Value) + ")))))))))) AS ТерриториальныйОрган_1 CROSS JOIN " +
            //              "(SELECT     ФИО " +
            //                "FROM          dbo.ТерриториальныйОрган AS ТерриториальныйОрган_2 " +
            //                "WHERE      (id_террОрган IN " +
            //                                           "(SELECT     id_террОргана " +
            //                                             "FROM          dbo.НаименованиеРайона AS НаименованиеРайона_1 " +
            //                                             "WHERE      (id_район = " +
            //                                                                        "(SELECT     id_район " +
            //                                                                          "FROM          dbo.Льготник AS Льготник_1 " +
            //                                                                          "WHERE      (id_льготник = " +
            //                                                                                                     "(SELECT     id_льготник " +
            //                                                                                                       "FROM          dbo.Договор AS Договор_1 " +
            //                                                                                                       "WHERE      (id_договор = " +
            //                                                                                                                                  "(SELECT     id_договор " +
            //                                                                                                                                    "FROM          dbo.АктВыполненныхРабот AS АктВыполненныхРабот_1 " +
            //                                                                                                                                    "WHERE      (id_акт = " + Convert.ToInt32(row.Cells["id_акт"].Value) + ")))))))))) AS derivedtbl_1 CROSS JOIN " +
            //              "(SELECT     Обращение " +
            //                "FROM          dbo.ТерриториальныйОрган AS ТерриториальныйОрган_3 " +
            //                "WHERE      (id_террОрган IN " +
            //                                           "(SELECT     id_террОргана " +
            //                                             "FROM          dbo.НаименованиеРайона AS НаименованиеРайона_2 " +
            //                                             "WHERE      (id_район = " +
            //                                                                        "(SELECT     id_район " +
            //                                                                          "FROM          dbo.Льготник AS Льготник_2 " +
            //                                                                          "WHERE      (id_льготник = " +
            //                                                                                                     "(SELECT     id_льготник " +
            //                                                                                                       "FROM          dbo.Договор AS Договор_2 " +
            //                                                                                                       "WHERE      (id_договор = " +
            //                                                                                                                                  "(SELECT     id_договор " +
            //                                                                                                                                    "FROM          dbo.АктВыполненныхРабот AS АктВыполненныхРабот_2 " +
            //                                                                                                                                    "WHERE      (id_акт = " + Convert.ToInt32(row.Cells["id_акт"].Value) + ")))))))))) AS derivedtbl_3 CROSS JOIN " +
            //              "(SELECT     Фамилия, Имя, Отчество, улица, НомерДома, корпус, НомерКвартиры, СерияДокумента, НомерДокумента, " +
            //                                       "ДатаВыдачиДокумента " +
            //                "FROM          dbo.Льготник AS Льготник_3 " +
            //                "WHERE      (id_льготник = " +
            //                                           "(SELECT     id_льготник " +
            //                                             "FROM          dbo.Договор AS Договор_3 " +
            //                                             "WHERE      (id_договор = " +
            //                                                                        "(SELECT     id_договор " +
            //                                                                          "FROM          dbo.АктВыполненныхРабот AS АктВыполненныхРабот_3 " +
            //                                                                          "WHERE      (id_акт = " + Convert.ToInt32(row.Cells["id_акт"].Value) + ")))))) AS derivedtbl_2 CROSS JOIN " +
            //              "(SELECT     СуммаАктаВыполненныхРабот " +
            //                "FROM          dbo.Договор AS Договор_4 " +
            //                "WHERE      (id_договор = " +
            //                                           "(SELECT     id_договор " +
            //                                             "FROM          dbo.АктВыполненныхРабот AS АктВыполненныхРабот_4 " +
            //                                             "WHERE      (id_акт = " + Convert.ToInt32(row.Cells["id_акт"].Value) + ")))) AS derivedtbl_4 ";

            //    //Заполним класс
            //    DataTable table = ТаблицаБД.GetTableSQL(query, "Letter");

            //    foreach (DataRow r in table.Rows)
            //    {
            //        Letter letter = new Letter();
            //        letter.Комитет = r["Терр орагн"].ToString().Trim();

            //        letter.Начальник = r["ФИО"].ToString().Trim();
            //        letter.Обращение = r["Обращение"].ToString().Trim();

            //        string fio = r["Фамилия"].ToString().Trim() + " " + r["Имя"].ToString().Trim() + " " + r["Отчество"].ToString().Trim();
            //        letter.Фио = fio;

            //        string address = r["улица"].ToString().Trim() + " " + r["НомерДома"].ToString().Trim() + " " + r["корпус"].ToString().Trim() + " " + r["НомерКвартиры"].ToString().Trim();
            //        letter.Адрес = address.Trim();

            //        string numDoc = r["СерияДокумента"].ToString().Trim() + " " + r["НомерДокумента"].ToString().Trim() + " " + r["ДатаВыдачиДокумента"].ToString().Trim();
            //        letter.НомерДокумента = numDoc.Trim();

            //        letter.CуммаАкта = Convert.ToDecimal(r["СуммаАктаВыполненныхРабот"]).ToString("c");
            //        list.Add(letter);

            //    }


            //}

            //List<Letter> iList = list;

            //string queryTab = "SELECT [НаименованиеТеррОргана] FROM [ТерриториальныйОрган]";
            //DataTable dt = ТаблицаБД.GetTableSQL(queryTab, "ТерриториальныйОрган");

            //int iCount = 0;
            //Dictionary<int, Letter> dictionary = new Dictionary<int, Letter>();

            //foreach (DataRow rdt in dt.Rows)
            //{

            //    foreach (Letter let in list)
            //    {
            //        if (let.Комитет == rdt["НаименованиеТеррОргана"].ToString().Trim())
            //        {
            //            dictionary.Add(iCount, let);
            //        }
            //    }

            //    iCount++;
            //}

           // Dictionary<int, Letter> iTest = dictionary;


        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Реализуем паттерн фабричный метод.

             if (this.checkBox1.Checked == false)
             {
                  //Осуществляем поиск по номеру счет-фактуры.
                 if (this.textBox2.Text.Length > 0 && this.textBox1.Text.Length == 0)
                 {
                      //Поиск по номеру счет фактуры.
                     FindReestrInvoce reestr = new FindReestrInvoce();
                     DataTable tab = reestr.FindInvoice(this.textBox2.Text, this.dateTimePicker1.Value.ToShortDateString(), this.dateTimePicker2.Value.ToShortDateString());

                     if (tab.Rows.Count > 0)
                     {
                         LoadDataGridView(tab, reestr.GetCountContract(), false);
                     }
                 }

                 // Поиск по номеру реестра.
                 if (this.textBox2.Text.Length == 0 && this.textBox1.Text.Length > 0)
                 {
                      //Поиск по номеру реестра.
                     //FindReestrInvoce reestr = new FindReestrInvoce();
                     //DataTable tab = reestr.FindNumRegistr(this.textBox1.Text, this.dateTimePicker1.Value.ToShortDateString(), this.dateTimePicker2.Value.ToShortDateString());

                     FindNumRegistr registr = new FindNumRegistr();
                     DataTable tab = registr.FindRegistrNum(this.textBox1.Text, this.dateTimePicker1.Value.ToShortDateString(), this.dateTimePicker2.Value.ToShortDateString());

                     if (tab.Rows.Count > 0)
                     {
                         LoadDataGridView(tab, registr.GetCountContract(), true);
                     }
                 }
             }
             else
             {
                  //Поиск по номеру реестра и номеру счет фактуры.
                 //FindReestrInvoce reestr = new FindReestrInvoce();
                 //DataTable tab = reestr.FindInvoiceAndNumRegistr(this.textBox2.Text, this.textBox1.Text, this.dateTimePicker1.Value.ToShortDateString(), this.dateTimePicker2.Value.ToShortDateString());

                 FIndRegistrInvoiceAndNumRegistr reestr = new FIndRegistrInvoiceAndNumRegistr();
                 DataTable tab = reestr.FindInvoiceAndNumRegistr(this.textBox2.Text, this.textBox1.Text, this.dateTimePicker1.Value.ToShortDateString(), this.dateTimePicker2.Value.ToShortDateString());

                 if (tab.Rows.Count > 0)
                 {
                     LoadDataGridView(tab, reestr.GetCountContract(), true);
                 }

             }


        }

        /// <summary>
        /// Вывод данных на экран.
        /// </summary>
        /// <param name="tab">Таблица с реестрами</param>
        /// <param name="iCountD">КОличество договоров</param>
        private void LoadDataGridView(DataTable tab, int iCountD, bool flagLetter)
        {
            this.dataGridView1.DataSource = tab;

            this.dataGridView1.Columns["id_акт"].Width = 100;
            this.dataGridView1.Columns["id_акт"].Visible = false;

            this.dataGridView1.Columns["id_договор"].Width = 100;
            this.dataGridView1.Columns["id_договор"].Visible = false;

            this.dataGridView1.Columns["НомерАкта"].Width = 100;
            this.dataGridView1.Columns["НомерАкта"].DisplayIndex = 1;
            this.dataGridView1.Columns["НомерАкта"].HeaderText = "№ Акта";

            this.dataGridView1.Columns["НомерРеестра"].Width = 100;
            this.dataGridView1.Columns["НомерРеестра"].DisplayIndex = 2;
            this.dataGridView1.Columns["НомерРеестра"].HeaderText = "№ Реестра";

            this.dataGridView1.Columns["НомерСчётФактуры"].Width = 100;
            this.dataGridView1.Columns["НомерСчётФактуры"].DisplayIndex = 3;
            this.dataGridView1.Columns["НомерСчётФактуры"].HeaderText = "№ Счёт-фактуры";

            this.dataGridView1.Columns["НомерДоговора"].Width = 100;
            this.dataGridView1.Columns["НомерДоговора"].DisplayIndex = 4;
            this.dataGridView1.Columns["НомерДоговора"].HeaderText = "№ Договора";

            this.dataGridView1.Columns["Фамилия"].Width = 200;
            this.dataGridView1.Columns["Фамилия"].DisplayIndex = 5;

            this.dataGridView1.Columns["Имя"].Width = 200;
            this.dataGridView1.Columns["Имя"].DisplayIndex = 6;

            this.dataGridView1.Columns["Отчество"].Width = 200;
            this.dataGridView1.Columns["Отчество"].DisplayIndex = 7;

            this.dataGridView1.Columns["ЛьготнаяКатегория"].Width = 300;
            this.dataGridView1.Columns["ЛьготнаяКатегория"].DisplayIndex = 8;

            this.dataGridView1.Columns["Сумма"].Width = 200;
            this.dataGridView1.Columns["Сумма"].DisplayIndex = 9;

            this.dataGridView1.Columns["ДатаЗаписиДоговора"].Width = 70;
            this.dataGridView1.Columns["ДатаЗаписиДоговора"].DisplayIndex = 10;

            this.dataGridView1.Columns["ДатаРеестра"].Width = 70;
            this.dataGridView1.Columns["ДатаРеестра"].DisplayIndex = 11;

            //this.dataGridView1.Columns["ДатаЗаписиДоговора"].Width = 70;
            //this.dataGridView1.Columns["ДатаЗаписиДоговора"].DisplayIndex = 12;

            if (flagLetter == false)
            {
                this.dataGridView1.Columns["ДатаАктаВыполненныхРабот"].Width = 70;
                this.dataGridView1.Columns["ДатаАктаВыполненныхРабот"].DisplayIndex = 12;


                this.dataGridView1.Columns["logWrite"].Width = 70;
                this.dataGridView1.Columns["logWrite"].DisplayIndex = 13;
                this.dataGridView1.Columns["logWrite"].HeaderText = "Кто записал";

                //this.dataGridView1.Columns["logDate"].Width = 70;
                //this.dataGridView1.Columns["logDate"].DisplayIndex = 14;
                //this.dataGridView1.Columns["logDate"].HeaderText = "Дата записи";


                //Проверим если в поле logDate указано время то кнопку сохранения записи сделаем не активной
                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    if (row.Cells["logDate"].Value != "")
                    {
                        this.dtnWrite.Enabled = true;
                    }
                    else
                    {
                        this.dtnWrite.Enabled = false;
                    }
                }



                //Наименование населённого пункта
                this.dataGridView1.Columns["Наименование"].Width = 70;
                this.dataGridView1.Columns["Наименование"].DisplayIndex = 14;
                this.dataGridView1.Columns["Наименование"].HeaderText = "Населённый пункт";

                // Адрес проживания льготника
                this.dataGridView1.Columns["Адрес"].Width = 70;
                this.dataGridView1.Columns["Адрес"].DisplayIndex = 15;
                this.dataGridView1.Columns["Адрес"].HeaderText = "Адрес";

                // Район области в котором проживает льготник
                this.dataGridView1.Columns["Район"].Width = 70;
                this.dataGridView1.Columns["Район"].DisplayIndex = 16;
                this.dataGridView1.Columns["Район"].HeaderText = "Район";
            }
            else
            {
                //Наименование населённого пункта
                this.dataGridView1.Columns["Наименование"].Width = 70;
                this.dataGridView1.Columns["Наименование"].DisplayIndex = 14;
                this.dataGridView1.Columns["Наименование"].HeaderText = "Населённый пункт";

                // Адрес проживания льготника
                this.dataGridView1.Columns["Адрес"].Width = 70;
                this.dataGridView1.Columns["Адрес"].DisplayIndex = 15;
                this.dataGridView1.Columns["Адрес"].HeaderText = "Адрес";

                // Район области в котором проживает льготник
                this.dataGridView1.Columns["Район"].Width = 70;
                this.dataGridView1.Columns["Район"].DisplayIndex = 16;
                this.dataGridView1.Columns["Район"].HeaderText = "Район";
            }

            //Выведим количество договоров на экран
            this.label4.Visible = true;
            this.label4.Text = String.Format("Количество договоров = {0}", iCountD);
        }
    }
}