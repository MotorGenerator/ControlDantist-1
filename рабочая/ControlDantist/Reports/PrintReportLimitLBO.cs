using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.UnpaidStatistic;
using ControlDantist.Repository;
using ControlDantist.UnpaidStatistic;
using ControlDantist.Classes;
using System.Data;
using System.Data.SqlClient;

namespace ControlDantist.Reports
{
    public class PrintReportLimitLBO : IPrintReport
    {

        private int year;

        public PrintReportLimitLBO()
        {
            this.year = DateTime.Today.Year;
        }

        private List<ItemReportLimitLBO> GetList()
        {
            // Список для хранения данных для отчета.
            List<ItemReportLimitLBO> list = new List<ItemReportLimitLBO>();

            ItemReportLimitLBO head = new ItemReportLimitLBO();
            head.Category = "Категория";
            head.Vt_BBS = "ВТ+ВВС";
            head.VtSO = "ВТСО";
            head.TT = "ТТ";
            head.Reab = "Реаб.";

            list.Add(head);

            // ЛБО на год.
            ItemReportLimitLBO lboForYear = new ItemReportLimitLBO();

            lboForYear.Category = "ЛБО на год";

            decimal vt_BBC = 0.0m;

            decimal vt = GetLimit(PreferencCategory.ВетеранТруда);
            decimal bbc = GetLimit(PreferencCategory.ВетеранВоеннойСлужбы);

            vt_BBC = Math.Round(vt + bbc, 4);

            lboForYear.Vt_BBS = vt_BBC.ToString("c");

            lboForYear.VtSO = GetLimit(PreferencCategory.ВетеранТрудаСаратовскойОбласти).ToString("c");

            lboForYear.TT = GetLimit(PreferencCategory.ТруженикТыла).ToString("c");

            lboForYear.Reab = GetLimit(PreferencCategory.РеабелитированныеЛица).ToString("c");

            list.Add(lboForYear);

            // Использовано ЛБО.
            ItemReportLimitLBO lboForYearPayment = new ItemReportLimitLBO();

            lboForYearPayment.Category = "Использовано ЛБО.";

            decimal vt_BBCP = 0.0m;

            decimal vtP = GetSumContractPaymentNoPayment(PreferencCategory.ВетеранТруда);
            decimal bbcP = GetSumContractPaymentNoPayment(PreferencCategory.ВетеранВоеннойСлужбы);

            vt_BBCP = Math.Round(vtP + bbcP, 4);

            lboForYearPayment.Vt_BBS = vt_BBCP.ToString("c");

            lboForYearPayment.VtSO = GetSumContractPaymentNoPayment(PreferencCategory.ВетеранТрудаСаратовскойОбласти).ToString("c");

            lboForYearPayment.TT = GetSumContractPaymentNoPayment(PreferencCategory.ТруженикТыла).ToString("c");

            lboForYearPayment.Reab = GetSumContractPaymentNoPayment(PreferencCategory.РеабелитированныеЛица).ToString("c");

            list.Add(lboForYearPayment);

            // Принято проектов.
            ItemReportLimitLBO принято = new ItemReportLimitLBO();

            принято.Category = "Принято проектов.";

            decimal vt_BBCП = 0.0m;

            decimal vtП = GetSumContractОжиданиеПроверки(PreferencCategory.ВетеранТруда);
            decimal bbcП = GetSumContractОжиданиеПроверки(PreferencCategory.ВетеранВоеннойСлужбы);

            vt_BBCП = Math.Round(vtП + bbcП, 4);

            принято.Vt_BBS = vt_BBCП.ToString("c");

            принято.VtSO = GetSumContractОжиданиеПроверки(PreferencCategory.ВетеранТрудаСаратовскойОбласти).ToString("c");

            принято.TT = GetSumContractОжиданиеПроверки(PreferencCategory.ТруженикТыла).ToString("c");

            принято.Reab = GetSumContractОжиданиеПроверки(PreferencCategory.РеабелитированныеЛица).ToString("c");

            list.Add(принято);

            // Остаток ЛБО.
            ItemReportLimitLBO остаток = new ItemReportLimitLBO();

            остаток.Category = "Остаток ЛБО";

            // Остаток ВТ + ВВС.
            var resВТВВС = Math.Round(vt_BBC - Math.Round(vt_BBCP + vt_BBCП, 4), 4);

            остаток.Vt_BBS = resВТВВС.ToString("c");

            // Остаток ВТСО.
            var resВТСО = Math.Round(GetLimit(PreferencCategory.ВетеранТрудаСаратовскойОбласти) - Math.Round(GetSumContractPaymentNoPayment(PreferencCategory.ВетеранТрудаСаратовскойОбласти) + GetSumContractОжиданиеПроверки(PreferencCategory.ВетеранТрудаСаратовскойОбласти), 4), 4);

            остаток.VtSO = resВТСО.ToString("c");

            // Остаток ТТ.
            var resТТ = Math.Round(GetLimit(PreferencCategory.ТруженикТыла) - Math.Round(GetSumContractPaymentNoPayment(PreferencCategory.ТруженикТыла) + GetSumContractОжиданиеПроверки(PreferencCategory.ТруженикТыла), 4), 4);

            остаток.TT = resТТ.ToString("c");

            // Остаток реабелитированные.
            // Остаток ТТ.
            var resРеабелитированные = Math.Round(GetLimit(PreferencCategory.РеабелитированныеЛица) - Math.Round(GetSumContractPaymentNoPayment(PreferencCategory.РеабелитированныеЛица) + GetSumContractОжиданиеПроверки(PreferencCategory.РеабелитированныеЛица), 4), 4);

            остаток.Reab = resРеабелитированные.ToString("c");

            list.Add(остаток);

            return list;
        }

