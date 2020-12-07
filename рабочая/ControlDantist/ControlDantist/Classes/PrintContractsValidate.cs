using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Номера договоров которые уже заключены с льготником
    /// </summary>
    class PrintContractsValidate
    {
        private string фио;
        private string текущийДог;
        private string номераДоговоров;

        //Хранит ФИО и номер текущегно договора
        public string ФИО_Номер_ТекущийДоговор
        {
            get
            {
                return фио;
            }
            set
            {
                фио = value;
            }
        }

        /// <summary>
        /// Хранит номер текущего договора
        /// </summary>
        public string НомерТекущийДоговор
        {
            get
            {
                return текущийДог;
            }
            set
            {
                текущийДог = value;
            }
        }



        /// <summary>
        /// Номера договоров заключённые ранее
        /// </summary>
        public string НомераДоговоров
        {
            get
            {
                return номераДоговоров;
            }
            set
            {
                номераДоговоров = value;
            }
        } 
    }
}
