using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ControlDantist.Repozirories
{
    /// <summary>
    /// Репозиторий для доступа к БД отчета VipNet.
    /// </summary>
    public class RepositoryViewPersonVipNet : IRepositoryDocVipNet<ViewPersonVipNet>
    {

        private DataClasses1DataContext dc;

        public RepositoryViewPersonVipNet(DataClasses1DataContext dcc)
        {
            dc = dcc;
        }

        public IEnumerable<ViewPersonVipNet> GetDocs(int idRegion, DateTime dtStart, DateTime dtEnd)
        {
            var list = dc.ViewPersonVipNet.Where(w => w.idRegion == idRegion && w.ДатаЗаписиДоговора >= dtStart && w.ДатаЗаписиДоговора <= dtEnd);
            //var list =   .Where(w => w.idRegion == idRegion && w.ДатаЗаписиДоговора >= dtStart && w.ДатаЗаписиДоговора <= dtEnd);

            return list;
        }

        public void Dispose()
        {
            Dispose();

            // Сообщает системе о запрете вызова данного класса.
            GC.SuppressFinalize(this);
        }
    }
}
