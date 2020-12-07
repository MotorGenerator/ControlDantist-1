using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class ЛЬготникAddRepository : IRepository<ЛьготникAdd>
    {

        private DataClasses1DataContext dc;

        public ЛЬготникAddRepository(DataClasses1DataContext dc)
        {
            this.dc = dc;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Insert(ЛьготникAdd item)
        {
            dc.ЛьготникAdd.InsertOnSubmit(item);

            dc.SubmitChanges();
        }

        public IEnumerable<ЛьготникAdd> Select(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ЛьготникAdd item)
        {
            ЛьготникAdd itRez = new ЛьготникAdd();

            if (item.Отчество != null && item.Отчество != "")
            {
                itRez = dc.ЛьготникAdd.Where(w => w.Фамилия.Replace("ё", "е").ToLower().Trim() == item.Фамилия.Replace("ё", "е").ToLower().Trim() &&
                w.Имя.Replace("ё", "е").ToLower().Trim() == item.Имя.Replace("ё", "е").ToLower().Trim() && w.Отчество.Replace("ё", "е").ToLower().Trim() == item.Отчество.Replace("ё", "е").ToLower().Trim()
                && w.ДатаРождения == item.ДатаРождения).FirstOrDefault();
            }
            else
            {
                itRez = dc.ЛьготникAdd.Where(w => w.Фамилия.Replace("ё", "е").ToLower().Trim() == item.Фамилия.Replace("ё", "е").ToLower().Trim() &&
                w.Имя.Replace("ё", "е").ToLower().Trim() == item.Имя.Replace("ё", "е").ToLower().Trim() && w.ДатаРождения == item.ДатаРождения).FirstOrDefault();
            }

           

                // Обновим данные по льготнику.
                itRez.id_документ = item.id_документ;
                itRez.id_льготнойКатегории = item.id_льготнойКатегории;
                itRez.id_насПункт = item.id_насПункт;
                itRez.id_область = 1;
                itRez.id_район = item.id_район;
                itRez.ДатаВыдачиДокумента = item.ДатаВыдачиДокумента;

                if (itRez.Фамилия == "Гринь" && itRez.Имя == "Владимир" && itRez.Отчество == "Иванович")
                {
                    DateTime dt = new DateTime(2002, 9, 11);
                    itRez.ДатаВыдачиПаспорта = dt;
                }
                else
                {
                    itRez.ДатаВыдачиПаспорта = item.ДатаВыдачиПаспорта;
                }

                itRez.КемВыданДокумент = item.КемВыданДокумент;
                itRez.КемВыданПаспорт = item.КемВыданПаспорт;
                itRez.корпус = item.корпус;
                itRez.НомерДокумента = item.НомерДокумента;
                itRez.НомерДома = item.НомерДома;
                itRez.НомерКвартиры = item.НомерКвартиры;
                itRez.НомерПаспорта = item.НомерПаспорта;
                itRez.СерияДокумента = item.СерияДокумента;
                itRez.СерияПаспорта = item.СерияПаспорта;
                itRez.Снилс = item.Снилс;
                itRez.улица = item.улица;

                // Сохраним изменения.
                dc.SubmitChanges();
        }

        public ЛьготникAdd SelectPerson(ЛьготникAdd item)
        {
            ЛьготникAdd itRez = new ЛьготникAdd();

            if(item.Отчество != null && item.Отчество != "")
            {
                //item.Фамилия = it.Фамилия;
                //item.Имя = it.Имя;
                //item.Отчество = it.Отчество;

                itRez = dc.ЛьготникAdd.Where(w => w.Фамилия.Replace("ё", "е").ToLower().Trim() == item.Фамилия.Replace("ё","е").ToLower().Trim() &&
                w.Имя.Replace("ё", "е").ToLower().Trim() == item.Имя.Replace("ё", "е").ToLower().Trim() && w.Отчество.Replace("ё", "е").ToLower().Trim() == item.Отчество.Replace("ё", "е").ToLower().Trim()
                && w.ДатаРождения == item.ДатаРождения).FirstOrDefault();
            }
            else
            {
                itRez = dc.ЛьготникAdd.Where(w => w.Фамилия.Replace("ё", "е").ToLower().Trim() == item.Фамилия.Replace("ё", "е").ToLower().Trim() &&
                w.Имя.Replace("ё", "е").ToLower().Trim() == item.Имя.Replace("ё", "е").ToLower().Trim() && w.ДатаРождения == item.ДатаРождения).FirstOrDefault();
            }

            return itRez;

        }

    }
}
