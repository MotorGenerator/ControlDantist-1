using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.ClassValidRegions;
using System.Data;
using System.Data.SqlClient;
using ControlDantist.LetterClassess;
using Microsoft.Office.Interop.Word;
using ControlDantist.Classes;

namespace ControlDantist.Letter
{
    public class ПодтвержденныеПисьма : ILetter
    {
        private List<PersonValidEsrn> dictionary;

        private DataTable dataTable;

        private string инн = string.Empty;

        public ПодтвержденныеПисьма(List<PersonValidEsrn> dictionary, string инн)
        {
            this.dictionary = dictionary;

            this.инн = инн;
        }

        public void Print()
        {
            // Узнаем поликлиннику.
            АтрибутПисьма атрибутПисьма = new АтрибутПисьма(инн);

            // Получим атрибут письма.
            DataTable tabAtribut = атрибутПисьма.GetAttributLetter();

            string filName = System.Windows.Forms.Application.StartupPath + @"\Шаблон\Письмо В работу.doc";


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

            Microsoft.Office.Interop.Word.Table table = doc.Tables.Add(wrdRng, 1, 3, ref behavior, ref autobehavior);
            table.Range.ParagraphFormat.SpaceAfter = 6;
            table.Columns[1].Width = 40;
            table.Columns[2].Width = 140;
            table.Columns[3].Width = 300;
            table.Borders.Enable = 1; // Рамка - сплошная линия
            table.Range.Font.Name = "Times New Roman";
            table.Range.Font.Size = 10;
            //счётчик строк
            int i = 1;

            //Список для хранения данных для таблицы
            List<ReestrNoPrintDog> listItem = new List<ReestrNoPrintDog>();

            //Сформируем шапку таблицы
            ReestrNoPrintDog шапка = new ReestrNoPrintDog();
            шапка.Number = "№ п/п";
            шапка.NumDog = "№ договора";
            шапка.Fio = "ФИО";

            listItem.Add(шапка);

            //Счётчик
            int iCount = 1;

            //Заполним данными таблицу
            foreach (PersonValidEsrn un in dictionary)
            {
                //Строка таблицы
                ReestrNoPrintDog item = new ReestrNoPrintDog();

                // Порядковый номер.
                item.Number = iCount.ToString();

                // № договора.
                item.NumDog = un.номерДоговора.Trim();


                string фамилия = un.фамилия.Trim();
                string имя = un.имя.ToString().Trim();

                string отчество = "";

                if (un.отчество != null)
                {
                    отчество = un.отчество.ToString().Do(w => w, "").Trim();
                }
                

                // ФИО льготника.
                string фио = фамилия + " " + имя + " " + отчество;
                item.Fio = фио.Trim();

                item.Примечание = "";

                listItem.Add(item);

                iCount++;
            }

            //Заполним таблицу
            int k = 1;
            //запишем данные в таблицу
            foreach (ReestrNoPrintDog item in listItem)
            {
                //table.Cell(i, 1).Range.Text = i.ToString();//item.НомерПорядковый;
                table.Cell(k, 1).Range.Text = item.Number;

                table.Cell(k, 2).Range.Text = item.NumDog;
                table.Cell(k, 3).Range.Text = item.Fio;


                //doc.Words.Count.ToString();
                Object beforeRow1 = Type.Missing;
                table.Rows.Add(ref beforeRow1);

                k++;
            }

            table.Rows[k].Delete();



            string адрес = string.Empty;
            string обращение = string.Empty;
            string fio = string.Empty;

            if (tabAtribut.Rows.Count > 0)
            {
                адрес = tabAtribut.Rows[0]["F2"].ToString();

                обращение = tabAtribut.Rows[0]["F3"].ToString();

                fio = tabAtribut.Rows[0]["ФИО"].ToString();
            }

            object wdrepl = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt = "адресат";
            object newtxt = (object)адрес;
            //object frwd = true;
            object frwd = false;
            doc.Content.Find.Execute(ref searchtxt, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd, ref missing, ref missing, ref newtxt, ref wdrepl, ref missing, ref missing,
            ref missing, ref missing);

            //Запишем кому
            object wdrepl1 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt1 = "кому";
            object newtxt1 = (object)обращение;
            //object frwd = true;
            object frwd1 = false;
            doc.Content.Find.Execute(ref searchtxt1, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd1, ref missing, ref missing, ref newtxt1, ref wdrepl1, ref missing, ref missing,
            ref missing, ref missing);

            //запишем фио
            object wdrepl2 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt2 = "fio";
            object newtxt2 = (object)fio;
            //object frwd = true;
            object frwd2 = false;
            doc.Content.Find.Execute(ref searchtxt2, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd2, ref missing, ref missing, ref newtxt2, ref wdrepl2, ref missing, ref missing,
            ref missing, ref missing);

            string исполнитель = System.Configuration.ConfigurationSettings.AppSettings["Исполнитель"].Trim();

            //запишем фио
            object wdrepl3 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt3 = "Исполнитель";
            object newtxt3 = (object)исполнитель;
            //object frwd = true;
            object frwd3 = false;
            doc.Content.Find.Execute(ref searchtxt3, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd3, ref missing, ref missing, ref newtxt3, ref wdrepl3, ref missing, ref missing,
            ref missing, ref missing);

            //Отобразим документ
            app.Visible = true;
        }

    }
}
