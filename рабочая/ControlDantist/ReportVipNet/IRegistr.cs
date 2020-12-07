using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ReportVipNet
{
    public interface IRegistr
    {
        bool GetPersons();

        /// <summary>
        /// ID выбранного района.
        /// </summary>
        int IdRegion { get; set; }

        /// <summary>
        /// Название района области.
        /// </summary>
        string NameRegion { get; set; }

        /// <summary>
        /// Дата начала отчетного периода.
        /// </summary>
        DateTime DateStartPeriod { get; set; }

        /// <summary>
        /// Дата окончания отчетного периода.
        /// </summary>
        DateTime DateEndPeriod { get; set; }
    }
}
