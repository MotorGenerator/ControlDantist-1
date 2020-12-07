using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarlosAg.ExcelXmlWriter;
using ControlDantist.Reports;
using ControlDantist.Classes;


namespace ControlDantist.Reports
{
    /// <summary>
    /// Отчет Информация по бесплатному зубопротезированию 
    /// </summary>
    public class ReportInformToYear:IReport
    {

        public void Print(List<Repozirories.ReportYear> listData)
        {

            string path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

            // Путь где лежит документ.
            string fileName = path + "\\Документы\\ReportYear.xls";
            string filename = fileName;// @"D:\test.xls";
            Workbook book = new Workbook();

            // Сформируем стиль для ячеек.
            WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
            style2.Font.FontName = "Times New Roman";
            style2.Font.Size = 8;
            style2.Font.Bold = true;
            style2.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            //style2.Font.Color = "White";
            //style2.Interior.Color = "Blue";
            //style2.Interior.Pattern = StyleInteriorPattern.DiagCross;
            style2.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style2.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style2.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style2.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            style2.Alignment.WrapText = true;
            //style2.Interior.Pattern = StyleInteriorPattern.Gray75;

            // Стиль для ячеек с центрированием по левому краю.
            // Сформируем стиль для ячеек.
            WorksheetStyle styleLeft = book.Styles.Add("StyleAlignmentLeft");
            styleLeft.Font.FontName = "Times New Roman";
            styleLeft.Font.Size = 8;
            styleLeft.Font.Bold = true;
            styleLeft.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            //style2.Font.Color = "White";
            //style2.Interior.Color = "Blue";
            //style2.Interior.Pattern = StyleInteriorPattern.DiagCross;
            styleLeft.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            styleLeft.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            styleLeft.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            styleLeft.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            styleLeft.Alignment.WrapText = true;

             WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
            style3.Font.FontName = "Times New Roman";
            style3.Font.Size = 8;
            style3.Font.Bold = true;
            style3.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            style3.NumberFormat = "0,00";
            //style3.NumberFormat = "### ##0,00";
            //style2.Font.Color = "White";
            //style2.Interior.Color = "Blue";
            //style2.Interior.Pattern = StyleInteriorPattern.DiagCross;
            style3.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style3.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style3.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style3.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            style3.Alignment.WrapText = true;

           

            // Добавить Лист с некоторым данных
            Worksheet sheet = book.Worksheets.Add("Лист1");

            // Установим ширину столбцов документа.
            sheet.Table.Columns.Add(new WorksheetColumn(20));
            sheet.Table.Columns.Add(new WorksheetColumn(120));
            sheet.Table.Columns.Add(new WorksheetColumn(170));
            sheet.Table.Columns.Add(new WorksheetColumn(120));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));

            // Добавим новую строку.
            WorksheetRow rowHead = sheet.Table.Rows.Add();

            // Наименование отчета.
            WorksheetCell myCellaHead = rowHead.Cells.Add("Оперативная информация по вопросу бесплатного зубопротезирования на " + DataReport());
            myCellaHead.Index = 1;
            myCellaHead.MergeAcross = 10;
            //cellFG.MergeAcross = 1;
            myCellaHead.StyleID = "HeaderStyle2";

            // Добавим новую строку.
            WorksheetRow rowT1 = sheet.Table.Rows.Add();

            WorksheetCell myCella = rowT1.Cells.Add("№ п/п");
            myCella.Index = 1;
            myCella.MergeDown = 1;
            //cellFG.MergeAcross = 1;
            myCella.StyleID = "HeaderStyle2";

            WorksheetCell myCella2 = rowT1.Cells.Add("Район");
            myCella2.Index = 2;
            myCella2.MergeDown = 1;
            //cellFG.MergeAcross = 1;
            myCella2.StyleID = "HeaderStyle2";

            WorksheetCell myCella3 = rowT1.Cells.Add("Наименование учреждения здравоохранения");
            myCella3.Index = 3;
            myCella3.MergeDown = 1;
            //cellFG.MergeAcross = 1;
            myCella3.StyleID = "HeaderStyle2";

            WorksheetCell myCella4 = rowT1.Cells.Add("Пропускная способность за год (чел.)");
            myCella4.Index = 4;
            myCella4.MergeDown = 1;
            myCella4.StyleID = "HeaderStyle2";

