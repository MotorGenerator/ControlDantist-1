using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Repozirories;

namespace ControlDantist.Reports
{
    /// <summary>
    /// Абстрактный класс описывающий метод запуска отчета.
    /// </summary>
    public abstract class PrintReport
    {
        /// <summary>
        /// Метод запуска отчета.
        /// </summary>
        public abstract void Print(List<ReportYear> listDate);
    }
}
