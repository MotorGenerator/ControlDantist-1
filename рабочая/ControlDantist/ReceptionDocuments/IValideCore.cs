using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ReceptionDocuments
{
    public interface IValideCore<T>
    {
        List<T> Execute();
    }
}
