using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ReportVipNet
{
    /// <summary>
    /// ID Адрес.
    /// </summary>
    public interface IAddress
    {
        /// <summary>
        /// Район области.
        /// </summary>
        string Region { get; set; }

        /// <summary>
        /// Населенный пункт.
        /// </summary>
        string City { get; set; }

        /// <summary>
        /// Улица.
        /// </summary>
        string Street { get; set; }

        /// <summary>
        /// Номер дома.
        /// </summary>
        string NumHous { get; set; }

        /// <summary>
        /// Номер квартиры.
        /// </summary>
        string NumApartment { get; set; }

        /// <summary>
        /// Номер корпуса.
        /// </summary>
        string NumBobyBuilder { get; set; }
    }
}
