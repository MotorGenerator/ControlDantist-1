using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ReceptionDocuments
{
    /// <summary>
    /// Пункт в списке проектов договоров
    /// </summary>
    public class DisplayContract
    {
        /// <summary>
        /// Номер проекта договоров.
        /// </summary>
        public string NumberContract { get; set; }

        /// <summary>
        /// Сумма контракта.
        /// </summary>
        public decimal SummContract { get; set; }

        /// <summary>
        /// ФИО льгоотника.
        /// </summary>
        public string FioPerson { get; set; }

        /// <summary>
        /// Флаг указывает что район проживания льготника установлен.
        /// </summary>
        public bool FlagRegion { get; set; }
    }
}
