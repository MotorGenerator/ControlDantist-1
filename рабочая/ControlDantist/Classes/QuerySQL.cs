using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Запрос к ЭСРН
    /// </summary>
    class QuerySQL
    {

        /// <summary>
        /// Поиск человека по базе ЭСРН
        /// </summary>
        /// <param name="фамилия">Фамилия</param>
        /// <param name="имя">Имя</param>
        /// <param name="отчество">Отчество</param>
        /// <returns></returns>
        public static string QueryЭСРН(string фамилия, string имя, string отчество)
        {
            string query = "select WM_PERSONAL_CARD.OUID, SPR_FIO_SURNAME.A_NAME as Фамилия,dbo.SPR_FIO_NAME.A_NAME as Имя,SPR_FIO_SECONDNAME.A_NAME as Отчество,WM_ACTDOCUMENTS.DOCUMENTSTYPE,WM_ACTDOCUMENTS.DOCUMENTSERIES as 'Серия документа',WM_ACTDOCUMENTS.DOCUMENTSNUMBER as 'Номер документа',WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE as 'дата выдачи',PPR_DOC.A_NAME,WM_ADDRESS.A_ADRTITLE as 'Адрес'  from dbo.WM_PERSONAL_CARD " +
                           "join  SPR_FIO_SURNAME " +
                           "on WM_PERSONAL_CARD.SURNAME = SPR_FIO_SURNAME.OUID " +
                           "join SPR_FIO_NAME " +
                           "on SPR_FIO_NAME.OUID = WM_PERSONAL_CARD.A_NAME " +
                           "join SPR_FIO_SECONDNAME " +
                           "on SPR_FIO_SECONDNAME.OUID = WM_PERSONAL_CARD.A_SECONDNAME " +
                           "join dbo.WM_ACTDOCUMENTS " +
                           "on WM_PERSONAL_CARD.OUID = dbo.WM_ACTDOCUMENTS.PERSONOUID " +
                           "join PPR_DOC " +
                           "on WM_ACTDOCUMENTS.DOCUMENTSTYPE = PPR_DOC.A_ID " +
                           "join WM_ADDRESS " +
                           "on WM_ADDRESS.OUID = WM_PERSONAL_CARD.A_REGFLAT " +
                           "where SPR_FIO_SURNAME.A_NAME = '" + фамилия.Trim() + "' and SPR_FIO_NAME.A_NAME = '" + имя.Trim() + "' and SPR_FIO_SECONDNAME.A_NAME = '" + отчество.Trim() + "' " +
                           //"and CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 104) = '" + датаРождения + "'  " +
                           "and PPR_DOC.A_NAME in ('Удостоверение ветерана труда','Удостоверение ветерана труда Саратовской области','Удостоверение о праве на льготы (отметка - ст.20)','Свидетельство о праве на льготы для реабилитированных лиц','Справка о реабилитации','Свидетельство о праве на льготы для лиц, признанных пострадавшими от политических репрессий','Справка о признании пострадавшим от политических репрессий','Удостоверение ветерана военной службы') and WM_PERSONAL_CARD.A_PCSTATUS = 1";//WM_PERSONAL_CARD.A_PCSTATUS = 1 - действующий статус

            return query;
        }


        /// <summary>
        /// Поиск человека по базе ЭСРН
        /// </summary>
        /// <param name="фамилия">Фамилия</param>
        /// <param name="имя">Имя</param>
        /// <param name="отчество">Отчество</param>
        /// <param name="датаРождения">Дата рождения</param>
        /// <returns></returns>
        public static string QueryЭСРН(string фамилия, string имя, string отчество, string датаРождения)
        {
            string query = "select WM_PERSONAL_CARD.OUID, SPR_FIO_SURNAME.A_NAME as Фамилия,dbo.SPR_FIO_NAME.A_NAME as Имя,SPR_FIO_SECONDNAME.A_NAME as Отчество,WM_ACTDOCUMENTS.DOCUMENTSTYPE,WM_ACTDOCUMENTS.DOCUMENTSERIES as 'Серия документа',WM_ACTDOCUMENTS.DOCUMENTSNUMBER as 'Номер документа',WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE as 'дата выдачи',PPR_DOC.A_NAME,WM_ADDRESS.A_ADRTITLE as 'Адрес'  from dbo.WM_PERSONAL_CARD " +
                           "join  SPR_FIO_SURNAME " +
                           "on WM_PERSONAL_CARD.SURNAME = SPR_FIO_SURNAME.OUID " +
                           "join SPR_FIO_NAME " +
                           "on SPR_FIO_NAME.OUID = WM_PERSONAL_CARD.A_NAME " +
                           "join SPR_FIO_SECONDNAME " +
                           "on SPR_FIO_SECONDNAME.OUID = WM_PERSONAL_CARD.A_SECONDNAME " +
                           "join dbo.WM_ACTDOCUMENTS " +
                           "on WM_PERSONAL_CARD.OUID = dbo.WM_ACTDOCUMENTS.PERSONOUID " +
                           "join PPR_DOC " +
                           "on WM_ACTDOCUMENTS.DOCUMENTSTYPE = PPR_DOC.A_ID " +
                           "join WM_ADDRESS " +
                           "on WM_ADDRESS.OUID = WM_PERSONAL_CARD.A_REGFLAT " +
                           "where SPR_FIO_SURNAME.A_NAME = '"+ фамилия.Trim() +"' and SPR_FIO_NAME.A_NAME = '"+ имя.Trim() +"' and SPR_FIO_SECONDNAME.A_NAME = '"+ отчество.Trim() +"' " +
                           "and CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 104) = '"+ датаРождения + "'  " +
                           "and PPR_DOC.A_NAME in ('Удостоверение ветерана труда','Удостоверение ветерана труда Саратовской области','Удостоверение о праве на льготы (отметка - ст.20)','Свидетельство о праве на льготы для реабилитированных лиц','Справка о реабилитации','Свидетельство о праве на льготы для лиц, признанных пострадавшими от политических репрессий','Справка о признании пострадавшим от политических репрессий','Удостоверение ветерана военной службы') and WM_PERSONAL_CARD.A_PCSTATUS = 1";//WM_PERSONAL_CARD.A_PCSTATUS = 1 - действующий статус

            return query;
        }



        /// <summary>
        /// Поиск человека по базе ЭСРН
        /// </summary>
        /// <param name="фамилия">Фамилия</param>
        /// <param name="имя">Имя</param>
        /// <param name="отчество">Отчество</param>
        /// <param name="датаРождения">Дата рождения</param>
        /// <param name="датаВыдачиДокумента">Дата выдачи документа дающего право на льготу</param>
        /// <returns></returns>
        public static string QueryЭСРН(string фамилия, string имя, string отчество, string датаРождения, string датаВыдачиДокумента)
        {
            string query = "select WM_PERSONAL_CARD.OUID, SPR_FIO_SURNAME.A_NAME as Фамилия,dbo.SPR_FIO_NAME.A_NAME as Имя,SPR_FIO_SECONDNAME.A_NAME as Отчество,WM_ACTDOCUMENTS.DOCUMENTSTYPE,WM_ACTDOCUMENTS.DOCUMENTSERIES as 'Серия документа',WM_ACTDOCUMENTS.DOCUMENTSNUMBER as 'Номер документа',WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE as 'дата выдачи',PPR_DOC.A_NAME,WM_ADDRESS.A_ADRTITLE as 'Адрес'  from dbo.WM_PERSONAL_CARD " +
                           "join  SPR_FIO_SURNAME " +
                           "on WM_PERSONAL_CARD.SURNAME = SPR_FIO_SURNAME.OUID " +
                           "join SPR_FIO_NAME " +
                           "on SPR_FIO_NAME.OUID = WM_PERSONAL_CARD.A_NAME " +
                           "join SPR_FIO_SECONDNAME " +
                           "on SPR_FIO_SECONDNAME.OUID = WM_PERSONAL_CARD.A_SECONDNAME " +
                           "join dbo.WM_ACTDOCUMENTS " +
                           "on WM_PERSONAL_CARD.OUID = dbo.WM_ACTDOCUMENTS.PERSONOUID " +
                           "join PPR_DOC " +
                           "on WM_ACTDOCUMENTS.DOCUMENTSTYPE = PPR_DOC.A_ID " +
                           "join WM_ADDRESS " +
                           "on WM_ADDRESS.OUID = WM_PERSONAL_CARD.A_REGFLAT " +
                           "where SPR_FIO_SURNAME.A_NAME = '" + фамилия.Trim() + "' and SPR_FIO_NAME.A_NAME = '" + имя.Trim() + "' and SPR_FIO_SECONDNAME.A_NAME = '" + отчество.Trim() + "' " +
                           "and CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 104) = '" + датаРождения + "'  " +
                           "and PPR_DOC.A_NAME in ('Удостоверение ветерана труда','Удостоверение ветерана труда Саратовской области','Удостоверение о праве на льготы (отметка - ст.20)','Свидетельство о праве на льготы для реабилитированных лиц','Справка о реабилитации','Свидетельство о праве на льготы для лиц, признанных пострадавшими от политических репрессий','Справка о признании пострадавшим от политических репрессий','Удостоверение ветерана военной службы') " +
                           "and CONVERT(char(10), WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE, 104) = '" + датаВыдачиДокумента.Trim() + "' and WM_PERSONAL_CARD.A_PCSTATUS = 1";//WM_PERSONAL_CARD.A_PCSTATUS = 1 - действующий статус // and WM_ACTDOCUMENTS.DOCUMENTSERIES = '64' and WM_ACTDOCUMENTS.DOCUMENTSNUMBER = '4106938'

            return query;
        }


        /// <summary>
        /// Поиск человека по базе ЭСРН
        /// </summary>
        /// <param name="фамилия">Фамилия</param>
        /// <param name="имя">Имя</param>
        /// <param name="отчество">Отчество</param>
        /// <param name="датаРождения">Дата рождения</param>
        /// <param name="датаВыдачиДокумента">Дата выдачи документа дающего право на льготу</param>
       /// <param name="серияДокумента">Серия документа</param>
       /// <param name="номерДокумента">Номер документа</param>
       /// <returns></returns>
        public static string QueryЭСРН(string фамилия, string имя, string отчество, string датаРождения, string датаВыдачиДокумента, string серияДокумента, string номерДокумента)
        {
            string query = "select WM_PERSONAL_CARD.OUID, SPR_FIO_SURNAME.A_NAME as Фамилия,dbo.SPR_FIO_NAME.A_NAME as Имя,SPR_FIO_SECONDNAME.A_NAME as Отчество,WM_ACTDOCUMENTS.DOCUMENTSTYPE,WM_ACTDOCUMENTS.DOCUMENTSERIES as 'Серия документа',WM_ACTDOCUMENTS.DOCUMENTSNUMBER as 'Номер документа',WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE as 'дата выдачи',PPR_DOC.A_NAME,WM_ADDRESS.A_ADRTITLE as 'Адрес'  from dbo.WM_PERSONAL_CARD " +
                           "join  SPR_FIO_SURNAME " +
                           "on WM_PERSONAL_CARD.SURNAME = SPR_FIO_SURNAME.OUID " +
                           "join SPR_FIO_NAME " +
                           "on SPR_FIO_NAME.OUID = WM_PERSONAL_CARD.A_NAME " +
                           "join SPR_FIO_SECONDNAME " +
                           "on SPR_FIO_SECONDNAME.OUID = WM_PERSONAL_CARD.A_SECONDNAME " +
                           "join dbo.WM_ACTDOCUMENTS " +
                           "on WM_PERSONAL_CARD.OUID = dbo.WM_ACTDOCUMENTS.PERSONOUID " +
                           "join PPR_DOC " +
                           "on WM_ACTDOCUMENTS.DOCUMENTSTYPE = PPR_DOC.A_ID " +
                           "join WM_ADDRESS " +
                           "on WM_ADDRESS.OUID = WM_PERSONAL_CARD.A_REGFLAT " +
                           "where SPR_FIO_SURNAME.A_NAME = '" + фамилия.Trim() + "' and SPR_FIO_NAME.A_NAME = '" + имя.Trim() + "' and SPR_FIO_SECONDNAME.A_NAME = '" + отчество.Trim() + "' " +
                           "and CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 104) = '" + датаРождения + "'  " +
                           "and PPR_DOC.A_NAME in ('Удостоверение ветерана труда','Удостоверение ветерана труда Саратовской области','Удостоверение о праве на льготы (отметка - ст.20)','Свидетельство о праве на льготы для реабилитированных лиц','Справка о реабилитации','Свидетельство о праве на льготы для лиц, признанных пострадавшими от политических репрессий','Справка о признании пострадавшим от политических репрессий','Удостоверение ветерана военной службы') " +
                           "and CONVERT(char(10), WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE, 104) = '" + датаВыдачиДокумента.Trim() + "' and WM_ACTDOCUMENTS.DOCUMENTSERIES = '" + серияДокумента + "' and WM_ACTDOCUMENTS.DOCUMENTSNUMBER = '" + номерДокумента + "' and WM_PERSONAL_CARD.A_PCSTATUS = 1";//WM_PERSONAL_CARD.A_PCSTATUS = 1 - действующий статус

            return query;
        }


        /// <summary>
        /// Поиск человека по базе ЭСРН
        /// </summary>
        /// <param name="фамилия">Фамилия</param>
        /// <param name="имя">Имя</param>
        /// <param name="отчество">Отчество</param>
        /// <param name="датаРождения">Дата рождения</param>
        /// <param name="датаВыдачиДокумента">Дата выдачи документа дающего право на льготу</param>
        /// <param name="серияДокумента">Серия документа</param>
        /// <param name="номерДокумента">Номер документа</param>
        /// <returns></returns>
        public static string QueryЭСРН(string фамилия, string имя, string отчество, string датаРождения, string датаВыдачиДокумента, string серияДокумента, string номерДокумента, string датаВыдачиПаспорта, string серияПаспорта, string номерПаспорта)
        {

            string query = "select WM_PERSONAL_CARD.OUID, SPR_FIO_SURNAME.A_NAME as Фамилия,dbo.SPR_FIO_NAME.A_NAME as Имя,SPR_FIO_SECONDNAME.A_NAME as Отчество,WM_ACTDOCUMENTS.DOCUMENTSTYPE,WM_ACTDOCUMENTS.DOCUMENTSERIES as 'Серия документа',WM_ACTDOCUMENTS.DOCUMENTSNUMBER as 'Номер документа',WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE as 'дата выдачи',PPR_DOC.A_NAME,WM_ADDRESS.A_ADRTITLE as 'Адрес',WM_PERSONAL_CARD.BIRTHDATE as 'ДатаРождения'  from dbo.WM_PERSONAL_CARD " +
                                     "join  SPR_FIO_SURNAME " +
                                     "on WM_PERSONAL_CARD.SURNAME = SPR_FIO_SURNAME.OUID " +
                                     "join SPR_FIO_NAME " +
                                     "on SPR_FIO_NAME.OUID = WM_PERSONAL_CARD.A_NAME " +
                                     "join SPR_FIO_SECONDNAME " +
                                     "on SPR_FIO_SECONDNAME.OUID = WM_PERSONAL_CARD.A_SECONDNAME " +
                                     "join dbo.WM_ACTDOCUMENTS " +
                                     "on WM_PERSONAL_CARD.OUID = dbo.WM_ACTDOCUMENTS.PERSONOUID " +
                                     "join PPR_DOC " +
                                     "on WM_ACTDOCUMENTS.DOCUMENTSTYPE = PPR_DOC.A_ID " +
                                     "join WM_ADDRESS " +
                                     "on WM_ADDRESS.OUID = WM_PERSONAL_CARD.A_REGFLAT " +
                                     "where SPR_FIO_SURNAME.A_NAME = '" + фамилия.Trim() + "' and SPR_FIO_NAME.A_NAME = '" + имя.Trim() + "' and SPR_FIO_SECONDNAME.A_NAME = '" + отчество.Trim() + "' " +
                                     "and CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 104) = '" + датаРождения + "'  " +
                                     "and PPR_DOC.A_NAME in ('Удостоверение ветерана труда','Удостоверение ветерана труда Саратовской области','Удостоверение о праве на льготы (отметка - ст.20)','Свидетельство о праве на льготы для реабилитированных лиц','Справка о реабилитации','Свидетельство о праве на льготы для лиц, признанных пострадавшими от политических репрессий','Справка о признании пострадавшим от политических репрессий','Удостоверение ветерана военной службы','Паспорт гражданина России') " +
                                     "and (WM_ACTDOCUMENTS.DOCUMENTSERIES = '" + серияДокумента + "' and WM_ACTDOCUMENTS.DOCUMENTSNUMBER = '" + номерДокумента + "') " + // -- документ
                                     " and (CONVERT(char(10), WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE, 104) = '" + датаВыдачиДокумента.Trim() + "')  " +
                                     //"and (WM_ACTDOCUMENTS.DOCUMENTSERIES = '" + серияПаспорта + "' and WM_ACTDOCUMENTS.DOCUMENTSNUMBER = '" + номерПаспорта + "') " + //-- паспорт
                                     "and WM_PERSONAL_CARD.A_PCSTATUS = 1 ";


            return query;
        }

        /// <summary>
        /// Поиск человека по базе ЭСРН по паспорту гражданина РФ
        /// </summary>
        /// <param name="фамилия">Фамилия</param>
        /// <param name="имя">Имя</param>
        /// <param name="отчество">Отчество</param>
        /// <param name="датаРождения">дата рождения</param>
        /// <param name="датаВыдачиДокумента">дата выдачи документа</param>
        /// <param name="серияДокумента">серия документа</param>
        /// <param name="номерДокумента">номер документа</param>
        /// <param name="датаВыдачиПаспорта">дата выдачи паспорта</param>
        /// <param name="серияПаспорта">серия паспорта</param>
        /// <param name="номерПаспорта">номер паспорта</param>
        /// <returns></returns>
        public static string QueryPassportЭСРН(string фамилия, string имя, string отчество, string датаРождения, string датаВыдачиДокумента, string серияДокумента, string номерДокумента, string датаВыдачиПаспорта, string серияПаспорта, string номерПаспорта)
        {
            ////Осуществляем выборку по серии и номеру паспорта
            //string query = "select WM_PERSONAL_CARD.OUID, SPR_FIO_SURNAME.A_NAME as Фамилия,dbo.SPR_FIO_NAME.A_NAME as Имя,SPR_FIO_SECONDNAME.A_NAME as Отчество,WM_ACTDOCUMENTS.DOCUMENTSTYPE,WM_ACTDOCUMENTS.DOCUMENTSERIES as 'Серия документа',WM_ACTDOCUMENTS.DOCUMENTSNUMBER as 'Номер документа',WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE as 'дата выдачи',PPR_DOC.A_NAME,WM_ADDRESS.A_ADRTITLE as 'Адрес'  from dbo.WM_PERSONAL_CARD " +
            //               "join  SPR_FIO_SURNAME " +
            //               "on WM_PERSONAL_CARD.SURNAME = SPR_FIO_SURNAME.OUID " +
            //               "join SPR_FIO_NAME " +
            //               "on SPR_FIO_NAME.OUID = WM_PERSONAL_CARD.A_NAME " +
            //               "join SPR_FIO_SECONDNAME " +
            //               "on SPR_FIO_SECONDNAME.OUID = WM_PERSONAL_CARD.A_SECONDNAME " +
            //               "join dbo.WM_ACTDOCUMENTS " +
            //               "on WM_PERSONAL_CARD.OUID = dbo.WM_ACTDOCUMENTS.PERSONOUID " +
            //               "join PPR_DOC " +
            //               "on WM_ACTDOCUMENTS.DOCUMENTSTYPE = PPR_DOC.A_ID " +
            //               "join WM_ADDRESS " +
            //               "on WM_ADDRESS.OUID = WM_PERSONAL_CARD.A_REGFLAT " +
            //               "where SPR_FIO_SURNAME.A_NAME = '" + фамилия.Trim() + "' and SPR_FIO_NAME.A_NAME = '" + имя.Trim() + "' and SPR_FIO_SECONDNAME.A_NAME = '" + отчество.Trim() + "' " +
            //               //"and CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 104) = '" + датаРождения + "'  " +
            //               "and PPR_DOC.A_NAME in ('Удостоверение ветерана труда','Удостоверение ветерана труда Саратовской области','Удостоверение о праве на льготы (отметка - ст.20)','Свидетельство о праве на льготы для реабилитированных лиц','Справка о реабилитации','Свидетельство о праве на льготы для лиц, признанных пострадавшими от политических репрессий','Справка о признании пострадавшим от политических репрессий','Удостоверение ветерана военной службы','Паспорт гражданина России') " +
            //               //"and (WM_ACTDOCUMENTS.DOCUMENTSERIES = '" + серияДокумента.Trim() + "' and WM_ACTDOCUMENTS.DOCUMENTSNUMBER = '" + номерДокумента.Trim() + "') " + // -- документ
            //               "and (WM_ACTDOCUMENTS.DOCUMENTSERIES = '" + серияПаспорта.Trim() + "' and WM_ACTDOCUMENTS.DOCUMENTSNUMBER = '" + номерПаспорта.Trim() + "') " + //-- паспорт
            //               "and WM_PERSONAL_CARD.A_PCSTATUS = 1 ";


            string query = "select WM_PERSONAL_CARD.OUID, SPR_FIO_SURNAME.A_NAME as Фамилия,dbo.SPR_FIO_NAME.A_NAME as Имя,SPR_FIO_SECONDNAME.A_NAME as Отчество,WM_ACTDOCUMENTS.DOCUMENTSTYPE,WM_ACTDOCUMENTS.DOCUMENTSERIES as 'Серия документа',WM_ACTDOCUMENTS.DOCUMENTSNUMBER as 'Номер документа',WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE as 'дата выдачи',PPR_DOC.A_NAME,WM_ADDRESS.A_ADRTITLE as 'Адрес',WM_PERSONAL_CARD.BIRTHDATE as 'ДатаРождения',dbo.WM_PERSONAL_CARD.A_SNILS, dbo.REGISTER_CONFIG.A_REGREGIONCODE  from dbo.WM_PERSONAL_CARD " +
                          "join  SPR_FIO_SURNAME " +
                          "on WM_PERSONAL_CARD.SURNAME = SPR_FIO_SURNAME.OUID " +
                          "join SPR_FIO_NAME " +
                          "on SPR_FIO_NAME.OUID = WM_PERSONAL_CARD.A_NAME " +
                          "join SPR_FIO_SECONDNAME " +
                          "on SPR_FIO_SECONDNAME.OUID = WM_PERSONAL_CARD.A_SECONDNAME " +
                          "join dbo.WM_ACTDOCUMENTS " +
                          "on WM_PERSONAL_CARD.OUID = dbo.WM_ACTDOCUMENTS.PERSONOUID " +
                          "join PPR_DOC " +
                          "on WM_ACTDOCUMENTS.DOCUMENTSTYPE = PPR_DOC.A_ID " +
                          "join WM_ADDRESS " +
                          "on WM_ADDRESS.OUID = WM_PERSONAL_CARD.A_REGFLAT " +
                          "CROSS JOIN dbo.REGISTER_CONFIG " +
                          "where LOWER(LTRIM(RTRIM(SPR_FIO_SURNAME.A_NAME))) = '" + фамилия.ToLower().Trim() + "' and LOWER(RTRIM(LTRIM(SPR_FIO_NAME.A_NAME))) = '" + имя.ToLower().Trim() + "' and LOWER(RTRIM(LTRIM(SPR_FIO_SECONDNAME.A_NAME))) = '" + отчество.ToLower().Trim() + "' " +
                //"and CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 104) = '" + датаРождения + "'  " +
                          "and PPR_DOC.A_NAME in ('Удостоверение ветерана труда','Удостоверение ветерана труда Саратовской области','Удостоверение о праве на льготы (отметка - ст.20)','Свидетельство о праве на льготы для реабилитированных лиц','Справка о реабилитации','Свидетельство о праве на льготы для лиц, признанных пострадавшими от политических репрессий','Справка о признании пострадавшим от политических репрессий','Удостоверение ветерана военной службы','Паспорт гражданина России') " +
                //"and (WM_ACTDOCUMENTS.DOCUMENTSERIES = '" + серияДокумента + "' and WM_ACTDOCUMENTS.DOCUMENTSNUMBER = '" + номерДокумента + "') " + // -- документ
                          "and (LOWER(RTRIM(LTRIM(WM_ACTDOCUMENTS.DOCUMENTSERIES))) = '" + серияПаспорта.ToLower().Trim() + "' and LOWER(RTRIM(LTRIM(WM_ACTDOCUMENTS.DOCUMENTSNUMBER))) = '" + номерПаспорта.ToLower().Trim() + "') " + //-- паспорт
                          "and (LOWER(RTRIM(LTRIM(CONVERT(char(10), WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE, 112)))) = '" + датаВыдачиПаспорта.ToLower().Trim() + "') " +
                          "and WM_PERSONAL_CARD.A_PCSTATUS = 1 ";

            return query;
        }

        


    }
}
