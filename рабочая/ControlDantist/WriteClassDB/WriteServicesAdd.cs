using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.WriteClassDB
{
    class WriteServicesAdd : IQueryServices
    {

        private IServicesContract servicesContract;

        public WriteServicesAdd(IServicesContract sc)
        {
            if (sc != null)
            {
                servicesContract = sc;
            }
        }

        public string Query(string id_contract)
        {
            string query = " INSERT INTO [УслугиПоДоговоруAdd] " +
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

        public string QueryReceptionContract(string id_contract)
        {
            string query = " INSERT INTO [ПриемРеестровУслугиПоДоговоруAdd] " +
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
