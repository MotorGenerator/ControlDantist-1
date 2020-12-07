using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    public class ValidatePrefCategory
    {
        // Поля для хранения данных о льгготнике
        private string _фамилия = string.Empty;
        private string _имя = string.Empty;
        private string _отчество = string.Empty;
        private string _датаРождения = string.Empty;
        private string _датаВыдачиДокумента = string.Empty;
        private string _серияДокумента = string.Empty;
        private string _номерДокумента = string.Empty;

        public ValidatePrefCategory(string фамилия, string имя, string отчество, string датаРождения, string датаВыдачиДокумента, string серияДокумента, string номерДокумента)
        {
            _фамилия = фамилия;
            _имя = имя;
            _отчество = отчество;
            _датаРождения = датаРождения;
            _датаВыдачиДокумента = датаВыдачиДокумента;
            _серияДокумента = серияДокумента;
            _номерДокумента = номерДокумента;
        }

        /// <summary>
        /// Поиск по всем льготникам которые есть у нас.
        /// </summary>
        /// <returns></returns>
        public string ValidateAllВВС()
        {
            StringBuilder buildInsertInto = new StringBuilder();

            string createTable = "create table #t1_temp ([id_карточки] [int] IDENTITY(1,1) NOT NULL, " +
                                 "[Фамилия] [nvarchar](50) NULL, " +
                                 "[Имя] [nvarchar](50) NULL, " +
                                 "[Отчество] [nvarchar](50) NULL, " +
                                 "[ДатаРождения] DateTime " +
                                 "[ВидЛьготногоУдостоверения] [nvarchar](50) NULL, " +
                                 "[СерияДокумента] [nvarchar](50) NULL, " +
                                 "[НомерДокумента] [nvarchar](50) NULL, " +
                                  "[ДатаВыдачиДокумента] DateTime) ";




            return buildInsertInto.ToString();
        }

        /// <summary>
        /// Возвращает строку запроса проверки ветеранов военной службы.
        /// </summary>
        /// <returns></returns>
        public string ValidateВВС()
        {

          
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
                                    "where LOWER(RTRIM(LTRIM(SPR_FIO_SURNAME.A_NAME))) = '" + _фамилия.Trim().ToLower() + "' and LOWER(LTRIM(RTRIM(SPR_FIO_NAME.A_NAME))) = '" + _имя.Trim().ToLower() + "' and LOWER(LTRIM(RTRIM(SPR_FIO_SECONDNAME.A_NAME))) = '" + _отчество.Trim().ToLower() + "' " +
                //"and RTRIM(LTRIM(CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 104))) = '" + _датаРождения.Trim() + "'  " +
                                    "and RTRIM(LTRIM(CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 112))) = '" + _датаРождения.Trim() + "' " +
                                    "and PPR_DOC.A_NAME in ('Удостоверение ветерана военной службы')  " +
                                    "and (LOWER(RTRIM(LTRIM(WM_ACTDOCUMENTS.DOCUMENTSERIES))) = '" + _серияДокумента.ToLower().Trim() + "' and LOWER(RTRIM(LTRIM(WM_ACTDOCUMENTS.DOCUMENTSNUMBER))) = '" + _номерДокумента.ToLower().Trim() + "')  " +
                                    "and (RTRIM(LTRIM(CONVERT(char(10), WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE, 112))) = '" + _датаВыдачиДокумента.Trim() + "')  " +
                                    "and WM_PERSONAL_CARD.A_PCSTATUS = 1  ";
                                    //"and PPR_DOC.A_NAME in ('Удостоверение ветерана военной службы') " +
                                    //"and (WM_ACTDOCUMENTS.DOCUMENTSERIES = '" + _серияДокумента.Trim() + "' and WM_ACTDOCUMENTS.DOCUMENTSNUMBER = '" + _номерДокумента + "') " + // -- документ
                                    //" and (CONVERT(char(10), WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE, 104) = '" + _датаВыдачиДокумента.Trim() + "')  " +
                ////"and (WM_ACTDOCUMENTS.DOCUMENTSERIES = '" + серияПаспорта + "' and WM_ACTDOCUMENTS.DOCUMENTSNUMBER = '" + номерПаспорта + "') " + //-- паспорт
                                    //"and WM_PERSONAL_CARD.A_PCSTATUS = 1 ";


            return query;
        }

        /// <summary>
        /// Возвращает строку запроса проверки ветеранов труда.
        /// </summary>
        /// <returns></returns>
        public string ValidateВТ()
        {
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
                                    "where LOWER(RTRIM(LTRIM(SPR_FIO_SURNAME.A_NAME))) = '" + _фамилия.ToLower().Trim() + "' and LOWER(LTRIM(RTRIM(SPR_FIO_NAME.A_NAME))) = '" + _имя.ToLower().Trim() + "' and LOWER(LTRIM(RTRIM(SPR_FIO_SECONDNAME.A_NAME))) = '" + _отчество.ToLower().Trim() + "' " +
                                    "and RTRIM(LTRIM(CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 112))) = '" + _датаРождения.Trim() + "'  " +
                                    "and PPR_DOC.A_NAME in ('Удостоверение ветерана труда') " +
                                    "and (LOWER(RTRIM(LTRIM(WM_ACTDOCUMENTS.DOCUMENTSERIES))) = '" + _серияДокумента.ToLower().Trim() + "' and LOWER(RTRIM(LTRIM(WM_ACTDOCUMENTS.DOCUMENTSNUMBER))) = '" + _номерДокумента.ToLower().Trim() + "') " + // -- документ
                                    "and (RTRIM(LTRIM(CONVERT(char(10), WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE, 112))) = '" + _датаВыдачиДокумента.Trim() + "')  " +
                //"and (WM_ACTDOCUMENTS.DOCUMENTSERIES = '" + серияПаспорта + "' and WM_ACTDOCUMENTS.DOCUMENTSNUMBER = '" + номерПаспорта + "') " + //-- паспорт
                                    "and WM_PERSONAL_CARD.A_PCSTATUS = 1 ";


            return query;
        }

        /// <summary>
        /// Возвращает строку запроса проверки труженников тыла.
        /// </summary>
        /// <returns></returns>
        public string ValidateТруженикТыла()
        {
            string query = "select WM_PERSONAL_CARD.OUID, SPR_FIO_SURNAME.A_NAME as Фамилия,dbo.SPR_FIO_NAME.A_NAME as Имя,SPR_FIO_SECONDNAME.A_NAME as Отчество,WM_ACTDOCUMENTS.DOCUMENTSTYPE,WM_ACTDOCUMENTS.DOCUMENTSERIES as 'Серия документа',WM_ACTDOCUMENTS.DOCUMENTSNUMBER as 'Номер документа',WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE as 'дата выдачи',PPR_DOC.A_NAME,WM_ADDRESS.A_ADRTITLE as 'Адрес',WM_PERSONAL_CARD.BIRTHDATE as 'ДатаРождения', dbo.WM_PERSONAL_CARD.A_SNILS, dbo.REGISTER_CONFIG.A_REGREGIONCODE  from dbo.WM_PERSONAL_CARD " +
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
                                    "where LOWER(RTRIM(LTRIM(SPR_FIO_SURNAME.A_NAME))) = '" + _фамилия.ToLower().Trim() + "' and LOWER(LTRIM(RTRIM(SPR_FIO_NAME.A_NAME))) = '" + _имя.ToLower().Trim() + "' and LOWER(LTRIM(RTRIM(SPR_FIO_SECONDNAME.A_NAME))) = '" + _отчество.ToLower().Trim() + "' " +
                                    "and RTRIM(LTRIM(CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 112))) = '" + _датаРождения.Trim() + "'  " +
                                    "and PPR_DOC.A_NAME in ('Удостоверение о праве на льготы (отметка - ст.20)','Удостоверение ветерана ВОВ (отметка - ст.20)') " +
                                      "and (LOWER(RTRIM(LTRIM(WM_ACTDOCUMENTS.DOCUMENTSERIES))) = '" + _серияДокумента.ToLower().Trim() + "' and LOWER(RTRIM(LTRIM(WM_ACTDOCUMENTS.DOCUMENTSNUMBER))) = '" + _номерДокумента.ToLower().Trim() + "') " + // -- документ-- документ
                                    "and (RTRIM(LTRIM(CONVERT(char(10), WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE, 112))) = '" + _датаВыдачиДокумента.Trim() + "')  " +
                //"and (WM_ACTDOCUMENTS.DOCUMENTSERIES = '" + серияПаспорта + "' and WM_ACTDOCUMENTS.DOCUMENTSNUMBER = '" + номерПаспорта + "') " + //-- паспорт
                                    "and WM_PERSONAL_CARD.A_PCSTATUS = 1 ";


            return query;
        }

        /// <summary>
        /// Проверка по ветеранм труда саратовской области.
        /// </summary>
        /// <returns></returns>
        public string ValidateВТСО()
        {
            string query = "select WM_PERSONAL_CARD.OUID, SPR_FIO_SURNAME.A_NAME as Фамилия,dbo.SPR_FIO_NAME.A_NAME as Имя,SPR_FIO_SECONDNAME.A_NAME as Отчество,WM_ACTDOCUMENTS.DOCUMENTSTYPE,WM_ACTDOCUMENTS.DOCUMENTSERIES as 'Серия документа',WM_ACTDOCUMENTS.DOCUMENTSNUMBER as 'Номер документа',WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE as 'дата выдачи',PPR_DOC.A_NAME,WM_ADDRESS.A_ADRTITLE as 'Адрес',WM_PERSONAL_CARD.BIRTHDATE as 'ДатаРождения', dbo.WM_PERSONAL_CARD.A_SNILS, dbo.REGISTER_CONFIG.A_REGREGIONCODE  from dbo.WM_PERSONAL_CARD " +
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
                                    "where LOWER(RTRIM(LTRIM(SPR_FIO_SURNAME.A_NAME))) = '" + _фамилия.ToLower().Trim() + "' and LOWER(LTRIM(RTRIM(SPR_FIO_NAME.A_NAME))) = '" + _имя.ToLower().Trim() + "' and LOWER(LTRIM(RTRIM(SPR_FIO_SECONDNAME.A_NAME))) = '" + _отчество.ToLower().Trim() + "' " +
                                    "and RTRIM(LTRIM(CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 112))) = '" + _датаРождения.Trim() + "'  " +
                                    "and PPR_DOC.A_NAME in ('Удостоверение ветерана труда Саратовской области') " +
                                    "and (LOWER(RTRIM(LTRIM(WM_ACTDOCUMENTS.DOCUMENTSERIES))) = '" + _серияДокумента.ToLower().Trim() + "' and LOWER(RTRIM(LTRIM(WM_ACTDOCUMENTS.DOCUMENTSNUMBER))) = '" + _номерДокумента.ToLower().Trim() + "') " + // -- документ
                                    "and (RTRIM(LTRIM(CONVERT(char(10), WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE, 112))) = '" + _датаВыдачиДокумента.Trim() + "')  " +
                //"and (WM_ACTDOCUMENTS.DOCUMENTSERIES = '" + серияПаспорта + "' and WM_ACTDOCUMENTS.DOCUMENTSNUMBER = '" + номерПаспорта + "') " + //-- паспорт
                                    "and WM_PERSONAL_CARD.A_PCSTATUS = 1 ";

            return query;
        }

        /// <summary>
        /// Проверка реабелитированные лица.
        /// </summary>
        /// <returns></returns>
        public string ValidateReabel()
        {
            string query = "select WM_PERSONAL_CARD.OUID, SPR_FIO_SURNAME.A_NAME as Фамилия,dbo.SPR_FIO_NAME.A_NAME as Имя,SPR_FIO_SECONDNAME.A_NAME as Отчество,WM_ACTDOCUMENTS.DOCUMENTSTYPE,WM_ACTDOCUMENTS.DOCUMENTSERIES as 'Серия документа',WM_ACTDOCUMENTS.DOCUMENTSNUMBER as 'Номер документа',WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE as 'дата выдачи',PPR_DOC.A_NAME,WM_ADDRESS.A_ADRTITLE as 'Адрес',WM_PERSONAL_CARD.BIRTHDATE as 'ДатаРождения', dbo.WM_PERSONAL_CARD.A_SNILS, dbo.REGISTER_CONFIG.A_REGREGIONCODE  from dbo.WM_PERSONAL_CARD " +
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
                                    "where LOWER(RTRIM(LTRIM(SPR_FIO_SURNAME.A_NAME))) = '" + _фамилия.ToLower().Trim() + "' and LOWER(LTRIM(RTRIM(SPR_FIO_NAME.A_NAME))) = '" + _имя.ToLower().Trim() + "' and LOWER(LTRIM(RTRIM(SPR_FIO_SECONDNAME.A_NAME))) = '" + _отчество.ToLower().Trim() + "' " +
                                      "and RTRIM(LTRIM(CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 112))) = '" + _датаРождения.Trim() + "'  " +
                                    "and PPR_DOC.A_NAME in ('Свидетельство о праве на льготы для реабилитированных лиц','Справка о реабилитации','Свидетельство о праве на льготы для лиц, признанных пострадавшими от политических репрессий','Справка о признании пострадавшим от политических репрессий') " +
                                    "and (LOWER(RTRIM(LTRIM(WM_ACTDOCUMENTS.DOCUMENTSERIES))) = '" + _серияДокумента.Do(w=>w,"").ToLower().Trim() + "' and LOWER(RTRIM(LTRIM(WM_ACTDOCUMENTS.DOCUMENTSNUMBER))) = '" + _номерДокумента.ToLower().Trim() + "') " + // -- документ
                                    "and (RTRIM(LTRIM(CONVERT(char(10), WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE, 112))) = '" + _датаВыдачиДокумента.Trim() + "')  " +
                //"and (WM_ACTDOCUMENTS.DOCUMENTSERIES = '" + серияПаспорта + "' and WM_ACTDOCUMENTS.DOCUMENTSNUMBER = '" + номерПаспорта + "') " + //-- паспорт
                                    "and WM_PERSONAL_CARD.A_PCSTATUS = 1 ";

            return query;
        }
    }
}
