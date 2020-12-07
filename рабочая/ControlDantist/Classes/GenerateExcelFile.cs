using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Excel = Microsoft.Office.Interop.Excel;

namespace ControlDantist.Classes
{
    public class GenerateExcelFile
    {
        // Переменная для хранения имени файла.
        private string fileName = string.Empty;

        // Переменная приложения Excel.
        private Excel._Application excelapp;

        // Книга Excel.
        private Excel.Workbook excelappworkbook;

        // Листы книги Excel.
        private Excel.Sheets excelsheets;

        // Лист книги Excel.
        private Excel.Worksheet excelworksheet;

        // Ячейка листа Excel.
        private Excel.Range excelcells;

        public GenerateExcelFile()
        {
            excelapp = new Excel.Application();
            excelapp.Visible = true;
        }


        /// <summary>
        /// Создание книги.
        /// </summary>
        public void ExcelWorkbook()
        {
           excelappworkbook = excelapp.Workbooks.Add(Type.Missing);

            
        }


        /// <summary>
        /// Открывем лист Excel.
        /// </summary>
        /// <param name="numSheet">Номер листа</param>
        public void ExcelWorksheet(int numSheet)
        {
            // Получаем ссылку на листы книги.
            excelsheets = excelappworkbook.Worksheets;

            // Получаем ссылку на лист лист.
            excelworksheet = excelsheets.get_Item(numSheet);
        }

        

        /// <summary>
        /// Возвращает значение ячейки.
        /// </summary>
        /// <param name="адрес"></param>
        /// <returns></returns>
        public Excel.Range ExcelCellsValue(string адрес)
        {
            excelcells = excelworksheet.get_Range(адрес, Type.Missing);

            return excelcells;
        }

        /// <summary>
        /// Возвращает значение ячейки.
        /// </summary>
        /// <param name="адрес"></param>
        /// <returns></returns>
        public void ExcelCellsAddValue(string адрес, object obj)
        {
            excelcells = excelworksheet.get_Range(адрес, Type.Missing);

            /*
             * excelcells.NumberFormat = "@" - текстовый.
             * excelcells.NumberFormat = "Общий";
             * excelcells.NumberFormat="Д ММММ, ГГГГ";
             * excelcells.NumberFormat="### ##0,00"; - числовой формат
             * 
             */

            //excelcells.NumberFormat = format;

            excelcells.Value2 = obj;

        }

        /// <summary>
        /// Возвращает значение ячейки.
        /// </summary>
        /// <param name="адрес"></param>
        /// <returns></returns>
        public void ExcelCellsAddValueFormat(string адрес, object obj, object format)
        {
            excelcells = excelworksheet.get_Range(адрес, Type.Missing);

            /*
             * excelcells.NumberFormat = "@" - текстовый.
             * excelcells.NumberFormat = "Общий";
             * excelcells.NumberFormat="Д ММММ, ГГГГ";
             * excelcells.NumberFormat="### ##0,00"; - числовой формат
             * 
             */

            excelcells.NumberFormat = format;

            excelcells.Value2 = obj;

        }

        public void ExcelCellsAddValueFormatWidth(string адрес, object obj, object format, object width )
        {
            excelcells = excelworksheet.get_Range(адрес, Type.Missing);

            /*
             * excelcells.NumberFormat = "@" - текстовый.
             * excelcells.NumberFormat = "Общий";
             * excelcells.NumberFormat="Д ММММ, ГГГГ";
             * excelcells.NumberFormat="### ##0,00"; - числовой формат
             * 
             */
            excelcells.ColumnWidth = width;
            excelcells.NumberFormat = format;

            excelcells.Value2 = obj;

        }

        /// <summary>
        /// Жирный текст
        /// </summary>
        /// <param name="адрес"></param>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <param name="width"></param>
        public void ExcelCellsAddValueFormatWidthBold(string адрес, object obj, object format, object width)
        {
            excelcells = excelworksheet.get_Range(адрес, Type.Missing);

            /*
             * excelcells.NumberFormat = "@" - текстовый.
             * excelcells.NumberFormat = "Общий";
             * excelcells.NumberFormat="Д ММММ, ГГГГ";
             * excelcells.NumberFormat="### ##0,00"; - числовой формат
             * 
             */
            excelcells.ColumnWidth = width;
            excelcells.NumberFormat = format;
            excelcells.Font.Bold = 1;

            excelcells.Value2 = obj;

        }

        public void ExcelCellsAddValueFontBold(string адрес, object obj)
        {
            excelcells = excelworksheet.get_Range(адрес, Type.Missing);
            excelcells.Font.Bold = 1;

            excelcells.Value2 = obj;

        }

        /// <summary>
        /// Указывает количество строк в документе.
        /// </summary>
        /// <returns></returns>
        public int ExcelWorksheetRowsCount()
        {
            return excelworksheet.UsedRange.Rows.Count;
        }


        /// <summary>
        /// Закроем лист, книгу и Excel.
        /// </summary>
        public void ExcelClose()
        {
            // Закроем лист.
            //excelworksheet.Delete();

            // Закроем книгу.
            excelappworkbook.Close();

            // Закроем Excel.
            excelapp.Quit();

        }
    }
}
