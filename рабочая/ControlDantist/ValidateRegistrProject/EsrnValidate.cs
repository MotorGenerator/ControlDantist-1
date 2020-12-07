using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Classes;
using System.Data;
using System.Data.SqlClient;
using ControlDantist.ClassValidRegions;
using ControlDantist.Repository;
using System.Configuration;
using ControlDantist.ValidateMedicalServices;
using System.Net;
using System.Net.NetworkInformation;

namespace ControlDantist.ValidateRegistrProject
{
    public class EsrnValidate
    {
        private IEnumerable<ViewЛьготникДоговорРеестр> contracts;

        // Переменная для хранения льготной категории.
        private string льготнаяКатегория = string.Empty;

        // Метка льготной категории.
        private string stringLk = string.Empty;

        /// <summary>
        /// Флаг включены сервера или нет.
        /// </summary>
        public bool FlagConnectServer { get; set; }

        public EsrnValidate(IEnumerable<ViewЛьготникДоговорРеестр> договоры, string льготнаяКатегория)
        {
            if(договоры.Count() > 0)
            contracts = договоры;

            this.льготнаяКатегория = льготнаяКатегория;

        }

        public Dictionary<string, PersonValidEsrn> ValidateList()
        {

            //Переменная указывает что цикл запустился в первый раз
            int iFirst = 0;

            //////Пройдёмся по пулу строк подключения
            //PullConnectBD pull = new PullConnectBD();

            ////// Словарь со строками подключения к АИС ЭСРН.
            //Dictionary<string, string> pullConnect = pull.GetPull(FlagConnectServer);

            // Строки подключения к БД АИС ЭСРН.
            ConfigLibrary.Config config = new ConfigLibrary.Config();

            //Пока закоментирован.
             //Получаем словарь со строками подключения к АИС ЭСРН.
            Dictionary<string, string> pullConnect = config.Select();

            //Получим список льготников которые необходимо проверить по ЭСРН.
            List<PersonValidEsrn> listPersonValidEsrn = this.CreateList();

            //// Для теста.
            //Dictionary<string, string> pullConnect = new Dictionary<string, string>();

            ////pullConnect.Add("Кировский", "Data Source = 10.159.103.242; Initial Catalog = esrn_kir; User ID = kom; Password = kom1234sql; ");
            //pullConnect.Add("Заводской", "Data Source = 10.159.106.8; Initial Catalog = esrn_zav; User ID = sa; Password = sitex; ");

            //// Для теста.
            //List<PersonValidEsrn> listPersonValidEsrn = new List<PersonValidEsrn>();

            //PersonValidEsrn p = new PersonValidEsrn();
            //p.flagValidEsrn = false;
            //p.flagValidMedicalServices = false;
            //p.FlagValidPersonFioEsrn = false;
            //p.FlagValidPersonPassword = false;
            //p.IdContract = 0;
            //p.Адрес = null;
            //p.датаВыдачиДокумента = "22.05.2010";
            //p.датаВыдачиПаспорта = "16.10.2002";
            //p.датаРождения = "14.09.1954";
            //p.имя = "Татьяна";
            //p.номерДоговора = "3-1/6259";
            //p.номерДокумента = "026408";
            //p.номерПаспорта = "283657";
            //p.отчество = "Александровна";
            //p.серияДокумента = "64";
            //p.серияПаспорта = "6303";
            //p.фамилия = "Семенова";

            //listPersonValidEsrn.Add(p);

            // Конец теста.


            #region 18.08.2020 ТЕСТ включить
            foreach (KeyValuePair<string, string> dStringConnect in pullConnect)
            {

                string sConnection = string.Empty;
                sConnection = dStringConnect.Value.ToString().Trim();


                bool isConnected = false;

                //Выполним проверку в единой транзакции для конкретного района (строки подключения)
                using (SqlConnection con = new SqlConnection(sConnection))
                {
                    try
                    {
                        Ping pingSender = new Ping();
                        PingReply reply = pingSender.Send("10.159.104.5");

                        if (reply.Status != IPStatus.Success)
                        {
                            System.Windows.Forms.MessageBox.Show("Сервер в районе " + dStringConnect.Key + " в настоящий момент не доступен.");
                            continue;
                        }


                        //Подключимся к текущему серверу
                        con.Open();

                    }
                    catch
                    {
                        isConnected = false;
                        //t.Abort();
                        //Если сервре не доступен 
                        System.Windows.Forms.MessageBox.Show("Сервер в районе " + dStringConnect.Key + " в настоящий момент не доступен.");
                        continue;
                    }


                    //// Получим список льготников которые необходимо проверить по ЭСРН.
                    //List<PersonValidEsrn> listPersonValidEsrn = this.CreateList();

                    // Льготная категория льготников прописанных в текущем реестре.
                    this.PreferenceCategory(this.льготнаяКатегория);

                    // Запрос на поиск льготников по ЭСРН по фио, дате рождения номеру и дате выдачи документа.
                    string queryФИО = QueryValidate(listPersonValidEsrn);

                    SqlTransaction sqlTransaction = con.BeginTransaction();

                    // Получим льготников найденных в ЭСРН по документам дающим право на получение льгот.
                    DataTable tabФИО = ТаблицаБД.GetTableSQL(queryФИО, "ФИО", con, sqlTransaction);

                    // Пометим всех льготников флагом как прошедших проверку по ФИО.
                    SetFLagFio(tabФИО, listPersonValidEsrn);

                    // Проверка льготников по ФИО дате рождения, а так же по паспорту.
                    string queryFio = QueryValidatePassword(listPersonValidEsrn);

                    DataTable tabPassFio = ТаблицаБД.GetTableSQL(queryFio, "ФиоPassword", con, sqlTransaction);

                    // Пометим всех льготников флагом как прошедших проверку по Паспорту.
                    SetFLagPassword(tabPassFio, listPersonValidEsrn);

                    // Проверим есть ли 
                    if (tabФИО.Rows.Count != 0 && tabPassFio.Rows.Count != 0)
                    {

                        // Получим список льготников которые прошли проверку и по ФИО и льготным документам, а так же по паспорту.
                        var listJoin = listPersonValidEsrn.Where(w => w.FlagValidPersonFioEsrn == true && w.FlagValidPersonPassword == true && w.flagValidEsrn == false);

                        // Установим что льготник прошел проверку в ЭСРН.
                        foreach (var itm in listJoin)
                        {

                            DataRow[] rows = tabPassFio.Select("id_договор = " + itm.IdContract + " ");

                            if (rows.Count() > 0)
                            {
                                if (rows[0]["A_ADRTITLE"] != DBNull.Value)
                                {
                                    itm.Адрес = rows[0]["A_ADRTITLE"].ToString();

                                    itm.flagValidEsrn = true;
                                }
                                else
                                {
                                    itm.Адрес = "";

                                    itm.flagValidEsrn = false;
                                }
                            }
                            else
                            {
                                itm.Адрес = "";

                                itm.flagValidEsrn = false;
                            }


                        }
                    }

                    //System.Windows.Forms.MessageBox.Show("Проход по " + dStringConnect.Key + " завершен");
                }

                System.Windows.Forms.Application.DoEvents();// Application.DoEvents();

            }
            #endregion

            // Сформируем для совместимости библиотеку с проектами договоров.
            Dictionary<string, PersonValidEsrn> dictionary = new Dictionary<string, PersonValidEsrn>();

            ValidateServicesMedical vsm = new ValidateServicesMedical(listPersonValidEsrn);

            foreach(PersonValidEsrn itm in listPersonValidEsrn)//.Where(w=>w.flagValidEsrn == true))
            {
                #region 18.08.2020 Для теста
                // INNER JOIN Услуги на сервере и договоре.
                string itmServ = vsm.ValidateServer(itm.номерДоговора.Trim());

                // Услуги в договоре.
                string itmContract = vsm.ValidateContract(itm.номерДоговора.Trim());

                // Таблица с услугами которые совпали с сервером и с договором.
                DataTable tServ = ТаблицаБД.GetTableSQL(itmServ, "УслугиСервер");

                // Услуги которые есть у договора.
                DataTable tContr = ТаблицаБД.GetTableSQL(itmContract, "ДоговораСервер");

                var asd = tServ.Rows.Count;

                var asd2 = tContr.Rows.Count;

                // Проверим расхождение в количество услуг.
                if (tServ.Rows.Count == tContr.Rows.Count)
                {
                    // Если расхождений нет, считаем что проверка прошла.
                    itm.flagValidMedicalServices = true;
                }
                #endregion

                //// Для теста проставим флаги проверки.
                //itm.flagValidMedicalServices = true;
                //itm.flagValidEsrn = true;

                dictionary.Add(itm.номерДоговора, itm);

            }
                    return dictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabФИО"></param>
        /// <param name="listPersonValidEsrn"></param>
        private void SetFLagFio(DataTable tabФИО, List<PersonValidEsrn> listPersonValidEsrn)
        {
            foreach(DataRow row in tabФИО.Rows)
            {
                var listPersonVE = listPersonValidEsrn.Where(w => w.IdContract == Convert.ToInt32(row["id_договор"])).OrderByDescending(w=>w.IdContract).FirstOrDefault();

                if(listPersonVE != null)
                {
                    listPersonVE.FlagValidPersonFioEsrn = true;
                }
            }
        }


        private void SetFLagPassword(DataTable tabФИО, List<PersonValidEsrn> listPersonValidEsrn)
        {
            foreach (DataRow row in tabФИО.Rows)
            {
                var listPersonVE = listPersonValidEsrn.Where(w => w.IdContract == Convert.ToInt32(row["id_договор"])).OrderByDescending(w=>w.IdContract).FirstOrDefault();

                if (listPersonVE != null)
                {
                    listPersonVE.FlagValidPersonPassword = true;
                }
            }
        }

        //private string QueryValidatePreferency()
        //{

        //}


        /// <summary>
        /// Проверка льготника в ЭСР по паспорту.
        /// </summary>
        /// <param name="listPersonValidEsrn"></param>
        /// <returns></returns>
        private string QueryValidatePassword(List<PersonValidEsrn> listPersonValidEsrn)
        {

            // Создадим экземпляр класса.
            ValidatePrefCategoryList validCategor = new ValidatePrefCategoryList(listPersonValidEsrn);

            //string 
            return validCategor.PasswordCheck();
        }

        /// <summary>
        /// Генерируем запрос на поиск льготников в ЭСРН.
        /// </summary>
        /// <param name="listPersonValidEsrn"></param>
        /// <returns></returns>
        private string QueryValidate(List<PersonValidEsrn> listPersonValidEsrn)
        {
            string queryФИО = string.Empty;

            // Создадим экземпляр класса.
            ValidatePrefCategoryList validCategor = new ValidatePrefCategoryList(listPersonValidEsrn);

            // Проверим льготника по льготной категории.
            if (stringLk.ToLower().Trim().Replace(" ", string.Empty) == "Ветеранвоеннойслужбы".ToLower().Trim())
            {
                string docPreferencyCategory = " PPR_DOC.A_NAME in ('Удостоверение ветерана военной службы')";

                queryФИО = validCategor.Validate(docPreferencyCategory);
            }

            // Проверим льготника по льготной категории.
            if (stringLk.ToLower().Trim().Replace(" ", string.Empty) == "Ветерантруда".ToLower().Trim())
            {
                string docPreferencyCategory = " PPR_DOC.A_NAME in ('Удостоверение ветерана труда') ";
                queryФИО = validCategor.Validate(docPreferencyCategory);
            }

            // Проверим льготника по льготной категории.
            if (stringLk.ToLower().Trim().Replace(" ", string.Empty) == "Тружениктыла".ToLower().Trim())
            {
                string docPreferencyCategory = " PPR_DOC.A_NAME in ('Удостоверение о праве на льготы (отметка - ст.20)','Удостоверение ветерана ВОВ (отметка - ст.20)') ";
                queryФИО = validCategor.Validate(docPreferencyCategory);
            }

            // Проверим льготника по льготной категории.
            if (stringLk.ToLower().Trim().Replace(" ", string.Empty) == "ВетерантрудаСаратовскойобласти".ToLower().Trim())
            {
                string docPreferencyCategory = " PPR_DOC.A_NAME in ('Удостоверение ветерана труда Саратовской области') ";
                queryФИО = validCategor.Validate(docPreferencyCategory);
            }

            // Проверим льготника по льготной категории.
            if (stringLk.ToLower().Trim().Replace(" ", string.Empty) == "Реабилитированныелица".ToLower().Trim())
            {
                string docPreferencyCategory = " PPR_DOC.A_NAME in ('Свидетельство о праве на льготы для реабилитированных лиц','Справка о реабилитации' ) ";//,'Свидетельство о праве на льготы для лиц, признанных пострадавшими от политических репрессий','Справка о признании пострадавшим от политических репрессий') ";
                queryФИО = validCategor.Validate(docPreferencyCategory);
            }

            return queryФИО;
        }

        /// <summary>
        /// Возвращает льготную категорию.
        /// </summary>
        /// <param name="льготнаяКатегория"></param>
        /// <returns></returns>
        private string PreferenceCategory(string льготнаяКатегория)
        {
            // Проверим льготника по льготной категории.
            if (льготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty) == "Ветеранвоеннойслужбы".ToLower().Trim())
            {
                stringLk = "Ветеранвоеннойслужбы";
            }

            // Проверим льготника по льготной категории.
            if (льготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty) == "Ветерантруда".ToLower().Trim())
            {
                stringLk = "Ветерантруда";
            }

