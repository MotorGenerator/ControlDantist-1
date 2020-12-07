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
    /// Выводит на экран документ word содержащий реестр договоров
    /// </summary>
    class PrintReestrControl
    {
        //Коллекция хранит содержание реестра
        private List<ReestrControl> list;
        
        //переменная хранит имя загружаемого файла
        private string fName = string.Empty;

        //Хранит сумму итого реестра
        private string суммаИтого = string.Empty;

        //Хранит название льготной категории
        private string льготКатегор = string.Empty;

        public PrintReestrControl(List<ReestrControl> listReestrControl,string fileName, string итогоСумма, string льготнаяКатегория)
        {
            list = listReestrControl;
            fName = fileName;
            суммаИтого = итогоСумма;
            льготКатегор = льготнаяКатегория;
        }

        public void Print()
        {
            //string fName = "Реестр " + this.dtStart.Value.ToShortDateString().Replace('.', '_') + " " + this.dtEnd.Value.ToShortDateString().Replace('.', '_');
            //string fName = fileName;

            //try
            //{
            //    //Скопируем шаблон в папку Документы
            //    //FileInfo fn = new FileInfo(System.Windows.Forms.Application.StartupPath + @"\Шаблон\Реестр.doc");
            //    ////fn.CopyTo(System.Windows.Forms.Application.StartupPath + @"\Документы\" + fName + ".doc", true);
            //    ////fn.CopyTo(@"\Документы\" + fName + ".doc", true);
            //    ////fn.CopyTo(@"\Документы\" + Реестр + ".doc", true);
            //    //fn.Delete();
            //}
            //catch
            //{
            //    MessageBox.Show("Документ с таки именем уже существует");
            //}

            //string filName = System.Windows.Forms.Application.StartupPath + @"\Документы\" + fName + ".doc";
            string filName = System.Windows.Forms.Application.StartupPath + @"\Шаблон\Реестр.doc";
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

            //Вставить таблицу
            object bookNaziv = "таблица";
            Range wrdRng = doc.Bookmarks.get_Item(ref  bookNaziv).Range;

            object behavior = Microsoft.Office.Interop.Word.WdDefaultTableBehavior.wdWord8TableBehavior;
            object autobehavior = Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitWindow;


            Microsoft.Office.Interop.Word.Table table = doc.Tables.Add(wrdRng, 1, 6, ref behavior, ref autobehavior);
            table.Range.ParagraphFormat.SpaceAfter = 6;
            table.Columns[1].Width = 40;
            table.Columns[2].Width = 140;
            table.Columns[3].Width = 80;
            table.Columns[4].Width = 80;
            table.Columns[5].Width = 80;
            table.Columns[5].Width = 80;
            table.Borders.Enable = 1; // Рамка - сплошная линия
            table.Range.Font.Name = "Times New Roman";
            table.Range.Font.Size = 10;
            //счётчик строк
            int i = 1;

            List<ReestrControl> listItem = new List<ReestrControl>();

            ReestrControl шапка = new ReestrControl();
            шапка.Номер = "№ п.п.";
            шапка.ФИО = "Ф.И.О.";
            шапка.ДатаНомерДоговора = "№ и дата договора на предоставление услуг";
            шапка.НомерАктаОказанныхУслуг = "№ и дата акта оказания услуг";
            шапка.ДокументЛьгота = "Серия и дата документа о праве на льготу";
            шапка.СтоимостьУслуг = "Стоимость услуги руб.";

            listItem.Add(шапка);

            foreach (ReestrControl item in list)
            {
                ReestrControl r = new ReestrControl();
                r.Номер = i.ToString();
                r.ФИО  = item.ФИО;
                r.ДатаНомерДоговора = item.ДатаНомерДоговора;
                r.НомерАктаОказанныхУслуг  = item.НомерАктаОказанныхУслуг;
                r.ДокументЛьгота = item.ДокументЛьгота;
                r.СтоимостьУслуг = item.СтоимостьУслуг;

                listItem.Add(r);
                i++;
            }

            //Добавим строку Итого
            ReestrControl итого = new ReestrControl();
            итого.Номер = "";
            итого.ФИО = "Всего по реестру:";
            итого.ДатаНомерДоговора = "";
            итого.НомерАктаОказанныхУслуг = "";
            итого.ДокументЛьгота = "";
            итого.СтоимостьУслуг = суммаИтого;

            //Добавим в коллекцию строку с итоговой суммой
            listItem.Add(итого);

            int k = 1;
            //запишем данные в таблицу
            foreach (ReestrControl item in listItem)
            {
                //table.Cell(i, 1).Range.Text = i.ToString();//item.НомерПорядковый;
                table.Cell(k, 1).Range.Text = item.Номер;

                table.Cell(k, 2).Range.Text = item.ФИО;

                table.Cell(k, 3).Range.Text = item.ДатаНомерДоговора;
                table.Cell(k, 4).Range.Text = item.НомерАктаОказанныхУслуг;
                table.Cell(k, 5).Range.Text = item.ДокументЛьгота;
                table.Cell(k, 6).Range.Text = item.СтоимостьУслуг;

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
            object newtxt = (object)льготКатегор;
            //object frwd = true;
            object frwd = false;
            doc.Content.Find.Execute(ref searchtxt, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd, ref missing, ref missing, ref newtxt, ref wdrepl, ref missing, ref missing,
            ref missing, ref missing);

            app.Visible = true;



        }
                
    }
}
