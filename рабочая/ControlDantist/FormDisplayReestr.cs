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

using Microsoft.Office.Interop.Word;

using System.Globalization;
using System.Threading;
using System.IO;


//Добавим DLL
using DantistLibrary;


namespace ControlDantist
{
    public partial class FormDisplayReestr : Form
    {
        private Dictionary<string, DisplayReestr> disp;
        private List<Unload> unload;

        private string _НомерРеестра = string.Empty;
        private string _НомерСчётФактуры = string.Empty;
        private string _ДатаРеестра = string.Empty;
        private string _ДатаСчётФактуры = string.Empty;

        private DataTable dtServ;
        private DataTable dtFile;

        //Переменная для хранения количество актов
        private int countAct = 0;


        /// <summary>
        /// Дата счёт фактуры
        /// </summary>
        public string ДатаСчётФактуры
        {
            get
            {
                return _ДатаСчётФактуры;
            }
            set
            {
                _ДатаСчётФактуры = value;
            }
        }


        /// <summary>
        /// Номер счёт фактуры
        /// </summary>
        public string НомерСчётФактуры
        {
            get
            {
                return _НомерСчётФактуры;
            }
            set
            {
                _НомерСчётФактуры = value;
            }
        }


        /// <summary>
        /// Дата реестра
        /// </summary>
        public string ДатаРеестра
        {
            get
            {
                return _ДатаРеестра;
            }
            set
            {
                _ДатаРеестра = value;
            }
        }


        /// <summary>
        /// Номер реестра
        /// </summary>
        public string НомерРеестра
        {
            get
            {
                return _НомерРеестра;
            }
            set
            {
                _НомерРеестра = value;
            }
        }

        /// <summary>
        /// Хранит выгрузку актов
        /// </summary>
        public List<Unload> Unloads
        {
            get
            {
                return unload;
            }
            set
            {
                unload = value;
            }
        }

        /// <summary>
        /// Хранит Библиотеку актов выполненных работ
        /// </summary>
        public Dictionary<string, DisplayReestr> АктыРабот
        {
            get
            {
                return disp;
            }
            set
            {
                disp = value;
            }
        }

        public FormDisplayReestr()
        {
            InitializeComponent();
        }

        private void FormDisplayReestr_Load(object sender, EventArgs e)
        {
            CultureInfo newCInfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            newCInfo.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = newCInfo;

            //Присвоим переменной countAct количество актов
            countAct = this.АктыРабот.Count;

            //List<DisplayReestr> list = new List<DisplayReestr>();
            List<DisplayReestrCheck> list = new List<DisplayReestrCheck>();

            //Сумма контракта
             decimal sumРеестрАкт = 0.0m;

            //заполним данными коллекцию 
            foreach (DisplayReestr dr in this.АктыРабот.Values)
            {

                ReestrControl rkTest = new ReestrControl();
                rkTest = dr.Льготник;

                if (rkTest.ФИО == "Иванова Лидия Владимировна")
                {
                    string iTerst = "Test";
                }

                DisplayReestrCheck drck = new DisplayReestrCheck();

                // Запишем данные по льготнымка категориям.
                drck.ЛьготнаяКатегория = dr.ЛьготнаяКатегория;

                /*если FlagError = true - значит по услугам ошибки нет
                 * если ErrorPerson = false то значит по персональным данным ошибки нет
                 * если FlagError = false - значит по услугам ошибка
                 * если ErrorPerson = true - то значит по персональным данным ошибка
                 */

                //ошибки нет не в услугах не в персональных данных
                if (dr.FlagError == true && dr.ErrorPerson == false)
                {
                    drck.FlagError = true;
                }

                //ошибка и в персональных данных и в услугах
                if (dr.FlagError == false && dr.ErrorPerson == true)
                {
                    drck.FlagError = false;
                }

                //
                if (dr.FlagError == true && dr.ErrorPerson == true)
                {
                    drck.FlagError = false;
                }

                if (dr.FlagError == false && dr.ErrorPerson == false)
                {
                    drck.FlagError = false;
                }

                // Если договор уже оплачивался.
                if (dr.FlagErrorОплаченныйДоговор == true)
                {
                    drck.FlagError = false;

                    drck.ОплаченныйРанееДоговорАкт = dr.НомерОплаченногоАкта.Trim();

                    drck.FlagErrorActОплата = true;
                }

                if (dr.FlagError == false && dr.ErrorPrefCategory == true)
                {
                    drck.FlagError = false;
                }
                
                //сумма оказываемых услуг
                //drck.Sum = dr.Sum.ToString("c");
                drck.Sum = Math.Round(dr.СуммаДоговорСервер,2).ToString("c");

                ReestrControl rck = dr.Льготник;
                drck.ФИО_Льготник = rck.ФИО;

                //разделим строку и получим отдельно номер договора и дату договора
                string[] srryДатаНомерДоговор = rck.ДатаНомерДоговора.Split(' ');

                //Запишем номер договора
                drck.НомерДоговора = srryДатаНомерДоговор[0];

                //получим дату договора
                drck.ДатаДоговора = srryДатаНомерДоговор[1];

                //получим отдельно номер акта и дату акта
                string[] arryДатаНомерАкта = rck.НомерАктаОказанныхУслуг.Split(' ');

                //получим номе акта 
                drck.НомерАкта = arryДатаНомерАкта[0];

                //получим дату акта
                drck.ДатаАкта =  arryДатаНомерАкта[1];

                drck.СписокОшибок = dr.СписокОшибок;

                //запишем сумму акта
                drck.СуммаАкта = Math.Round(dr.СуммаАктаВыполненныхРабот,2).ToString("c");

                //укажем наличие доп соглашения
                drck.FlagAddContract = dr.FlagAddContract;

                //drck.НомерАкта = rck.НомерАктаОказанныхУслуг;
                list.Add(drck);

                //sumРеестрАкт = Math.Round(Math.Round(Math.Round(sumРеестрАкт, 2) + Math.Round(dr.СуммаАктаВыполненныхРабот, 2)), 2);
                sumРеестрАкт = Math.Round(Math.Round(sumРеестрАкт, 2) + Math.Round(dr.СуммаАктаВыполненныхРабот, 2), 2);
            }

            this.dataGridView1.DataSource = list;

            this.dataGridView1.Columns["НомерДоговора"].HeaderText = "№ договора";
            this.dataGridView1.Columns["НомерДоговора"].DisplayIndex = 1;
            this.dataGridView1.Columns["НомерДоговора"].Width = 100;
            this.dataGridView1.Columns["НомерДоговора"].ReadOnly = true;

            this.dataGridView1.Columns["ДатаДоговора"].HeaderText = "дата договора";
            this.dataGridView1.Columns["ДатаДоговора"].DisplayIndex = 2;
            this.dataGridView1.Columns["ДатаДоговора"].Width = 80;
            this.dataGridView1.Columns["ДатаДоговора"].ReadOnly = true;

            this.dataGridView1.Columns["Sum"].HeaderText = "Сумма договора";
            this.dataGridView1.Columns["Sum"].DisplayIndex = 3;
            this.dataGridView1.Columns["Sum"].Width = 80;
            this.dataGridView1.Columns["Sum"].ReadOnly = true;

           
             this.dataGridView1.Columns["НомерАкта"].HeaderText = "№ акта";
             this.dataGridView1.Columns["НомерАкта"].DisplayIndex = 4;
             this.dataGridView1.Columns["НомерАкта"].Width = 100;
            this.dataGridView1.Columns["НомерАкта"].ReadOnly = true;

             this.dataGridView1.Columns["ДатаАкта"].HeaderText = "дата акта";
             this.dataGridView1.Columns["ДатаАкта"].DisplayIndex = 5;
             this.dataGridView1.Columns["ДатаАкта"].Width = 80;
            this.dataGridView1.Columns["ДатаАкта"].ReadOnly = true;

            this.dataGridView1.Columns["СуммаАкта"].HeaderText = "Сумма акта";
            this.dataGridView1.Columns["СуммаАкта"].DisplayIndex = 6;
            this.dataGridView1.Columns["СуммаАкта"].Width = 100;
            this.dataGridView1.Columns["СуммаАкта"].ReadOnly = true;

             this.dataGridView1.Columns["ФИО_Льготник"].HeaderText = "ФИО";
             this.dataGridView1.Columns["ФИО_Льготник"].DisplayIndex = 0;
             this.dataGridView1.Columns["ФИО_Льготник"].Width = 200;
            this.dataGridView1.Columns["ФИО_Льготник"].ReadOnly = true;

             this.dataGridView1.Columns["FlagError"].HeaderText = "Результат проверки";
             this.dataGridView1.Columns["FlagError"].DisplayIndex = 7;
             this.dataGridView1.Columns["FlagError"].Width = 100;

          
            //Откроем для редактирования
             this.dataGridView1.Columns["FlagError"].ReadOnly = false;

             this.dataGridView1.Columns["FlagAddContract"].HeaderText = "Наличие доп. соглашения";
             this.dataGridView1.Columns["FlagAddContract"].DisplayIndex = 8;
             this.dataGridView1.Columns["FlagAddContract"].Width = 100;
             this.dataGridView1.Columns["FlagAddContract"].ReadOnly = true;

             this.dataGridView1.Columns["ОплаченныйРанееДоговорАкт"].Visible = false;
             this.dataGridView1.Columns["FlagErrorActОплата"].Visible = false;

            //Пройдёмся по DataGrid и псочитаем суммы договоров и суммы актов
             decimal sumРеестрДог = 0.0m;

             this.dataGridView1.Columns["FlagЛьготнаяКатегория"].Visible = false;

             this.dataGridView1.Columns["ЛьготнаяКатегория"].Visible = false;
            

             if (this.dataGridView1.Rows.Count != 0)
             {
                 //foreach (DataGridViewRow row in this.dataGridView1.Rows)
                 //{
                 //   //Подсчитаем сумму реестра выполненных договоров (Сумму актов)
                 //    string s1 = row.Cells["СуммаАкта"].Value.ToString();
                 //    string sum1 = s1.Replace("р.", " ");

                 //    string summ1 = sum1.Replace(',', '.');
                 //    sumРеестрАкт = Math.Round(Math.Round(Math.Round(sumРеестрАкт, 2) + Math.Round(Convert.ToDecimal(summ1),2)),2);
                 //    //}
                 //}
             }

             //this.labelServ.Text  = "Сумма реестра актов " + sumРеестрДог.ToString("c");
             this.labelFile.Text = "Сумма реестра выполненных договоров " + Math.Round(sumРеестрАкт, 2).ToString("c") + " количество договоров " + this.АктыРабот.Count.ToString() + " шт.";
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells["FlagErrorActОплата"].Value) == true)
            {
                FormMessageAct form = new FormMessageAct();
                form.НомерАкта = this.dataGridView1.CurrentRow.Cells["ОплаченныйРанееДоговорАкт"].Value.ToString().Trim();
                form.Show();
            }

