using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DantistLibrary;
using System.Threading;
using ControlDantist.ClassValidRegions;


namespace ControlDantist.Classes
{
    /// <summary>
    /// Осуществляет проверку данных в ЭСРН
    /// </summary>
    class ValidateЭСРН
    {
        private DataTable tabЛьготник;
        
        //Хранит выгруженный реестр проектов договоров
        Dictionary<string, Unload> unload;

        // Результат проверки.
        Dictionary<string, ValidateContract> listValContr;
        string queryФИО = string.Empty;

        ////хранит строку запроса по поиску 
        //string queryFIO = string.Empty;

        //хранит строку запроас на поиск граждаднина в ЭСРН по паспорту гражданина РФ
        string queryPassFio = string.Empty;

        //Хранит строку запрос на поиск гражданина по паспорту где серия ы формате 00 00
        string queryPassFIO = string.Empty;

        //хранит значение конфигурации поиска льготника в ЭСРН
        private int iconfig = 0;

        //Счётчик в котором отображаются не доступные сервера
        private int[] iList;

        DataTable tabPassFio;
        DataTable tabPassFIO;
        DataTable tableФИО;

        private bool flagЗагрузки = false;

        /// <summary>
        /// Флаг указывает подключать ся ли к серверам или нет.
        /// </summary>
        public bool FlagConnectServer { get; set; }

        /// <summary>
        /// Указывает что класс заргужает всю базу
        /// </summary>
        public bool FlagЗагрузки
        {
            get
            {
                return flagЗагрузки;
            }
            set
            {
                flagЗагрузки = value;
            }
        }

        public ValidateЭСРН(Dictionary<string, Unload> unloads, Dictionary<string, ValidateContract> ValContracts, int iConfig)
        {
            //Присвоем полученные данные
            //tabЛьготник = таблицаЛьготник;
            unload = unloads;
            listValContr = ValContracts;
            
            //передадим в класс номер конфигруации поиска льготника в ЭСРН
            iconfig = iConfig;
        }

