using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Строка таблицы письма в стоматологию
    /// </summary>
    class ReestrNoPrintDog
    {
        private string number;
        private string numDog;
        private string fio;

        /// <summary>
        /// Порядковыый номер
        /// </summary>
        public string Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }


        /// <summary>
        /// Номер договора
        /// </summary>
        public string NumDog
        {
            get
            {
                return numDog;
            }
            set
            {
                numDog = value;
            }
        }

        /// <summary>
        /// ФИО льготника
        /// </summary>
        public string Fio
        {
            get
            {
                return fio;
            }
            set
            {
                fio = value;
            }
        }
    }
}
