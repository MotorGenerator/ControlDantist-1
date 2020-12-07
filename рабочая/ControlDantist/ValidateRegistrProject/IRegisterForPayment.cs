using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ValidateRegistrProject
{
    public interface IRegisterForPayment : IRegistrItem //, IInvoice
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