            //обнулим таблицы 
            this.dataGridView2.DataSource = null;

            //отобразим правильные данные
            this.dataGridView3.DataSource = null;

            //получим номер договора
            string НомерАкта = this.dataGridView1.CurrentRow.Cells["НомерАкта"].Value.ToString().Trim();

            this.lblServerLK.Text = "";
            this.lblFileLK.Text = "";

            // Отобразим льготные категории из сервера и из файла.
            PreferentCategory preCat = (PreferentCategory)this.dataGridView1.CurrentRow.Cells["ЛьготнаяКатегория"].Value;

            // Отобразим льготные категории.
            this.lblServerLK.Text = preCat.PCategoryServer.Trim();
            this.lblFileLK.Text = preCat.PActegoryFile.Trim();

            if (this.АктыРабот.ContainsKey(НомерАкта))
            {
                //this.iv = ВыгрузкаПроектДоговоров[numberDog.Trim()];
                DisplayReestr dr = this.АктыРабот[НомерАкта];

                // Если не совпали льготные категории установленные на сервере и в договоре.
                if (preCat.PCategoryServer.Trim().ToLower() != preCat.PActegoryFile.Trim().ToLower())
                {
                    dr.ErrorPerson = true;
                }

                if (dr.ErrorPerson == true) // || dr.ErrorPerson == false)
                {
                   // MessageBox.Show("Изменены данные о льготнике");
                    MyMessage message = new MyMessage();
                    message.ShowDialog();

                    //Переменные для хранения данных с сервера и из файла выгрузки
                   

                    if (message.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {

                        //Получим id льготника
                        string номерДоговора = this.dataGridView1.CurrentRow.Cells["НомерДоговора"].Value.ToString().Trim();

                        //получим ФИО льготника
                        string фио = this.dataGridView1.CurrentRow.Cells["ФИО_Льготник"].Value.ToString().Trim();

                        string queryЛьготник = "select * from Льготник " +
                                               "where id_льготник in ( " +
                                               "select id_льготник from Договор where НомерДоговора = '" + номерДоговора + "') ";

                        //Получим данные по льготнику которые храняться у нас на сервере
                        dtServ = ТаблицаБД.GetTableSQL(queryЛьготник, "Льготник");

                        //фамилию
                        string[] фамилия = фио.Split(' ');

                        if (фамилия.Length == 3)
                        {

                            // Получим данные из файла.
                            IEnumerable<Unload> un = this.Unloads.Where(u => u.Льготник.Rows[0]["Фамилия"].ToString().Trim() == фамилия[0].Trim()
                                &&
                                u.Льготник.Rows[0]["Имя"].ToString().Trim() == фамилия[1].Trim()
                                && u.Льготник.Rows[0]["Отчество"].ToString().Trim() == фамилия[2].Trim()
                                ).Select(u => u).Take(1);

                            if (un != null)
                            {
                                foreach (Unload u in un)
                                {
                                    //list.Add(u);
                                    dtFile = u.Льготник;
                                }

                                if (un.Count() == 0)
                                {
                                    DataTable dtNull = new DataTable();

                                    dtFile = dtNull;
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                IEnumerable<Unload> un = this.Unloads.Where(u => u.Льготник.Rows[0]["Фамилия"].ToString().Trim() == фамилия[0].Trim() &&
                                   u.Льготник.Rows[0]["Имя"].ToString().Trim() == фамилия[0].Trim()).Select(u => u).Take(1);

                                if (un != null)
                                {
                                    foreach (Unload u in un)
                                    {
                                        //list.Add(u);
                                        dtFile = u.Льготник;
                                    }

                                    if (un.Count() == 0)
                                    {
                                        DataTable dtNull = new DataTable();

                                        dtFile = dtNull;
                                    }

                                }
                            }
                            catch
                            {
                                DataTable dtNull = new DataTable();

                                dtFile = dtNull;
                            }
                        }

                        FormDisplayError ferr = new FormDisplayError();
                        ferr.DtServer = dtServ;
                        ferr.DtFile = dtFile;
                        ferr.LkFile = preCat.PActegoryFile.Trim();
                        ferr.LkServer = preCat.PCategoryServer.Trim();
                        ferr.ShowDialog();
                    }
                    else
                    {
                        message.Close();
                    }
                   
                }

                if (dr.СписокОшибок != null)
                {
                    List<ErrorsReestrUnload> listError = dr.СписокОшибок;

                    //список для хранения корректных данных
                    List<DateService> listCorrect = new List<DateService>();

                    //Список для хранения ошибочных данных
                    List<DateService> listErrorD = new List<DateService>();

                    //формируем список ошибок и список реальных данных
                    foreach (ErrorsReestrUnload list in listError)
                    {
                        if (list.НаименованиеУслуги != null)
                        {
                            if (list.НаименованиеУслуги.Length != 0)
                            {
                                //запишем правильные данные
                                DateService correct = new DateService();
                                correct.Наименование = list.НаименованиеУслуги;

                                correct.Сумма = list.Сумма.ToString("c");
                                correct.Количество = list.Количество.ToString().Trim();

                                correct.Цена = list.Цена.ToString("c");

                                //добавим класс в коллекцию
                                listCorrect.Add(correct);
                            }
                        }

                        if (list.ErrorНаименованиеУслуги != null)
                        {
                            if (list.ErrorНаименованиеУслуги.Length != 0)
                            {
                                //запишем ошибочные данные
                                DateService error = new DateService();
                                error.Наименование = list.ErrorНаименованиеУслуги;
                                error.Сумма = list.ErrorСумма.ToString("c");

                                error.Количество = list.ОшибкаКоличество.ToString().Trim();

                                error.Цена = list.ErrorЦена.ToString("c");
                                listErrorD.Add(error);
                            }
                        }
                    }

                    //отобразим ошибочные данные
                    this.dataGridView2.DataSource = listErrorD;

                    this.dataGridView2.Columns["Наименование"].HeaderText = "Наименование";
                    this.dataGridView2.Columns["Наименование"].DisplayIndex = 0;
                    
                    this.dataGridView2.Columns["Наименование"].ReadOnly = true;

                    this.dataGridView2.Columns["Цена"].HeaderText = "Цена";
                    this.dataGridView2.Columns["Цена"].DisplayIndex = 1;
                    
                    this.dataGridView2.Columns["Цена"].ReadOnly = true;

                    this.dataGridView2.Columns["Количество"].HeaderText = "Кол-во";
                    this.dataGridView2.Columns["Количество"].DisplayIndex = 2;
                    this.dataGridView2.Columns["Количество"].Width = 50;
                    this.dataGridView2.Columns["Количество"].ReadOnly = true;

                    this.dataGridView2.Columns["Сумма"].HeaderText = "Сумма";
                    this.dataGridView2.Columns["Сумма"].DisplayIndex = 3;
                    
                    this.dataGridView2.Columns["Сумма"].ReadOnly = true;

                    if (listCorrect.Count != 0)
                    {
                        //отобразим правильные данные
                        this.dataGridView3.DataSource = listCorrect;

                        this.dataGridView3.Columns["Наименование"].HeaderText = "Наименование";
                        this.dataGridView3.Columns["Наименование"].DisplayIndex = 0;
                        
                        this.dataGridView3.Columns["Наименование"].ReadOnly = true;

                        this.dataGridView3.Columns["Цена"].HeaderText = "Цена";
                        this.dataGridView3.Columns["Цена"].DisplayIndex = 1;
                        
                        this.dataGridView3.Columns["Цена"].ReadOnly = true;

                        this.dataGridView3.Columns["Количество"].HeaderText = "Кол-во";
                        this.dataGridView3.Columns["Количество"].DisplayIndex = 2;
                        this.dataGridView3.Columns["Количество"].Width = 50;
                        this.dataGridView3.Columns["Количество"].ReadOnly = true;

                        this.dataGridView3.Columns["Сумма"].HeaderText = "Сумма";
                        this.dataGridView3.Columns["Сумма"].DisplayIndex = 3;
                        
                        this.dataGridView3.Columns["Сумма"].ReadOnly = true;
                    }
                }

            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Пройдёмся по DataGridy и запишем в БД только те договора у которых стоит флаг
            List<DisplayReestrCheck> list = new List<DisplayReestrCheck>();
            List<Unload> unload = this.Unloads; 
          
            //int countContracts = this.АктыРабот.Count;- не работает
            int countContracts = countAct;
            
            //счётчик количесва договоров прошедших проверку
            int countD = 0;

            //пройдёмся по DataGridView и узнаем все ли договора прошли проверку если они прошли все то можно записывать в БД
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {

                //Узнаем сколько договоров прошли проверку
                //string номДого = un.Договор.Rows[0]["НомерДоговора"].ToString().Trim();
                bool fl = Convert.ToBoolean(row.Cells["FlagError"].Value);
                string sumContrControl = row.Cells["Sum"].Value.ToString().Trim();

                if (fl == true && sumContrControl != "0,00р.")
                {
                    countD++;
                }
            }

            //сравним количество договоров
            if (countContracts == countD)
            {


                //FormReestr formR = new FormReestr();
                //formR.ShowDialog();

                //if (formR.DialogResult == DialogResult.OK)
                //{

                    //получим номер реестра
                string numReestr = this.НомерРеестра;// formR.НомерРеестра;

                    //получим дату реестра
                string dateReestr = this.ДатаРеестра;// formR.ДатаРеестра;

                    //получим номер счёт-фактуры
                string num_счётФактуры = this.НомерСчётФактуры;// formR.НомерСчётФактуры;

                    //получим дату счёт фактуры
                string date_СчётФактуры = this.ДатаСчётФактуры;// formR.ДатаСчётФактуры;

                // Выполним запись акта выполненных работ с номером реестра и счет-фактуры в единой транзакции.
                using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
                {
                    // Установим соединение.
                    con.Open();

                    // Откроем транзакцию.
                    SqlTransaction transact = con.BeginTransaction();

                    foreach (DataGridViewRow row in this.dataGridView1.Rows)
                    {
                        //Установим русскую культуру
                        CultureInfo newCInfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
                        newCInfo.NumberFormat.NumberDecimalSeparator = ".";
                        Thread.CurrentThread.CurrentCulture = newCInfo;

                        //получим номер Акта
                        string numDog = row.Cells["НомерДоговора"].Value.ToString().Trim();


                        //получим флаг который стоит в DataGridView 
                        bool flag = Convert.ToBoolean(row.Cells["FlagError"].Value);

                        //получим нужный нам договор и акт выгрузки
                        foreach (Unload un in this.Unloads)
                        {
                            //Найдём номер договора который соответсвует помеченному как прошедший проверку
                            string номерДоговора = un.Договор.Rows[0]["НомерДоговора"].ToString().Trim();

                            if (numDog == номерДоговора && flag == true)
                            {

                                //получим договор который закрывается текушим актом
                                DataRow rowDog = un.Договор.Rows[0];

                                //Получим id договора текущего договора (Договор который последним записан в БД)
                                string query = "select top 1 * from dbo.Договор " +
                                               "where НомерДоговора = '" + номерДоговора + "' and ФлагПроверки = 'True' order by id_договор desc";

                                DataTable tabID;

                                //Выполним всё в единой транзакции
                                tabID = ТаблицаБД.GetTableSQL(query, "Договор", con, transact);

                                //Получим id договора которое у нас на сервере  id льготной категории
                                int id_договор = 0;

                                int id_льготнойКатегории = 0;
                                if (tabID.Rows.Count != 0)
                                {
                                    id_договор = Convert.ToInt32(tabID.Rows[0]["id_договор"]);
                                    id_льготнойКатегории = Convert.ToInt32(tabID.Rows[0]["id_льготнаяКатегория"]);
                                }

                                //всатвим дату договора полученную из файла.
                                string датаДоговора = Convert.ToDateTime(rowDog["ДатаДоговора"]).ToShortDateString();

                                // Получим акт выполненных работ из файла.
                                DataTable табАкт = un.АктВыполненныхРабот;

                                // Получим из файла ДатуАкта.
                                string датаАкта = Convert.ToDateTime(табАкт.Rows[0]["ДатаПодписания"]).ToShortDateString();

                                decimal sumDog = 0.0m;

                                //получим сумму акта выполненных работ из файла.
                                DataTable tabУслугиДоговор = un.УслугиПоДоговору;

                                foreach (DataRow rowДог in tabУслугиДоговор.Rows)
                                {
                                    sumDog = Math.Round(sumDog + Convert.ToDecimal(rowДог["Сумма"]), 2);
                                }

                                decimal summ = sumDog;

                                //Получим данные по акту выполненных работ
                                DataTable tabAct = un.АктВыполненныхРабот;

                                //Проверим существует ли указанный Акт выполненных работ на сервере
                                string countRow = "select COUNT(НомерАкта) as 'Количество' from dbo.АктВыполненныхРабот " +
                                                  "where НомерАкта = '" + tabAct.Rows[0]["НомерАкта"].ToString().Trim() + "' ";

                                DataTable tab;
                                //using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
                                //{
                                //    con.Open();

                                //    //Выполним всё в единой транзакции
                                //    SqlTransaction transact = con.BeginTransaction();
                                    tab = ТаблицаБД.GetTableSQL(countRow, "АктСервер", con, transact);
                                //}


                                if (Convert.ToInt32(tab.Rows[0]["Количество"]) == 0)
                                {

                                    StringBuilder builder = new StringBuilder();

                                    DataRow rowЛьгот = un.Льготник.Rows[0];

                                    //Обновим записи в таблице по льготнику
                                    string updateLgot = " UPDATE [Льготник] " +
                                                        "SET [Фамилия] = '" + rowЛьгот["Фамилия"].ToString().Trim() + "' " +
                                                        ",[Имя] = '" + rowЛьгот["Имя"].ToString().Trim() + "' " +
                                                        ",[Отчество] = '" + rowЛьгот["Отчество"].ToString().Trim() + "' " +
                                                        ",[ДатаРождения] = '" + rowЛьгот["ДатаРождения"].ToString().Trim() + "' " +
                                                        ",[улица] = '" + rowЛьгот["улица"].ToString().Trim() + "' " +
                                                        ",[НомерДома] = '" + rowЛьгот["НомерДома"].ToString().Replace("'",string.Empty).Trim() + "' " +
                                                        ",[корпус] = '" + rowЛьгот["корпус"].ToString().Trim() + "' " +
                                                        ",[НомерКвартиры] = '" + rowЛьгот["НомерКвартиры"].ToString().Trim() + "' " +
                                                        ",[СерияПаспорта] = '" + rowЛьгот["СерияПаспорта"].ToString().Trim() + "' " +
                                                        ",[НомерПаспорта] = '" + rowЛьгот["НомерПаспорта"].ToString().Trim() + "' " +
                                                        ",[ДатаВыдачиПаспорта] = '" + rowЛьгот["ДатаВыдачиПаспорта"].ToString().Trim() + "' " +
                                                        ",[КемВыданПаспорт] = '" + rowЛьгот["КемВыданПаспорт"].ToString().Trim() + "' " +
                                                        ",[id_льготнойКатегории] = " + id_льготнойКатегории + " " +
                                                        ",[id_документ] = " + Convert.ToInt32(rowЛьгот["id_документ"]) + " " +
                                                        ",[СерияДокумента] = '" + rowЛьгот["СерияДокумента"].ToString().Trim() + "' " +
                                                        ",[НомерДокумента] = '" + rowЛьгот["НомерДокумента"].ToString().Trim() + "' " +
                                                        ",[ДатаВыдачиДокумента] = '" + rowЛьгот["ДатаВыдачиДокумента"].ToString().Trim() + "' " +
                                                        ",[КемВыданДокумент] = '" + rowЛьгот["КемВыданДокумент"].ToString().Trim().Replace("'",string.Empty) + "' " +
                                        //",[id_область] = " + Convert.ToInt32(rowЛьгот["id_область"]) + " " +
                                        //",[id_район] = " + Convert.ToInt32(rowЛьгот["id_район"]) + " " +
                                        //",[id_насПункт] = " + Convert.ToInt32(rowЛьгот["id_насПункт"]) + " " +
                                                        "where id_льготник = ( " +
                                                        "select id_льготник from dbo.Договор " +
                                                        "where id_договор = " + id_договор + ") ";

                                    builder.Append(updateLgot);

                                    //внесём изменения в таблицу договор
                                    string updateQuery = "declare @id int " +
                                           "select @id = id_льготнойКатегории from dbo.ЛьготнаяКатегория " +
                                           "where ЛьготнаяКатегория = '" + un.ЛьготнаяКатегория + "' " +
                                           "UPDATE [Договор] " +
                                           //"UPDATE ДоговорTemp " + 
                                           "SET [ДатаДоговора] = '" + датаДоговора.Trim() + "' " +
                                           ",[ДатаАктаВыполненныхРабот] = '" + датаАкта.Trim() + "' " +
                                           ",СуммаАктаВыполненныхРабот = " + summ + " " +
                                        //",id_льготнаяКатегория = @id " + id_льготнойКатегории
                                            ",id_льготнаяКатегория = " + id_льготнойКатегории + " " +
                                           ",ФлагНаличияДоговора = 'True' " +
                                           ",ФлагНаличияАкта = 'True' " +
                                        //",ДатаЗаписиДоговора = '"+ DateTime.Today +"' " + // не будем замнять дату
                                           ",[НомерРеестра] = '" + numReestr + "' " +
                                           ",[ДатаРеестра] = '" + dateReestr + "' " +
                                           ",[НомерСчётФактрура] = '" + num_счётФактуры + "' " +
                                           ",[ДатаСчётФактура] = '" + date_СчётФактуры + "' " +
                                           ",[logWrite] = '" + MyAplicationIdentity.GetUses() + "' " +
                                           "WHERE id_договор = " + id_договор + " ";

                                    builder.Append(updateQuery);

                                    /*
                                     * Предположем, что если пользоватьель проставил флаг, что акт прошёл проверку
                                     * значит мы соглашаемся с видами работ указанными в данном акте.
                                     * Для этого мы удаляем все виды работ указанные для текущего акта и записываем новые данные
                                     * из файла.
                                     * */

                                    string queryDelete = "delete УслугиПоДоговору " +
                                                         "where id_договор = " + id_договор + " ";

                                    builder.Append(queryDelete);

                                    //счётчик циклов
                                    int iCount = 0;

                                    //запишем новые услуги
                                    foreach (DataRow rowU in un.УслугиПоДоговору.Rows)
                                    {
                                        string queryInsertУслуги = "declare @idДоговор_" + iCount + " int " +
                                            //"select @idДоговор_" + iCount + " = id_договор from dbo.Договор where НомерДоговора = '" + номерДоговора + "' " +
                                                  "select @idДоговор_" + iCount + " = id_договор from dbo.Договор where НомерДоговора = '" + номерДоговора + "' and ФлагПроверки = 'True' " +
                                                  //"INSERT INTO УслугиПоДоговору " +
                                                  "INSERT INTO УслугиПоДоговору " +
                                                  "([НаименованиеУслуги] " +
                                                  ",[цена] " +
                                                  ",[Количество] " +
                                                  ",[id_договор] " +
                                                  ",[НомерПоПеречню] " +
                                                  ",[Сумма] " +
                                                  ",[ТехЛист]) " +
                                                  "VALUES " +
                                                   "('" + rowU["НаименованиеУслуги"].ToString() + "' " +
                                                   "," + Convert.ToDecimal(rowU["Цена"]) + " " +
                                                   "," + Convert.ToInt32(rowU["Количество"]) + " " +
                                                   ",@idДоговор_" + iCount + " " +
                                                   ",'" + rowU["НомерПоПеречню"].ToString() + "' " +
                                                   "," + Convert.ToDecimal(rowU["Сумма"]) + " " +
                                                   "," + Convert.ToInt32(rowU["Количество"]) + " ) ";

                                        builder.Append(queryInsertУслуги);

                                        iCount++;
                                    }


                                    //try
                                    //{

                                        string insertAct = "INSERT INTO [АктВыполненныхРабот] " +
                                                           "([НомерАкта] " +
                                                           ",[id_договор] " +
                                                           ",[ФлагПодписания] " +
                                                           ",[ДатаПодписания] " +
                                                           ",[НомерПоПеречню] " +
                                                           ",[НаименованиеУслуги] " +
                                                           ",[Цена] " +
                                                           ",[Количество] " +
                                                           ",[Сумма] " +
                                                           ",[ФлагДопСоглашение] " +
                                                           ",[НомерРеестра] " +
                                                           ",[ДатаРеестра] " +
                                                           ",[НомерСчётФактуры] " +
                                                           ",[ДатаСчётФактуры] " +
                                                           ",[ДатаОплаты] ) " +
                                                           "VALUES " +
                                                           "('" + tabAct.Rows[0]["НомерАкта"].ToString().Trim() + "' " +
                                                           "," + id_договор + " " +
                                                           ",'" + tabAct.Rows[0]["ФлагПодписания"].ToString().Trim() + "' " +
                                                           ",'" + датаАкта.Trim() + "' " +
                                                           ",'" + tabAct.Rows[0]["НомерПоПеречню"].ToString().Trim() + "' " +
                                                           ",'" + tabAct.Rows[0]["НаименованиеУслуги"].ToString().Trim() + "' " +
                                                           ",'" + tabAct.Rows[0]["Цена"].ToString().Trim() + "' " +
                                                           "," + Convert.ToInt32(tabAct.Rows[0]["Количество"]) + " " +
                                                           ",'" + tabAct.Rows[0]["Сумма"].ToString().Trim() + "' " +
                                                           ",'" + tabAct.Rows[0]["ФлагДопСоглашение"].ToString().Trim() + "' " +
                                                           ", '" + this.НомерРеестра.ToString().Trim() + "' " +
                                                           ", '" + this.ДатаРеестра + "' " +
                                                           ",'" + this.НомерСчётФактуры.ToString().Trim() + "' " +
                                                           ", '" + this.ДатаСчётФактуры + "' " +
                                                           ", '' ) ";


                                        builder.Append(insertAct);

                                        //Выполним запрос в единой транзакции
                                        //ExecuteQuery.Execute(builder.ToString());

                                        ExecuteQuery.Execute(builder.ToString().Trim(), con, transact);
                                        
                                        break;

                                    //}
                                    //catch
                                    //{
                                    //    MessageBox.Show("Заполнены не все поля");
                                    //    return;
                                    //}
                                }
                                else
                                {
                                    //MessageBox.Show("Акт № " + tabAct.Rows[0]["НомерАкта"].ToString().Trim() + " уже записан в базе данных");
                                }


                            }
                        }
                    }

                    // Завершим транзакцию
                    transact.Commit();

                    

                }

                    if (this.НомерРеестра != "" && this.НомерСчётФактуры != "" && this.ДатаРеестра != "" && this.ДатаСчётФактуры != "")
                    {
                        this.btnSlRead.Enabled = true;
                        this.btnReturnReestr.Enabled = true;
                    }

                MessageBox.Show("Договара записал");

                //Закроем форму
                this.Close();
               // }

                //if (formR.DialogResult == DialogResult.No)
                //{
                //    formR.Close();
                //}
            }
            else
            {
                MessageBox.Show("Реестр не прошёл проверку");
            }
        }

        private void btnSlRead_Click(object sender, EventArgs e)
        {
            //Установим русскую культуру
            CultureInfo newCInfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            newCInfo.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = newCInfo;


            //Получим наименование стоматологической поликлинники
            DataTable tabHosp = this.Unloads[0].Поликлинника;
            string hosp = tabHosp.Rows[0]["НаименованиеПоликлинники"].ToString().Trim();

            //Получим наименование льготной категории
            string льготКатегория = this.Unloads[0].ЛьготнаяКатегория.Trim();

            DataTable tabНачальник;

            //получим текущего начальника отдела автоматизации
            string queryНачальник = "select должность,фамилия,инициалы from ДолжностьАналитичУправ " +
                                    "where id_должность = (select id_должность from dbo.ПодписьДолжность) ";

            using(SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
            {
                con.Open();
                SqlTransaction transact= con.BeginTransaction();
                
                tabНачальник = ТаблицаБД.GetTableSQL(queryНачальник,"Должность",con,transact);

            }

            //try
            //{

                //Получим должность начальника отдела
                string dolzhnost = tabНачальник.Rows[0]["должность"].ToString().Trim();

                //получим ФИО начальника отдела
                string familija = tabНачальник.Rows[0]["фамилия"].ToString().Trim();

                //Получим инициалы
                string inicialy = tabНачальник.Rows[0]["инициалы"].ToString().Trim();


                //Счётчик договоров
                int iCountContr = 0;

                decimal mani = 0.0m;

                //Подсчитаем количество договоров
                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["FlagError"].Value) == true)
                    {
                        iCountContr++;

                        //Подсчитаем сумму реестра выполненных договоров (Сумму актов)
                        string s1 = row.Cells["СуммаАкта"].Value.ToString();
                        string sum1 = s1.Replace("р.", " ");

                        string summ1 = sum1.Replace(',', '.');
                        mani = Math.Round(Math.Round(mani, 2) + Math.Round(Convert.ToDecimal(summ1), 2), 2);
                    }
                }

                //Ещё нужно получить ФИО начальника информационно-аналитического управлекния


                //Распечатаем служебную записку
                string filName = System.Windows.Forms.Application.StartupPath + @"\Шаблон\Шаблон служебной записки на оплату.doc";


                //Создаём новый Word.Application
                Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();

                //Загружаем документ
                Microsoft.Office.Interop.Word.Document doc = null;

                object fileName = filName;
                object falseValue = false;
                object trueValue = true;
                object missing = Type.Missing;

                doc = app.Documents.Open(ref fileName, ref missing, ref trueValue,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing);

                ////запишем фио
                //object wdrepl2 = WdReplace.wdReplaceAll;
                ////object searchtxt = "GreetingLine";
                //object searchtxt2 = "fio";
                //object newtxt2 = (object)fio;
                ////object frwd = true;
                //object frwd2 = false;
                //doc.Content.Find.Execute(ref searchtxt2, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd2, ref missing, ref missing, ref newtxt2, ref wdrepl2, ref missing, ref missing,
                //ref missing, ref missing);

                //запишем фио
                object wdrepl2 = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt2 = "поликлинника";
                object newtxt2 = (object)hosp;
                //object frwd = true;
                object frwd2 = false;
                doc.Content.Find.Execute(ref searchtxt2, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd2, ref missing, ref missing, ref newtxt2, ref wdrepl2, ref missing, ref missing,
                ref missing, ref missing);

                object wdrepl3 = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt3 = "льготкатегория";
                object newtxt3 = (object)льготКатегория;
                //object frwd = true;
                object frwd3 = false;
                doc.Content.Find.Execute(ref searchtxt3, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd3, ref missing, ref missing, ref newtxt3, ref wdrepl3, ref missing, ref missing,
                ref missing, ref missing);

                object wdrepl4 = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt4 = "количество";
                object newtxt4 = (object)iCountContr;
                //object frwd = true;
                object frwd4 = false;
                doc.Content.Find.Execute(ref searchtxt4, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd4, ref missing, ref missing, ref newtxt4, ref wdrepl4, ref missing, ref missing,
                ref missing, ref missing);

                object wdrepl5 = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt5 = "mani";
                object newtxt5 = (object)mani;
                //object frwd = true;
                object frwd5 = false;
                doc.Content.Find.Execute(ref searchtxt5, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd5, ref missing, ref missing, ref newtxt5, ref wdrepl5, ref missing, ref missing,
                ref missing, ref missing);

                object wdrepl6 = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt6 = "familija";
                object newtxt6 = (object)familija;
                //object frwd = true;
                object frwd6 = false;
                doc.Content.Find.Execute(ref searchtxt6, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd6, ref missing, ref missing, ref newtxt6, ref wdrepl6, ref missing, ref missing,
                ref missing, ref missing);

                object wdrepl7 = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt7 = "dolzhnost";
                object newtxt7 = (object)dolzhnost;
                //object frwd = true;
                object frwd7 = false;
                doc.Content.Find.Execute(ref searchtxt7, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd7, ref missing, ref missing, ref newtxt7, ref wdrepl7, ref missing, ref missing,
                ref missing, ref missing);

                object wdrepl8 = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt8 = "inicialy";
                object newtxt8 = (object)inicialy;
                //object frwd = true;
                object frwd8 = false;
                doc.Content.Find.Execute(ref searchtxt8, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd8, ref missing, ref missing, ref newtxt8, ref wdrepl8, ref missing, ref missing,
                ref missing, ref missing);


                object wdrepl9 = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt9 = "num";
                object newtxt9 = (object)this.НомерСчётФактуры.Trim();
                //object frwd = true;
                object frwd9 = false;
                doc.Content.Find.Execute(ref searchtxt9, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd9, ref missing, ref missing, ref newtxt9, ref wdrepl9, ref missing, ref missing,
                ref missing, ref missing);


                object wdrepl10 = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt10 = "date";
                object newtxt10 = (object)this.ДатаСчётФактуры.Trim();
                //object frwd = true;
                object frwd10 = false;
                doc.Content.Find.Execute(ref searchtxt10, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd10, ref missing, ref missing, ref newtxt10, ref wdrepl10, ref missing, ref missing,
                ref missing, ref missing);

                //Отобразим документ
                app.Visible = true;
            //}
            //catch (IndexOutOfRangeException i)
            //{
            //    MessageBox.Show("Возможно Вы не выбрали начальника аналитического отдела");
            //}

        }

