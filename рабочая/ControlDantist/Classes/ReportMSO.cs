using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Отчёт министерства соц развития.
    /// </summary>
    class ReportMSO
    {
        /// <summary>
        /// Порядковый номер в отчёте.
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// Наименование поликлинники.
        /// </summary>
        public string NameHospital { get; set; }

        /// <summary>
        /// Количество заключонных договоров.
        /// </summary>
        public string CountContractHospital { get; set; }

        /// <summary>
        /// Сумма на которую заключили договора.
        /// </summary>
        public string SumContractHospital { get; set; }

        /// <summary>
        /// Сумма выплаченная по договорам (заполняют в ручную).
        /// </summary>
        public string SumPaidContract { get; set; }

        /// <summary>
        /// Количество вставших на учёт (заполняют в ручную).
        /// </summary>
        public string CountRoseRecords { get; set; }


        /// <summary>
        /// ИНН поликлинники.
        /// </summary>
        public string ИНН { get; set; }

        /// <summary>
        /// Флаг наличия записи.
        /// </summary>
        public bool Flag { get; set; }
    }
}