            WorksheetCell myCella5 = rowT1.Cells.Add("Заключенные договоры");
            myCella5.Index = 5;
            myCella5.MergeAcross = 1;
            myCella5.StyleID = "HeaderStyle2";

            WorksheetCell myCella6 = rowT1.Cells.Add("Поступило на оплату");
            myCella6.Index = 7;
            myCella6.MergeAcross = 1;
            myCella6.StyleID = "HeaderStyle2";

            WorksheetCell myCella7 = rowT1.Cells.Add("Оплачено");
            myCella7.Index = 9;
            myCella7.MergeAcross = 1;
            myCella7.StyleID = "HeaderStyle2";

            WorksheetCell myCella8 = rowT1.Cells.Add("Лимиты на год (тыс.руб)");
            myCella8.Index = 11;
            myCella8.MergeDown = 1;
            //cellFG.MergeAcross = 1;
            myCella8.StyleID = "HeaderStyle2";

            // Строка которая частично поглащается ячейкми № 1,2,3,4,11.
            WorksheetRow rowT0 = sheet.Table.Rows.Add();
            WorksheetRow rowT11 = sheet.Table.Rows.Add();

            WorksheetCell myCella1 = rowT11.Cells.Add("1");
            myCella1.Index = 1;
            //myCella1.MergeDown = 1;
            //cellFG.MergeAcross = 1;
            myCella1.StyleID = "HeaderStyle2";

            WorksheetCell myCella21 = rowT11.Cells.Add("2");
            myCella21.Index = 2;
            //myCella21.MergeDown = 1;
            myCella21.StyleID = "HeaderStyle2";

            WorksheetCell myCella31 = rowT11.Cells.Add("3");
            myCella31.Index = 3;
            //myCella31.MergeDown = 1;
            myCella31.StyleID = "HeaderStyle2";

            WorksheetCell myCella41 = rowT11.Cells.Add("4");
            myCella41.Index = 4;
            //myCella41.MergeDown = 1;
            myCella41.StyleID = "HeaderStyle2";

            WorksheetCell myCella51 = rowT0.Cells.Add("кол-во");
            myCella51.Index = 5;
            //cellFG.MergeAcross = 1;
            myCella51.StyleID = "HeaderStyle2";

            WorksheetCell myCellT1151 = rowT11.Cells.Add("5");
            myCellT1151.Index = 5;
            //cellFG.MergeAcross = 1;
            myCellT1151.StyleID = "HeaderStyle2";

            WorksheetCell myCella61 = rowT0.Cells.Add("сумма(тыс.руб.)");
            myCella61.Index = 6;
            //cellFG.MergeAcross = 1;
            myCella61.StyleID = "HeaderStyle2";

            // Ставим внизу ячейку с номером.
            WorksheetCell myCellT1161 = rowT11.Cells.Add("6");
            myCellT1161.Index = 6;
            //cellFG.MergeAcross = 1;
            myCellT1161.StyleID = "HeaderStyle2";

            WorksheetCell myCella71 = rowT0.Cells.Add("кол-во");
            myCella71.Index = 7;
            //cellFG.MergeAcross = 1;
            myCella71.StyleID = "HeaderStyle2";

            // Ставим внизу ячейку с номером.
            WorksheetCell myCellT1171 = rowT11.Cells.Add("7");
            myCellT1171.Index = 7;
            //cellFG.MergeAcross = 1;
            myCellT1171.StyleID = "HeaderStyle2";

            WorksheetCell myCella81 = rowT0.Cells.Add("сумма(тыс.руб.)");
            myCella81.Index = 8;
            //cellFG.MergeAcross = 1;
            myCella81.StyleID = "HeaderStyle2";

            // Ставим внизу ячейку с номером.
            WorksheetCell myCellT1181 = rowT11.Cells.Add("8");
            myCellT1181.Index = 8;
            //cellFG.MergeAcross = 1;
            myCellT1181.StyleID = "HeaderStyle2";

            WorksheetCell myCella91 = rowT0.Cells.Add("кол-во");
            myCella91.Index = 9;
            //cellFG.MergeAcross = 1;
            myCella91.StyleID = "HeaderStyle2";

