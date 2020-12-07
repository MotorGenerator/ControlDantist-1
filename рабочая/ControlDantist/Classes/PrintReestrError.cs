using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using System.Runtime.Serialization.Formatters.Binary;

using ControlDantist.Classes;


namespace ControlDantist.Classes
{
    /// <summary>
    /// Выводит на экран реестр с ошибкой
    /// </summary>
    class PrintReestrError
    {
        private List<ErrorReestr> list;
        string категория;

        public PrintReestrError(List<ErrorReestr> listError, string льготнаяКатегория)
        {
            list = listError;
            категория = льготнаяКатегория;
        }

        public void Print()
        {
            //string filName = System.Windows.Forms.Application.StartupPath + @"\Документы\" + fName + ".doc";
            string filName = System.Windows.Forms.Application.StartupPath + @"\Шаблон\Реестр ошибочных сведений.doc";
            //string filName = fName;

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

            doc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;

            //Вставить таблицу
            object bookNaziv = "таблица";
            Range wrdRng = doc.Bookmarks.get_Item(ref  bookNaziv).Range;

            object behavior = Microsoft.Office.Interop.Word.WdDefaultTableBehavior.wdWord8TableBehavior;
            object autobehavior = Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitWindow;

            Microsoft.Office.Interop.Word.Table table = doc.Tables.Add(wrdRng, 1, 9, ref behavior, ref autobehavior);
            table.Range.ParagraphFormat.SpaceAfter = 6;
            table.Columns[1].Width = 40;
            table.Columns[2].Width = 140;
            table.Columns[3].Width = 140;
            table.Columns[4].Width = 60;
            table.Columns[5].Width = 60;
            table.Columns[6].Width = 60;
            table.Columns[6].Width = 60;
            table.Columns[6].Width = 60;
            table.Columns[6].Width = 60;
            table.Borders.Enable = 1; // Рамка - сплошная линия
            table.Range.Font.Name = "Times New Roman";
            table.Range.Font.Size = 10;
            //счётчик строк
            int i = 1;

            //Объявим коллекцию классов содержащую данные для таблицы реестра
            List<ReestrErrorControl> listItem = new List<ReestrErrorControl>();

            //Сформируем шапку таблицы
            ReestrErrorControl шапка = new ReestrErrorControl();
            шапка.НомерПорядковый = "№ п.п.";
            шапка.ФИО = "ФИО";

            шапка.НаименованиеУслуги = "Наименование услуги";
            шапка.ЦенаControl = "Контрольная цена услуги";
            шапка.ЦенаError = "Ошибочная цена услуги";

            шапка.СтоимостьУслугиControl = "Контрольная стоимость услуги";
            шапка.СтоимостьУслугиError = "Ошибочная стоимость услуги";
            шапка.СуммаControl = "Контрольная сумма оказанных услуг";
            шапка.СумаError = "Ошибочная стоимость оказанных услуг";

            listItem.Add(шапка);

            //Заполним данными таблицу
            foreach (ErrorReestr item in list)
            {
                ReestrErrorControl r = new ReestrErrorControl();
                r.НомерПорядковый = i.ToString();
                r.ФИО = item.ФИО;

                //счётчик
                int iCount = 0;

                foreach (ErrorsReestrUnload err in item.ErrorListУслуги)
                {
                    if (iCount == 0)
                    {
                        r.НаименованиеУслуги = err.НаименованиеУслуги;
                        r.ЦенаControl = err.Цена.ToString();
                        r.ЦенаError = err.ErrorЦена.ToString();
                        r.СтоимостьУслугиControl = err.Сумма.ToString();
                        r.СтоимостьУслугиError = err.ErrorСумма.ToString();

                        //Добавим в коллекцию
                        listItem.Add(r);
                    }
                    else
                    {
                        ReestrErrorControl r2 = new ReestrErrorControl();
                        r2.НомерПорядковый = "";
                        r2.ФИО = "";

                        r2.НаименованиеУслуги = err.НаименованиеУслуги;
                        r2.ЦенаControl = err.Цена.ToString();
                        r2.ЦенаError = err.ErrorЦена.ToString();
                        r2.СтоимостьУслугиControl = err.Сумма.ToString();
                        r2.СтоимостьУслугиError = err.ErrorСумма.ToString();

                        //Добавим в коллекцию
                        listItem.Add(r2);
                    }

                    iCount++;
                }

                ReestrErrorControl rСумм = new ReestrErrorControl();
                rСумм.НомерПорядковый = "";
                rСумм.ФИО = "";

                rСумм.НаименованиеУслуги = "";
                rСумм.ЦенаControl = "";
                rСумм.ЦенаError = "";
                rСумм.СтоимостьУслугиControl = "";
                rСумм.СтоимостьУслугиError = "";

                rСумм.СуммаControl = item.СуммаИтогоСтоимостьУслуг.ToString();
                rСумм.СумаError = item.ErrorСуммаИтогоСтоимостьУслуг.ToString();

                listItem.Add(rСумм);
                i++;
            }

            int k = 1;
            //запишем данные в таблицу
            foreach (ReestrErrorControl item in listItem)
            {
                //table.Cell(i, 1).Range.Text = i.ToString();//item.НомерПорядковый;
                table.Cell(k, 1).Range.Text = item.НомерПорядковый;

                table.Cell(k, 2).Range.Text = item.ФИО;
                table.Cell(k, 3).Range.Text = item.НаименованиеУслуги;

                table.Cell(k, 4).Range.Text = item.ЦенаControl;
                table.Cell(k, 5).Range.Text = item.ЦенаError;

                table.Cell(k, 6).Range.Text = item.СтоимостьУслугиControl;
                table.Cell(k, 7).Range.Text = item.СтоимостьУслугиError;

                table.Cell(k, 8).Range.Text = item.СуммаControl;
                table.Cell(k, 9).Range.Text = item.СумаError;


                //doc.Words.Count.ToString();
                Object beforeRow1 = Type.Missing;
                table.Rows.Add(ref beforeRow1);

                k++;
            }

            table.Rows[k].Delete();

            ////Номер договора
            object wdrepl = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt = "категория";
            object newtxt = (object)категория;
            //object frwd = true;
            object frwd = false;
            doc.Content.Find.Execute(ref searchtxt, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd, ref missing, ref missing, ref newtxt, ref wdrepl, ref missing, ref missing,
            ref missing, ref missing);



            //Отобразим документ
            app.Visible = true;

        }
    }
}
