using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class LimitMoneyRepository : IRepository<LimittMoney>
    {
        private DataClasses1DataContext dc;
        public LimitMoneyRepository(DataClasses1DataContext dc)
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

        public void Insert(LimittMoney item)
        {
            dc.LimittMoney.InsertOnSubmit(item);

            dc.SubmitChanges();
        }

        public IEnumerable<LimittMoney> Select(int id)
        {
            return dc.LimittMoney.Where(w => w.idLimitMoney == id);
        }

        public IEnumerable<LimittMoney> SelectToYear(int idYear)
        {
            return dc.LimittMoney.Where(w => w.idYear == idYear);
        }

        public void Update(LimittMoney item)
        {
            var itmUpdate = dc.LimittMoney.Where(w => w.idLimitMoney == item.idLimitMoney).FirstOrDefault();

            if(itmUpdate != null)
            {
                itmUpdate.Limit = item.Limit;
            }

            dc.SubmitChanges();

        }
    }
}
