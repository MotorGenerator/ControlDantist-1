using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.UnpaidStatistic;
using ControlDantist.Classes;
using ControlDantist.BalanceContract;

namespace ControlDantist.Reports
{
    public class PrintReportStatistic : IPrintReport
    {
        private DateTime dt;

        private string dateStart = string.Empty;
        private string dateEnd = string.Empty;

        // Список для хранения данных отчета.
        private List<ReportBalanceStatistic> listReestr = new List<ReportBalanceStatistic>();

        public PrintReportStatistic(string dt)
        {
            this.dt = Convert.ToDateTime(dt);
        }

        private IEnumerable<ReportBalanceStatistic> ReestrStatisticAdd(List<ReportBalanceStatistic> listReestr, string hospital, string preferentCategory)
        {
            // Дата начала отчетного периода.
            dateStart = Время.Дата(this.dt.Date.ToShortDateString());

            // Дата откончания отчетного периода.
            dateEnd = Время.Дата(DateTime.Now.Date.ToShortDateString());

            // Возвращает баланс по неоплаченным договорам.
            QueryBalance qb = new QueryBalance(hospital, preferentCategory, dateStart, dateEnd);
            return qb.GenerateList();

        }

        private List<СуммаКоличествоДоговоров> GetContracts()
        {
            List<СуммаКоличествоДоговоров> listExcel = new List<СуммаКоличествоДоговоров>();

            // Получим список поликлинник.
            RepositroyrHospital repozHosp = new RepositroyrHospital();
            foreach (var hosp in repozHosp.GetListHospital())
            {
                List<ReportBalanceStatistic> items = new List<ReportBalanceStatistic>();

                СуммаКоличествоДоговоров item = new СуммаКоличествоДоговоров();
                item.НаименованиеПоликлинники = hosp.Наименование;

                // Запишем в реестр ВВС.
                var ВВС = ReestrStatisticAdd(listReestr, hosp.Наименование, PreferencCategory.ВетеранВоеннойСлужбы);

                if (ВВС.FirstOrDefault() != null)
                {
                    item.ВетераныВоеннойСлужбыКОличество = ВВС.First().КоличествоДоговоров;
                    item.ВетераныВоеннойСлужбыСумма = ВВС.First().СуммаДоговоров;
                }

                // Запишев в реестр ветеранов труда.
                var ВТ = ReestrStatisticAdd(listReestr, hosp.Наименование, PreferencCategory.ВетеранТруда);

                if (ВТ.FirstOrDefault() != null)
                {
                    item.ВетераныТрудаКоличество = ВТ.First().КоличествоДоговоров;
                    item.ВетераныТрудаСумма = ВТ.First().СуммаДоговоров;
                }

                // Запишев в реестр ветеранов труда Саратовской области.
                var ВТСО = ReestrStatisticAdd(listReestr, hosp.Наименование, PreferencCategory.ВетеранТрудаСаратовскойОбласти);

                if (ВТСО.FirstOrDefault() != null)
                {
                    item.ВетераныТрудаСаратовскойОбластиКОличество = ВТСО.First().КоличествоДоговоров;
                    item.ВетераныТрудаСаратовскойОбластиСумма = ВТСО.First().СуммаДоговоров;
                }

                // Запишев в реестр реабелитированных.
                var РЛ = ReestrStatisticAdd(listReestr, hosp.Наименование, PreferencCategory.РеабелитированныеЛица);

                if (РЛ.FirstOrDefault() != null)
                {
                    //var query = from v in ВТСО
                    //            where v != null
                    //            select v.КоличествоДоговоров;

                    item.РеабелитированныеКоличество = РЛ.First().КоличествоДоговоров;
                    item.РеабелитированныеСумма = РЛ.First().СуммаДоговоров;
                }

                // Запишев в реестр труженников тыла.
                var TT = ReestrStatisticAdd(listReestr, hosp.Наименование, PreferencCategory.ТруженикТыла);

                if (TT.FirstOrDefault() != null)
                {
                    item.ТруженникиТылаКоличество = TT.First().КоличествоДоговоров;
                    item.ТруженникиТылаСумма = TT.First().СуммаДоговоров;
                }

                items.AddRange(ВВС);
                items.AddRange(ВТ);
                items.AddRange(ВТСО);
                items.AddRange(РЛ);
                items.AddRange(TT);

                // Всего количество и сумма.
                item.ВсегоСумма = items.Sum(w => w.СуммаДоговоров);

                // Сумма количество договоров.
                item.ВсегоКоличество = items.Sum(w => w.КоличествоДоговоров);

                listExcel.Add(item);


            }

            return listExcel;
        }

