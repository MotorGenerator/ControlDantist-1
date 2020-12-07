using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repozirories
{
    /// <summary>
    /// Репозитории доступа к БД.
    /// </summary>
    public class UnitWork
    {
        private DataClasses1DataContext dc;

        // Переменная для доступа к репозиторию RepositoryViewPersonVipNet.
        private RepositoryViewPersonVipNet repositoryDocsToVipNet;

        private RepositoryViewPersonVipNet2 repositoryViewPersonVipNet2;

        private RepositoryRegion region;

        private NameOrganizationVipNetRepository organizationVipNet;

        public UnitWork()
        {
            dc = new DataClasses1DataContext();
        }

        /// <summary>
        /// Доступ к репозиторию район области.
        /// </summary>
        public RepositoryRegion RegionRepository
        {
            get
            {
                if (region == null)
                    region = new RepositoryRegion(dc);
                return region;
            }
        }

        /// <summary>
        /// Доступ к репозиторию представления вызова документов для VipNet.
        /// </summary>
        public RepositoryViewPersonVipNet2 RepositoryViewPersonVipNet2
        {
            get
            {
                if (repositoryViewPersonVipNet2 == null)
                    repositoryViewPersonVipNet2 = new RepositoryViewPersonVipNet2(dc);
                return repositoryViewPersonVipNet2;
            }

        }

        /// <summary>
        /// Доступ к репозиторию представления вызова документов для VipNet.
        /// </summary>
        public RepositoryViewPersonVipNet RepositoryDocsToVipNet
        {
            get
            {
                if (repositoryDocsToVipNet == null)
                    repositoryDocsToVipNet = new RepositoryViewPersonVipNet(dc);
                return repositoryDocsToVipNet;
            }
            
        }

        /// <summary>
        /// Доступ к репозиторию таблицы описывающей организации области для отправления писем по VipNet.
        /// </summary>
        public NameOrganizationVipNetRepository OrganizationVipNet
        {
            get
            {
                if (organizationVipNet == null)
                    organizationVipNet = new NameOrganizationVipNetRepository(dc);

                return organizationVipNet;
            }
        }
    }
}
