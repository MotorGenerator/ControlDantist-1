using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Classes;
using Microsoft.Office.Interop.Word;

namespace ControlDantist.ValidPersonContract
{
    public class WordReport : IWordPrint
    {

        private List<PrintContractsValidate> listDoc;

        public WordReport(List<PrintContractsValidate> listDoc)
        {
            this.listDoc = listDoc;
        }

        public void Print()
        {
            string filName = System.Windows.Forms.Application.StartupPath + @"\Шаблон\Список договоров.doc";

            //Создаём новый Word.Application
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();

            //Загружаем документ
            Microsoft.Office.Interop.Word.Document doc = null;

            object fileName = filName;
            object falseValue = false;
            object trueValue = true;
            object missing = Type.Missing;

            doc = app.Documents.Open(ref fileName, ref missing, ref trueValue,
            ref missing, ref missing, ref missing, ref missing, ref missing,
            ref missing, ref missing, ref missing, ref missing, ref missing,
            ref missing, ref missing, ref missing);

            //Горизонтальное ориентирование листа
            //doc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;


            object bookNaziv = "таблица";
            Range wrdRng = doc.Bookmarks.get_Item(ref bookNaziv).Range;

            object behavior = Microsoft.Office.Interop.Word.WdDefaultTableBehavior.wdWord8TableBehavior;
            object autobehavior = Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitWindow;

            Microsoft.Office.Interop.Word.Table table = doc.Tables.Add(wrdRng, 1, 4, ref behavior, ref autobehavior);
            table.Range.ParagraphFormat.SpaceAfter = 6;
            table.Columns[1].Width = 40;
            table.Columns[2].Width = 140;
            table.Columns[3].Width = 80;
            table.Columns[4].Width = 200;
            table.Borders.Enable = 1; // Рамка - сплошная линия
            table.Range.Font.Name = "Times New Roman";
            table.Range.Font.Size = 10;
            //счётчик строк
            int i = 1;

            //Список с данными для таблицы
            List<ContractListItem> list = new List<ContractListItem>();

            //Сформируем шапку таблицы
            ContractListItem шапка = new ContractListItem();
            шапка.Num = "№ п/п";
            шапка.FIO = "ФИО льготника";
            шапка.NumCurrentContract = "Номер текущего договора";
            //шапка.NumContracts = "Ранее заключённые договора в других поликлинниках";
            шапка.NumContr = "Ранее заключённые договора";

            list.Add(шапка);

            //Счётчик
            int iCount = 1;

            foreach (PrintContractsValidate un in listDoc)
            {
                //Заполним данными таблицу
                ContractListItem item = new ContractListItem();

                //Порядковый номер
                item.Num = iCount.ToString().Trim();

                //Запишем ФИО льготника
                item.FIO = un.ФИО_Номер_ТекущийДоговор.Trim();

                //номер текущего договора
                item.NumCurrentContract = un.НомерТекущийДоговор.Trim();

                //Запишем номера договоров
                //item.NumContracts = un.НомераДоговоров.Trim();
                if(un.СписокДоговоров != null)
                item.NumContr = un.СписокДоговоров.Trim();

                list.Add(item);

                //Увеличим счётчик на 1
                iCount++;
            }

            //Заполним таблицу
            int k = 1;
            //запишем данные в таблицу
            foreach (ContractListItem item in list)
            {
                //table.Cell(i, 1).Range.Text = i.ToString();//item.НомерПорядковый;
                table.Cell(k, 1).Range.Text = item.Num;

                table.Cell(k, 2).Range.Text = item.FIO;

                table.Cell(k, 3).Range.Text = item.NumCurrentContract;

                //table.Cell(k, 4).Range.Text = item.NumContracts;
                table.Cell(k, 4).Range.Text = item.NumContr;


                //doc.Words.Count.ToString();
                Object beforeRow1 = Type.Missing;
                table.Rows.Add(ref beforeRow1);

                k++;
            }

            table.Rows[k].Delete();

            //Отобразим документ
            app.Visible = true;


        }
    }
}
