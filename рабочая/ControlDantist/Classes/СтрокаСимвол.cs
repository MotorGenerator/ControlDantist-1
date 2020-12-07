using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    public class СтрокаСимвол
    {
        public string DoShortText(string input)
        {
            string[] strs = input.Trim().Split(' ');

            StringBuilder stringBuilder = new StringBuilder();

            foreach (string str in strs)
            {

                if (str.Length > 0)
                {
                    string ch = str.Substring(0, 1).ToUpper();

                    stringBuilder.Append(ch);
                }
            }

            return stringBuilder.ToString().ToUpper();
        }
    }
}
