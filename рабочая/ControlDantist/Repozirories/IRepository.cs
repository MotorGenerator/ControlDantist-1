using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repozirories
{
    interface IRepository<T> : IDisposable
        where T : class
    {
        void Insert(T item);
        void Update(T item);
        IEnumerable<T> Select(int id);
        void Delete(int id);
    }
}
