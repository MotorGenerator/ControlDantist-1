using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ValidateRegistrProject
{
    /// <summary>
    /// Счет фактура.
    /// </summary>
    public interface IInvoice
    {
        /// <summary>
        /// Дата Реестра.
        /// </summary>
        DateTime ДатаРеестра { get; set; }

        /// <summary>
        /// Номер реестра.
        /// </summary>
        string НомерРеестра { get; set; }

    }
}
