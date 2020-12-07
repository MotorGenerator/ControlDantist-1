using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    /// <summary>
    /// Применим паттерн декоратор.
    /// </summary>
    public class RepositoryДоговорWhere : ДоговорRepository
    {
        private DataClasses1DataContext dc;

        private ДоговорRepository договорRepository;

        //public RepositoryДоговорWhere(DataClasses1DataContext dc) : base(dc)
        public RepositoryДоговорWhere(DataClasses1DataContext dc, ДоговорRepository договорRepository) : base(dc)
        {
            this.dc = dc;

            this.договорRepository = договорRepository;
        }

        /// <summary>
        /// Поиск договора по номеру.
        /// </summary>
        /// <param name="numContract"></param>
        /// <returns></returns>
        public IEnumerable<ViewАктДоговор> WhereNumContract(string numContract)
        {
            // Это конечно не паттерн декоратор.
            //return this.договорRepository.SelectAll().Where(w => w.НомерДоговора.ToLower().Trim() == numContract.Trim().ToLower() && w.ФлагПроверки == true);
            
            return this.dc.ViewАктДоговор.Where(w => w.НомерДоговора.ToLower().Trim() == numContract.Trim().ToLower() && w.ФлагПроверки == true && w.НомерАкта != null);

            //return this.dc.Договор.Where(w => w.НомерДоговора.ToLower().Trim() == numContract.Trim().ToLower() && w.ФлагПроверки == true);
        }

        public void Update(int idContract, bool flagValidate)
        {
            var contract = this.договорRepository.SelectContract(idContract);

            if(contract != null)
            {
                contract.ФлагПроверки = flagValidate;
            }
        }

       
    }
}
