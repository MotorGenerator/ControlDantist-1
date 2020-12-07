using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Find
{
    /// <summary>
    /// Интерфейс описывающий поиск льготника по БД.
    /// </summary>
    public interface IFindPerson
    {
        string Query();
    }
}
