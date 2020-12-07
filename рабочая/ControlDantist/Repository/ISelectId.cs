using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repository
{
    public interface ISelectId <T>
    {
        IEnumerable<T> SelectId(int idHospital);
    }
}
