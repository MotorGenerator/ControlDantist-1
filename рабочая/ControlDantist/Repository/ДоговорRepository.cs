using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class ДоговорRepository : IRepository<Договор>
    {
        private DataClasses1DataContext dc;

        public ДоговорRepository(DataClasses1DataContext dc)
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

        public void Insert(Договор item)
        {
            dc.Договор.InsertOnSubmit(item);

            dc.SubmitChanges();
        }

        public IEnumerable<Договор> SelectAll()
        {
            return dc.Договор;
        }

        public Договор FiltrДоговор(int idContract)
        {
            return dc.Договор.Where(w => w.id_договор == idContract).FirstOrDefault();
        }

        /// <summary>
        /// Возвращает проекты договоров
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Договор> Select(int id)
        {
            return dc.Договор.Where(w => w.idFileRegistProgect == id && w.flagОжиданиеПроверки == false && w.flagАнулирован == false).AsQueryable<Договор>();
        }

        public void Update(Договор item)
        {
            dc.SubmitChanges();
        }

        //public void Save()
        //{
        //    dc.SubmitChanges();
        //}

        /// <summary>
        /// Возвращает договор.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Договор SelectContract(int id)
        {
            return dc.Договор.Where(w => w.id_договор == id).FirstOrDefault();
        }
    }
}
