using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Find
{
    public class FindPersonESRN_FIO : IFindPerson
    {
        private string фамилия = string.Empty;
        private string имя = string.Empty;
        private string отчество = string.Empty;
        public FindPersonESRN_FIO(string фамилия, string имя, string отчество)
        {
            this.фамилия = фамилия;
            this.имя = имя;
            this.отчество = отчество;
        }

        public string Query()
        {
            return @"SELECT     SPR_FIO_SURNAME.A_NAME AS Фамилия,
           SPR_FIO_NAME.A_NAME AS Имя,
           SPR_FIO_SECONDNAME.A_NAME AS Отчество,
           CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 104) AS ДР,
           (
                    SELECT ISNULL(RTRIM(LTRIM(WM_PCPHONE.A_NUMBER)) + ', ', ' ') AS[text()]
 
                     FROM WM_PCPHONE
 
                     WHERE WM_PCPHONE.A_PERSCARD = WM_PERSONAL_CARD.OUID
 
                     ORDER BY WM_PCPHONE.A_PERSCARD
 
                     FOR XML PATH('')
                           ) [Телефон]

                            FROM SPR_FIO_SECONDNAME INNER JOIN
                                      WM_PERSONAL_CARD INNER JOIN
                                      SPR_FIO_SURNAME ON WM_PERSONAL_CARD.SURNAME = SPR_FIO_SURNAME.OUID ON
                                      SPR_FIO_SECONDNAME.OUID = WM_PERSONAL_CARD.A_SECONDNAME INNER JOIN
                                      SPR_FIO_NAME ON WM_PERSONAL_CARD.A_NAME = SPR_FIO_NAME.OUID


                GROUP BY WM_PERSONAL_CARD.OUID, 
                         SPR_FIO_SURNAME.A_NAME, 
                         SPR_FIO_NAME.A_NAME, 
                         SPR_FIO_SECONDNAME.A_NAME, 
                         CONVERT(char(10), WM_PERSONAL_CARD.BIRTHDATE, 104),
                         WM_PERSONAL_CARD.A_STATUS, 
                         WM_PERSONAL_CARD.A_SNILS, 
                         WM_PERSONAL_CARD.A_PCSTATUS


                HAVING(WM_PERSONAL_CARD.A_STATUS = 10) AND
                      (WM_PERSONAL_CARD.A_PCSTATUS = 1) and LTRIM(RTRIM(SPR_FIO_SURNAME.A_NAME)) = '" + this.фамилия.ToLower().Trim() + "' " +
                " and LTRIM(RTRIM(SPR_FIO_NAME.A_NAME)) = '" + this.имя.ToLower().Trim() + "' " +
                " and LTRIM(RTRIM(SPR_FIO_SECONDNAME.A_NAME)) = '" + this.отчество.ToLower().Trim() + "' " +
                " ORDER BY Фамилия";
        }
    }
}
