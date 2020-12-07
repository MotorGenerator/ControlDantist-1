using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Ревлизация паттерна команда
    /// </summary>
    interface ICommand
    {
        void Execute();
    }
}
