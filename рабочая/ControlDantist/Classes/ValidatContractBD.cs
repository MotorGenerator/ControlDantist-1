using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DantistLibrary;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Сравнивает услуги записанные в договоре с услугами записанными в контрольной базе данных
    /// </summary>
    class ValidatContractBD
    {
        private string льготнаяКатегория = string.Empty;
        private int idHosp;
        private string видУслуг = string.Empty;
        private decimal цена = 0m;
        private bool errorFlagВидУслуг = false;
        private bool errorFlagЦена = false;
        private decimal суммаСтоимостьУслуг = 0m;
        private decimal errorСуммаСтоимостьУслуг = 0m;
        private bool errorFlagСтоимостьУслуги = false;
        private bool errorРеестр = false;
        private string датаДоговора = string.Empty;
        private DataRow rowControlReestrАкт;
        private string номерДоговора = string.Empty;
        private string кодУслуги = string.Empty;

        
        private bool flag = false;

        /// <summary>
        /// Указываент что происходит первичная загрузка
        /// </summary>
        public bool FlagЗагрузка
        {
            get
            {
                return flag;
            }
            set
            {
                flag = value;
            }
        }

        public ValidatContractBD()
        {

        }

        /// <summary>
        /// Функция сравнивает виды услуг записанными в файле выгрузки с записями на сервере SQL
        /// </summary>
        /// <param name="unloads"></param>
        /// <param name="ValContracts"></param>
        public void Validate(Dictionary<string, Unload> unloads, Dictionary<string, ValidateContract> ValContracts)
        {
            //Присвоем полученные данные
            //tabЛьготник = таблицаЛьготник;
            //unload = unloads;
            //listValContr = ValContracts;

            //List<bool> 

            //Создадим словарь содержащий log
            Dictionary<string, List<Log>> dicList = new Dictionary<string, List<Log>>();

            //Теперь сравним наименование и стоимость услуг оказываемых льготнику с стоимостью и наименование м услуг записанных в контрольную БД
            foreach (Unload un in unloads.Values)
            {
                ////Создадим экземпляр объекта для хранения ошибочной информации
                //ErrorsReestrUnload error = new ErrorsReestrUnload();

                //Сбросим флаг перед итерацией
                errorРеестр = false;

                ErrorReestr errorReestr = new ErrorReestr();

                //Создадим экземпляр объекта для хранения строки реестра на оказанные услуги успешно прошедшие проверку
                ReestrControl rControl = new ReestrControl();

                //Получим ФИО льготника которого запишем в случае ошибки в реестр
                DataRow rowЛьготник = un.Льготник.Rows[0];

                string фамилия = rowЛьготник["Фамилия"].ToString();

                string имя = rowЛьготник["Имя"].ToString();
                string отчество = rowЛьготник["Отчество"].ToString();

                //Запишем в реестр ФИО текущего льготника
                errorReestr.ФИО = фамилия + " " + имя + " " + отчество;

                //Запишем ФИО льготника
                rControl.ФИО = фамилия + " " + имя + " " + отчество;

                //Запишем дату и номер договора на оказание услуг
                DataRow rowControlReestrДоговор = un.Договор.Rows[0];

                //Запишем номер поликлинники и номер договора
                номерДоговора = rowControlReestrДоговор["НомерДоговора"].ToString();

                //Запишем дату договора
                if (rowControlReestrДоговор["ДатаДоговора"] != DBNull.Value)
                {
                    датаДоговора = Convert.ToDateTime(rowControlReestrДоговор["ДатаДоговора"]).ToShortDateString();
                }

                //запишем дату и номер договора в реестр
                rControl.ДатаНомерДоговора = номерДоговора + " " + датаДоговора;

                //Запишем дату и номер акта оказанных услуг
                if (un.АктВыполненныхРабот.Rows.Count != 0)
                {
                    rowControlReestrАкт = un.АктВыполненныхРабот.Rows[0];
                }

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

                 //Создадим список содержащий log
                 List<Log> listLog = new List<Log>();

               //Подключимся к серверу

                using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
                {

                    //откроем соединение
                    con.Open();

                    //Выполним всё в единой транзакции
                    SqlTransaction transact = con.BeginTransaction();

                    //Получим id поликлинники
                    string queryIdHosp = "select top 1 id_поликлинника from dbo.Поликлинника where ИНН = '" + rowHosp["ИНН"].ToString() + "' order by id_поликлинника desc ";

                    SqlCommand com = new SqlCommand(queryIdHosp, con);
                    com.Transaction = transact;
                    SqlDataReader read = com.ExecuteReader();

                    while (read.Read())
                    {
                        idHosp = Convert.ToInt32(read["id_поликлинника"]);
                        //idHosp = 107;
                    }

                    read.Close();

                    if (номерДоговора == "1/8479")
                    {
                        string iTest = "Test";
                    }


                    //Получим список услуг по текущему договору
                    DataTable tabДоговор = un.УслугиПоДоговору;
                    foreach (DataRow rowDog in tabДоговор.Rows)
                    {

                        if (rowDog["НаименованиеУслуги"].ToString().Trim() == "Консультация врача стоматолога-ортопеда")
                        {
                            string asd = "";
                        }

                        //Создадим экземпляр объекта для хранения ошибочной информации для конкретного льготника
                        ErrorsReestrUnload error = new ErrorsReestrUnload();

                        //string linkText = "'%" + rowDog["НаименованиеУслуги"].ToString() + "%'"; с использование like
                        string linkText = "'" + rowDog["НаименованиеУслуги"].ToString().Trim() + "'";

                        // Заменим в номере по перечню пробел на пустой символ.
                        string код = "'" + rowDog["НомерПоПеречню"].ToString().Replace(" ",string.Empty) + "'";
                        
                        //проверим название вида услуг и стоимость которая указана в файле реестра
                        string queryViewServices = "select ВидУслуги,Цена,НомерПоПеречню from dbo.ВидУслуг " +
                                                  "where id_поликлинника = " + idHosp + " and LOWER(RTRIM(LTRIM(ВидУслуги))) = " + linkText.Trim().ToLower() + " and REPLACE(НомерПоПеречню,' ','') = " + код.Trim() + " ";

                        SqlCommand comViewServ = new SqlCommand(queryViewServices, con);
                        comViewServ.Transaction = transact;
                        SqlDataReader readViewServ = comViewServ.ExecuteReader();

                       
                        //Создадим экземпляр класса Log
                        Log log = new Log();

                        //Получим название услуги и стоимость которая находится у нас на сервере
                        while (readViewServ.Read())
                        {
                            //вид услуги на сервере
                            видУслуг = readViewServ["ВидУслуги"].ToString().Trim();

                            //цена услуги на сервере
                            цена = Convert.ToDecimal(readViewServ["Цена"]);

                            //if (readViewServ["НомерПоПеречню"].ToString().Trim() == "7.3.1.18")
                            //{
                                кодУслуги = readViewServ["НомерПоПеречню"].ToString().Replace(" ",string.Empty).Trim();
                            //}

                            //Зпишем в лог информацию хранящуюся у нас на сервере
                            log.НаименованиеSQL = видУслуг;
                            log.ЦенаSQL = цена.ToString("c");
                        }

                        //закроем DataReader
                        readViewServ.Close();

                        //теперь сравним что лежит в базе и что лежит в файле реестра
                        if (rowDog["НаименованиеУслуги"].ToString().Trim().ToLower() == видУслуг.Trim().ToLower() && rowDog["НомерПоПеречню"].ToString().Replace(" ",string.Empty).Trim() == кодУслуги.Replace(" ",string.Empty).Trim())
                        {
                            error.ErrorНаименованиеУслуги = rowDog["НаименованиеУслуги"].ToString().Trim();
                            //error.НаименованиеУслуги = rowDog["НаименованиеУслуги"].ToString().Trim();

                            log.НаименованиеФайл = rowDog["НаименованиеУслуги"].ToString().Trim();

                            //ошибки нет
                            errorFlagВидУслуг = false;

                        }
                        else
                        {
                            //ошибка
                            errorFlagВидУслуг = true;

                            //запишем правильное наименование
                            error.НаименованиеУслуги = видУслуг.Trim();

                            //запишем ошибку
                            error.ErrorНаименованиеУслуги = rowDog["НаименованиеУслуги"].ToString().Trim();

                            //Запишем в лог
                            log.НаименованиеError = error.ErrorНаименованиеУслуги;
                            
                        }

                        //теперь сравним стоимость услуги
                        if (Convert.ToDecimal(rowDog["Цена"]) == цена && rowDog["НомерПоПеречню"].ToString().Replace(" ", string.Empty).Trim() == кодУслуги.Trim().Replace(" ", string.Empty))
                        {
                            //test
                            decimal iTest = Convert.ToDecimal(rowDog["Цена"]);

                            //ошибки нет
                            errorFlagЦена = false;

                            log.ЦенаФайл = Convert.ToDecimal(rowDog["Цена"]).ToString("c");
                        }
                        else
                        {
                            //ошибка
                            errorFlagЦена = true;

                            if (видУслуг != null)
                            {
                                //Запишем наименование услуги 
                                error.НаименованиеУслуги = rowDog["НаименованиеУслуги"].ToString().Trim(); //видУслуг.Trim();

                                //запишем разницу
                                error.Цена = цена;
                                error.ErrorЦена = Convert.ToDecimal(rowDog["Цена"]);

                                log.ЦенаError = цена.ToString("c");
                            }

                            //Запишем в ЛОГ

                        }

                        //Запишем в список listLog данные по наименованию услуги и её цене
                        listLog.Add(log);

                        //теперь проверим правильно ли вычеслина сумма оказанных услуг по данному виду работы
                        int количество = Convert.ToInt32(rowDog["Количество"]);

                        //подсчитаем контрольную сумму стоимости услуг
                        decimal сумма = Math.Round((Math.Round(цена, 2) * количество), 2);

                        //Подсчитаем итоговую сумму услуг для конкретного льготника
                        суммаСтоимостьУслуг = Math.Round((суммаСтоимостьУслуг + сумма), 2);

                        //подсчитаем сумму в файле выгрузки для конкретного реестра
                        errorСуммаСтоимостьУслуг = Math.Round((errorСуммаСтоимостьУслуг + Convert.ToDecimal(rowDog["Сумма"])), 2);

                        //сравим сумму по услуге
                        if (Convert.ToDecimal(rowDog["Сумма"]) == сумма)
                        {
                            //test
                            decimal iTest = Convert.ToDecimal(rowDog["Сумма"]);

                            //ошибки нет
                            errorFlagСтоимостьУслуги = false;
                        }
                        else
                        {
                            //ошибка
                            errorFlagСтоимостьУслуги = true;

                            //Запишем наименование услуги 
                            error.НаименованиеУслуги = видУслуг.Trim();

                            //запишем разницу
                            error.Сумма = сумма;
                            error.ErrorСумма = Convert.ToDecimal(rowDog["Сумма"]);
                        }

                        //Сравним результат который у нас получился
                        if (errorFlagВидУслуг == false && errorFlagЦена == false && errorFlagСтоимостьУслуги == false)
                        {
                            if (ValContracts.Count != 0)//Льготник найден
                            {
                                try
                                {
                                    //ошибки в данном виде услуг не произошло
                                    ValContracts[номерДоговора.Trim()].flagErrorSumm = true;
                                }
                                catch
                                {
                                    //Если возникло исключение значит ключ в данном словре не найден значит нет такого льготника в ЭСРН 
                                    ValidateContract vc = new ValidateContract();
                                    vc.FlagPersonЭСРН = false;//указываем что льготник в ЭСРН не найден
                                    vc.flagErrorSumm = true;//

                                    //Запишем список расхождений льготных услуг с базой на сервре
                                    //vc.СписокРасхождений = listError;
                                    ValContracts.Add(номерДоговора.Trim(), vc);
                                }
                            }
                            else
                            {
                                ValidateContract vc = new ValidateContract();
                                vc.FlagPersonЭСРН = false;//указываем что льготник в ЭСРН не найден
                                vc.flagErrorSumm = true;//

                                //Запишем список расхождений льготных услуг с базой на сервре
                                //vc.СписокРасхождений = listError;
                                ValContracts.Add(номерДоговора.Trim(), vc);
                            }
                        }
                        else
                        {
                            //произошла ошибка и мы выставим флаг реестра в ошибку 
                            errorРеестр = true;

                            //запишем результат проверки текущей строки в список ошибки
                            listError.Add(error);

                            if (ValContracts.ContainsKey(номерДоговора.Trim()))
                            {
                                ValContracts[номерДоговора.Trim()].flagErrorSumm = false;
                            }


                            ////установим флаг данного договора в ошибку - false
                            //ValContracts[номерДоговора.Trim()].flagErrorSumm = false;
                            //ValContracts[номерДоговора.Trim()].СписокРасхождений = listError;
                        }

                        //Обнулим переменные для хранения информации 
                        видУслуг = string.Empty;
                        цена = 0m;
                    }

                    //узнаем про флаг ошибки
                    bool iTestError = errorРеестр;

                    //Вот гдето сдесь происходит ошибка надо понять почему флаг errorРеестр стал = true
                    if (errorРеестр == true && ValContracts.Count != 0)//Льготник в ЭСРН есть
                    {
                        //Если ошибка значит добавим ValContracts с текущим номером договора
                       // try
                        //{
                            //установим флаг данного договора в ошибку - false
                            ValContracts[номерДоговора.Trim()].flagErrorSumm = false;

                            //Запишем список расхождений льготных услуг с базой на сервре
                            ValContracts[номерДоговора.Trim()].СписокРасхождений = listError;
                       // }
                       // catch (KeyNotFoundException  e)
                       //{
                       //    ValidateContract vc = new ValidateContract();
                       //    vc.FlagPersonЭСРН = false;
                       //    vc.flagErrorSumm = false;

                       //    //Запишем список расхождений льготных услуг с базой на сервре
                       //    vc.СписокРасхождений = listError;
                       //    ValContracts.Add(номерДоговора.Trim(), vc);
                       //}
                    }

                    //если льготника в ЭСРН нет
                    if (errorРеестр == true && ValContracts.Count == 0)
                    {
                        ValidateContract vc = new ValidateContract();
                        vc.FlagPersonЭСРН = false;
                        vc.flagErrorSumm = false;

                        //Запишем список расхождений льготных услуг с базой на сервре
                        vc.СписокРасхождений = listError;
                        ValContracts.Add(номерДоговора.Trim(), vc);
                    }

                    //if (errorРеестр == false)// && ValContracts.Count == 0)
                    //{
                    //    //установим флаг данного договора в ошибку - false
                    //    ValContracts[номерДоговора.Trim()].flagErrorSumm = true;
                    //}
                    

                    //Что то сдесь не так
                }

                //if (this.FlagЗагрузка == false)
                //{
                //    dicList.Add(номерДоговора, listLog);
                //}
                //else
                //{
                //    номерДоговора
                //}


            }

            //Dictionary<string, List<Log>> iTest2 = dicList;
        }

        /// <summary>
        /// Сравнивает записи в контрольной базе и в коллекции классов
        /// </summary>
        /// <param name="unloads"></param>
        /// <param name="ValContracts"></param>
        //public void Validate(Dictionary<string, Unload> unloads, Dictionary<string, ValidateContract> ValContracts)
        //{
        //    //Присвоем полученные данные
        //    //tabЛьготник = таблицаЛьготник;
        //    //unload = unloads;
        //    //listValContr = ValContracts;

        //    //List<bool> 

        //    //Теперь сравним наименование и стоимость услуг оказываемых льготнику с стоимостью и наименование м услуг записанных в контрольную БД
        //    foreach (Unload un in unloads.Values)
        //    {
        //        ////Создадим экземпляр объекта для хранения ошибочной информации
        //        //ErrorsReestrUnload error = new ErrorsReestrUnload();

        //        ErrorReestr errorReestr = new ErrorReestr();

        //        //Создадим экземпляр объекта для хранения строки реестра на оказанные услуги успешно прошедшие проверку
        //        ReestrControl rControl = new ReestrControl();

        //        //Получим ФИО льготника которого запишем в случае ошибки в реестр
        //        DataRow rowЛьготник = un.Льготник.Rows[0];

        //        string фамилия = rowЛьготник["Фамилия"].ToString();
        //        string имя = rowЛьготник["Имя"].ToString();
        //        string отчество = rowЛьготник["Отчество"].ToString();

        //        //Запишем в реестр ФИО текущего льготника
        //        errorReestr.ФИО = фамилия + " " + имя + " " + отчество;

        //        //Запишем ФИО льготника
        //        rControl.ФИО = фамилия + " " + имя + " " + отчество;

        //        //Запишем дату и номер договора на оказание услуг
        //        DataRow rowControlReestrДоговор = un.Договор.Rows[0];

        //        //Запишем номер поликлинники и номер договора
        //        номерДоговора = rowControlReestrДоговор["НомерДоговора"].ToString();

        //        //Запишем дату договора
        //        if (rowControlReestrДоговор["ДатаДоговора"] != DBNull.Value)
        //        {
        //            датаДоговора = Convert.ToDateTime(rowControlReestrДоговор["ДатаДоговора"]).ToShortDateString();
        //        }

        //        //запишем дату и номер договора в реестр
        //        rControl.ДатаНомерДоговора = номерДоговора + " " + датаДоговора;

        //        //Запишем дату и номер акта оказанных услуг
        //        if (un.АктВыполненныхРабот.Rows.Count != 0)
        //        {
        //            rowControlReestrАкт = un.АктВыполненныхРабот.Rows[0];
        //        }

        //        //Получим номер акта 
        //        //string номерАкта = rowControlReestrАкт["НомерАкта"].ToString();

        //        //Запишем дату акта оказанных услуг
        //        //string датаАкта = Convert.ToDateTime(rowControlReestrАкт["ДатаПодписания"]).ToShortDateString();

        //        //Запишем в реестр номер и дату акта оказанных услуг 
        //        //rControl.НомерАктаОказанныхУслуг = номерАкта + " " + датаАкта;

        //        //Получим серию и номер документа о праве на льготу
        //        DataRow rowПравоЛьготы = un.Льготник.Rows[0];

        //        //Серия документа
        //        string серия = rowПравоЛьготы["СерияДокумента"].ToString();

        //        //Запишем номер поликлинники и номер договора
        //        string номерДокумента = rowПравоЛьготы["НомерДокумента"].ToString();

        //        //Запишем дату договора
        //        string датаДокумента = Convert.ToDateTime(rowПравоЛьготы["ДатаВыдачиДокумента"]).ToShortDateString();

        //        rControl.ДокументЛьгота = серия + " " + номерДокумента + " " + датаДокумента;

        //        //Запишем название льготной категории
        //        льготнаяКатегория = un.ЛьготнаяКатегория;


        //        //Создадим список который содержит расхождения в реестре
        //        List<ErrorsReestrUnload> listError = new List<ErrorsReestrUnload>();

        //        //Узнаем какой поликлиннике принадлежит файл реестра
        //        DataRow rowHosp = un.Поликлинника.Rows[0];

        //        //Подключимся к серверу
                
        //        using(SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
        //        {

        //            //откроем соединение
        //            con.Open();

        //            //Выполним всё в единой транзакции
        //            SqlTransaction transact = con.BeginTransaction();

        //        //Получим id поликлинники
        //        string queryIdHosp = "select id_поликлинника from dbo.Поликлинника where ИНН = " + rowHosp["ИНН"].ToString() + " ";

        //        SqlCommand com = new SqlCommand(queryIdHosp, con);
        //        com.Transaction = transact;
        //        SqlDataReader read = com.ExecuteReader();

        //        while (read.Read())
        //        {
        //            idHosp = Convert.ToInt32(read["id_поликлинника"]);
        //        }

        //        read.Close();

        //        //Получим список услуг по текущему договору
        //        DataTable tabДоговор = un.УслугиПоДоговору;
        //        foreach (DataRow rowDog in tabДоговор.Rows)
        //        {
        //            //Создадим экземпляр объекта для хранения ошибочной информации для конкретного льготника
        //            ErrorsReestrUnload error = new ErrorsReestrUnload();

        //            //string linkText = "'%" + rowDog["НаименованиеУслуги"].ToString() + "%'"; с использование like
        //            string linkText = "'" + rowDog["НаименованиеУслуги"].ToString() + "'";
        //            //проверим название вида услуг и стоимость которая указана в файле реестра
        //            string queryViewServices = "select ВидУслуги,Цена from dbo.ВидУслуг " +
        //                                       "where id_поликлинника = " + idHosp + " and ВидУслуги = " + linkText + " ";

        //            SqlCommand comViewServ = new SqlCommand(queryViewServices, con);
        //            comViewServ.Transaction = transact;
        //            SqlDataReader readViewServ = comViewServ.ExecuteReader();

        //            //Получим название услуги и стоимость которая находится у нас на сервере
        //            while (readViewServ.Read())
        //            {
        //                //вид услуги на сервере
        //                видУслуг = readViewServ["ВидУслуги"].ToString().Trim();

        //                //цена услуги на сервере
        //                цена = Convert.ToDecimal(readViewServ["Цена"]);
        //            }

        //            //закроем DataReader
        //            readViewServ.Close();

        //            //теперь сравним что лежит в базе и что лежит в файле реестра
        //            if (rowDog["НаименованиеУслуги"].ToString().Trim() == видУслуг.Trim())
        //            {
        //                error.ErrorНаименованиеУслуги = rowDog["НаименованиеУслуги"].ToString().Trim();


        //                //ошибки нет
        //                errorFlagВидУслуг = false;

        //            }
        //            else
        //            {
        //                //ошибка
        //                errorFlagВидУслуг = true;

        //                //запишем правильное наименование
        //                error.НаименованиеУслуги = видУслуг.Trim();

        //                //запишем ошибку
        //                error.ErrorНаименованиеУслуги = rowDog["НаименованиеУслуги"].ToString().Trim();

        //            }

        //            //теперь сравним стоимость услуги
        //            if (Convert.ToDecimal(rowDog["Цена"]) == цена)
        //            {
        //                //test
        //                decimal iTest = Convert.ToDecimal(rowDog["Цена"]);

        //                //ошибки нет
        //                errorFlagЦена = false;
        //            }
        //            else
        //            {
        //                //ошибка
        //                errorFlagЦена = true;

        //                if (видУслуг != null)
        //                {
        //                    //Запишем наименование услуги 
        //                    error.НаименованиеУслуги = rowDog["НаименованиеУслуги"].ToString().Trim(); //видУслуг.Trim();

        //                    //запишем разницу
        //                    error.Цена = цена;
        //                    error.ErrorЦена = Convert.ToDecimal(rowDog["Цена"]);
        //                }
        //            }

        //            //теперь проверим правильно ли вычеслина сумма оказанных услуг по данному виду работы
        //            int количество = Convert.ToInt32(rowDog["Количество"]);

        //            //подсчитаем контрольную сумму стоимости услуг
        //            decimal сумма = Math.Round((Math.Round(цена, 2) * количество), 2);

        //            //Подсчитаем итоговую сумму услуг для конкретного льготника
        //            суммаСтоимостьУслуг = Math.Round((суммаСтоимостьУслуг + сумма), 2);

        //            //подсчитаем сумму в файле выгрузки для конкретного реестра
        //            errorСуммаСтоимостьУслуг = Math.Round((errorСуммаСтоимостьУслуг + Convert.ToDecimal(rowDog["Сумма"])), 2);

        //            //сравим сумму по услуге
        //            if (Convert.ToDecimal(rowDog["Сумма"]) == сумма)
        //            {
        //                //test
        //                decimal iTest = Convert.ToDecimal(rowDog["Сумма"]);

        //                //ошибки нет
        //                errorFlagСтоимостьУслуги = false;
        //            }
        //            else
        //            {
        //                //ошибка
        //                errorFlagСтоимостьУслуги = true;

        //                //Запишем наименование услуги 
        //                error.НаименованиеУслуги = видУслуг.Trim();

        //                //запишем разницу
        //                error.Сумма = сумма;
        //                error.ErrorСумма = Convert.ToDecimal(rowDog["Сумма"]);
        //            }

        //            //Сравним результат который у нас получился
        //            if (errorFlagВидУслуг == false && errorFlagЦена == false && errorFlagСтоимостьУслуги == false)
        //            {
        //                if (ValContracts.Count != 0)//Льготник найден
        //                {
        //                    try
        //                    {
        //                        //ошибки в данном виде услуг не произошло
        //                        ValContracts[номерДоговора.Trim()].flagErrorSumm = true;
        //                    }
        //                    catch
        //                    {
        //                        //Если возникло исключение значит ключ в данном словре не найден значит нет такого льготника в ЭСРН 
        //                        ValidateContract vc = new ValidateContract();
        //                        vc.FlagPersonЭСРН = false;//указываем что льготник в ЭСРН не найден
        //                        vc.flagErrorSumm = true;//

        //                        //Запишем список расхождений льготных услуг с базой на сервре
        //                        //vc.СписокРасхождений = listError;
        //                        ValContracts.Add(номерДоговора.Trim(), vc);
        //                    }
        //                }
        //                else
        //                {
        //                    ValidateContract vc = new ValidateContract();
        //                    vc.FlagPersonЭСРН = false;//указываем что льготник в ЭСРН не найден
        //                    vc.flagErrorSumm = true;//

        //                    //Запишем список расхождений льготных услуг с базой на сервре
        //                    //vc.СписокРасхождений = listError;
        //                    ValContracts.Add(номерДоговора.Trim(), vc);
        //                }
        //            }
        //            else
        //            {
        //                //произошла ошибка и мы выставим флаг реестра в ошибку 
        //                errorРеестр = true;

        //                //запишем результат проверки текущей строки в список ошибки
        //                listError.Add(error);

        //                ////установим флаг данного договора в ошибку - false
        //                //ValContracts[номерДоговора.Trim()].flagErrorSumm = false;
        //                //ValContracts[номерДоговора.Trim()].СписокРасхождений = listError;
        //            }

        //            //Обнулим переменные для хранения информации 
        //            видУслуг = string.Empty;
        //            цена = 0m;


        //        }

        //        if (errorРеестр == true && ValContracts.Count != 0)
        //        {
        //            //установим флаг данного договора в ошибку - false
        //            ValContracts[номерДоговора.Trim()].flagErrorSumm = false;

        //            //Запишем список расхождений льготных услуг с базой на сервре
        //            ValContracts[номерДоговора.Trim()].СписокРасхождений = listError;
        //        }

        //            //если льготника в ЭСРН нет
        //        if (errorРеестр == true && ValContracts.Count == 0)
        //        {
        //            ValidateContract vc = new ValidateContract();
        //            vc.FlagPersonЭСРН = false;
        //            vc.flagErrorSumm = false;

        //            //Запишем список расхождений льготных услуг с базой на сервре
        //            vc.СписокРасхождений = listError;
        //            ValContracts.Add(номерДоговора.Trim(), vc);
        //        }

        //            //if(errorРеестр == false && ValContracts.Count == 0)


        //        }
        //    }

        //}
    }
}
