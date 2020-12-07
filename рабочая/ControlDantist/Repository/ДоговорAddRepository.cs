using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class ДоговорAddRepository : IRepository<ДоговорAdd>
    {
        private DataClasses1DataContext dc;

        public ДоговорAddRepository(DataClasses1DataContext dc)
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

        public void Insert(ДоговорAdd item)
        {
            dc.ДоговорAdd.InsertOnSubmit(item);

            dc.SubmitChanges();
        }

        public IEnumerable<ДоговорAdd> Select(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ДоговорAdd item)
        {
            throw new NotImplementedException();
        }
    }
}
