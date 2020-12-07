using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    class ВидУслугCompare
    {
        private string _НаименованиеУслуги;
        private decimal _цена; 
        private int _Количество; 
        private decimal _Сумма; 
        private string _Flag;


        public string НаименованиеУслуги 
        { 
            get
            {
                return _НаименованиеУслуги;
            }
            set
            {
                _НаименованиеУслуги = value;
            }
         }

        public decimal цена
        {
            get
            {
                return _цена;
            }
            set
            {
                _цена = value;
            }
        }

        public int Количество
        {
            get
            {
                return _Количество;
            }
            set
            {
                _Количество = value;
            }
        }

        public decimal Сумма
        {
            get
            {
                return _Сумма;
            }
            set
            {
                _Сумма = value;
            }
        }

        public string Flag
        {
            get
            {
                return _Flag;
            }
            set
            {
                _Flag = value;
            }
        }
    }
}
