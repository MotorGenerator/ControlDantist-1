using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Classes;


using System.Globalization;
using System.Threading;


namespace ControlDantist
{
    public partial class FirstLoadHospital : Form
    {
        //Хранит временную таблицу с видом услуг из выбранной БД
        private DataTable tabВидУслуги;

        //Хранит временную таблицу с классификатором услуг из выбраннной БД
        private DataTable tabКлассифУслуг;
        
        //хранит id поликлинники
        private int id_Hosp;

        //переменная хранит название классификатора услуг
        private string классификаторУслуг = string.Empty;

        //Хранит id постановления
        private int _id;

        //Хранит id времени
        private int id_time;

        /// <summary>
        /// Хранит название выбранного постановления
        /// </summary>
        public int ID_Постановления
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

        //Хранит id услуги
        private int id_Услуг;

        public FirstLoadHospital()
        {
            InitializeComponent();

            //установим русскую культуру
            CultureInfo newCInfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            newCInfo.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = newCInfo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Файл базы данных";
            openFileDialog1.Filter = "|*.mdb";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtBdPach.Text = openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.txtNameHospital.Text.Trim() != "" && this.txtИНН.Text.Trim() != "" && this.textBox1.Text.Length != 0 && this.txtBdPach.Text.Length != 0)
            {
                //================Считаем данные из указанной БД

                //получим строку подключения к указанной БД
                string connectStringAcess = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.txtBdPach.Text + " ";
                using (OleDbConnection con = new OleDbConnection(connectStringAcess))
                {
                    con.Open();
                    OleDbTransaction transact = con.BeginTransaction();

                    //Получим классификатор услуг
                    string queryКлУслуг = "select * from КлассификаторУслуги";
                    tabКлассифУслуг = ТаблицаБД.GetTable(queryКлУслуг, "КлассификаторУслуги", con, transact);

                    ////Получим виды услуг
                    //string queryВидУслуг = "select * from ВидУслуги";
                    //tabВидУслуги = ТаблицаБД.GetTable(queryВидУслуг, "КлассификаторУслуги", con, transact);

                }

                //Запишем полученные данные в базу на SQL SERVER в соответсвующие таблицы

                //Подключимся к SQL serverу

                using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
                {
                    con.Open();
                    //SqlTransaction transact = con.BeginTransaction();

                    //Запишем поликлиннику
                    string insertHosp = "INSERT INTO Поликлинника " +
                                        "([НаименованиеПоликлинники] " +
                                        ",[КодПоликлинники] " +
                                        ",[ЮридическийАдрес] " +
                                        ",[ФактическийАдрес] " +
                                        ",[id_главВрач] " +
                                        ",[id_главБух] " +
                                        ",[СвидетельствоРегистрации] " +
                                        ",[ИНН] " +
                                        ",[КПП] " +
                                        ",[БИК] " +
                                        ",[НаименованиеБанка] " +
                                        ",[РасчётныйСчёт] " +
                                        ",[ЛицевойСчёт] " +
                                        ",[НомерЛицензии] " +
                                        ",[ДатаРегистрацииЛицензии] " +
                                        ",[ОГРН] " +
                                        ",[СвидетельствоРегистрацииЕГРЮЛ] " +
                                        ",[ОрганВыдавшийЛицензию] " +
                                        ",[Постановление] " +
                                        ",[ОКПО] " +
                                        ",[ОКАТО] " +
                                        ",[Flag] " +
                                        ",[НачальныйНомерДоговора]) " +
                                        "VALUES " +
                                        "('" + this.txtNameHospital.Text.Trim() + "' " +
                                        ",''" +
                                        ",''" +
                                        ",''" +
                                        ",0" +
                                        ",0" +
                                        ",''" +
                                        ",'" + this.txtИНН.Text.Trim() + "' " +
                                        ",''" +
                                        ",''" +
                                        ",''" +
                                        ",''" +
                                        ",''" +
                                        ",''" +
                                        ",'" + DateTime.Today.ToShortDateString() + "' " +
                                        ",''" +
                                        ",''" +
                                        ",''" +
                                        ",''" +
                                        ",''" +
                                        ",''" +
                                        ",0" +
                                        ",0" + ")";

                    //Выполним запрос на вставку
                    SqlCommand comInsertHosp = new SqlCommand(insertHosp, con);
                    //comInsertHosp.Transaction = transact;
                    comInsertHosp.ExecuteNonQuery();

                    //Получим id временного интервала
                    string queryTime = "select id_время from [ВременнойИнтервал] where id_постановление = "+ this.ID_Постановления +" ";
                    SqlCommand comInsertTime = new SqlCommand(queryTime, con);
                    //comInsertTime.Transaction = transact;

                    SqlDataReader readTime = comInsertTime.ExecuteReader();
                    while (readTime.Read())
                    {
                        id_time = Convert.ToInt32(readTime["id_время"]);
                    }
                    readTime.Close();

                }

                //получим текущей поликлинники
                string querySelectHosp = "select id_поликлинника from Поликлинника where ИНН = '" + this.txtИНН.Text.Trim() + "' ";

                SqlConnection conS = new SqlConnection(ConnectDB.ConnectionString());
                conS.Open();
                SqlCommand comSelHosp = new SqlCommand(querySelectHosp, conS);

                SqlDataReader read = comSelHosp.ExecuteReader();

                while (read.Read())
                {
                    id_Hosp = Convert.ToInt32(read["id_поликлинника"]);
                }
                read.Close();
                conS.Close();

                //Запишем на SQL Server данные в классификатор услуг
                foreach (DataRow rowКлУсл in tabКлассифУслуг.Rows)
                {
                    //Запишем текущее значение классификатора услуг в таблицу
                    string queryInsКлУслуг = "INSERT INTO КлассификаторУслуг " +
                                             "([КлассификаторУслуги] " +
                                             ",[id_поликлинника] " +
                                             ",id_постановление ) " +
                                             "VALUES " +
                                             "('" + rowКлУсл["КлассификаторУслуги"].ToString() + "' " +
                                             "," + id_Hosp + " " +
                                             "," + this.ID_Постановления + " ) ";

                    SqlConnection con = new SqlConnection(ConnectDB.ConnectionString());
                    SqlCommand comInsertКлассУслуг = new SqlCommand(queryInsКлУслуг, con);

                    con.Open();
                    comInsertКлассУслуг.ExecuteNonQuery();
                    con.Close();

                    //Получим текущий id Классификатор услуг
                    string querIdУслуг = "select top 1 id_кодУслуги,КлассификаторУслуги from КлассификаторУслуг order by id_кодУслуги desc";
                    SqlConnection conSel = new SqlConnection(ConnectDB.ConnectionString());

                    conSel.Open();
                    SqlCommand comSelIdУслуг = new SqlCommand(querIdУслуг, conSel);

                    //comSelIdУслуг.Transaction = transact;
                    SqlDataReader readIdУслуг = comSelIdУслуг.ExecuteReader();
                    while (readIdУслуг.Read())
                    {
                        id_Услуг = Convert.ToInt32(readIdУслуг["id_кодУслуги"]);
                        классификаторУслуг = readIdУслуг["КлассификаторУслуги"].ToString();
                    }
                    readIdУслуг.Close();

                    //Получим виды услуг
                    OleDbConnection conOLE = new OleDbConnection(connectStringAcess);
                    conOLE.Open();

                    string queryВидУслуг = "select * from ВидУслуги where id_кодУслуги in (select id_кодУслуги from КлассификаторУслуги where КлассификаторУслуги = '" + классификаторУслуг + "')";
                    //tabВидУслуги = ТаблицаБД.GetTable(queryВидУслуг,conOLE, "КлассификаторУслуги");
                    tabВидУслуги = ТаблицаБД.GetTable(queryВидУслуг, connectStringAcess, "КлассификаторУслуги");
                    conOLE.Close();


                    //Запишем виды услуг соотвествующие текущему классификатору
                    foreach (DataRow row in tabВидУслуги.Rows)
                    {
                        string queryInsertВидУслуг = "INSERT INTO [ВидУслуг] " +
                                                     "([ВидУслуги] " +
                                                     ",[Цена] " +
                                                     ",[id_поликлинника] " +
                                                     ",[НомерПоПеречню] " +
                                                     ",[Выбрать] " +
                                                     ",[id_кодУслуги] " +
                                                     ",[ТехЛист] " +
                                                     ",id_время " +
                                                     ",id_постановление ) " +
                                                     "VALUES " +
                                                     "('" + row["ВидУслуги"].ToString() + "' " +
                                                     "," + Convert.ToDecimal(row["Цена"]) + " " +
                                                     "," + id_Hosp + " " +
                                                     ",'" + row["НомерПоПеречню"].ToString() + "' " +
                                                     ",'" + Convert.ToBoolean(row["Выбрать"]) + "' " +
                                                     "," + id_Услуг + " " +
                                                     ",0" +
                                                     ","+ this.id_time +" " +
                                                     "," + this.ID_Постановления + " ) ";

                        SqlConnection conВидУслуг = new SqlConnection(ConnectDB.ConnectionString());
                        conВидУслуг.Open();

                        SqlCommand com = new SqlCommand(queryInsertВидУслуг, conВидУслуг);

                        com.ExecuteNonQuery();
                        conВидУслуг.Close();
                    }
                }

                //    con.Close();
                //}


            }
            else
            {
                MessageBox.Show("Не введены исходные данные");
            }
        }

        private void FirstLoadHospital_Load(object sender, EventArgs e)
        {
            //string queryLoad = "select * from dbo.ПостановлениеПравительства";

            //SqlConnection con = new SqlConnection(ConnectDB.ConnectionString());
            //SqlDataAdapter da = new SqlDataAdapter(queryLoad, con);

            //con.Open();
            //DataTable tab = new DataTable("Постановление");
            
            //da.Fill(tab);
            //con.Close();

            ////заполним раскрывающейся список с постановлениями проавительства
            ////Потстановление,Номер,ДатаПостановления
            //this.comboBox1.SelectedIndex = 

        }

        private void btnПостановление_Click(object sender, EventArgs e)
        {
            FormSelectПостановление form = new FormSelectПостановление();
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
            {
                this.textBox1.Text = form.Постановление;
                this.ID_Постановления = form.ID_Поствановления;
            }
        }
    }
}