using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class УслугиПоДоговоруRepository : IRepository<УслугиПоДоговору>
    {
        private DataClasses1DataContext dc;

        public УслугиПоДоговоруRepository(DataClasses1DataContext dc)
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

        public void Insert(УслугиПоДоговору item)
        {
            dc.УслугиПоДоговору.InsertOnSubmit(item);

            dc.SubmitChanges();
        }

        public IEnumerable<УслугиПоДоговору> Select(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(УслугиПоДоговору item)
        {
            throw new NotImplementedException();
        }
    }
}
