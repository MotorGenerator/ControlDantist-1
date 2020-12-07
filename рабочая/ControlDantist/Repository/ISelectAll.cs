using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    /// <summary>
    /// Интерфейс возвращение всех записей.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface ISelectAll<T>
    {
        IQueryable<T> SelectAll();
    }
}
