using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class ПоликлинникаИННRepository :  IRepository<ПоликлинникиИнн>, IReppositorySelectAll<ПоликлинникиИнн> //,IFiltrHospital<ПоликлинникиИнн>
    {
        private DataClasses1DataContext dc;

        public ПоликлинникаИННRepository(DataClasses1DataContext dc)
        {
            this.dc = dc;

            var conString = this.dc.Connection.ConnectionString;
        }

        //public List<ПоликлинникиИнн> FiltrHospital(string nameHospitel)
        //{
        //    var rez = from inn in dc.ПоликлинникиИнн
        //              where inn.F2.Contains(nameHospitel)
        //              select new ПоликлинникиИнн
        //              {
        //                  F1 = inn.F1,
        //                  F2 = inn.F2,
        //                  F3 = inn.F3;
        //              }

        //        return rez;
        //}

        /// <summary>
        /// Возвращает список поликлинник.
        /// </summary>
        /// <returns></returns>
        public List<ПоликлинникиИнн> SelectAll()
        {
            return dc.ПоликлинникиИнн.Select(w => new ПоликлинникиИнн { F1 = w.F1, F2 = w.F2.Replace("ГУЗ СО",string.Empty).Replace("ГУЗ",string.Empty).Replace("«",string.Empty).Replace("»",string.Empty) }).OrderBy(w=>w.F1).ToList();
        }

        /// <summary>
        /// Возвращает id поликлинники.
        /// </summary>
        /// <param name="инн"></param>
        /// <returns></returns>
        public int SelectИнн(string инн)
        {

           ПоликлинникиИнн поликлинникиИнн = dc.ПоликлинникиИнн.Where(w => w.F3.ToString().Trim() == инн.ToString().Trim()).FirstOrDefault();

           return dc.Поликлинника.Where(w => w.ИНН.Trim() == поликлинникиИнн.F3.ToString().Trim()).Max(w => w.id_поликлинника);

        }

        void IRepository<ПоликлинникиИнн>.Delete(int id)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        //public List<ПоликлинникиИнн> FiltrHospital(string nameHospitel)
        //{
        //    //return dc.ПоликлинникиИнн.Where(w => w.F2.Contains(nameHospitel));

        //    var rez = from inn in dc.ПоликлинникиИнн
        //              where inn.F2.Contains(nameHospitel)
        //              select new ПоликлинникиИнн
        //              {
        //                  F1 = inn.F1,
        //                  F2 = inn.F2,
        //                  F3 = inn.F3;
        //              }

        //    return rez;
        //}



        void IRepository<ПоликлинникиИнн>.Insert(ПоликлинникиИнн item)
        {
            throw new NotImplementedException();
        }

        IEnumerable<ПоликлинникиИнн> IRepository<ПоликлинникиИнн>.Select(int id)
        {
            throw new NotImplementedException();
        }

        void IRepository<ПоликлинникиИнн>.Update(ПоликлинникиИнн item)
        {
            throw new NotImplementedException();
        }
    }
}
