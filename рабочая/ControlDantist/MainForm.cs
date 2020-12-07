using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Linq;
using System.Data.OleDb;

using Microsoft.Office.Interop.Word;
using DantistLibrary;
using ControlDantist.Classes;

using System.Globalization;
using System.Threading;

using ControlDantist.ClassValidRegions;
using ControlDantist.Repozirories;

using ControlDantist.Repository;
using ControlDantist.ReceptionDocuments;
using ControlDantist.ValidateRegistrProject;

using ControlDantist.WriteClassDB;
using ControlDantist.Find;
using ControlDantist.FindEsrnWoW;



namespace ControlDantist
{
    public partial class MainForm : Form
    {
        //Переменная хранит id поликлинники которой принадлежит файл реестра
        private int idHosp;
        private string видУслуг = string.Empty;
        private decimal цена = 0.0m;
        private decimal суммаСервер = 0.0m;

        //переменная хранит метку о том что реестр содержит ошибки
        //если false то ошибок нет, если true то реестр содержит ошибку
        private bool errorFlagВидУслуг = false;
        private bool errorFlagЦена = false;
        private bool errorFlagСтоимостьУслуги = false;

        //флаг хранит состояние ошибки в реестре целиком
        private bool errorРеестр = false;

        //пременная хранит итоговую сумму услуг для данного льготника
        private decimal суммаСтоимостьУслуг = 0.0m;

        //переменная хранит ошибочную стоимость услуг по данному льготнику
        private decimal errorСуммаСтоимостьУслуг = 0.0m;

        //переменная хранит итоговую сумму по реестру
        private decimal суммаВсегоРеестр = 0.0m;

        //Переменная хранит название льготной категории
        private string льготнаяКатегория = string.Empty;

        //Переменная для хранения коллекции проектов договоров
        Dictionary<string, Unload> unload;

        /// <summary>
        /// Позволяет загрузить всю базу
        /// </summary>
        private bool flagЗагрузкаБД = false;

