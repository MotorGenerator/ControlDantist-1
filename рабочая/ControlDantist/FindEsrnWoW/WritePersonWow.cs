using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Classes;

namespace ControlDantist.FindEsrnWoW
{
    /// <summary>
    /// Пишет льготнирка ВОВ в таблицу PersonWOW.
    /// </summary>
    public class WritePersonWow
    {
        string firstName = string.Empty;
        string name = string.Empty;
        string surName = string.Empty;
        string bDate = string.Empty;
        string address = string.Empty;
        string category = string.Empty;
        string regionName = string.Empty;

        public WritePersonWow(string firstName, string name, string surName, string bDate, string address, string category, string regionName)
        {
            this.firstName = firstName;
            this.name = name;
            this.surName = surName;
            this.bDate = bDate;
            this.address = address;
            this.category = category;
            this.regionName = regionName;
        }

        /// <summary>
        /// Возвращает SQl звапрос на Insert.
        /// </summary>
        /// <returns></returns>
        public string InsertPerson()
        {
            string query = @" INSERT INTO [PersonWow]
                           ([FistName]
                           ,[Name]
                           ,[Surname]
                           ,[BerstDay]
                           ,[Address],
                            [Категория],
                            [Region])
                            VALUES
                           ('" + firstName + "' " +
                           ",'" + name + "' " +
                           ",'" + surName + "' " +
                           ", '" + bDate + "' " +
                           ",'" + address + "' " +
                           ", '" + category + "' " +
                           ", '" + regionName + "' )";

            return query;
        }

    }
}
