using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Classes;

namespace ControlDantist.ClassValidRegions
{
    public class ValidatePrefCategoryList
    {
        private List<PersonValidEsrn> listPersonValidEsrn;

        // Переменная для хранения названия временной таблицы.
        private string nameTempTable = string.Empty;

        public ValidatePrefCategoryList(List<PersonValidEsrn> listPersonValidEsrn)
        {
            this.listPersonValidEsrn = listPersonValidEsrn;
        }

        /// <summary>
        /// Создание временной таблицы.
        /// </summary>
        /// <returns></returns>
        private string CreateTempTable(string nameTempTable)
        {

            this.nameTempTable = nameTempTable;

            string createTable = "create table "+ nameTempTable + " ([id_карточки] [int] IDENTITY(1,1) NOT NULL, " +
                                "id_договор int NULL, " +
                                "[Фамилия] [nvarchar](50) NULL, " +
                                "[Имя] [nvarchar](50) NULL, " +
                                "[Отчество] [nvarchar](50) NULL, " +
                                "[ДатаРождения] DateTime, " +
                                "[ВидЛьготногоУдостоверения] [nvarchar](50) NULL, " +
                                "[СерияДокумента] [nvarchar](50) NULL, " +
                                "[НомерДокумента] [nvarchar](50) NULL, " +
                                 "[ДатаВыдачиДокумента] DateTime, " +
                                 "СерияПаспорта nvarchar (10) NULL, " +
                                 "НомерПаспорта nvarchar (10) NULL, " +
                                 "ДатаВыдачиПаспорта DateTime )";

            return createTable;
        }

        /// <summary>
        /// Заполнение временной таблицы данными.
        /// </summary>
        /// <param name="builderQuery"></param>
        private void InsertDateTempTable(StringBuilder builderQuery)
        {
            foreach (var itm in this.listPersonValidEsrn)
            {
                string queryInsert = " insert into "+ nameTempTable + " (id_договор,Фамилия,Имя,Отчество,ДатаРождения,СерияДокумента,НомерДокумента,ДатаВыдачиДокумента,СерияПаспорта,НомерПаспорта,ДатаВыдачиПаспорта) " +
                                     " values("+ itm.IdContract +",'" + itm.фамилия.Trim().ToLower() + "','" + itm.имя.Trim().ToLower() + "','" + itm.отчество.Trim().ToLower() + "','" + Время.Дата(itm.датаРождения.Trim()) + "','" + itm.серияДокумента.Do(x=>x,"").ToLower().Trim() + "','" + itm.номерДокумента.ToLower().Trim() + "','" + Время.Дата(itm.датаВыдачиДокумента.Trim()) + "',  " + 
                                     " '"+ itm.серияПаспорта.Trim().ToLower() +"','"+ itm.номерПаспорта.Trim().ToLower() +"','"+ Время.Дата(itm.датаВыдачиПаспорта) +"' ) ";

                builderQuery.Append(queryInsert);
            }

        }

        /// <summary>
        /// Проверка по паспорту.
        /// </summary>
        /// <returns></returns>
        public string PasswordCheck()
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Создаем временную таблицу.
            stringBuilder.Append(CreateTempTable("#t2_temp"));

            InsertDateTempTable(stringBuilder);

