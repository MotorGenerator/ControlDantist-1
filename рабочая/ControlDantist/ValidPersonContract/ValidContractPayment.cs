using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Repository;

namespace ControlDantist.ValidPersonContract
{
    public static class ValidContractPayment
    {
        public static string ExecActPayment(string numberContract)
        {
            UnitDate unitDate = new UnitDate();
            unitDate.RepositoryДоговорWhere.WhereNumContract(numberContract);

            return null;
        }
    }
}
