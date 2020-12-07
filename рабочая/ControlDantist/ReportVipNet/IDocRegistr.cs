using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ReportVipNet
{
    /// <summary>
    /// Номер и серия удостоверения, сумма акта, дата подписания.
    /// </summary>
    public interface IDocRegistr
    {
        /// <summary>
        /// Серия документа.
        /// </summary>
        string SeriesDoc { get; set; }

        /// <summary>
        /// Номер документа.
        /// </summary>
        string NumDoc { get; set; }

        /// <summary>
        /// Сумма акта.
        /// </summary>
        decimal SumAct { get; set; }

        /// <summary>
        /// Дата акта.
        /// </summary>
        DateTime DateWriteAct { get; set; }
    }
}
