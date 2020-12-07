using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.WriteClassDB
{
    public class WriteServices : IQueryServices
    {

        private IServicesContract servicesContract;

        public WriteServices(IServicesContract sc)
        {
            if (sc != null)
            {
                servicesContract = sc;
            }
        }

        public string Query(string id_contract)
        {
            string query = " INSERT INTO [УслугиПоДоговору] " +
                           " ([НаименованиеУслуги] " +
                           ",[цена] " +
                           ",[Количество] " +
                           ",[id_договор] " +
                           ",[НомерПоПеречню] " +
                           ",[Сумма] " +
                           ",[ТехЛист]) " +
                           "VALUES " +
                           "('" + servicesContract.НаименованиеУслуги + "' " +
                           "," + servicesContract.цена + " " +
                           "," + servicesContract.Количество + " " +
                           "," + id_contract + " " +
                           ",'" + servicesContract.НомерПоПеречню + "' " +
                           "," + servicesContract.Сумма + " " +
                           "," + servicesContract.ТехЛист + ") ";

              return query;
         }

        /// <summary>
        /// Заполнение таблицы прием договора услуги по договору.
        /// </summary>
        /// <param name="id_contract"></param>
        /// <returns></returns>
        public string QueryReceptionContract(string id_contract)
        {
            string query = " INSERT INTO [ПриемРеестровУслугиПоДоговору] " +
                           " ([НаименованиеУслуги] " +
                           ",[цена] " +
                           ",[Количество] " +
                           ",[id_договор] " +
                           ",[НомерПоПеречню] " +
                           ",[Сумма] " +
                           ",[ТехЛист]) " +
                           "VALUES " +
                           "('" + servicesContract.НаименованиеУслуги + "' " +
                           "," + servicesContract.цена + " " +
                           "," + servicesContract.Количество + " " +
                           "," + id_contract + " " +
                           ",'" + servicesContract.НомерПоПеречню + "' " +
                           "," + servicesContract.Сумма + " " +
                           "," + servicesContract.ТехЛист + ") ";

            return query;
        }

    }
}
