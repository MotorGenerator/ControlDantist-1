using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    public class DisplayReestr
    {
        private string номер;
        List<ErrorsReestrUnload> errorList;
        private bool flagError;
        private ReestrControl льготник;
        private decimal sum;
        private decimal errorSum;
        private bool errorPerson;

        //дл€ хранени€ доп соглашений
        private bool flagAddContract;

        //дл€ хранени€ суммы у нас на серврер дл€ текущего договора
        private decimal sumContractServ;

        //’ранит сумму акта выполненных работ к текущему договору
        private decimal sumAct;

        /// <summary>
        /// ”казывает что возникла ошибка, договор уже оплачивалс€.
        /// </summary>
        public bool FlagErrorќплаченныйƒоговор { get; set; }

        /// <summary>
        /// —одержит номер оплаченного акта.
        /// </summary>
        public string Ќомерќплаченногојкта { get; set; }

        /// <summary>
        /// ’ранит состо€ние есть ли доп соглашение
        /// </summary>
        public bool FlagAddContract
        {
            get
            {
                return flagAddContract;
            }
            set
            {
                flagAddContract = value;
            }
        }

        /// <summary>
        /// ’ранит сумму акта выполненных работ к текущему договору который пришол из файла выгрузки реестра выполненных работ
        /// </summary>
        public decimal —уммајкта¬ыполненных–абот
        {
            get
            {
                return sumAct;
            }
            set
            {
                sumAct = value;
            }
        }

        /// <summary>
        /// сумма текущего договора котора€ хранитьс€ у нас на сервере
        /// </summary>
        public decimal —уммаƒоговор—ервер
        {
            get
            {
                return sumContractServ;
            }
            set
            {
                sumContractServ = value;
            }
        }

        /// <summary>
        /// ’ранит сумму расхождени€ услуг оказанных льготнику
        /// </summary>
        public decimal SumError
        {
            get
            {
                return errorSum;
            }
            set
            {
                errorSum = value;
            }
        }


        /// <summary>
        /// ’ранит сумму услуг оказанных льготнику
        /// </summary>
        public decimal Sum
        {
            get
            {
                return sum;
            }
            set
            {
                sum = value;
            }
        }



        /// <summary>
        /// Ћьготник прошедший проверку
        /// </summary>
        public ReestrControl Ћьготник
        {
            get
            {
                return льготник;
            }
            set
            {
                льготник = value;
            }
        }

        /// <summary>
        /// флаг если равен false то указывает что акт приЄма содержит ошибки в видах работ 
        /// </summary>
        public bool FlagError
        {
            get
            {
                return flagError;
            }
            set
            {
                flagError = value;
            }
        }

        /// <summary>
        /// Ќомер акта выполненных работ
        /// </summary>
        public string Ќомерјкта
        {
            get
            {
                return номер;
            }
            set
            {
                номер = value;
            }
        }

        /// <summary>
        /// —писок ошибок
        /// </summary>
        public List<ErrorsReestrUnload> —писокќшибок
        {
            get
            {
                return errorList;
            }
            set
            {
                errorList = value;
            }
        }

        /// <summary>
        /// если true ‘лаг указывает что в персональных данных возникли ошибки
        /// </summary>
        public bool ErrorPerson
        {
            get
            {
                return errorPerson;
            }
            set
            {
                errorPerson = value;
            }
        }

        /// <summary>
        /// ќшибка если льготные категории не совпали.
        /// </summary>
        public bool ErrorPrefCategory { get; set; }

        public PreferentCategory Ћьготна€ атегори€ { get; set; }
    }
}
