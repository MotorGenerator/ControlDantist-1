using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class UnitDate
    {
        private DataClasses1DataContext dc;

        // Таблица с названиями поликлинник.
        private ПоликлинникаИННRepository поликлинникаИННRepository;

        // Таблица с файлами реестров проектов договоров.
        private ProjectRegistrFilesRepository projectRegistrFilesRepository;

        // Список номеров проектов договооров.
        private ListNumberProjectRepository listNumberProjectRepository;

        private ЛьготникRepository льготникRepository;

        private ЛьготнаяКатегорияRepository льготнаяКатегория;

        private LimitMoneyRepository limitMoneyRepository;

        private LimitPreferenceCategoryRepository limitPreferenceCategoryRepository;

        private YearRepository yearRepository;

        private ДоговорRepository договорRepository;

        private ViewЛьготникДоговорРеестрRepository льготникДоговорРеестрRepository;

        private НаселенныйПунктRepository населенныйПунктRepository;

        private RepositoryДоговорWhere repositoryДоговорWhere;

        private ЛЬготникAddRepository льготникAddRepository;

        private ДоговорAddRepository договорAddRepository;

        private УслугиПоДоговоруAddRepository услугиПоДоговоруAddRepository;

        private УслугиПоДоговоруRepository услугиПоДоговоруRepository;

        public UnitDate()
        {
            dc = new DataClasses1DataContext();
        }

        public DataClasses1DataContext DateContext
        {
            get
            {
                return this.dc;
            }
        }

        /// <summary>
        /// Доступ к репозиторию к таблице услуги по договору.
        /// </summary>
        public УслугиПоДоговоруRepository УслугиПоДоговоруRepository
        {
            get
            {
                if (услугиПоДоговоруRepository == null)
                    услугиПоДоговоруRepository = new УслугиПоДоговоруRepository(dc);
                return услугиПоДоговоруRepository;
            }
        }

        public УслугиПоДоговоруAddRepository УслугиПоДоговоруAddRepository
        {
            get
            {
                if (услугиПоДоговоруAddRepository == null)
                    услугиПоДоговоруAddRepository = new УслугиПоДоговоруAddRepository(dc);
                return услугиПоДоговоруAddRepository;
            }
        }

        public ДоговорAddRepository ДоговорAddRepository
        {
            get
            {
                if (договорAddRepository == null)
                    договорAddRepository = new ДоговорAddRepository(dc);
                return договорAddRepository;
            }
        }

        public ЛЬготникAddRepository ЛЬготникAddRepository
        {
            get
            {
                if (льготникAddRepository == null)
                    льготникAddRepository = new ЛЬготникAddRepository(dc);
                return льготникAddRepository;
            }
        }

        public RepositoryДоговорWhere RepositoryДоговорWhere
        {
            get
            {
                if (repositoryДоговорWhere == null)
                    repositoryДоговорWhere = new RepositoryДоговорWhere(dc,ДоговорRepository);
                return repositoryДоговорWhere;
            }
        }

        public НаселенныйПунктRepository НаселенныйПунктRepository
        {
            get
            {
                if (населенныйПунктRepository == null)
                    населенныйПунктRepository = new НаселенныйПунктRepository(dc);
                return населенныйПунктRepository;
            }
        }

        public ViewЛьготникДоговорРеестрRepository ViewЛьготникДоговорРеестрRepository
        {
            get
            {
                if (льготникДоговорРеестрRepository == null)
                    льготникДоговорРеестрRepository = new ViewЛьготникДоговорРеестрRepository(dc);
                return льготникДоговорРеестрRepository;
            }
        }

        public ДоговорRepository ДоговорRepository
        {
            get
            {
                if (договорRepository == null)
                    договорRepository = new ДоговорRepository(dc);
                return договорRepository;
            }
        }

        /// <summary>
        /// Репозиторий доступа к таблице LimitMoneyRepository.
        /// </summary>
        public YearRepository YearRepository
        {
            get
            {
                if (yearRepository == null)
                    yearRepository = new YearRepository(dc);
                return yearRepository;
            }
        }

        /// <summary>
        /// Репозиторий доступа к таблице LimitMoneyRepository.
        /// </summary>
        public LimitMoneyRepository LimitMoneyRepository
        {
            get
            {
                if (limitMoneyRepository == null)
                    limitMoneyRepository = new LimitMoneyRepository(dc);
                return limitMoneyRepository;
            }
        }

        /// <summary>
        /// Репозиторий доступа к таблице LimitPreferenceCategoryRepository
        /// </summary>
        public LimitPreferenceCategoryRepository LimitPreferenceCategoryRepository
        {
            get
            {
                if (limitPreferenceCategoryRepository == null)
                    limitPreferenceCategoryRepository = new LimitPreferenceCategoryRepository(dc);
                return limitPreferenceCategoryRepository;
            }
        }

        /// <summary>
        /// Репозиторий доступа к таблице Льготник.
        /// </summary>
        public ЛьготнаяКатегорияRepository ЛьготнаяКатегорияRepository
        {
            get
            {
                if (льготнаяКатегория == null)
                    льготнаяКатегория = new ЛьготнаяКатегорияRepository(dc);
                return льготнаяКатегория;
            }
        }

        /// <summary>
        /// Репозиторий доступа к таблице Льготник.
        /// </summary>
        public ЛьготникRepository ЛьготникRepository
        {
            get
            {
                if (льготникRepository == null)
                    льготникRepository = new ЛьготникRepository(dc);
                return льготникRepository;
            }
        }

        /// <summary>
        /// Репозиторий доступа к таблице Поликлинника ИНН.
        /// </summary>
        public ПоликлинникаИННRepository ПоликлинникаИННRepository
        {
            get
            {
                if (поликлинникаИННRepository == null)
                    поликлинникаИННRepository = new ПоликлинникаИННRepository(dc);
                return поликлинникаИННRepository;
            }
        }

        /// <summary>
        /// Репозиторий доступа к таблице хранения файлов проектов договоров.
        /// </summary>
        public ProjectRegistrFilesRepository ProjectRegistrFilesRepository
        {
            get
            {
                if (projectRegistrFilesRepository == null)
                    projectRegistrFilesRepository = new ProjectRegistrFilesRepository(dc);
                return projectRegistrFilesRepository;
            }
        }

        /// <summary>
        /// Репозиторий доступа к таблице хранения списка проектов договоров.
        /// </summary>
        public ListNumberProjectRepository ListNumbersProgectsReestr
        {
            get
            {
                if (listNumberProjectRepository == null)
                    listNumberProjectRepository = new ListNumberProjectRepository(dc);
                return listNumberProjectRepository;
            }
        }


    }
}
