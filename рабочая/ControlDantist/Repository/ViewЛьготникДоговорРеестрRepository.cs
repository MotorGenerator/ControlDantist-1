using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class ViewЛьготникДоговорРеестрRepository : IRepository<ViewЛьготникДоговорРеестр>
    {
        private DataClasses1DataContext dc;

        public ViewЛьготникДоговорРеестрRepository(DataClasses1DataContext dc)
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

        public void Insert(ViewЛьготникДоговорРеестр item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewЛьготникДоговорРеестр> Select(int id)
        {
            return dc.ViewЛьготникДоговорРеестр.Where(w => w.idFileRegistProgect == id && w.flagОжиданиеПроверки == false);
        }

        public void Update(ViewЛьготникДоговорРеестр item)
        {
            throw new NotImplementedException();
        }
    }
}
