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

using DantistLibrary;
using ControlDantist.Classes;


namespace ControlDantist
{
    public partial class MainForm : Form
    {
        //Переменная хранит id поликлинники которой принадлежит файл реестра
        private int idHosp;
        private string видУслуг = string.Empty;
        private decimal цена = 0.0m;

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
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открыть файл реестра";
            openFileDialog1.Filter = "|*.r";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string fileName = openFileDialog1.FileName;

               //SerializableObject objToSerialize = null;
                FileStream fstream = File.Open(fileName, FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                List<Unload> unload = (List<Unload>)binaryFormatter.Deserialize(fstream);

                ////Создадим список который содержит расхождения в реестре
                List<ErrorReestr> list = new List<ErrorReestr>();

                //Создадим список который содержит реестр который прошёл проверку 
                List<ReestrControl> listControlReestr = new List<ReestrControl>();

                //Откроем соединение с БД на сервере
                using(SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
                {
                    con.Open();
                    //Выполним всё в единой транзакции
                    SqlTransaction transact = con.BeginTransaction();

                    //Проверим информацию из реестра выполненных договоров с информацией записанной в базе данных
                    //дання информация содержит кроме эталонных значений вида услуг и их стоимости, ещё и данные которые
                    //выгруженных проектов договоров

                    //Сравним содержимое реестра с записями в базе данных
                    foreach (Unload un in unload)
                    {
                        //Получим ФИО льготника
                        DataRow rowЛьготник = un.Льготник.Rows[0];

                        string фамилия = rowЛьготник["Фамилия"].ToString();
                        string имя = rowЛьготник["Имя"].ToString();
                        string отчество = rowЛьготник["Отчество"].ToString();

                        //
                        string фио = фамилия + " " + имя + " " + отчество;

                        //Получим данные по текущему договору
                        DataRow rowContr = un.Договор.Rows[0];

                        //Получим номер договора
                        string numContr = rowContr["НомерДоговора"].ToString().Trim();

                        //Сравним есть ли такой договор в нашей БД
                        string queryDog = "select НомерДоговора from dbo.Договор where НомерДоговора = '" + numContr + "' ";

                        //получим номер договора в нашей базе
                        DataTable TabDogS = ТаблицаБД.GetTableSQL(queryDog, "Договор", con, transact);

                        //если текущий договор существует в БД
                        if (TabDogS.Rows.Count != 0)
                        {
                            //Запишем данные в таблицу АктВыполненныхРабот
                            DataTable tab = un.АктВыполненныхРабот;

                            //Запишем данные по акту выполненных работ в БД на сервер

                        }
                        //else
                        //{
                        //    //Если текущий договор в БД отсутствует
                        //    MessageBox.Show("Договор № = " + numContr + " в базе данных отсутсвует");
                        //}



                    }

                    #region Старая реализация
                    //Сравним содержимое реестра с записями в базе данных
                    //foreach (Unload un in unload)
                    //{
                    //    ////Создадим экземпляр объекта для хранения ошибочной информации
                    //    //ErrorsReestrUnload error = new ErrorsReestrUnload();

                    //    ErrorReestr errorReestr = new ErrorReestr();

                    //    //Создадим экземпляр объекта для хранения строки реестра на оказанные услуги успешно прошедшие проверку
                    //    ReestrControl rControl = new ReestrControl();

                    //    //Получим ФИО льготника которого запишем в случае ошибки в реестр
                    //    DataRow rowЛьготник = un.Льготник.Rows[0];

                    //    string фамилия = rowЛьготник["Фамилия"].ToString();
                    //    string имя = rowЛьготник["Имя"].ToString();
                    //    string отчество = rowЛьготник["Отчество"].ToString();

                    //    //Запишем в реестр ФИО текущего льготника
                    //    errorReestr.ФИО = фамилия + " " + имя + " " + отчество;

                    //    //Запишем ФИО льготника
                    //    rControl.ФИО = фамилия + " " + имя + " " + отчество;

                    //    //Запишем дату и номер договора на оказание услуг
                    //    DataRow rowControlReestrДоговор = un.Договор.Rows[0];

                    //    //Запишем номер поликлинники и номер договора
                    //    string номерДоговора = rowControlReestrДоговор["НомерДоговора"].ToString();

                    //    //Запишем дату договора
                    //    string датаДоговора = Convert.ToDateTime(rowControlReestrДоговор["ДатаДоговора"]).ToShortDateString();

                    //    //запишем дату и номер договора в реестр
                    //    rControl.ДатаНомерДоговора = номерДоговора + " " + датаДоговора;

                    //    //Запишем дату и номер акта оказанных услуг
                    //    DataRow rowControlReestrАкт = un.АктВыполненныхРабот.Rows[0];

                    //    //Получим номер акта 
                    //    string номерАкта = rowControlReestrАкт["НомерАкта"].ToString();

                    //    //Запишем дату акта оказанных услуг
                    //    string датаАкта = Convert.ToDateTime(rowControlReestrАкт["ДатаПодписания"]).ToShortDateString();

                    //    //Запишем в реестр номер и дату акта оказанных услуг 
                    //    rControl.НомерАктаОказанныхУслуг = номерАкта + " " + датаАкта;

                    //    //Получим серию и номер документа о праве на льготу
                    //    DataRow rowПравоЛьготы = un.Льготник.Rows[0];

                    //    //Серия документа
                    //    string серия = rowПравоЛьготы["СерияДокумента"].ToString();

                    //    //Запишем номер поликлинники и номер договора
                    //    string номерДокумента = rowПравоЛьготы["НомерДокумента"].ToString();

                    //    //Запишем дату договора
                    //    string датаДокумента = Convert.ToDateTime(rowПравоЛьготы["ДатаВыдачиДокумента"]).ToShortDateString();

                    //    rControl.ДокументЛьгота = серия + " " + номерДокумента + " " + датаДокумента;

                    //    //Запишем название льготной категории
                    //    льготнаяКатегория = un.ЛьготнаяКатегория;


                    //    //Создадим список который содержит расхождения в реестре
                    //    List<ErrorsReestrUnload> listError = new List<ErrorsReestrUnload>();

                    //    //Узнаем какой поликлиннике принадлежит файл реестра
                    //    DataRow rowHosp = un.Поликлинника.Rows[0];

                    //    //Получим id поликлинники
                    //    string queryIdHosp = "select id_поликлинника from dbo.Поликлинника where ИНН = " + rowHosp["ИНН"].ToString() + " ";

                    //    SqlCommand com = new SqlCommand(queryIdHosp, con);
                    //    com.Transaction = transact;
                    //    SqlDataReader read = com.ExecuteReader();

                    //    while (read.Read())
                    //    {
                    //        idHosp = Convert.ToInt32(read["id_поликлинника"]);
                    //    }

                    //    read.Close();

                    //    //Получим список услуг по текущему договору
                    //    DataTable tabДоговор = un.УслугиПоДоговору;
                    //    foreach (DataRow rowDog in tabДоговор.Rows)
                    //    {
                    //        //Создадим экземпляр объекта для хранения ошибочной информации для конкретного льготника
                    //        ErrorsReestrUnload error = new ErrorsReestrUnload();

                    //        string linkText = "'%" + rowDog["НаименованиеУслуги"].ToString() + "%'"; 
                    //        //проверим название вида услуг и стоимость которая указана в файле реестра
                    //        string queryViewServices = "select ВидУслуги,Цена from dbo.ВидУслуг " +
                    //                                   "where id_поликлинника = " + idHosp + " and ВидУслуги like "+ linkText +" ";

                    //        SqlCommand comViewServ = new SqlCommand(queryViewServices, con);
                    //        comViewServ.Transaction = transact;
                    //        SqlDataReader readViewServ = comViewServ.ExecuteReader();

                    //        //Получим название услуги и стоимость которая находится у нас на сервере
                    //        while (readViewServ.Read())
                    //        {
                    //            видУслуг = readViewServ["ВидУслуги"].ToString().Trim();
                    //            цена = Convert.ToDecimal(readViewServ["Цена"]);
                    //        }
                            
                    //        //закроем DataReader
                    //        readViewServ.Close();

                    //        //теперь сравним что лежит в базе и что лежит в файле реестра
                    //        if (rowDog["НаименованиеУслуги"].ToString().Trim() == видУслуг.Trim())
                    //        {
                    //            //ошибки нет
                    //            errorFlagВидУслуг = false;

                    //        }
                    //        else
                    //        {
                    //            //ошибка
                    //            errorFlagВидУслуг = true;

                    //            //запишем правильное наименование
                    //            error.НаименованиеУслуги = видУслуг.Trim();

                    //            //запишем ошибку
                    //            error.ErrorНаименованиеУслуги = rowDog["НаименованиеУслуги"].ToString().Trim();

                    //        }

                    //        //теперь сравним стоимость услуги
                    //        if (Convert.ToDecimal(rowDog["Цена"]) == цена)
                    //        {
                    //            //ошибки нет
                    //            errorFlagЦена = false;
                    //        }
                    //        else
                    //        {
                    //            //ошибка
                    //            errorFlagЦена = true;

                    //            //Запишем наименование услуги 
                    //            error.НаименованиеУслуги = видУслуг.Trim();


                    //            //запишем разницу
                    //            error.Цена = цена;
                    //            error.ErrorЦена = Convert.ToDecimal(rowDog["Цена"]);
                    //        }

                    //        //теперь проверим правильно ли вычеслина сумма оказанных услуг по данному виду работы
                    //        int количество = Convert.ToInt32(rowDog["Количество"]);

                    //        //подсчитаем контрольную сумму стоимости услуг
                    //        decimal сумма = Math.Round((Math.Round(цена, 2) * количество), 2);

                    //        //Подсчитаем итоговую сумму услуг для конкретного льготника
                    //        суммаСтоимостьУслуг = Math.Round((суммаСтоимостьУслуг + сумма), 2);

                    //        //подсчитаем сумму в файле выгрузки для конкретного реестра
                    //        errorСуммаСтоимостьУслуг = Math.Round((errorСуммаСтоимостьУслуг + Convert.ToDecimal(rowDog["Сумма"])), 2);

                    //        //сравим сумму по услуге
                    //        if (Convert.ToDecimal(rowDog["Сумма"]) == сумма)
                    //        {
                    //            //ошибки нет
                    //            errorFlagСтоимостьУслуги = false;
                    //        }
                    //        else
                    //        {
                    //            //ошибка
                    //            errorFlagСтоимостьУслуги = true;

                    //            //запишем разницу
                    //            error.Сумма = сумма;
                    //            error.ErrorСумма = Convert.ToDecimal(rowDog["Сумма"]);

                    //        }

                    //        //Сравним результат который у нас получился
                    //        if (errorFlagВидУслуг == false && errorFlagЦена == false && errorFlagСтоимостьУслуги == false)
                    //        {
                    //            //ошибки в данном виде услуг не произошло
                                
                    //        }
                    //        else
                    //        {
                    //            //произошла ошибка и мы выставим флаг реестра в ошибку 
                    //            errorРеестр = true;

                    //            //запишем результат проверки текущей строки в список ошибки
                    //            listError.Add(error);
                    //        }

                            
                    //    }

                       

                    //    //Сравним стоимости услуг из файла и из сервера в БД
                    //    if (суммаСтоимостьУслуг == errorСуммаСтоимостьУслуг)
                    //    {
                    //         //Запишем стоимость услуг
                    //        rControl.СтоимостьУслуг = суммаСтоимостьУслуг.ToString();
                    //    }
                    //    else
                    //    {
                    //        errorReestr.СуммаИтогоСтоимостьУслуг = суммаСтоимостьУслуг;
                    //        errorReestr.ErrorСуммаИтогоСтоимостьУслуг = errorСуммаСтоимостьУслуг;
                    //    }

                    //    //запишем в реестр список содержащий услуги в которых обнаружены расхождения
                    //    errorReestr.ErrorListУслуги = listError;

                    //    if (errorРеестр == true)
                    //    {
                    //        //запишем в список с ошибками текущего льготника со всеми расхождениями
                    //        list.Add(errorReestr);
                    //    }

                    //    listControlReestr.Add(rControl);
                    //}
                    #endregion

                }
                //fstream.Close();

                //sr.Close();
               // string iTest = "test";

                if (errorРеестр == true)
                {
                    //Выведим ошибку 
                    //MessageBox.Show("Выведим информацию об ошибках");

                    List<ErrorReestr> listTest = list;

                    //Сформируем реестр договоров содержащий ошибки
                    PrintReestrError printReest = new PrintReestrError(list, льготнаяКатегория);
                    printReest.Print();
                    
                }
                else
                {
                    //Выведим информацию на экран в виде документа word
                    List<ReestrControl> lTest = listControlReestr;

                    //Распечатаем реестр содержащий проверенную информацию
                    PrintReestrControl printReestr = new PrintReestrControl(listControlReestr, fileName, суммаСтоимостьУслуг.ToString(), льготнаяКатегория);
                    printReestr.Print();

                    //Запишем в БД
                    //MessageBox.Show("Записываем в БД");
                    WriteBD writBD = new WriteBD(unload);
                    writBD.Write();
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

            //переменная конфигурации
            int iConfig;
            
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Открыть файл реестра";
            openFileDialog1.Filter = "|*.r";
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

                //Установим для нашей программы текущую директорию для корректного считывания пути к БД
                Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;


                ////Создадим список объектов для хранения результатов проверки проектов договоров
                //Dictionary<string, ValidateContract> listValContr = new Dictionary<string, ValidateContract>();

                //ValidateЭСРН эсрн = new ValidateЭСРН(unload,listValContr);
                //Dictionary<string, ValidateContract> validReestr = эсрн.Validate();

                //Выведим результат проверки на экран

                //Закроем диалоговое окно
                openFileDialog1.Dispose();

            }
            else
            {
                //если пользователь нажал кнопку отмена то выходим из поиска
                return;
            }


            ////Закроем диалоговое окно
            //openFileDialog1.Dispose();

            ////Установим для нашей программы текущую директорию для корректного считывания пути к БД
            //Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;


            //Запустим второй поток
            //Thread t = new Thread(Display);
            //t.Start();
            //t.Join();

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

            
            ////string querySelect = "select configSearch from ConfgSearch";

            ////SqlConnection con = new SqlConnection(ConnectDB.ConnectionString());
            ////SqlCommand com = new SqlCommand(querySelect,con);

            ////int iConfig = 0;

            ////con.Open();
            ////SqlDataReader read = com.ExecuteReader();
            ////while(read.Read())
            ////{
            ////   iConfig = Convert.ToInt32(read["configSearch"]);  
            ////}
            ////con.Close();

            //this.progressBar1.Value = 15;

            //Установим для нашей программы текущую директорию для корректного считывания пути к БД
            Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;


            //проверим льготников по базе данных в ЭСРН
            ValidateЭСРН эсрн = new ValidateЭСРН(unload, listValContr, iConfig);
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

        //private void Display()
        //{
        //    MessageBox.Show("Выполняется проверка");
        //}
    }
}