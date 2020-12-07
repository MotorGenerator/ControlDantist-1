using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Строка списка договоров
    /// </summary>
    class ContractListItem
    {
        private string num;
        private string fio;
        private string numContrCurrent;
        private string numContracts;

        /// <summary>
        /// Номер по порядку
        /// </summary>
        public string Num
        {
            get
            {
                return num;
            }
            set
            {
                num = value;
            }
        }

        /// <summary>
        /// ФИО льготника с текущим договором
        /// </summary>
        public string FIO
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

        /// <summary>
        /// Номер текущего договора
        /// </summary>
        public string NumCurrentContract
        {
            get
            {
                return numContrCurrent;
            }
            set
            {
                numContrCurrent = value;
            }
        }

        /// <summary>
        /// Номера договоров
        /// </summary>
        public string NumContracts
        {
            get
            {
                return numContracts;
            }
            set
            {
                numContracts = value;
            }
        }


    }
}
