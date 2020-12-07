using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Repository;
using ControlDantist.Classes;
using System.Data;
using System.Data.SqlClient;

namespace ControlDantist.BalanceContract
{
    public class BalanceDisplay
    {

        /// <summary>
        /// Лимит денежных средств по льготной категории.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="namePreferencyCategory"></param>
        /// <returns></returns>
        public decimal GetLimitPreferentyCategory(int year, string namePreferencyCategory)
        {
            decimal sum = 0.0m;

            //string query = @"select Limit from ЛьготнаяКатегория
            //                inner join[dbo].[LimitPreferenceCategory]
            //                        ON ЛьготнаяКатегория.id_льготнойКатегории = [dbo].[LimitPreferenceCategory].id_льготнойКатегории
            //                inner join[dbo].[LimittMoney]
            //                ON [dbo].[LimittMoney].idLimitMoney = [dbo].[LimitPreferenceCategory].idLimitMoney
            //                inner join [Year]
            //                ON [Year].intYear = [dbo].[LimittMoney].idYear
            //                where[Year].[Year] = " + year + " and REPLACE(LTRIM(RTRIM(ЛьготнаяКатегория.ЛьготнаяКатегория)),' ','') = REPLACE(LTRIM(RTRIM('" + namePreferencyCategory + "')),' ','') ";

            string query = @"select Tab1.id_льготнойКатегории,LimitYear,SUM(СуммаДоговоров),LimitYear - SUM(СуммаДоговоров) as Limit  from(
                            select ЛьготнаяКатегория.id_льготнойКатегории, Limit as LimitYear from ЛьготнаяКатегория
                                                        inner join[dbo].[LimitPreferenceCategory]
                                                        ON ЛьготнаяКатегория.id_льготнойКатегории = [dbo].[LimitPreferenceCategory].id_льготнойКатегории
                                                        inner join[dbo].[LimittMoney]
                                                        ON[dbo].[LimittMoney].idLimitMoney = [dbo].[LimitPreferenceCategory].idLimitMoney
                                                        inner join[Year]
                                                        ON[Year].intYear = [dbo].[LimittMoney].idYear
                                                        where[Year].[Year] = " + DateTime.Today.Year + " and REPLACE(LTRIM(RTRIM(ЛьготнаяКатегория.ЛьготнаяКатегория)), ' ', '') = REPLACE(LTRIM(RTRIM('" + namePreferencyCategory + "')), ' ', '')) as Tab1 " +
                                                        " left outer join( " +
                                                        " select УслугиПоДоговору.Сумма as СуммаДоговоров,ЛьготнаяКатегория.id_льготнойКатегории from Договор " +
                                                        " inner join ЛьготнаяКатегория " +
                                                        " ON ЛьготнаяКатегория.id_льготнойКатегории = Договор.id_льготнаяКатегория " +
                                                        " inner join УслугиПоДоговору " +
                                                        " ON УслугиПоДоговору.id_договор = Договор.id_договор " +
                                                        " where YEAR(Договор.ДатаЗаписиДоговора) = "+ DateTime.Today.Year +" and idFileRegistProgect is not null " +
                                                        " and REPLACE(LTRIM(RTRIM(ЛьготнаяКатегория.ЛьготнаяКатегория)),' ','') = REPLACE(LTRIM(RTRIM('" + namePreferencyCategory + "')), ' ', '') ) as Tab2 " +
                                                        " ON Tab1.id_льготнойКатегории = Tab2.id_льготнойКатегории " +
                                                        " group by Tab1.id_льготнойКатегории,LimitYear ";

            DataTable tab1 = ТаблицаБД.GetTableSQL(query, "Limit");

            if(tab1.Rows.Count > 0)
            {
                if (tab1.Rows[0]["Limit"] != DBNull.Value)
                {
                    sum = Convert.ToDecimal(tab1.Rows[0]["Limit"]);
                }
                else
                {
                    sum = Convert.ToDecimal(tab1.Rows[0]["LimitYear"]);
                }
            }

            return sum;
        }

        /// <summary>
        /// Отображает список с лимитом денежных средсв по льготным категориям.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<ItemBalanceDisplay> CreateList(int year)
        {
            // Список для хранения и отображения льготных категорий по которым выделин лимит.
            List<ItemBalanceDisplay> list = new List<ItemBalanceDisplay>();

            UnitDate unitDate = new UnitDate();
            var groupLkLimit = unitDate.DateContext.ViewDisplayLimit.Where(w => w.Year == year).GroupBy(w => w.idLimitMoney);

            // Для теста.
            //FakeRepository fakeRepository = new FakeRepository();

            // Тест.
            //var groupLkLimit = fakeRepository.SelectFull().Where(w => w.Year == year).GroupBy(w => w.idLimitMoney);

            foreach (var gr in groupLkLimit)
            {
                ItemBalanceDisplay itemBalanceDisplay = new ItemBalanceDisplay();

                string strId = string.Empty;

                string sNameLK = string.Empty;

                decimal sum = 0.0m;

                int idKey = 0;

                int iCount = gr.Count();

                foreach (var itm in gr)
                {
                    strId += " " + itm.id_льготнойКатегории;

                    try
                    {
                        СтрокаСимвол строкаСимвол = new СтрокаСимвол();
                        sNameLK += " " + строкаСимвол.DoShortText(itm.ЛьготнаяКатегория);
                    }
                    catch(Exception ex)
                    {
                        string iTest = ex.Message;
                    }

                    //СтрокаСимвол.DoHortText(itm.ЛьготнаяКатегория);
                    sum = itm.Limit;

                    idKey = itm.idLimitMoney;
                }

                itemBalanceDisplay.Ids = strId;
                itemBalanceDisplay.NameLK = sNameLK.Trim();
                itemBalanceDisplay.SumMoney = sum.ToString();
                itemBalanceDisplay.IdLimitedMoney = idKey;

                list.Add(itemBalanceDisplay);
            }

            return list;
        }


    }
}
