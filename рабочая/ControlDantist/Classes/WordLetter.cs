using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word = Microsoft.Office.Interop.Word;
using ControlDantist.Classes;

namespace ControlDantist.Classes
{
    public class WordLetter
    {
        private Word.Application wordapp;
        private Word.Document worddocument; 

         List<PersonRecipient> persons;
         public WordLetter(List<PersonRecipient> personsList)
         {
             if (personsList == null || personsList.Count == 0)
             {
                 throw new ExceptionYearNumber();
             }

             persons = personsList;
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

            worddocument.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;

            worddocument.Content.Font.Size = 10;
            //worddocument.Content.Font.Bold = 1;
            // Скроем подчёркивание текста по умолчанию во всём документе.
            //worddocument.Content.Font.Underline = Word.WdUnderline.wdUnderlineSingle;

            // Установим выравнивание текста в документе по умолчанию по центру.
            worddocument.Content.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            // Отключим отступы в ячейках.
            //worddocument.Content.ParagraphFormat.LeftIndent = worddocument.Content.Application.CentimetersToPoints((float)2);
            //worddocument.Content.ParagraphFormat.RightIndent = worddocument.Content.Application.CentimetersToPoints((float)1);
            
            // Сгруппируем льготников и получим группы для разных писем в разные адресаты.
            var groups = persons.GroupBy(w => w.РайонОбласти);


            //Создадим первый параграф.

            // Счётчик таблиц.
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

                // Получим название льготной категории граждан.
                var selectLK = itd.Select(w => new { w.ЛьготнаяКатегория }).Select(w => w).Distinct();

                // Строка для хранения льготной категории.
                StringBuilder buildЛготКат = new StringBuilder();

                foreach (var a in selectLK)
                {
                    buildЛготКат.Append(a.ЛьготнаяКатегория);
                }


                // Получим название района.
                var asd = itd.Select(w => new { w.РайонОбласти }).Select(w => w).Distinct();

                // Строка для хранения района.
                StringBuilder buildРайон = new StringBuilder();

                foreach (var a in asd)
                {
                    buildРайон.Append(a.РайонОбласти);
                }



                // Коментарии пока оставим вдруг вернёмся к данному виду.
                //if (buildРайон.ToString().Trim() != "")
                //{
                    // запишем в параграф текст.
                    para.Range.Text = "Список граждан, получивших меры социальной поддержки в виде изготовления и ремонта зубных протезов по категории " + buildЛготКат.ToString().Trim() + "\n " +
                                      " проживающих на территории " + buildРайон.ToString().Trim();
                //}
                //else
                //{
                //    // запишем в параграф текст.
                //    para.Range.Text = "Список граждан, получивших меры социальной поддержки в виде изготовления и ремонта зубных протезов по категории " + buildЛготКат.ToString().Trim() + "\n " +
                //                      " проживающих на территории __________________________";
                //}

                
                
                    // Выделение отделного слова в обзацце пока не работает.
                    //////Выделяем второе слово абзаца
                    //unit =  Word.WdUnits.wdParagraph; 
                    //count = 34;
                    //extend = Word.WdMovementType.wdExtend;
                    //para.Range.Move(unit, count);
                    //count = 36;
                    //para.Range.MoveEnd(unit, count);
                    
                    //para.Range.Font.Bold = 1;
                    
                    
                    ////wordapp.Selection.MoveRight(ref unit, ref count,
                    ////ref extend);
                    ////wordapp.Selection.Font.Size = 15;
                    ////wordapp.Selection.Font.Color = Word.WdColor.wdColorRed;

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

                // Выведим таблицу из countRow строк и 13 колонок.
                Word.Table wordtable1 = worddocument.Tables.Add(wordrange, countRow+1, 12, ref defaultTableBehavior, ref autoFitBehavior);

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
                wordtable1.Rows[1].Cells[1].Range.Text = "Населённый пункт";
                wordtable1.Rows[1].Cells[2].Range.Text = "Фамилия";
                wordtable1.Rows[1].Cells[3].Range.Text = "Имя";
                wordtable1.Rows[1].Cells[4].Range.Text = "Отчество";
                wordtable1.Rows[1].Cells[5].Range.Text = "Дата рождения";
                wordtable1.Rows[1].Cells[6].Range.Text = "Улица";
                wordtable1.Rows[1].Cells[7].Range.Text = "Дом";
                wordtable1.Rows[1].Cells[8].Range.Text =  "Корпус";
                wordtable1.Rows[1].Cells[9].Range.Text = "Квартира";
                wordtable1.Rows[1].Cells[10].Range.Text = "Серия документа";
                wordtable1.Rows[1].Cells[11].Range.Text = "Номер документа";
                wordtable1.Rows[1].Cells[12].Range.Text = "Сумма";
                

                  int countRows = 2;
  
                foreach (PersonRecipient p in itd)
                {

                    for (int n = 0; n < wordtable1.Columns.Count; n++)
                    {
                        #region Полная таблица
                        //// Район области.
                        //Word.Range wordcellrange = worddocument.Tables[countTable].Cell(countRows, 1).Range;
                        //wordcellrange.Text = p.РайонОбласти.Trim();

                        //// Населённый пункт.
                        //Word.Range wordcellrange2 = worddocument.Tables[countTable].Cell(countRows, 2).Range;
                        //wordcellrange2.Text = p.НаселённыйПункт.Trim();
                      

                        //// Фамилия.
                        //Word.Range wordcellrange3 = worddocument.Tables[countTable].Cell(countRows, 3).Range;
                        //wordcellrange3.Text = p.Фамилия.Trim();

                        //// Имя.
                        //Word.Range wordcellrange4 = worddocument.Tables[countTable].Cell(countRows, 4).Range;
                        //wordcellrange4.Text = p.Имя.Trim();

                        //// Отчество.
                        //Word.Range wordcellrange5 = worddocument.Tables[countTable].Cell(countRows, 5).Range;
                        //wordcellrange5.Text = p.Отчество.Trim();

                        //// Дата рождения.
                        //Word.Range wordcellrange6 = worddocument.Tables[countTable].Cell(countRows, 6).Range;
                        //wordcellrange6.Text = p.ДатаРождения.ToShortDateString().Trim();

                        //// Улица.
                        //Word.Range wordcellrange7 = worddocument.Tables[countTable].Cell(countRows, 7).Range;
                        //wordcellrange7.Text = p.Улица.Trim();

                        //// Номер дома.
                        //Word.Range wordcellrange8 = worddocument.Tables[countTable].Cell(countRows, 8).Range;
                        //wordcellrange8.Text = p.НомерДома.Trim();

                        //// Корпус.
                        //Word.Range wordcellrange9 = worddocument.Tables[countTable].Cell(countRows, 9).Range;
                        //wordcellrange9.Text = p.Корпус.Trim();

                        //// Номер квартиры.
                        //Word.Range wordcellrange10 = worddocument.Tables[countTable].Cell(countRows, 10).Range;
                        //wordcellrange10.Text = p.НомерКвартиры.Trim();

                        //// Серия документа.
                        //Word.Range wordcellrange11 = worddocument.Tables[countTable].Cell(countRows, 11).Range;
                        //wordcellrange11.Text = p.СерияДокумента.Trim();

                        //// Номер документа.
                        //Word.Range wordcellrange12 = worddocument.Tables[countTable].Cell(countRows, 12).Range;
                        //wordcellrange12.Text = p.НомерДокумента.Trim();

                        //// Льготная категория.
                        //Word.Range wordcellrange13 = worddocument.Tables[countTable].Cell(countRows, 13).Range;
                        //wordcellrange13.Text = p.ЛьготнаяКатегория.Trim();
                        #endregion

                       
                            #region Сокращённая таблица
                            // Район области.
                            //Word.Range wordcellrange = worddocument.Tables[countTable].Cell(countRows, 1).Range;
                            //wordcellrange.Text = p.РайонОбласти.Trim();

                            // Населённый пункт.
                            Word.Range wordcellrange2 = worddocument.Tables[countTable].Cell(countRows, 1).Range;
                            wordcellrange2.Text = p.НаселённыйПункт.Trim();


                            // Фамилия.
                            Word.Range wordcellrange3 = worddocument.Tables[countTable].Cell(countRows, 2).Range;
                            wordcellrange3.Text = p.Фамилия.Trim();

                            // Имя.
                            Word.Range wordcellrange4 = worddocument.Tables[countTable].Cell(countRows, 3).Range;
                            wordcellrange4.Text = p.Имя.Trim();

                            // Отчество.
                            Word.Range wordcellrange5 = worddocument.Tables[countTable].Cell(countRows, 4).Range;
                            wordcellrange5.Text = p.Отчество.Trim();

                            // Дата рождения.
                            Word.Range wordcellrange6 = worddocument.Tables[countTable].Cell(countRows, 5).Range;
                            wordcellrange6.Text = p.ДатаРождения.ToShortDateString().Trim();

                            // Улица.
                            Word.Range wordcellrange7 = worddocument.Tables[countTable].Cell(countRows, 6).Range;
                            wordcellrange7.Text = p.Улица.Trim();

                            // Номер дома.
                            Word.Range wordcellrange8 = worddocument.Tables[countTable].Cell(countRows, 7).Range;
                            wordcellrange8.Text = p.НомерДома.Trim();

                            // Корпус.
                            Word.Range wordcellrange9 = worddocument.Tables[countTable].Cell(countRows, 8).Range;
                            wordcellrange9.Text = p.Корпус.Trim();

                            // Номер квартиры.
                            Word.Range wordcellrange10 = worddocument.Tables[countTable].Cell(countRows, 9).Range;
                            wordcellrange10.Text = p.НомерКвартиры.Trim();

                            // Серия документа.
                            Word.Range wordcellrange11 = worddocument.Tables[countTable].Cell(countRows, 10).Range;
                            wordcellrange11.Text = p.СерияДокумента.Trim();

                            // Номер документа.
                            Word.Range wordcellrange12 = worddocument.Tables[countTable].Cell(countRows, 11).Range;
                            wordcellrange12.Text = p.НомерДокумента.Trim();

                            // Сумма выполненных работ.
                            Word.Range wordcellrange13 = worddocument.Tables[countTable].Cell(countRows, 12).Range;
                            wordcellrange13.Text = decimal.Parse(p.СуммаВыполненныхРабот).ToString("c");
                            #endregion
                        

                    }

                    countRows++;
                }

                countTable++;
            }

            //// Выведим информацию в ячейку.
            //Word.Range wordcellrange = worddocument.Tables[1].Cell(1, 1).Range;
            //wordcellrange = wordtable1.Cell(1, 1).Range;

            //wordcellrange.Text = "Строка для вывода";

         
        }

    }
}
