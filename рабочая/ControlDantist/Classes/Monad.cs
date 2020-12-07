using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    public static class Monad
    {
        public static TResult Do<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult failureValue) 
            where TInput : class
        {
            if (o == null) return failureValue;
            return evaluator(o);
        }

        public static DateTime DoDateTime(this DateTime? dtInput)
        {
            DateTime dtResult;

            if (dtInput == null)
            {
                dtResult = new DateTime(1900, 1, 1);
            }
            else
            {
                dtResult = (DateTime)dtInput;
            }

            return dtResult;
        }
                

        public static Int16 DoInt16(this Int16 input, Int16 rezult)
        {
            if (input == null)
            {
                rezult = 0;
            }
            else
            {
                rezult = input;
            }

            return rezult;
        }

        public static Int16 DoInt16To0(this Int16 input, Int16 rezult)
        {
            if (input == 0)
            {
                return rezult;
            }
            else
            {
                rezult = input;
            }

            return rezult;
        }

        public static Decimal DoDecimalTo0(this Decimal input, Decimal rezult)
        {
            if (input == 0)
            {
                return rezult;
            }
            else
            {
                rezult = input;
            }

            return rezult;
        }

        public static Decimal DoDecimalNull(this Decimal? input, Decimal rezult)
        {
            if (input == null)
            {
                return rezult;
            }
            else
            {
                rezult = (Decimal)input;
            }

            return rezult;
        }

        public static Decimal DoDecimal(this Decimal input, Decimal rezult)
        {
            if (input == null)
            {
                rezult = 0;
            }
            else
            {
                rezult = input;
            }

            return rezult;
        }

        public static string DoHortText(this string input, string output)
        {
            string[] strs = input.Split(' ');
            
            StringBuilder stringBuilder = new StringBuilder();

            foreach(string str in strs)
            {
                stringBuilder.Append(str.Trim().Take(1));
            }

            return stringBuilder.ToString().ToUpper();
        }


        public static bool? DoBool(this bool? input)
        {
            if(input == null)
            {
                input = false;
            }

            return (bool)input;
        }

    }
}
