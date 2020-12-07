using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repozirories
{
    /// <summary>
    /// Репозиторий для доступа к таблице NameOrganizationVipNet.
    /// </summary>
    public class NameOrganizationVipNetRepository
    {
        private DataClasses1DataContext dc;

        public NameOrganizationVipNetRepository(DataClasses1DataContext dcc)
        {
            dc = dcc;
        }

        public NameOrganizationVipNet GetOrganization(int idRegion)
        {
            return dc.NameOrganizationVipNet.Where(w => w.idRegion == idRegion).FirstOrDefault();
        }
    }
}
