using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Repository;

namespace ControlDantist.BalanceContract
{
    public class YearLimit
    {
        private int Year = 0;
        public YearLimit(int Year)
        {
            this.Year = Year;
        }

        public bool ValidateEdit()
        {
            // Окно работает на добавление лимитов в льготные категории.
            bool flagEDitLimit = false;

            string query = "select Tab1.id_льготнойКатегории, ЛьготнаяКатегория from ЛьготнаяКатегория " +
                            "left outer join( " +
                            "select id_льготнойКатегории from LimittMoney " +
                            "inner join LimitPreferenceCategory " +
                            "ON LimittMoney.[idLimitMoney] = LimitPreferenceCategory.idLimitMoney " +
                            "inner join [Year] " +
                            "ON Year.intYear = LimittMoney.[idYear] " +
                            "where [Year] = "+ this.Year +") as Tab1 " +
                            "ON ЛьготнаяКатегория.id_льготнойКатегории = Tab1.id_льготнойКатегории " +
                            "where Tab1.id_льготнойКатегории is not null";

            UnitDate unitDate = new UnitDate();
            var result = unitDate.DateContext.ExecuteQuery<ЛьготнаяКатегория>(query);

            var countRowValid = result.Count();

            // Для теста.
            //countRowValid = 5;

            // Отимим 1 от количества записейв таблице ЛьготнаяКатегория, так как первая запись содержит 0 и не несет смысловой нагрузки.
            var resultLK = unitDate.ЛьготнаяКатегорияRepository.SelectAll().Count() - 1;

            // Отнимим 
            if(countRowValid == resultLK)
            {
                // Редактирование лимитов.
                flagEDitLimit = true;
            }

            return flagEDitLimit;
                
        }
    }
}
