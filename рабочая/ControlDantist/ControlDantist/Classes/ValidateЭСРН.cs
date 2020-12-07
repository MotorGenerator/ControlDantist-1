using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DantistLibrary;
using System.Threading;


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
            Dictionary<string,string> pullConnect = pull.GetPull();

            

            foreach (KeyValuePair<string, string> dStringConnect in pullConnect)
            {
                //Выполним проверку в единой транзакции для конкретного района (строки подключения)
                using (SqlConnection con = new SqlConnection(dStringConnect.Value))
                {

                    //Thread t = new Thread(Display);
                    //Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(Display));
                    //t.Start();

                    try
                    {
                    //Подключимся к текущему серверу
                    con.Open();



                        SqlTransaction transact = con.BeginTransaction();

                        //Пройдёмся по записям в файле реестра заключённых договоров
                        foreach (Unload un in unload.Values)
                        {
                            //Создадим экземпляр класса с результатом проверки
                            ValidateContract vContract = new ValidateContract();

                            //получим номер договора
                            DataRow rDog = un.Договор.Rows[0];
                            string номерДоговора = rDog["НомерДоговора"].ToString().Trim();

                            //listValContr.Add(номерДоговора

                            //Получим строку с льготником
                            DataRow rowLgot = un.Льготник.Rows[0];
                            string фамилия = rowLgot["Фамилия"].ToString().Trim();
                            string имя = rowLgot["Имя"].ToString().Trim();

                            string отчество = rowLgot["Отчество"].ToString().Trim();
                            string датаРождения = Convert.ToDateTime(rowLgot["ДатаРождения"]).ToShortDateString();
                            
                            //серия, номер пасорта и дата выдачи
                            //string серияПаспорта = string.Empty;
                            
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
                            

                            ////установим серию паспорта в формате 00 00 
                            //if (серПаспорт.Length == 4)
                            //{
                            //    //строка для хранения разделённой серии паспорта
                            //    StringBuilder build = new StringBuilder();

                            //    //счётчик циклов
                            //    int iiCount = 0;
                            //    foreach (char ch in серПаспорт)
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
                            //    серияПаспорта = build.ToString().Trim();
                            //}
                            //else
                            //{
                            //    //если изначально серия и номер сохранены в формате 00 00
                            //    серияПаспорта = серПаспорт;
                            //}

                            string номерПаспорта = rowLgot["НомерПаспорта"].ToString().Trim();
                            string датаВыдачиПаспорта = Convert.ToDateTime(rowLgot["ДатаВыдачиПаспорта"]).ToShortDateString().Trim();

                            //серия, номер и дата выдачи документа дающего право на льготу
                            string серияДокумента = rowLgot["СерияДокумента"].ToString().Trim();
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
                                //сравним данные с ЭСРН по документу дающему право на льготу
                                queryФИО = QuerySQL.QueryЭСРН(фамилия, имя, отчество, датаРождения, датаВыдачиДокумента, серияДокумента, номерДокумента, датаВыдачиПаспорта, серияПаспорта, номерПаспорта);

                                //найдём гражданина в ЭСРН по паспорту гражданина РФ
                                queryPassFio = QuerySQL.QueryPassportЭСРН(фамилия, имя, отчество, датаРождения, датаВыдачиДокумента, серияДокумента, номерДокумента, датаВыдачиПаспорта, серияПаспорта, номерПаспорта);

                                queryPassFIO = QuerySQL.QueryPassportЭСРН(фамилия, имя, отчество, датаРождения, датаВыдачиДокумента, серияДокумента, номерДокумента, датаВыдачиПаспорта, serPass, номерПаспорта);

                                //ЭСРН папорт гражданина
                                tabPassFio = ТаблицаБД.GetTableSQL(queryPassFio, "ФИО_Pass", con, transact);

                                //Эсре паспорт гражданина где серия паспорта в формате 00 00
                                tabPassFIO = ТаблицаБД.GetTableSQL(queryPassFIO, "ФИО_PASS", con, transact);

                            }

                            DataTable tabФИО = ТаблицаБД.GetTableSQL(queryФИО, "ФИО", con, transact);

                            ////ЭСРН папорт гражданина
                            //tabPassFio = ТаблицаБД.GetTableSQL(queryPassFio, "ФИО_Pass", con, transact);

                            ////Эсре паспорт гражданина где серия паспорта в формате 00 00
                            //tabPassFIO = ТаблицаБД.GetTableSQL(queryPassFIO, "ФИО_PASS", con, transact);

                            //if (tabPassFIO.Rows.Count != 0 && tabФИО.Rows.Count != 0)
                            //{
                            //    DataTable iTabTest = tabPassFIO;
                            //}
                            

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
                                    //Если запись в таблице существует то проверку считаем успешной
                                    vContract.FlagPersonЭСРН = true;

                                    //создадим список проверки в ЭСРН
                                    List<ЭСРНvalidate> эсрнValidate = new List<ЭСРНvalidate>();

                                    //Запишем все документы которые дают право на льготу
                                    foreach (DataRow r in tabФИО.Rows)
                                    {
                                        ЭСРНvalidate val = new ЭСРНvalidate();
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
                                //string s1 = string.Empty;
                                //if (tabPassFIO.Rows.Count != 0)
                                //{
                                //    if (tabPassFIO.Rows[0]["Фамилия"].ToString() == "Чесноков")
                                //    {
                                //        string iTest = s1;
                                //    }
                                //}


                                if ((tabФИО.Rows.Count != 0 && tabPassFio.Rows.Count != 0) || (tabФИО.Rows.Count != 0 && tabPassFIO.Rows.Count != 0))
                                {
                                    //Если запись в обоих таблицах существует то проверку считаем успешной
                                    vContract.FlagPersonЭСРН = true;

                                    //создадим список проверки в ЭСРН
                                    List<ЭСРНvalidate> эсрнValidate = new List<ЭСРНvalidate>();

                                    //Запишем все документы которые дают право на льготу
                                    foreach (DataRow r in tabФИО.Rows)
                                    {
                                        ЭСРНvalidate val = new ЭСРНvalidate();
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

                                        эсрнValidate.Add(val);
                                    }

                                    vContract.СписокДокументов = эсрнValidate;

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
                                    string _фио = string.Empty;
                                    foreach (ЭСРНvalidate эср in vContract.СписокДокументов)
                                    {
                                        _фио = эср.Фамилия + " " + эср.Имя + " " + эср.Отчество;
                                    }


                                    System.Windows.Forms.MessageBox.Show("Встретился задвоенный номер договора " + номерДоговора.Trim() + " для льготника " + _фио.Trim());
                                }
                            }
                        }

                    }
                    catch
                    {
                        //t.Abort();
                        //Если сервре не доступен 
                        System.Windows.Forms.MessageBox.Show("Сервер в районе " + dStringConnect.Key + " в настоящий момент не доступен.");
                        continue;

                    }
                    finally
                    {
                        //t.Abort();
                    }

                    
                }

            }

            //Сравнение данных в контрольной базе на нашем сервере
            ValidatContractBD vConValid = new ValidatContractBD();
            vConValid.Validate(unload, listValContr);

            //System.Windows.Forms.MessageBox.Show("Проверка завершилась");
            return listValContr;
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



    }
}

