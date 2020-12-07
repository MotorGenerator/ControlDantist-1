using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    class ContractDisplay
    {
        private string numbContract;
        private bool validEsrn;
        private bool validServ;
        private bool controlSaveBD;
        private string фио;
        private string sumServ;

        //// Id района области где найден льготник.
        //public string IdRegion { get; set; }


        //// Снилс льготника.
        //public string Sinls { get; set; }
        

        /// <summary>
        /// Отображает номер по порядку.
        /// </summary>
        //public string НомерПорядковый { get; set; }

        /// <summary>
        /// Номер догвора
        /// </summary>
        public string НомерДоговора
        {
            get
            {
                return numbContract;
            }
            set
            {
                numbContract = value;
            }
        }

        public string ФИО
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
        /// Указывает прошёл проверку льготник в ЭСРН или нет 
        /// </summary>
        public bool ПроверкаЭСРН
        {
            get
            {
                return validEsrn;
            }
            set
            {
                validEsrn = value;
            }
        }

        /// <summary>
        /// Указывает прошёл ли проверку на стоимость услуг
        /// </summary>
        public bool ПроверкаУслуг
        {
            get
            {
                return validServ;
            }
            set
            {
                validServ = value;
            }
        }

        /// <summary>
        /// Сохранить договор в базу данных
        /// </summary>
        public bool СохранитьДоговор
        {
            get
            {
                return controlSaveBD;
            }
            set
            {
                controlSaveBD = value;
            }
        }


        /// <summary>
        /// Сумма услуг по договору
        /// </summary>
        public string SumService
        {
            get
            {
                return sumServ;
            }
            set
            {
                sumServ = value;
            }
        }



    }
}