            string queryJoin = @"select  " + nameTempTable + ".id_договор, OUID,Tab1.Фамилия,Tab1.Имя,Tab1.Отчество,DOCUMENTSTYPE,DOCUMENTSERIES,DOCUMENTSNUMBER,ISSUEEXTENSIONSDATE,A_NAME,A_ADRTITLE,BIRTHDATE,A_SNILS from ( " +
                               "  select WM_PERSONAL_CARD.OUID, SPR_FIO_SURNAME.A_NAME as Фамилия,dbo.SPR_FIO_NAME.A_NAME as 'Имя',SPR_FIO_SECONDNAME.A_NAME as 'Отчество',WM_ACTDOCUMENTS.DOCUMENTSTYPE, " +
                                 " WM_ACTDOCUMENTS.DOCUMENTSERIES ,WM_ACTDOCUMENTS.DOCUMENTSNUMBER ,WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE ,PPR_DOC.A_NAME,WM_ADDRESS.A_ADRTITLE, " +
                                " WM_PERSONAL_CARD.BIRTHDATE ,dbo.WM_PERSONAL_CARD.A_SNILS, dbo.REGISTER_CONFIG.A_REGREGIONCODE from dbo.WM_PERSONAL_CARD " +
                                 " join  SPR_FIO_SURNAME " +
                                 " on WM_PERSONAL_CARD.SURNAME = SPR_FIO_SURNAME.OUID " +
                                  " join SPR_FIO_NAME " +
                                  " on SPR_FIO_NAME.OUID = WM_PERSONAL_CARD.A_NAME " +
                                  " join SPR_FIO_SECONDNAME " +
                                  " on SPR_FIO_SECONDNAME.OUID = WM_PERSONAL_CARD.A_SECONDNAME " +
                                  " join dbo.WM_ACTDOCUMENTS " +
                                  " on WM_PERSONAL_CARD.OUID = dbo.WM_ACTDOCUMENTS.PERSONOUID " +
                                  " join PPR_DOC " +
                                  " on WM_ACTDOCUMENTS.DOCUMENTSTYPE = PPR_DOC.A_ID " +
                                  " join WM_ADDRESS " +
                                  " on WM_ADDRESS.OUID = WM_PERSONAL_CARD.A_REGFLAT " +
                                  " CROSS JOIN dbo.REGISTER_CONFIG  " +
                                  " where  WM_PERSONAL_CARD.A_PCSTATUS = 1  " +
                                  " and PPR_DOC.A_NAME in ('Удостоверение ветерана труда','Удостоверение ветерана труда Саратовской области', " +
                                " 'Удостоверение о праве на льготы (отметка - ст.20)','Свидетельство о праве на льготы для реабилитированных лиц', " +
                                " 'Справка о реабилитации','Свидетельство о праве на льготы для лиц, признанных пострадавшими от политических репрессий', " +
                                " 'Справка о признании пострадавшим от политических репрессий','Удостоверение ветерана военной службы','Паспорт гражданина России') ) as Tab1  " +
                                 " inner join "+ nameTempTable + " " +
                                " on LOWER(RTRIM(LTRIM(Tab1.Фамилия))) = LOWER(RTRIM(LTRIM("+ nameTempTable + ".Фамилия))) " +
                                 " and LOWER(RTRIM(LTRIM(Tab1.Имя))) = LOWER(RTRIM(LTRIM("+ nameTempTable + ".Имя))) " +
                                 " and((LOWER(RTRIM(LTRIM(Tab1.Отчество))) = LOWER(RTRIM(LTRIM("+ nameTempTable + ".Отчество))) or  "+ nameTempTable + ".Отчество is NULL)) " +
                                 " and REPLACE(CONVERT(char(10), LOWER(RTRIM(LTRIM(Tab1.BIRTHDATE))), 104),' ','') = REPLACE(CONVERT(char(10), LOWER(RTRIM(LTRIM( " + nameTempTable + ".ДатаРождения))), 104), ' ','') " +
                                 " and REPLACE(LOWER(RTRIM(LTRIM(Tab1.DOCUMENTSERIES))),' ','') = REPLACE(LOWER(RTRIM(LTRIM( " + nameTempTable + ".СерияПаспорта))),' ','') " +
                                 " and LOWER(RTRIM(LTRIM(Tab1.DOCUMENTSNUMBER))) = LOWER(RTRIM(LTRIM( " + nameTempTable + ".НомерПаспорта))) " +
                                 " and CONVERT(char(10), LOWER(RTRIM(LTRIM(Tab1.ISSUEEXTENSIONSDATE))), 104) = CONVERT(char(10), LOWER(RTRIM(LTRIM( " + nameTempTable + ".ДатаВыдачиПаспорта))), 104) ";

            stringBuilder.Append(queryJoin);

            return stringBuilder.ToString();
        }

        //public string ValidatePreferentyCategory()
        //{
        //    //  Перемнная для хранения строки запроса к БД.
        //    StringBuilder builderQuery = new StringBuilder();

        //    // Создадим временную таблицу.
        //    builderQuery.Append(CreateTempTable());
        //}