        /// <summary>
        /// Проверяет данные в ЭСРН
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ValidateContract> Validate()
        {
            //Переменная указывает что цикл запустился в первый раз
            int iFirst = 0;

            //Пройдёмся по пулу строк подключения
            PullConnectBD pull = new PullConnectBD();
            Dictionary<string, string> pullConnect = pull.GetPull(FlagConnectServer);

            foreach (KeyValuePair<string, string> dStringConnect in pullConnect)
            {

                string sConnection = string.Empty;
                sConnection = dStringConnect.Value.ToString().Trim();

                //Выполним проверку в единой транзакции для конкретного района (строки подключения)
                using (SqlConnection con = new SqlConnection(sConnection))
                {

                    try
                    {
                        //Подключимся к текущему серверу
                        con.Open();
                    }
                    catch
                    {
                        //t.Abort();
                        //Если сервре не доступен 
                        System.Windows.Forms.MessageBox.Show("Сервер в районе " + dStringConnect.Key + " в настоящий момент не доступен.");
                        continue;

                    }

                        SqlTransaction transact = con.BeginTransaction();

                        //счётчик договоров
                        int iCount = 1;

                        //Пройдёмся по записям в файле реестра заключённых договоров
                        foreach (Unload un in unload.Values)
                        {
                            //Создадим экземпляр класса с результатом проверки
                            ValidateContract vContract = new ValidateContract();

                            //получим номер договора
                            DataRow rDog = un.Договор.Rows[0];
                            string номерДоговора = rDog["НомерДоговора"].ToString().Trim();

                            //un.Договор.Select("НомерДоговора = ''"

                            if (номерДоговора.Trim().ToLower() == "ОРБ/43".ToLower().Trim())
                            {
                                string sTest = "";
                            }

                            //Получим строку с льготником
                            DataRow rowLgot = un.Льготник.Rows[0];
                            string фамилия = rowLgot["Фамилия"].ToString().Replace('Ё','Е').Replace('ё','е').Trim();
                            string имя = rowLgot["Имя"].ToString().Replace('Ё', 'Е').Replace('ё', 'е').Trim();

                            string отчество = rowLgot["Отчество"].ToString().Replace('Ё', 'Е').Replace('ё', 'е').Trim();
                            string датаРождения = Convert.ToDateTime(rowLgot["ДатаРождения"]).ToShortDateString().Trim();
                            
                            //Хранит серию паспорта в формате 00 00 
                            string serPass = string.Empty;
                            
                            //string серПаспорт = rowLgot["СерияПаспорта"].ToString().Trim();
                            string серияПаспорта = rowLgot["СерияПаспорта"].ToString().Trim();

                            //Установим формат серии паспорта в формате 00 00
                                StringBuilder build = new StringBuilder();

                                //счётчик циклов
                                int iiCount = 0;
                                foreach (char ch in серияПаспорта)
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
                            
                            string номерПаспорта = rowLgot["НомерПаспорта"].ToString().Trim();
                            string датаВыдачиПаспорта = Convert.ToDateTime(rowLgot["ДатаВыдачиПаспорта"]).ToShortDateString().Trim();

                            //серия, номер и дата выдачи документа дающего право на льготу
                            string серияДокумента = rowLgot["СерияДокумента"].ToString().Trim();

                            if (серияДокумента.Trim().ToLower() == "-".Trim().ToLower())
                            {
                                серияДокумента = null;
                            }

                            string номерДокумента = rowLgot["НомерДокумента"].ToString().Trim();
                            string датаВыдачиДокумента = Convert.ToDateTime(rowLgot["ДатаВыдачиДокумента"]).ToShortDateString().Trim();


                            //Проверим ФИО и дату рождения в ЭСРН в зависимости от настроек в конфигурации поиска
                            if (iconfig == 1)
                            {
                                queryФИО = QuerySQL.QueryЭСРН(фамилия, имя, отчество);
                            }

                            if (iconfig == 2)
                            {
                                queryФИО = QuerySQL.QueryЭСРН(фамилия, имя, отчество, датаРождения);
                            }

                            if (iconfig == 3)
                            {
                                queryФИО = QuerySQL.QueryЭСРН(фамилия, имя, отчество, датаРождения, датаВыдачиДокумента);
                            }

                            if (iconfig == 4)
                            {
                                queryФИО = QuerySQL.QueryЭСРН(фамилия, имя, отчество, датаРождения, датаВыдачиДокумента, серияДокумента, номерДокумента);
                            }

                            if (iconfig == 5)
                            {
                                // Создадим экземпляр класса.
                                ValidatePrefCategory validCategor = new ValidatePrefCategory(фамилия, имя, отчество, Время.Дата(датаРождения), Время.Дата(датаВыдачиДокумента), серияДокумента, номерДокумента);
                                
                                // Проверим льготника по льготной категории.
                                if (un.ЛьготнаяКатегория.ToLower().Trim().Replace(" ",string.Empty) == "Ветеранвоеннойслужбы".ToLower().Trim())
                                {
                                    queryФИО = validCategor.ValidateВВС();
                                }

                                // Проверим льготника по льготной категории.
                                if (un.ЛьготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty) == "Ветерантруда".ToLower().Trim())
                                {
                                    queryФИО = validCategor.ValidateВТ();
                                }

                                // Проверим льготника по льготной категории.
                                if (un.ЛьготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty) == "Тружениктыла".ToLower().Trim())
                                {
                                    queryФИО = validCategor.ValidateТруженикТыла();
                                }

                                // Проверим льготника по льготной категории.
                                if (un.ЛьготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty) == "ВетерантрудаСаратовскойобласти".ToLower().Trim())
                                {
                                    queryФИО = validCategor.ValidateВТСО();
                                }

                                // Проверим льготника по льготной категории.
                                if (un.ЛьготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty) == "Реабилитированныелица".ToLower().Trim())
                                {
                                    queryФИО = validCategor.ValidateReabel();
                                }

                                //сравним данные с ЭСРН по документу дающему право на льготу
                                //queryФИО = QuerySQL.QueryЭСРН(фамилия, имя, отчество, датаРождения, датаВыдачиДокумента, серияДокумента, номерДокумента, датаВыдачиПаспорта, серияПаспорта, номерПаспорта);
                                //tableФИО = ТаблицаБД.GetTableSQL(queryФИО, "ФИО", con, transact);

                                ////найдём гражданина в ЭСРН по паспорту гражданина РФ
                                queryPassFio = QuerySQL.QueryPassportЭСРН(фамилия, имя, отчество, датаРождения, датаВыдачиДокумента, серияДокумента, номерДокумента, Время.Дата(датаВыдачиПаспорта), серияПаспорта, номерПаспорта);

                                queryPassFIO = QuerySQL.QueryPassportЭСРН(фамилия, имя, отчество, датаРождения, датаВыдачиДокумента, серияДокумента, номерДокумента, Время.Дата(датаВыдачиПаспорта), serPass, номерПаспорта);

                                ////ЭСРН папорт гражданина
                                tabPassFio = ТаблицаБД.GetTableSQL(queryPassFio, "ФИО_Pass", con, transact);

                                //ЭСРН паспорт гражданина где серия паспорта в формате 00 00
                                tabPassFIO = ТаблицаБД.GetTableSQL(queryPassFIO, "ФИО_PASS", con, transact);

                            }

