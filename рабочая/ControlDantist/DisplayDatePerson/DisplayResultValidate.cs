using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Repository;
using ControlDantist.Classes;
using ControlDantist.ClassValidRegions;


namespace ControlDantist.DisplayDatePerson
{
    public class DisplayResultValidate
    {
        private int idContract = 0;

        private UnitDate unitDate;

        public DisplayResultValidate(int idContract)
        {
            this.idContract = idContract;

            unitDate = new UnitDate();
        }

        public DatePersonForDisplay GetFioPerson(Dictionary<string,PersonValidEsrn> list)
        {
            //var result = from c in unitDate.ДоговорRepository.SelectAll()
            //             join p in unitDate.ЛьготникRepository.SelectAll()
            //             on c.id_льготник equals p.id_льготник
            //             join t in unitDate.НаселенныйПунктRepository.SelectAll()
            //             on p.id_насПункт  equals t.id_насПункт
            //             where c.id_договор == idContract
            //             select new { ФИО = p.Фамилия + " " + p.Имя + " " + p.Отчество.Do(w => w, ""), Адрес = " г. " + t.Наименование + " ул. " + p.улица + " корп. " + p.корпус + " д. " + p.НомерДома + " кв. "  + p.НомерКвартиры, Удостоверение = "серия " + p.СерияДокумента + "№ " + p.НомерДокумента };

            var persons = list.Values.Where(w => w.IdContract == this.idContract);//.Select(new DatePersonForDisplay { Фио = w.Фамилия + " " + p.Имя + " " + p.Отчество.Do(w => w, ""))

            DatePersonForDisplay dp = persons.Select(w => new DatePersonForDisplay { Фио = w.фамилия + " " + w.имя + " " + w.отчество.Do(x => x, ""), Адрес = w.Адрес, Удостоверение = "серия " + w.серияДокумента + "№ " + w.номерДокумента, ДатаРождения = w.датаРождения.Trim() }).FirstOrDefault();

            return dp;
        }
    }
}
