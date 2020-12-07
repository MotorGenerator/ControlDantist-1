using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Reports;
using ControlDantist.Classes;
using Excel = Microsoft.Office.Interop.Excel;

namespace ControlDantist.Reports
{
    public class ReportInformToYearM : IReport
    {
        //Объект Excel
        private Microsoft.Office.Interop.Excel.Application ObjExcel;

        //объект массив excel книг
        private Microsoft.Office.Interop.Excel.Workbooks ObjWorkBooks;

        //Объект excel книга
        private Microsoft.Office.Interop.Excel.Workbook ObjWorkBook;

        //объект excel лист
        private Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet;

        public void Print(List<Repozirories.ReportYear> listData)
        {
            Microsoft.Office.Interop.Excel.Application ObjExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook ObjWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet;

            //Книга.
            ObjWorkBook = ObjExcel.Workbooks.Add(System.Reflection.Missing.Value);

            //Таблица.
            ObjWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ObjWorkBook.Sheets[1];

            //ширина первой строки документа.
            int ширПервойСтроки = 80;
            int ширинаСтроки = 50;

            //Запишем шапку
            //Объеденим ячейки
            ObjWorkSheet.get_Range("A1", "K1").Merge(Type.Missing);
            ObjWorkSheet.get_Range("A1", "K1").Font.Size = 12;
            ObjWorkSheet.get_Range("A1", "K1").Font.Bold = true;
            ObjWorkSheet.get_Range("A1", "K1").HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
            ObjWorkSheet.get_Range("A1", Type.Missing).Value2 = "Оперативная информация по вопросу бесплатного зубопртезирования на " + DateTime.Now.ToShortDateString();

            ExcelЯчейка excПП = new ExcelЯчейка();
            excПП.ГраницаЯчейки("A2", "A4", ObjWorkSheet);

            //Объеденим ячейки
            ObjWorkSheet.get_Range("B2", "B4").Merge(Type.Missing);

            // Зададим ширину колонки.
            ObjWorkSheet.get_Range("B2", "B4").ColumnWidth = 70;
            //ObjWorkSheet.get_Range("B2", "B4").RowHeight = ширинаСтроки;


            //ObjWorkSheet.get_Range("B2", "B4").RowHeight = ширПервойСтроки;
            ObjWorkSheet.get_Range("B2", Type.Missing).Value2 = "Наименование корреспондента";

            // Выровним текст по горизонтали.
            ObjWorkSheet.get_Range("B2", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            ObjWorkSheet.get_Range("B2", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Отобразим документ.
            ObjExcel.Visible = true;
            ObjExcel.UserControl = true;


        }
    }
}