        public string Validate(string nameDocPreferentCategory)
        {
            //  Перемнная для хранения строки запроса к БД.
            StringBuilder builderQuery = new StringBuilder();

            // Создадим временную таблицу.
            builderQuery.Append(CreateTempTable("#t1_temp"));

            // Заполним данными временную таблицу.
            InsertDateTempTable(builderQuery);

            string queryJoin = @"select  " + nameTempTable + ".id_договор, OUID,Tab1.Фамилия,Tab1.Имя,Tab1.Отчество,DOCUMENTSTYPE,DOCUMENTSERIES,DOCUMENTSNUMBER,ISSUEEXTENSIONSDATE,A_NAME,A_ADRTITLE,BIRTHDATE,A_SNILS from ( " +
                                " select WM_PERSONAL_CARD.OUID, SPR_FIO_SURNAME.A_NAME as Фамилия,dbo.SPR_FIO_NAME.A_NAME as 'Имя',SPR_FIO_SECONDNAME.A_NAME as 'Отчество',WM_ACTDOCUMENTS.DOCUMENTSTYPE, " +
                                " WM_ACTDOCUMENTS.DOCUMENTSERIES ,WM_ACTDOCUMENTS.DOCUMENTSNUMBER ,WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE ,PPR_DOC.A_NAME,WM_ADDRESS.A_ADRTITLE, " +
                                " WM_PERSONAL_CARD.BIRTHDATE ,dbo.WM_PERSONAL_CARD.A_SNILS, dbo.REGISTER_CONFIG.A_REGREGIONCODE from dbo.WM_PERSONAL_CARD " +
                                 " join  SPR_FIO_SURNAME " +
                                 " on WM_PERSONAL_CARD.SURNAME = SPR_FIO_SURNAME.OUID " +
                                  " join SPR_FIO_NAME " +
                                  " on SPR_FIO_NAME.OUID = WM_PERSONAL_CARD.A_NAME " +
                                  " join SPR_FIO_SECONDNAME " +
                                  " on SPR_FIO_SECONDNAME.OUID = WM_PERSONAL_CARD.A_SECONDNAME " +
                                  " join dbo.WM_ACTDOCUMENTS " +
                                  " on WM_PERSONAL_CARD.OUID = dbo.WM_ACTDOCUMENTS.PERSONOUID " +
                                  " join PPR_DOC " +
                                  " on WM_ACTDOCUMENTS.DOCUMENTSTYPE = PPR_DOC.A_ID " +
                                  " join WM_ADDRESS " +
                                  " on WM_ADDRESS.OUID = WM_PERSONAL_CARD.A_REGFLAT " +
                                  " CROSS JOIN dbo.REGISTER_CONFIG  " +
                                  " where  WM_PERSONAL_CARD.A_PCSTATUS = 1 " +
                                  " and " + nameDocPreferentCategory + " " +
                                  " ) as Tab1 " +
                                  " inner join " + nameTempTable + " " +
                                    " on LOWER(RTRIM(LTRIM(Tab1.Фамилия))) = LOWER(RTRIM(LTRIM(" + nameTempTable + ".Фамилия))) " +
                                    " and LOWER(RTRIM(LTRIM(Tab1.Имя))) = LOWER(RTRIM(LTRIM(" + nameTempTable + ".Имя))) " +
                                    " and((LOWER(RTRIM(LTRIM(Tab1.Отчество))) = LOWER(RTRIM(LTRIM(" + nameTempTable + ".Отчество))) or " + nameTempTable + ".Отчество is NULL)) " +
                                     " and CONVERT(char(10), LOWER(RTRIM(LTRIM(Tab1.BIRTHDATE))), 104) = CONVERT(char(10), LOWER(RTRIM(LTRIM(" + nameTempTable + ".ДатаРождения))), 104) " +
                                    " and LOWER(RTRIM(LTRIM(Tab1.DOCUMENTSERIES))) = LOWER(RTRIM(LTRIM(" + nameTempTable + ".СерияДокумента))) " +
                                    " and LOWER(RTRIM(LTRIM(Tab1.DOCUMENTSNUMBER))) = LOWER(RTRIM(LTRIM(" + nameTempTable + ".НомерДокумента))) " +
                                    "and CONVERT(char(10), Tab1.ISSUEEXTENSIONSDATE, 104) = CONVERT(char(10), #t1_temp.ДатаВыдачиДокумента, 104) ";
                                   // " and CONVERT(char(10), LOWER(RTRIM(LTRIM(Tab1.ISSUEEXTENSIONSDATE))), 104) = CONVERT(char(10), LOWER(RTRIM(LTRIM(" + nameTempTable + ".ДатаВыдачиДокумента))), 104) ";

            builderQuery.Append(queryJoin);

            return builderQuery.ToString();
        }
    }
}
