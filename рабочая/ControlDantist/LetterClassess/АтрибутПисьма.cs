using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ControlDantist.Classes;

namespace ControlDantist.LetterClassess
{
    public class АтрибутПисьма
    {
        string инн = string.Empty;

        public АтрибутПисьма(string инн)
        {
            this.инн = инн;
        }

        /// <summary>
        /// Получаем атрибуты поликлинники.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAttributLetter()
        {
            return ТаблицаБД.GetTableSQL(this.Query(), "ГлавВрач");
        }

        private string Query()
        {

            string query = @" select  Лист1$.F2,Лист1$.F3,Лист1$.ФИО from ПоликлинникиИнн
                            inner join[dbo].[Лист1$]
                            ON LOWER(LTRIM(RTRIM(REPLACE(ПоликлинникиИнн.F2,' ','')))) = LOWER(LTRIM(RTRIM(REPLACE([Лист1$].F1, ' ', ''))))
                            where ПоликлинникиИнн.F3 = '" + this.инн + "' ";

            return query;

        }
    }
}
