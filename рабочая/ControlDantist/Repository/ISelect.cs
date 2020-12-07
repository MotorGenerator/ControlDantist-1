using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public interface IFiltr<T>
    {
        T Select(T item);
    }
}
