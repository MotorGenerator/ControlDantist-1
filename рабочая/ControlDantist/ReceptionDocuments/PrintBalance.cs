using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ReceptionDocuments
{
    public class PrintBalance : IPrintBalance
    {
        private decimal sumBalance;
        private decimal summRegistr;

        public PrintBalance(decimal sumBalance, decimal summRegistr)
        {
            this.sumBalance = sumBalance;
            this.summRegistr = summRegistr;
        }

        /// <summary>
        /// Сумма баланса.
        /// </summary>
        /// <returns></returns>
        decimal IPrintBalance.PrintBalance()
        {
            return Math.Round(sumBalance - summRegistr, 4);
        }
    }
}
