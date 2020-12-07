using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.BalanceContract
{
    /// <summary>
    /// Описывает строку с балансом.
    /// </summary>
    public class ItemBalanceDisplay 
    {
        /// <summary>
        /// Содержит строку с выбранными id льготными категориями.
        /// </summary>
        public string Ids { get; set; }

        /// <summary>
        /// Строка с наименованиями льготных категорий в сокращенном виде.
        /// </summary>
        public string NameLK { get; set; }

        /// <summary>
        /// Сумма денежных средств.
        /// </summary>
        public string SumMoney { get; set; }

        /// <summary>
        /// id лимита для текушей категории.
        /// </summary>
        public int IdLimitedMoney { get; set; }

    }
}
