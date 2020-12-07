using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Reports
{
    /// <summary>
    /// Пункт меню.
    /// </summary>
    public class ItemReportLimitLBO
    {
        /// <summary>
        /// Категория.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// ВВС.
        /// </summary>
        public string Vt_BBS { get; set; }
        
        /// <summary>
        /// ВтСО.
        /// </summary>
        public string VtSO { get; set; }

        /// <summary>
        /// Труженники тыла.
        /// </summary>
        public string TT { get; set; }

        /// <summary>
        /// Реабилитированные.
        /// </summary>
        public string Reab { get; set; }
    }
}
