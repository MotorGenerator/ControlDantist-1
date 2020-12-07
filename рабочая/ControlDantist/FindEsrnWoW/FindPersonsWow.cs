using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Find;

namespace ControlDantist.FindEsrnWoW
{
    /// <summary>
    /// Поиск участиников ВОВ по БД ЭСРН.
    /// </summary>
    public class FindPersonsWow : IFindPerson
    {

        public string Query()
        {
            return @"SELECT     TOP (100) PERCENT SPR_PC_STATUS.A_NAME AS [Статус ЛД], 
                             derivedtbl_1.OUID,
                             derivedtbl_1.f AS Ф, 
                             derivedtbl_1.i AS И, 
                             derivedtbl_1.o AS О, 
                             derivedtbl_1.dr AS ДР,
                             derivedtbl_1.adr AS Адрес, 
                                    (
                                    SELECT ISNULL(RTRIM(LTRIM(WM_PCPHONE.A_NUMBER)) + ', ', ' ') AS[text()]
                                    FROM WM_PCPHONE
                                    WHERE WM_PCPHONE.A_PERSCARD = derivedtbl_1.OUID
                                    ORDER BY WM_PCPHONE.A_PERSCARD
                                    FOR XML PATH('')
                                    ) [Телефон],
                                                             PPR_DOC.A_NAME AS Документ, 
                                                             WM_ACTDOCUMENTS.DOCUMENTSERIES AS[Серия документа],
                                                             WM_ACTDOCUMENTS.DOCUMENTSNUMBER AS[Номер документа],
                                                             CONVERT(char(10), WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE, 104) AS[Дата выдачи],
                                                             CONVERT(char(10), WM_ACTDOCUMENTS.COMPLETIONSACTIONDATE, 104) AS[Дата окончания действия документа],
                                                             SPR_ORG_BASE.A_NAME1 AS[Организация, выдавшая документ],
                                                             SPR_DOC_STATUS.A_NAME AS[Статус документа],
                                                             PPR_CAT.A_NAME AS Категория,
                                                             CONVERT(char(10), WM_CATEGORY.A_DATELAST, 104) AS[Дата снятия с учета категории]
                                                             ,CONVERT(char(10), WM_PERSONAL_CARD.A_DEATHDATE, 104) AS[Дата смерти]
                                FROM SPR_ORG_BASE RIGHT OUTER JOIN
                                              WM_ACTDOCUMENTS INNER JOIN
                                                      PPR_DOC ON WM_ACTDOCUMENTS.DOCUMENTSTYPE = PPR_DOC.A_ID INNER JOIN
                                                      SPR_DOC_STATUS ON WM_ACTDOCUMENTS.A_DOCSTATUS = SPR_DOC_STATUS.A_OUID ON
                                                      SPR_ORG_BASE.OUID = WM_ACTDOCUMENTS.GIVEDOCUMENTORG RIGHT OUTER JOIN
                                                      PPR_REL_NPD_CAT INNER JOIN
                                                          (SELECT TOP (100) PERCENT WM_PERSONAL_CARD_1.OUID, SPR_FIO_SURNAME.A_NAME AS f, SPR_FIO_NAME.A_NAME AS i, 
                                                                                   SPR_FIO_SECONDNAME.A_NAME AS o, CONVERT(char(10), WM_PERSONAL_CARD_1.BIRTHDATE, 104) AS dr, WM_ADDRESS.A_ADRTITLE AS adr, 
                                                                                   WM_PERSONAL_CARD_1.A_SNILS
                                                            FROM          SPR_FIO_SECONDNAME RIGHT OUTER JOIN
                                                                                   WM_ADDRESS RIGHT OUTER JOIN
                                                                                   WM_PERSONAL_CARD AS WM_PERSONAL_CARD_1 ON WM_ADDRESS.OUID = WM_PERSONAL_CARD_1.A_REGFLAT ON
                                                                                   SPR_FIO_SECONDNAME.OUID = WM_PERSONAL_CARD_1.A_SECONDNAME LEFT OUTER JOIN
                                                                                   SPR_FIO_NAME ON WM_PERSONAL_CARD_1.A_NAME = SPR_FIO_NAME.OUID LEFT OUTER JOIN
                                                                                   SPR_FIO_SURNAME ON WM_PERSONAL_CARD_1.SURNAME = SPR_FIO_SURNAME.OUID
                                                            WHERE(WM_PERSONAL_CARD_1.A_STATUS = 10) OR
                                                                             (WM_PERSONAL_CARD_1.A_STATUS IS NULL)
                                                            GROUP BY WM_PERSONAL_CARD_1.OUID, SPR_FIO_SURNAME.A_NAME, SPR_FIO_NAME.A_NAME, SPR_FIO_SECONDNAME.A_NAME, CONVERT(char(10),
                                                                                   WM_PERSONAL_CARD_1.BIRTHDATE, 104), WM_ADDRESS.A_ADRTITLE, WM_PERSONAL_CARD_1.A_SNILS, WM_PERSONAL_CARD_1.OUID
                                                            ORDER BY f, i, o) AS derivedtbl_1 INNER JOIN
                                                      WM_PERSONAL_CARD ON derivedtbl_1.OUID = WM_PERSONAL_CARD.OUID INNER JOIN
                                                      SPR_PC_STATUS ON WM_PERSONAL_CARD.A_PCSTATUS = SPR_PC_STATUS.OUID INNER JOIN
                                                      WM_CATEGORY ON WM_PERSONAL_CARD.OUID = WM_CATEGORY.PERSONOUID ON PPR_REL_NPD_CAT.A_ID = WM_CATEGORY.A_NAME INNER JOIN
                                                      PPR_CAT ON PPR_REL_NPD_CAT.A_CAT = PPR_CAT.A_ID ON WM_ACTDOCUMENTS.PERSONOUID = WM_PERSONAL_CARD.OUID
                                WHERE      (dbo.PPR_DOC.A_CODE = 'UdoLgt69')
                                            AND(WM_PERSONAL_CARD.A_STATUS = 10)
                                            AND(dbo.WM_PERSONAL_CARD.A_DEATHDATE IS NULL
                                            or dbo.WM_PERSONAL_CARD.A_DEATHDATE >= '2020-01-01 00:00:00.000')
                                            AND(dbo.SPR_DOC_STATUS.A_CODE = 'active')
                                        --  AND(SPR_PC_STATUS.A_NAME = 'Действующее')
                                        --  AND(WM_ACTDOCUMENTS.A_STATUS = 10)
                                        --  AND(PPR_CAT.A_NAME LIKE 'Инвалид%')
                                        --    AND(WM_ACTDOCUMENTS.COMPLETIONSACTIONDATE BETWEEN '2020-04-01' AND '2020-10-01')----Срок окончания документа
                                       --AND(WM_CATEGORY.A_DATELAST BETWEEN '2020-04-01' AND '2020-10-01')---- Срок окончания категории

                                GROUP BY PPR_DOC.A_NAME, CONVERT(char(10), WM_ACTDOCUMENTS.ISSUEEXTENSIONSDATE, 104), CONVERT(char(10),
                                                   WM_ACTDOCUMENTS.COMPLETIONSACTIONDATE, 104), SPR_ORG_BASE.A_NAME1, derivedtbl_1.f, derivedtbl_1.i, derivedtbl_1.o, derivedtbl_1.dr, derivedtbl_1.adr, 
                                                      SPR_PC_STATUS.A_NAME, SPR_DOC_STATUS.A_NAME, WM_ACTDOCUMENTS.DOCUMENTSERIES, WM_ACTDOCUMENTS.DOCUMENTSNUMBER, 
                                                      WM_ACTDOCUMENTS.A_STATUS, derivedtbl_1.OUID, PPR_CAT.A_NAME
                                                      ,WM_CATEGORY.A_DATELAST, WM_PERSONAL_CARD.A_DEATHDATE

                                ORDER BY Ф, И, О";
        }

      
    }
}
