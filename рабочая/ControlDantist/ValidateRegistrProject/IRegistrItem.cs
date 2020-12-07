using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ValidateRegistrProject
{
    public interface IRegistrItem
    {
        /// <summary>
        /// ID реестра файлов проектов договоров.
        /// </summary>
        int IdProjectRegistr { get; set; }

        /// <summary>
        /// Номер письма.
        /// </summary>
        string NumberLetter { get; set; }

        /// <summary>
        /// Дата письма.
        /// </summary>
        DateTime DataLetter { get; set; }

        /// <summary>
        /// Статус ожидание проверки - false, проверка проведена - true;
        /// </summary>
        bool StatusValide { get; set; }

    }
}
