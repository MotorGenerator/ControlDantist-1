using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    public class ExceptionYearNumber:Exception
    {
        /// <summary>
        /// Хранит сообщение об ошибке.
        /// </summary>
        public string ErrorText { get; set; }
    }
}
