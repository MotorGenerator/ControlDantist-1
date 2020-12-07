using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class YearRepository : IRepository<Year>, IFiltr<Year>
    {
        private DataClasses1DataContext dc;

        public YearRepository(DataClasses1DataContext dc)
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

        public void Insert(Year item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Year> Select(int id)
        {
            throw new NotImplementedException();
        }

        public Year Select(Year item)
        {
            return dc.Year.Where(w => w.Year1 == item.Year1).FirstOrDefault();
        }

        public void Update(Year item)
        {
            throw new NotImplementedException();
        }
    }
}
