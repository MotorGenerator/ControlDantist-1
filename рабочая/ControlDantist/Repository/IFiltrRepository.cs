using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public interface IFiltrRepository<T>
    {
        T Select(int id);
    }
}
