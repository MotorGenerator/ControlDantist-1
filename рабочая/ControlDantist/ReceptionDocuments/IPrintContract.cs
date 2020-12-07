using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ReceptionDocuments
{
    /// <summary>
    /// Печать проектов договров.
    /// </summary>
    public interface IPrintContract
    {
        bool PrintContractDraft();
    }
}
