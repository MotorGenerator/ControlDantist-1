using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Хранит результат проверки проектра договора на оказание услуг льготникам
    /// </summary>
    public class ValidateContract
    {
        private List<ЭСРНvalidate> эсрнValidate;
        private bool flagЭСРН;
        private List<ErrorsReestrUnload> listError;
        private bool flagError;

        /// <summary>
        /// Указывает что льготник в ЭСРН существует
        /// </summary>
        public bool FlagPersonЭСРН
        {
            get
            {
                return flagЭСРН;
            }
            set
            {
                flagЭСРН = value;
            }
        }

        /// <summary>
        /// Хранит список документов проходящих для данного льготника в ЭСРН
        /// </summary>
        public List<ЭСРНvalidate> СписокДокументов
        {
            get
            {
                return эсрнValidate;
            }
            set
            {
                эсрнValidate = value;
            }
        }

        /// <summary>
        /// Хранит список расхождений стоимость услуги и сумма
        /// </summary>
        public List<ErrorsReestrUnload> СписокРасхождений
        {
            get
            {
                return listError;
            }
            set
            {
                listError = value;
            }
        }

        /// <summary>
        /// флаг указывает на отсутсвие ошибки в стоимости услуг (если true - ошибки нет)
        /// </summary>
        public bool flagErrorSumm
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


    
    }
}
