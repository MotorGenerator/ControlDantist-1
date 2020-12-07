using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ReceptionDocuments
{
    /// <summary>
    /// Вычисляет остаток лимита.
    /// </summary>
    public interface IPrintBalance
    {
        decimal PrintBalance();
    }
}