                            if (con.ConnectionString == "Data Source=10.143.100.11;Initial Catalog=sitex;User ID=KSZN;")
                            //if (con.ConnectionString == System.Configuration.ConfigurationSettings.AppSettings["conKras"].ToString()+";")// "Data Source=10.143.100.11;Initial Catalog=sitex;User ID=KSZN;Password=KZxx6")
                            {
                                string iiTest = "Тест";
                            }

                            DataTable tabФИО = ТаблицаБД.GetTableSQL(queryФИО, "ФИО", con, transact);

                            if (iconfig != 5)
                            {
                                //Проверочное условие после теста стереть
                                if (tabФИО.Rows.Count != 0)
                                {
                                    DataTable iTable = tabФИО;
                                }

                                //Если в ЭСРН человек найден
                                if (tabФИО.Rows.Count != 0)
                                {
                                    //Если запись в таблице существует то проверку сичтаем успешной, однако проверим к какой льготной категории относиться данный льготник
                                    DataRow rowL = tableФИО.Rows[0];

                                    //Если запись в таблице существует то проверку считаем успешной
                                    vContract.FlagPersonЭСРН = true;

                                    //создадим список проверки в ЭСРН
                                    List<ЭСРНvalidate> эсрнValidate = new List<ЭСРНvalidate>();

                                    //Запишем все документы которые дают право на льготу
                                    foreach (DataRow r in tabФИО.Rows)
                                    {
                                        ЭСРНvalidate val = new ЭСРНvalidate();

                                        // Получим название района области.
                                        ListRegion listRegion = new ListRegion();

                                        listRegion.FindRegion(Convert.ToInt16(un.Льготник.Rows[0]["id_район"]));

                                        // Название района оласти.
                                        val.NameRegion = listRegion.FindRegion(Convert.ToInt16(un.Льготник.Rows[0]["id_район"]));// un.НаименованиеРайона.Rows[0]["РайонОбласти"].ToString().Trim();

                                        if (r["Имя"] != DBNull.Value)
                                        {
                                            val.Имя = r["Имя"].ToString();
                                        }

                                        if (r["Отчество"] != DBNull.Value)
                                        {
                                            val.Отчество = r["Отчество"].ToString();
                                        }

                                        if (r["Фамилия"] != DBNull.Value)
                                        {
                                            val.Фамилия = r["Фамилия"].ToString();
                                        }

                                        if (r["A_NAME"] != DBNull.Value)
                                        {
                                            val.НазваниеДокумента = r["A_NAME"].ToString();
                                        }

                                        if (r["Серия документа"] != DBNull.Value)
                                        {
                                            val.СерияДокумента = r["Серия документа"].ToString().Trim();
                                        }

                                        if (r["Номер документа"] != DBNull.Value)
                                        {
                                            val.НомерДокумента = r["Номер документа"].ToString().Trim();
                                        }

                                        if (r["дата выдачи"] != DBNull.Value)
                                        {
                                            val.ДатаВыдачиДокумента = Convert.ToDateTime(r["дата выдачи"]).ToShortDateString();
                                        }

                                        val.ДатаРождения = датаРождения;

                                        if (r["дата выдачи"] != DBNull.Value)
                                        {
                                            val.Адрес = r["Адрес"].ToString().Trim();
                                        }

                                        // Снилс льготника прочитанный из ЭСРН.
                                        if (r["A_SNILS"] != DBNull.Value)
                                        {
                                            vContract.SnilsPerson = r["A_SNILS"].ToString().Trim();
                                        }

                                        // ID района области где проживает льготник.
                                        if (r["A_REGREGIONCODE"] != DBNull.Value)
                                        {
                                            vContract.IdRegionEsrn = r["A_REGREGIONCODE"].ToString().Trim();
                                        }

                                        эсрнValidate.Add(val);
                                    }

                                    vContract.СписокДокументов = эсрнValidate;
                                }
                                else
                                {
                                    //проверку не прошёл
                                    vContract.FlagPersonЭСРН = false;
                                }
                            }
                            else
                            {

                                //Если номер удостоверения дающего право на льготу и номер паспорта льготника одинаковые то льготник не прошёл проверку
                                if ((датаВыдачиДокумента != датаВыдачиПаспорта) && (серияДокумента != серияПаспорта) && (номерДокумента != номерПаспорта))
                                {

                                    if ((tabФИО.Rows.Count != 0 && tabPassFio.Rows.Count != 0) || (tabФИО.Rows.Count != 0 && tabPassFIO.Rows.Count != 0))
                                    {

                                        if (tabФИО.Rows.Count != 0 && tabPassFio.Rows.Count != 0)
                                        {
                                            DataRow rowF = tabФИО.Rows[0];
                                            DataRow rowPF = tabPassFio.Rows[0];

                                            //Получим фамилию
                                            string _фамилия = rowF["Фамилия"].ToString().Trim();

                                            //Получим имя
                                            string _имя = rowF["Имя"].ToString().Trim();

                                            //полчим отчество
                                            string _отчество = rowF["Отчество"].ToString().Trim();

                                            //получим дату рождения
                                            string др = Convert.ToDateTime(rowF["ДатаРождения"]).ToShortDateString();

                                            //Получим фамилию
                                            string _фамилия2 = rowPF["Фамилия"].ToString().Trim();

                                            //Получим имя
                                            string _имя2 = rowPF["Имя"].ToString().Trim();

                                            //полчим отчество
                                            string _отчество2 = rowPF["Отчество"].ToString().Trim();

                                            //получим дату рождения
                                            string др2 = Convert.ToDateTime(rowPF["ДатаРождения"]).ToShortDateString();

                                            //Сделаем контроль льготной категории для конкретного лльготника
                                            if ((_фамилия == _фамилия2) && (_имя == _имя2) && (_отчество == _отчество2) && (др2 == др))
                                            {
                                                //Записываем льготную категорию
                                                vContract.FlagPersonЭСРН = Flag_ЛК_ESRN(_фамилия, _имя, _отчество, др, un.ЛьготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty), con, transact);

                                                //vContract.FlagPersonЭСРН = true;
                                            }

                                        }

                                        if (tabФИО.Rows.Count != 0 && tabPassFIO.Rows.Count != 0)
                                        {
                                            DataRow rowF = tabФИО.Rows[0];
                                            DataRow rowPF = tabPassFIO.Rows[0];

                                            //Получим фамилию
                                            string _фамилия = rowF["Фамилия"].ToString().Trim();

                                            //Получим имя
                                            string _имя = rowF["Имя"].ToString().Trim();

                                            //полчим отчество
                                            string _отчество = rowF["Отчество"].ToString().Trim();

                                            //получим дату рождения
                                            string др = Convert.ToDateTime(rowF["ДатаРождения"]).ToShortDateString();

                                            //Получим фамилию
                                            string _фамилия2 = rowPF["Фамилия"].ToString().Trim();

                                            //Получим имя
                                            string _имя2 = rowPF["Имя"].ToString().Trim();

                                            //полчим отчество
                                            string _отчество2 = rowPF["Отчество"].ToString().Trim();

                                            //получим дату рождения
                                            string др2 = Convert.ToDateTime(rowPF["ДатаРождения"]).ToShortDateString();

                                            //Сделаем контроль льготной категории для конкретного лльготника
                                            if ((_фамилия == _фамилия2) && (_имя == _имя2) && (_отчество == _отчество2) && (др2 == др))
                                            {
                                                vContract.FlagPersonЭСРН = Flag_ЛК_ESRN(_фамилия, _имя, _отчество, др, un.ЛьготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty), con, transact);

                                                //vContract.FlagPersonЭСРН = true;
                                            }

                                        }


                                        //Если запись в обоих таблицах существует то проверку считаем успешной
                                        //vContract.FlagPersonЭСРН = true;

                                        //создадим список проверки в ЭСРН
                                        List<ЭСРНvalidate> эсрнValidate = new List<ЭСРНvalidate>();

                                        //Запишем все документы которые дают право на льготу
                                        foreach (DataRow r in tabФИО.Rows)
                                        {
                                            ЭСРНvalidate val = new ЭСРНvalidate();

                                            // Название района оласти.
                                            //val.NameRegion = un.НаименованиеРайона.Rows[0]["РайонОбласти"].ToString().Trim();


                                            if (r["Имя"] != DBNull.Value)
                                            {
                                                val.Имя = r["Имя"].ToString();
                                            }

                                            if (r["Отчество"] != DBNull.Value)
                                            {
                                                val.Отчество = r["Отчество"].ToString();
                                            }

                                            if (r["Фамилия"] != DBNull.Value)
                                            {
                                                val.Фамилия = r["Фамилия"].ToString();
                                            }

                                            if (r["A_NAME"] != DBNull.Value)
                                            {
                                                val.НазваниеДокумента = r["A_NAME"].ToString();
                                            }

                                            if (r["Серия документа"] != DBNull.Value)
                                            {
                                                val.СерияДокумента = r["Серия документа"].ToString().Trim();
                                            }

                                            if (r["Номер документа"] != DBNull.Value)
                                            {
                                                val.НомерДокумента = r["Номер документа"].ToString().Trim();
                                            }

                                            if (r["дата выдачи"] != DBNull.Value)
                                            {
                                                val.ДатаВыдачиДокумента = Convert.ToDateTime(r["дата выдачи"]).ToShortDateString();
                                            }

                                            val.ДатаРождения = датаРождения;

                                            if (r["дата выдачи"] != DBNull.Value)
                                            {
                                                val.Адрес = r["Адрес"].ToString().Trim();
                                            }

                                            // Снилс льготника прочитанный из ЭСРН.
                                            if (r["A_SNILS"] != DBNull.Value)
                                            {
                                                vContract.SnilsPerson = r["A_SNILS"].ToString().Trim();
                                                val.SnilsPerson = r["A_SNILS"].ToString().Trim();
                                            }

                                            // ID района области где проживает льготник.
                                            if (r["A_REGREGIONCODE"] != DBNull.Value)
                                            {
                                                vContract.IdRegionEsrn = r["A_REGREGIONCODE"].ToString().Trim();
                                                val.IdRegionEsrn = r["A_REGREGIONCODE"].ToString().Trim();
                                            }

                                            эсрнValidate.Add(val);
                                        }

                                        vContract.СписокДокументов = эсрнValidate;

                                    }
                                }
                                else
                                {
                                    //если номер дата выдачи удостоверения дающего право на льготу совпадают с номером серией и датой выдачи паспорта то льготник не прошёл проверку
                                    vContract.FlagPersonЭСРН = false;
                                }
                            }

                            if (vContract.FlagPersonЭСРН == true)
                            {
                                try
                                {
                                        //Запишем результат проверки в коллекцию
                                        listValContr.Add(номерДоговора, vContract);

                                }
                                catch
                                {
                                    if (flagЗагрузки == false)
                                    {
                                        string _фио = string.Empty;

                                        foreach (ЭСРНvalidate эср in vContract.СписокДокументов)
                                        {
                                            _фио = эср.Фамилия + " " + эср.Имя + " " + эср.Отчество;
                                        }


                                        //System.Windows.Forms.MessageBox.Show("Встретился задвоенный номер договора " + номерДоговора.Trim() + " для льготника " + _фио.Trim());
                                    }

                                    if (flagЗагрузки == true)
                                    {
                                        //если загружаем всю базу тогда загружаем и задублированные номер договоров
                                        номерДоговора = номерДоговора + " " + iCount;
                                        listValContr.Add(номерДоговора, vContract);

                                        iCount++;
                                    }
                                }
                            }
                        }

                    //}
                    //catch
                    //{
                    //    //t.Abort();
                    //    //Если сервре не доступен 
                    //    System.Windows.Forms.MessageBox.Show("Сервер в районе " + dStringConnect.Key + " в настоящий момент не доступен.");
                    //    continue;

                    //}
                   

                    
                }

            }

            foreach (var iTestValid in listValContr)
            {
                var asd = iTestValid;
            }

            //Сравнение данных в контрольной базе на нашем сервере
            ValidatContractBD vConValid = new ValidatContractBD();
            vConValid.Validate(unload, listValContr);

            //System.Windows.Forms.MessageBox.Show("Проверка завершилась");
            return listValContr;
        }


        public Dictionary<string, ValidateContract> ValidateList()
        {
            //Переменная указывает что цикл запустился в первый раз
            int iFirst = 0;

            //Пройдёмся по пулу строк подключения
            PullConnectBD pull = new PullConnectBD();
            Dictionary<string, string> pullConnect = pull.GetPull(FlagConnectServer);

            foreach (KeyValuePair<string, string> dStringConnect in pullConnect)
            {

                string sConnection = string.Empty;
                sConnection = dStringConnect.Value.ToString().Trim();

                //Выполним проверку в единой транзакции для конкретного района (строки подключения)
                using (SqlConnection con = new SqlConnection(sConnection))
                {
                    try
                    {
                        //Подключимся к текущему серверу
                        con.Open();
                    }
                    catch
                    {
                        //t.Abort();
                        //Если сервре не доступен 
                        System.Windows.Forms.MessageBox.Show("Сервер в районе " + dStringConnect.Key + " в настоящий момент не доступен.");
                        continue;

                    }

                    SqlTransaction transact = con.BeginTransaction();

                    //счётчик договоров
                    int iCount = 1;

                    List<PersonValidEsrn> listPersonValidEsrn = new List<PersonValidEsrn>();

                    // Переменная для хранения наименования льготной категории.
                    string stringLk = string.Empty;

                    foreach (Unload un in unload.Values)
                    {
                        //Создадим экземпляр класса с результатом проверки
                        ValidateContract vContract = new ValidateContract();

                        PersonValidEsrn personValidEsrn = new PersonValidEsrn();

                        //получим номер договора
                        DataRow rDog = un.Договор.Rows[0];
                        personValidEsrn.номерДоговора = rDog["НомерДоговора"].ToString().Trim();

                        //Получим строку с льготником
                        DataRow rowLgot = un.Льготник.Rows[0];
                        personValidEsrn.фамилия = rowLgot["Фамилия"].ToString().Replace('Ё', 'Е').Replace('ё', 'е').Trim();
                        personValidEsrn.имя = rowLgot["Имя"].ToString().Replace('Ё', 'Е').Replace('ё', 'е').Trim();

                        personValidEsrn.отчество = rowLgot["Отчество"].ToString().Replace('Ё', 'Е').Replace('ё', 'е').Trim();
                        personValidEsrn.датаРождения = Convert.ToDateTime(rowLgot["ДатаРождения"]).ToShortDateString().Trim();

                        //Хранит серию паспорта в формате 00 00 
                        string serPass = string.Empty;

                        //string серПаспорт = rowLgot["СерияПаспорта"].ToString().Trim();
                        personValidEsrn.серияПаспорта = rowLgot["СерияПаспорта"].ToString().Trim();

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

                        personValidEsrn.номерПаспорта = rowLgot["НомерПаспорта"].ToString().Trim();
                        personValidEsrn.датаВыдачиПаспорта = Convert.ToDateTime(rowLgot["ДатаВыдачиПаспорта"]).ToShortDateString().Trim();

                        //серия, номер и дата выдачи документа дающего право на льготу
                        personValidEsrn.серияДокумента = rowLgot["СерияДокумента"].ToString().Trim();

                        if (personValidEsrn.серияДокумента.Trim().ToLower() == "-".Trim().ToLower())
                        {
                            personValidEsrn.серияДокумента = null;
                        }

                        personValidEsrn.номерДокумента = rowLgot["НомерДокумента"].ToString().Trim();
                        personValidEsrn.датаВыдачиДокумента = Convert.ToDateTime(rowLgot["ДатаВыдачиДокумента"]).ToShortDateString().Trim();

                        listPersonValidEsrn.Add(personValidEsrn);

                        // Проверим льготника по льготной категории.
                        if (un.ЛьготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty) == "Ветеранвоеннойслужбы".ToLower().Trim())
                        {
                            stringLk = "Ветеранвоеннойслужбы";
                        }

                        // Проверим льготника по льготной категории.
                        if (un.ЛьготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty) == "Ветерантруда".ToLower().Trim())
                        {
                            stringLk = "Ветерантруда";
                        }

                        // Проверим льготника по льготной категории.
                        if (un.ЛьготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty) == "Тружениктыла".ToLower().Trim())
                        {
                            stringLk = "Тружениктыла";
                        }

                        // Проверим льготника по льготной категории.
                        if (un.ЛьготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty) == "ВетерантрудаСаратовскойобласти".ToLower().Trim())
                        {
                            stringLk = "ВетерантрудаСаратовскойобласти";
                        }

                        // Проверим льготника по льготной категории.
                        if (un.ЛьготнаяКатегория.ToLower().Trim().Replace(" ", string.Empty) == "Реабилитированныелица".ToLower().Trim())
                        {
                            stringLk = "Реабилитированныелица";
                        }
                    }

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
                        string docPreferencyCategory = " PPR_DOC.A_NAME in ('Свидетельство о праве на льготы для реабилитированных лиц','Справка о реабилитации','Свидетельство о праве на льготы для лиц, признанных пострадавшими от политических репрессий','Справка о признании пострадавшим от политических репрессий') ";
                        queryФИО = validCategor.Validate(docPreferencyCategory);
                    }

                    //сравним данные с ЭСРН по документу дающему право на льготу
                    //queryФИО = QuerySQL.QueryЭСРН(фамилия, имя, отчество, датаРождения, датаВыдачиДокумента, серияДокумента, номерДокумента, датаВыдачиПаспорта, серияПаспорта, номерПаспорта);
                    //tableФИО = ТаблицаБД.GetTableSQL(queryФИО, "ФИО", con, transact);

                    //////найдём гражданина в ЭСРН по паспорту гражданина РФ
                    //queryPassFio = QuerySQL.QueryPassportЭСРН(фамилия, имя, отчество, датаРождения, датаВыдачиДокумента, серияДокумента, номерДокумента, Время.Дата(датаВыдачиПаспорта), серияПаспорта, номерПаспорта);

                    //queryPassFIO = QuerySQL.QueryPassportЭСРН(фамилия, имя, отчество, датаРождения, датаВыдачиДокумента, серияДокумента, номерДокумента, Время.Дата(датаВыдачиПаспорта), serPass, номерПаспорта);

                    //////ЭСРН папорт гражданина
                    //tabPassFio = ТаблицаБД.GetTableSQL(queryPassFio, "ФИО_Pass", con, transact);

                    ////ЭСРН паспорт гражданина где серия паспорта в формате 00 00
                    //tabPassFIO = ТаблицаБД.GetTableSQL(queryPassFIO, "ФИО_PASS", con, transact);

                }
            }

            return null;
        }

        private void Display()
        {
            FormОжидание form = new FormОжидание();
            //form.Left = (this.Left) + this.Width / 2 - (form.Width / 2);
            //form.Top = (this.Top) + this.Height / 2 - (form.Height / 2);
            form.TopMost = true;
            form.ShowDialog();
            //form.Show();

        }

        private void UnDisplay()
        {

        }

        /// <summary>
        /// Проверяет отноститься ли к текущей льготной категории льготник
        /// </summary>
        /// <param name="фамилия"></param>
        /// <param name="имя"></param>
        /// <param name="отчество"></param>
        /// <param name="льготКатегор"></param>
        /// <returns></returns>
        private bool Flag_ЛК_ESRN(string фамилия,string имя,string отчество,string датаРождения,string льготКатегор,SqlConnection con, SqlTransaction transact)
        {
            bool flag = false;

            string query = string.Empty;

            if (льготКатегор.Trim().ToLower() != "тружениктыла".Trim())
            {
                query = "select * from PPR_CAT " +
                               "where A_ID in ( " +
                               "select A_CATEGORY from SPR_NPD_MSP_CAT " +
                               "where A_ID in ( " +
                               "select A_SERV from ESRN_SERV_SERV " +
                               "where A_PERSONOUID in ( " +
                               "select OUID from WM_PERSONAL_CARD " +
                               "where SURNAME in (select OUID from SPR_FIO_SURNAME " +
                               "where LOWER(RTRIM(LTRIM(A_NAME))) = '" + фамилия + "') and A_NAME = (select OUID from SPR_FIO_NAME " +
                               "where LOWER(RTRIM(LTRIM(A_NAME))) = '" + имя + "') and A_SECONDNAME = (select OUID from SPR_FIO_SECONDNAME " +
                               "where LOWER(RTRIM(LTRIM(A_NAME))) = '" + отчество + "') and CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 112) = '" + Время.Дата(датаРождения) + "' ))) and REPLACE(LOWER(RTRIM(LTRIM(A_NAME))),' ','') = '" + льготКатегор.Trim().ToLower().Replace(" ", string.Empty) + "' ";
            }
            else if (льготКатегор.Trim().ToLower() == "тружениктыла".Trim())
            {

                string strLK = "Лица, проработавшие в тылу в период с 22.06.1941 по 9.05.1945 не менее 6 месяцев, исключая период работы на временно оккупированных территориях СССР, либо награжденным орденами или медалями СССР за самоотверженный труд в период Великой Отечественной войны".ToLower().Trim();

                query = "select * from PPR_CAT " +
                              "where A_ID in ( " +
                              "select A_CATEGORY from SPR_NPD_MSP_CAT " +
                              "where A_ID in ( " +
                              "select A_SERV from ESRN_SERV_SERV " +
                              "where A_PERSONOUID in ( " +
                              "select OUID from WM_PERSONAL_CARD " +
                              "where SURNAME in (select OUID from SPR_FIO_SURNAME " +
                              "where LOWER(RTRIM(LTRIM(A_NAME))) = '" + фамилия + "') and A_NAME = (select OUID from SPR_FIO_NAME " +
                              "where LOWER(RTRIM(LTRIM(A_NAME))) = '" + имя + "') and A_SECONDNAME = (select OUID from SPR_FIO_SECONDNAME " +
                              "where LOWER(RTRIM(LTRIM(A_NAME))) = '" + отчество + "') and CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 112) = '" + Время.Дата(датаРождения) + "' ))) and REPLACE(LOWER(RTRIM(LTRIM(A_NAME))),' ','') = '" + strLK.ToLower().Replace(" ", string.Empty) + "' ";
       
            }

            //Лица, проработавшие в тылу в период с 22.06.1941 по 9.05.1945 не менее 6 месяцев, исключая период работы на временно оккупированных территориях СССР, либо награжденным орденами или медалями СССР за самоотверженный труд в период Великой Отечественной войны


            DataTable tab = ТаблицаБД.GetTableSQL(query, "ЛК", con, transact);

            if (tab.Rows.Count != 0)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }



            return flag;
        }



    }
}

