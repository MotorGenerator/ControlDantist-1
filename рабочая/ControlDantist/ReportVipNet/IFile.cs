using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ReportVipNet
{
    public interface IFile
    {
        void Save(string fileName);

        // Период времени.
        string Period { get; set; }
        bool FlagSave();
    }
}
