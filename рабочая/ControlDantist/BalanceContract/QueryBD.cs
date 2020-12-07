using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ControlDantist.Classes;

namespace ControlDantist.BalanceContract
{
    /// <summary>
    ///  Запрос к базе данных.
    /// </summary>
    public class QueryBD
    {
        private string query = string.Empty;

        private List<ReportBalance> listPrint;

        public QueryBD(string dateStartReports)
        {
            query = "SELECT     ЛьготнаяКатегория, COUNT(Expr1) AS [Количество договоров], SUM(Expr2) AS Сумма " +
                     "FROM         (SELECT     id_договор AS Expr1, SUM(Сумма) AS Expr2, ЛьготнаяКатегория " +
                     "FROM          (SELECT     dbo.Договор.id_договор, dbo.Договор.ДатаЗаписиДоговора, dbo.Договор.ФлагПроверки, dbo.Договор.ФлагНаличияАкта, " +
                                                                      " dbo.УслугиПоДоговору.Сумма, dbo.ЛьготнаяКатегория.ЛьготнаяКатегория " +
                                               " FROM          dbo.Договор INNER JOIN " +
                                                                      " dbo.УслугиПоДоговору ON dbo.Договор.id_договор = dbo.УслугиПоДоговору.id_договор INNER JOIN " +
                                                                      " dbo.ЛьготнаяКатегория ON dbo.Договор.id_льготнаяКатегория = dbo.ЛьготнаяКатегория.id_льготнойКатегории " +
                                               " WHERE      (dbo.Договор.ДатаЗаписиДоговора >= CONVERT(DATETIME, '"+ dateStartReports +"', 102)) AND " +
                                                                      "(dbo.Договор.ФлагПроверки = 1) AND (dbo.Договор.ФлагНаличияАкта = 0)) AS derivedtbl_1_1 " +
                       "GROUP BY ЛьготнаяКатегория, id_договор) AS derivedtbl_1 " +
                        "GROUP BY ЛьготнаяКатегория";

            listPrint = new List<ReportBalance>();
        }


        /// <summary>
        /// Получает данные из БД в виде списка.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ReportBalance> GenerateList()
        {

            DataTable table = ТаблицаБД.GetTableSQL(query, "Balance");

            List<ReportBalance> list = new List<ReportBalance>();

            foreach (DataRow row in table.Rows)
            {
                ReportBalance it = new ReportBalance();
                it.ЛьготнаяКатегория = row["ЛьготнаяКатегория"].ToString();
                it.КоличествоДоговоров = Convert.ToInt32(row["Количество договоров"]);
                it.СуммаДоговоров = Convert.ToDecimal(row["Сумма"]);

                list.Add(it);
            }

            return list;
        }

        public void GenerateReport(string dateStart, string dateEnd)
        {

            // Выведим на бумагу.
            GenerateExcelFile excel = new GenerateExcelFile();

            listPrint.AddRange(GenerateList());

            // Создадим книгу.
            excel.ExcelWorkbook();

            // Создадим лист.
            excel.ExcelWorksheet(1);

            string cellA1 = "a1";
            excel.ExcelCellsAddValueFontBold(cellA1, "Баланс по количеству и суммам договоров в диапазоне с - " + dateStart.Trim() + " до " + dateEnd.Trim());

            // Выведим шапку таблицы.
            excel.ExcelCellsAddValueFontBold("a3", "Льготная категория");
            excel.ExcelCellsAddValueFontBold("b3", "Количество догворов");
            excel.ExcelCellsAddValueFontBold("c3", "Сумма");

            // Индекс элемента в списке данных для отчёта.
            int k = 0;

            // Счётчик порядкового номера.
            int count = 4;

            // Номер по порядку.
            int countPP = 1;

            foreach (var item in listPrint)
            {
                // Сгенерируем содержимое письма.
                for (int i = 1; i <= 3; i++)
                {

                    if (i == 1)
                    {
                        string cellA = "a" + count.ToString();
                        excel.ExcelCellsAddValueFormat(cellA, item.ЛьготнаяКатегория, "@");
                    }

                    if (i == 2)
                    {
                        string cellB = "b" + count.ToString();
                        excel.ExcelCellsAddValueFormat(cellB, item.КоличествоДоговоров, "@");
                    }

                    if (i == 3)
                    {
                        string cellB = "c" + count.ToString();
                        excel.ExcelCellsAddValueFormat(cellB, item.СуммаДоговоров, "@");
                    }

                    k++;

                }

                count++;

                countPP++;
            }

            // Выведим общее количество договоров и общую сумму.

            count++;

            string cellИтого = "b" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтого, "Итого : " + listPrint.Sum(w=>w.КоличествоДоговоров).ToString());

            string cellСумма = "c" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellСумма, "Сумма : " + listPrint.Sum(w => w.СуммаДоговоров).ToString("c"));


            //// Выводим сумму по катрегориям. 
            //int j = 1;
            //foreach (var kat in comboBox1.Items)
            //{
            //    // Вместо нуля пишем пустую строку
            //    if (kat.ToString().Trim() == "0")
            //    { j++; continue; }

            //    var number = (from s in listPrint
            //                  where s.ЛьготнаяКатегория == kat.ToString()
            //                  select s.СуммаАктаВыполненныхРабот).Sum();
            //    int kolvo = listPrint.Count(p => p.ЛьготнаяКатегория == kat.ToString());

            //    string cellСумма1 = "j" + (count + j).ToString();
            //    string kolvo1 = "i" + (count + j).ToString();

            //    excel.ExcelCellsAddValueFontBold(cellСумма1, kat.ToString().Trim() + ". Сумма : " + number);
            //    excel.ExcelCellsAddValueFontBold(kolvo1, " Итого : " + kolvo);

            //    j++;
            //}
        }
    }
}
