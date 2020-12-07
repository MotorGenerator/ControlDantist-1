using System;
using System.Collections.Generic;
using System.Text;
using ControlDantist.ValidPersonContract;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Номера договоров которые уже заключены с льготником
    /// </summary>
    public class PrintContractsValidate
    {
        private string фио;
        private string текущийДог;
        private string номераДоговоров;

        /// <summary>
        /// Список дубликатов договоров
        /// </summary>
        public string СписокДоговоров { get; set; }

        public List<ValidItemsContract> listContracts { get; set; }// = new List<ValidItemsContract>();

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