            // Ставим внизу ячейку с номером.
            WorksheetCell myCellT1191 = rowT11.Cells.Add("9");
            myCellT1191.Index = 9;
            //cellFG.MergeAcross = 1;
            myCellT1191.StyleID = "HeaderStyle2";

            WorksheetCell myCella101 = rowT0.Cells.Add("сумма(тыс.руб.)");
            myCella101.Index = 10;
            //cellFG.MergeAcross = 1;
            myCella101.StyleID = "HeaderStyle2";

            // Ставим внизу ячейку с номером.
            WorksheetCell myCellT11101 = rowT11.Cells.Add("10");
            myCellT11101.Index = 10;
            //cellFG.MergeAcross = 1;
            myCellT11101.StyleID = "HeaderStyle2";

            // Ставим внизу ячейку с номером.
            WorksheetCell myCellT11111 = rowT11.Cells.Add("11");
            myCellT11111.Index = 11;
            //cellFG.MergeAcross = 1;
            myCellT11111.StyleID = "HeaderStyle2";

            // Счетчик порядкового номера.
            int numCount = 1;

            // Загрузм данными таблицу.
            foreach (var item in listData)
            {
                WorksheetRow rowT = sheet.Table.Rows.Add();

                // Порядковый номер.
                WorksheetCell myCell = rowT.Cells.Add(numCount.ToString());
                myCell.Index = 1;
                myCell.StyleID = "StyleAlignmentLeft";

                // Увеличим счетчик на 1.
                numCount++;

                // Добавим к городу Саратов обозначение в виде буквы г.
                string region = item.Район;

                if (item.Район.Trim().ToLower() == "Саратов".Trim().ToLower())
                {
                    region = "г. " + item.Район;
                }
                
                // Район
                WorksheetCell myCell2 = rowT.Cells.Add(region);
                myCell2.Index = 2;
                myCell2.StyleID = "StyleAlignmentLeft";

                // Наименование учреждения здравоохранения.
                WorksheetCell myCell3 = rowT.Cells.Add(item.Поликлинника);
                myCell3.Index = 3;
                myCell3.StyleID = "StyleAlignmentLeft";

                // Пропускная способность.
                WorksheetCell myCell4 = rowT.Cells.Add(item.ПропускнаяСпособностьГод.ToString());
                myCell4.Index = 4;
                myCell4.StyleID = "HeaderStyle2";

                // КОличество заключенных договоров.
                if (item.КоличествоЗаключенныхДоговоров > 0)
                {
                    WorksheetCell myCell5 = rowT.Cells.Add(item.КоличествоЗаключенныхДоговоров.ToString());
                    myCell5.Index = 5;
                    myCell5.StyleID = "HeaderStyle3";
                }
                else
                {
                    WorksheetCell myCell5 = rowT.Cells.Add("");
                    myCell5.Index = 5;
                    myCell5.StyleID = "HeaderStyle3";
                }

                // Сумма заключенных договоров.
                if (item.СуммаЗаключенныхДоговоров > 0)
                {
                    WorksheetCell myCell6 = rowT.Cells.Add(Math.Round(item.СуммаЗаключенныхДоговоров/1000,6).ToString());
                    myCell6.Index = 6;
                    //cellFG.MergeAcross = 1;
                    myCell6.StyleID = "HeaderStyle2";
                }
                else
                {
                    WorksheetCell myCell6 = rowT.Cells.Add("");
                    myCell6.Index = 6;
                    //cellFG.MergeAcross = 1;
                    myCell6.StyleID = "HeaderStyle2";
                }

                // Количество поступивших на оплату.
                if (item.КоличествоДоговоровПоступившихНаОплату > 0)
                {
                    WorksheetCell myCell7 = rowT.Cells.Add(item.КоличествоДоговоровПоступившихНаОплату.ToString());
                    myCell7.Index = 7;
                    //cellFG.MergeAcross = 1;
                    myCell7.StyleID = "HeaderStyle2";
                }
                else
                {
                    WorksheetCell myCell7 = rowT.Cells.Add("");
                    myCell7.Index = 7;
                    //cellFG.MergeAcross = 1;
                    myCell7.StyleID = "HeaderStyle2";
                }

                // Сумма договоров поступивших на оплату.
                if (item.СуммаДоговороПоступившихНаОплату > 0)
                {
                    WorksheetCell myCell8 = rowT.Cells.Add(Math.Round(item.СуммаДоговороПоступившихНаОплату/1000,6).ToString());
                    myCell8.Index = 8;
                    //cellFG.MergeAcross = 1;
                    myCell8.StyleID = "HeaderStyle2";
                }
                else
                {
                    WorksheetCell myCell8 = rowT.Cells.Add("");
                    myCell8.Index = 8;
                    //cellFG.MergeAcross = 1;
                    myCell8.StyleID = "HeaderStyle2";
                }

                // Количество оплаченных (пока оставляем пустыми откуда брать данные пока не известно)
                WorksheetCell myCell9 = rowT.Cells.Add("");
                myCell9.Index = 9;
                myCell9.StyleID = "HeaderStyle2";

                // Сумма оплаченных договоров (пока оставляем пустыми откуда брать данные пока не известно)
                WorksheetCell myCell10 = rowT.Cells.Add("".ToString());
                myCell10.Index = 10;
                myCell10.StyleID = "HeaderStyle2";
                
                // Лимит на год тоже пока пустое.
                WorksheetCell myCell11 = rowT.Cells.Add("");
                myCell11.Index = 11;
                //cellFG.MergeAcross = 1;
                myCell11.StyleID = "HeaderStyle2";
            }