        private void btnReturnReestr_Click(object sender, EventArgs e)
        {
            //Сохраним копию списка актов работ
            Dictionary<string, DisplayReestr> listАкт = this.АктыРабот;

            List<string> номераАктов = new List<string>();
            //Выберим не помеченные акты
            //Подсчитаем количество договоров
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["FlagError"].Value) == false && (Convert.ToBoolean(row.Cells["FlagAddContract"].Value) == false))
                {
                    номераАктов.Add(row.Cells["НомерАкта"].Value.ToString().Trim());
                }
                else
                {
                    //полчим номер акта
                    string keyAkt = row.Cells["НомерАкта"].Value.ToString();
                    listАкт.Remove(keyAkt);
                }
            }

            //Список хранит недостатки по всем актам в текущем реестре
            List<DisplayReestr> listReestr = new List<DisplayReestr>();

            //Проходим по актам и находим в них ошибки которые нам нужно отобразить
            //foreach (string drKey in this.АктыРабот.Keys)
            foreach (string drKey in listАкт.Keys)
            {
                foreach (string sNUm in номераАктов)
                {
                    if (drKey == sNUm)
                    {
                        DisplayReestr dr = this.АктыРабот[drKey];
                        //dr.НомерАкта = drKey;
                        listReestr.Add(dr);
                    }
                }
            }

            List<DisplayReestr> iListReestr = listReestr;

            //Узнаем какая у нас поликлинника
            DataRow rowПоликлинника = this.Unloads[0].Поликлинника.Rows[0];
            string инн = rowПоликлинника["ИНН"].ToString().Trim();

            string queryHosp = "SELECT [F2],[F3],[ФИО] " +
                               "FROM Лист1$ " +
                               "where id in ( " +
                               "SELECT [id_поликлинника] " +
                               "FROM Поликлинника " +
                               "where ИНН = '" + инн + "' )";
            
            string кому = string.Empty;
            string obrashhenie = string.Empty;
            string esculap = string.Empty;

            SqlConnection con = new SqlConnection(ConnectDB.ConnectionString());
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(queryHosp, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "Лист1$");

            //DataTable tabНачальник = ТаблицаБД.GetTableSQL(queryHosp, "Кому", con, transact);
            кому = ds.Tables["Лист1$"].Rows[0]["F2"].ToString().Trim();// +" " + ds.Tables["Лист1$"].Rows[0]["ФИО"].ToString().Trim();
            obrashhenie = ds.Tables["Лист1$"].Rows[0]["F3"].ToString().Trim();
            esculap = ds.Tables["Лист1$"].Rows[0]["ФИО"].ToString().Trim();

            //подсчитаем количество договоров указанныхз в реестре
            //Счётчик договоров
            int iCountContr = 0;

            //Подсчитаем количество договоров
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["FlagError"].Value) == false)
                {
                    iCountContr++;
                }
            }

            //выведим всё в WORD
            //Распечатаем служебную записку
            string filName = System.Windows.Forms.Application.StartupPath + @"\Шаблон\Письмо на возврат.doc";


            //Создаём новый Word.Application
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();

            //Загружаем документ
            Microsoft.Office.Interop.Word.Document doc = null;

            object fileName = filName;
            object falseValue = false;
            object trueValue = true;
            object missing = Type.Missing;

            doc = app.Documents.Open(ref fileName, ref missing, ref trueValue,
            ref missing, ref missing, ref missing, ref missing, ref missing,
            ref missing, ref missing, ref missing, ref missing, ref missing,
            ref missing, ref missing, ref missing);

            //выведим адресат письма
            object wdrepl2 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt2 = "кому";
            object newtxt2 = (object)кому;
            //object frwd = true;
            object frwd2 = false;
            doc.Content.Find.Execute(ref searchtxt2, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd2, ref missing, ref missing, ref newtxt2, ref wdrepl2, ref missing, ref missing,
            ref missing, ref missing);

            //Выведим обращение Уважаемый Иван Иванович!
            object wdrepl3 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt3 = "obrashhenie";
            object newtxt3 = (object)obrashhenie;
            //object frwd = true;
            object frwd3 = false;
            doc.Content.Find.Execute(ref searchtxt3, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd3, ref missing, ref missing, ref newtxt3, ref wdrepl3, ref missing, ref missing,
            ref missing, ref missing);


            //Выведим количество договоров
            object wdrepl4 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt4 = "icountcontr";
            object newtxt4 = (object)iCountContr;
            //object frwd = true;
            object frwd4 = false;
            doc.Content.Find.Execute(ref searchtxt4, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd4, ref missing, ref missing, ref newtxt4, ref wdrepl4, ref missing, ref missing,
            ref missing, ref missing);

            object wdrepl5 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt5 = "num";
            object newtxt5 = (object)this.НомерСчётФактуры;
            //object frwd = true;
            object frwd5 = false;
            doc.Content.Find.Execute(ref searchtxt5, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd5, ref missing, ref missing, ref newtxt5, ref wdrepl5, ref missing, ref missing,
            ref missing, ref missing);


            object wdrepl6 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt6 = "date";
            object newtxt6 = (object)this.ДатаРеестра;
            //object frwd = true;
            object frwd6 = false;
            doc.Content.Find.Execute(ref searchtxt6, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd6, ref missing, ref missing, ref newtxt6, ref wdrepl6, ref missing, ref missing,
            ref missing, ref missing);

            //esculap
            object wdrepl7 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt7 = "esculap";
            object newtxt7 = (object)esculap;
            //object frwd = true;
            object frwd7 = false;
            doc.Content.Find.Execute(ref searchtxt7, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd7, ref missing, ref missing, ref newtxt7, ref wdrepl7, ref missing, ref missing,
            ref missing, ref missing);


            object bookNaziv = "таблица";
            Range wrdRng = doc.Bookmarks.get_Item(ref  bookNaziv).Range;

            object behavior = Microsoft.Office.Interop.Word.WdDefaultTableBehavior.wdWord8TableBehavior;
            object autobehavior = Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitWindow;

            Microsoft.Office.Interop.Word.Table table = doc.Tables.Add(wrdRng, 1, 3, ref behavior, ref autobehavior);
            table.Range.ParagraphFormat.SpaceAfter = 6;
            table.Columns[1].Width = 180;
            table.Columns[2].Width = 180;
            table.Columns[3].Width = 180;
            table.Borders.Enable = 1; // Рамка - сплошная линия
            table.Range.Font.Name = "Times New Roman";
            table.Range.Font.Size = 12;
             //table.Rows[1].Alignment = WdRowAlignment.wdAlignRowCenter;
            table.Rows.First.Alignment = WdRowAlignment.wdAlignRowCenter;
            
            //счётчик строк
            int i = 1;

            //Список для хранения данных для таблицы
            List<ReestrNoPrintDog> listItem = new List<ReestrNoPrintDog>();

            //хранит список данных для договора
            List<TabLetter> list = new List<TabLetter>();

            //Сформируем шапку таблицы
            TabLetter шапка = new TabLetter();
            шапка.ФИО = "ФИО льготника";
            шапка.УслугаДоговор = "Проект договора";
            шапка.УслугаАкт = "Акт выполненных работ";

            list.Add(шапка);

  

            //Подготовим данные для таблицы
            foreach (DisplayReestr item in listReestr)
            {
                int iCount = 0;

                bool flagError = false;
                if (item.ErrorPerson == true)
                {
                    flagError = true;
                }

                //Получим список ошибок
                List<ErrorsReestrUnload> unRest = item.СписокОшибок;
                foreach (ErrorsReestrUnload eru in unRest)
                {
                    //письмо содержит расхождение в договоре с актом
                    TabLetter tLet = new TabLetter();

                    //получим ФИО льготника
                    ReestrControl льготник = item.Льготник;
                    string фио = льготник.ФИО;
                    //Заполним ФИО льготника только при первой итерации по списку ошибок
                    if (iCount == 0)
                    {
                        tLet.ФИО = фио;

                        string услуга = string.Empty;
                        string количество = string.Empty;
                        string сумма = string.Empty;

                        //Подготовим данные по договору для записи в одну строку
                        if (eru.НаименованиеУслуги != null)
                        {
                            услуга = eru.НаименованиеУслуги.ToString().Trim();
                        }
                        else
                        {
                            услуга = "";
                        }

                        if (eru.Количество != null)
                        {
                            количество = eru.Количество.ToString().Trim();
                        }

                        if (eru.Сумма != null)
                        {
                            сумма = eru.Сумма.ToString("c").Trim();
                            if (сумма == "0,00р.")
                            {
                                //сумма = "_______";
                                сумма = "";
                            }
                        }
                        else
                        {
                            сумма = "";
                        }


                        string услугаДоговор = услуга + " " + сумма;

                        //запишем данные по договору
                        tLet.УслугаДоговор = услугаДоговор.Trim();

                        string услуга2 = string.Empty;
                        string количество2 = string.Empty;
                        string сумма2 = string.Empty;

                        //Подготовим данные по акту для записи
                        if (eru.ErrorНаименованиеУслуги != null)
                        {
                            услуга2 = eru.ErrorНаименованиеУслуги.ToString().Trim();

                        }
                        else
                        {
                            услуга2 = "";
                        }

                        if (eru.ОшибкаКоличество != null)
                        {
                            количество2 = eru.ОшибкаКоличество.ToString().Trim();
                        }

                        if (eru.ErrorСумма != null)
                        {
                            сумма2 = eru.ErrorСумма.ToString("c").Trim();
                            if (сумма2 == "0,00р.")
                            {
                                //сумма2 = "_______";
                                сумма2 = "";
                            }
                        }
                        else
                        {
                            сумма2 = "";
                        }
                        
                        //string услугаДоговор2 = услуга2 + " " + количество2 + " " + сумма2;
                        string услугаДоговор2 = услуга2 + " " + сумма2;

                        //если ошибка в персональных данных
                        if (item.ErrorPerson == true)
                        {
                            услугаДоговор2 += "Изменены данные по льготнику";
                        }
                                

                        tLet.УслугаАкт = услугаДоговор2;
                    }
                    else
                    {
                        //в ФИО записываем пустую строку
                        tLet.ФИО = "";

                        string услуга = string.Empty;
                        string количество = string.Empty;
                        string сумма = string.Empty;

                        //Подготовим данные по договору для записи в одну строку
                        if (eru.НаименованиеУслуги != null)
                        {
                            услуга = eru.НаименованиеУслуги.ToString().Trim();
                        }
                        else
                        {
                            услуга = "";
                        }

                        if (eru.Количество != null)
                        {
                            количество = eru.Количество.ToString().Trim();
                        }

                        if (eru.Сумма != null)
                        {
                            сумма = eru.Сумма.ToString("c").Trim();
                            if (сумма == "0,00р.")
                            {
                                //сумма = "_______";
                                сумма = "";
                            }

                        }
                        else
                        {
                            сумма = "";
                        }


                        string услугаДоговор = услуга + " " + сумма;

                        //запишем данные по договору
                        tLet.УслугаДоговор = услугаДоговор.Trim();

                        string услуга2 = string.Empty;
                        string количество2 = string.Empty;
                        string сумма2 = string.Empty;

                        //Подготовим данные по акту для записи
                        if (eru.ErrorНаименованиеУслуги != null)
                        {
                            услуга2 = eru.ErrorНаименованиеУслуги.ToString().Trim();
                        }
                        else
                        {
                            услуга2 = "";
                        }

                        if (eru.ОшибкаКоличество != null)
                        {
                            количество2 = eru.ОшибкаКоличество.ToString().Trim();
                        }


                        if (eru.ErrorСумма != null)
                        {
                            сумма2 = eru.ErrorСумма.ToString("c").Trim();
                            if (сумма2 == "0,00р.")
                            {
                                сумма2 = "";
                            }
                        }
                        else
                        {
                            сумма2 = "";
                        }

                        //string услугаДоговор2 = услуга2 + " " + количество2 + " " + сумма2;
                        string услугаДоговор2 = услуга2 + " " + сумма2;
                        tLet.УслугаАкт = услугаДоговор2;
                    }

                    list.Add(tLet);

                    iCount++;
                }
            }

            List<TabLetter> iTest = list;

            int k = 1;

            //запишем данные в таблицу
            foreach (TabLetter item in list)
            {
                table.Cell(k, 1).Range.Text = item.ФИО.Trim();
                table.Cell(k, 2).Range.Text = item.УслугаДоговор.Trim();
                table.Cell(k, 3).Range.Text = item.УслугаАкт.Trim();


                //doc.Words.Count.ToString();
                Object beforeRow1 = Type.Missing;
                table.Rows.Add(ref beforeRow1);

                k++;
            }

            table.Rows[k].Delete();

            //Выровним первую строку по центру
            //table.Rows[1].Alignment = WdRowAlignment.wdAlignRowCenter;

            //Отобразим документ
            app.Visible = true;

        }

        private void numReestr_Click(object sender, EventArgs e)
        {
                FormReestr formR = new FormReestr();
                formR.ShowDialog();

                if (formR.DialogResult == DialogResult.OK)
                {
                    this.НомерСчётФактуры = formR.НомерСчётФактуры.Trim();
                    this.НомерРеестра = formR.НомерРеестра.Trim();
                    this.ДатаРеестра = formR.ДатаРеестра.Trim();
                    this.ДатаСчётФактуры = formR.ДатаСчётФактуры.Trim();

                    this.btnSlRead.Enabled = true;
                    this.btnReturnReestr.Enabled = true;
                    this.btnSave.Enabled = true;
                }

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("Test Message");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder buildNumberContract = new StringBuilder();

            // УЗнаем количество строк в таблице.
            int iCountRow = this.dataGridView1.Rows.Count;

            // Счетчик.
            int iCount = 0;

            // Библиотека для хранения номеров контрактов.
            Dictionary<string, ItemLetSumDataAct> dNumContract = new Dictionary<string, ItemLetSumDataAct>();

            // Построим строку с SQL запросами.
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                iCount++;


                if (iCount < iCountRow)
                {
                    // Добавим к запросу подстроку с номером договора, строка заканчивается запятой.
                    string sItem = "LOWER(LTRIM(RTRIM('" + row.Cells["НомерДоговора"].Value.ToString().Trim() + "'))),";

                    buildNumberContract.Append(sItem);
                }
                else
                {
                    // Добавляем поледний номер договора в перечислении без запятой.
                    string sItem = "LOWER(LTRIM(RTRIM('" + row.Cells["НомерДоговора"].Value.ToString().Trim() + "')))";

                    buildNumberContract.Append(sItem);
                }

                // Получим номер контракта.
                string numContract = row.Cells["НомерДоговора"].Value.ToString().Trim();

                var sumActString = row.Cells["СуммаАкта"].Value.ToString().Replace(",",".").Replace("Р",string.Empty).Replace("P",string.Empty).Replace("р",string.Empty).Replace("p",string.Empty);

                // Удалим последную точку в строке с суммой акта.
                int length = sumActString.Length;

                var summaActStr = sumActString.Substring(0, length - 1);

                // Получим сумму акта.
                decimal sumAct = Convert.ToDecimal(summaActStr);

                // Вспомогательный класс описывающий сумму акта выполненных работ и дату подписания акта.
                ItemLetSumDataAct sumDataAct = new ItemLetSumDataAct();

                // Сумма акта.
                sumDataAct.SummAct = sumAct;

                // Дата подписания акта выполненных работ.
                sumDataAct.DateAct = row.Cells["ДатаАкта"].Value.ToString().Trim();

                // Запишем ключ и сумму акта в бибилиотеку.
                dNumContract.Add(numContract, sumDataAct);
                
            }

            // Список для хранения данных в письме.
            //List<ItemLetterToMinistr> listLetter = new List<ItemLetterToMinistr>();

            ContractsForLetter contractLetter = new ContractsForLetter();
            List<ItemLetterToMinistr> listContract = contractLetter.GetPersons(buildNumberContract.ToString());

            ControlDantist.ValidPersonRegion.Region region = new ValidPersonRegion.Region();
            region.GetRegions(listContract);

            var listPerson = listContract;

            string iTest = "";

            //Пройдемся по библиотеке и проставим сумму акта в список договоров.
            foreach (var sum in dNumContract)
            {
                var item = listContract.Where(w => w.НомерДоговора.ToLower().Trim() == sum.Key.ToLower().Trim()).FirstOrDefault();

                if (item != null)
                {

                    item.СуммаАкта = sum.Value.SummAct;

                    item.ДатаАкта = sum.Value.DateAct.Trim();
                }
            }

            if (Directory.Exists(System.Windows.Forms.Application.StartupPath + @"\Документы VipNet") == false)
            {
                // Создадим папку.
                Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + @"\Документы VipNet");
            }
            else
            {

                DirectoryInfo dirInfo = new DirectoryInfo(System.Windows.Forms.Application.StartupPath + @"\Документы VipNet");

                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
            }
                

                DirectoryInfo dirInfoVip = new DirectoryInfo(System.Windows.Forms.Application.StartupPath + @"\Документы VipNet");

                foreach (FileInfo file in dirInfoVip.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch
                    {
                        MessageBox.Show("Не могу получить доступ к файлу - " + file.Name);

                        return;
                    }
                }

           // Сгруппируем списки по районам.
            var group = listContract.GroupBy(w => w.РайонОбласти);

            // Счетчик циклов.
            int rCount = 1;

            //Очистим директорию от файлов.
            //ClearDirecory();

            // Пройдемся по группам и создадим титульный лист VipNet и файл Excel для каждой группы.
            foreach (var listContractFile in group)
            {

                // Переменная для хранения имени района области.
                string nameRegion = string.Empty;

                // Получим первую строку из реестра.
                var rtest = listContractFile.Count();

                // Получим название районаобласти
                var filePath = listContractFile.First().РайонОбласти;

                // Сведения для отправления формирования сопроводительного письма VipNet.
                if(listContractFile.First().IdRegion !=null)
                {
                    int idRegion = (int)listContractFile.First().IdRegion;

                    string NameFile = filePath + ".xls";

                    // Сформируем титульный лист VipNet.
                    VipNetLetter vipNetLett = new VipNetLetter(idRegion, NameFile);
                    vipNetLett.CreateLetter(out nameRegion);
                }
                else
                {
                    MessageBox.Show(listContractFile.First().РайонОбласти + "Не опознан id района в БД");
                }


                if (nameRegion.Trim() == "".Trim())
                {
                    nameRegion = "Ошибка регион " + rCount.ToString();
                }
                
                ////string fileName = @"c:\Письма в Министерство\" + filePath + ".xls";
                string fileName = System.Windows.Forms.Application.StartupPath + @"\Документы VipNet\" + nameRegion + ".xls";

                GenerateExcelFileLetter file = new GenerateExcelFileLetter(fileName);
                file.GenerateFile(listContractFile.ToList());

                rCount++;
            }

            MessageBox.Show("Реестр VipNet составлен");
           
        }

        /// <summary>
        /// Очищает директорию от VipNet документов
        /// </summary>
        private void ClearDirecory()
        {
            string nameDir = "\\Документы VipNet\\";
            string nsmeDirectory = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + nameDir;

            DirectoryInfo dirInfo = new DirectoryInfo(nsmeDirectory);

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }
        }

        private void dataGridView1_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //if (Convert.ToInt32(this.dataGridView1.Columns["НомерДоговора"]) > 0)
            //{

            //}

            if (Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells["FlagErrorActОплата"].Value) == true)
            {
                FormMessageAct form = new FormMessageAct();
                form.НомерАкта = this.dataGridView1.CurrentRow.Cells["ОплаченныйРанееДоговорАкт"].Value.ToString().Trim();
                form.Show();
            }

        }


    }
}