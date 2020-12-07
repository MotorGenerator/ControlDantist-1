using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Выгрузка данных (Паттерн адаптер).
    /// </summary>
    public interface IFindContract
    {
        List<ValideContract> Adapter();
    }
}
