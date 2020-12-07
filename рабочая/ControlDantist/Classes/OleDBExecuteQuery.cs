using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace ControlDantist.Classes
{
    public class OleDBExecuteQuery
    {
        string sConn = string.Empty;
        public OleDBExecuteQuery(string sConnect)
        {
            sConn = sConnect;
        }

        public void QueryExcecute(string query)
        {
            using (OleDbConnection con = new OleDbConnection(sConn))
            {
                con.Open();
                OleDbCommand comHosp = new OleDbCommand(query, con);
                //OleDbDataReader readHosp = comHosp.ExecuteReader();
                comHosp.ExecuteNonQuery();
            }
        }
    }
}
