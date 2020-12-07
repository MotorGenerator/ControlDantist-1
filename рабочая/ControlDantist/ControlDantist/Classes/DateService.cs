using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// ’ранит данные по услугам
    /// </summary>
    class DateService
    {
        private string наименование”слуги;
        //private decimal цена;
        //private decimal сумма;
        private string цена;
        private string сумма;

        /// <summary>
        /// ’ранит наименование
        /// </summary>
        public string Ќаименование
        {
            get
            {
                return наименование”слуги;
            }
            set
            {
                наименование”слуги = value;
            }
        }

        /// <summary>
        /// ’ранит стоимость услуги
        /// </summary>
        //public decimal ÷ена
        public string ÷ена
        {
            get
            {
                return цена;
            }
            set
            {
                цена = value;
            }
        }

        /// <summary>
        /// ’ранит сумму услуги
        /// </summary>
        //public decimal —умма
        public string —умма
        {
            get
            {
                return сумма;
            }
            set
            {
                сумма = value;
            }
        }
    }
}
