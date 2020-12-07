using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public class FiltrRepositoryДоговор : IFiltrRepository<Договор>
    {
        //private ДоговорRepository договорRepository;

        private UnitDate unitDate;

        //public FiltrRepositoryДоговор(ДоговорRepository договорRepository)
        public FiltrRepositoryДоговор(UnitDate unitDate)
        {
            //договорRepository = договорRepository;
            this.unitDate = unitDate;
        }


        public Договор Select(int id)
        {
            var asd = unitDate.ДоговорRepository.SelectAll();

            return unitDate.ДоговорRepository.SelectAll().Where(w => w.id_договор == id).FirstOrDefault();
        }

     
    }
}