        public void Print()
        {
            var listExcel = GetContracts();

            // Выведим всё в Excel.
            // Выведим на бумагу.
            GenerateExcelFile excel = new GenerateExcelFile();

            // Создадим книгу.
            excel.ExcelWorkbook();

            // Создадим лист.
            excel.ExcelWorksheet(1);


            string cellA1 = "a1";
            excel.ExcelCellsAddValueFontBold(cellA1, "Отчёт по суммам проектов договоров  в диапазоне от " + this.dateStart + " до " + this.dateEnd + " ");

            // Выведим шапку таблицы.
            excel.ExcelCellsAddValueFontBold("a3", "Наименование учреждения здравоохранения");
            excel.ExcelCellsAddValueFontBold("b3", "Ветераны труда");
            excel.ExcelCellsAddValueFontBold("c3", "Ветераны труда кол-во договоров");
            excel.ExcelCellsAddValueFontBold("d3", "Ветераны военной службы");
            excel.ExcelCellsAddValueFontBold("e3", "Ветераны военной службы - кол-во договоров");
            excel.ExcelCellsAddValueFontBold("f3", "Ветераны труда Саратовской области");
            excel.ExcelCellsAddValueFontBold("g3", "Ветераны труда Саратовской области кол-во договоров");
            excel.ExcelCellsAddValueFontBold("h3", "Труженники тыла");
            excel.ExcelCellsAddValueFontBold("i3", "Труженники тыла кол-во договоров");
            excel.ExcelCellsAddValueFontBold("j3", "Реабилитированные");
            excel.ExcelCellsAddValueFontBold("k3", "Реабилитированные кол-во договоров");
            excel.ExcelCellsAddValueFontBold("l3", "Всего");
            excel.ExcelCellsAddValueFontBold("l3", "Всего кол-во договоров");

            // Индекс элемента в списке данных для отчёта.
            int k = 0;

            // Счётчик порядкового номера.
            int count = 4;

            // Номер по порядку.
            int countPP = 1;

            foreach (var item in listExcel)
            {
                // Сгенерируем содержимое письма.
                for (int i = 1; i <= 7; i++)
                {

                    if (i == 1)
                    {
                        string cellA = "a" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellA, item.НаименованиеПоликлинники, "@", 100);

                    }

                    if (i == 2)
                    {
                        string cellB = "b" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellB, item.ВетераныТрудаСумма, "#,##0.00$", 20);

                        string cellC = "c" + count.ToString();
                        excel.ExcelCellsAddValueFontBold(cellC, item.ВетераныТрудаКоличество.ToString());//, "#,##0.00$", 20);
                    }

                    if (i == 3)
                    {
                        string cellD = "d" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellD, item.ВетераныВоеннойСлужбыСумма, "#,##0.00$", 20);

                        string cellE = "e" + count.ToString();
                        excel.ExcelCellsAddValueFontBold(cellE, item.ВетераныВоеннойСлужбыКОличество.ToString());//, "#,##0.00$", 20);
                    }

                    if (i == 4)
                    {
                        string cellF = "f" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellF, item.ВетераныТрудаСаратовскойОбластиСумма, "#,##0.00$", 20);

                        string cellG = "g" + count.ToString();
                        excel.ExcelCellsAddValueFontBold(cellG, item.ВетераныТрудаСаратовскойОбластиКОличество.ToString());
                    }

                    if (i == 5)
                    {
                        string cellH = "h" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellH, item.ТруженникиТылаСумма, "#,##0.00$", 20);