        /// <summary>
        /// Флаг указывает подключать сервера или нет.
        /// </summary>
        public bool FlagConnectServer { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Установим для нашей программы текущую директорию для корректного считывания пути к БД
            Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открыть файл реестра";
            //openFileDialog1.Filter = "|*.r";
            openFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string fileName = openFileDialog1.FileName;

                //SerializableObject objToSerialize = null;
                FileStream fstream = File.Open(fileName, FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                //получим выгрузку реестра выполненных договоров
                List<Unload> unloads = (List<Unload>)binaryFormatter.Deserialize(fstream);

                // Закроем поток.
                fstream.Dispose();

                // Пройдемся по выгрузке циклично.
                foreach (var unl in unloads)
                {
                    // Получим таблицу с договором.
                    DataTable numDog = unl.Договор;

                    foreach (DataRow row in numDog.Rows)
                    {
                        if (row[1].ToString() == "8/6815")
                        {
                            string iTest = "test";
                        }
                    }

                    var itemUnload = unl;
                }

                IEnumerable<Unload> unloadS = unloads.Where(un => un.АктВыполненныхРабот != null).Select(un => un).ToList();

                List<Unload> unload = new List<Unload>();

                unload.AddRange(unloadS);

               // Список выгрузки с договорам.
                List<Unload> iUnload = unload;

                string queryFlag = "select [УслугиДоговора] from [TableКонфигурацияУслугиДоговора] " +
                                 "WHERE idConfig = 1";

                int intFlag = Convert.ToInt32(ТаблицаБД.GetTableSQL(queryFlag, "TabConfig").Rows[0]["УслугиДоговора"]);

                if (intFlag == 1)
                {
                    FormИсправленияУслугДоговоров formCorrect = new FormИсправленияУслугДоговоров();
                    formCorrect.ListUnload = unload;
                    formCorrect.Show();

                    return;
                }

                ////Создадим список который содержит расхождения в реестре
                List<ErrorReestr> list = new List<ErrorReestr>();

                //Создадим список который содержит реестр который прошёл проверку 
                List<ReestrControl> listControlReestr = new List<ReestrControl>();

                //Словарь с результатами проверки
                Dictionary<string, DisplayReestr> listReest = new Dictionary<string, DisplayReestr>();

                //Откроем соединение с БД на сервере
                using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
                {
                    con.Open();
                    //Выполним всё в единой транзакции
                    SqlTransaction transact = con.BeginTransaction();

                    //Обнулим сумму стоимосит услуг
                    суммаСтоимостьУслуг = 0.0m;

                    //обнулим список с ошибками
                    list.Clear();

                    listReest.Clear();

                    listControlReestr.Clear();

                    #region Старая реализация
                    //Сравним содержимое реестра с записями в базе данных

                    foreach (Unload un in unload)
                    {
                        ErrorReestr errorReestr = new ErrorReestr();

                        string лгт = un.Льготник.Rows[0][1].ToString();
                        if (лгт == "Мельник")
                        {
                            string s = "test";
                        }

                        //Создадим экземпляр объекта для хранения строки реестра на оказанные услуги успешно прошедшие проверку
                        ReestrControl rControl = new ReestrControl();

                        //Получим ФИО льготника которого запишем в случае ошибки в реестр
                        DataRow rowЛьготник = un.Льготник.Rows[0];

                        string фамилия = rowЛьготник["Фамилия"].ToString().Trim();

                        string имя = rowЛьготник["Имя"].ToString().Trim();
                        string отчество = rowЛьготник["Отчество"].ToString().Trim();

                        //получим дату рождения
                        string queryДатаДоговора = "";

                        //Запишем в реестр ФИО текущего льготника
                        errorReestr.ФИО = фамилия + " " + имя + " " + отчество;

                        //Запишем ФИО льготника
                        rControl.ФИО = фамилия + " " + имя + " " + отчество;

                        //Запишем дату и номер договора на оказание услуг
                        DataRow rowControlReestrДоговор = un.Договор.Rows[0];

                        //Запишем номер поликлинники и номер договора
                        string номерДоговора = rowControlReestrДоговор["НомерДоговора"].ToString();

                        //Получим на сервере данные по договору и сравним их с получиными данными и с данными на льготника

                        //Запишем дату договора
                        string датаДоговора = Convert.ToDateTime(rowControlReestrДоговор["ДатаДоговора"]).ToShortDateString();


                        //запишем дату и номер договора в реестр
                        rControl.ДатаНомерДоговора = номерДоговора + " " + датаДоговора;

                        //Запишем дату и номер акта оказанных услуг
                        DataRow rowControlReestrАкт = un.АктВыполненныхРабот.Rows[0];

                        //Получим номер акта 
                        string номерАкта = rowControlReestrАкт["НомерАкта"].ToString();

                        //Запишем дату акта оказанных услуг
                        string датаАкта = Convert.ToDateTime(rowControlReestrАкт["ДатаПодписания"]).ToShortDateString();

                        //Создадим экземпляр класса DisplayReestr
                        DisplayReestr dispR = new DisplayReestr();
                        dispR.НомерАкта = датаАкта;

                        //запишем номер акта выполненных работ
                        rControl.Номер = номерАкта.Trim();

                        //Запишем в реестр номер и дату акта оказанных услуг 
                        rControl.НомерАктаОказанныхУслуг = номерАкта + " " + датаАкта;

                        //Получим серию и номер документа о праве на льготу
                        DataRow rowПравоЛьготы = un.Льготник.Rows[0];

                        //Серия документа
                        string серия = rowПравоЛьготы["СерияДокумента"].ToString();

                        //Запишем номер поликлинники и номер договора
                        string номерДокумента = rowПравоЛьготы["НомерДокумента"].ToString();

                        //Запишем дату договора
                        string датаДокумента = Convert.ToDateTime(rowПравоЛьготы["ДатаВыдачиДокумента"]).ToShortDateString();

                        rControl.ДокументЛьгота = серия + " " + номерДокумента + " " + датаДокумента;

                        //Запишем название льготной категории
                        льготнаяКатегория = un.ЛьготнаяКатегория;

                        //Создадим список который содержит расхождения в реестре
                        List<ErrorsReestrUnload> listError = new List<ErrorsReestrUnload>();

                        //Узнаем какой поликлиннике принадлежит файл реестра
                        DataRow rowHosp = un.Поликлинника.Rows[0];

                        //Получим id поликлинники
                        string queryIdHosp = "select id_поликлинника from dbo.Поликлинника where ИНН = '" + rowHosp["ИНН"].ToString() + "' ";

                        SqlCommand com = new SqlCommand(queryIdHosp, con);
                        com.Transaction = transact;
                        SqlDataReader read = com.ExecuteReader();

                        while (read.Read())
                        {
                            idHosp = Convert.ToInt32(read["id_поликлинника"]);
                        }

                        read.Close();

                        /*проверим что к текущему договору 
                         * подключен именно этот льготник, его ФИО, паспортные данные
                         * кем выдан паспорт, его адрес, 
                         * 
                         * */

                        if (номерДоговора.Trim() == "ОРБ/56")
                        {
                            string i2Test = "Test";
                        }

                        //получаем id договора
                        string sid_догServ = "select top 1 id_договор from Договор where НомерДоговора = '" + номерДоговора.Trim() + "' and ФлагПроверки = 'True' ";
                        DataTable tab_idServ = ТаблицаБД.GetTableSQL(sid_догServ, "Договор", con, transact);

                        //проверим существует ли такой договор у нас на сервере
                        if (tab_idServ.Rows.Count != 0)
                        {
                   
                            string sЛьготник = " SELECT     dbo.Льготник.Фамилия, dbo.Льготник.Имя, dbo.Льготник.Отчество, dbo.Льготник.ДатаРождения, dbo.Льготник.улица, dbo.Льготник.НомерДома, " +
                                                " dbo.Льготник.корпус, dbo.Льготник.НомерКвартиры, dbo.Льготник.СерияПаспорта, dbo.Льготник.НомерПаспорта, " +
                                                "dbo.Льготник.ДатаВыдачиПаспорта, dbo.Льготник.КемВыданПаспорт, dbo.Льготник.СерияДокумента, dbo.Льготник.НомерДокумента,  " +
                                                "dbo.Льготник.ДатаВыдачиДокумента, dbo.Льготник.КемВыданДокумент, dbo.Льготник.id_льготнойКатегории,  " +
                                                "dbo.ЛьготнаяКатегория.ЛьготнаяКатегория " +
                                                "FROM         dbo.Льготник INNER JOIN " +
                                                "dbo.ЛьготнаяКатегория ON dbo.Льготник.id_льготнойКатегории = dbo.ЛьготнаяКатегория.id_льготнойКатегории " +
                                                "WHERE     (dbo.Льготник.id_льготник IN " +
                                                "(SELECT     id_льготник " +
                                                "FROM          dbo.Договор " +
                                                "WHERE ( LOWER(LTRIM(RTRIM(НомерДоговора))) = '" + номерДоговора.Trim().ToLower() + "') AND (ФлагПроверки = 'True'))) ";


                            DataTable tab_Льготник = ТаблицаБД.GetTableSQL(sЛьготник, "Льготник", con, transact);

                            foreach (DataRow rowFIO in tab_Льготник.Rows)
                            {
                                if (фамилия != rowFIO["Фамилия"].ToString().Trim())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                if (имя != rowFIO["Имя"].ToString().Trim())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                if (отчество != rowFIO["Отчество"].ToString().Trim())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                //дата рожддения
                                string датРожд = Convert.ToDateTime(rowЛьготник["ДатаРождения"]).ToShortDateString();
                                if (датРожд != Convert.ToDateTime(rowFIO["ДатаРождения"]).ToShortDateString())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                //улица
                                string улица = rowЛьготник["улица"].ToString().Trim();
                                if (улица != rowFIO["улица"].ToString().Trim())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                //номер дома
                                string номерДома = rowЛьготник["НомерДома"].ToString().Replace("'", string.Empty).Trim();
                                if (номерДома != rowFIO["НомерДома"].ToString().Trim())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                //корпус
                                string корпус = rowЛьготник["корпус"].ToString().Trim();
                                if (корпус != rowFIO["корпус"].ToString().Trim())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                //номер квартиры
                                string numApartment = rowЛьготник["НомерКвартиры"].ToString().Trim();
                                if (numApartment != rowFIO["НомерКвартиры"].ToString().Trim())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                //серия паспорта
                                string серияПаспорта = rowЛьготник["СерияПаспорта"].ToString().Trim();
                                if (серияПаспорта != rowFIO["СерияПаспорта"].ToString().Trim())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                //номер паспорта
                                string serPassport = rowЛьготник["НомерПаспорта"].ToString().Trim();
                                if (serPassport != rowFIO["НомерПаспорта"].ToString().Trim())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                //ДатаВыдачиПаспорта
                                //string датаВыдачиПаспорта =  rowЛьготник["ДатаВыдачиПаспорта"].ToString().Trim();
                                string датаВыдачиПаспорта = Convert.ToDateTime(rowЛьготник["ДатаВыдачиПаспорта"]).ToShortDateString();
                                //if (датаВыдачиПаспорта != rowFIO["ДатаВыдачиПаспорта"].ToString().Trim())
                                if (датаВыдачиПаспорта != Convert.ToDateTime(rowFIO["ДатаВыдачиПаспорта"]).ToShortDateString())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                //КемВыданПаспорт
                                string кемВыданПаспорт = rowЛьготник["КемВыданПаспорт"].ToString().Trim();
                                if (кемВыданПаспорт != rowFIO["КемВыданПаспорт"].ToString().Trim())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                //серия документа
                                string серияДокумента = rowЛьготник["СерияДокумента"].ToString().Trim();
                                if (серияДокумента != rowFIO["СерияДокумента"].ToString().Trim())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                //номер документа
                                string numДокумента = rowЛьготник["НомерДокумента"].ToString().Trim();
                                if (numДокумента != rowFIO["НомерДокумента"].ToString().Trim())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                //дата выдачи документа
                                //string датаВыдачиДокумента = rowЛьготник["ДатаВыдачиДокумента"].ToString().Trim();
                                string датаВыдачиДокумента = Convert.ToDateTime(rowЛьготник["ДатаВыдачиДокумента"]).ToShortDateString();//.ToString().Trim();
                                if (датаВыдачиДокумента != Convert.ToDateTime(rowFIO["ДатаВыдачиДокумента"]).ToShortDateString())// rowFIO["ДатаВыдачиДокумента"].ToString().Trim())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }

                                //КемВыданДокумент
                                string кемВыданДокумент = rowЛьготник["КемВыданДокумент"].ToString().Trim();
                                if (кемВыданДокумент != rowFIO["КемВыданДокумент"].ToString().Trim())
                                {
                                    //ошибка
                                    dispR.FlagError = false;

                                    //ошибка персональных данных
                                    dispR.ErrorPerson = true;
                                }


                                //Пока оставим тестовое воздействие.
                                if (номерДоговора.Trim() == "ОРБ/56")
                                {
                                    //un.ЛьготнаяКатегория = "Другая льготная категория";
                                    string sTest = "";
                                }

                                if (un.ЛьготнаяКатегория.ToLower().Replace(" ", string.Empty).Trim() != rowFIO["ЛьготнаяКатегория"].ToString().Trim().ToLower().Replace(" ", string.Empty))
                                {
                                    // Ошибка нет совпадения по льготной категории.
                                    dispR.ErrorPrefCategory = true;
                                }

                                // Запишем льготную категорию из БД и льготную категорию из файла.
                                PreferentCategory pc = new PreferentCategory();
                                pc.PActegoryFile = un.ЛьготнаяКатегория.Trim();
                                pc.PCategoryServer = rowFIO["ЛьготнаяКатегория"].ToString().Trim();

                                dispR.ЛьготнаяКатегория = pc;
                            }

                            //Обнулим переменную для хранения суммы услуг у нас на сервере
                            суммаСервер = 0.0m;

                            //переменная для хранения суммы по договору хранящееся у нас на сервере
                            decimal summaServer = 0.0m;

                            //счётчик циклов
                            int iCount = 0;

                            //Получим список услуг по текущему договору из файла выгрузки
                            DataTable tabДоговор = un.УслугиПоДоговору;
                            foreach (DataRow rowDog in tabДоговор.Rows)
                            {
                                //Обнулим видУслуги 
                                видУслуг = string.Empty;
                                цена = 0.0m;
                                //суммаСервер = 0.0m;

                                //переменная хранит номер услуги по перечню;
                                string номерПоПеречню = string.Empty;

                                //Создадим экземпляр объекта для хранения ошибочной информации для конкретного льготника
                                ErrorsReestrUnload error = new ErrorsReestrUnload();

                                string linkText = "'" + rowDog["НаименованиеУслуги"].ToString() + "'";

                                //получим id текущего договора
                                int id_договор = Convert.ToInt32(rowDog["id_договор"]);

                                //Проверим на задвоенность услуг по договору (Есть такой баг)


                                //Получим услуги которые из
                                DataTable dtУслуги = un.УслугиПоДоговору;


                                //получим название услуги её номер по перечню и цену которая храниться у нас на сервере для данного договора (информацию получаем из сохранённых проектов договоров)
                                string queryViewServices = "select top 1  НаименованиеУслуги,Цена,НомерПоПеречню,Сумма from dbo.УслугиПоДоговору " +
                                                           "where НаименованиеУслуги = " + linkText + " and id_договор in( " +
                                                           "SELECT [id_договор] " +
                                                           "FROM [Договор] " +
                                                           "where [НомерДоговора] = '" + номерДоговора.Trim() + "' and ФлагПроверки = 'True' ) ";



                                SqlCommand comViewServ = new SqlCommand(queryViewServices, con);
                                comViewServ.Transaction = transact;
                                SqlDataReader readViewServ = comViewServ.ExecuteReader();

                                //если усулга не содержиться не в одной одной строке у нас на сервере запишем их из файла выгрузки
                                if (readViewServ.HasRows == false)
                                {
                                    //запишем правильное наименование
                                    error.ErrorНаименованиеУслуги = rowDog["НаименованиеУслуги"].ToString().Trim();

                                    //запишем не правильную цену
                                    error.ErrorЦена = Convert.ToDecimal(rowDog["Цена"]);

                                    //запишем не правильную сумму
                                    error.ErrorСумма = Convert.ToDecimal(rowDog["Сумма"]);

                                    //запишем результат проверки текущей строки в список ошибки
                                    listError.Add(error);
                                }

                                //Получим название услуги и стоимость которая находится у нас на сервере
                                while (readViewServ.Read())
                                {
                                    //обнулим переменные
                                    видУслуг = string.Empty;
                                    цена = 0.0m;
                                    номерПоПеречню = string.Empty;
                                    //суммаСервер = 0.0m;

                                    видУслуг = readViewServ["НаименованиеУслуги"].ToString().Trim();
                                    цена = Convert.ToDecimal(readViewServ["Цена"]);
                                    номерПоПеречню = readViewServ["НомерПоПеречню"].ToString().Trim();
                                    //суммаСервер = Convert.ToDecimal(readViewServ["Сумма"]);

                                    //Получим сумму для хранения суммы которая храниться у нас на сервере
                                    summaServer = Math.Round(Math.Round(summaServer, 2) + Math.Round(суммаСервер, 2), 2);

                                    if (номерДоговора.Trim() == "ОГВВ/54")
                                    {
                                        string sTest = "test";
                                    }

                                    //теперь сравним что лежит в базе и что лежит в файле реестра
                                    if (rowDog["НаименованиеУслуги"].ToString().Trim() == видУслуг.Trim() && rowDog["НомерПоПеречню"].ToString().Trim() == номерПоПеречню.Trim())
                                    {
                                        //ошибки нет
                                        errorFlagВидУслуг = false;

                                        //запишем вид услуг
                                        //видУслуг = string.Empty;

                                        //запишем правильное наименование
                                        error.НаименованиеУслуги = видУслуг.Trim();

                                        //запишем правильное наименование
                                        error.ErrorНаименованиеУслуги = rowDog["НаименованиеУслуги"].ToString().Trim();

                                        DateService ds = new DateService();
                                        ds.Наименование = видУслуг.Trim();

                                        ds.Цена = Convert.ToDecimal(цена).ToString("c");

                                    }
                                    else
                                    {
                                        //ошибка
                                        errorFlagВидУслуг = true;

                                        //запишем правильное наименование
                                        error.НаименованиеУслуги = видУслуг.Trim();

                                        //запишем ошибку
                                        error.ErrorНаименованиеУслуги = rowDog["НаименованиеУслуги"].ToString().Trim();

                                    }

                                    //теперь сравним стоимость услуги
                                    if (Convert.ToDecimal(rowDog["Цена"]) == цена && rowDog["НомерПоПеречню"].ToString().Trim() == номерПоПеречню.Trim())
                                    {
                                        //ошибки нет
                                        errorFlagЦена = false;

                                        //error.Цена = цена;
                                        //error.ErrorЦена = Convert.ToDecimal(rowDog["Цена"]);
                                    }
                                    else
                                    {
                                        //ошибка
                                        errorFlagЦена = true;

                                        //запишем разницу
                                        error.Цена = цена;
                                        error.ErrorЦена = Convert.ToDecimal(rowDog["Цена"]);
                                    }

                                    //теперь проверим правильно ли вычеслина сумма оказанных услуг по данному виду работы
                                    int количество = Convert.ToInt32(rowDog["Количество"]);

                                    //подсчитаем контрольную сумму стоимости услуг
                                    decimal сумма = Math.Round((Math.Round(цена, 2) * количество), 2);

                                    //Подсчитаем итоговую сумму услуг для конкретного льготника
                                    суммаСтоимостьУслуг = Math.Round((суммаСтоимостьУслуг + сумма), 2);

                                    //Обнулим переменную чтобы там не было мусора
                                    errorСуммаСтоимостьУслуг = 0.0m;

                                    //подсчитаем сумму в файле выгрузки для конкретного реестра
                                    errorСуммаСтоимостьУслуг = Math.Round((errorСуммаСтоимостьУслуг + Convert.ToDecimal(rowDog["Сумма"])), 2);

                                    //сравим сумму по услуге
                                    //if (Convert.ToDecimal(rowDog["Сумма"]) == сумма)
                                    if (Convert.ToDecimal(rowDog["Сумма"]) == сумма && Convert.ToDecimal(rowDog["Цена"]) == цена && rowDog["НомерПоПеречню"].ToString().Trim() == номерПоПеречню.Trim())
                                    {
                                        //ошибки нет
                                        errorFlagСтоимостьУслуги = false;
                                    }
                                    else
                                    {
                                        //ошибка
                                        errorFlagСтоимостьУслуги = true;

                                        //запишем разницу
                                        error.Сумма = сумма;
                                        error.ErrorСумма = Convert.ToDecimal(rowDog["Сумма"]);
                                    }

                                    //Сравним результат который у нас получился
                                    if (errorFlagВидУслуг == false && errorFlagЦена == false && errorFlagСтоимостьУслуги == false)
                                    {
                                        //ошибки в данном виде услуг не произошло
                                    }
                                    else
                                    {
                                        //произошла ошибка и мы выставим флаг реестра в ошибку 
                                        errorРеестр = true;

                                        //запишем результат проверки текущей строки в список ошибки
                                        listError.Add(error);
                                    }

                                }

                                //Запишем сумму услуг для текущего договора которая храниться у нас на сервере
                                dispR.СуммаДоговорСервер = summaServer;

                                //закроем DataReader
                                readViewServ.Close();

                                //увеличим счётчик на 1
                                iCount++;

                                //соберём новый экземпляр класса который содержит listError и rControl
                            }

                            //Установим флаг указывающий что есть ошибка в проверке или нет
                            if (listError.Count == 0)
                            {
                                //ошибок нет
                                dispR.FlagError = true;
                            }
                            else
                            {
                                //добавим список ошибок
                                dispR.СписокОшибок = listError;

                                //флаг указывает что существуют расхождения с содержимым сервера
                                dispR.FlagError = false;
                            }

                            //Сравним стоимости услуг из файла и из сервера в БД
                            if (суммаСтоимостьУслуг == errorСуммаСтоимостьУслуг)
                            {
                                //Запишем стоимость услуг
                                rControl.СтоимостьУслуг = суммаСтоимостьУслуг.ToString();
                            }
                            else
                            {
                                errorReestr.СуммаИтогоСтоимостьУслуг = суммаСтоимостьУслуг;
                                errorReestr.ErrorСуммаИтогоСтоимостьУслуг = errorСуммаСтоимостьУслуг;
                            }

                            //запишем в реестр список содержащий услуги в которых обнаружены расхождения
                            errorReestr.ErrorListУслуги = listError;

                            if (errorРеестр == true)
                            {
                                //запишем в список с ошибками текущего льготника со всеми расхождениями
                                list.Add(errorReestr);
                            }

                            listControlReestr.Add(rControl);

                            dispR.Льготник = rControl;

                            //добавим сумму улуг
                            dispR.Sum = Math.Round(суммаСтоимостьУслуг, 2);

                            //перменная хранит количество услуг на сервере
                            int countService = 0;

                            //переменная хранит количество услуг в файле выгрузки
                            int countServiceFile = 0;


                            //ПРОВЕРИМ ЕСТЬ ЛИ РАСХОЖДЕНИЯ В КОЛИЧЕСТВАХ УСЛУГ НА СЕРВЕРЕ И В ФАЙЛЕ ВЫГРУЗКИ

                            //получим количество услуг у нас на сервере для текущего договора
                            string queryCountServices = "select * from dbo.УслугиПоДоговору " +
                                                        "where id_договор in (select top 1 id_договор from dbo.Договор where НомерДоговора = '" + номерДоговора.Trim() + "' and ФлагПроверки = 'True' )";//  and ФлагНаличияАкта = 'True')";

                            //таблица содержит услуги которые у нас на сервере 
                            DataTable tab = ТаблицаБД.GetTableSQL(queryCountServices, "КоличествоУслугиПоДоговору", con, transact);

                            //если количество услуг на сервере и в файле не равны 0
                            //if (tab.Rows.Count != 0 && un.УслугиПоДоговору.Rows.Count != 0)
                            //{

                            //если колличество услуг и на сервере и в файле выгрузки одинаковое
                            if (tab.Rows.Count == un.УслугиПоДоговору.Rows.Count)
                            {
                                List<ErrorsReestrUnload> listR = new List<ErrorsReestrUnload>();

                                //харинт список улуг записанных на сервере
                                List<ErrorsReestrUnload> listServcS = new List<ErrorsReestrUnload>();


                                //Заполним спсок данным из таблицы с услугами записанными на сервере
                                foreach (DataRow rowS in tab.Rows)
                                {
                                    //создадим экземпляр для хранения данных с сервера
                                    ErrorsReestrUnload itemS = new ErrorsReestrUnload();

                                    itemS.НаименованиеУслуги = rowS["НаименованиеУслуги"].ToString().Trim();
                                    itemS.Цена = Math.Round(Convert.ToDecimal(rowS["Цена"]), 2);
                                    itemS.Сумма = Math.Round(Convert.ToDecimal(rowS["Сумма"]), 2);
                                    itemS.FlagWrite = false;
                                    itemS.Количество = Convert.ToInt32(rowS["Количество"]);
                                    listServcS.Add(itemS);
                                }

                                //харинт список улуг записанных в файле
                                List<ErrorsReestrUnload> listServcF = new List<ErrorsReestrUnload>();

                                //Заполним спсок данным из таблицы с услугами записанными на сервере
                                foreach (DataRow rowF in un.УслугиПоДоговору.Rows)
                                {
                                    //создадим экземпляр для хранения данных с сервера
                                    ErrorsReestrUnload itemF = new ErrorsReestrUnload();

                                    itemF.ErrorНаименованиеУслуги = rowF["НаименованиеУслуги"].ToString().Trim();
                                    itemF.ErrorЦена = Math.Round(Convert.ToDecimal(rowF["Цена"]), 2);
                                    itemF.ErrorСумма = Math.Round(Convert.ToDecimal(rowF["Сумма"]), 2);
                                    itemF.FlagWrite = false;
                                    itemF.ОшибкаКоличество = Convert.ToInt32(rowF["Количество"]);
                                    listServcF.Add(itemF);
                                }

                                List<ErrorsReestrUnload> test1 = listServcS;
                                List<ErrorsReestrUnload> test2 = listServcF;

                                //Пройдёмся по коллекции
                                foreach (ErrorsReestrUnload itemF in listServcF)
                                {
                                    string iTest = itemF.ErrorНаименованиеУслуги;

                                    if (iTest == "Коррекция протеза" && номерДоговора.Trim() == "СпКмрСо/216")
                                    {
                                        string aTest = "Test";
                                    }

                                    foreach (ErrorsReestrUnload itemS in listServcS)
                                    {
                                        //==========тест стереть==========
                                        string iiTest = itemS.НаименованиеУслуги;


                                        if (iiTest == "Коррекция протеза" && номерДоговора.Trim() == "СпКмрСо/216")
                                        {
                                            string aTest = "Test";
                                        }
                                        //=======конец теста==============

                                        if (itemF.ErrorНаименованиеУслуги.Trim() == itemS.НаименованиеУслуги.Trim() && itemF.ErrorЦена == itemS.Цена && itemF.ErrorСумма == itemS.Сумма && itemF.FlagWrite != true && itemS.FlagWrite != true)
                                        {
                                            //если наименование услуги стоимость и сумма равны и на сервере и в файле то ставим флаг в true
                                            itemF.FlagWrite = true;
                                            itemS.FlagWrite = true;
                                        }
                                    }
                                }


                                //Создадим коллекцию в которую поместим экземпляры классов у которых поле FlagWrite == false
                                List<ErrorsReestrUnload> listErrors = new List<ErrorsReestrUnload>();
                                foreach (ErrorsReestrUnload itemF in listServcF)
                                {
                                    if (itemF.FlagWrite == false)
                                    {
                                        listErrors.Add(itemF);
                                    }
                                }



                                //Создадим аналогичный список для сервера
                                List<ErrorsReestrUnload> listServer = new List<ErrorsReestrUnload>();
                                foreach (ErrorsReestrUnload itemS in listServcS)
                                {
                                    if (itemS.FlagWrite == false)
                                    {
                                        listServer.Add(itemS);
                                    }
                                }

                                //теперь сравним обе коллекции
                                if (listErrors.Count == listServer.Count)
                                {
                                    foreach (ErrorsReestrUnload er in listErrors)
                                    {
                                        if (er.FlagWrite == false)
                                        {
                                            ErrorsReestrUnload eru = new ErrorsReestrUnload();
                                            eru.ErrorНаименованиеУслуги = er.НаименованиеУслуги;

                                            eru.ErrorЦена = er.Цена;
                                            eru.ErrorСумма = er.Сумма;
                                            eru.ОшибкаКоличество = er.ОшибкаКоличество;

                                            listR.Add(er);
                                        }
                                    }

                                    int iCountR = 0;
                                    foreach (ErrorsReestrUnload lo in listServer)
                                    {
                                        if (lo.FlagWrite == false)
                                        {
                                            listR[iCountR].НаименованиеУслуги = lo.НаименованиеУслуги;
                                            listR[iCountR].Цена = lo.Цена;
                                            listR[iCountR].Сумма = lo.Сумма;
                                            listR[iCountR].Количество = lo.Количество;
                                        }

                                        iCountR++;
                                    }
                                }

                                //Выставим флаг ошибки
                                if (listR.Count == 0)
                                {
                                    //ошибок нет
                                    dispR.FlagError = true;
                                }
                                else
                                {
                                    //ошибка
                                    //ошибок нет
                                    dispR.FlagError = false;
                                }

                                dispR.СписокОшибок = listR;
                                ////===============================================================
                                // }

                                decimal summAct = 0.0m;

                                //запишем сумму акта выполненных работ проссуммировав услуги которые храняться в файле выгрузки реестра выполненных работ
                                foreach (DataRow row in un.УслугиПоДоговору.Rows)
                                {
                                    summAct = Math.Round(Math.Round(summAct, 2) + Math.Round(Convert.ToDecimal(row["Сумма"]), 2), 2);
                                }

                                dispR.СуммаАктаВыполненныхРабот = summAct;

                                //Реализация Суммы услуг записанных у нас на сервере для текущего договора
                                string queryViewServices = "select top 1 Sum(Сумма) as 'Сумма' from dbo.УслугиПоДоговору " +
                                                           "where id_договор in( " +
                                                           "SELECT top 1 [id_договор] " +
                                                           "FROM [Договор] " +
                                                           "where [НомерДоговора] = '" + номерДоговора.Trim() + "' and  ФлагПроверки = 'True' ) ";

                                DataTable tabSumServer = ТаблицаБД.GetTableSQL(queryViewServices, "УслугипоДоговоруСервер", con, transact);

                                if (tabSumServer.Rows[0]["Сумма"] != DBNull.Value)
                                    dispR.СуммаДоговорСервер = Math.Round(Convert.ToDecimal(tabSumServer.Rows[0]["Сумма"]));


                                #region Старая реализация
                                ///*
                                // * так как услуги которых у нас на сервере нет уже записаны в список расхождения listError
                                // */
                                ////проверим а одинаковые они или нет по названиям (УЗНАЕМ ЧТО ЛЕЖИТ У НАС НА СЕРВРЕ)

                                //////соберём список из наименования услуг и цены которые соответсвуют текущему договору
                                //string queryDictionary = "select id_услугиДоговор,НаименованиеУслуги,Цена,НомерПоПеречню,Сумма from dbo.УслугиПоДоговору " +
                                //                         "where id_договор in( " +
                                //                         "SELECT [id_договор] " +
                                //                         "FROM [Договор] " +
                                //                         "where [НомерДоговора] = '" + номерДоговора.Trim() + "') ";

                                //List<ErrorsReestrUnload> listTest2 = listError;



                                ////Получим таблицу содержащую услуги  для текущего договора хранящиеся у нас на сервере
                                //DataTable tabServ = ТаблицаБД.GetTableSQL(queryDictionary, "ТаблицаУслуги", con, transact);

                                //DataTable tabFile = un.УслугиПоДоговору;

                                //////Заполним библиотеку содержащую услуги хранящиеся у нас на сервере
                                //Dictionary<string, DateService> dicServer = new Dictionary<string, DateService>();

                                //foreach (DataRow row in tabServ.Rows)
                                //{
                                //    DateService ds = new DateService();
                                //    ds.Наименование = row["НаименованиеУслуги"].ToString().Trim();

                                //    ds.Цена = Convert.ToDecimal(row["Цена"]).ToString("c");
                                //    ds.Сумма = Convert.ToDecimal(row["Сумма"]).ToString("c");

                                //    dicServer.Add(row["id_услугиДоговор"].ToString().Trim(), ds);
                                //}

                                //if (номерДоговора.Trim() == "8/324")
                                //{
                                //    string sTest = "test";
                                //}

                                ////удалим из библиотеки услуги которые есть и на сервере и в файле выгрузки
                                //foreach (DataRow rowS in tabServ.Rows)
                                //{
                                //    foreach (DataRow rowF in tabFile.Rows)
                                //    {
                                //        if (rowS["НаименованиеУслуги"].ToString().Trim() == rowF["НаименованиеУслуги"].ToString().Trim())
                                //        {
                                //            string s_id = rowS["id_услугиДоговор"].ToString().Trim();

                                //            dicServer.Remove(s_id);
                                //            //break;

                                //            goto Found;

                                //        }
                                //    }

                                //Found:
                                //    continue;
                                //}

                                //if (номерДоговора.Trim() == "8/324")
                                //{
                                //    string sTest = "test";
                                //}

                                //Dictionary<string, DateService> dTest = dicServer;

                                //int iCount2 = 0;
                                //foreach (DateService ds in dicServer.Values)
                                //{
                                //    listError[iCount2].НаименованиеУслуги = ds.Наименование;


                                //    //string sцена = ds.Цена.Replace(',', '.');
                                //    //string ценар = sцена.Replace('р',' ');
                                //    string ценар = ds.Цена.Replace('р', ' ');
                                //    int ценаi = ценар.Length;
                                //    string ценаz = ценар.Remove(ценаi - 1, 1);
                                //    //string цена = ценаz.Replace(',', '.').Trim();

                                //    decimal ценаd = decimal.Parse("50,00");

                                //    if (ценаd != 0.0m)
                                //    {
                                //        listError[iCount2].Цена = ценаd;// Math.Round(Convert.ToDecimal(цена.Trim()), 2);
                                //    }

                                //    string суммар = ds.Сумма.Replace('р', ' ');
                                //    int суммаi = суммар.Length;
                                //    string суммаz = ценар.Remove(суммаi - 1, 1);
                                //    //string сумма = суммаz.Replace(',', '.');

                                //    decimal суммаd = decimal.Parse(суммаz);

                                //    if (суммаd != 0.0m)
                                //    {
                                //        listError[iCount2].Сумма = суммаd;// Math.Round(Convert.ToDecimal(сумма.Trim()), 2);
                                //    }


                                //    iCount2++;
                                //}
                                #endregion

                            }

                            //работает
                            List<ErrorsReestrUnload> listTest = listError;

                            if (tab.Rows.Count > un.УслугиПоДоговору.Rows.Count)
                            {
                                //===================================
                                List<ErrorsReestrUnload> listR = new List<ErrorsReestrUnload>();

                                //харинт список улуг записанных на сервере
                                List<ErrorsReestrUnload> listServcS = new List<ErrorsReestrUnload>();


                                //Заполним спсок данным из таблицы с услугами записанными на сервере
                                foreach (DataRow rowS in tab.Rows)
                                {
                                    //создадим экземпляр для хранения данных с сервера
                                    ErrorsReestrUnload itemS = new ErrorsReestrUnload();

                                    itemS.НаименованиеУслуги = rowS["НаименованиеУслуги"].ToString().Trim();
                                    itemS.Цена = Math.Round(Convert.ToDecimal(rowS["Цена"]), 2);
                                    itemS.Сумма = Math.Round(Convert.ToDecimal(rowS["Сумма"]), 2);
                                    itemS.FlagWrite = false;
                                    itemS.Количество = Convert.ToInt32(rowS["Количество"]);
                                    listServcS.Add(itemS);
                                }

                                //харинт список улуг записанных в файле
                                List<ErrorsReestrUnload> listServcF = new List<ErrorsReestrUnload>();

                                //Заполним спсок данным из таблицы с услугами записанными на сервере
                                foreach (DataRow rowF in un.УслугиПоДоговору.Rows)
                                {
                                    //создадим экземпляр для хранения данных с сервера
                                    ErrorsReestrUnload itemF = new ErrorsReestrUnload();

                                    itemF.ErrorНаименованиеУслуги = rowF["НаименованиеУслуги"].ToString().Trim();
                                    itemF.ErrorЦена = Math.Round(Convert.ToDecimal(rowF["Цена"]), 2);
                                    itemF.ErrorСумма = Math.Round(Convert.ToDecimal(rowF["Сумма"]), 2);
                                    itemF.FlagWrite = false;
                                    itemF.Количество = Convert.ToInt32(rowF["Количество"]);
                                    listServcF.Add(itemF);
                                }

                                //Пройдёмся по коллекции
                                foreach (ErrorsReestrUnload itemF in listServcF)
                                {
                                    string iTest = itemF.ErrorНаименованиеУслуги;
                                    foreach (ErrorsReestrUnload itemS in listServcS)
                                    {
                                        //==========тест стереть==========
                                        string iiTest = itemS.НаименованиеУслуги;


                                        if (iiTest == "Снятие оттиска силиконового")
                                        {
                                            string aTest = "Test";
                                        }
                                        //=======конец теста==============

                                        if (itemF.ErrorНаименованиеУслуги.Trim() == itemS.НаименованиеУслуги.Trim() && itemF.ErrorЦена == itemS.Цена && itemF.ErrorСумма == itemS.Сумма && itemF.FlagWrite != true && itemS.FlagWrite != true)
                                        {
                                            //если наименование услуги стоимость и сумма равны и на сервере и в файле то ставим флаг в true
                                            itemF.FlagWrite = true;
                                            itemS.FlagWrite = true;
                                        }
                                    }
                                }

                                //Создадим коллекцию в которую поместим экземпляры классов у которых поле FlagWrite == false
                                List<ErrorsReestrUnload> listErrors = new List<ErrorsReestrUnload>();

                                foreach (ErrorsReestrUnload itemF in listServcF)
                                {
                                    if (itemF.FlagWrite == false)
                                    {
                                        listErrors.Add(itemF);
                                    }
                                }


                                //Создадим аналогичный список для сервера
                                List<ErrorsReestrUnload> listServer = new List<ErrorsReestrUnload>();
                                foreach (ErrorsReestrUnload itemS in listServcS)
                                {
                                    if (itemS.FlagWrite == false)
                                    {
                                        listServer.Add(itemS);
                                    }
                                }

                                //теперь сравним обе коллекции
                                if (listErrors.Count >= listServer.Count)
                                {
                                    foreach (ErrorsReestrUnload er in listErrors)
                                    {
                                        if (er.FlagWrite == false)
                                        {
                                            ErrorsReestrUnload eru = new ErrorsReestrUnload();
                                            eru.ErrorНаименованиеУслуги = er.НаименованиеУслуги;

                                            eru.ErrorЦена = er.Цена;
                                            eru.ErrorСумма = er.Сумма;
                                            eru.ОшибкаКоличество = er.ОшибкаКоличество;

                                            listR.Add(er);
                                        }
                                    }

                                    int iCountR = 0;
                                    foreach (ErrorsReestrUnload lo in listServer)
                                    {
                                        if (lo.FlagWrite == false)
                                        {
                                            listR[iCountR].НаименованиеУслуги = lo.НаименованиеУслуги;
                                            listR[iCountR].Цена = lo.Цена;
                                            listR[iCountR].Сумма = lo.Сумма;
                                            listR[iCountR].Количество = lo.Количество;
                                        }

                                        iCountR++;
                                    }
                                }

                                if (listErrors.Count <= listServer.Count)
                                {
                                    foreach (ErrorsReestrUnload er in listServer)
                                    {
                                        if (er.FlagWrite == false)
                                        {
                                            ErrorsReestrUnload eru = new ErrorsReestrUnload();
                                            eru.НаименованиеУслуги = er.НаименованиеУслуги;

                                            eru.Цена = er.Цена;
                                            eru.Сумма = er.Сумма;

                                            listR.Add(er);
                                        }
                                    }

                                    int iCountR = 0;
                                    foreach (ErrorsReestrUnload lo in listErrors)
                                    {
                                        if (lo.FlagWrite == false)
                                        {
                                            listR[iCountR].ErrorНаименованиеУслуги = lo.ErrorНаименованиеУслуги;
                                            listR[iCountR].ErrorЦена = lo.ErrorЦена;
                                            listR[iCountR].ErrorСумма = lo.ErrorСумма;
                                        }

                                        iCountR++;
                                    }
                                }

                                //Выставим флаг ошибки
                                if (listR.Count == 0)
                                {
                                    //ошибок нет
                                    dispR.FlagError = true;
                                }
                                else
                                {
                                    //ошибка
                                    dispR.FlagError = false;
                                }

                                dispR.СписокОшибок = listR;

                                //Запишем сумму акта выполненных работ
                                decimal summAct = 0.0m;

                                //запишем сумму акта выполненных работ проссуммировав услуги которые храняться в файле выгрузки реестра выполненных работ
                                foreach (DataRow row in un.УслугиПоДоговору.Rows)
                                {
                                    summAct = Math.Round(Math.Round(summAct, 2) + Math.Round(Convert.ToDecimal(row["Сумма"]), 2), 2);
                                }

                                dispR.СуммаАктаВыполненныхРабот = summAct;

                                string queryViewServices = "select top 1 Sum(Сумма) as 'Сумма' from dbo.УслугиПоДоговору " +
                                                           "where id_договор in( " +
                                                           "SELECT top 1 [id_договор] " +
                                                           "FROM [Договор] " +
                                                           "where [НомерДоговора] = '" + номерДоговора.Trim() + "' and  ФлагПроверки = 'True') ";

                                DataTable tabSumServer = ТаблицаБД.GetTableSQL(queryViewServices, "УслугипоДоговоруСервер", con, transact);

                                if (tabSumServer.Rows[0]["Сумма"] != DBNull.Value)
                                    dispR.СуммаДоговорСервер = Math.Round(Convert.ToDecimal(tabSumServer.Rows[0]["Сумма"]), 2);

                            }


                            //в файле выгрузки услуг больше чем записано у нас на сервере
                            if (tab.Rows.Count < un.УслугиПоДоговору.Rows.Count)
                            {

                                List<ErrorsReestrUnload> listR = new List<ErrorsReestrUnload>();

                                //харинт список улуг записанных на сервере
                                List<ErrorsReestrUnload> listServcS = new List<ErrorsReestrUnload>();


                                //Заполним спсок данным из таблицы с услугами записанными на сервере
                                foreach (DataRow rowS in tab.Rows)
                                {
                                    //создадим экземпляр для хранения данных с сервера
                                    ErrorsReestrUnload itemS = new ErrorsReestrUnload();

                                    itemS.НаименованиеУслуги = rowS["НаименованиеУслуги"].ToString().Trim();
                                    itemS.Цена = Math.Round(Convert.ToDecimal(rowS["Цена"]), 2);
                                    itemS.Сумма = Math.Round(Convert.ToDecimal(rowS["Сумма"]), 2);
                                    itemS.FlagWrite = false;
                                    itemS.ОшибкаКоличество = Convert.ToInt32(rowS["Количество"]);
                                    listServcS.Add(itemS);
                                }

                                //харинт список улуг записанных в файле
                                List<ErrorsReestrUnload> listServcF = new List<ErrorsReestrUnload>();

                                //Заполним спсок данным из таблицы с услугами записанными на сервере
                                foreach (DataRow rowF in un.УслугиПоДоговору.Rows)
                                {
                                    //создадим экземпляр для хранения данных с сервера
                                    ErrorsReestrUnload itemF = new ErrorsReestrUnload();

                                    itemF.ErrorНаименованиеУслуги = rowF["НаименованиеУслуги"].ToString().Trim();
                                    itemF.ErrorЦена = Math.Round(Convert.ToDecimal(rowF["Цена"]), 2);
                                    itemF.ErrorСумма = Math.Round(Convert.ToDecimal(rowF["Сумма"]), 2);
                                    itemF.FlagWrite = false;
                                    itemF.ОшибкаКоличество = Convert.ToInt32(rowF["Количество"]);
                                    listServcF.Add(itemF);
                                }

                                //Пройдёмся по коллекции
                                foreach (ErrorsReestrUnload itemF in listServcF)
                                {
                                    string iTest = itemF.ErrorНаименованиеУслуги;
                                    foreach (ErrorsReestrUnload itemS in listServcS)
                                    {
                                        //==========тест стереть==========
                                        string iiTest = itemS.НаименованиеУслуги;


                                        //if (iiTest == "Снятие оттиска силиконового")
                                        //{
                                        //    string aTest = "Test";
                                        //}
                                        //=======конец теста==============

                                        if (itemF.ErrorНаименованиеУслуги.Trim() == itemS.НаименованиеУслуги.Trim() && itemF.ErrorЦена == itemS.Цена && itemF.ErrorСумма == itemS.Сумма && itemF.FlagWrite != true && itemS.FlagWrite != true)
                                        {
                                            //если наименование услуги стоимость и сумма равны и на сервере и в файле то ставим флаг в true
                                            itemF.FlagWrite = true;
                                            itemS.FlagWrite = true;
                                        }
                                    }
                                }

                                //Создадим коллекцию в которую поместим экземпляры классов у которых поле FlagWrite == false
                                List<ErrorsReestrUnload> listErrors = new List<ErrorsReestrUnload>();
                                foreach (ErrorsReestrUnload itemF in listServcF)
                                {
                                    if (itemF.FlagWrite == false)
                                    {
                                        listErrors.Add(itemF);
                                    }
                                }


                                //Создадим аналогичный список для сервера
                                List<ErrorsReestrUnload> listServer = new List<ErrorsReestrUnload>();
                                foreach (ErrorsReestrUnload itemS in listServcS)
                                {
                                    if (itemS.FlagWrite == false)
                                    {
                                        listServer.Add(itemS);
                                    }
                                }

                                //теперь сравним обе коллекции
                                if (listErrors.Count >= listServer.Count)
                                {
                                    foreach (ErrorsReestrUnload er in listErrors)
                                    {
                                        if (er.FlagWrite == false)
                                        {
                                            ErrorsReestrUnload eru = new ErrorsReestrUnload();
                                            eru.ErrorНаименованиеУслуги = er.НаименованиеУслуги;

                                            eru.ErrorЦена = er.Цена;
                                            eru.ErrorСумма = er.Сумма;
                                            eru.ОшибкаКоличество = er.ОшибкаКоличество;

                                            listR.Add(er);
                                        }
                                    }

                                    int iCountR = 0;
                                    foreach (ErrorsReestrUnload lo in listServer)
                                    {
                                        if (lo.FlagWrite == false)
                                        {
                                            listR[iCountR].НаименованиеУслуги = lo.НаименованиеУслуги;
                                            listR[iCountR].Цена = lo.Цена;
                                            listR[iCountR].Сумма = lo.Сумма;
                                            listR[iCountR].Количество = lo.Количество;
                                        }

                                        iCountR++;
                                    }
                                }

                                if (listErrors.Count <= listServer.Count)
                                {
                                    foreach (ErrorsReestrUnload er in listServer)
                                    {
                                        if (er.FlagWrite == false)
                                        {
                                            ErrorsReestrUnload eru = new ErrorsReestrUnload();
                                            eru.НаименованиеУслуги = er.НаименованиеУслуги;

                                            eru.Цена = er.Цена;
                                            eru.Сумма = er.Сумма;

                                            listR.Add(er);
                                        }
                                    }

                                    int iCountR = 0;
                                    foreach (ErrorsReestrUnload lo in listErrors)
                                    {
                                        if (lo.FlagWrite == false)
                                        {
                                            listR[iCountR].ErrorНаименованиеУслуги = lo.ErrorНаименованиеУслуги;
                                            listR[iCountR].ErrorЦена = lo.ErrorЦена;
                                            listR[iCountR].ErrorСумма = lo.ErrorСумма;
                                        }

                                        iCountR++;
                                    }
                                }

                                //Выставим флаг ошибки
                                if (listR.Count == 0)
                                {
                                    //ошибок нет
                                    dispR.FlagError = true;
                                }
                                else
                                {
                                    //ошибка
                                    dispR.FlagError = false;
                                }

                                dispR.СписокОшибок = listR;
                                ////===============================================================
                            }

                            //}
                            bool flagДопСоглашение = false;

                            //Узнаем есть ли у текущего договора доп соглашение
                            DataTable tabДопСогл = un.ДопСоглашение;
                            if (tabДопСогл.Rows.Count != 0)
                            {
                                //если есть то ставим flag в true
                                flagДопСоглашение = true;
                                dispR.FlagAddContract = flagДопСоглашение;
                            }


                            // Проверим оплачивался ли текущий договор.
                            string queryValidDoc = "select НомерАкта from АктВыполненныхРабот " +
                                                   "where id_договор = ( " +
                                                    "select id_договор from Договор " +
                                                    "where LTRIM(RTRIM(LOWER(НомерДоговора))) = '" + un.Договор.Rows[0]["НомерДоговора"].ToString().Trim().ToLower() + "' and ФлагПроверки = 'True') ";

                            DataTable tabValidAct = ТаблицаБД.GetTableSQL(queryValidDoc, "ValidAct");

                            if (tabValidAct.Rows.Count > 0)
                            {
                                // errorReestr.АктВыполненныхРабот = tabValidAct.Rows[0]["НомерАкта"].ToString().Trim();

                                // Акт содержит ошибку, уже был оплачен.
                                dispR.FlagErrorОплаченныйДоговор = true;

                                // Укажем номер ранее оплаченного акта.
                                dispR.НомерОплаченногоАкта = tabValidAct.Rows[0]["НомерАкта"].ToString().Trim();
                            }

                            // Если у нас ошибка по лльготной категории, тогда высвечиваем ошибку.
                            if (dispR.ErrorPrefCategory == true)
                            {
                                dispR.FlagError = false;
                            }

                            var test = dispR;

                            listReest.Add(номерАкта, dispR);

                            //Запишем сумму акта выполненных работ
                            decimal summAct1 = 0.0m;

                            //запишем сумму акта выполненных работ проссуммировав услуги которые храняться в файле выгрузки реестра выполненных работ
                            foreach (DataRow row in un.УслугиПоДоговору.Rows)
                            {
                                summAct1 = Math.Round(Math.Round(summAct1, 2) + Math.Round(Convert.ToDecimal(row["Сумма"]), 2), 2);
                            }

                            dispR.СуммаАктаВыполненныхРабот = summAct1;

                            string queryViewServices1 = "select top 1 Sum(Сумма) as 'Сумма' from dbo.УслугиПоДоговору " +
                                                           "where id_договор in( " +
                                                           "SELECT top 1 [id_договор] " +
                                                           "FROM [Договор] " +
                                                           "where [НомерДоговора] = '" + номерДоговора.Trim() + "' and  ФлагПроверки = 'True') ";

                            DataTable tabSumServer1 = ТаблицаБД.GetTableSQL(queryViewServices1, "УслугипоДоговоруСервер", con, transact);

                            if (tabSumServer1.Rows[0]["Сумма"] != DBNull.Value)
                                dispR.СуммаДоговорСервер = Math.Round(Convert.ToDecimal(tabSumServer1.Rows[0]["Сумма"]), 2);

                            //}
                            //else
                            //{
                            //    MessageBox.Show("Проект договора № " + номерДоговора.Trim() + " на сервере не существует");
                            //}
                        }


                        #endregion
                    }

                    //Dictionary<string, ValidateContract> validReestr 
                    Dictionary<string, DisplayReestr> iTestDisp = listReest;

                    FormDisplayReestr fd = new FormDisplayReestr();
                    fd.Unloads = unload;
                    fd.АктыРабот = listReest;
                    fd.Show();

                    fstream.Close();

                }

            }
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FirstLoadHospital flHosp = new FirstLoadHospital();
            flHosp.ShowDialog();

            if (flHosp.DialogResult == DialogResult.OK)
            {

            }

            if (flHosp.DialogResult == DialogResult.Cancel)
            {
                flHosp.Close();
            }
        }