             //Выведим строку ИТОГО.
             WorksheetRow rowCount = sheet.Table.Rows.Add();

             WorksheetCell сellCount = rowCount.Cells.Add("");
             сellCount.Index = 1;
             сellCount.StyleID = "HeaderStyle2";

             WorksheetCell сellCount2 = rowCount.Cells.Add("Итого:");
             сellCount2.Index = 2;
             сellCount2.StyleID = "HeaderStyle2";

             WorksheetCell сellCount3 = rowCount.Cells.Add("");
             сellCount3.Index = 3;
             сellCount3.StyleID = "HeaderStyle2";

             WorksheetCell сellCount4 = rowCount.Cells.Add(listData.Sum(w => w.ПропускнаяСпособностьГод).ToString());
             сellCount4.Index = 4;
             сellCount4.StyleID = "HeaderStyle2";

             WorksheetCell сellCount5 = rowCount.Cells.Add(listData.Sum(w => w.КоличествоЗаключенныхДоговоров).ToString());
             сellCount5.Index = 5;
             сellCount5.StyleID = "HeaderStyle2";

             WorksheetCell сellCount6 = rowCount.Cells.Add(listData.Sum(w =>Math.Round(w.СуммаЗаключенныхДоговоров/1000,6)).ToString());
             сellCount6.Index = 6;
             сellCount6.StyleID = "HeaderStyle2";

             WorksheetCell сellCount7 = rowCount.Cells.Add(listData.Sum(w => w.КоличествоДоговоровПоступившихНаОплату).ToString());
             сellCount7.Index = 7;
             сellCount7.StyleID = "HeaderStyle2";

             WorksheetCell сellCount8 = rowCount.Cells.Add(listData.Sum(w => Math.Round(w.СуммаДоговороПоступившихНаОплату/1000,6)).ToString());
             сellCount8.Index = 8;
             сellCount8.StyleID = "HeaderStyle2";

             WorksheetCell сellCount9 = rowCount.Cells.Add("");
             сellCount9.Index = 9;
             сellCount9.StyleID = "HeaderStyle2";

             WorksheetCell сellCount10 = rowCount.Cells.Add("");
             сellCount10.Index = 10;
             //cellFG.MergeAcross = 1;
             сellCount10.StyleID = "HeaderStyle2";

            // Сумма лимита.
             decimal sumYear = 377577400;
             //decimal sumYear = 352398530;

             WorksheetCell сellCount11 = rowCount.Cells.Add(Math.Round(sumYear/1000,6).ToString());
             сellCount11.Index = 11;
             сellCount11.StyleID = "HeaderStyle2";

            try
            {

                // Сохраним файл.
                book.Save(filename);

                // Откроем файл.
                System.Diagnostics.Process.Start(filename);
            }
            catch
            {

                System.Windows.Forms.MessageBox.Show("Не могу открыть документ, вероятно такой документ уже открыт, закройте его.");
            }

        }

        /// <summary>
        /// Возвращает текущую дату.
        /// </summary>
        /// <returns></returns>
        private string DataReport()
        {
            return DateTime.Today.ToShortDateString();
        }

        
    }
}
