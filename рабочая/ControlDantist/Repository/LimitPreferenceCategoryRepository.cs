using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class LimitPreferenceCategoryRepository : IRepository<LimitPreferenceCategory>
    {
        private DataClasses1DataContext dc;

        public LimitPreferenceCategoryRepository(DataClasses1DataContext dc)
        {
            this.dc = dc;
        }

        public void Delete(int id)
        {
            var deleteCategory = dc.LimitPreferenceCategory.Where(w => w.idLimitMoney == id);

            if(deleteCategory != null)
            {
                foreach(var itm in deleteCategory)
                {
                    dc.LimitPreferenceCategory.DeleteOnSubmit(itm);

                    dc.SubmitChanges();
                }
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Insert(LimitPreferenceCategory item)
        {
            dc.LimitPreferenceCategory.InsertOnSubmit(item);

            dc.SubmitChanges();
        }

        public IEnumerable<LimitPreferenceCategory> Select(int id)
        {
            return dc.LimitPreferenceCategory.Where(w => w.idLimitMoney == id).Select(w => w);
        }

        public void Update(LimitPreferenceCategory item)
        {
            throw new NotImplementedException();
        }
    }
}