        private void наименованиеПостановленияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormПостановление form = new FormПостановление();
            form.MdiParent = this;
            form.Show();
        }

        private void открытьРеестрПроектToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //Получим из файла словарь с договорами
            //Dictionary<string, Unload> unload;
            //Установим для нашей программы текущую директорию для корректного считывания пути к БД
            Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;


            //переменная конфигурации
            int iConfig;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открыть файл реестра";
            //openFileDialog1.Filter = "|*.r";
            openFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string fileName = openFileDialog1.FileName;

                try
                {

                    //SerializableObject objToSerialize = null;
                    FileStream fstream = File.Open(fileName, FileMode.Open);
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    //Получим из файла словарь с договорами
                    unload = (Dictionary<string, Unload>)binaryFormatter.Deserialize(fstream);

                     //Закроем файловый поток
                    fstream.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Вероятно Вы открыли реестр который не является реестром проектов договоров.");

                    return;
                }

                //Установим для нашей программы текущую директорию для корректного считывания пути к БД
                Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;

                //Закроем диалоговое окно
                openFileDialog1.Dispose();



            }
            else
            {
                //если пользователь нажал кнопку отмена то выходим из поиска
                return;
            }


            //Создадим список объектов для хранения результатов проверки проектов договоров
            Dictionary<string, ValidateContract> listValContr = new Dictionary<string, ValidateContract>();

            //Получим конфигурацию поиска льготника в базе данных ЭСРН
            using (FileStream fs = File.OpenRead("Config.dll"))
            using (TextReader read = new StreamReader(fs))
            {
                iConfig = Convert.ToInt32(read.ReadLine().Trim());
                fs.Close();
                read.Close();
            }


            //Установим для нашей программы текущую директорию для корректного считывания пути к БД
            Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;

            // Это для кривых договоров.
            //Dictionary<string, Unload> unload2 = unload.Where(w => w.Key != "СпКмрСо/1554").ToDictionary(w => w.Key,w=>w.Value);


            //проверим льготников по базе данных в ЭСРН
            ValidateЭСРН эсрн = new ValidateЭСРН(unload, listValContr, iConfig);

            // Укажем подключать ся ли к серверам или нет.
            эсрн.FlagConnectServer = this.FlagConnectServer;

            if (flagЗагрузкаБД == true)
            {
                эсрн.FlagЗагрузки = true;
            }

            // Выполним сверку проектов договоров с базами данных ЭСРН.
            Dictionary<string, ValidateContract> validReestr = эсрн.Validate();

            //Выведим результат проверки на экран
            FormValidOut formOutVal = new FormValidOut();

            //Передадим в форму выгруженные договора
            formOutVal.ПроектыДоговоров = validReestr;

            //передадим в результаты проверки договоров
            formOutVal.ВыгрузкаПроектДоговоров = unload;

            //Выведим форму на первое место
            //formOutVal.TopMost = true;


            formOutVal.Show();

        }

        private void конфигурироватьПоискToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigSearch formCS = new FormConfigSearch();
            formCS.MdiParent = this;
            formCS.Show();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormОжидание f = new FormОжидание();
            f.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Проверм существует ли файл установки
            FileStream fs = File.Open("Config.dll", FileMode.OpenOrCreate);
            if (fs.Length != 0)
            {
                //Считываем из файла настройку на выгрузку
                ;
            }
            else
            {
                //Создаём файл и записываем настройку запретим выгрузку
                using (TextWriter writ = new StreamWriter(fs))
                {
                    writ.WriteLine("4");
                }
            }
            fs.Close();

        }

        private void договораПрошедшиеПроверкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormValidContract fvc = new FormValidContract();
            fvc.MdiParent = this;
            fvc.WindowState = FormWindowState.Maximized;

            fvc.FlagValid = true;
            fvc.Show();
        }

        private void возвратПроектовДоговоровToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormValidContract fvc = new FormValidContract();
            fvc.MdiParent = this;

            fvc.WindowState = FormWindowState.Maximized;

            fvc.FlagValid = false;
            fvc.Show();
        }

        private void загрузитьВсюБазуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.flagЗагрузкаБД = true;

            // Напишем функционал для записи реестров в БД после проверки прокуратурой, т.е. запишем в БД реестры которые составлялись в первую половину 2013 г.


            //Установим для нашей программы текущую директорию для корректного считывания пути к БД
            Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открыть файл реестра";
            openFileDialog1.Filter = "|*.r";
            openFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string fileName = openFileDialog1.FileName;

                //SerializableObject objToSerialize = null;
                FileStream fstream = File.Open(fileName, FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();


                //получим выгрузку реестра выполненных договоров
                List<Unload> unloads = (List<Unload>)binaryFormatter.Deserialize(fstream);

                IEnumerable<DataTable> iTable = unloads.Select(a => a.Договор);

                foreach (DataTable numDog in iTable)
                {
                    foreach (DataRow row in numDog.Rows)
                    {
                        if (row[1].ToString() == "5/1087")
                        {
                            string iTest = "test";
                        }
                    }
                }

                IEnumerable<Unload> unloadS = unloads.Where(un => un.АктВыполненныхРабот != null).Select(un => un);

                List<Unload> unload = new List<Unload>();
                foreach (Unload und in unloadS)
                {
                    unload.Add(und);
                }

                List<Unload> iUnload = unload;

                // Выведим окно в которое запишем 
                FormReestrDate frdForm = new FormReestrDate();
                frdForm.ShowDialog();

                if (frdForm.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    string номерРеестра = frdForm.НомерРеестра;
                    DateTime датаРеестра = frdForm.ДатаРеестра;

                    string номерСчётФактуры = frdForm.НомерСчётФактуры;
                    DateTime датаСчётФактуры = frdForm.ДатаСчётФактуры;

                    ClassLibrary1.DataClasses1DataContext dc = new ClassLibrary1.DataClasses1DataContext();
                    using (System.Transactions.TransactionScope myScope = new System.Transactions.TransactionScope())
                    {
                        try
                        {

                            foreach (Unload un in iUnload)
                            {
                                // Запишем в БД данные по льготнику.
                                DataTable dtАктВыполненныыхРабот = un.АктВыполненныхРабот;

                                DataTable dtPerson = un.Льготник;



                                int id_льготник = 0;
                                int id_договор = 0;

                                int id_ЛК = 0;

                                // Льготник.
                                foreach (DataRow itPerson in dtPerson.Rows)
                                {
                                    ClassLibrary1.Льготник person = new ClassLibrary1.Льготник();

                                    // Запишем ЛЬГОТНИКА.
                                    person.Фамилия = itPerson[1].ToString().Trim();
                                    person.Имя = itPerson[2].ToString().Trim();
                                    person.Отчество = itPerson[3].ToString().Trim();
                                    person.ДатаРождения = Convert.ToDateTime(itPerson[4]);
                                    person.улица = itPerson[5].ToString().Trim();
                                    person.НомерДома = itPerson[6].ToString().Trim();
                                    person.корпус = itPerson[7].ToString().Trim();
                                    person.НомерКвартиры = itPerson[8].ToString().Trim();
                                    person.СерияПаспорта = itPerson[9].ToString().Trim();
                                    person.НомерПаспорта = itPerson[10].ToString().Trim();
                                    person.ДатаВыдачиПаспорта = Convert.ToDateTime(itPerson[11]);
                                    person.КемВыданПаспорт = itPerson[12].ToString().Trim();

                                    // id_льготной категории.
                                    id_ЛК = dc.ЛьготнаяКатегория.Where(l => l.ЛьготнаяКатегория1 == un.ЛьготнаяКатегория.Trim()).Select(l => l.id_льготнойКатегории).First();
                                    person.id_льготнойКатегории = id_ЛК;

                                    // id документ.
                                    int id_doc = dc.ТипДокумента.Where(doc => doc.НаименованиеТипаДокумента == un.ТипДокумента.Rows[0][1].ToString().Trim()).Select(doc => doc.id_документ).First();
                                    person.id_документ = id_doc;

                                    // Серия документа.
                                    person.СерияДокумента = itPerson[15].ToString().Trim();
                                    person.НомерДокумента = itPerson[16].ToString().Trim();

                                    // Датат выдачи документа.
                                    person.ДатаВыдачиДокумента = Convert.ToDateTime(itPerson[17]);
                                    // Кем выдан документ.
                                    person.КемВыданДокумент = itPerson[18].ToString().Trim();

                                    // Запишем id саратовской области у нас это 1.
                                    person.id_область = 1;

                                    // Запишем id района.
                                    person.id_район = -1;
                                    person.id_насПункт = -1;

                                    // Запишем в БД.
                                    dc.Льготник.InsertOnSubmit(person);
                                    dc.SubmitChanges();

                                    // Узнаем id льготника.
                                    id_льготник = person.id_льготник;

                                }


                                // Договор.
                                foreach (DataRow rCont in un.Договор.Rows)
                                {
                                    // Запишем ДОГОВОР.
                                    ClassLibrary1.Договор contr = new ClassLibrary1.Договор();
                                    contr.НомерДоговора = rCont[1].ToString().Trim();
                                    contr.ДатаДоговора = Convert.ToDateTime(rCont[2]);

                                    // Узнаем дату подписания акта выполненных работ.
                                    DateTime dateAct = Convert.ToDateTime(un.АктВыполненныхРабот.Rows[0][4]);

                                    contr.ДатаАктаВыполненныхРабот = dateAct;

                                    // Узнаем сумму акта выполненных работ.
                                    DataTable tabSum = un.УслугиПоДоговору;

                                    decimal sum = 0.0m;
                                    foreach (DataRow rSum in tabSum.Rows)
                                    {
                                        sum = Math.Round(sum + Convert.ToDecimal(rSum[6]), 4);
                                    }


                                    contr.СуммаАктаВыполненныхРабот = sum;

                                    // узнаем id льготной категории.
                                    contr.id_льготнаяКатегория = id_ЛК;

                                    // id поликлинники (id 1 поликлинника = 92).
                                    contr.id_поликлинника = 92;

                                    contr.id_комитет = 1;
                                    contr.ФлагНаличияДоговора = true;
                                    contr.ФлагНаличияАкта = true;

                                    contr.id_льготник = id_льготник;
                                    contr.ФлагДопСоглашения = rCont[12].ToString().Trim();

                                    contr.Примечание = "Доливка данных 2013 г.";

                                    contr.ДатаЗаписиДоговора = DateTime.Now.Date;

                                    contr.ФлагПроверки = true;
                                    contr.НомерРеестра = номерРеестра;
                                    contr.ДатаРеестра = датаРеестра;
                                    contr.НомерСчётФактрура = номерСчётФактуры;
                                    contr.ДатаСчётФактура = датаСчётФактуры;

                                    contr.logWrite = "Заливка базы 2013";

                                    // Запишем в БД и узнаем id_договора.
                                    dc.Договор.InsertOnSubmit(contr);
                                    dc.SubmitChanges();

                                    // Узнаем id договор.
                                    id_договор = contr.id_договор;


                                }

                                // Запишем акт выполненных работ.
                                foreach (DataRow rAct in un.АктВыполненныхРабот.Rows)
                                {
                                    ClassLibrary1.АктВыполненныхРабот act = new ClassLibrary1.АктВыполненныхРабот();
                                    act.НомерАкта = rAct[1].ToString().Trim();
                                    act.id_договор = id_договор;
                                    act.ФлагПодписания = "True";
                                    act.ДатаПодписания = Convert.ToDateTime(rAct[4]);
                                    act.НомерПоПеречню = "True";
                                    act.НаименованиеУслуги = "";
                                    act.Цена = 0.0m;
                                    act.Количество = 0;
                                    act.Сумма = 0.0m;
                                    act.ФлагДопСоглашение = "";
                                    act.НомерРеестра = номерРеестра;
                                    act.ДатаРеестра = датаРеестра;
                                    act.НомерСчётФактуры = номерСчётФактуры;
                                    act.ДатаСчётФактуры = датаСчётФактуры;
                                    act.ДатаОплаты = "";
                                    act.logWrite = "запись 2013 год доливка";
                                    act.logDate = DateTime.Now.Date.ToShortDateString();

                                    dc.АктВыполненныхРабот.InsertOnSubmit(act);
                                    dc.SubmitChanges();




                                }

                                // Запишем услуги по договору.
                                foreach (DataRow rUsl in un.УслугиПоДоговору.Rows)
                                {
                                    ClassLibrary1.УслугиПоДоговору usl = new ClassLibrary1.УслугиПоДоговору();
                                    usl.НаименованиеУслуги = rUsl[1].ToString().Trim();
                                    usl.цена = Convert.ToDecimal(rUsl[2]);
                                    usl.Количество = Convert.ToInt32(rUsl[3]);
                                    usl.id_договор = id_договор;
                                    usl.НомерПоПеречню = rUsl[5].ToString().Trim();
                                    usl.Сумма = Convert.ToDecimal(rUsl[6]);
                                    usl.ТехЛист = Convert.ToInt32(rUsl[7]);

                                    // Запишем в БД.
                                    dc.УслугиПоДоговору.InsertOnSubmit(usl);
                                    dc.SubmitChanges();
                                }

                            }
                        }
                        catch
                        {
                            MessageBox.Show("При записи возникла ошибка.");
                        }

                        myScope.Complete();

                        MessageBox.Show("Данные успешно записаны");
                    }

                }

            }
        }

        private void проверкаДоговоровПоНомерамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Установим для нашей программы текущую директорию для корректного считывания пути к БД
            Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;


            //переменная конфигурации
            int iConfig;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открыть файл реестра";
            openFileDialog1.Filter = "|*.r";
            openFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string fileName = openFileDialog1.FileName;

                //SerializableObject objToSerialize = null;
                FileStream fstream = File.Open(fileName, FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                //Получим из файла словарь с договорами
                unload = (Dictionary<string, Unload>)binaryFormatter.Deserialize(fstream);

                //Закроем файловый поток
                fstream.Close();

                //для теста


                //Установим для нашей программы текущую директорию для корректного считывания пути к БД
                Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;


                //Закроем диалоговое окно
                openFileDialog1.Dispose();

            }
            else
            {
                //если пользователь нажал кнопку отмена то выходим из поиска
                return;
            }


            using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
            {
                con.Open();
                SqlTransaction transact = con.BeginTransaction();

                //Список для хранения номеров договоров
                Dictionary<string, string> list = new Dictionary<string, string>();

                //Заполним библиотеку данными
                foreach (Unload ul in unload.Values)
                {
                    DataTable tabДог = ul.Договор;
                    DataRow row = tabДог.Rows[0];

                    list.Add(row["НомерДоговора"].ToString().Trim(), row["НомерДоговора"].ToString().Trim());
                }

                Dictionary<string, string> iList2 = list;

                //Сравним данные с данными на сервере
                foreach (Unload ul in unload.Values)
                {
                    DataTable tabДог = ul.Договор;
                    DataRow row = tabДог.Rows[0];

                    string query = "select НомерДоговора from Договор where НомерДоговора = '" + row["НомерДоговора"].ToString().Trim() + "' ";
                    DataTable tabServ = ТаблицаБД.GetTableSQL(query, "Договор", con, transact);

                    //если данные равны то удаляем данный договор из библиотеки
                    if (row["НомерДоговора"].ToString().Trim() == tabServ.Rows[0]["НомерДоговора"].ToString().Trim())
                    {
                        list.Remove(row["НомерДоговора"].ToString().Trim());
                    }
                }

                Dictionary<string, string> iList = list;

                List<string> listNum = new List<string>();
                foreach (string num in list.Keys)
                {
                    listNum.Add(num);
                }
                //Выведим список с номероми договоров которые не записались в базу
                FormControlContract fcc = new FormControlContract();
                fcc.List = listNum;
                fcc.MdiParent = this;
                fcc.Show();


            }
        }

        private static bool EndsWithSaurus(string sName, DateService ds)
        {
            bool flag = false;
            List<DateService> list = new List<DateService>();
            DateService dss = new DateService();
            dss.Наименование = ds.Наименование.Trim();

            dss.Цена = ds.Сумма;//.ToString("c");
            dss.Сумма = ds.Цена;//.ToString("c");

            list.Add(dss);

            if (dss.Наименование == sName)
            {
                flag = true;
            }

            return flag;
        }

        private void договораПоКоторымНаправленЗапросToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormValidContract fvc = new FormValidContract();
            fvc.MdiParent = this;
            fvc.WindowState = FormWindowState.Maximized;

            fvc.FlagValid = false;

            //договора не прошедшие проверку
            fvc.FlagCheck = true;
            fvc.Show();
        }

        private void начальникАналитическогоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHeadDepartment fhd = new FormHeadDepartment();
            fhd.MdiParent = this;
            fhd.Show();
        }

        private void выбратьНачалToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSelectHeadDepartment fSelect = new FormSelectHeadDepartment();
            fSelect.MdiParent = this;
            fSelect.Show();
        }

        private void конвертерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;


            //переменная конфигурации
            int iConfig;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открыть файл реестра";
            openFileDialog1.Filter = "|*.r";
            openFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string fileName = openFileDialog1.FileName;

                //SerializableObject objToSerialize = null;
                FileStream fstream = File.Open(fileName, FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                //Получим из файла словарь с договорами
                unload = (Dictionary<string, Unload>)binaryFormatter.Deserialize(fstream);
            }
            else
                return;

            foreach (Unload un in unload.Values)
            {
                //Получим название льготной категории которой принадлежит текущий льготник
                string iЛьготнаяКатегория = un.ЛьготнаяКатегория;

                //получим номер договора
                string numContract = un.Договор.Rows[0]["НомерДоговора"].ToString().Trim();

                using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
                {
                    con.Open();


                    //конвертируем правильные id льготной категории
                    string queryUpdateConvert = "declare @id_Lk int " +
                                                "select @id_Lk = id_льготнойКатегории from dbo.ЛьготнаяКатегория " +
                                                "where ЛьготнаяКатегория = '" + iЛьготнаяКатегория + "' " +
                                                "declare @id_льготник int " +
                                                "select @id_льготник = id_льготник from dbo.Льготник " +
                                                "where id_льготник in ( " +
                                                "select id_льготник from dbo.Договор " +
                                                "where НомерДоговора = '" + numContract + "') " +
                                                "update dbo.Договор " +
                                                "set id_льготнаяКатегория = @id_Lk " +
                                                "where id_договор in ( " +
                                                "select id_договор from dbo.Договор " +
                                                "where НомерДоговора = '" + numContract + "') " +
                                                "update dbo.Льготник " +
                                                "set id_льготнойКатегории = @id_Lk " +
                                                "where id_льготник = @id_льготник ";

                    //ExecuteQuery.Execute(queryUpdateConvert);
                }




            }
        }

        private void поискПоРееструToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReestrDisplay fdr = new FormReestrDisplay();
            fdr.MdiParent = this;
            fdr.WindowState = FormWindowState.Maximized;
            fdr.Show();
        }

        private void наименованиеРайоновToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDepartment fd = new FormDepartment();
            fd.MdiParent = this;
            fd.Show();
        }

        private void кОнфигурироватьРайоныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigРайон fc = new FormConfigРайон();
            fc.MdiParent = this;
            fc.Show();
        }

        private void конверторЛьготнойКтегорииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;


            //переменная конфигурации
            int iConfig;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открыть файл реестра";
            openFileDialog1.Filter = "|*.r";
            openFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string fileName = openFileDialog1.FileName;

                //SerializableObject objToSerialize = null;
                FileStream fstream = File.Open(fileName, FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                //Получим из файла словарь с договорами
                unload = (Dictionary<string, Unload>)binaryFormatter.Deserialize(fstream);
            }

            //string queryЛьготник = "select id_льготник,Фамилия,Имя,Отчество,ДатаРождения,id_льготнойКатегории from Льготник " +
            //                        "where id_льготник in ( " +
            //                        "SELECT  id_льготник FROM Договор where id_льготнаяКатегория is null ) ";


            //DataTable tabЛьготник = ТаблицаБД.GetTableSQL(queryЛьготник, "Льготник");

            foreach (Unload un in unload.Values)
            {
                //Получим название льготной категории которой принадлежит текущий льготник
                string iЛьготнаяКатегория = un.ЛьготнаяКатегория;

                //получим номер договора
                DataRow rDog = un.Договор.Rows[0];
                string номерДоговора = rDog["НомерДоговора"].ToString().Trim();

                //Получим строку с льготником
                DataRow rowLgot = un.Льготник.Rows[0];
                string фамилия = rowLgot["Фамилия"].ToString().Trim();
                string имя = rowLgot["Имя"].ToString().Trim();

                string отчество = rowLgot["Отчество"].ToString().Trim();
                string датаРождения = Время.Дата(Convert.ToDateTime(rowLgot["ДатаРождения"]).ToShortDateString().Trim());

                string _датаРождения = Время.Дата(датаРождения);


                string queryЛьготник = "select id_льготник,Фамилия,Имя,Отчество,ДатаРождения,id_льготнойКатегории from Льготник " +
                                        "where id_льготник in ( " +
                                        "SELECT  id_льготник FROM Договор where id_льготнаяКатегория is null and " +
                                        "Фамилия = '" + фамилия + "' and Имя = '" + имя + "' and Отчество = '" + отчество + "' and ДатаРождения = '" + датаРождения + "') ";
                try
                {
                    DataTable tabЛьгот = ТаблицаБД.GetTableSQL(queryЛьготник, "Льготник");
                    if (tabЛьгот.Rows.Count != 0)
                    {
                        DataTable tabЛьгот2 = ТаблицаБД.GetTableSQL(queryЛьготник, "Льготник");

                        string update = "update Льготник " +
                                   "set id_льготнойКатегории = (select id_льготнойКатегории from ЛьготнаяКатегория " +
                                   "where ЛьготнаяКатегория = '" + iЛьготнаяКатегория + "') " +
                                   "where id_льготник = " + Convert.ToInt32(tabЛьгот.Rows[0]["id_льготник"]) + " " +
                                   "update Договор " +
                                   "set id_льготнаяКатегория = (select id_льготнойКатегории from ЛьготнаяКатегория " +
                                   "where ЛьготнаяКатегория = '" + iЛьготнаяКатегория + "') " +
                                   "where id_льготник = " + Convert.ToInt32(tabЛьгот.Rows[0]["id_льготник"]) + " ";

                        ExecuteQuery.Execute(update);
                    }
                }
                catch
                {
                    MessageBox.Show("Не корректная запись в файле выгрузки");
                }


            }

            MessageBox.Show("Конвертирование закончено");


            //foreach (Unload un in unload.Values)
            //{
            //    //Получим название льготной категории которой принадлежит текущий льготник
            //    string iЛьготнаяКатегория = un.ЛьготнаяКатегория;

            //    //получим номер договора
            //    DataRow rDog = un.Договор.Rows[0];
            //    string номерДоговора = rDog["НомерДоговора"].ToString().Trim();

            //    //Получим строку с льготником
            //    DataRow rowLgot = un.Льготник.Rows[0];
            //    string фамилия = rowLgot["Фамилия"].ToString().Trim();
            //    string имя = rowLgot["Имя"].ToString().Trim();

            //    string отчество = rowLgot["Отчество"].ToString().Trim();
            //    string датаРождения = Время.Дата(Convert.ToDateTime(rowLgot["ДатаРождения"]).ToShortDateString().Trim());

            //    //серия, номер пасорта и дата выдачи
            //    //string серияПаспорта = string.Empty;

            //    //Хранит серию паспорта в формате 00 00 
            //    string serPass = string.Empty;

            //    //string серПаспорт = rowLgot["СерияПаспорта"].ToString().Trim();
            //    string серияПаспорта = rowLgot["СерияПаспорта"].ToString().Trim();

            //    //Установим формат серии паспорта в формате 00 00
            //    StringBuilder build = new StringBuilder();

            //    //счётчик циклов
            //    int iiCount = 0;
            //    foreach (char ch in серияПаспорта)
            //    {
            //        if (iiCount == 2)
            //        {
            //            string sNum = " " + ch.ToString().Trim();
            //            build.Append(sNum);
            //        }
            //        else
            //        {
            //            build.Append(ch);
            //        }


            //        iiCount++;
            //    }

            //    //сохраним в переменную серию и номер паспорта в формате 00 00
            //    serPass = build.ToString().Trim();

            //    string номерПаспорта = rowLgot["НомерПаспорта"].ToString().Trim();
            //    string датаВыдачиПаспорта = Время.Дата(Convert.ToDateTime(rowLgot["ДатаВыдачиПаспорта"]).ToShortDateString().Trim());

            //    //серия, номер и дата выдачи документа дающего право на льготу
            //    string серияДокумента = rowLgot["СерияДокумента"].ToString().Trim();
            //    string номерДокумента = rowLgot["НомерДокумента"].ToString().Trim();
            //    string датаВыдачиДокумента = Время.Дата(Convert.ToDateTime(rowLgot["ДатаВыдачиДокумента"]).ToShortDateString().Trim());

            //    string queryUpdate = "declare @id_льготнойКатегории int " +
            //                         "select @id_льготнойКатегории = id_льготнойКатегории from ЛьготнаяКатегория " +
            //                         "where ЛьготнаяКатегория = '" + iЛьготнаяКатегория + "' " +
            //                         "declare @id_льготник int " +
            //                         "select @id_льготник = id_льготник from Льготник " +
            //                         "where Фамилия = '" + фамилия + "' and Имя = '" + имя + "' and Отчество = '" + отчество + "' and ДатаРождения = '" + датаРождения + "' and СерияПаспорта = '" + серияПаспорта + "' and НомерПаспорта = '" + номерПаспорта + "'  " +
            //                         "and ДатаВыдачиПаспорта = '" + датаВыдачиПаспорта + "' and СерияДокумента = '" + серияДокумента + "' and НомерДокумента = '" + номерДокумента + "' and ДатаВыдачиДокумента = '" + датаВыдачиДокумента + "' " +
            //                         "update dbo.Договор " +
            //                         "set id_льготнаяКатегория = @id_льготнойКатегории " +
            //                         "where id_договор in( " +
            //                         "select id_договор from Договор " +
            //                         "where id_льготник in ( " +
            //                         "select id_льготник from dbo.Льготник " +
            //                         "where id_льготник  = @id_льготник)) " +
            //                         "update dbo.Льготник " +
            //                         "set id_льготнойКатегории = @id_льготнойКатегории " +
            //                         "where id_льготник  = @id_льготник ";

            //    ExecuteQuery.Execute(queryUpdate);
            //}


        }

        private void поискРеестраПоДатеОплатыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDate fDate = new FormDate();
            fDate.ShowDialog();

            if (fDate.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                FormReestrDisplay fdr = new FormReestrDisplay();
                fdr.MdiParent = this;
                fdr.Date = fDate.Date;
                fdr.FlagLetter = true;
                fdr.WindowState = FormWindowState.Maximized;
                fdr.Show();
            }
        }

        private void запросПоДублямToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Сфоримруем запрос по дублирующим договороам по льготникам. Т.Е. найдём льготников которые имеют более одного договора
            string queryBi = "SELECT     Фамилия, Имя, Отчество,ДатаРождения, НомерДоговора, id_льготник " +
                             "FROM         (SELECT     dbo.Льготник.Фамилия, dbo.Льготник.Имя, dbo.Льготник.Отчество,dbo.Льготник.ДатаРождения, dbo.Договор.НомерДоговора, dbo.Договор.id_льготник " +
                             "FROM          dbo.Договор INNER JOIN " +
                             "dbo.Льготник ON dbo.Договор.id_льготник = dbo.Льготник.id_льготник) AS derivedtbl_1 " +
                             "where id_льготник in (SELECT      id_льготник " +
                             "FROM         (SELECT     НомерДоговора, COUNT(НомерДоговора) AS Expr1, id_льготник, COUNT(id_льготник) AS Expr2 " +
                             "FROM          dbo.Договор " +
                             "WHERE      (id_льготник IN " +
                             "(SELECT     id_льготник " +
                             "FROM          dbo.Договор AS Договор_1 " +
                             "GROUP BY id_льготник " +
                             "HAVING      (COUNT(НомерДоговора) > 1))) " +
                             "GROUP BY НомерДоговора, id_льготник) AS derivedtbl_1 " +
                             "where id_льготник in ( " +
                             "SELECT      id_льготник  " +
                             "FROM         (SELECT     НомерДоговора, COUNT(НомерДоговора) AS Expr1, id_льготник, COUNT(id_льготник) AS Expr2 " +
                             "FROM          dbo.Договор " +
                             "WHERE      (id_льготник IN " +
                             "(SELECT     id_льготник " +
                             "FROM          dbo.Договор AS Договор_1 " +
                             "GROUP BY id_льготник " +
                             "HAVING      (COUNT(НомерДоговора) > 1))) " +
                             "GROUP BY НомерДоговора, id_льготник) AS derivedtbl_1 " +
                             "group by id_льготник " +
                             "having count(id_льготник) > 1)) " +
                             "group by Фамилия, Имя, Отчество,ДатаРождения, НомерДоговора, id_льготник " +
                             "order by Фамилия asc ";

            DataTable tableDubli = ТаблицаБД.GetTableSQL(queryBi, "ДоблиДоговоров");

            //Создадим коллекцию которая содержит данные для отчёта
            List<ЭлементПисьма> list = new List<ЭлементПисьма>();

            //Запишем шапку отчёта
            ЭлементПисьма шапка = new ЭлементПисьма();
            шапка.Номер = "№ п.п.";

            шапка.ФИО = "ФИО льготника";
            шапка.ДатаРождения = "дата рождения льготника";

            шапка.НомерДоговора = "Номер договора";
            list.Add(шапка);

            int iCount = 1;

            //Заполним отчёт данными
            foreach (DataRow row in tableDubli.Rows)
            {
                string f = row["Фамилия"].ToString().Trim();
                string i = row["Имя"].ToString().Trim();
                string o = row["Отчество"].ToString().Trim();

                ЭлементПисьма item = new ЭлементПисьма();
                item.Номер = iCount.ToString().Trim();

                item.ФИО = f + " " + i + " " + o;
                item.ДатаРождения = Convert.ToDateTime(row["ДатаРождения"]).ToShortDateString();

                item.НомерДоговора = row["НомерДоговора"].ToString().Trim();
                list.Add(item);

                iCount++;
            }


            string filName = System.Windows.Forms.Application.StartupPath + @"\Шаблон\Список дублированных договоров.doc";


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

            //Горизонтальное ориентирование листа
            //doc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;


            object bookNaziv = "таблица";
            Range wrdRng = doc.Bookmarks.get_Item(ref bookNaziv).Range;

            object behavior = Microsoft.Office.Interop.Word.WdDefaultTableBehavior.wdWord8TableBehavior;
            object autobehavior = Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitWindow;

            Microsoft.Office.Interop.Word.Table table = doc.Tables.Add(wrdRng, 1, 4, ref behavior, ref autobehavior);
            table.Range.ParagraphFormat.SpaceAfter = 6;
            table.Columns[1].Width = 40;
            table.Columns[2].Width = 150;
            table.Columns[3].Width = 150;
            table.Columns[3].Width = 150;
            table.Borders.Enable = 1; // Рамка - сплошная линия
            table.Range.Font.Name = "Times New Roman";
            table.Range.Font.Size = 10;
            //счётчик строк
            //int i = 1;

            //Заполним таблицу
            int k = 1;
            //запишем данные в таблицу
            foreach (ЭлементПисьма item in list)
            {

                table.Cell(k, 1).Range.Text = item.Номер;
                table.Cell(k, 2).Range.Text = item.ФИО;

                table.Cell(k, 3).Range.Text = item.ДатаРождения;
                table.Cell(k, 4).Range.Text = item.НомерДоговора;


                //doc.Words.Count.ToString();
                Object beforeRow1 = Type.Missing;
                table.Rows.Add(ref beforeRow1);

                k++;
            }

            table.Rows[k].Delete();

            //Отобразим документ
            app.Visible = true;
        }

        private void конвертерСуммаАктToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Изменим в поле Сумма акта выполненных работ таблицы Договор Сумму
            string queryTab = "select id_акт from АктВыполненныхРабот";
            DataTable tabАкт = ТаблицаБД.GetTableSQL(queryTab, "АктВыполненныхРабот");

            //Счётчик
            int iCount = 0;

            StringBuilder builder = new StringBuilder();

            foreach (DataRow row in tabАкт.Rows)
            {

                string queryUpdate = " declare @summ_" + iCount + " money " +
                       "select @summ_" + iCount + " = SUM(Сумма) from УслугиПоДоговору " +
                       "where id_договор = ( " +
                       "SELECT id_договор FROM АктВыполненныхРабот " +
                       "where id_акт = " + Convert.ToInt32(row["id_акт"]) + " ) " +
                       "update Договор " +
                       "set СуммаАктаВыполненныхРабот = @summ_" + iCount + " " +
                       "where id_договор = (SELECT id_договор FROM АктВыполненныхРабот " +
                       "where id_акт = " + Convert.ToInt32(row["id_акт"]) + " ) ";

                builder.Append(queryUpdate);

                iCount++;
            }

            string iTest = builder.ToString().Trim();

            ExecuteQuery.Execute(iTest);
        }

        private void конвертерЛьготнаяКатегорияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Пройдёмся по записям в таблице Льготник
            //string queryЛьготник = "select id_льготник,Фамилия,Имя,Отчество,ДатаРождения,id_льготнойКатегории from Льготник";
            string queryЛьготник = "select id_льготник,Фамилия,Имя,Отчество,ДатаРождения,СерияПаспорта,НомерПаспорта,id_льготнойКатегории from Льготник " +
                                    "where id_льготник in ( " +
                                    "SELECT  id_льготник FROM Договор where id_льготнаяКатегория is null ) ";


            DataTable tabЛьготник = ТаблицаБД.GetTableSQL(queryЛьготник, "Льготник");
            foreach (DataRow rowL in tabЛьготник.Rows)
            {
                //переменная хранит льготную категорию
                string льготКат = string.Empty;

                //Пройдёмся по пулу строк подключения
                PullConnectBD pull = new PullConnectBD();
                Dictionary<string, string> pullConnect = pull.GetPull(this.FlagConnectServer);

                foreach (KeyValuePair<string, string> dStringConnect in pullConnect)
                {

                    string sConnection = string.Empty;
                    sConnection = dStringConnect.Value.ToString().Trim();

                    //Выполним проверку в единой транзакции для конкретного района (строки подключения)
                    using (SqlConnection con = new SqlConnection(sConnection))
                    {
                        //string queryL = "select A_NAME from PPR_CAT " +
                        //                "where A_ID in ( " +
                        //                "select A_CATEGORY from SPR_NPD_MSP_CAT " +
                        //                "where A_ID in ( " +
                        //                "select A_SERV from ESRN_SERV_SERV  " +
                        //                "where A_PERSONOUID = ( " +
                        //                "select OUID from WM_PERSONAL_CARD  " +
                        //                "where SURNAME = (select OUID from SPR_FIO_SURNAME  " +
                        //                "where A_NAME = '"+ rowL["Фамилия"].ToString().Trim() +"') and A_NAME = (select OUID from SPR_FIO_NAME " +
                        //                "where A_NAME = '"+ rowL["Имя"].ToString().Trim() +"') and A_SECONDNAME = (select OUID from SPR_FIO_SECONDNAME " +
                        //                //"where A_NAME = '" + rowL["Отчество"].ToString().Trim() + "') and CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 104) = '" + Convert.ToDateTime(rowL["ДатаРождения"]).ToShortDateString().Trim() + "' )))and A_NAME in ('Ветеран труда','Реабилитированные лица','Труженик тыла','Ветеран  военной службы','Ветеран труда Саратовской области')  ";
                        //                "where A_NAME = '" + rowL["Отчество"].ToString().Trim() + "') )))and A_NAME in ('Ветеран труда','Реабилитированные лица','Труженик тыла','Ветеран  военной службы','Ветеран труда Саратовской области')  ";

                        ////Установим формат серии паспорта в формате 00 00
                        //StringBuilder build = new StringBuilder();

                        ////счётчик циклов
                        //int iiCount = 0;
                        //foreach (char ch in rowL["СерияПаспорта"].ToString().Trim())
                        //{
                        //    if (iiCount == 2)
                        //    {
                        //        string sNum = " " + ch.ToString().Trim();
                        //        build.Append(sNum);
                        //    }
                        //    else
                        //    {
                        //        build.Append(ch);
                        //    }


                        //    iiCount++;
                        //}

                        ////сохраним в переменную серию и номер паспорта в формате 00 00
                        //string serPass = build.ToString().Trim();
                        //

                        string serPass = rowL["СерияПаспорта"].ToString().Trim();
                        serPass = serPass + " ";

                        string queryL = "select PPR_CAT.A_NAME from PPR_CAT " +
                                        "where PPR_CAT.A_ID  " +
                                        "in ( select A_CATEGORY from SPR_NPD_MSP_CAT where A_ID " +
                                        "in ( select A_SERV from ESRN_SERV_SERV  " +
                                         "where A_PERSONOUID = (select OUID from WM_PERSONAL_CARD  " +
                                         "where SURNAME = (select OUID from SPR_FIO_SURNAME  where A_NAME = '" + rowL["Фамилия"].ToString().Trim() + "') " +
                                         "and A_NAME = (select OUID from SPR_FIO_NAME where A_NAME = '" + rowL["Имя"].ToString().Trim() + "') " +
                                         "and A_SECONDNAME = (select OUID from SPR_FIO_SECONDNAME where A_NAME = '" + rowL["Отчество"].ToString().Trim() + "') " +
                                         "and OUID = (select PERSONOUID from WM_ACTDOCUMENTS " +
                                         "where DOCUMENTSERIES = '" + serPass + "' and DOCUMENTSNUMBER = '" + rowL["НомерПаспорта"].ToString().Trim() + "') " +
                                         "))and A_NAME in ('Ветеран труда','Реабилитированные лица','Труженик тыла','Ветеран  военной службы','Ветеран труда Саратовской области'))  ";

                        try
                        {
                            DataTable tabLK = ТаблицаБД.GetTableSQL(queryL, "PPR_CAT", con);
                            if (tabLK.Rows.Count != 0)
                            {
                                льготКат = tabLK.Rows[0]["A_NAME"].ToString();

                                string update = "update Льготник " +
                                                "set id_льготнойКатегории = (select id_льготнойКатегории from ЛьготнаяКатегория " +
                                                "where ЛьготнаяКатегория = '" + льготКат + "') " +
                                                "where id_льготник = " + Convert.ToInt32(rowL["id_льготник"]) + " " +
                                                "update Договор " +
                                                "set id_льготнаяКатегория = (select id_льготнойКатегории from ЛьготнаяКатегория " +
                                                "where ЛьготнаяКатегория = '" + льготКат + "') " +
                                                "where id_льготник = " + Convert.ToInt32(rowL["id_льготник"]) + " ";

                                ExecuteQuery.Execute(update);

                                break;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Сервер " + dStringConnect.Key + "не доступен");
                        }

                    }
                }


                //string update = "update Льготник " +
                //                "set id_льготнойКатегории = (select id_льготнойКатегории from ЛьготнаяКатегория " +
                //                "where ЛьготнаяКатегория = '" + льготКат + "') " +
                //                "where id_льготник = "+ Convert.ToInt32(rowL["id_льготник"]) +" " +
                //                "update Договор " +
                //                "set id_льготнаяКатегория = (select id_льготнойКатегории from ЛьготнаяКатегория " +
                //                "where ЛьготнаяКатегория = '" + льготКат + "') " +
                //                "where id_льготник = " + Convert.ToInt32(rowL["id_льготник"]) + " ";

                //ExecuteQuery.Execute(update);
            }

            MessageBox.Show("Процесс завершился");

        }

        private void проверкаЛьготнойКатегорииПоПаспортуToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void отчётДляМинистерстваСоцРазвитияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Установим русскую культуру
            CultureInfo newCInfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            newCInfo.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = newCInfo;


            //Получим количество поликлинник
            string queryCountHosp = "select id_поликлинника,НаименованиеПоликлинники,ИНН,COUNT(ИНН) from Поликлинника " +
                                    "where id_поликлинника in (select id_поликлинника from Договор) " +
                                    "group by ИНН,id_поликлинника,НаименованиеПоликлинники " +
                                    "order by id_поликлинника asc";



            DataTable tabNameH = ТаблицаБД.GetTableSQL(queryCountHosp, "НазваниеПоликлинник");

            //// Коллекция поликлинник.
            //List<HospitalCount> listHosp = new List<HospitalCount>();

            //// Преобразуем в коллекцию таблицу tabNameH.
            // foreach (DataRow row in tabNameH.Rows)
            // {
            //     HospitalCount hosp = new HospitalCount();
            //     hosp.IdПоликлинники = Convert.ToInt32(row["id_поликлинника"]);
            //     hosp.НаименованиеПоликлинники = row["НаименованиеПоликлинники"].ToString().Trim();
            //     hosp.ИНН = row["ИНН"].ToString().Trim();
            //     //hosp.CountИНН = Convert.ToInt32(row["COUNT(ИНН)"]);

            //     listHosp.Add(hosp);
            // }

            // IEnumerable<IGrouping<string,HospitalCount>> query = listHosp.GroupBy(l => l.ИНН);//.Select(l => l);

            // List<string> lll = new List<string>();

            // int iiCount = 0;
            // foreach (IGrouping<string,HospitalCount> HospitalCount in query)
            // {
            //     foreach (HospitalCount kk in HospitalCount)
            //     {
            //         string sTest = kk.ИНН;
            //         lll.Add(sTest);
            //     }
            // }

            // List<string> asd = lll;

            // int iTest = iiCount;

            //Список для хранения данных для отчёта
            List<ReportMSO> list = new List<ReportMSO>();

            //Сформируем шапку таблицы
            ReportMSO шапка = new ReportMSO();
            шапка.Num = "№ п/п";
            шапка.NameHospital = "Наименование учреждения здравоохранения";
            шапка.CountContractHospital = "Количество заключённых договоров с учреждениями здравоохранения (нарастающим итогом)";

            шапка.SumContractHospital = "Сумма, на которую заключены договора (нарастающим итогом тыс. руб)";
            шапка.SumPaidContract = "Сумма выплаченная по договорам (нарастающим итогом) (тыс. руб.)";
            шапка.CountRoseRecords = "Численность вставших на учёт (нарастающим итогом)";

            list.Add(шапка);

            //счётчик порядкового номера
            int iCount = 1;

            //сч1тчик количество контрактов
            int iCountContract = 0;

            //счётчик ксумма итого
            decimal iCountSum = 0m;

            //пройдёмся по поликлинникам
            foreach (DataRow row in tabNameH.Rows)// -- Старая реализация.
            //foreach (HospitalCount item in listHosp)
            {


                //так как в таблице Поликлинника базы данных Dentist все поликлинники которые относятся к г. Саратов
                //находятся с верху (первые 8 записей) то мы ставим условие 
                if (iCount != 8)
                {
                    ReportMSO reportItem = new ReportMSO();


                    //Запишем порядковый номер
                    reportItem.Num = iCount.ToString();

                    string nameHosp = row["НаименованиеПоликлинники"].ToString().Trim();
                    //string nameHosp = item.НаименованиеПоликлинники


                    //Запишем наименование учреждения
                    NameHosp hosp = new NameHosp(nameHosp);
                    reportItem.NameHospital = hosp.GetNameHosp();

                    //Подсчитаем количество заключонных договоров
                    //string queryCountContract = "select Max(id_договор),id_льготник from View_Договора " +
                    //                            "where id_поликлинника in (" + Convert.ToInt32(row["id_поликлинника"]) + ") " +
                    //                            "group by id_льготник ";

                    string queryCountContract = "select id_договор,id_льготник from View_Договора " +
                                                "where id_поликлинника in (" + Convert.ToInt32(row["id_поликлинника"]) + ") ";
                    //"group by id_льготник ";

                    //подсчитаем количество заключонных договоров для текущей поликлиннике
                    DataTable tabCount = ТаблицаБД.GetTableSQL(queryCountContract, "CountContr");
                    int counContr = tabCount.Rows.Count + hosp.GetCount();

                    //reportItem.CountContractHospital = tabCount.Rows.Count.ToString();
                    reportItem.CountContractHospital = counContr.ToString();

                    //Подсчитаем итого количество заключонных договоров
                    iCountContract = iCountContract + tabCount.Rows.Count + hosp.GetCount();

                    //Подсчитаем сумму на которую заключены договора
                    //string queryCount = "select SUM(Сумма) as 'Сумма' from dbo.УслугиПоДоговору " +
                    //                    "where id_договор in (select Max(id_договор) from View_Договора where id_поликлинника in (" + Convert.ToInt32(row["id_поликлинника"]) + ")  " +
                    //                    "group by id_льготник) ";

                    string queryCount = "select SUM(Сумма) as 'Сумма' from dbo.УслугиПоДоговору " +
                                        "where id_договор in (select id_договор from View_Договора where id_поликлинника in (" + Convert.ToInt32(row["id_поликлинника"]) + ") ) ";
                    //"group by id_льготник) ";

                    DataTable tabSum = ТаблицаБД.GetTableSQL(queryCount, "count");

                    if (tabSum.Rows[0]["Сумма"] != DBNull.Value)
                    {
                        //узнаем сумму для записей в БД после 29.08.2013 г
                        decimal sum = Math.Round(Convert.ToDecimal(tabSum.Rows[0]["Сумма"]), 2);
                        //reportItem.SumContractHospital = (sum / 1000).ToString("c");

                        //Прибавим к сумме договоров заключонных 30.08.2013 г и далее а так же сумму договоров заключонных до 29.08.2013 г. включительно 29.08.2013 г
                        decimal sumContr = Math.Round(Math.Round(sum, 2) + Math.Round(hosp.GetSum(), 2), 2);
                        reportItem.SumContractHospital = Math.Round((sumContr / 1000), 2).ToString();

                        //iCountSum = Math.Round((Math.Round(iCountSum, 2) + Math.Round((sum), 2)), 2);
                        iCountSum = Math.Round((Math.Round(iCountSum, 2) + Math.Round((sumContr), 2)), 2);
                    }
                    else
                    {
                        //если записей после 29.08.2013 г. нет то выводим суммы указанные до 29.08.2013 г. включительно
                        decimal sumContr = Math.Round(hosp.GetSum(), 2);
                        reportItem.SumContractHospital = (sumContr / 1000).ToString();

                        iCountSum = Math.Round((Math.Round(iCountSum, 2) + Math.Round((sumContr), 2)), 2);
                    }

                    // Запишем ИНН.
                    reportItem.ИНН = row["ИНН"].ToString().Trim();

                    list.Add(reportItem);
                }
                else
                {
                    ReportMSO reportItemSar = new ReportMSO();
                    reportItemSar.NameHospital = "Итого Саратов";
                    reportItemSar.CountContractHospital = (iCountContract).ToString();
                    reportItemSar.SumContractHospital = Math.Round(iCountSum / 1000, 2).ToString();
                    list.Add(reportItemSar);

                    // Test
                    //if (row["НаименованиеПоликлинники"].ToString().Trim() == "Татищевская ЦРБ")
                    //{
                    //    string iTest = "Test";
                    //}


                    ReportMSO reportItem = new ReportMSO();

                    //Запишем порядковый номер
                    reportItem.Num = iCount.ToString();

                    string nameHosp = row["НаименованиеПоликлинники"].ToString().Trim();


                    //Запишем наименование учреждения
                    NameHosp hosp = new NameHosp(nameHosp);
                    reportItem.NameHospital = hosp.GetNameHosp();

                    //Подсчитаем количество заключонных договоров
                    //string queryCountContract = "select Max(id_договор),id_льготник from View_Договора " +
                    //                            "where id_поликлинника in (" + Convert.ToInt32(row["id_поликлинника"]) + ") " +
                    //                            "group by id_льготник ";

                    string queryCountContract = "select Count(id_договор),id_льготник from View_Договора " +
                                               "where id_поликлинника in (" + Convert.ToInt32(row["id_поликлинника"]) + ") " +
                                               "group by id_льготник ";

                    //Подсчитаем количество заключонных договоров для текущей поликлинники
                    DataTable tabCount = ТаблицаБД.GetTableSQL(queryCountContract, "CountContr");
                    int counContr = tabCount.Rows.Count + hosp.GetCount();

                    //Выведим количество заключонных договоров
                    reportItem.CountContractHospital = counContr.ToString();

                    //Подсчитаем итого количество заключонных договоров
                    //iCountContract = iCountContract + tabCount.Rows.Count;

                    //Подсчитаем итого количество заключонных договоров
                    iCountContract = iCountContract + tabCount.Rows.Count + hosp.GetCount();

                    //Подсчитаем сумму на которую заключены договора
                    string queryCount = "select SUM(Сумма) as 'Сумма' from dbo.УслугиПоДоговору " +
                                        "where id_договор in (select Max(id_договор) from View_Договора where id_поликлинника in (" + Convert.ToInt32(row["id_поликлинника"]) + ")  " +
                                        "group by id_льготник) ";

                    DataTable tabSum = ТаблицаБД.GetTableSQL(queryCount, "count");

                    if (tabSum.Rows[0]["Сумма"] != DBNull.Value)
                    {
                        decimal sum = Math.Round(Convert.ToDecimal(tabSum.Rows[0]["Сумма"]), 2);
                        //reportItem.SumContractHospital = (sum / 1000).ToString("c");

                        decimal sumContr = Math.Round(Math.Round(sum, 2) + Math.Round(hosp.GetSum(), 2), 2);
                        reportItem.SumContractHospital = (sumContr / 1000).ToString();

                        iCountSum = Math.Round((Math.Round(iCountSum, 2) + Math.Round((sumContr), 2)), 2);
                    }
                    else
                    {
                        decimal sumContr = Math.Round(hosp.GetSum(), 2);
                        reportItem.SumContractHospital = (sumContr / 1000).ToString();

                        iCountSum = Math.Round((Math.Round(iCountSum, 2) + Math.Round((sumContr), 2)), 2);
                    }

                    // Запишем ИНН.
                    reportItem.ИНН = row["ИНН"].ToString().Trim();

                    list.Add(reportItem);
                }

                iCount++;

                //list.Add(reportItem);
            }


            //Подсчитаем Итого
            ReportMSO itenItog = new ReportMSO();
            itenItog.NameHospital = "Итого";
            itenItog.CountContractHospital = iCountContract.ToString();
            itenItog.SumContractHospital = Math.Round(iCountSum / 1000, 2).ToString();

            list.Add(itenItog);

            //// Запишем в новую коллекцию классы с суммами.
            List<ReportMSO> newList = new List<ReportMSO>();

            foreach (ReportMSO rmso in list)
            {
                int iCountRow = 0;
                if (rmso.ИНН != null)
                {
                    // Найдём среди записанных в новую коллекцию элемент с текущим номером ИНН.
                    iCountRow = newList.Where(n => n.ИНН == rmso.ИНН).Select(n => n).Count();
                }
                if (iCountRow == 1)
                {
                    // Найдём конкретную поликлиннику.
                    var query = newList.Where(n => n.ИНН == rmso.ИНН).Select(n => n);
                    foreach (ReportMSO myRmso in query)
                    {
                        // Сложим количество заключонных договоров.
                        int newCountContractHospital = Convert.ToInt32(myRmso.CountContractHospital);
                        int CountContractHospital = Convert.ToInt32(rmso.CountContractHospital);

                        int sumCountContractHospital = newCountContractHospital + CountContractHospital;
                        myRmso.CountContractHospital = sumCountContractHospital.ToString().Trim();


                        myRmso.CountRoseRecords = rmso.CountRoseRecords;

                        myRmso.NameHospital = rmso.NameHospital;
                        myRmso.Num = rmso.Num;

                        // Узнаем сумму на которую заключили договора.
                        decimal newSumContractHospital = Convert.ToDecimal(myRmso.SumContractHospital);
                        decimal ContractHospital = Convert.ToDecimal(rmso.SumContractHospital);

                        decimal sumContractHospital = Math.Round(Math.Round(newSumContractHospital, 2) + Math.Round(ContractHospital, 2), 2);
                        myRmso.SumContractHospital = sumContractHospital.ToString().Trim();

                        myRmso.SumPaidContract = rmso.SumPaidContract;
                    }
                }
                //if (rmso.Flag == false)
                else
                {
                    ReportMSO newItem = new ReportMSO();
                    newItem.CountContractHospital = rmso.CountContractHospital;
                    newItem.CountRoseRecords = rmso.CountRoseRecords;

                    newItem.NameHospital = rmso.NameHospital;
                    newItem.Num = rmso.Num;

                    newItem.SumContractHospital = rmso.SumContractHospital;
                    newItem.SumPaidContract = rmso.SumPaidContract;

                    newItem.ИНН = rmso.ИНН;

                    // Установим флаг в true, что укажет что данный объект уже использовался.
                    rmso.Flag = true;

                    newList.Add(newItem);
                }

            }


            //Выведим данные на лист 
            string filName = System.Windows.Forms.Application.StartupPath + @"\Шаблон\Мин-во По заключ договорам и численности.doc";


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

            //Горизонтальное ориентирование листа
            //doc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;


            object bookNaziv = "таблица";
            Range wrdRng = doc.Bookmarks.get_Item(ref bookNaziv).Range;

            object behavior = Microsoft.Office.Interop.Word.WdDefaultTableBehavior.wdWord8TableBehavior;
            object autobehavior = Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitWindow;

            Microsoft.Office.Interop.Word.Table table = doc.Tables.Add(wrdRng, 1, 6, ref behavior, ref autobehavior);
            table.Range.ParagraphFormat.SpaceAfter = 6;
            table.Columns[1].Width = 40;
            table.Columns[2].Width = 120;
            table.Columns[3].Width = 80;
            table.Columns[4].Width = 80;
            table.Columns[5].Width = 80;
            table.Columns[6].Width = 80;
            table.Borders.Enable = 1; // Рамка - сплошная линия
            table.Range.Font.Name = "Times New Roman";
            table.Range.Font.Size = 10;
            //счётчик строк
            //int i = 1;

            string date = DateTime.Now.ToShortDateString();

            object wdrepl = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt = "date";
            object newtxt = (object)date;
            //object frwd = true;
            object frwd = false;
            doc.Content.Find.Execute(ref searchtxt, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd, ref missing, ref missing, ref newtxt, ref wdrepl, ref missing, ref missing,
            ref missing, ref missing);

            //Заполним таблицу
            int k = 1;
            //запишем данные в таблицу
            //foreach (ReportMSO item in list)
            foreach (ReportMSO item in newList)
            {
                //table.Cell(i, 1).Range.Text = i.ToString();//item.НомерПорядковый;
                table.Cell(k, 1).Range.Text = item.Num;

                table.Cell(k, 2).Range.Text = item.NameHospital;
                table.Cell(k, 3).Range.Text = item.CountContractHospital;
                table.Cell(k, 4).Range.Text = item.SumContractHospital;
                table.Cell(k, 5).Range.Text = item.SumPaidContract;
                table.Cell(k, 6).Range.Text = item.CountRoseRecords;


                //doc.Words.Count.ToString();
                Object beforeRow1 = Type.Missing;
                table.Rows.Add(ref beforeRow1);

                k++;
            }

            table.Rows[k].Delete();


            //Подпишем договор Должность инициалы и фамилия
            DataTable tabНачальник;

            //получим текущего начальника отдела автоматизации
            string queryНачальник = "select должность,фамилия,инициалы from ДолжностьАналитичУправ " +
                                    "where id_должность = (select id_должность from dbo.ПодписьДолжность) ";

            using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
            {
                con.Open();
                SqlTransaction transact = con.BeginTransaction();

                tabНачальник = ТаблицаБД.GetTableSQL(queryНачальник, "Должность", con, transact);

            }

            //try
            //{

            //Получим должность начальника отдела
            string dolzhnost = tabНачальник.Rows[0]["должность"].ToString().Trim();

            //получим ФИО начальника отдела
            string familija = tabНачальник.Rows[0]["фамилия"].ToString().Trim();

            //Получим инициалы
            string inicialy = tabНачальник.Rows[0]["инициалы"].ToString().Trim();

            object wdrepl2 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt2 = "familija";
            object newtxt2 = (object)familija;
            //object frwd = true;
            object frwd2 = false;
            doc.Content.Find.Execute(ref searchtxt2, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd2, ref missing, ref missing, ref newtxt2, ref wdrepl2, ref missing, ref missing,
            ref missing, ref missing);

            object wdrepl3 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt3 = "dolzhnost";
            object newtxt3 = (object)dolzhnost;
            //object frwd = true;
            object frwd3 = false;
            doc.Content.Find.Execute(ref searchtxt3, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd3, ref missing, ref missing, ref newtxt3, ref wdrepl3, ref missing, ref missing,
            ref missing, ref missing);

            object wdrepl4 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt4 = "inicialy";
            object newtxt4 = (object)inicialy;
            //object frwd = true;
            object frwd4 = false;
            doc.Content.Find.Execute(ref searchtxt4, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd4, ref missing, ref missing, ref newtxt4, ref wdrepl4, ref missing, ref missing,
            ref missing, ref missing);


            //Отобразим документ
            app.Visible = true;

        }

        private void отчётыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void статистикаЗа2014ГToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Так как поставлена задача выводить отчёт только за 2014 г. с нарастающим итогом, то мы и будем забивать даты жёстко в код.

            // Первое января 2014 г.
            //string beginDate = "01.01.2014";

            //string endDate = DateTime.Now.ToShortDateString();

            // Для теста.
            string beginDate = "01.01.2014";

            string endDate = "31.12.2014";


            StatisticHospital statReport = new StatisticHospital(beginDate, endDate);
            statReport.ReportWord();
        }

        private void конвертироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string scom = "select * from Договор where ФлагНаличияАкта = 'True' and ФлагПроверки = 'True'";
            DataTable tabДоговор = ТаблицаБД.GetTableSQL(scom, "Договор");

            //Узнаем содержатся ли ещё договора у текущего льготника
            using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
            {
                con.Open();

                foreach (DataRow row in tabДоговор.Rows)
                {
                    // Получим по номер договора.
                    string numContr = row["НомерДоговора"].ToString().Trim();

                    // Получим id договора.
                    int idдоговор = Convert.ToInt32(row["id_договор"]);

                    // Получим номер реестра.
                    string номерРеестра = row["НомерРеестра"].ToString().Trim();

                    // Узнаем есть ли такой договор в таблице с актами.

                    string like = numContr + "%";

                    string queryComp = "select * from АктВыполненныхРабот where НомерАкта like '" + like + "' ";
                    DataTable tabCompare = ТаблицаБД.GetTableSQL(queryComp, "АктВыполненныхРаботПроверка");

                    if (tabCompare.Rows.Count != 0)
                    {
                        string queryUpdate = "update АктВыполненныхРабот " +
                                              "set id_договор = " + idдоговор + ", " +
                                              "НомерРеестра = '" + номерРеестра + "' " +
                                              "where НомерАкта like '" + like + "' ";


                        SqlCommand comUpdate = new SqlCommand(queryUpdate, con);
                        comUpdate.ExecuteNonQuery();

                    }

                }

            }
        }

        private void изменитьДатыАктаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            СhangeAkt frmChange = new СhangeAkt();
            frmChange.MdiParent = this;
            frmChange.Show();
        }

        private void открытьФайлРеестрПроектовДоговоровToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Получим из файла словарь с договорами
            //Dictionary<string, Unload> unload;
            //Установим для нашей программы текущую директорию для корректного считывания пути к БД
            Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;

            //Создадим список для отображения результатов проверки
            List<ContractDisplayWord> list = new List<ContractDisplayWord>();

            // Добавим в коллекцию list шапку таблицы.
            ContractDisplayWord шапка = new ContractDisplayWord();
            шапка.НомерПорядковый = "№ п/п";
            шапка.НомерДоговора = "№ договора";
            шапка.ФИО = "ФИО";
            шапка.SumService = "Сумма договора";

            list.Add(шапка);


            //переменная конфигурации
            int iConfig;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открыть файл реестра";
            openFileDialog1.Filter = "|*.r";
            openFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string fileName = openFileDialog1.FileName;

                //SerializableObject objToSerialize = null;
                FileStream fstream = File.Open(fileName, FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                //Получим из файла словарь с договорами
                unload = (Dictionary<string, Unload>)binaryFormatter.Deserialize(fstream);

                // Отобразим список в документе Word.

                //переменная хранит сумму проектов договоров в текущем реестре (списке договоров)
                decimal sumCount = 0.0m;

                // Переменная для хранения порядкового номера в таблице.
                int numPP = 1;

                foreach (string keyVK in unload.Keys)
                {
                    ContractDisplayWord contract = new ContractDisplayWord();
                    contract.НомерДоговора = keyVK.Trim();

                    // Порядковый номер.
                    contract.НомерПорядковый = numPP.ToString();

                    //Получим под текущий ключ реестр договоров
                    Unload iv = unload[keyVK.Trim()];

                    //Узнаем из поля объекта реестр договоров ФИО льготника
                    DataRow rowL = iv.Льготник.Rows[0];

                    // 
                    string фамилия = rowL["Фамилия"].ToString();
                    string имя = rowL["Имя"].ToString();
                    string отчество = rowL["Отчество"].ToString();

                    //Присвоим списку индикации ФИО льготника
                    contract.ФИО = фамилия + " " + имя + " " + отчество;


                    //Получим сумму по текущему договору
                    DataTable tabSum = iv.УслугиПоДоговору;

                    decimal sum = 0.0m;
                    foreach (DataRow r in tabSum.Rows)
                    {
                        sum = sum + Convert.ToDecimal(r["Сумма"]);
                    }

                    //Присвоим 
                    contract.SumService = sum.ToString("c");

                    //Получим сумму по реестру
                    sumCount = sumCount + sum;

                    list.Add(contract);

                    numPP++;
                }

                // Сформируем документ Word.
                //Выведим данные на лист 
                string filName = System.Windows.Forms.Application.StartupPath + @"\Шаблон\Шаблон проектов договоров.doc";

                //Создаём новый Word.Application
                Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();

                //Загружаем документ
                Microsoft.Office.Interop.Word.Document doc = null;

                object fileName2 = filName;
                object falseValue = false;
                object trueValue = true;
                object missing = Type.Missing;

                doc = app.Documents.Open(ref fileName2, ref missing, ref trueValue,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing);

                //Горизонтальное ориентирование листа
                //doc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;


                object bookNaziv = "таблица";
                Range wrdRng = doc.Bookmarks.get_Item(ref bookNaziv).Range;

                object behavior = Microsoft.Office.Interop.Word.WdDefaultTableBehavior.wdWord8TableBehavior;
                object autobehavior = Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitWindow;

                Microsoft.Office.Interop.Word.Table table = doc.Tables.Add(wrdRng, 1, 4, ref behavior, ref autobehavior);
                table.Range.ParagraphFormat.SpaceAfter = 6;
                table.Columns[1].Width = 30;
                table.Columns[2].Width = 80;
                table.Columns[3].Width = 180;
                table.Columns[4].Width = 180;
                //table.Columns[5].Width = 80;
                //table.Columns[6].Width = 80;
                table.Borders.Enable = 1; // Рамка - сплошная линия
                table.Range.Font.Name = "Times New Roman";
                table.Range.Font.Size = 10;

                //string date = DateTime.Now.ToShortDateString();

                //object wdrepl = WdReplace.wdReplaceAll;
                ////object searchtxt = "GreetingLine";
                //object searchtxt = "date";
                //object newtxt = (object)date;
                ////object frwd = true;
                //object frwd = false;
                //doc.Content.Find.Execute(ref searchtxt, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd, ref missing, ref missing, ref newtxt, ref wdrepl, ref missing, ref missing,
                //ref missing, ref missing);

                //Заполним таблицу
                int k = 1;
                //запишем данные в таблицу
                //foreach (ReportMSO item in list)
                foreach (ContractDisplayWord item in list)
                {

                    //table.Cell(i, 1).Range.Text = i.ToString();//item.НомерПорядковый;
                    table.Cell(k, 1).Range.Text = item.НомерПорядковый;

                    table.Cell(k, 2).Range.Text = item.НомерДоговора;
                    table.Cell(k, 3).Range.Text = item.ФИО;
                    table.Cell(k, 4).Range.Text = item.SumService;



                    //doc.Words.Count.ToString();
                    Object beforeRow1 = Type.Missing;
                    table.Rows.Add(ref beforeRow1);

                    k++;
                }

                table.Rows[k].Delete();

                //Отобразим документ
                app.Visible = true;

                //Закроем файловый поток
                fstream.Close();

                //Установим для нашей программы текущую директорию для корректного считывания пути к БД
                Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;

                //Закроем диалоговое окно
                openFileDialog1.Dispose();

            }
            else
            {
                //если пользователь нажал кнопку отмена то выходим из поиска
                return;
            }
        }

        private void записьДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormWriteBD form = new FormWriteBD();
            form.MdiParent = this;
            form.Show();
        }

        private void списокПолучателейМерСоцПоддержкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormСписокПолучателей form = new FormСписокПолучателей();
            form.ShowDialog();

            if (form.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string реестр = form.НомерРеестра.Trim();
                int год = form.Год;
                List<PersonRecipient> persons;
                try
                {
                    persons = GetPersonsПолучатеиСоцПоддержки.GetPersons(реестр, год);
                }
                catch (ExceptionYearNumber ex)
                {
                    ex.ErrorText = "Не указан номер реестра или год";
                    MessageBox.Show(ex.ErrorText, "Ошибка", MessageBoxButtons.OK);

                    return;
                }

                // Выведим полученных льготников на бумагу.
                WordLetter word = new WordLetter(persons);
                word.PrintDoc();

            }
        }

        private void редактированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Провайдер БД.
            string sProvid = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открыть файл реестра";
            openFileDialog1.Filter = "|*.mdb";
            openFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string fileName = openFileDialog1.FileName;

                string connect = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + fileName;

                try
                {
                    DataSetClass ds = new DataSetClass(connect);
                    DataSetHospital dsh = ds.GetDataHospital();

                    FormEditDateHospital form = new FormEditDateHospital(dsh, connect);
                    form.Show();
                }
                catch (ExceptionUser exc)
                {
                    MessageBox.Show(exc.ErrorText, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


            }



            //StreamReader s = File.OpenText(System.Windows.Forms.Application.StartupPath + "\\path.txt");
            //Data.connectionString = s.ReadLine();
            //s.Close();
            //Data.connectionString = Data.connectionString + Application.StartupPath + "\\db.mdb";

            //OleDbConnection con = new OleDbConnection(
        }

        private void статистикаИнвентаризацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormInv form = new FormInv();
            form.Show();
        }

        private void включитьИзменениеСтоимостиДоговораToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfig form = new FormConfig();
            form.Show();
        }

        private void отключитьПроверкуСерверовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FlagConnectServer == false)
            {
                FlagConnectServer = true;

                foreach (ToolStripMenuItem tsmi in this.menuStrip1.Items)
                {
                    if (tsmi.Text == "Настройка")
                    {
                        foreach (ToolStripMenuItem item in tsmi.DropDownItems)
                        {
                            if (item.Text == "Отключить проверку серверов")
                            {
                                item.Text = "Включить проверку серверов";
                            }
                        }
                    }
                }
            }
            else
            {
                FlagConnectServer = false;

                foreach (ToolStripMenuItem tsmi in this.menuStrip1.Items)
                {
                    if (tsmi.Text == "Настройка")
                    {
                        foreach (ToolStripMenuItem item in tsmi.DropDownItems)
                        {
                            if (item.Text == "Включить проверку серверов")
                            {
                                item.Text = "Отключить проверку серверов";
                            }
                        }
                    }
                }
            }
        }

        private void провестиПроверкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillPerson fillPerson = new FillPerson();
            List<Person> listPerson = fillPerson.GetPersonzs();

            var rTestCOunt = listPerson.Count;

            if (listPerson.Count > 0)
            {
                PullConnectBD pull = new PullConnectBD();
                Dictionary<string, string> pullConnect = pull.GetPull(FlagConnectServer);

                foreach (KeyValuePair<string, string> dStringConnect in pullConnect)
                {

                    string sConnection = string.Empty;
                    sConnection = dStringConnect.Value.ToString().Trim();

                    var test = listPerson;

                    EsrnValid esrn = new EsrnValid(sConnection, listPerson);
                    bool flagErrorConnect = esrn.FlagError;

                    if (flagErrorConnect == false)
                    {

                        esrn.Validate();
                    }

                }

                // Получим найденных льготников.
                var listWrite = listPerson.Where(w => w.FlagValid == true);

                var iCount = listWrite.Count();

                WriteSity wr = new WriteSity(listWrite);
                wr.WriteDB();

                MessageBox.Show("Запись районов произведена");
            }
        }

        private void информацияПоБесплатномуЗубопротезированиюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReportPrint formPrint = new FormReportPrint();
            formPrint.Show();
        }

        private void правитьЛьготнуюКатегориюToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // 
            Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;

            //переменная конфигурации
            int iConfig;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открыть файл реестра";
            openFileDialog1.Filter = "|*.r";
            //openFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string fileName = openFileDialog1.FileName;

                FileInfo fi = new FileInfo(fileName);

                // Получим путь к папке.
                string DirectoryPath = fi.DirectoryName;

                try
                {

                    // Откроем текущуюб папку и прочитаем все файлы из неё.
                    DirectoryInfo dir = new DirectoryInfo(DirectoryPath);

                    // ПОлучим файлы из директории.
                    FileInfo[] files = dir.GetFiles();

                    var asd = files.Count();

                    foreach (FileInfo file in files)
                    {
                        var sTest = "";

                        var iLegth = file.Name.Split('+').Length;

                        if (file.Name.Split('+').Length == 2)
                        {

                            var rr = file.Name;

                            // Откроем файловый поток.
                            FileStream fstream = file.Open(FileMode.Open);

                            BinaryFormatter binaryFormatter = new BinaryFormatter();

                            // Прочтем содержмиое файла.
                            unload = (Dictionary<string, Unload>)binaryFormatter.Deserialize(fstream);

                            List<Fire.FireHelp> listFire = new List<Fire.FireHelp>();

                            foreach (var itm in unload)
                            {
                                Fire.FireHelp item = new Fire.FireHelp();

                                item.NumberContract = itm.Value.Договор.Rows[0]["НомерДоговора"].ToString().Trim();
                                item.ЛьготнаяКатегория = itm.Value.ЛьготнаяКатегория.Trim();

                                listFire.Add(item);
                            }

                            Fire.IFireExecute fireLK = new Fire.HelpFireЛьготнаяКатегория();
                            string query = fireLK.Query(listFire);

                            try
                            {
                                ExecuteQuery.Execute(query);
                            }
                            catch (Exception ex)
                            {
                                string queryTest = query;


                                string iTest = "";
                            }

                            //Закроем файловый поток
                            fstream.Close();
                        }


                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Вероятно Вы открыли реестр который не является реестром проектов договоров.");

                    return;
                }

                //Установим для нашей программы текущую директорию для корректного считывания пути к БД
                Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;

                //Закроем диалоговое окно
                openFileDialog1.Dispose();

                MessageBox.Show("Готово");

            }
            else
            {
                //если пользователь нажал кнопку отмена то выходим из поиска
                return;
            }

        }

        private void списокГражданVipNetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FiltrPersonForVipNet fp = new FiltrPersonForVipNet();
            fp.Show();
        }

        /// <summary>
        /// Записать номера контрактов договоров.
        /// </summary>
        /// <param name="unitDate"></param>
        /// <param name="displayContracts"></param>
        /// <param name="idProjectRegistr"></param>
        private string InsertNumbersContracts(List<DisplayContract> displayContracts, int idProjectRegistr)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (DisplayContract itm in displayContracts)
            {
                string query = @"INSERT INTO [dbo].[ListNumbersProgectsReestr]
                               ([IdProjectRegistrFiles]
                               ,[NumberContract]
                               ,[SummaContract]
                               ,[FIO])
                         VALUES
                               (" + idProjectRegistr + " " +
                               ",'" + itm.NumberContract + "' " +
                               "," + itm.SummContract + " " +
                               ",'" + itm.FioPerson + "') ";

                stringBuilder.Append(query);

                //ListNumbersProgectsReestr item = new ListNumbersProgectsReestr();

                //// Id записи файла проектов договоров.
                //item.IdProjectRegistrFiles = idProjectRegistr;

                //// Номер контракта.
                //item.NumberContract = itm.NumberContract;

                //// Сумма контракта.
                //item.SummaContract = itm.SummContract;

                //// ФИО льготника.
                //item.FIO = itm.FioPerson;

                //// Добавить в БД.
                //unitDate.ListNumbersProgectsReestr.Insert(item);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Записать номера контрактов договоров.
        /// </summary>
        /// <param name="unitDate"></param>
        /// <param name="displayContracts"></param>
        /// <param name="idProjectRegistr"></param>
        private void InsertNumbersContracts(UnitDate unitDate, List<DisplayContract> displayContracts, int idProjectRegistr)
        {
            foreach (DisplayContract itm in displayContracts)
            {
                ListNumbersProgectsReestr item = new ListNumbersProgectsReestr();

                // Id записи файла проектов договоров.
                item.IdProjectRegistrFiles = idProjectRegistr;

                // Номер контракта.
                item.NumberContract = itm.NumberContract;

                // Сумма контракта.
                item.SummaContract = itm.SummContract;

                // ФИО льготника.
                item.FIO = itm.FioPerson;

                // Добавить в БД.
                unitDate.ListNumbersProgectsReestr.Insert(item);
            }
        }



        private void приёмToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Текущая директория приложения.
            Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;

            //переменная конфигурации
            int iConfig;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открыть файл реестра";
            //openFileDialog1.Filter = "|*.r";
            // openFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;

            // Путь к папке мой компьютер.
            openFileDialog1.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string fileName = openFileDialog1.FileName;

                //try
                //{
                using (FileStream fstream = File.Open(fileName, FileMode.Open))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    // Получим из файла словарь с договорами.
                    unload = (Dictionary<string, Unload>)binaryFormatter.Deserialize(fstream);
                }

                //fstream.Close();

                // Список проектов договоров отображаемых в письме.
                List<DisplayContract> listContracts = new List<DisplayContract>();

                // Загружаемый реестр проектов договоров.
                Registr registrContracts = new Registr(unload);

                IPrintContract printContract = new PrintContract(registrContracts);
                bool flagWriteDB = printContract.PrintContractDraft();

                if (flagWriteDB == true)
                {
                    // Репозиторий для доступа к БД.
                    UnitDate unitDate = new UnitDate();

                    // Откроем для теста окно сохранения проектов договоров.
                    ReceptionContractsForm receptionContractsForm = new ReceptionContractsForm(unitDate);

                    receptionContractsForm.SaveProjectFIle = true;

                    DialogResult dialogResult = receptionContractsForm.ShowDialog();

                    if (dialogResult == DialogResult.OK) // && receptionContractsForm.FlagWriteRegistrBase == true)
                    {

                        #region Новый функционал

                        //    // Запишем реестр проектов договоров.
                        //    ProjectRegistrFiles projectRegistrFiles = new ProjectRegistrFiles();

                        //    // Массив двоичных данных для хранения реестра проектов договоров.
                        //    byte[] fileData;

                        //    //// Запишем файл в битовый массив.
                        //    //fileData = new byte[fstream.Length];

                        //    //// Файл реестра в двоичном формате.
                        //    //projectRegistrFiles.Registr = fileData;

                        //// Для теста десериализуем.

                        //BinaryFormatter bf = new BinaryFormatter();
                        //using (MemoryStream ms = new MemoryStream())
                        //{
                        //    bf.Serialize(ms, unload);
                        //    fileData = ms.ToArray();
                        //}

                        //MemoryStream stream2 = new MemoryStream(fileData);

                        //BinaryFormatter binaryFormatter2 = new BinaryFormatter();
                        //Dictionary<string, Unload>  unload2 = (Dictionary<string, Unload>)binaryFormatter2.Deserialize(stream2);

                        //stream2.Close();




                        //// Номер письма.
                        //projectRegistrFiles.NumberLatter = receptionContractsForm.NumberLetter;

                        //    // Дата письма.
                        //    projectRegistrFiles.DateLetter = receptionContractsForm.DateLetter;

                        //    // Id поликлинники.
                        //    projectRegistrFiles.IdHospital = receptionContractsForm.IdHospital;

                        //    // Получили реестр который нужно обновить.
                        //    ProjectRegistrFiles prFiles = unitDate.ProjectRegistrFilesRepository.Find(projectRegistrFiles);

                        //    if (prFiles == null)
                        //    {
                        //        string connectionString = "Data Source=10.159.102.68;Initial Catalog=Dentists;User ID=admin;Password=12a86SQL";

                        //        using (SqlConnection con = new SqlConnection(connectionString))
                        //        {
                        //            // 
                        //            con.Open();

                        //            SqlTransaction sqlTransaction = con.BeginTransaction();
                        //            unitDate.ProjectRegistrFilesRepository.InsertBinaryFIle(projectRegistrFiles, fileData, con, sqlTransaction);

                        //            sqlTransaction.Commit();
                        //        }
                        //    }
                        //    else
                        //    {

                        //    }
                        #endregion

                        #region Старый функционал

                        // Установим уровни изоляции транзакций.
                        var option = new System.Transactions.TransactionOptions();
                        option.IsolationLevel = System.Transactions.IsolationLevel.Serializable;

                        // Добавим льготника и адрес в БД.
                        // Внесём данные в таблицу в единой транзакции.
                        using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, option))
                        {

                            Repository.DataClasses1DataContext dc = unitDate.DateContext;

                            var table = dc.ПоликлинникиИнн.Select(w => new ИннHosp { IdHosp = Convert.ToInt32(w.F1), ИНН = (double)w.F3 }).ToList();

                            // Запишем реестр проектов договоров.
                            ProjectRegistrFiles projectRegistrFiles = new ProjectRegistrFiles();

                            // Текущий год.
                            int year = DateTime.Today.Year;

                            // Перменная для хранеия имени файла.
                            StringBuilder nameFile = new StringBuilder();

                            // Сотсавляющая года.
                            nameFile.Append(year.ToString().Trim() + "_");

                            // Сотсавляющая id поликлинники.
                            nameFile.Append("ID_hospital_" + receptionContractsForm.IdHospital.ToString().Trim() + "_");

                            string nameFileRegistr = System.IO.Path.GetFileName(fileName);

                            string pathDirectory = Path.GetDirectoryName(fileName);

                            string newFileRegistrPostCopy = pathDirectory + @"\" + "+_" + nameFileRegistr;

                            // Сотсавляющая года.
                            nameFile.Append(nameFileRegistr.ToString().Trim());

                            // Имя файла хранящегося на сервере.
                            projectRegistrFiles.RegistrFileName = nameFile.ToString();

                            // Номер письма.
                            projectRegistrFiles.NumberLatter = receptionContractsForm.NumberLetter;

                            // Дата письма.
                            projectRegistrFiles.DateLetter = receptionContractsForm.DateLetter;

                            // Id поликлинники.
                            projectRegistrFiles.IdHospital = receptionContractsForm.IdHospital;

                            // Запишем ФИО принявшего файл реестра.    
                            projectRegistrFiles.logWriteFIle = MyAplicationIdentity.GetUses();

                            // Дата записи письма.
                            projectRegistrFiles.DateWriteLetter = (DateTime)DateTime.Now.Date;

                            //Время.Дата(DateTime.Now.Date.ToShortDateString());

                            // Получили реестр который нужно обновить.
                            ProjectRegistrFiles prFiles = unitDate.ProjectRegistrFilesRepository.Find(projectRegistrFiles);

                            if (prFiles == null)
                            {

                                // Копируем файл на сервер.
                                unitDate.ProjectRegistrFilesRepository.Insert(projectRegistrFiles, fileName, newFileRegistrPostCopy);

                                var id = projectRegistrFiles.IdProjectRegistr;

                                // Запишем номера договоров хранящиеся в файле реестра проектов договоров.
                                InsertNumbersContracts(unitDate, registrContracts.GetDisplayContract(), projectRegistrFiles.IdProjectRegistr);
                            }
                            else
                            {
                                //DialogResult dialogResult1 = MessageBox.Show("Файл реестра проектов договоров уже записан в БД. Обновить файл реестра?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                                //if (dialogResult1 == DialogResult.Yes)
                                //{
                                //    //// Загружаемый реестр проектов договоров.
                                //    //Registr registrContractsUpdate = new Registr(unload);

                                //    // Установим id редактируемого договора.
                                //    projectRegistrFiles.IdProjectRegistr = prFiles.IdProjectRegistr;

                                //    // Обновим файл реестра договоров.
                                //    unitDate.ProjectRegistrFilesRepository.Update(projectRegistrFiles);

                                //    // Получим номера договоров которые сейчас находятся в БД и которые нужно удалить для обновления.
                                //    var contracts = unitDate.ListNumbersProgectsReestr.Select(prFiles.IdProjectRegistr);

                                //    if (contracts.Count() > 0)
                                //    {
                                //        unitDate.ListNumbersProgectsReestr.DeleteAll(contracts);

                                //        // Запишем номера договоров хранящиеся в файле реестра проектов договоров.
                                //        //InsertNumbersContracts(unitDate, registrContractsUpdate.GetDisplayContract(), projectRegistrFiles.IdProjectRegistr);

                                //        InsertNumbersContracts(unitDate, registrContracts.GetDisplayContract(), projectRegistrFiles.IdProjectRegistr);
                                //    }

                                //}
                                //else
                                //{
                                //    return;
                                //}
                            }

                            // Закроем файловый поток
                            //fstream.Close();

                            var listUnload = unload.Values.ToList<Unload>();

                            // Получим список проектов договоров.
                            foreach(var unload in listUnload)
                            {
                                // // Наименование населенного пункта.
                                ISity sity = new NameSity();

                                DataTable tabSity = unload.НаселённыйПункт;

                                if (tabSity.Rows.Count > 0 && tabSity.Rows[0]["Наименование"] != DBNull.Value)
                                {
                                    // Получим наименование населенного пункта в котором проживает льготник.
                                    sity.NameTown = tabSity.Rows[0]["Наименование"].ToString().Trim();
                                }
                                else
                                {
                                    sity.NameTown = "";
                                }

                                // Получим льготную категорию.
                                Repository.ЛьготнаяКатегория льготнаяКатегория = unitDate.ЛьготнаяКатегорияRepository.GetЛьготнаяКатегория(unload.ЛьготнаяКатегория.Trim());

                                // Порочитаем из файла выгрузки все данные по льготнику.
                                DataRow rw_Льготник = unload.Льготник.Rows[0];

                                Льготник personFull = new Льготник();

                                personFull.Фамилия = rw_Льготник["Фамилия"].ToString().Trim();
                                personFull.Имя = rw_Льготник["Имя"].ToString().Trim();
                                personFull.Отчество = rw_Льготник["Отчество"].ToString().Trim();
                                //personFull.DateBirtch = " Convert(datetime,'" + Время.Дата(Convert.ToDateTime(rw_Льготник["ДатаРождения"]).ToShortDateString().Trim()) + "',112)  ";
                                personFull.ДатаРождения = Convert.ToDateTime(rw_Льготник["ДатаРождения"]);
                                personFull.улица = rw_Льготник["улица"].ToString().Trim();
                                personFull.НомерДома = rw_Льготник["НомерДома"].ToString().Trim();
                                personFull.корпус = rw_Льготник["корпус"].ToString().Trim();
                                personFull.НомерКвартиры = rw_Льготник["НомерКвартиры"].ToString().Trim();
                                personFull.СерияПаспорта = rw_Льготник["СерияПаспорта"].ToString().Trim();
                                personFull.НомерПаспорта = rw_Льготник["НомерПаспорта"].ToString().Trim();
                                personFull.ДатаВыдачиПаспорта = Convert.ToDateTime(rw_Льготник["ДатаВыдачиПаспорта"]);
                                personFull.КемВыданПаспорт = rw_Льготник["КемВыданПаспорт"].ToString().Trim();
                                personFull.id_льготнойКатегории = льготнаяКатегория.id_льготнойКатегории;
                                personFull.id_документ = (int)unload.ТипДокумента.Rows[0][0];//                      ",@idДокумент_" + iCount + " " +
                                personFull.СерияДокумента = rw_Льготник["СерияДокумента"].ToString().Trim();
                                personFull.НомерДокумента = rw_Льготник["НомерДокумента"].ToString().Trim();
                                personFull.ДатаВыдачиДокумента = Convert.ToDateTime(rw_Льготник["ДатаВыдачиДокумента"]);
                                personFull.КемВыданДокумент = rw_Льготник["КемВыданДокумент"].ToString().Trim();
                                personFull.id_область = 1;//id области у нас по умолчанию 
                                personFull.id_район = Convert.ToInt16(rw_Льготник["id_район"]);

                                // Запишем id населенного пункта.
                                var findSity = unitDate.НаселенныйПунктRepository.FiltrНаселенныйПункт(sity.NameTown);

                                if (findSity != null)
                                {
                                    personFull.id_насПункт = findSity.id_насПункт;
                                }
                                else
                                {
                                    НаселённыйПункт населённыйПункт = new НаселённыйПункт();
                                    населённыйПункт.Наименование = sity.NameTown;

                                    // Запишем по новой населенный пункт.
                                    unitDate.НаселенныйПунктRepository.Insert(населённыйПункт);

                                    personFull.id_насПункт = населённыйПункт.id_насПункт;
                                }

                                // Запишем льготника в таблицу.
                                unitDate.ЛьготникRepository.Insert(personFull);

                                // Запишем договор.
                                DataRow rowC = unload.Договор.Rows[0];

                               ControlDantist.Repository.Договор contract = new ControlDantist.Repository.Договор();

                                // Получим данные по поликлиннике.
                                DataRow rowHosp = unload.Поликлинника.Rows[0];

                                // Прочитам данные по поликлиннике.
                                var hospitalИнн = table.Where(w => w.ИНН == Convert.ToDouble(rowHosp["ИНН"])).FirstOrDefault();

                                // Получим id поликлинники.
                                int idHospital = dc.Поликлинника.Where(w => w.ИНН.ToLower().Trim() == hospitalИнн.ИНН.ToString().Trim()).Max(w => w.id_поликлинника);

                                    contract.НомерДоговора = rowC["НомерДоговора"].ToString();
                                    contract.ДатаДоговора = Convert.ToDateTime("01.01.1900");
                                    contract.ДатаАктаВыполненныхРабот = Convert.ToDateTime("01.01.1900");
                                    contract.СуммаАктаВыполненныхРабот = 0.0m;
                                    contract.id_льготнаяКатегория = льготнаяКатегория.id_льготнойКатегории;
                                    contract.id_льготник = personFull.id_льготник;
                                    contract.id_комитет = 1;
                                    contract.id_поликлинника = idHospital;
                                    contract.датаВозврата = null;
                                    contract.ДатаЗаписиДоговора = DateTime.Now.Date;
                                    contract.ДатаРеестра = null;
                                    contract.ДатаСчётФактура = null;
                                    contract.НомерРеестра = null;
                                    contract.НомерСчётФактрура = null;
                                    contract.Примечание = null;
                                    contract.СуммаАктаВыполненныхРабот = 0.0m;
                                    
                                    contract.ФлагДопСоглашения = rowC["НомерДоговора"].ToString();
                                    contract.ФлагНаличияАкта = false;
                                    contract.ФлагНаличияДоговора = false;
                                    contract.ФлагПроверки = false;
                                    contract.флагСРН = null;
                                    contract.флагУслуги = null;

                                   
                                    contract.flagАнулирован = false;
                                    contract.flagОжиданиеПроверки = false;

                                    // Id файла с реестром проектов договоров.
                                    contract.idFileRegistProgect = projectRegistrFiles.IdProjectRegistr;

                                    // Флаг анулирован.
                                    contract.ФлагАнулирован = false;

                                    // Флаг возврат на доработку.
                                    contract.ФлагВозвратНаДоработку = false;

                                    // Дата проверки.
                                    contract.ДатаПроверки = null;

                                    // Запишем ЛОГ кто записал.
                                    contract.logWrite = MyAplicationIdentity.GetUses();

                                    // Запишем данные по договору.
                                    unitDate.ДоговорRepository.Insert(contract);

                                    // Услуги по договору.
                                    DataTable tabServices = unload.УслугиПоДоговору;

                                    // Переменная для хранения строки запроса на добавление услуг в контракт.
                                    StringBuilder servicesInsert = new StringBuilder();

                                    List<IServicesContract> listServicesContract = new List<IServicesContract>();

                                    // Сформируем запрос на добавление услуг.
                                    foreach (DataRow row in tabServices.Rows)
                                    {
                                        Repository.УслугиПоДоговору services = new Repository.УслугиПоДоговору();
                                        services.НаименованиеУслуги = row["НаименованиеУслуги"].ToString();
                                        services.цена = Convert.ToDecimal(row["Цена"]);
                                        services.Количество = Convert.ToInt32(row["Количество"]);
                                        services.id_договор = contract.id_договор;
                                        services.НомерПоПеречню = row["НомерПоПеречню"].ToString();
                                        services.Сумма = Convert.ToDecimal(row["Сумма"]);
                                        services.ТехЛист = Convert.ToInt16(row["ТехЛист"]);

                                        unitDate.УслугиПоДоговоруRepository.Insert(services);
                                    }

                            }

                            // Завершим транзакцию
                            scope.Complete();

                            #endregion
                            MessageBox.Show("Файл проектов договоров в БД записан");
                        }
                    }

                }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message + " Вероятно Вы открыли реестр который не является реестром проектов договоров.");

                //    return;
                //}
            }
        }

        private void лимитДенежныхСредствToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLimitMoney formLimitMoney = new FormLimitMoney();
            formLimitMoney.MdiParent = this;
            formLimitMoney.Show();
        }

        private void найтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Откроем для теста окно сохранения проектов договоров.
            ReceptionContractsForm receptionContractsForm = new ReceptionContractsForm();

            receptionContractsForm.SaveProjectFIle = false;

            DialogResult dialogResult = receptionContractsForm.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                //    DateTime dtOut;

                //    if (DateTime.TryParse(receptionContractsForm.DateLetter.ToShortDateString(), out dtOut) == true)
                //    {
                //        IVisableRegistrs registrProjectContract = new RegistrProjectContract(receptionContractsForm.NumberLetter, receptionContractsForm.DateLetter, receptionContractsForm.IdHospital, this.FlagConnectServer);

                //        registrProjectContract.Create();
                //    }

                return;

            }
        }

        private void статистикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormStatistic formStatistic = new FormStatistic();
            formStatistic.Show();

            return;
        }

        private void поискПроектаДоговораToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Поиск сопроводительного письма.
            FindLettert findLettert = new FindLettert();
            //findLettert.MdiParent = this;
            //findLettert.WindowState = FormWindowState.Maximized;
            findLettert.Show();
        }

        private void проверкаToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void остаткиЛБОToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlDantist.Reports.PrintReportLimitLBO pr = new ControlDantist.Reports.PrintReportLimitLBO();
            pr.Print();
        }

        private void залить2019ГодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;

            //переменная конфигурации
            int iConfig;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открыть файл реестра";
            //openFileDialog1.Filter = "|*.r";
            // openFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;

            // Путь к папке мой компьютер.
            openFileDialog1.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string fileName = openFileDialog1.FileName;

            }
        }

        private void залить2019ГодToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;

            //// Строка содержащее путь к директории.
            //string dirName = null;

            ////переменная конфигурации
            //int iConfig;

            ////// Диалог для доступа к директориям.
            ////FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            ////DialogResult result = folderBrowserDialog1.ShowDialog();

            //// Репозиторий для доступа к БД.
            //UnitDate unitDate = new UnitDate();

            //Repository.DataClasses1DataContext dc = unitDate.DateContext;

            //var table = dc.ПоликлинникиИнн.Select(w => new ИннHosp { IdHosp = Convert.ToInt32(w.F1), ИНН = (double)w.F3 }).ToList();

            ////if (result == DialogResult.OK)
            ////{
            //// Получим путь к файлу.
            ////dirName = folderBrowserDialog1.SelectedPath;

            ////dirName = @"\\10.159.102.128\d$\Для обмена\ЗУБОПРОТЕЗИРОВАНИЕ 2019\ПРОВЕРКА\8\Реестры\"; // Отработано";

            ////MessageBox.Show("Количество файлов - " + Directory.GetDirectories(dirName).Count());

            //dirName = @"F:\!\1Прокуратура";

            //    // Пройдемся по всем вложенным каталогам.
            //foreach (var dir in Directory.GetDirectories(dirName))
            //    {
            //        // Проверим существует ли данная директория.
            //        if (Directory.Exists(dir))
            //        {
            //            string[] files = Directory.GetFiles(dir);

            //            // Пройдеся по файлам в выбранной директории.
            //            foreach (var fileName in files)
            //            {
            //                // Получим имя файла и его путь для записи пути и имени файла в лог.
            //                FileInfo fileInf = new FileInfo(fileName);

            //                string fileFull = fileInf.FullName;

            //                using (FileStream fstream = File.Open(fileName, FileMode.Open))
            //                {
            //                    BinaryFormatter binaryFormatter = new BinaryFormatter();

            //                //try
            //                //{
            //                // Проверим соответствует ли файл реестру.
            //                //if ((Dictionary<string, Unload>)binaryFormatter.Deserialize(fstream) is Dictionary<string, Unload>)
            //                //{
            //                // Получим из файла словарь с договорами.
            //                unload = (Dictionary<string, DantistLibrary.Unload>)binaryFormatter.Deserialize(fstream);
            //                //List<Unload> unload = (List<Unload>)binaryFormatter.Deserialize(fstream);
            //                //}
            //                //else
            //                //{
            //                //    continue;
            //                //}
            //                //}
            //                //catch
            //                //{
            //                //    using (TextWriter write = File.AppendText(@"F:\!\Зубы\2020 год\Ошибки\fileLog.txt"))
            //                //    {
            //                //        write.WriteLine(fileFull);
            //                //    }

            //                //    continue;
            //                //}

            //                // Загружаемый реестр проектов договоров.
            //                Registr registrContracts = new Registr(unload);

            //                    // Установим уровни изоляции транзакций.
            //                    var option = new System.Transactions.TransactionOptions();
            //                    option.IsolationLevel = System.Transactions.IsolationLevel.Serializable;

            //                    // Добавим льготника и адрес в БД.
            //                    // Внесём данные в таблицу в единой транзакции.
            //                    using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, option))
            //                    {
            //                        var listUnload = unload.Values.ToList<Unload>();

            //                        foreach (Unload unload2 in listUnload)
            //                        {
            //                            // // Наименование населенного пункта.
            //                            ISity sity = new NameSity();

            //                            DataTable tabSity = unload.НаселённыйПункт;

            //                            if (tabSity.Rows.Count > 0 && tabSity.Rows[0]["Наименование"] != DBNull.Value)
            //                            {
            //                                // Получим наименование населенного пункта в котором проживает льготник.
            //                                sity.NameTown = tabSity.Rows[0]["Наименование"].ToString().Trim();
            //                            }
            //                            else
            //                            {
            //                                sity.NameTown = "";
            //                            }

            //                            // Получим льготную категорию.
            //                            Repository.ЛьготнаяКатегория льготнаяКатегория = unitDate.ЛьготнаяКатегорияRepository.GetЛьготнаяКатегория(unload.ЛьготнаяКатегория.Trim());

            //                            // Порочитаем из файла выгрузки все данные по льготнику.
            //                            DataRow rw_Льготник = unload.Льготник.Rows[0];

            //                            ЛьготникAdd personFull = new ЛьготникAdd();

            //                            personFull.Фамилия = rw_Льготник["Фамилия"].ToString().Trim();
            //                            personFull.Имя = rw_Льготник["Имя"].ToString().Trim();
            //                            personFull.Отчество = rw_Льготник["Отчество"].ToString().Trim();
            //                            //personFull.DateBirtch = " Convert(datetime,'" + Время.Дата(Convert.ToDateTime(rw_Льготник["ДатаРождения"]).ToShortDateString().Trim()) + "',112)  ";
            //                            personFull.ДатаРождения = Convert.ToDateTime(rw_Льготник["ДатаРождения"]);
            //                            personFull.улица = rw_Льготник["улица"].ToString().Trim();
            //                            personFull.НомерДома = rw_Льготник["НомерДома"].ToString().Trim();
            //                            personFull.корпус = rw_Льготник["корпус"].ToString().Trim();
            //                            personFull.НомерКвартиры = rw_Льготник["НомерКвартиры"].ToString().Trim();
            //                            personFull.СерияПаспорта = rw_Льготник["СерияПаспорта"].ToString().Trim();
            //                            personFull.НомерПаспорта = rw_Льготник["НомерПаспорта"].ToString().Trim();
            //                            personFull.ДатаВыдачиПаспорта = Convert.ToDateTime(rw_Льготник["ДатаВыдачиПаспорта"]);
            //                            personFull.КемВыданПаспорт = rw_Льготник["КемВыданПаспорт"].ToString().Trim();
            //                            personFull.id_льготнойКатегории = льготнаяКатегория.id_льготнойКатегории;
            //                            personFull.id_документ = (int)unload.ТипДокумента.Rows[0][0];//                      ",@idДокумент_" + iCount + " " +
            //                            personFull.СерияДокумента = rw_Льготник["СерияДокумента"].ToString().Trim();
            //                            personFull.НомерДокумента = rw_Льготник["НомерДокумента"].ToString().Trim();
            //                            personFull.ДатаВыдачиДокумента = Convert.ToDateTime(rw_Льготник["ДатаВыдачиДокумента"]);
            //                            personFull.КемВыданДокумент = rw_Льготник["КемВыданДокумент"].ToString().Trim();
            //                            personFull.id_область = 1;//id области у нас по умолчанию 
            //                            personFull.id_район = Convert.ToInt16(rw_Льготник["id_район"]);

            //                            if ((personFull.Фамилия.Trim().ToLower() == "Тюрина".ToLower().Trim()) && (personFull.Имя.Trim().ToLower() == "Галина".ToLower().Trim()) && (personFull.Отчество.Trim().ToLower() == "Викторовна".ToLower().Trim()))
            //                            {
            //                                DateTime dt = new DateTime(1942, 8, 2);

            //                                personFull.ДатаВыдачиПаспорта = dt;
            //                            }

            //                            // Запишем id населенного пункта.
            //                            var findSity = unitDate.НаселенныйПунктRepository.FiltrНаселенныйПункт(sity.NameTown);

            //                            if (findSity != null)
            //                            {
            //                                personFull.id_насПункт = findSity.id_насПункт;
            //                            }
            //                            else
            //                            {
            //                                НаселённыйПункт населённыйПункт = new НаселённыйПункт();
            //                                населённыйПункт.Наименование = sity.NameTown;

            //                                // Запишем по новой населенный пункт.
            //                                unitDate.НаселенныйПунктRepository.Insert(населённыйПункт);

            //                                personFull.id_насПункт = населённыйПункт.id_насПункт;
            //                            }

            //                            ЛьготникAdd personAdd = unitDate.ЛЬготникAddRepository.SelectPerson(personFull);

            //                            // Проверим есть ли льготник с ФИО и датой рождения в БД.
            //                            if (personAdd != null)
            //                            {
            //                                // Обновим данные по льготнику.
            //                                unitDate.ЛЬготникAddRepository.Update(personFull);

            //                                // Присвоим id льготника.
            //                                personFull.id_льготник = personAdd.id_льготник;
            //                            }
            //                            else
            //                            {
            //                                // Запишем нового льготника.
            //                                unitDate.ЛЬготникAddRepository.Insert(personFull);
            //                            }

            //                            // Запишем договор.
            //                            DataRow rowC = unload.Договор.Rows[0];

            //                            ДоговорAdd contract = new ДоговорAdd();

            //                            // Получим данные по поликлиннике.
            //                            DataRow rowHosp = unload.Поликлинника.Rows[0];

            //                            // Прочитам данные по поликлиннике.
            //                            //int idHospital = Convert.ToInt32(unitDate.ПоликлинникаИННRepository.SelectAll().ToList().Where(w=>w.F3.Value.ToString().Trim() == rowHosp["ИНН"].ToString()).FirstOrDefault().F3);
            //                            int idHospital = table.Where(w => w.ИНН == Convert.ToDouble(rowHosp["ИНН"])).FirstOrDefault().IdHosp;

            //                            contract.НомерДоговора = rowC["НомерДоговора"].ToString();
            //                            contract.ДатаДоговора = Convert.ToDateTime("01.01.1900");
            //                            contract.ДатаАктаВыполненныхРабот = Convert.ToDateTime("01.01.1900");
            //                            contract.СуммаАктаВыполненныхРабот = 0.0m;
            //                            contract.id_льготнаяКатегория = льготнаяКатегория.id_льготнойКатегории;
            //                            contract.id_льготник = personFull.id_льготник;
            //                            contract.id_комитет = 1;
            //                            contract.id_поликлинника = idHospital;
            //                            contract.датаВозврата = null;
            //                            contract.ДатаЗаписиДоговора = DateTime.Now.Date;
            //                            contract.ДатаПроверки = null;
            //                            contract.ДатаРеестра = null;
            //                            contract.ДатаСчётФактура = null;
            //                            contract.НомерРеестра = null;
            //                            contract.НомерСчётФактрура = null;
            //                            contract.Примечание = null;
            //                            contract.СуммаАктаВыполненныхРабот = 0.0m;
            //                            contract.ФлагАнулирован = false;
            //                            contract.ФлагВозвратНаДоработку = false;
            //                            contract.ФлагДопСоглашения = rowC["НомерДоговора"].ToString();
            //                            contract.ФлагНаличияАкта = false;
            //                            contract.ФлагНаличияДоговора = false;
            //                            contract.ФлагПроверки = false;
            //                            contract.флагСРН = null;
            //                            contract.флагУслуги = null;
            //                            contract.idFileRegistProgect = 0;
            //                            contract.flagАнулирован = false;
            //                            contract.flagОжиданиеПроверки = false;

            //                            // Запишем ЛОГ кто записал.
            //                            contract.logWrite = MyAplicationIdentity.GetUses();

            //                            // Запишем данные по договору.
            //                            unitDate.ДоговорAddRepository.Insert(contract);

            //                            // Услуги по договору.
            //                            DataTable tabServices = unload.УслугиПоДоговору;

            //                            // Переменная для хранения строки запроса на добавление услуг в контракт.
            //                            StringBuilder servicesInsert = new StringBuilder();

            //                            List<IServicesContract> listServicesContract = new List<IServicesContract>();

            //                            // Сформируем запрос на добавление услуг.
            //                            foreach (DataRow row in tabServices.Rows)
            //                            {
            //                                УслугиПоДоговоруAdd services = new УслугиПоДоговоруAdd();
            //                                services.НаименованиеУслуги = row["НаименованиеУслуги"].ToString();
            //                                services.цена = Convert.ToDecimal(row["Цена"]);
            //                                services.Количество = Convert.ToInt32(row["Количество"]);
            //                                services.id_договор = contract.id_договор;
            //                                services.НомерПоПеречню = row["НомерПоПеречню"].ToString();
            //                                services.Сумма = Convert.ToDecimal(row["Сумма"]);
            //                                services.ТехЛист = Convert.ToInt16(row["ТехЛист"]);

            //                                unitDate.УслугиПоДоговоруAddRepository.Insert(services);
            //                            }


            //                        }



            //                        //// Запишем в БД проекты договоров со статусом - ожидающие проверку.
            //                        //WriteBD writBD = new WriteBD(listUnload);

            //                        //    writBD.UnitDate = unitDate;

            //                        //    IStringQuery stringQuery = new StringQueryAdd();

            //                        //    writBD.queryWrite = stringQuery;

            //                        //    // Запишем в БД.
            //                        //string query = writBD.Write();

            //                        //dc.ExecuteCommand(query);

            //                        // Завершим транзакцию
            //                        scope.Complete();

            //                        //MessageBox.Show("Файл проектов договоров в БД записан");
            //                    }
            //                    //}


            //                }
            //            }
            //        }
            //    }

                

            ////}


            //MessageBox.Show("Реестры в БД записаны");

            //// Список проектов договоров отображаемых в письме.
            ////List<DisplayContract> listContracts = new List<DisplayContract>();


        }

        private void проектыДоговоровToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void конвертироватьДоговорToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void поискПоЭСРНToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindPersonESRN findPersonESRN = new FindPersonESRN();
            findPersonESRN.MdiParent = this;
            findPersonESRN.Show();
        }

        private void уУстраняемКасякСАктамиToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void конвертор2019ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;

            // Строка содержащее путь к директории.
            string dirName = null;

            //переменная конфигурации
            int iConfig;

            //// Диалог для доступа к директориям.
            //FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            //DialogResult result = folderBrowserDialog1.ShowDialog();

            // Репозиторий для доступа к БД.
            UnitDate unitDate = new UnitDate();

            Repository.DataClasses1DataContext dc = unitDate.DateContext;

            var table = dc.ПоликлинникиИнн.Select(w => new ИннHosp { IdHosp = Convert.ToInt32(w.F1), ИНН = (double)w.F3 }).ToList();

            ////if (result == DialogResult.OK)
            ////{
            //// Получим путь к файлу.
            ////dirName = folderBrowserDialog1.SelectedPath;

            ////dirName = @"\\10.159.102.128\d$\Для обмена\ЗУБОПРОТЕЗИРОВАНИЕ 2019\ПРОВЕРКА\8\Реестры\"; // Отработано";

            ////MessageBox.Show("Количество файлов - " + Directory.GetDirectories(dirName).Count());

            //dirName = @"F:\!\1Прокуратура\Отработано 1 файл";
            dirName = @"F:\!\1Прокуратура\Файлы госпиталя за 2019";

            //// Пройдемся по всем вложенным каталогам.
            //foreach (var dir in Directory.GetDirectories(dirName))
            //{
            //    // Проверим существует ли данная директория.
            //    if (Directory.Exists(dir))
            //    {

            string[] files = Directory.GetFiles(dirName);

                   // Пройдеся по файлам в выбранной директории.
                    foreach (var fileName in files)
                    {
                        // Получим имя файла и его путь для записи пути и имени файла в лог.
                        FileInfo fileInf = new FileInfo(fileName);

                        string fileFull = fileInf.FullName;

                        using (FileStream fstream = File.Open(fileName, FileMode.Open))
                        {
                            BinaryFormatter binaryFormatter = new BinaryFormatter();

                            //try
                            //{
                            // Проверим соответствует ли файл реестру.
                            //if ((Dictionary<string, Unload>)binaryFormatter.Deserialize(fstream) is Dictionary<string, Unload>)
                            //{
                            // Получим из файла словарь с договорами.
                            //unload = (Dictionary<string, Unload>)binaryFormatter.Deserialize(fstream);
                            //List<Unload> unload = (List<Unload>)binaryFormatter.Deserialize(fstream);
                            List<Unload> listUnload = (List<Unload>)binaryFormatter.Deserialize(fstream);
                            //}
                            //else
                            //{
                            //    continue;
                            //}
                            //}
                            //catch
                            //{
                            //    using (TextWriter write = File.AppendText(@"F:\!\Зубы\2020 год\Ошибки\fileLog.txt"))
                            //    {
                            //        write.WriteLine(fileFull);
                            //    }

                            //    continue;
                            //}

                            // Загружаемый реестр проектов договоров.
                            // Registr registrContracts = new Registr(unload);

                            // Установим уровни изоляции транзакций.
                            var option = new System.Transactions.TransactionOptions();
                            option.IsolationLevel = System.Transactions.IsolationLevel.Serializable;


                            // Добавим льготника и адрес в БД.
                            // Внесём данные в таблицу в единой транзакции.
                            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, option))
                            {
                        //var listUnload = unload.Values.ToList<Unload>();

                        foreach (Unload unload2 in listUnload)
                        {

                            //// Запишем договор.
                            //DataRow rowC2 = unload2.Договор.Rows[0];

                            //if (rowC2["НомерДоговора"].ToString().Trim() == "8/6669".Trim())
                            //{
                            //    var test = "";

                            //    string fileNameStop = fileName;

                            //    MessageBox.Show("Номер договора - " + rowC2["НомерДоговора"].ToString() + " файл - " + fileNameStop);
                            //}

                            //continue;


                            // // Наименование населенного пункта.
                            ISity sity = new NameSity();

                                    DataTable tabSity = unload2.НаселённыйПункт;

                                    if (tabSity.Rows.Count > 0 && tabSity.Rows[0]["Наименование"] != DBNull.Value)
                                    {
                                        // Получим наименование населенного пункта в котором проживает льготник.
                                        sity.NameTown = tabSity.Rows[0]["Наименование"].ToString().Trim();
                                    }
                                    else
                                    {
                                        sity.NameTown = "";
                                    }

                                    // Получим льготную категорию.
                                    Repository.ЛьготнаяКатегория льготнаяКатегория = unitDate.ЛьготнаяКатегорияRepository.GetЛьготнаяКатегория(unload2.ЛьготнаяКатегория.Trim());

                                    // Порочитаем из файла выгрузки все данные по льготнику.
                                    DataRow rw_Льготник = unload2.Льготник.Rows[0];

                                    ЛьготникAdd personFull = new ЛьготникAdd();

                                    personFull.Фамилия = rw_Льготник["Фамилия"].ToString().Trim();
                                    personFull.Имя = rw_Льготник["Имя"].ToString().Trim();
                                    personFull.Отчество = rw_Льготник["Отчество"].ToString().Trim();
                                    //personFull.DateBirtch = " Convert(datetime,'" + Время.Дата(Convert.ToDateTime(rw_Льготник["ДатаРождения"]).ToShortDateString().Trim()) + "',112)  ";
                                    personFull.ДатаРождения = Convert.ToDateTime(rw_Льготник["ДатаРождения"]);
                                    personFull.улица = rw_Льготник["улица"].ToString().Trim();
                                    personFull.НомерДома = rw_Льготник["НомерДома"].ToString().Trim();
                                    personFull.корпус = rw_Льготник["корпус"].ToString().Trim();
                                    personFull.НомерКвартиры = rw_Льготник["НомерКвартиры"].ToString().Trim();
                                    personFull.СерияПаспорта = rw_Льготник["СерияПаспорта"].ToString().Trim();
                                    personFull.НомерПаспорта = rw_Льготник["НомерПаспорта"].ToString().Trim();
                                    personFull.ДатаВыдачиПаспорта = Convert.ToDateTime(rw_Льготник["ДатаВыдачиПаспорта"]);
                                    personFull.КемВыданПаспорт = rw_Льготник["КемВыданПаспорт"].ToString().Trim();
                                    personFull.id_льготнойКатегории = льготнаяКатегория.id_льготнойКатегории;
                                    personFull.id_документ = (int)unload2.ТипДокумента.Rows[0][0];//                      ",@idДокумент_" + iCount + " " +
                                    personFull.СерияДокумента = rw_Льготник["СерияДокумента"].ToString().Trim();
                                    personFull.НомерДокумента = rw_Льготник["НомерДокумента"].ToString().Trim();
                                    personFull.ДатаВыдачиДокумента = Convert.ToDateTime(rw_Льготник["ДатаВыдачиДокумента"]);
                                    personFull.КемВыданДокумент = rw_Льготник["КемВыданДокумент"].ToString().Trim();
                                    personFull.id_область = 1;//id области у нас по умолчанию 
                                    personFull.id_район = Convert.ToInt16(rw_Льготник["id_район"]);

                                    if ((personFull.Фамилия.Trim().ToLower() == "Тюрина".ToLower().Trim()) && (personFull.Имя.Trim().ToLower() == "Галина".ToLower().Trim()) && (personFull.Отчество.Trim().ToLower() == "Викторовна".ToLower().Trim()))
                                    {
                                        DateTime dt = new DateTime(1942, 8, 2);

                                        personFull.ДатаВыдачиПаспорта = dt;
                                    }

                                    // Запишем id населенного пункта.
                                    var findSity = unitDate.НаселенныйПунктRepository.FiltrНаселенныйПункт(sity.NameTown);

                                    if (findSity != null)
                                    {
                                        personFull.id_насПункт = findSity.id_насПункт;
                                    }
                                    else
                                    {
                                        НаселённыйПункт населённыйПункт = new НаселённыйПункт();
                                        населённыйПункт.Наименование = sity.NameTown;

                                        // Запишем по новой населенный пункт.
                                        unitDate.НаселенныйПунктRepository.Insert(населённыйПункт);

                                        personFull.id_насПункт = населённыйПункт.id_насПункт;
                                    }

                                    ЛьготникAdd personAdd = unitDate.ЛЬготникAddRepository.SelectPerson(personFull);

                                    // Проверим есть ли льготник с ФИО и датой рождения в БД.
                                    if (personAdd != null)
                                    {
                                        // Обновим данные по льготнику.
                                        unitDate.ЛЬготникAddRepository.Update(personFull);

                                        // Присвоим id льготника.
                                        personFull.id_льготник = personAdd.id_льготник;
                                    }
                                    else
                                    {
                                        // Запишем нового льготника.
                                        unitDate.ЛЬготникAddRepository.Insert(personFull);
                                    }

                                    // Запишем договор.
                                    DataRow rowC = unload2.Договор.Rows[0];

                                    ДоговорAdd contract = new ДоговорAdd();

                                    // Получим данные по поликлиннике.
                                    DataRow rowHosp = unload2.Поликлинника.Rows[0];

                                    // Прочитам данные по поликлиннике.
                                    //int idHospital = Convert.ToInt32(unitDate.ПоликлинникаИННRepository.SelectAll().ToList().Where(w=>w.F3.Value.ToString().Trim() == rowHosp["ИНН"].ToString()).FirstOrDefault().F3);
                                    int idHospital = table.Where(w => w.ИНН == Convert.ToDouble(rowHosp["ИНН"])).FirstOrDefault().IdHosp;

                                    contract.НомерДоговора = rowC["НомерДоговора"].ToString();
                                    contract.ДатаДоговора = Convert.ToDateTime("01.01.1900");
                                    contract.ДатаАктаВыполненныхРабот = Convert.ToDateTime("01.01.1900");
                                    contract.СуммаАктаВыполненныхРабот = 0.0m;
                                    contract.id_льготнаяКатегория = льготнаяКатегория.id_льготнойКатегории;
                                    contract.id_льготник = personFull.id_льготник;
                                    contract.id_комитет = 1;
                                    contract.id_поликлинника = idHospital;
                                    contract.датаВозврата = null;
                                    contract.ДатаЗаписиДоговора = DateTime.Now.Date;
                                    contract.ДатаПроверки = null;
                                    contract.ДатаРеестра = null;
                                    contract.ДатаСчётФактура = null;
                                    contract.НомерРеестра = null;
                                    contract.НомерСчётФактрура = null;
                                    contract.Примечание = null;
                                    contract.СуммаАктаВыполненныхРабот = 0.0m;
                                    contract.ФлагАнулирован = false;
                                    contract.ФлагВозвратНаДоработку = false;
                                    contract.ФлагДопСоглашения = rowC["НомерДоговора"].ToString();
                                    contract.ФлагНаличияАкта = false;
                                    contract.ФлагНаличияДоговора = false;
                                    contract.ФлагПроверки = false;
                                    contract.флагСРН = null;
                                    contract.флагУслуги = null;
                                    contract.idFileRegistProgect = 0;
                                    contract.flagАнулирован = false;
                                    contract.flagОжиданиеПроверки = false;

                                    // Запишем ЛОГ кто записал.
                                    contract.logWrite = MyAplicationIdentity.GetUses();

                                    // Запишем данные по договору.
                                    unitDate.ДоговорAddRepository.Insert(contract);

                                    // Услуги по договору.
                                    DataTable tabServices = unload2.УслугиПоДоговору;

                                    // Переменная для хранения строки запроса на добавление услуг в контракт.
                                    StringBuilder servicesInsert = new StringBuilder();

                                    List<IServicesContract> listServicesContract = new List<IServicesContract>();

                                    // Сформируем запрос на добавление услуг.
                                    foreach (DataRow row in tabServices.Rows)
                                    {
                                        УслугиПоДоговоруAdd services = new УслугиПоДоговоруAdd();
                                        services.НаименованиеУслуги = row["НаименованиеУслуги"].ToString();
                                        services.цена = Convert.ToDecimal(row["Цена"]);
                                        services.Количество = Convert.ToInt32(row["Количество"]);
                                        services.id_договор = contract.id_договор;
                                        services.НомерПоПеречню = row["НомерПоПеречню"].ToString();
                                        services.Сумма = Convert.ToDecimal(row["Сумма"]);
                                        services.ТехЛист = Convert.ToInt16(row["ТехЛист"]);

                                        unitDate.УслугиПоДоговоруAddRepository.Insert(services);
                                    }


                                }



                                //// Запишем в БД проекты договоров со статусом - ожидающие проверку.
                                //WriteBD writBD = new WriteBD(listUnload);

                                //    writBD.UnitDate = unitDate;

                                //    IStringQuery stringQuery = new StringQueryAdd();

                                //    writBD.queryWrite = stringQuery;

                                //    // Запишем в БД.
                                //string query = writBD.Write();

                                //dc.ExecuteCommand(query);

                                // Завершим транзакцию
                                scope.Complete();

                                

                            }
                        }

                //  
               // MessageBox.Show("Файл проектов договоров в БД записан");
            }
            //    }
            //}

            MessageBox.Show("Реестры в БД записаны");


        }

        private void поискУчастиниковВОВToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //PullConnectBD pull = new PullConnectBD();
            //Dictionary<string, string> pullConnect = pull.GetPull(FlagConnectServer);

            // Строки подключения к БД АИС ЭСРН.
            ConfigLibrary.Config config = new ConfigLibrary.Config();

            //Пока закоментирован.
            //Получаем словарь со строками подключения к АИС ЭСРН.
            Dictionary<string, string> pullConnect = config.Select();

            // Счетчик.
            int iCount = 1;

            foreach (KeyValuePair<string, string> dStringConnect in pullConnect)
            {
                // Учимся

                //ТаблицаБД.GetTableSQL()

                //========

                string regionName = dStringConnect.Key;

                // Строка для хранения SQL запроса.
                StringBuilder queryInsert = new StringBuilder();

                // Строка для хранения SQL запроса к БД.
                string sConnection = string.Empty;
                sConnection = dStringConnect.Value.ToString().Trim();

                // Строка запроса к БД ЭСРН, для поиска льготников в ЭСРН.
                IFindPerson findPersonQuery = new FindPersonsWow();
                string query = findPersonQuery.Query();

                DataTable tabPersons = ТаблицаБД.GetTableSQL(query, "PersonWoW2" + iCount.ToString(), sConnection);
                
                // Проверим наличие таблицы, строк в таблице и количество строк в таблице.
                if(tabPersons != null && tabPersons.Rows != null && tabPersons.Rows.Count > 0)
                {
                    foreach (DataRow row in tabPersons.Rows)
                    {
                        string famili = string.Empty;
                        string name = string.Empty;
                        string surName = string.Empty;
                        string dr = string.Empty;
                        string address = string.Empty;
                        string category = string.Empty;

                        if (row["Ф"] != null)
                            famili = row["Ф"].ToString();

                        if (row["И"] != null)
                            name = row["И"].ToString();

                        if (row["О"] != null)
                            surName = row["О"].ToString();

                        if (row["ДР"] != null)
                        {
                            dr = Convert.ToDateTime(row["ДР"]).ToShortDateString();
                        }
                        else
                        {
                            DateTime dt = new DateTime(1900, 1, 1);
                            dr = dt.ToShortDateString();
                        }

                        if (row["Адрес"] != null)
                            address = row["Адрес"].ToString().Replace(","," ");

                        if (row["Категория"] != null)
                            category = row["Категория"].ToString().Replace(",", " ");

                        // Добавляем льготника в строку запроса.
                        WritePersonWow writePersonWow = new WritePersonWow(famili, name, surName,Время.Дата(dr), address, category, regionName);
                        queryInsert.Append(writePersonWow.InsertPerson());

                    }
                }

                //  Получаем строку запроса для Insert в единой транзакции.
                string strQueryInsert = queryInsert.ToString();

                if (strQueryInsert != "")
                {
                    // Выполним запрос в БД.
                    ExecuteQuery.Execute(strQueryInsert, ConnectDB.ConnectionString());
                }
                iCount++;

            }

            MessageBox.Show("Сканирование закончено");
        }

    }
}

