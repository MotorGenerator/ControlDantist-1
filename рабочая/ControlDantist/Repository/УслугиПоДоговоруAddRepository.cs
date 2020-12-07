using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class УслугиПоДоговоруAddRepository : IRepository<УслугиПоДоговоруAdd>
    {
        private DataClasses1DataContext dc;

        public УслугиПоДоговоруAddRepository(DataClasses1DataContext dc)
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

        public void Insert(УслугиПоДоговоруAdd item)
        {
            dc.УслугиПоДоговоруAdd.InsertOnSubmit(item);

            dc.SubmitChanges();
        }

        public IEnumerable<УслугиПоДоговоруAdd> Select(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(УслугиПоДоговоруAdd item)
        {
            throw new NotImplementedException();
        }
    }
}