                        string cellI = "i" + count.ToString();
                        excel.ExcelCellsAddValueFontBold(cellI, item.ТруженникиТылаКоличество.ToString());
                    }

                    if (i == 6)
                    {
                        string cellJ = "j" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellJ, item.РеабелитированныеСумма, "#,##0.00$", 20);

                        string cellK = "k" + count.ToString();
                        excel.ExcelCellsAddValueFontBold(cellK, item.РеабелитированныеКоличество.ToString());
                    }


                    if (i == 7)
                    {
                        string cellL = "l" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellL, item.ВсегоСумма, "#,##0.00$", 20);

                        string cellM = "m" + count.ToString();
                        excel.ExcelCellsAddValueFontBold(cellM, item.ВсегоКоличество.ToString());
                    }


                    k++;

                }

                count++;

                countPP++;
            }

            // Выведим общее количество договоров и общую сумму.

            count++;

            string cellИтогоA = "a" + count.ToString();
            excel.ExcelCellsAddValueFormatWidthBold(cellИтогоA, "Всего :", "@", 100);


            string cellИтогоB = "b" + count.ToString();
            excel.ExcelCellsAddValueFormatWidthBold(cellИтогоB, listExcel.Sum(w => w.ВетераныТрудаСумма), "#,##0.00$", 20);

            string cellИтогоC = "c" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоC, listExcel.Sum(w => w.ВетераныТрудаКоличество));

            string cellИтогоD = "d" + count.ToString();
            excel.ExcelCellsAddValueFormatWidthBold(cellИтогоD, listExcel.Sum(w => w.ВетераныВоеннойСлужбыСумма), "#,##0.00$", 20);

            string cellИтогоE = "e" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоE, listExcel.Sum(w => w.ВетераныВоеннойСлужбыКОличество));

            string cellИтогоF = "f" + count.ToString();
            excel.ExcelCellsAddValueFormatWidthBold(cellИтогоF, listExcel.Sum(w => w.ВетераныТрудаСаратовскойОбластиСумма), "#,##0.00$", 20);

            string cellИтогоG = "g" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоG, listExcel.Sum(w => w.ВетераныТрудаСаратовскойОбластиКОличество));

            string cellИтогоH = "h" + count.ToString();
            excel.ExcelCellsAddValueFormatWidthBold(cellИтогоH, listExcel.Sum(w => w.ТруженникиТылаСумма), "#,##0.00$", 20);

            string cellИтогоI = "i" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоI, listExcel.Sum(w => w.ТруженникиТылаКоличество));

            string cellИтогоJ = "j" + count.ToString();
            excel.ExcelCellsAddValueFormatWidthBold(cellИтогоJ, listExcel.Sum(w => w.РеабелитированныеСумма), "#,##0.00$", 20);

            string cellИтогоK = "k" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоK, listExcel.Sum(w => w.РеабелитированныеКоличество));

            string cellИтогоL = "l" + count.ToString();
            excel.ExcelCellsAddValueFormatWidthBold(cellИтогоL, listExcel.Sum(w => w.ВсегоСумма), "#,##0.00$", 20);

            string cellИтогоM = "m" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоM, listExcel.Sum(w => w.ВсегоКоличество));

            count += 1;

            string cellИтогоAЛимит = "a" + count.ToString();
            excel.ExcelCellsAddValue(cellИтогоAЛимит, null);//.ExcelCellsAddValueFormatWidthBold(cellИтогоAЛимит, "", 100);

            // Ветераны военной службы.
            BalanceDisplay balanceDisplay = new BalanceDisplay();
            string sumВВС = balanceDisplay.GetLimitPreferentyCategory(DateTime.Today.Year, PreferencCategory.ВетеранВоеннойСлужбы).ToString("c");

            excel.ExcelCellsAddValueFontBold(cellИтогоAЛимит, "Лимит ВВС - " + sumВВС);

            // Ветераны труда.
            BalanceDisplay balanceDisplayВТ = new BalanceDisplay();
            string sumВТ = balanceDisplayВТ.GetLimitPreferentyCategory(DateTime.Today.Year, PreferencCategory.ВетеранТруда).ToString("c");

            count += 1;

            string cellИтогоAЛимит1 = "a" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоAЛимит1, "Лимит ВТ - " + sumВТ);

            // Ветераны труда Саратовской области.
            BalanceDisplay balanceDisplayВТСО = new BalanceDisplay();
            string sumВТСО = balanceDisplayВТСО.GetLimitPreferentyCategory(DateTime.Today.Year, PreferencCategory.ВетеранТрудаСаратовскойОбласти).ToString("c");

            count += 1;

            string cellИтогоAЛимит2 = "a" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоAЛимит2, "Лимит ВТСО - " + sumВТСО);

            // Ветераны труда Саратовской области.
            BalanceDisplay balanceDisplayРЛ = new BalanceDisplay();
            string sumРЛ = balanceDisplayРЛ.GetLimitPreferentyCategory(DateTime.Today.Year, PreferencCategory.РеабелитированныеЛица).ToString("c");

            count += 1;

            string cellИтогоAЛимит3 = "a" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоAЛимит3, "Лимит Реабелитированные лица - " + sumРЛ);

            // Ветераны труда Саратовской области.
            BalanceDisplay balanceDisplayТТ = new BalanceDisplay();
            string sumТТ = balanceDisplayТТ.GetLimitPreferentyCategory(DateTime.Today.Year, PreferencCategory.ТруженикТыла).ToString("c");

            count += 1;

            string cellИтогоAЛимит4 = "a" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоAЛимит4, "Лимит Труженники тыла - " + sumТТ);




        }

    }
}
