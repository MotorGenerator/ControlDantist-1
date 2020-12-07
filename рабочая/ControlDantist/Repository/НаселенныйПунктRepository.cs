using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class НаселенныйПунктRepository : IRepository<НаселённыйПункт>, ISelectAll<НаселённыйПункт>
    {
        private DataClasses1DataContext dc;

        public НаселенныйПунктRepository(DataClasses1DataContext dc)
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

        public void Insert(НаселённыйПункт item)
        {
            dc.НаселённыйПункт.InsertOnSubmit(item);

            dc.SubmitChanges();
        }

        public IEnumerable<НаселённыйПункт> Select(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<НаселённыйПункт> SelectAll()
        {
            return dc.НаселённыйПункт;
        }

        public НаселённыйПункт FiltrНаселенныйПункт(int idCity)
        {
            return dc.НаселённыйПункт.Where(w => w.id_насПункт == idCity).FirstOrDefault();
        }

        public НаселённыйПункт FiltrНаселенныйПункт(string cityName)
        {
            return dc.НаселённыйПункт.Where(w => w.Наименование.ToLower().Trim() == cityName.ToLower().Trim()).FirstOrDefault();
        }

        public void Update(НаселённыйПункт item)
        {
            throw new NotImplementedException();
        }
    }
}