        public void Print()
        {
            var listExcel = this.GetList();

            // Выведим всё в Excel.
            // Выведим на бумагу.
            GenerateExcelFile excel = new GenerateExcelFile();

            // Создадим книгу.
            excel.ExcelWorkbook();

            // Создадим лист.
            excel.ExcelWorksheet(1);

            string cellA1 = "a1";
            excel.ExcelCellsAddValueFontBold(cellA1, "Остатки ЛБО " + DateTime.Today.Year + " ");

            int count = 2;

            foreach (var item in listExcel)
            {
                // Сгенерируем содержимое письма.
                for (int i = 1; i <= 5; i++)
                {
                    if (i == 1)
                    {
                        string cellA = "a" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellA, item.Category, "@", 100);
                    }

                    if (i == 2)
                    {
                        string cellb = "b" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellb, item.Vt_BBS, "#,##0.00$", 20);
                    }

                    if (i == 3)
                    {
                        string cellc = "c" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellc, item.VtSO, "#,##0.00$", 20);
                    }

                    if (i == 4)
                    {
                        string celld = "d" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(celld, item.TT, "#,##0.00$", 20);
                    }

                    if (i == 5)
                    {
                        string celle = "e" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(celle, item.Reab, "#,##0.00$", 20);
                    }

                }

                count++;
            }

        }

        private decimal GetLimit(string preferentCategory)
        {
            decimal limit = 0.0m;

            UnitDate unitDate = new UnitDate();
            var groupLkLimit = unitDate.DateContext.ViewDisplayLimit.Where(w => w.Year == this.year && w.ЛьготнаяКатегория.Trim().ToLower() == preferentCategory.ToLower().Trim()).GroupBy(w => w.idLimitMoney);

            foreach (var gr in groupLkLimit)
            {
                foreach (var itm in gr)
                {
                    limit = itm.Limit;
                }
            }

            return limit;
        }

        /// <summary>
        /// Возвращает сумму оплаченных и не оплаченных.
        /// </summary>
        /// <param name="preferentCategory"></param>
        /// <returns></returns>
        private decimal GetSumContractPaymentNoPayment(string preferentCategory)
        {
            decimal sum = 0.0m;

            string query = @"select SUM(УслугиПоДоговору.Сумма) as Сумма from Договор
                            inner join УслугиПоДоговору
                            ON Договор.id_договор = УслугиПоДоговору.id_договор
                            inner join ЛьготнаяКатегория
                            ON Договор.id_льготнаяКатегория = ЛьготнаяКатегория.id_льготнойКатегории
                            where LOWER(LTRIM(RTRIM(ЛьготнаяКатегория.ЛьготнаяКатегория))) = LOWER(LTRIM(RTRIM('"+ preferentCategory +"'))) and Договор.flagАнулирован = 'false' " +
                            " and Договор.idFileRegistProgect is not null " +
                            " and Договор.flagОжиданиеПроверки = 'true' " +
                            " and((Договор.ФлагПроверки = 'true' and Договор.ФлагНаличияАкта = 'false') " +
                            " or(Договор.ФлагПроверки = 'true' and Договор.ФлагНаличияАкта = 'true')) " +
                            " and YEAR(Договор.ДатаЗаписиДоговора) = "+ this.year +" ";

            DataTable tab = ТаблицаБД.GetTableSQL(query, "selectPerson");

            if(tab.Rows.Count > 0)
            {
                if (tab.Rows[0]["Сумма"] != DBNull.Value)
                {
                    sum = Convert.ToDecimal(tab.Rows[0]["Сумма"].Do(x => x, 0.0m));
                }
            }

            return sum;
        }

        /// <summary>
        /// Возвращает сумму оплаченных и не оплаченных.
        /// </summary>
        /// <param name="preferentCategory"></param>
        /// <returns></returns>
        private decimal GetSumContractОжиданиеПроверки(string preferentCategory)
        {
            decimal sum = 0.0m;

            string query = @"select SUM(УслугиПоДоговору.Сумма) as Сумма from Договор
                            inner join УслугиПоДоговору
                            ON Договор.id_договор = УслугиПоДоговору.id_договор
                            inner join ЛьготнаяКатегория
                            ON Договор.id_льготнаяКатегория = ЛьготнаяКатегория.id_льготнойКатегории
                            where LOWER(LTRIM(RTRIM(ЛьготнаяКатегория.ЛьготнаяКатегория))) = LOWER(LTRIM(RTRIM('" + preferentCategory + "'))) and Договор.flagАнулирован = 'false' " +
                            " and Договор.idFileRegistProgect is not null " +
                            " and Договор.flagОжиданиеПроверки = 'false' " +
                            " and YEAR(Договор.ДатаЗаписиДоговора) = " + this.year + " ";

            DataTable tab = ТаблицаБД.GetTableSQL(query, "selectPerson");

            if (tab.Rows.Count > 0)
            {
                if (tab.Rows[0]["Сумма"] != DBNull.Value)
                {
                    sum = Convert.ToDecimal(tab.Rows[0]["Сумма"].Do(x => x, 0.0m));
                }
            }

            return sum;
        }


    }
}
