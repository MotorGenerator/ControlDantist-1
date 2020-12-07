using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ControlDantist.Classes;

namespace ControlDantist.UnpaidStatistic
{
    /// <summary>
    /// Баланс неоплаченных договоров по поликлинниками льготной категории.
    /// </summary>
    public class QueryBalance
    {
        public string query = string.Empty;

        //public QueryBalance(string hospital, string preferentCategor, string dateStart, string dateEnd)
        //{
        //    query = "SELECT     COUNT(Tab2.ДоговорКоличествоУслуг) AS КоличествоДоговоров, SUM(Tab2.СуммаДоговора) AS Сумма, Tab2.ЛьготнаяКатегория, " +
        //            "dbo.ПоликлинникиИнн.F2 AS НаименованиеПоликлинники " +
        //            "FROM         (SELECT     COUNT(id_договор) AS ДоговорКоличествоУслуг, SUM(Сумма) AS СуммаДоговора, ЛьготнаяКатегория, ИНН " +
        //            "FROM          (SELECT     dbo.Договор.id_договор, dbo.УслугиПоДоговору.Сумма, dbo.ЛьготнаяКатегория.ЛьготнаяКатегория, " +
        //            "dbo.Поликлинника.ИНН " +
        //            "  FROM          dbo.Договор INNER JOIN " +
        //                                                       "       dbo.УслугиПоДоговору ON dbo.Договор.id_договор = dbo.УслугиПоДоговору.id_договор INNER JOIN " +
        //                                                       " dbo.ЛьготнаяКатегория ON dbo.Договор.id_льготнаяКатегория = dbo.ЛьготнаяКатегория.id_льготнойКатегории INNER JOIN " +
        //                                                              "dbo.Поликлинника ON dbo.Поликлинника.id_поликлинника = dbo.Договор.id_поликлинника " +
        //                                       "WHERE      (dbo.Договор.ДатаЗаписиДоговора >= '" + dateStart + "') AND (dbo.Договор.ДатаЗаписиДоговора <= '" + dateEnd + "') " +
        //                                       " AND (dbo.Договор.flagОжиданиеПроверки = 'True') and (dbo.Договор.ФлагПроверки = 'True') " +
        //                                       " and (dbo.Договор.flagАнулирован = 0) and(dbo.Договор.ФлагВозвратНаДоработку = 0) " + 
        //                                       "and dbo.Договор.idFileRegistProgect > 0) AS Tab1 " +
        //               "GROUP BY id_договор, ЛьготнаяКатегория, ИНН) AS Tab2 INNER JOIN " +
        //              "dbo.ПоликлинникиИнн ON Tab2.ИНН = dbo.ПоликлинникиИнн.F3 " +
        //        "GROUP BY Tab2.ЛьготнаяКатегория, dbo.ПоликлинникиИнн.F2 " +
        //        "having LOWER(RTRIM(LTRIM(dbo.ПоликлинникиИнн.F2))) = LOWER(RTRIM(LTRIM('" + hospital + "'))) and LOWER(RTRIM(LTRIM(Tab2.ЛьготнаяКатегория))) = LOWER(RTRIM(LTRIM('" + preferentCategor + "'))) ";
        //}

        public QueryBalance(string hospital, string preferentCategor, string dateStart, string dateEnd)
        {
            query = "SELECT     COUNT(Tab2.ДоговорКоличествоУслуг) AS КоличествоДоговоров, SUM(Tab2.СуммаДоговора) AS Сумма, Tab2.ЛьготнаяКатегория, " +
                    "dbo.ПоликлинникиИнн.F2 AS НаименованиеПоликлинники " +
                    "FROM         (SELECT     COUNT(id_договор) AS ДоговорКоличествоУслуг, SUM(Сумма) AS СуммаДоговора, ЛьготнаяКатегория, ИНН " +
                    "FROM          (SELECT     dbo.ПриемРеестрвДоговор.id_договор, dbo.УслугиПоДоговору.Сумма, dbo.ЛьготнаяКатегория.ЛьготнаяКатегория, " +
                    "dbo.Поликлинника.ИНН " +
                    "  FROM          dbo.ПриемРеестрвДоговор INNER JOIN " +
                                                               "       dbo.УслугиПоДоговору ON dbo.ПриемРеестрвДоговор.id_договор = dbo.УслугиПоДоговору.id_договор INNER JOIN " +
                                                               " dbo.ЛьготнаяКатегория ON dbo.ПриемРеестрвДоговор.id_льготнаяКатегория = dbo.ЛьготнаяКатегория.id_льготнойКатегории INNER JOIN " +
                                                                      "dbo.Поликлинника ON dbo.Поликлинника.id_поликлинника = dbo.ПриемРеестрвДоговор.id_поликлинника " +
                                               "WHERE      (dbo.ПриемРеестрвДоговор.ДатаЗаписиДоговора >= '" + dateStart + "') AND (dbo.ПриемРеестрвДоговор.ДатаЗаписиДоговора <= '" + dateEnd + "') " +
                                               " AND (dbo.ПриемРеестрвДоговор.flagОжиданиеПроверки = 'True') and (dbo.ПриемРеестрвДоговор.ФлагПроверки = 'True') " +
                                               " and (dbo.ПриемРеестрвДоговор.flagАнулирован = 0) and(dbo.ПриемРеестрвДоговор.ФлагВозвратНаДоработку = 0) " +
                                               "and dbo.ПриемРеестрвДоговор.idFileRegistProgect > 0) AS Tab1 " +
                       "GROUP BY id_договор, ЛьготнаяКатегория, ИНН) AS Tab2 INNER JOIN " +
                      "dbo.ПоликлинникиИнн ON Tab2.ИНН = dbo.ПоликлинникиИнн.F3 " +
                "GROUP BY Tab2.ЛьготнаяКатегория, dbo.ПоликлинникиИнн.F2 " +
                "having LOWER(RTRIM(LTRIM(dbo.ПоликлинникиИнн.F2))) = LOWER(RTRIM(LTRIM('" + hospital + "'))) and LOWER(RTRIM(LTRIM(Tab2.ЛьготнаяКатегория))) = LOWER(RTRIM(LTRIM('" + preferentCategor + "'))) ";
        }


        /// <summary>
        /// Получает данные из БД в виде списка.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReportBalanceStatistic> GenerateList()
        {

            DataTable table = ТаблицаБД.GetTableSQL(query, "Balance");

            List<ReportBalanceStatistic> list = new List<ReportBalanceStatistic>();

            foreach (DataRow row in table.Rows)
            {
                ReportBalanceStatistic it = new ReportBalanceStatistic();
                it.ЛьготнаяКатегория = row["ЛьготнаяКатегория"].ToString();
                it.КоличествоДоговоров = Convert.ToInt32(row["КоличествоДоговоров"]);
                it.СуммаДоговоров = Convert.ToDecimal(row["Сумма"]);
                it.НаименованиеПоликлинники = row["НаименованиеПоликлинники"].ToString();

                list.Add(it);
            }

            return list;
        }

    }
}
