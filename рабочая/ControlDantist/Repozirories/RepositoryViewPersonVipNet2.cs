using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ControlDantist.Repozirories
{
    public class RepositoryViewPersonVipNet2 : IRepositoryDocVipNet<ViewPersonVipNet2>
    {
        private DataClasses1DataContext dc;

        public RepositoryViewPersonVipNet2(DataClasses1DataContext dcc)
        {
            dc = dcc;
        }

        public IEnumerable<ViewPersonVipNet2> GetDocs(int idRegion, DateTime dtStart, DateTime dtEnd)
        {
            var list = dc.ViewPersonVipNet2.Where(w => w.id_район == idRegion && w.ДатаПодписания >= dtStart && w.ДатаПодписания <= dtEnd);
            //var list =   .Where(w => w.idRegion == idRegion && w.ДатаЗаписиДоговора >= dtStart && w.ДатаЗаписиДоговора <= dtEnd);

            return list;
        }

        public void Dispose()
        {
            Dispose();

            // Сообщает системе о запрете вызова данного класса.
            GC.SuppressFinalize(this);
        }

        IEnumerable<ViewPersonVipNet2> IRepositoryDocVipNet<ViewPersonVipNet2>.GetDocs(int idRegion, DateTime dtStart, DateTime dtEnd)
        {
            throw new NotImplementedException();
        }
    }
}
