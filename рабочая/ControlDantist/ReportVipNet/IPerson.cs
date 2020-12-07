using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ReportVipNet
{
    public interface IPerson
    {
        string FirstName { get; set; }
        string Name { get; set; }
        string SecondName { get; set; }
        DateTime DateBirth { get; set; }
    }
}
