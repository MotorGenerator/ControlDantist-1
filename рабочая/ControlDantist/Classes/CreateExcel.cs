using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace ControlDantist.Classes
{
    public class CreateExcel
    {
        //Объект Excel
        private Microsoft.Office.Interop.Excel.Application ObjExcel;

        //объект массив excel книг
        private Microsoft.Office.Interop.Excel.Workbooks ObjWorkBooks;

        //Объект excel книга
        private Microsoft.Office.Interop.Excel.Workbook ObjWorkBook;

        //объект excel лист
        private Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet;


        public void CreateReportHospital(Dictionary<string, StatisticaHospital> dictionary)
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
            ObjWorkSheet.get_Range("E1", "K1").Merge(Type.Missing);
            ObjWorkSheet.get_Range("E1", "E1").Font.Size = 12;
            ObjWorkSheet.get_Range("E1", "E1").Font.Bold = true;
            ObjWorkSheet.get_Range("E1", Type.Missing).Value2 = "Информация по бесплатному зубопротезированию на "+ DateTime.Now.ToShortDateString() +" по КСЗН г. Саратова";

            // Формируем таблицу.

            //Объеденим ячейки
            ObjWorkSheet.get_Range("A2", "A4").Merge(Type.Missing);
            ObjWorkSheet.get_Range("A2", Type.Missing).Value2 = "№ п.п.";
            // Выровним текст по горизонтали.
            ObjWorkSheet.get_Range("A2", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            ObjWorkSheet.get_Range("A2", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            //ObjWorkSheet.get_Range("A2", "A4").RowHeight = ширинаСтроки;

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

            // Нарисуем границу.
            ExcelЯчейка excРайон = new ExcelЯчейка();
            excРайон.ГраницаЯчейки("B2", "B4", ObjWorkSheet);

            // Количество поликлинник

            //Объеденим ячейки
            ObjWorkSheet.get_Range("C2", "C4").Merge(Type.Missing);

             //Зададим ширину колонки.
            ObjWorkSheet.get_Range("C2", "C4").ColumnWidth = 8;

            ObjWorkSheet.get_Range("C2", "C4").RowHeight = ширинаСтроки;
            ObjWorkSheet.get_Range("C2", Type.Missing).Value2 = "Количество поликлинник";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("C2", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("C2", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("C2", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excКолПол = new ExcelЯчейка();
            excКолПол.ГраницаЯчейки("C2", "C4", ObjWorkSheet);

            // Пропускная способность за год.

            //Объеденим ячейки
            ObjWorkSheet.get_Range("D2", "D4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("D2", "D4").ColumnWidth = 8;
            ObjWorkSheet.get_Range("D2", "D4").RowHeight = ширинаСтроки;
            ObjWorkSheet.get_Range("D2", Type.Missing).Value2 = "Пропускная способность за год";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("D2", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("D2", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("D2", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excПропускСпособность = new ExcelЯчейка();
            excПропускСпособность.ГраницаЯчейки("D2", "D4", ObjWorkSheet);

            //Потребность в денежных средствах за год по средней стоимости 12 тыс. руб.

            //Объеденим ячейки
            ObjWorkSheet.get_Range("E2", "E4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("E2", "E4").ColumnWidth = 8;

            ObjWorkSheet.get_Range("E2", "E4").RowHeight = ширинаСтроки;
            ObjWorkSheet.get_Range("E2", Type.Missing).Value2 = "Потребность в денежных средствах за год по средней стоимости 12 тыс. руб.";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("E2", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("E2", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("E2", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excПотребность = new ExcelЯчейка();
            excПотребность.ГраницаЯчейки("E2", "E4", ObjWorkSheet);

            // Численность граждан, состоящих на учёте.

            //Объеденим ячейки
            ObjWorkSheet.get_Range("F2", "H2").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("F2", "H2").ColumnWidth = 10;
            ObjWorkSheet.get_Range("F2", "H2").RowHeight = ширинаСтроки;
            ObjWorkSheet.get_Range("F2", Type.Missing).Value2 = "Численность льготников,состоящих на учёте.";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("F2", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("F2", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("F2", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excЧислГражд = new ExcelЯчейка();
            excЧислГражд.ГраницаЯчейки("F2", "H2", ObjWorkSheet);

            // Ячейка F3.

            //Объеденим ячейки
            ObjWorkSheet.get_Range("F3", "F4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("F3", "F4").ColumnWidth = 12;

            // Ширина строки.
            ObjWorkSheet.get_Range("F3", "F4").RowHeight = ширинаСтроки;
            ObjWorkSheet.get_Range("F3", Type.Missing).Value2 = "Всего";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("F3", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("F3", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("F3", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excОстУч2013 = new ExcelЯчейка();
            excОстУч2013.ГраницаЯчейки("F3", "F4", ObjWorkSheet);

            // Ячейка GH.

            //Объеденим ячейки
            ObjWorkSheet.get_Range("G3", "H3").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("G3", "H3").ColumnWidth = 12;

            ObjWorkSheet.get_Range("G3", "H3").RowHeight = ширинаСтроки;
            ObjWorkSheet.get_Range("G3", Type.Missing).Value2 = "в том числе";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("G3", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("G3", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("G3", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excЧислВТомЧисле = new ExcelЯчейка();
            excЧислВТомЧисле.ГраницаЯчейки("G3", "H3", ObjWorkSheet);

            // Ячейка G3.
            //Объеденим ячейки
            ObjWorkSheet.get_Range("G4", "G4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("G4", "G4").ColumnWidth = 12;
            ObjWorkSheet.get_Range("G4", Type.Missing).Value2 = "оставшиеся на учёте с 2013 г.на 01.01.2014 г.";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("G4", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("G4", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("G4", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excОстУч2014 = new ExcelЯчейка();
            excОстУч2014.ГраницаЯчейки("G4", "G4", ObjWorkSheet);


            // Ячейка H.

            //Объеденим ячейки
            ObjWorkSheet.get_Range("H4", "H4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("H4", "H4").ColumnWidth = 10;
            ObjWorkSheet.get_Range("H4", Type.Missing).Value2 = "Вставшие на учёт в 2014 г.";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("H4", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("H4", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("H4", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excЗакДог = new ExcelЯчейка();
            excЗакДог.ГраницаЯчейки("H4", "H4", ObjWorkSheet);

            // Ячейка IJ.

            //Объеденим ячейки
            ObjWorkSheet.get_Range("I2", "J2").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("I2", "J2").ColumnWidth = 10;
            ObjWorkSheet.get_Range("I2", Type.Missing).Value2 = "Договора заключёные в 2013 г.оду, по которым не произведена выплата на 01.01.2014 г.";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("I2", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("I2", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("I2", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excДоговора2013 = new ExcelЯчейка();
            excДоговора2013.ГраницаЯчейки("I2", "J2", ObjWorkSheet);


            // Ячейка H3.
            //Объеденим ячейки
            ObjWorkSheet.get_Range("I3", "I4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("I3", "I4").ColumnWidth = 12;
            ObjWorkSheet.get_Range("I3", Type.Missing).Value2 = "количество";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("I3", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("I3", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("I3", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excКол = new ExcelЯчейка();
            excКол.ГраницаЯчейки("I3", "I4", ObjWorkSheet);

            // ========Ячейка I3.
            //Объеденим ячейки
            ObjWorkSheet.get_Range("J3", "J4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("J3", "J4").ColumnWidth = 12;
            ObjWorkSheet.get_Range("J3", Type.Missing).Value2 = "сумма (тыс. руб.)";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("J3", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("J3", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("J3", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excСум = new ExcelЯчейка();
            excСум.ГраницаЯчейки("J3", "J4", ObjWorkSheet);

            // Ячейка KL.
            //Объеденим ячейки
            ObjWorkSheet.get_Range("K2", "L2").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("K2", "L2").ColumnWidth = 10;
            ObjWorkSheet.get_Range("K2", Type.Missing).Value2 = "Договора заключёные в 2014 г.";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("K2", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("K2", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("K2", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excДоговора2014= new ExcelЯчейка();
            excДоговора2014.ГраницаЯчейки("K2", "L2", ObjWorkSheet);

            // ========Ячейка I3.
            //Объеденим ячейки
            ObjWorkSheet.get_Range("K3", "K4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("K3", "K4").ColumnWidth = 12;
            ObjWorkSheet.get_Range("K3", Type.Missing).Value2 = "количество";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("K3", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("K3", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("K3", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excK3 = new ExcelЯчейка();
            excK3.ГраницаЯчейки("K3", "K4", ObjWorkSheet);

            //== Ячейка L.
            //Объеденим ячейки
            ObjWorkSheet.get_Range("L3", "L4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("L3", "L4").ColumnWidth = 12;
            ObjWorkSheet.get_Range("L3", Type.Missing).Value2 = "сумма (тыс. руб.)";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("L3", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("L3", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("L3", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excL3 = new ExcelЯчейка();
            excL3.ГраницаЯчейки("L3", "L4", ObjWorkSheet);

            // Ячейка M-R
            //Объеденим ячейки
            ObjWorkSheet.get_Range("M2", "R2").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("M2", "R2").ColumnWidth = 12;
            ObjWorkSheet.get_Range("M2", Type.Missing).Value2 = "Выплачено в 2014 г.";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("M2", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("M2", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("M2", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excM3R = new ExcelЯчейка();
            excM3R.ГраницаЯчейки("M2", "R2", ObjWorkSheet);

            // Ячейка M3
            //Объеденим ячейки
            ObjWorkSheet.get_Range("M3", "M4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("M3", "M4").ColumnWidth = 12;
            ObjWorkSheet.get_Range("M3", Type.Missing).Value2 = "Всего количество договоров";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("M3", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("M3", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("M3", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excM3 = new ExcelЯчейка();
            excM3.ГраницаЯчейки("M3", "M4", ObjWorkSheet);

            // Ячейка NO
            //Объеденим ячейки
            ObjWorkSheet.get_Range("N3", "O3").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("N3", "O3").ColumnWidth = 12;
            ObjWorkSheet.get_Range("N3", Type.Missing).Value2 = "в том числе";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("N3", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("N3", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("N3", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excN3 = new ExcelЯчейка();
            excN3.ГраницаЯчейки("N3", "O3", ObjWorkSheet);

            // Ячейка N4
            //Объеденим ячейки
            ObjWorkSheet.get_Range("N4", "N4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("N4", "N4").ColumnWidth = 12;
            ObjWorkSheet.get_Range("N4", Type.Missing).Value2 = "за 2013";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("N4", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("N4", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("N4", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excN34 = new ExcelЯчейка();
            excN34.ГраницаЯчейки("N4", "N4", ObjWorkSheet);

            // Ячейка O4
            //Объеденим ячейки
            ObjWorkSheet.get_Range("O4", "O4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("O4", "O4").ColumnWidth = 12;
            ObjWorkSheet.get_Range("O4", Type.Missing).Value2 = "за 2014";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("O4", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("O4", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("O4", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excO4 = new ExcelЯчейка();
            excO4.ГраницаЯчейки("O4", "O4", ObjWorkSheet);

            // Ячейка P4
            //Объеденим ячейки
            ObjWorkSheet.get_Range("P3", "P4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("P3", "P4").ColumnWidth = 12;
            ObjWorkSheet.get_Range("P3", Type.Missing).Value2 = "Всего выплаченная сумма (тыс. руб.)";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("P3", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("P3", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("P3", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excP3 = new ExcelЯчейка();
            excP3.ГраницаЯчейки("P3", "P4", ObjWorkSheet);

            // Ячейка QR3
            //Объеденим ячейки
            ObjWorkSheet.get_Range("Q3", "R3").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("Q3", "R3").ColumnWidth = 12;
            ObjWorkSheet.get_Range("Q3", Type.Missing).Value2 = "в том числе";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("Q3", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("Q3", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("Q3", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excQR3 = new ExcelЯчейка();
            excQR3.ГраницаЯчейки("Q3", "R3", ObjWorkSheet);

            // Ячейка q2013
            //Объеденим ячейки
            ObjWorkSheet.get_Range("Q4", "Q4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("Q4", "Q4").ColumnWidth = 12;
            ObjWorkSheet.get_Range("Q4", Type.Missing).Value2 = "за 2013";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("Q4", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("Q4", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("Q4", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excQ4 = new ExcelЯчейка();
            excQ4.ГраницаЯчейки("Q4", "Q4", ObjWorkSheet);

            // Ячейка R2014
            //Объеденим ячейки
            ObjWorkSheet.get_Range("R4", "R4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("R4", "R4").ColumnWidth = 12;
            ObjWorkSheet.get_Range("R4", Type.Missing).Value2 = "за 2014";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("R4", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("R4", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("R4", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excR4 = new ExcelЯчейка();
            excR4.ГраницаЯчейки("R4", "R4", ObjWorkSheet);

            // Ячейка S
            //Объеденим ячейки
            ObjWorkSheet.get_Range("S2", "S4").Merge(Type.Missing);

            //Зададим ширину колонки.
            ObjWorkSheet.get_Range("S2", "S4").ColumnWidth = 12;
            ObjWorkSheet.get_Range("S2", Type.Missing).Value2 = "Лимиты 2014 года тыс. руб.";

            // выровним горизонтально.
            ObjWorkSheet.get_Range("S2", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            // выровним вертикально.
            ObjWorkSheet.get_Range("S2", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            // Перенос текста.
            ObjWorkSheet.get_Range("S2", Type.Missing).WrapText = true;

            // Нарисуем границу.
            ExcelЯчейка excS2 = new ExcelЯчейка();
            excS2.ГраницаЯчейки("S2", "S4", ObjWorkSheet);



            // Запишем нумерацию столбцов.
            for (int i = 1; i <= 19; i++)
            {
                string exclБукв = ExcelЯчейка.БукваКолонка(i);


                //Объеденим ячейки
                ObjWorkSheet.get_Range(exclБукв + "5", exclБукв + "5").Merge(Type.Missing);

                //Зададим ширину колонки.
                if (i != 2)
                {
                    ObjWorkSheet.get_Range(exclБукв + "5", exclБукв + "5").ColumnWidth = 14;
                }
                else
                {
                    ObjWorkSheet.get_Range(exclБукв + "5", exclБукв + "5").ColumnWidth = 70;
                }
                ObjWorkSheet.get_Range(exclБукв + "5", Type.Missing).Value2 = i;

                // выровним горизонтально.
                ObjWorkSheet.get_Range(exclБукв + "5", Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
                // выровним вертикально.
                ObjWorkSheet.get_Range(exclБукв + "5", Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

                // Перенос текста.
                ObjWorkSheet.get_Range(exclБукв + "5", Type.Missing).WrapText = true;

                // Нарисуем границу.
                ExcelЯчейка excNum = new ExcelЯчейка();
                excNum.ГраницаЯчейки(exclБукв + "5", exclБукв + "5", ObjWorkSheet);
            }

            //int iCount = dictionary.Count;

            // Счётчик циклов. Начнём с 5 потому что содержание таблица начинается с 5 строки.
            int iCount = 5;

            foreach (StatisticaHospital item in dictionary.Values)
            {
                if (item.Поликлинника != "ГУЗ  «Городская поликлиника №20»")
                {
                    //CreateItemExcel(iCount, item);
                    for (int i = 1; i <= 19; i++)
                    {

                        string exclБукв = ExcelЯчейка.БукваКолонка(i);

                        //Объеденим ячейки
                        ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), exclБукв + iCount.ToString()).Merge(Type.Missing);

                        //Зададим ширину колонки.
                        if (i != 2)
                        {
                            ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), exclБукв + iCount.ToString()).ColumnWidth = 14;
                        }
                        else
                        {
                            ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), exclБукв + iCount.ToString()).ColumnWidth = 70;
                        }

                        switch (i)
                        {
                            case 1:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.НомерПП;
                                break;
                            case 2:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.Поликлинника;
                                break;
                            case 3:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.КоличествоПоликлинник;
                                break;
                            case 4:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ПропускнаяСпособность;
                                break;
                            case 5:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ПотребностьДенежныхсредств;
                                break;
                            // Ячейка F
                            case 6:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ЧисленностьЛьготниковСостоящихНаУчётеВсего;
                                break;
                            // Ячейка G
                            case 7:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ЧисленностьГражданОставшихсяНаУчёте2013;
                                break;
                            // Ячейка H.
                            case 8:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ЧисленностьГражданВставшиеНаУчёте2014;
                                break;
                            // Ячейка I
                            case 9:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ЗаключённыеДоговораНеОплатаКоличество2013;
                                break;
                            // Ячейка J.
                            case 10:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ЗаключённыеДоговораНеОплатаСумма2013;
                                break;
                            // Ячейка K.
                            case 11:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ДоговораЗаключённыеКоличество2014;
                                break;
                            // Ячейка L.
                            case 12:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ДоговораЗаключённыеСумма2014;
                                break;
                            // Ячейка M.
                            case 13:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ВсегоКоличествоДоговоров;
                                break;
                            // Ячейка N.
                            case 14:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ВсегоКоличествоДоговоровЗа2013;
                                break;
                            // Ячейка O
                            case 15:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ВсегоКоличествоДоговоровЗа2014;
                                break;
                            // Ячейка P.
                            case 16:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ВсегоВыплачено;
                                break;
                            // Ячейка Q.
                            case 17:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.СуммаВыплаченнаяПоДоговорам2013;
                                break;
                            // Ячейка R.
                            case 18:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.СуммаВыплаченнаяПоДоговорам2014;
                                break;
                            // Ячейка S.
                            case 19:
                                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.Лимиты2014;
                                break;

                        }

                        // выровним горизонтально.
                        ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
                        // выровним вертикально.
                        ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

                        // Перенос текста.
                        ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).WrapText = true;

                        // Нарисуем границу.
                        ExcelЯчейка excNum = new ExcelЯчейка();
                        excNum.ГраницаЯчейки(exclБукв + iCount.ToString(), exclБукв + iCount.ToString(), ObjWorkSheet);
                    }
                }

                iCount++;
            }

            #region Старая реализация заполнения документа Excel.

            //// Запишем данные из библиотеки.
            //// Объявим двумерный массив для хранения данных.
            //string[,] muArray = new string[21, 16];

            //int countItem = 0;

            //int k = 1;
            //foreach (StatisticaHospital item in dictionary.Values)
            //{
            //    if (item.Поликлинника != "ГУЗ  «Городская поликлиника №20»")
            //    {
            //        muArray[countItem, 0] = (countItem + 1).ToString();
            //        muArray[countItem, 1] = item.Поликлинника.ToString().Trim();
            //        muArray[countItem, 2] = item.КоличествоПоликлинник.ToString().Trim();
            //        muArray[countItem, 3] = item.ПропускнаяСпособность.ToString();
            //        muArray[countItem, 4] = item.ПотребностьДенежныхсредств.ToString();
            //        muArray[countItem, 5] = item.ЧисленностьГражданОставшихсяНаУчёте2013.ToString().Trim();
            //        muArray[countItem, 6] = item.ЧисленностьГражданВставшиеНаУчёте2014.ToString().Trim();
            //        //muArray[countItem, 7] = item.ЗаключённыеДоговораКоличество2013.ToString().Trim();
            //        //muArray[countItem, 8] = item.ЗаключённыеДоговораСумма2013.ToString().Trim();
            //        muArray[countItem, 9] = item.ЗаключённыеДоговораНеОплатаКоличество2013.ToString().Trim();
            //        muArray[countItem, 10] = item.ЗаключённыеДоговораНеОплатаСумма2013.ToString();
            //        muArray[countItem, 11] = item.ДоговораЗаключённыеКоличество2014.ToString().Trim();
            //        string test = Math.Round(item.ДоговораЗаключённыеСумма2014, 2).ToString().Replace(".", ",").Trim();
            //        muArray[countItem, 12] = test;
            //        //decimal test = item.ДоговораЗаключённыеСумма2014;//.ToString().Replace(".", ",").Trim();
            //        //muArray[countItem, 12] = test;//.ToString();
            //        string sum = Math.Round(item.СуммаВыплаченнаяПоДоговорам2013, 2).ToString().Replace(".", ",").Trim();
            //        muArray[countItem, 13] = sum;
            //        string sum2014 = Math.Round(item.СуммаВыплаченнаяПоДоговорам2014, 2).ToString().Replace(".", ",").Trim();
            //        muArray[countItem, 14] = sum2014;
            //        muArray[countItem, 15] = item.Лимиты2014.ToString().Trim();
            //    }

            //    countItem++;

            //    k++;
            //}


            //string[,] muArrayTest = muArray;

            ////foreach (StatisticaHospital item in dictionary.Values)
            //for (int m = 1; m <= 21; m++)
            //{
            //    for (int i = 1; i <= 16; i++)
            //    {
            //        string exclБукв = ExcelЯчейка.БукваКолонка(i);


            //        //Объеденим ячейки
            //        ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), exclБукв + iCount.ToString()).Merge(Type.Missing);

            //        //Зададим ширину колонки.
            //        if (i != 2)
            //        {
            //            ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), exclБукв + iCount.ToString()).ColumnWidth = 14;
            //        }
            //        else
            //        {
            //            ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), exclБукв + iCount.ToString()).ColumnWidth = 70;
            //        }
            //        ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = muArray[m - 1, i - 1];// "Test";

            //        // выровним горизонтально.
            //        ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
            //        // выровним вертикально.
            //        ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

            //        // Перенос текста.
            //        ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).WrapText = true;

            //        // Нарисуем границу.
            //        ExcelЯчейка excNum = new ExcelЯчейка();
            //        excNum.ГраницаЯчейки(exclБукв + iCount.ToString(), exclБукв + iCount.ToString(), ObjWorkSheet);
            //    }

                iCount++;
            //}

            #endregion

            // Запишем окончание документа.
            // Высота шрифта.
            int fontSize = 8;
            // Добавим подписи.
            //Объеденим ячейки
            ObjWorkSheet.get_Range("B27", "B27").Merge(Type.Missing);
            ObjWorkSheet.get_Range("B27", "B27").Font.Size = fontSize;

            //Зададим ширину колонки.
            //ObjWorkSheet.get_Range("B27", "B27").ColumnWidth = 14;
            ObjWorkSheet.get_Range("B27", Type.Missing).Value2 = "Данные в столбцах №№4-5 предоставлены учреждениями здравоохранения по состоянию на 16.01.2014";


            ObjWorkSheet.get_Range("B28", "B28").Merge(Type.Missing);
            ObjWorkSheet.get_Range("B28", "B28").Font.Size = fontSize;

            //Зададим ширину колонки.
            //ObjWorkSheet.get_Range("B28", "B28").ColumnWidth = 14;
            ObjWorkSheet.get_Range("B28", Type.Missing).Value2 = "Данные в столбцах №№6-7 представлены учреждениями здравоохранения по состоянию на 30.01.2014";


            ObjWorkSheet.get_Range("B29", "B29").Merge(Type.Missing);
            ObjWorkSheet.get_Range("B29", "B29").Font.Size = fontSize;

            //Зададим ширину колонки.
            //ObjWorkSheet.get_Range("B29", "B29").ColumnWidth = 14;
            ObjWorkSheet.get_Range("B29", Type.Missing).Value2 = "Данные в столбцах №№12,13,14,14 сформированы с помощью программного комплекса \"Dantist\" ";


            ObjWorkSheet.get_Range("B30", "B30").Merge(Type.Missing);
            ObjWorkSheet.get_Range("B30", "B30").Font.Size = fontSize;

            //Зададим ширину колонки.
            //ObjWorkSheet.get_Range("B30", "B30").ColumnWidth = 14;
            ObjWorkSheet.get_Range("B30", Type.Missing).Value2 = "Данные в столбцах №№12 отражают количество проектов договоров, подписанных со стороны комитета. Окончательное заключение договора происходит в учреждении здравоохранения в соответствии с Положением";


            ObjWorkSheet.get_Range("B31", "B31").Merge(Type.Missing);
            ObjWorkSheet.get_Range("B31", "B31").Font.Size = fontSize;

            //Зададим ширину колонки.
            //ObjWorkSheet.get_Range("B31", "B31").ColumnWidth = 14;
            ObjWorkSheet.get_Range("B31", Type.Missing).Value2 = "Исполнитель";

            ObjWorkSheet.get_Range("B32", "B32").Merge(Type.Missing);
            ObjWorkSheet.get_Range("B32", "B32").Font.Size = fontSize;

            //Зададим ширину колонки.
            //ObjWorkSheet.get_Range("B32", "B32").ColumnWidth = 14;
            ObjWorkSheet.get_Range("B32", Type.Missing).Value2 = "Годунова И.А.";
            
            ObjWorkSheet.get_Range("B33", "B33").Merge(Type.Missing);
            ObjWorkSheet.get_Range("B33", "B33").Font.Size = fontSize;

            //Зададим ширину колонки.
           // ObjWorkSheet.get_Range("B33", "B33").ColumnWidth = 14;
            ObjWorkSheet.get_Range("B33", Type.Missing).Value2 = "44-76-11";


                //ObjWorkSheet.Cells[11, 1] = "№ п/п";
                //ObjWorkSheet.Cells[1, 5] = "Информация по бесплатному зубопротезированию по КСЗН г. Саратова";

                // Отобразим документ.
                ObjExcel.Visible = true;
            ObjExcel.UserControl = true;



        }

        //private void CreateItemExcel(int номерСтроки, StatisticaHospital item)
        //{

        //    int iCount = номерСтроки;

        //    for (int i = 1; i <= 19; i++)
        //    {

        //        string exclБукв = ExcelЯчейка.БукваКолонка(i);

        //        //Объеденим ячейки
        //        ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), exclБукв + iCount.ToString()).Merge(Type.Missing);

        //        //Зададим ширину колонки.
        //        if (i != 2)
        //        {
        //            ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), exclБукв + iCount.ToString()).ColumnWidth = 14;
        //        }
        //        else
        //        {
        //            ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), exclБукв + iCount.ToString()).ColumnWidth = 70;
        //        }

        //        switch(i)
        //        {
        //            case 1:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.НомерПП;
        //                break;
        //            case 2:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.Поликлинника;
        //                break;
        //            case 3:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.КоличествоПоликлинник;
        //                break;
        //            case 4:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ПропускнаяСпособность;
        //                break;
        //            case 5:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ПотребностьДенежныхсредств;
        //                break;
        //                // Ячейка F
        //            case 6:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ЧисленностьЛьготниковСостоящихНаУчётеВсего;
        //                break;
        //                // Ячейка G
        //            case 7:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ЧисленностьГражданОставшихсяНаУчёте2013;
        //                break;
        //                // Ячейка H.
        //            case 8:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ЧисленностьГражданВставшиеНаУчёте2014;
        //                break;
        //                // Ячейка I
        //            case 9:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ЗаключённыеДоговораНеОплатаКоличество2013;
        //                break;
        //                // Ячейка J.
        //            case 10:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ЗаключённыеДоговораНеОплатаСумма2013;
        //                break;
        //                // Ячейка K.
        //            case 11:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ДоговораЗаключённыеКоличество2014;
        //                break;
        //                // Ячейка L.
        //            case 12:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ДоговораЗаключённыеСумма2014;
        //                break;
        //                // Ячейка M.
        //            case 13:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ВсегоКоличествоДоговоров;
        //                break;
        //                // Ячейка N.
        //            case 14:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ВсегоКоличествоДоговоровЗа2013;
        //                break;
        //                // Ячейка O
        //            case 15:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ВсегоКоличествоДоговоровЗа2014;
        //                break;
        //                // Ячейка P.
        //            case 16:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.ВсегоВыплачено;
        //                break;
        //                // Ячейка Q.
        //            case 17:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.СуммаВыплаченнаяПоДоговорам2013;
        //                break;
        //                // Ячейка R.
        //            case 18:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.СуммаВыплаченнаяПоДоговорам2014;
        //                break;
        //            // Ячейка S.
        //            case 19:
        //                ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).Value2 = item.Лимиты2014;
        //                break;

        //        }

                

        //        // выровним горизонтально.
        //        ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).HorizontalAlignment = Excel.Constants.xlCenter;
        //        // выровним вертикально.
        //        ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).VerticalAlignment = Excel.Constants.xlCenter;

        //        // Перенос текста.
        //        ObjWorkSheet.get_Range(exclБукв + iCount.ToString(), Type.Missing).WrapText = true;

        //        // Нарисуем границу.
        //        ExcelЯчейка excNum = new ExcelЯчейка();
        //        excNum.ГраницаЯчейки(exclБукв + iCount.ToString(), exclБукв + iCount.ToString(), ObjWorkSheet);
        //    }
        //}



        /// <summary>
        /// Формирует ячейку.
        /// </summary>
        /// <param name="ceel1">адрес начальной ячейки.</param>
        /// <param name="ceel2">адрес второй ячейки</param>
        /// <param name="text">текст внутри ячейки.</param>
        /// <param name="width">ширина колонки</param>
            //private void Ячейка(string ceel1, string ceel2, string text, int width,(Microsoft.Office.Interop.Excel.Worksheet)ObjWorkBook.Sheets[1] ObjWorkSheet)
            //{


            //    //Объеденим ячейки
            //    ObjWorkSheet.get_Range(ceel1, ceel2).Merge(Type.Missing);

            //    // Зададим ширину колонки.
            //    ObjWorkSheet.get_Range(ceel1, ceel2).ColumnWidth = width;
            //    ObjWorkSheet.get_Range(ceel1, Type.Missing).Value2 = text;

            //    // Нарисуем границу.
            //    ExcelЯчейка excРайон = new ExcelЯчейка();
            //    excРайон.ГраницаЯчейки(ceel1, ceel2, ObjWorkSheet);
            //}

            
    }

         
}

