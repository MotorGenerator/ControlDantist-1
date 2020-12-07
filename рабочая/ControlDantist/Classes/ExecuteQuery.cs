using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Выполняет SQL инструкцию
    /// </summary>
    public static class ExecuteQuery
    {
        /// <summary>
        /// Выполнение запрос в единой транзакции
        /// </summary>
        /// <param name="query"></param>
        /// <param name="con"></param>
        /// <param name="transact"></param>
        public static void Execute(string query, SqlConnection con, SqlTransaction transact)
        {
            SqlCommand com = new SqlCommand(query, con);
            com.Transaction = transact;

            com.ExecuteNonQuery();
        }

        //Выполняет запрос SQL
        public static void Execute(string query)
        {
            SqlConnection con = new SqlConnection(ConnectDB.ConnectionString());
            SqlCommand com = new SqlCommand(query, con);

            // Дождемся выполнение инструкции.
            com.CommandTimeout = 0;
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }

        //Выполняет запрос SQL
        public static void Execute(string query, string stringConnection)
        {
            SqlConnection con = new SqlConnection(stringConnection);
            SqlCommand com = new SqlCommand(query, con);

            // Дождемся выполнение инструкции.
            com.CommandTimeout = 0;
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }
    }
}
