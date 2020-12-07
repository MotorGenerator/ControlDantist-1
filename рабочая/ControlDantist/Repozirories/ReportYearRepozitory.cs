using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ControlDantist.Classes;


namespace ControlDantist.Repozirories
{
    /// <summary>
    /// Репозиторий для годового отчета по бесплатному зубопротезированию.
    /// </summary>
    public class ReportYearRepozitory
    {
        /// <summary>
        /// Возвращает данные для отчета.
        /// </summary>
        /// <returns>Таблица с данными DataTable</returns>
        public DataTable GetData()
        {
            string query = "SELECT TOP 1000 [Район] " +
                          ",[Поликлинника] " +
                          ",[Пропускная способность за 2019 год] " +
                          ",[Очередность за 2019 год] " +
                          ",[количество заключенных договоров] " +
                          ",[сумма заключенных договоров] " +
                          ",[SerialNumber] " +
                          ",[кол-во договоров находящихся в деле] " +
                          ",[сумма договоров находящихся в деле] " +
                          ",[кол-во договоров поступивших на оплату] " +
                          ",[сумма договоро поступивщих на оплату] " +
                          "FROM [Dentists].[dbo].[ViewИнформацияПоЗубопротезированию_2019] " +
                          "order by [SerialNumber]asc ";

            DataTable dataReport = ТаблицаБД.GetTableSQL(query,"Report");

            return dataReport;
        }
    }
}
