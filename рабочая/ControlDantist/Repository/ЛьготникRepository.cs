using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class ЛьготникRepository : IRepository<Льготник>, IFiltr<Льготник>
    {
        private DataClasses1DataContext dc;
        public ЛьготникRepository(DataClasses1DataContext dc)
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

        public void Insert(Льготник item)
        {
            dc.Льготник.InsertOnSubmit(item);

            dc.SubmitChanges();
        }

        public IEnumerable<Льготник> SelectAll()
        {
            return dc.Льготник;
        }

        public Льготник FiltrЛьготник(int idЛьготник)
        {
            return dc.Льготник.Where(w => w.id_льготник == idЛьготник).FirstOrDefault();
        }

        public IEnumerable<Льготник> Select(int id)
        {
            return dc.Льготник.Where(w => w.id_льготник == id);
        }

        public Льготник Select(Льготник item)
        {

            List<Льготник> db = new List<Льготник>();

            Льготник л = new Льготник();

            л.Фамилия = "Лемашин";
            л.Имя = "Анатолий";
            л.Отчество = "Сергеевич";

            DateTime dr = new DateTime(1937, 10, 15);
            л.ДатаРождения = dr;

            db.Add(л);

            var result = db.Where(w => w.Фамилия.Trim().ToLower() == item.Фамилия.ToLower().Trim()
            && w.Имя.Trim().ToLower() == item.Имя.ToLower().Trim()
            && w.Отчество.Trim().ToLower() == item.Отчество.ToLower().Trim()
            && w.ДатаРождения.Value.Date == item.ДатаРождения.Value.Date).FirstOrDefault();


            //var result = dc.Льготник.Where(w => w.Фамилия.Trim().ToLower() == item.Фамилия.ToLower().Trim()
            //&& w.Имя.Trim().ToLower() == item.Имя.ToLower().Trim()
            //&& w.Отчество.Trim().ToLower() == item.Отчество.ToLower().Trim()).FirstOrDefault();
            //

            return result;
            //&& w.улица.ToLower().Trim() == item.улица.ToLower().Trim()
            //&& w.НомерДома.ToLower().Trim() == item.НомерДома.Replace("'",string.Empty).ToLower().Trim()
            //&& w.корпус.ToLower().Trim() == item.корпус.Replace("'", string.Empty).ToLower().Trim()
            //&& w.НомерКвартиры.ToLower().Trim() == item.НомерКвартиры.Replace("'", string.Empty).ToLower().Trim()
            //&& w.СерияПаспорта.ToLower().Trim() == item.СерияПаспорта.Replace("'", string.Empty).ToLower().Trim()
            //&& w.НомерПаспорта.ToLower().Trim() == item.НомерПаспорта.Replace("'", string.Empty).ToLower().Trim()
            //&& w.ДатаВыдачиПаспорта.Value.ToShortDateString().ToLower().Trim() == item.ДатаВыдачиПаспорта.Value.ToShortDateString().ToLower().Trim()
            //&& w.КемВыданПаспорт.ToLower().Trim() == item.КемВыданПаспорт.Replace("'", string.Empty).ToLower().Trim()
            //&& w.КемВыданПаспорт.ToLower().Trim() == item.КемВыданПаспорт.Replace("'", string.Empty).ToLower().Trim()).FirstOrDefault();
        }

        public void Update(Льготник item)
        {
            throw new NotImplementedException();
        }
    }
}
