using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repozirories
{
    public class RepositoryRegion
    {
        private DataClasses1DataContext dc;

        public RepositoryRegion(DataClasses1DataContext dcc)
        {
            dc = dcc;
        }

        /// <summary>
        /// Возвращает выбранный район.
        /// </summary>
        /// <param name="idRegion"></param>
        /// <returns></returns>
        public РайонОбласти getRegion(int idRegion)
        {
            return dc.РайонОбласти.Where(w => w.idRegion == idRegion).FirstOrDefault();
        }

        /// <summary>
        /// Возвращает все районы области.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<РайонОбласти> GetAll()
        {
            return dc.РайонОбласти.Select(w => w);
        }
    }
}
