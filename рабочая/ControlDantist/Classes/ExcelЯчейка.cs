using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Excel = Microsoft.Office.Interop.Excel;

namespace ControlDantist.Classes
{
    class ExcelЯчейка
    {
            public static string БукваКолонка(int номер)
            {
                string chr = string.Empty;
                switch (номер)
                {
                    case 1:
                        chr = "A";
                        break;
                    case 2:
                        chr = "B";
                        break;
                    case 3:
                        chr = "C";
                        break;
                    case 4:
                        chr = "D";
                        break;
                    case 5:
                        chr = "E";
                        break;
                    case 6:
                        chr = "F";
                        break;
                    case 7:
                        chr = "G";
                        break;
                    case 8:
                        chr = "H";
                        break;
                    case 9:
                        chr = "I";
                        break;
                    case 10:
                        chr = "J";
                        break;
                    case 11:
                        chr = "K";
                        break;
                    case 12:
                        chr = "L";
                        break;
                    case 13:
                        chr = "M";
                        break;
                    case 14:
                        chr = "N";
                        break;
                    case 15:
                        chr = "O";
                        break;
                    case 16:
                        chr = "P";
                        break;
                    case 17:
                        chr = "Q";
                        break;
                    case 18:
                        chr = "R";
                        break;
                    case 19:
                        chr = "S";
                        break;
                    case 20:
                        chr = "T";
                        break;
                    case 21:
                        chr = "U";
                        break;
                    case 22:
                        chr = "V";
                        break;
                    case 23:
                        chr = "W";
                        break;
                }

                return chr;
            }

            /// <summary>
            /// Записывает текст в ячейку с переносом слов
            /// </summary>
            /// <param name="текст"></param>
            /// <returns></returns>
            public string ТекстЯчейкаПеренос(string текст)
            {
                StringBuilder sb = new StringBuilder();
                foreach (char ch in текст)
                {
                    if (ch == ' ')
                    {
                        sb.Append((char)10);
                    }
                    else
                    {
                        sb.Append(ch);
                    }
                }

                return sb.ToString();
            }

            /// <summary>
            /// ДЛобавляет в ячейку текст
            /// </summary>
            /// <param name="текст"></param>
            /// <returns></returns>
            public string ТекстЯчейка(string текст)
            {
                return текст;
            }

            public string ТекстЯчейкаНазвЗакон(string текст)
            {
                StringBuilder sb = new StringBuilder();
                int count = 0;

                foreach (char ch in текст)
                {
                    if (ch == ' ')
                    {
                        count++;
                        sb.Append(ch);
                    }

                    if (count == 2)
                    {
                        sb.Append((char)10);
                        count = 0;
                    }
                    else
                    {
                        sb.Append(ch);
                    }

                }

                //foreach (char ch in текст)
                //{
                //    if (ch == ' ')
                //    {
                //        if (count == 2)
                //        {
                //            sb.Append((char)10);
                //            count = 0;
                //        }
                //        count++;
                //    }
                //    else
                //    {
                //        sb.Append(ch);
                //    }
                //}

                return sb.ToString();
            }

            /// <summary>
            /// Рисует границы вокруг указанной ячейки
            /// </summary>
            /// <param name="row"></param>
            /// <param name="column"></param>
            //public void ГраницаЯчейки(int row, int column, Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet)
            //{
            //    //(ObjWorkSheet.Cells[row, column] as Excel.Range).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble; // Двойная линия.
            //    //(ObjWorkSheet.Cells[row, column] as Excel.Range).Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlDouble;
            //    //(ObjWorkSheet.Cells[row, column] as Excel.Range).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDouble;
            //    //(ObjWorkSheet.Cells[row, column] as Excel.Range).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlDouble;

            //    (ObjWorkSheet.Cells[row, column] as Excel.Range).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous; // Простая линия.
            //    (ObjWorkSheet.Cells[row, column] as Excel.Range).Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            //    (ObjWorkSheet.Cells[row, column] as Excel.Range).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            //    (ObjWorkSheet.Cells[row, column] as Excel.Range).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            //}


            public void ГраницаЯчейки(string cell1, string cell2, Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet)
            {
                //var cells = WorkSheet.get_Range("B2", "F5")
                var cells = ObjWorkSheet.get_Range(cell1, cell2);

                // верхняя внешняя.
                cells.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;

                // правая внешняя.
                cells.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;

                // Левая внешная.
                cells.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;

                // Нижная верхная.
                cells.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;

            }

        }
    }

