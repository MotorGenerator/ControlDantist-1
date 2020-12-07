using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ValidateRegistrProject
{
    /// <summary>
    /// Реестр проекта договоров.
    /// </summary>
    public class RegistrProject : IRegistrItem
    {
        /// <summary>
        /// ID реестра файлов проектов договоров.
        /// </summary>
        public int IdProjectRegistr { get; set; }

        /// <summary>
        /// Номер письма.
        /// </summary>
        public string NumberLetter { get; set; }

        /// <summary>
        /// Дата письма.
        /// </summary>
        public DateTime DataLetter { get; set; }

        /// <summary>
        /// Статус ожидание проверки - false, проверка проведена - true;
        /// </summary>
        public bool StatusValide { get; set; }
    }
}
