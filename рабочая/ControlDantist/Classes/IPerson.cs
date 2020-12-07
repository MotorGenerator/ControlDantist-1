using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Class
{
    interface IЛьготник
    {
        string Фамилия { get; set; }
        string Имя { get; set; }
        string Отчество { get; set; }
        DateTime ДатаРождения { get; set; }
    }
}
