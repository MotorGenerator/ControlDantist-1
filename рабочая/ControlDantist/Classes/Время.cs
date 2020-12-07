using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    public class Время
    {
        /// <summary>
        /// Преобразует дату в формат SQL
        /// </summary>
        /// <param name="дата">дата</param>
        /// <returns></returns>
        public static string Дата(string дата)
        {
            //string BeginDateSQL = System.Text.RegularExpressions.Regex.Replace(дата, "\\b(?<day>\\d{1,2}).(?<month>\\d{1,2}).(?<year>\\d{2,4})\\b", "${month}-${day}-${year}");
            string BeginDateSQL = System.Text.RegularExpressions.Regex.Replace(дата, "\\b(?<day>\\d{1,2}).(?<month>\\d{1,2}).(?<year>\\d{2,4})\\b", "${year}${month}${day}");

            return BeginDateSQL;
        }

    }
}
