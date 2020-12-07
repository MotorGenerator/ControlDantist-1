using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    public class ExceptionUser:Exception
    {
        public string ErrorText { get; set; }

        public ExceptionUser(string errorText)
        {
            ErrorText = errorText;
        }
    }
}
