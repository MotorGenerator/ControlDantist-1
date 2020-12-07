using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repozirories
{
    interface IRepositoryDocVipNet<T> : IDisposable
        where T : class
    {
        IEnumerable<T> GetDocs(int idRegion, DateTime dtStart, DateTime dtEnd);
    }
}
