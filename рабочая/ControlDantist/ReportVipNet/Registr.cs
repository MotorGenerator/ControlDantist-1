using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Repozirories;
using ControlDantist.Classes;
using System.Windows.Forms;

namespace ControlDantist.ReportVipNet
{
    public class Registr : IRegistr
    {

        private List<Person> listPerson;
        UnitWork unit;

        /// <summary>
        /// ID выбранного района.
        /// </summary>
        public int IdRegion { get; set; }

        /// <summary>
        /// Дата начала отчетного периода.
        /// </summary>
        public DateTime DateStartPeriod { get; set; }

        /// <summary>
        /// Название района области.
        /// </summary>
        public string NameRegion { get; set; }

        /// <summary>
        /// Дата окончания отчетного периода.
        /// </summary>
        public DateTime DateEndPeriod { get; set; }

        public Registr(UnitWork uunit)
        {
            // Создадим экземпляр списка льготников описывающих в отчете.
            listPerson = new List<Person>();

            unit = uunit;
        }


        public bool GetPersons()
        {
            bool flagLetter = false;

            // 
            var persons = unit.RepositoryViewPersonVipNet2.GetDocs(this.IdRegion, this.DateStartPeriod.Date, this.DateEndPeriod.Date);

            if (persons.Count() > 0)
            {
                flagLetter = true;

                ConvertToPerson(persons);

                SaveFileDialog fileDialog = new SaveFileDialog();

                string period = "_от_" + this.DateStartPeriod.Date.ToShortDateString() + "_до_" + this.DateEndPeriod.Date.ToShortDateString();

                // Получим название файла.
                string nameFile = this.NameRegion + period + ".xls";

                fileDialog.FileName = nameFile;

                // Откроем диалог сохранения файла.
                DialogResult result = fileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // Создадим экземпляр сохранения файлов.
                    IFile fileExcel = new ExcelFileSave(listPerson);

                    fileExcel.Period = period;
                    
                    // Сохраним файл на диск.
                    fileExcel.Save(fileDialog.FileName);

                    if (fileExcel.FlagSave() == true)
                    {
                        // Сформируем письмо VipNet.
                        WordPageVipNet letterVipNet = new WordPageVipNet(this.IdRegion);
                        letterVipNet.CreateLetter(fileDialog.FileName);
                    }
                }

            }

            return flagLetter;
        }

        /// <summary>
        /// Конветрор для совместимости.
        /// </summary>
        /// <returns></returns>
        private void ConvertToPerson(IEnumerable<ViewPersonVipNet2> listPersons)
        {
            foreach (var itm in listPersons)
            {
                Person person = new Person();
                person.Region = itm.NameRegion;

                // Наименование населенного пункта.
                person.City = itm.Наименование;

                if (itm.Фамилия != null)
                {
                    person.FirstName = itm.Фамилия;
                }

                person.Name = itm.Имя.Do(w => w, ""); ;
                person.SecondName = itm.Отчество.Do(w => w, "");

                person.DateBirth = itm.ДатаРождения.DoDateTime();

                person.Street = itm.Улица;

                person.NumBobyBuilder = itm.Корпус;

                person.NumHous = itm.НомерДома;

                person.NumApartment = itm.НомерКвартиры;

                person.SeriesDoc = itm.СерияДокумента;

                person.NumDoc = itm.НомерДокумента;

                person.SumAct = itm.СуммаАктаВыполненныхРабот.DoDecimalNull(0.0m);

                person.DateWriteAct = itm.ДатаПодписания.DoDateTime();

                listPerson.Add(person);

            }

        }


        ///// <summary>
        ///// Конветрор для совместимости.
        ///// </summary>
        ///// <returns></returns>
        //private void ConvertToPerson(IEnumerable<ViewPersonVipNet> listPersons)
        //{
        //    foreach (var itm in listPersons)
        //    {
        //        Person person = new Person();
        //        person.Region = itm.NameRegion;

        //        // Наименование населенного пункта.
        //        person.City = itm.Наименование;

        //        if (itm.Фамилия != null)
        //        {
        //            person.FirstName = itm.Фамилия;
        //        }

        //        person.Name = itm.Имя.Do(w => w, ""); ;
        //        person.SecondName = itm.Отчество.Do(w => w, "");

        //        person.DateBirth = itm.ДатаРождения.DoDateTime();

        //        person.Street = itm.улица;

        //        person.NumBobyBuilder = itm.корпус;

        //        person.NumHous = itm.НомерДома;

        //        person.NumApartment = itm.НомерКвартиры;

        //        person.SeriesDoc = itm.СерияДокумента;

        //        person.NumDoc = itm.НомерДокумента;

        //        person.SumAct = itm.СуммаАктаВыполненныхРабот.DoDecimalNull(0.0m);

        //        person.DateWriteAct = itm.ДатаПодписания.DoDateTime();

        //        listPerson.Add(person);

        //    }

        //}
    }
}
