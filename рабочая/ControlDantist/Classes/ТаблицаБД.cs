using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using ControlDantist.ErrorHandlid;

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
            // Выполним команду в единой транзакции.
            SqlCommand com = new SqlCommand(query, con);
            com.CommandTimeout = 0;
            com.Transaction = transact;

            // Настроим адаптер данных.
            SqlDataAdapter da = new SqlDataAdapter(com);

            // Заполним таблицу данными.
            DataSet ds = new DataSet();
            da.Fill(ds, nameTable);

            return ds.Tables[nameTable];
        }

        /// <summary>
        /// Возвращает таблицу договоров
        /// </summary>
        /// <param name="query"></param>
        /// <param name="nameTable"></param>
        /// <returns></returns>
        static public DataTable GetTableSQL(string query, string nameTable)
        {

            SqlConnection con = new SqlConnection(ConnectDB.ConnectionString());
            con.Open();

            SqlCommand com = new SqlCommand(query, con);
            com.CommandTimeout = 0;

            //SqlDataAdapter da = new SqlDataAdapter(query, con);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds, nameTable);
            con.Close();

            return ds.Tables[0];

        }

        /// <summary>
        /// Выполнение SQL запроса к БД.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="nameTable"></param>
        /// <param name="sConnetion"></param>
        /// <returns></returns>
        static public DataTable GetTableSQL(string query, string nameTable, string sConnetion)
        {
            // Путь куда пишется ошибка.
            string writePath = @"F:\Поиск_ветеранов_ВОВ\ErrorConnectionString.txt";

            // Путь куда пишем какой район был записан.
            string writePatchRegion = @"F:\Поиск_ветеранов_ВОВ\RegionsName.txt";

            DataSet ds = new DataSet();

            SqlConnection con = new SqlConnection(sConnetion);
            try
            {
                con.Open();

                SqlCommand com = new SqlCommand(query, con);
                com.CommandTimeout = 0;

                //SqlDataAdapter da = new SqlDataAdapter(query, con);
                SqlDataAdapter da = new SqlDataAdapter(com);
                
                da.Fill(ds, nameTable);
                con.Close();
                
            }
            catch(Exception ex)
            {
                // Запишем лог в файл.
                IWriteError writeErrorFileText = new WriteErrorFileText(writePath, sConnetion);
                writeErrorFileText.Write();

                // Пустая таблица.
                DataTable dataTable = new DataTable();

                // Запишем пустую таблицу в DataSet.
                ds.Tables.Add(dataTable);
            }
            finally
            {
                // Запишем лог в файл.
                IWriteError writeErrorFileText = new WriteErrorFileText(writePatchRegion, sConnetion);
                writeErrorFileText.Write();

            }

            return ds.Tables[0]; 

        }


        /// <summary>
        /// Возвращает таблицу
        /// </summary>
        /// <param name="query"></param>
        /// <param name="nameTable"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        public static DataTable GetTableSQL(string query, string nameTable, SqlConnection con)
        {
            //выполним команду в единой транзакции
            SqlCommand com = new SqlCommand(query, con);

            com.CommandTimeout = 0;

            //настроим адаптер данных
            SqlDataAdapter da = new SqlDataAdapter(com);


            //Заполним таблицу данными
            DataSet ds = new DataSet();
            da.Fill(ds, nameTable);

            return ds.Tables[nameTable];
        }
                
    }
}
