using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Хранит строку с названиями поликлинник.
    /// </summary>
    public class HospitalCount
    {
        public int IdПоликлинники { get; set; }
        public string НаименованиеПоликлинники { get; set; }
        public string ИНН { get; set; }
        public int CountИНН { get; set; }
    }
}
