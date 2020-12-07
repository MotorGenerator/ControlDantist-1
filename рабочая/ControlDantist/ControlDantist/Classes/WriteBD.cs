using System;
using System.Collections.Generic;
using System.Text;
using ControlDantist.Classes;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;

using DantistLibrary;

namespace ControlDantist.Classes
{
    class WriteBD
    {
        //Хранит выгруженный реестр
        private List<Unload> list;

        private string льготнаяКатегория = string.Empty;
        private string документ = string.Empty;
        private string районОбласти = string.Empty;
        private string населённыйПункт = string.Empty;
        private string поликлинника = string.Empty;
        private string иннПоликлинники = string.Empty;
        private string главБух = string.Empty;

        //указывает, что договор с таким номером уже записан в БД
        private bool flagDog = false;

        public WriteBD(List<Unload> unload)
        {
            list = unload;
        }

        /// <summary>
        /// Записывает реестр в базу данных
        /// </summary>
        public void Write()
        {
            //Установим русскую культуру
            CultureInfo newCInfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            newCInfo.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = newCInfo;

            //Так как во время записи реестра в БД возхникает ситуация при которой 
            //льготник ещё не записан, а данные по поликлиннике уже в БД существуют
            //записывать будем в разных транзакциях(Решить тоже самое можно было с помощью TSQL)
            int iTest = list.Count;

                foreach (Unload unload in list)
                {
                    //обнулим 
                    flagDog = false;
                    
                    //узнаем есть ли такой договор в БД
                    using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
                    {
                        con.Open();

                       //получим номер договора в текущем реестре
                       DataTable tabДоговор = unload.Договор;
                       string numDog = tabДоговор.Rows[0]["НомерДоговора"].ToString().Trim();
                       //string queryDog = "select COUNT(*) as 'Количество' from Договор where НомерДоговора = '" + numDog + "' ";


                        DataTable tabЛьготник = unload.Льготник;

                        string фамилия = tabЛьготник.Rows[0]["Фамилия"].ToString().Trim();
                        string имя = tabЛьготник.Rows[0]["Имя"].ToString().Trim();
                        string отчество = tabЛьготник.Rows[0]["Отчество"].ToString().Trim();
                        string датаРождения = tabЛьготник.Rows[0]["ДатаРождения"].ToString().Trim();

                        string queryDog = "select COUNT(*) as 'Количество' from Договор where НомерДоговора = '" + numDog + "' " +
                                          "and id_льготник in (select id_льготник from Льготник " +
                                          "where Фамилия = '" + фамилия + "' and Имя = '" + имя + "' and Отчество = '" + отчество + "' and ДатаРождения = '" + датаРождения + "') ";
                       
                       
                        SqlTransaction transact = con.BeginTransaction();
                        DataTable t = ТаблицаБД.GetTableSQL(queryDog, "Договор", con, transact);

                        int i = Convert.ToInt32(t.Rows[0]["Количество"]);

                        if (i != 0)
                        {
                            //если в БД уже существует текущий договор
                            flagDog = true;

                            //Покажем пользователью, что данный договор уже существует
                            //DataTable tabЛьготник = unload.Льготник;
                            //string фамилия = tabЛьготник.Rows[0]["Фамилия"].ToString().Trim();
                            //string имя = tabЛьготник.Rows[0]["Имя"].ToString().Trim();
                            //string отчество = tabЛьготник.Rows[0]["Отчество"].ToString().Trim();

                            System.Windows.Forms.MessageBox.Show("Договор № " + numDog + " на " + фамилия + " " + имя + " " + отчество + " в базу данных уже записан");
                        }
                    }

                    if(flagDog == false)
                    {

                    //Выполним всё в одной транзакции
                    StringBuilder builderЛьготник = new StringBuilder();

                    using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
                    {
                        con.Open();
                        SqlTransaction transact = con.BeginTransaction();

                        //Выполним всё в одной транзакции
                        StringBuilder builder = new StringBuilder();

                        //Запишем населённый пункт и запомним его id
                        if (unload.НаселённыйПункт.Rows.Count != 0)
                        {
                            DataRow rowНасПункт = unload.НаселённыйПункт.Rows[0];
                            населённыйПункт = rowНасПункт["Наименование"].ToString();
                        }

                        //Проверим есть ли такой населённый пункт в базе данных на сервере
                        string controlНасПункт = "select count(Наименование) from НаселённыйПункт where Наименование like '%" + населённыйПункт + "%'";

                        //SqlCommand comContrНасПункт = new SqlCommand(controlНасПункт, con);
                        //comContrНасПункт.Transaction = transact;

                        //int iCountRowНасПункт = (int)comContrНасПункт.ExecuteScalar();

                        int iCountRowНасПункт = ValidateRows.Row(controlНасПункт, con, transact);
                        if (iCountRowНасПункт == 0)
                        {
                            string insertНасПункт = "INSERT INTO НаселённыйПункт(Наименование)VALUES('" + населённыйПункт + "') ";
                            builder.Append(insertНасПункт);
                        }

                        //Запишем названгие района
                        if (unload.НаименованиеРайона != null)
                        {
                            if (unload.НаименованиеРайона.Rows.Count != 0)
                            {
                                DataRow rowРайон = unload.НаименованиеРайона.Rows[0];
                                районОбласти = rowРайон["РайонОбласти"].ToString();
                            }
                        }

                        //Проверим есть ли в БД такой район
                        string controlРайон = "select count(РайонОбласти) from НаименованиеРайона where РайонОбласти like '%" + районОбласти + "%'";

                        int iCountРайон = ValidateRows.Row(controlРайон, con, transact);

                        if (iCountРайон == 0)
                        {
                            string insertРайон = "INSERT INTO НаименованиеРайона (РайонОбласти)VALUES('" + районОбласти + "')";
                            builder.Append(insertРайон);
                        }

                        //Запишем льготные категории
                        льготнаяКатегория = unload.ЛьготнаяКатегория;

                        //проверим есть ли такая льготная категория в БД
                        string controlЛК = "select count(ЛьготнаяКатегория) from ЛьготнаяКатегория where ЛьготнаяКатегория like '%" + льготнаяКатегория + "%'";
                        int iCountЛК = ValidateRows.Row(controlЛК, con, transact);

                        if (iCountЛК == 0)
                        {
                            string insertЛК = "INSERT INTO ЛьготнаяКатегория (ЛьготнаяКатегория)VALUES('" + льготнаяКатегория + "')";
                            builder.Append(insertЛК);
                        }

                        //Получим тип документа каким пользуется льготник
                        DataRow rowДок = unload.ТипДокумента.Rows[0];

                        //Узнаем есть ли токой документ в БД
                        документ = rowДок["НаименованиеТипаДокумента"].ToString();

                        string controlДок = "select count(НаименованиеТипаДокумента) from ТипДокумента where НаименованиеТипаДокумента like '%" + документ + "%'";

                        //Проверим есть ли такой док4умент в БД
                        int iCountДок = ValidateRows.Row(controlДок, con, transact);

                        if (iCountДок == 0)
                        {
                            string insertДок = "INSERT INTO ТипДокумента(НаименованиеТипаДокумента)VALUES('" + документ + "')";
                            builder.Append(insertДок);
                        }

                        //получим строку поликлинники
                        DataRow rowHosp = unload.Поликлинника.Rows[0];
                        //поликлинника = rowHosp["НаименованиеПоликлинники"].ToString().Trim();

                        //Получим ИНН поликлинники 
                        иннПоликлинники = rowHosp["ИНН"].ToString().Trim();

                        //Получим ФИО Глав врача
                        string главВарч = unload.ФиоВрач;
                        

                        //Проверим есть ли такой глав врач в БД
                        string controlBigEsculap = "select count(ФИО_ГлавВрач) from dbo.ФиоГлавВрач where ФИО_ГлавВрач like '%"+ главВарч +"%' ";
                        int iCountEsculap = ValidateRows.Row(controlBigEsculap, con, transact);

                        if(iCountEsculap == 0)
                        {
                            string queryГлавВрач = "INSERT INTO ФиоГлавВрач " +
                                           "([ФИО_ГлавВрач] " +
                                           ",[ИНН_поликлинники]) " +
                                           "VALUES " +
                                           "('"+ главВарч +"' " +
                                           ",'" + иннПоликлинники + "' ) ";

                            //Добавим в builder глав врача
                            builder.Append(queryГлавВрач);
                        }

                        //Получим ФИО глав буха
                        if (builder.ToString() != "")
                        {
                            //Выполним запрос на вставку в БД
                            ExecuteQuery.Execute(builder.ToString());
                        }
                    }
                
                    //Внесём данные в БД по поликлиннике
                    using(SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
                    {
                        con.Open();

                        //Проверим есть ли указанная поликлинника в БД
                        string controlHosp = "select COUNT(НаименованиеПоликлинники) from dbo.Поликлинника where НаименованиеПоликлинники like '%" + поликлинника + "%'";
                        int iCountHosp = ValidateRows.Row(controlHosp, con);

                        //Если нет данной поликлинники то добавим её в БД
                        if(iCountHosp == 0)
                        {
                            DataRow rowHosp = unload.Поликлинника.Rows[0];
                            поликлинника = rowHosp["НаименованиеПоликлинники"].ToString().Trim();

                            string insertHosp = "declare @idBigEsc int " +
                                               "select @idBigEsc = id_главВрач from dbo.ФиоГлавВрач where ФИО_ГлавВрач like '%" + unload.ФиоВрач + "%' " +
                                               "INSERT INTO [Поликлинника] " +
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
                                               "('"+ поликлинника +"' " +
                                               ",'"+ rowHosp["КодПоликлинники"].ToString().Trim() +"' " +
                                               ",'"+ rowHosp["ЮридическийАдрес"].ToString().Trim() +"' " +
                                               ",'"+ rowHosp["ФактическийАдрес"].ToString().Trim() +"' " +
                                               ",@idBigEsc " +
                                               ","+ Convert.ToInt32(rowHosp["id_главБух"]) +" " +
                                                ",'"+ rowHosp["СвидетельствоРегистрации"].ToString().Trim() +"' " +
                                                ",'"+ rowHosp["ИНН"].ToString().Trim() +"' " +
                                               ",'"+ rowHosp["КПП"].ToString().Trim() +"' " +
                                               ",'"+ rowHosp["БИК"].ToString().Trim() +"' " +
                                              ",'"+ rowHosp["НаименованиеБанка"].ToString().Trim() +"' " +
                                               ",'"+ rowHosp["РасчётныйСчёт"].ToString().Trim() +"' " +
                                              ",'"+ rowHosp["ЛицевойСчёт"].ToString().Trim() +"' " +
                                               ",'"+ rowHosp["НомерЛицензии"].ToString().Trim() +"' " +
                                               ",'"+ Convert.ToDateTime(rowHosp["ДатаРегистрацииЛицензии"]).ToShortDateString() +"' " +
                                               ",'"+ rowHosp["ОГРН"].ToString().Trim() +"' " +
                                               ",'"+ rowHosp["СвидетельствоРегистрацииЕГРЮЛ"].ToString().Trim() +"' " +
                                               ",'"+ rowHosp["ОрганВыдавшийЛицензию"].ToString().Trim() +"' " +
                                               ",'"+ rowHosp["Постановление"].ToString().Trim() +"' " +
                                               ",'"+ rowHosp["ОКПО"].ToString().Trim() +"' " +
                                               ",'"+ rowHosp["ОКАТО"].ToString().Trim() +"' " +
                                               ","+ Convert.ToInt32(rowHosp["Flag"]) +" " +
                                               "," + Convert.ToInt32(rowHosp["НачальныйНомерДоговора"]) + " )";

                            //Выполним запрос на вставку в БД
                            ExecuteQuery.Execute(insertHosp);
                        }
                    }

                    using (SqlConnection conЛьготник = new SqlConnection(ConnectDB.ConnectionString()))
                    {
                        conЛьготник.Open();
                        //SqlTransaction transact = conЛьготник.BeginTransaction();

                        //Счётчик циклов
                        int iCount = 0;

                        //Запишем данные по текущему льготнику
                        DataTable tabЛьготник = unload.Льготник;


                        if(tabЛьготник.Rows.Count != 0)
                        {
                            foreach (DataRow rwЛьготник in tabЛьготник.Rows)
                            {

                                string query = "if " +
                                           "(select count(Фамилия) from dbo.Льготник " +
                                           "where Фамилия = '" + rwЛьготник["Фамилия"].ToString().Trim() + "' " +
                                           "and Имя = '" + rwЛьготник["Имя"].ToString().Trim() + "' " +
                                           "and Отчество = '" + rwЛьготник["Отчество"].ToString().Trim() + "' " +
                                           "and ДатаРождения = '" + Convert.ToDateTime(rwЛьготник["ДатаРождения"]).ToShortDateString().Trim() + "' ) = 0 " +
                                           "begin " +
                                           "declare @idЛК_" + iCount + "  int " +
                                           "select @idЛК_" + iCount + " = id_льготнойКатегории from ЛьготнаяКатегория where ЛьготнаяКатегория like '%" + льготнаяКатегория + "%' " +
                                           "declare @idДокумент_" + iCount + " int " +
                                           "select @idДокумент_" + iCount + " = id_документ from ТипДокумента where НаименованиеТипаДокумента like '%" + документ + "%' " +
                                           "declare @id_Район_" + iCount + " int " +
                                           "select @id_Район_" + iCount + " = id_район from НаименованиеРайона where РайонОбласти like '%" + районОбласти + "%' " +
                                           "declare @id_насПункт_" + iCount + " int " +
                                           "select @id_насПункт_" + iCount + " = id_насПункт from НаселённыйПункт where Наименование like '%" + населённыйПункт + "%' " +
                                           "INSERT INTO Льготник " +
                                           "([Фамилия] " +
                                           ",[Имя] " +
                                           ",[Отчество] " +
                                           ",[ДатаРождения] " +
                                           ",[улица] " +
                                           ",[НомерДома] " +
                                           ",[корпус] " +
                                           ",[НомерКвартиры] " +
                                           ",[СерияПаспорта] " +
                                           ",[НомерПаспорта] " +
                                           ",[ДатаВыдачиПаспорта] " +
                                           ",[КемВыданПаспорт] " +
                                           ",[id_льготнойКатегории] " +
                                           ",[id_документ] " +
                                           ",[СерияДокумента] " +
                                           ",[НомерДокумента] " +
                                           ",[ДатаВыдачиДокумента] " +
                                           ",[КемВыданДокумент] " +
                                           ",[id_область] " +
                                           ",[id_район] " +
                                           ",[id_насПункт]) " +
                                           "VALUES " +
                                           "('" + rwЛьготник["Фамилия"].ToString().Trim() + "' " +
                                           ",'" + rwЛьготник["Имя"].ToString().Trim() + "' " +
                                           ",'" + rwЛьготник["Отчество"].ToString().Trim() + "' " +
                                           ",'" + Convert.ToDateTime(rwЛьготник["ДатаРождения"]).ToShortDateString().Trim() + "' " +
                                           ",'" + rwЛьготник["улица"].ToString().Trim() + "' " +
                                           ",'" + rwЛьготник["НомерДома"].ToString().Trim() + "' " +
                                           ",'" + rwЛьготник["корпус"].ToString().Trim() + "' " +
                                           ",'" + rwЛьготник["НомерКвартиры"].ToString().Trim() + "' " +
                                           ",'" + rwЛьготник["СерияПаспорта"].ToString().Trim() + "' " +
                                           ",'" + rwЛьготник["НомерПаспорта"].ToString().Trim() + "' " +
                                           ",'" + Convert.ToDateTime(rwЛьготник["ДатаВыдачиПаспорта"]).ToShortDateString().Trim() + "' " +
                                           ",'" + rwЛьготник["КемВыданПаспорт"].ToString().Trim() + "' " +
                                           ",@idЛК_" + iCount + " " +
                                           ",@idДокумент_" + iCount + " " +
                                           ",'" + rwЛьготник["СерияДокумента"].ToString().Trim() + "' " +
                                           ",'" + rwЛьготник["НомерДокумента"].ToString().Trim() + "' " +
                                           ",'" + Convert.ToDateTime(rwЛьготник["ДатаВыдачиДокумента"]).ToShortDateString().Trim() + "' " +
                                           ",'" + rwЛьготник["КемВыданДокумент"].ToString().Trim() + "' " +
                                           ",1 " + //id области у нас по умолчанию 1
                                           ",@id_Район_" + iCount + " " +
                                           ",@id_насПункт_" + iCount + " ) " +
                                           "end ";

                                //string query = "declare @idЛК_"+ iCount +"  int " +
                                //           "select @idЛК_" + iCount + " = id_льготнойКатегории from ЛьготнаяКатегория where ЛьготнаяКатегория like '%" + льготнаяКатегория + "%' " +
                                //           "declare @idДокумент_"+ iCount +" int " +
                                //           "select @idДокумент_" + iCount + " = id_документ from ТипДокумента where НаименованиеТипаДокумента like '%" + документ + "%' " +
                                //           "declare @id_Район_"+ iCount +" int " +
                                //           "select @id_Район_" + iCount + " = id_район from НаименованиеРайона where РайонОбласти like '%" + районОбласти + "%' " +
                                //           "declare @id_насПункт_"+ iCount +" int " +
                                //           "select @id_насПункт_" + iCount + " = id_насПункт from НаселённыйПункт where Наименование like '%" + населённыйПункт + "%' " +
                                //           "INSERT INTO Льготник " +
                                //           "([Фамилия] " +
                                //           ",[Имя] " +
                                //           ",[Отчество] " +
                                //           ",[ДатаРождения] " +
                                //           ",[улица] " +
                                //           ",[НомерДома] " +
                                //           ",[корпус] " +
                                //           ",[НомерКвартиры] " +
                                //           ",[СерияПаспорта] " +
                                //           ",[НомерПаспорта] " +
                                //           ",[ДатаВыдачиПаспорта] " +
                                //           ",[КемВыданПаспорт] " +
                                //           ",[id_льготнойКатегории] " +
                                //           ",[id_документ] " +
                                //           ",[СерияДокумента] " +
                                //           ",[НомерДокумента] " +
                                //           ",[ДатаВыдачиДокумента] " +
                                //           ",[КемВыданДокумент] " +
                                //           ",[id_область] " +
                                //           ",[id_район] " +
                                //           ",[id_насПункт]) " +
                                //           "VALUES " +
                                //           "('" + rwЛьготник["Фамилия"].ToString().Trim() + "' " +
                                //           ",'" + rwЛьготник["Имя"].ToString().Trim() + "' " +
                                //           ",'" + rwЛьготник["Отчество"].ToString().Trim() + "' " +
                                //           ",'" + Convert.ToDateTime(rwЛьготник["ДатаРождения"]).ToShortDateString().Trim() + "' " +
                                //           ",'" + rwЛьготник["улица"].ToString().Trim() + "' " +
                                //           ",'" + rwЛьготник["НомерДома"].ToString().Trim() + "' " +
                                //           ",'" + rwЛьготник["корпус"].ToString().Trim() + "' " +
                                //           ",'" + rwЛьготник["НомерКвартиры"].ToString().Trim() + "' " +
                                //           ",'" + rwЛьготник["СерияПаспорта"].ToString().Trim() + "' " +
                                //           ",'" + rwЛьготник["НомерПаспорта"].ToString().Trim() + "' " +
                                //           ",'" + Convert.ToDateTime(rwЛьготник["ДатаВыдачиПаспорта"]).ToShortDateString().Trim() + "' " +
                                //           ",'" + rwЛьготник["КемВыданПаспорт"].ToString().Trim() + "' " +
                                //           ",@idЛК_" + iCount + " " +
                                //           ",@idДокумент_" + iCount + " " +
                                //           ",'" + rwЛьготник["СерияДокумента"].ToString().Trim() + "' " +
                                //           ",'" + rwЛьготник["НомерДокумента"].ToString().Trim() + "' " +
                                //           ",'" + Convert.ToDateTime(rwЛьготник["ДатаВыдачиДокумента"]).ToShortDateString().Trim() + "' " +
                                //           ",'" + rwЛьготник["КемВыданДокумент"].ToString().Trim() + "' " +
                                //           ",1 " + //id области у нас по умолчанию 1
                                //           ",@id_Район_" + iCount + " " +
                                //           ",@id_насПункт_" + iCount + " ) ";


                                builderЛьготник.Append(query);

                                iCount++;
                            }

                            //Выполним запрос на вставку в БД
                            ExecuteQuery.Execute(builderЛьготник.ToString());
                        }
                    }

                    //Загрузим на сервер договор
                        using (SqlConnection conДоговор = new SqlConnection(ConnectDB.ConnectionString()))
                        {
                            conДоговор.Open();
                            DataTable tabДоговор = unload.Договор;

                            //Получим теккущего льготника
                            DataRow r = unload.Льготник.Rows[0];

                            //Получим серию и номер паспорта для идентификации льготника
                            string серия = r["СерияПаспорта"].ToString().Trim();
                            string номер = r["НомерПаспорта"].ToString().Trim();

                            //Выполним всё в единой транзакции
                            StringBuilder build = new StringBuilder();

                            if(tabДоговор.Rows.Count != 0)
                            {
                                int iCount = 0;

                                    foreach(DataRow row in tabДоговор.Rows)
                                    {
                                        string queryДоговор = "declare @idЛК_" + iCount + " int " +
                                                             "select @idЛК_" + iCount + " = id_льготнойКатегории from ЛьготнаяКатегория where ЛьготнаяКатегория like '%" + льготнаяКатегория + "%' " +
                                                             "declare @IdHosp_" + iCount + " int " +
                                                             "select @IdHosp_" + iCount + " = id_поликлинника from dbo.Поликлинника where ИНН like '%" + иннПоликлинники + "%' " +
                                                             "declare @idЛьготник_" + iCount + " int " +
                                                             "select @idЛьготник_" + iCount + " = id_льготник from dbo.Льготник where СерияПаспорта like '%" + серия + "%' and НомерПаспорта like '%" + номер + "%' " +
                                                               "INSERT INTO [Договор] " +
                                                               "([НомерДоговора] " +
                                                               ",[ДатаДоговора] " +
                                                               ",[ДатаАктаВыполненныхРабот] " +
                                                               ",[СуммаАктаВыполненныхРабот] " +
                                                               ",[id_льготнаяКатегория] " +
                                                               ",[id_поликлинника] " +
                                                               ",[Примечание] " +
                                                               ",[id_комитет] " +
                                                               ",[ФлагНаличияДоговора] " +
                                                               ",[ФлагНаличияАкта] " +
                                                               " ,[id_льготник] " +
                                                               ",[ФлагДопСоглашения]) " +
                                                               "VALUES " +
                                                               "('" + row["НомерДоговора"].ToString() + "' " +
                                                               //",'" + Convert.ToDateTime(row["ДатаДоговора"]).ToShortDateString() + "' " +
                                                               //",'" + Convert.ToDateTime(row["ДатаАктаВыполненныхРабот"]).ToShortDateString() + "' " +
                                                               //"," + Convert.ToDecimal(row["СуммаАктаВыполненныхРабот"]) + " " +
                                                               ", '' " +
                                                               ", '' " +
                                                               ", '' " +
                                                               ",@idЛК_" + iCount + " " +
                                                               ",@IdHosp_" + iCount + " " +
                                                               ",'" + row["Примечание"].ToString() + "' " +
                                                               ",1" + //Постиавим 1 так как id комитета по идее равно 1
                                                               ",'" + Convert.ToBoolean(row["ФлагНаличияДоговора"]) + "' " +
                                                               ",'" + Convert.ToBoolean(row["ФлагНаличияАкта"]) + "' " +
                                                               ",@idЛьготник_" + iCount + " " +
                                                               ",'" + row["НомерДоговора"].ToString() + "' ) ";

                                        //Сложим запрос в динамическую строку
                                          build.Append(queryДоговор);

                                        iCount++;
                                    }
                                    //Выполним запрос на вставку в БД
                                    ExecuteQuery.Execute(build.ToString());

                                }
                        }

                    //Запишем Услуги по договору
                        using (SqlConnection conУслуги = new SqlConnection(ConnectDB.ConnectionString()))
                        {
                            conУслуги.Open();

                            //Строка для хранения запроса выполняемого в единой транзакции
                            StringBuilder builder = new StringBuilder();
                        
                            //Счётчик
                            int iCount = 0;

                            //Получим текущий договор
                            DataRow rowДоговор = unload.Договор.Rows[0];

                            //Получим номер текущего договора
                            string номерДоговора = rowДоговор["НомерДоговора"].ToString();

                            //Получим услуги по договору
                            DataTable tabУслуги = unload.УслугиПоДоговору;

                            if (tabУслуги.Rows.Count != 0)
                            {
                                foreach (DataRow row in tabУслуги.Rows)
                                {
                                    string query = "declare @idДоговор_" + iCount + " int " +
                                                   "select @idДоговор_" + iCount + " = id_договор from dbo.Договор where НомерДоговора like '%" + номерДоговора + "%' " +
                                                   "INSERT INTO УслугиПоДоговору " +
                                                   "([НаименованиеУслуги] " +
                                                   ",[цена] " +
                                                   ",[Количество] " +
                                                   ",[id_договор] " +
                                                   ",[НомерПоПеречню] " +
                                                   ",[Сумма] " +
                                                   ",[ТехЛист]) " +
                                                   "VALUES " +
                                                    "('" + row["НаименованиеУслуги"].ToString() + "' " +
                                                    "," + Convert.ToDecimal(row["Цена"]) + " " +
                                                    "," + Convert.ToInt32(row["Количество"]) + " " +
                                                    ",@idДоговор_" + iCount + " " +
                                                    ",'" + row["НомерПоПеречню"].ToString() + "' " +
                                                    "," + Convert.ToDecimal(row["Сумма"]) + " " +
                                                    "," + Convert.ToInt32(row["Количество"]) + " ) ";

                                    builder.Append(query);

                                    iCount++;
                                }

                                //Выполним запрос на добавление услуг по договору в единой транзакции

                                ExecuteQuery.Execute(builder.ToString());
                            }
                        }

                    //Запишем в БД на сервер данные по доп соглашению для данного договора
                        using (SqlConnection conДопСоглашение = new SqlConnection(ConnectDB.ConnectionString()))
                        {
                            conДопСоглашение.Open();

                             //Счётчик
                            int iCount = 0;

                            //Получим текущий договор
                            DataRow rowДоговор = unload.Договор.Rows[0];

                            //Получим номер текущего договора
                            string номерДоговора = rowДоговор["НомерДоговора"].ToString();

                            DataTable tabДопСоглашение = unload.ДопСоглашение;

                            //Запишем ДопСоглашение в единой транзакции
                            StringBuilder builder = new StringBuilder();

                            if (tabДопСоглашение.Rows.Count != 0)
                            {
                                foreach (DataRow row in tabДопСоглашение.Rows)
                                {
                                    string query = "declare @idДоговор_" + iCount + " int " +
                                                   "select @idДоговор_" + iCount + " = id_договор from dbo.Договор where НомерДоговора like '%" + номерДоговора + "%' " +
                                                   "INSERT INTO ДопСоглашение " +
                                                   "([id_договор] " +
                                                   ",[НомерДопСоглашения] " +
                                                   ",[Дата]) " +
                                                   "VALUES " +
                                                   "(@idДоговор_" + iCount + " " +
                                                   ",'" + row["НомерДопСоглашения"].ToString() + "' " +
                                                   ",'" + Convert.ToDateTime(row["Дата"]).ToShortDateString() + "' ) ";

                                    builder.Append(query);
                                }

                                //Выполним запрос в единой транзакции
                                ExecuteQuery.Execute(builder.ToString());
                            }
                        }

                    //Запишем акт выполненных работ в базу данных на сервер
                        using (SqlConnection conАктВыпРабот = new SqlConnection(ConnectDB.ConnectionString()))
                        {
                            conАктВыпРабот.Open();

                             //Счётчик
                            int iCount = 0;

                            //Получим текущий договор
                            DataRow rowДоговор = unload.Договор.Rows[0];

                            //Получим номер текущего договора
                            string номерДоговора = rowДоговор["НомерДоговора"].ToString();

                            //При записи проектов реестров договоров в БД не будем записывать данные по актам выполненных работ

                            //Запишем Акт выполненных работ в единой транзакции
                            //StringBuilder builder = new StringBuilder();
                            //DataTable tabАктВыпРаб = unload.АктВыполненныхРабот;

                            ////Проверим вдруг в таблице нет записей
                            //if (tabАктВыпРаб.Rows.Count != 0)
                            //{
                            //    foreach (DataRow row in tabАктВыпРаб.Rows)
                            //    {
                            //        string query = "declare @idДоговор_" + iCount + " int " +
                            //                       "select @idДоговор_" + iCount + " = id_договор from dbo.Договор where НомерДоговора like '%" + номерДоговора + "%' " +
                            //                       "INSERT INTO АктВыполненныхРабот " +
                            //                       "([НомерАкта] " +
                            //                       ",[id_договор] " +
                            //                       ",[ФлагПодписания] " +
                            //                       ",[ДатаПодписания] " +
                            //                       ",[НомерПоПеречню] " +
                            //                       ",[НаименованиеУслуги] " +
                            //                       ",[Цена] " +
                            //                       ",[Количество] " +
                            //                       ",[Сумма] " +
                            //                       ",[ФлагДопСоглашение]) " +
                            //                 "VALUES " +
                            //                       "('" + row["НомерАкта"].ToString() + "' " +
                            //                       ",@idДоговор_" + iCount + " " +
                            //                       ",'" + row["ФлагПодписания"].ToString() + "' " +
                            //                       ",'" + Convert.ToDateTime(row["ДатаПодписания"]).ToShortDateString() + "' " +
                            //                       ",'" + row["НомерПоПеречню"].ToString() + "' " +
                            //                       ",'" + row["НаименованиеУслуги"].ToString() + "' " +
                            //                       "," + Convert.ToDecimal(row["Цена"]) + " " +
                            //                       "," + Convert.ToInt32(row["Количество"]) + " " +
                            //                       "," + Convert.ToDecimal(row["Сумма"]) + " " +
                            //                       ",'" + row["ФлагДопСоглашение"].ToString() + "') ";

                            //        builder.Append(query);
                            //    }

                            //    //Выполним запрос в единой транзакции
                            //    ExecuteQuery.Execute(builder.ToString());
                            //}

                        }

                    
                      }
                     }//Конец перебора записей в выгруженном реестр
                    
                    }//условие что такого договора в БД нет

                
           // }
        }
    }
//}
