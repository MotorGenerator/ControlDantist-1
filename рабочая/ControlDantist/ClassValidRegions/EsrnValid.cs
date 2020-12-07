using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ControlDantist.ClassValidRegions;
using ControlDantist.Classes;


namespace ControlDantist.ClassValidRegions
{
    public class EsrnValid
    {

        private List<Person> list;

        //SqlTransaction transact;
        SqlConnection con;
        StringBuilder builder;

        string strConnection = string.Empty;

        public bool FlagError { get; set; }

        public EsrnValid(string strConnectDB, List<Person> listPerson)
        {
            bool falgError = false;

           
           if (listPerson.Count > 0)
            {
                if (listPerson.Where(w => w.FlagValid == false).Count() > 0)
                {
                    list = listPerson;
                }
            }
            else
            {
                falgError = true;
            }

           strConnection = strConnectDB;

           builder = new StringBuilder();

        }

        public bool Validate()
        {
            bool flagError = false;

            using(con = new SqlConnection(strConnection))
            {
                try
                {
                    con.Open();

                    // Строка для хранения запроса к БД.
                    //StringBuilder builder = new StringBuilder();

                    // Создадим временную таблицу.
                    string createTable = "create table #t1_temp ([id_карточки] [int] IDENTITY(1,1) NOT NULL, " +
                                        " id_льготник int NULL, " +
                                        "[Фамилия] [varchar](255) NULL, " +
                                        "[Имя] [varchar](255) NULL, " +
                                        "[Отчество] [varchar](255) NULL, " +
                                        "[ДатаРождения] DateTime, " +
                                        "[СерияПаспорта] [varchar](255) NULL, " +
                                        "[НомерПаспорта] [varchar](255) NULL, " +
                                        "[ДатаВыдачиПаспорта] DateTime ," +
                                        "[СерияДокумента] [nvarchar](max) NULL, " +
                                        "[НомерДокумента] [nvarchar](max) NULL, " +
                                        "[ДатаВыдачиДокумента] [nvarchar](max) NULL) ";
                                        

                    // Сохраним в запросе создание временной таблицы для поиска льготников в ЭСРН из файла.
                    builder.Append(createTable);

                    // Создадим во временной таблице индексы.
                    string idxФамилия = "CREATE nonclustered INDEX Idx_Фамилия ON #t1_temp(Фамилия) ";
                    builder.Append(idxФамилия);

                    string idxИмя = "CREATE nonclustered INDEX Idx_Имя ON #t1_temp(Имя) ";
                    builder.Append(idxИмя);

                    string idxОтчество = "CREATE nonclustered INDEX Idx_Отчество ON #t1_temp(Отчество) ";
                    builder.Append(idxОтчество);

                    string idxДР = "CREATE nonclustered INDEX Idx_ДатаРождения ON #t1_temp(ДатаРождения) ";
                    builder.Append(idxДР);

                     string idxSP = "CREATE nonclustered INDEX Idx_СерияПаспорта ON #t1_temp(СерияПаспорта) ";
                    builder.Append(idxSP);

                    StringBuilder buildInsertInto = new StringBuilder();

                    // Заполним таблицу данными.
                    foreach (Person person in list)
                    {

                        string insert = "begin try insert into #t1_temp (id_льготник, Фамилия,Имя,Отчество,ДатаРождения,СерияПаспорта,НомерПаспорта,ДатаВыдачиПаспорта,СерияДокумента,НомерДокумента, ДатаВыдачиДокумента) " +
                                       "values (" + person.id_льготник + ",'" + person.Фамилия + "','" + person.Имя + "','" + person.Отчество + "','" + person.ДатаРождения.ToShortDateString().ToString() + "','" + person.СерияПаспорта + "','" + person.НомерПаспорта + "','" + Время.Дата(person.ДатаВыдачиПаспорта.ToShortDateString()) + "','" + person.СерияДокумента + "','" + person.НомерДокумента + "','" + Время.Дата(person.ДатаВыдачиДокумента.ToShortDateString()) + "') " +
                                       "end try begin catch end catch ";

                        buildInsertInto.Append(insert);
                    }

                    builder.Append(buildInsertInto.ToString());

                    string query = "select distinct A_REGREGIONCODE, [#t1_temp].id_льготник,A_SNILS  " +
                                   "FROM [#t1_temp] INNER JOIN " +
                                   "(select dbo.REGISTER_CONFIG.A_REGREGIONCODE,WM_PERSONAL_CARD.OUID, " +
                                   "SPR_FIO_SURNAME.A_NAME as Фамилия, " +
                                   "dbo.SPR_FIO_NAME.A_NAME as Имя,SPR_FIO_SECONDNAME.A_NAME as Отчество, " +
                                   "WM_ACTDOCUMENTS.DOCUMENTSTYPE,WM_ACTDOCUMENTS.DOCUMENTSERIES as 'СерияДокумента', " +
                                   "WM_ACTDOCUMENTS.DOCUMENTSNUMBER as 'НомерДокумента', " +
                                   "WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE as 'дата выдачи',PPR_DOC.A_NAME,WM_ADDRESS.A_ADRTITLE as 'Адрес', " +
                                   "WM_PERSONAL_CARD.BIRTHDATE as 'ДатаРождения',dbo.WM_PERSONAL_CARD.A_SNILS  from dbo.WM_PERSONAL_CARD  " +
                                   "join  SPR_FIO_SURNAME on WM_PERSONAL_CARD.SURNAME = SPR_FIO_SURNAME.OUID  " +
                                   " join SPR_FIO_NAME on SPR_FIO_NAME.OUID = WM_PERSONAL_CARD.A_NAME  " +
                                   "join SPR_FIO_SECONDNAME on SPR_FIO_SECONDNAME.OUID = WM_PERSONAL_CARD.A_SECONDNAME  " +
                                   "join dbo.WM_ACTDOCUMENTS on WM_PERSONAL_CARD.OUID = dbo.WM_ACTDOCUMENTS.PERSONOUID  " +
                                   "join PPR_DOC on WM_ACTDOCUMENTS.DOCUMENTSTYPE = PPR_DOC.A_ID  " +
                                   "join WM_ADDRESS on WM_ADDRESS.OUID = WM_PERSONAL_CARD.A_REGFLAT  " +
                                   " CROSS JOIN dbo.REGISTER_CONFIG ) as Tab1 " +
                                   "ON REPLACE(LOWER(Tab1.Фамилия),'ё','е') = REPLACE(LOWER([#t1_temp].Фамилия),'ё','е')  " +
                                   " and REPLACE(LOWER(Tab1.Имя),'ё','е') = REPLACE(LOWER([#t1_temp].Имя),'ё','е')  " +
                                   " and (REPLACE(LOWER(Tab1.Отчество),'ё','е')  = REPLACE(LOWER([#t1_temp].Отчество),'ё','е')) " +
                                   " AND stuff(Tab1.НомерДокумента, 1, patindex('%[^0]%', Tab1.НомерДокумента) - 1, '')  =  stuff([#t1_temp].НомерПаспорта, 1, patindex('%[^0]%', [#t1_temp].НомерПаспорта) - 1, '') " +
                                   "AND stuff(Tab1.СерияДокумента, 1, patindex('%[^0]%', Tab1.СерияДокумента) - 1, '')  =  stuff([#t1_temp].СерияПаспорта, 1, patindex('%[^0]%', [#t1_temp].СерияПаспорта) - 1, '') ";

                    // Закоментируем эту глупость. по паспорту и по документу в одной колонке не найдет.
                                   //" AND stuff(Tab1.НомерДокумента, 1, patindex('%[^0]%', Tab1.НомерДокумента) - 1, '')  =  stuff([#t1_temp].НомерДокумента, 1, patindex('%[^0]%', [#t1_temp].НомерДокумента) - 1, '') " +
                                   //"AND stuff(Tab1.СерияДокумента, 1, patindex('%[^0]%', Tab1.СерияДокумента) - 1, '')  =  stuff([#t1_temp].СерияДокумента, 1, patindex('%[^0]%', [#t1_temp].СерияДокумента) - 1, '') ";


                    builder.Append(query);

                    //Получим строку запроса.
                    string queryTable = builder.ToString();

                    SqlDataAdapter da = new SqlDataAdapter(queryTable, con);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Выборка");
                    con.Close();

                    DataTable tabPerson = ds.Tables[0];

                    // Пройдемся по списку районов.
                    foreach (DataRow row in tabPerson.Rows)
                    {
                        Person person = list.Where(w => w.id_льготник == Convert.ToInt32(row["id_льготник"])).FirstOrDefault();

                        if (person != null)
                        {
                            person.idRegion = Convert.ToInt32(row["A_REGREGIONCODE"]);

                            person.FlagValid = true;

                            if (row["A_SNILS"] != DBNull.Value)
                            {
                                person.SNILS = row["A_SNILS"].ToString().Trim();

                            }
                            else
                            {
                                person.SNILS = null;
                            }
                           
                        }

                    }

                }
                catch
                {
                    return true;
                }

            }

            return false;
        }
    }
}
