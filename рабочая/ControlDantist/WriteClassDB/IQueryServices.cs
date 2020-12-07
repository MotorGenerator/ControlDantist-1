using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.WriteClassDB
{
    /// <summary>
    /// ВУспомогательный класс для формирования запроса на добавление услуг по договору.
    /// </summary>
    interface IQueryServices
    {
        string Query(string id_contract);

        string QueryReceptionContract(string id_contract);
    }
}
