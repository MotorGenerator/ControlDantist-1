using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Строка письма содержащая льготников с задвоенными номерами
    /// </summary>
    class ЭлементПисьма
    {
        public string Номер { get; set; }
        public string ФИО {get; set;}
        public string ДатаРождения {get; set;}
        public string НомерДоговора { get; set; }
    }
}
