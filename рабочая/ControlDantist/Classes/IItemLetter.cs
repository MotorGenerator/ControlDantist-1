using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Данные по льготнику.
    /// </summary>
    //public interface IItemLetter
    public abstract class IItemLetter
    {
       public string Фамилия { get; set; }
       public string Имя { get; set; }
       public string Отчество { get; set; }
       public string ДатаРождения { get; set; }
       public virtual decimal СуммаАкта { get; set; }
    }
}