            // Проверим льготника по льготной категории.
            if (льготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty) == "Тружениктыла".ToLower().Trim())
            {
                stringLk = "Тружениктыла";
            }

            // Проверим льготника по льготной категории.
            if (льготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty) == "ВетерантрудаСаратовскойобласти".ToLower().Trim())
            {
                stringLk = "ВетерантрудаСаратовскойобласти";
            }

            // Проверим льготника по льготной категории.
            if (льготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty) == "Реабилитированныелица".ToLower().Trim())
            {
                stringLk = "Реабилитированныелица";
            }

            return stringLk;
        }

        /// <summary>
        /// Генерирует список льготников.
        /// </summary>
        /// <returns></returns>
        private List<PersonValidEsrn> CreateList()
        {
            List<PersonValidEsrn> listPersonValidEsrn = new List<PersonValidEsrn>();

            foreach (ViewЛьготникДоговорРеестр un in contracts)
            {
                //Создадим экземпляр класса с результатом проверки
                ValidateContract vContract = new ValidateContract();

                PersonValidEsrn personValidEsrn = new PersonValidEsrn();

                personValidEsrn.IdContract = un.id_договор;

                //получим номер договора
                //DataRow rDog = un.Договор.Rows[0];
                personValidEsrn.номерДоговора = un.НомерДоговора;// rDog["НомерДоговора"].ToString().Trim();

                //Получим строку с льготником
                //DataRow rowLgot = un.Льготник.Rows[0];
                personValidEsrn.фамилия = un.Фамилия;// rowLgot["Фамилия"].ToString().Replace('Ё', 'Е').Replace('ё', 'е').Trim();
                personValidEsrn.имя = un.Имя;// rowLgot["Имя"].ToString().Replace('Ё', 'Е').Replace('ё', 'е').Trim();

                personValidEsrn.отчество = un.Отчество; //rowLgot["Отчество"].ToString().Replace('Ё', 'Е').Replace('ё', 'е').Trim();
                personValidEsrn.датаРождения = un.ДатаРождения.Value.ToShortDateString();// Convert.ToDateTime(rowLgot["ДатаРождения"]).ToShortDateString().Trim();

                //Хранит серию паспорта в формате 00 00 
                string serPass = string.Empty;

                //string серПаспорт = rowLgot["СерияПаспорта"].ToString().Trim();
                personValidEsrn.серияПаспорта = un.СерияПаспорта;// rowLgot["СерияПаспорта"].ToString().Trim();

                //Установим формат серии паспорта в формате 00 00
                StringBuilder build = new StringBuilder();

                //счётчик циклов
                int iiCount = 0;
                foreach (char ch in personValidEsrn.серияПаспорта)
                {
                    if (iiCount == 2)
                    {
                        string sNum = " " + ch.ToString().Trim();
                        build.Append(sNum);
                    }
                    else
                    {
                        build.Append(ch);
                    }


                    iiCount++;
                }

                //сохраним в переменную серию и номер паспорта в формате 00 00
                serPass = build.ToString().Trim();

                personValidEsrn.номерПаспорта = un.НомерПаспорта;// rowLgot["НомерПаспорта"].ToString().Trim();
                personValidEsrn.датаВыдачиПаспорта = un.ДатаВыдачиПаспорта.Value.ToShortDateString();// Convert.ToDateTime(rowLgot["ДатаВыдачиПаспорта"]).ToShortDateString().Trim();

                //серия, номер и дата выдачи документа дающего право на льготу
                personValidEsrn.серияДокумента = un.СерияДокумента;// rowLgot["СерияДокумента"].ToString().Trim();

                if (personValidEsrn.серияДокумента.Trim().ToLower() == "-".Trim().ToLower())
                {
                    personValidEsrn.серияДокумента = null;
                }

                personValidEsrn.номерДокумента = un.НомерДокумента;// rowLgot["НомерДокумента"].ToString().Trim();
                personValidEsrn.датаВыдачиДокумента = un.ДатаВыдачиДокумента.Value.ToShortDateString();// Convert.ToDateTime(rowLgot["ДатаВыдачиДокумента"]).ToShortDateString().Trim();

                listPersonValidEsrn.Add(personValidEsrn);
            }

            return listPersonValidEsrn;
        }
    }
}
