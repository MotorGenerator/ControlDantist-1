using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ControlDantist.Classes
{
    /// <summary>
    /// ��������� SQL ����������
    /// </summary>
    public class ExecuteQuery
    {
        /// <summary>
        /// ���������� ������ � ������ ����������
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

        //��������� ������ SQL
        public static void Execute(string query)
        {
            SqlConnection con = new SqlConnection(ConnectDB.ConnectionString());
            SqlCommand com = new SqlCommand(query, con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }
                
    }
}
