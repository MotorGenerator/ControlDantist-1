using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.WriteClassDB
{
    /// <summary>
    /// Поиск поликлинники по ИНН.
    /// </summary>
    public class SelectHjspital : IHospital
    {
        /// <summary>
        /// ИНН поликлинники.
        /// </summary>
        public string ИНН { get; set; }
    }
}
