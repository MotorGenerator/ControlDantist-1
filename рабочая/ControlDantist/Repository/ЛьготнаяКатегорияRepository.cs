using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class ЛьготнаяКатегорияRepository : IRepository<ЛьготнаяКатегория>, ISelectAll<ЛьготнаяКатегория>
    {
        private DataClasses1DataContext dc;
        public ЛьготнаяКатегорияRepository(DataClasses1DataContext dc)
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

        public void Insert(ЛьготнаяКатегория item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ЛьготнаяКатегория> Select(int id)
        {
            return dc.ЛьготнаяКатегория.Where(w => w.id_льготнойКатегории == id);
        }


        public ЛьготнаяКатегория GetЛьготнаяКатегория(string льготнаяКатегория)
        {
            return dc.ЛьготнаяКатегория.Where(w => w.ЛьготнаяКатегория1.Replace(" ",string.Empty).ToLower().Trim() == льготнаяКатегория.Replace(" ",string.Empty).Trim().ToLower()).FirstOrDefault();
        }

        public void Update(ЛьготнаяКатегория item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает все записи таблицы льготные категории.
        /// </summary>
        /// <returns></returns>
        public IQueryable<ЛьготнаяКатегория> SelectAll()
        {
            return dc.ЛьготнаяКатегория.Select(w => w);
        }
    }
}
