using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public interface IReppositorySelectAll<T> 
        where T : class
    {
        List<T> SelectAll();
    }
}
