using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace ControlDantist.Classes
{
    class ТаблицаБД
    {
        /// <summary>
        /// Возвращает код поликлинники
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        static public DataTable GetTable(string query, string connectionString, string nameTable)
        {
            OleDbConnection con = new OleDbConnection(connectionString);
            //OleDbCommand com = new OleDbCommand(query, con);
            OleDbDataAdapter da = new OleDbDataAdapter(query, connectionString);

            //string код = string.Empty;
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds, nameTable);
            con.Close();

            return ds.Tables[0];

        }

        /// <summary>
        /// Возвращает код поликлинники
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        static public DataTable GetTable(string query, string nameTable, OleDbConnection con, OleDbTransaction transact)
        {
            //выполним команду в единой транзакции
            OleDbCommand com = new OleDbCommand(query, con);
            com.Transaction = transact;

            //настроим адаптер данных
            OleDbDataAdapter da = new OleDbDataAdapter(com);

            //Заполним таблицу данными
            DataSet ds = new DataSet();
            da.Fill(ds, nameTable);

            return ds.Tables[0];
        }

        /// <summary>
        /// Возвращает результат выполнения SQL инструкции на выборку данных
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        static public DataTable GetTableSQL(string query, string nameTable, SqlConnection con, SqlTransaction transact)
        {
            //выполним команду в единой транзакции
            SqlCommand com = new SqlCommand(query, con);
            com.Transaction = transact;

            //настроим адаптер данных
            SqlDataAdapter da = new SqlDataAdapter(com);

            //Заполним таблицу данными
            DataSet ds = new DataSet();
            da.Fill(ds, nameTable);

            return ds.Tables[0];
        }
    }
}
