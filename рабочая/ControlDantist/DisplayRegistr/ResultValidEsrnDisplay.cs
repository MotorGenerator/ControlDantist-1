using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.DisplayRegistr
{
    public class ResultValidEsrnDisplay
    {
        public int IdContract { get; set; }

        public string НомерДоговора { get; set; }

        public string ФиоЛьготник { get; set; }

        public string Адрес { get; set; }

        public string СерияНомерУдостоверения { get; set; }
        
        /// <summary>
        /// Флаг проверки в ЭСРН.
        /// </summary>
        public bool FlagValidEsrn { get; set; }

        /// <summary>
        /// Флаг проверки услуг договора.
        /// </summary>
        public bool FlagValidServices { get; set; }

        /// <summary>
        /// Флаг сохранить договор как прошедший проверку.
        /// </summary>
        public bool FlagSaveContract { get; set; }

        /// <summary>
        /// Сумма договора.
        /// </summary>
        public string SumContract { get; set; }

    }
}
