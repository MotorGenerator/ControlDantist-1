using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarlosAg.ExcelXmlWriter;

namespace ControlDantist.Classes
{
    public class GenerateExcelFileLetter
    {
        private string fileName = string.Empty;

        public GenerateExcelFileLetter(string filePath)
        {
            fileName = filePath;
        }


        public void GenerateFile(List<ItemLetterToMinistr> list)
        {

            //List<ItemLetterToMinistr> list = (List<ItemLetterToMinistr>)listRegistr;

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

            // Добавить Лист с некоторым данных
            Worksheet sheet = book.Worksheets.Add("Лист1");

            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));
            sheet.Table.Columns.Add(new WorksheetColumn(80));

            var ith = list.First();




            WorksheetRow rowT1 = sheet.Table.Rows.Add();

            WorksheetCell myCella = rowT1.Cells.Add("Район области");
            myCella.Index = 1;
            //cellFG.MergeAcross = 1;
            myCella.StyleID = "HeaderStyle2";

            WorksheetCell myCella2 = rowT1.Cells.Add("Населенный пункт");
            myCella2.Index = 2;
            //cellFG.MergeAcross = 1;
            myCella2.StyleID = "HeaderStyle2";

            WorksheetCell myCella3 = rowT1.Cells.Add("Фамилия");
            myCella3.Index = 3;
            //cellFG.MergeAcross = 1;
            myCella3.StyleID = "HeaderStyle2";

            WorksheetCell myCella4 = rowT1.Cells.Add("Имя");
            myCella4.Index = 4;
            //cellFG.MergeAcross = 1;
            myCella4.StyleID = "HeaderStyle2";

            WorksheetCell myCella5 = rowT1.Cells.Add("Отчество");
            myCella5.Index = 5;
            //cellFG.MergeAcross = 1;
            myCella5.StyleID = "HeaderStyle2";

            WorksheetCell myCella6 = rowT1.Cells.Add("Дата рождения");
            myCella6.Index = 6;
            //cellFG.MergeAcross = 1;
            myCella6.StyleID = "HeaderStyle2";

            WorksheetCell myCella7 = rowT1.Cells.Add("Улица");
            myCella7.Index = 7;
            //cellFG.MergeAcross = 1;
            myCella7.StyleID = "HeaderStyle2";

            WorksheetCell myCella8 = rowT1.Cells.Add("Дом");
            myCella8.Index = 8;
            //cellFG.MergeAcross = 1;
            myCella8.StyleID = "HeaderStyle2";

            WorksheetCell myCella9 = rowT1.Cells.Add("Корпус");
            myCella9.Index = 9;
            //cellFG.MergeAcross = 1;
            myCella9.StyleID = "HeaderStyle2";

            WorksheetCell myCella10 = rowT1.Cells.Add("Квартира");
            myCella10.Index = 10;
            //cellFG.MergeAcross = 1;
            myCella10.StyleID = "HeaderStyle2";

            WorksheetCell myCella11 = rowT1.Cells.Add("Серия документа");
            myCella11.Index = 11;
            //cellFG.MergeAcross = 1;
            myCella11.StyleID = "HeaderStyle2";

            WorksheetCell myCella12 = rowT1.Cells.Add("Номер документа");
            myCella12.Index = 12;
            //cellFG.MergeAcross = 1;
            myCella12.StyleID = "HeaderStyle2";

            WorksheetCell myCella13 = rowT1.Cells.Add("Сумма акта");
            myCella13.Index = 13;
            //cellFG.MergeAcross = 1;
            myCella13.StyleID = "HeaderStyle2";

            WorksheetCell myCella14 = rowT1.Cells.Add("Дата подписания акта");
            myCella14.Index = 14;
            //cellFG.MergeAcross = 1;
            myCella14.StyleID = "HeaderStyle2";



            // Заполним данными таблицу.
            //for (int i = 1; i <= 1; i++)
            foreach (var item in list)
            {
                WorksheetRow rowT = sheet.Table.Rows.Add();

                WorksheetCell myCell = rowT.Cells.Add(item.РайонОбласти);
                myCell.Index = 1;
                //cellFG.MergeAcross = 1;
                myCell.StyleID = "HeaderStyle2";

                WorksheetCell myCell2 = rowT.Cells.Add(item.НаселенныйПункт);
                myCell2.Index = 2;
                //cellFG.MergeAcross = 1;
                myCell2.StyleID = "HeaderStyle2";

                WorksheetCell myCell3 = rowT.Cells.Add(item.Фамилия);
                myCell3.Index = 3;
                //cellFG.MergeAcross = 1;
                myCell3.StyleID = "HeaderStyle2";

                WorksheetCell myCell4 = rowT.Cells.Add(item.Имя);
                myCell4.Index = 4;
                //cellFG.MergeAcross = 1;
                myCell4.StyleID = "HeaderStyle2";

                WorksheetCell myCell5 = rowT.Cells.Add(item.Отчество);
                myCell5.Index = 5;
                //cellFG.MergeAcross = 1;
                myCell5.StyleID = "HeaderStyle2";

                WorksheetCell myCell6 = rowT.Cells.Add(Convert.ToDateTime(item.ДатаРождения).ToShortDateString());
                myCell6.Index = 6;
                //cellFG.MergeAcross = 1;
                myCell6.StyleID = "HeaderStyle2";

                WorksheetCell myCell7 = rowT.Cells.Add(item.Улица);
                myCell7.Index = 7;
                //cellFG.MergeAcross = 1;
                myCell7.StyleID = "HeaderStyle2";

                WorksheetCell myCell8 = rowT.Cells.Add(item.НомерДома);
                myCell8.Index = 8;
                //cellFG.MergeAcross = 1;
                myCell8.StyleID = "HeaderStyle2";

                WorksheetCell myCell9 = rowT.Cells.Add(item.Корпус);
                myCell9.Index = 9;
                //cellFG.MergeAcross = 1;
                myCell9.StyleID = "HeaderStyle2";

                WorksheetCell myCell10 = rowT.Cells.Add(item.Квартира);
                myCell10.Index = 10;
                //cellFG.MergeAcross = 1;
                myCell10.StyleID = "HeaderStyle2";

                WorksheetCell myCell11 = rowT.Cells.Add(item.СерияДокумента);
                myCell11.Index = 11;
                //cellFG.MergeAcross = 1;
                myCell11.StyleID = "HeaderStyle2";

                WorksheetCell myCell12 = rowT.Cells.Add(item.НомерДокумента);
                myCell12.Index = 12;
                //cellFG.MergeAcross = 1;
                myCell12.StyleID = "HeaderStyle2";

                WorksheetCell myCell13 = rowT.Cells.Add(item.СуммаАкта.ToString());
                myCell13.Index = 13;
                //cellFG.MergeAcross = 1;
                myCell13.StyleID = "HeaderStyle2";

                WorksheetCell myCell14 = rowT.Cells.Add(item.ДатаАкта.ToString());
                myCell14.Index = 14;
                //cellFG.MergeAcross = 1;
                myCell14.StyleID = "HeaderStyle2";

               
            }

            book.Save(filename);

           
        }
    }
}
