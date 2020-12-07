using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word = Microsoft.Office.Interop.Word;
using ControlDantist.Classes;

namespace ControlDantist.Classes
{

    public class WordLetterStatistic
    {
        private Word.Application wordapp;
        private Word.Document worddocument;

        List<СтатистикаПректовДоговоров> listStat;

        public WordLetterStatistic(List<СтатистикаПректовДоговоров> personsList)
         {
             if (personsList == null || personsList.Count == 0)
             {
                 throw new ExceptionYearNumber();
             }

             listStat = personsList;
         }


        public void PrintDoc()
        {
            //Создаем объект Word - равносильно запуску Word
            wordapp = new Word.Application();

            // Параметры для создания листа.
            Object template = Type.Missing;
            Object newTemplate = false;
            Object documentType = Word.WdNewDocumentType.wdNewBlankDocument;
            Object visible = true;

            //Делаем его видимым
            wordapp.Visible = true;

            // Добавляем в документ список.
            worddocument = wordapp.Documents.Add(ref template, ref newTemplate, ref documentType, ref visible);

            // Оринтирование страницы
            worddocument.PageSetup.Orientation = Word.WdOrientation.wdOrientPortrait;

            worddocument.Content.Font.Size = 10;
            //worddocument.Content.Font.Bold = 1;
            // Скроем подчёркивание текста по умолчанию во всём документе.
            //worddocument.Content.Font.Underline = Word.WdUnderline.wdUnderlineSingle;

            // Установим выравнивание текста в документе по умолчанию по центру.
            worddocument.Content.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            // Сгруппируем льготников и получим группы для разных писем в разные адресаты.
            var groups = listStat.GroupBy(w => w.ЛьготнаяКатегория);

            int countTable = 1;

            foreach (var itd in groups)
            {
                object unit;
                object count;
                object extend;
                object missing = Type.Missing;
                // Создадим параграф.
                Word.Paragraph para = worddocument.Paragraphs.Add(ref missing);
                para.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                // Зададим размер шрифта.
                para.Range.Font.Size = 14F;

                // Параграф выведим жирным шрифтом.
                para.Range.Bold = 1;

                // вставим в документ новый параграф.
                para.Range.InsertParagraphAfter();

                para.Range.Text = "Льготная категория - "+ itd.Key.Trim() +" ";

                  // Создадим параграф для пустой строки.
                Word.Paragraph para11 = worddocument.Paragraphs.Add(ref missing);
                para11.Range.InsertParagraphAfter();

                //=============
                // Создадим ещё один параграф и выведим в него таблицу.
                Word.Paragraph para2 = worddocument.Paragraphs.Add(ref missing);
                para2.Range.InsertParagraphAfter();

                Word.Range wordrange = para2.Range;
                //Добавляем таблицу в начало первого параграфа.
                Object defaultTableBehavior = Word.WdDefaultTableBehavior.wdWord9TableBehavior;
                
                // Автоподбор ширины столбца таблицы по содержимому.
                Object autoFitBehavior = Word.WdAutoFitBehavior.wdAutoFitContent;

                // Узнаем количество элементов в коллекции с льготниками, для того чтобы задать колличество строк в таблицу.
                int countRow = itd.Count();

                // Выведим таблицу из countRow строк и 5 колонок.
                Word.Table wordtable1 = worddocument.Tables.Add(wordrange, countRow+2, 4, ref defaultTableBehavior, ref autoFitBehavior);

                // Зададим выравнивание строк по цетру страницы.
                wordtable1.Rows.Alignment = Word.WdRowAlignment.wdAlignRowCenter;

                // Зададим ширину колонок.
                wordtable1.Columns.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthAuto;

                // Разрешим автоподгонку.
                wordtable1.AllowAutoFit = true;

                // Разрешим разрыв страниц.
                wordtable1.AllowPageBreaks = true;

                int countTest = itd.ToList().Count;

                // Добавим в первую строку таблицы шапку.
                wordtable1.Rows[1].Cells[1].Range.Text = "№ п/п";
                wordtable1.Rows[1].Cells[2].Range.Text = "Номер договора";
                wordtable1.Rows[1].Cells[3].Range.Text = "ФИО";
                wordtable1.Rows[1].Cells[4].Range.Text = "Сумма";

                int countRows = 2;
                int iCountRow = 1;

                decimal coiuntContract = 0.0m;

                // Заполним таблицу данными.
                foreach (СтатистикаПректовДоговоров p in itd)
                {
                    coiuntContract += p.СтоимостьУслугПоДоговору;
                    
                    // Заполним ячейки таблицы данными.
                    for (int n = 0; n < wordtable1.Columns.Count; n++)
                    {
                        // Номер по пп.
                        Word.Range wordcellrange2 = worddocument.Tables[countTable].Cell(countRows, 1).Range;
                        wordcellrange2.Text = iCountRow.ToString().Trim();

                        string ФИО = p.Фамилия.Trim() + " " + p.Имя.Trim() + " " + p.Отчество.Trim();

                        // ФИО
                        Word.Range wordcellrange3 = worddocument.Tables[countTable].Cell(countRows, 2).Range;
                        wordcellrange3.Text = ФИО;

                        // Номер договора.
                        Word.Range wordcellrange4 = worddocument.Tables[countTable].Cell(countRows, 3).Range;
                        wordcellrange4.Text = p.НомерДоговора;

                        // Сумма
                        Word.Range wordcellrange5 = worddocument.Tables[countTable].Cell(countRows, 4).Range;
                        wordcellrange5.Text = p.СтоимостьУслугПоДоговору.ToString("c").Trim();
                    }

                    countRows++;
                    iCountRow++;
                }

                countRows = countRows + 1;

                // Добавим строку ИТОГО.
                // Номер по пп.
                Word.Range wordcellrange21 = worddocument.Tables[countTable].Cell(countRows, 1).Range;
                wordcellrange21.Text = ""; // iCountRow.ToString().Trim();
                
                // ФИО
                Word.Range wordcellrange31 = worddocument.Tables[countTable].Cell(countRows, 2).Range;
                wordcellrange31.Text = "Итого :";

                // Номер договора.
                Word.Range wordcellrange41 = worddocument.Tables[countTable].Cell(countRows, 3).Range;
                wordcellrange41.Text = "";

                // Сумма
                Word.Range wordcellrange51 = worddocument.Tables[countTable].Cell(countRows, 4).Range;
                wordcellrange51.Text = coiuntContract.ToString("c").Trim();
                
                countTable++;
            }
        }
    }
}
