using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using ControlDantist.Classes;
using System.Windows.Forms;

namespace ControlDantist
{
    public partial class FormConfig : Form
    {
        private DataClasses1DataContext dc;
        public FormConfig()
        {
            InitializeComponent();
            dc = new DataClasses1DataContext();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = dc.Connection.ConnectionString.Trim();

                con.Open();

                string query = string.Empty;

                if(this.checkBox1.Checked == true)
                {

                  query = "UPDATE [TableКонфигурацияУслугиДоговора] " +
                                "SET [УслугиДоговора] = "+ 1 +"  " +
                                "WHERE idConfig = 1";
                }
                else
                {
                    query = "UPDATE [TableКонфигурацияУслугиДоговора] " +
                                "SET [УслугиДоговора] = "+ 0 +"  " +
                                "WHERE idConfig = 1";
                }

                SqlCommand com = new SqlCommand(query, con);
                com.ExecuteNonQuery();
            }

            this.Close();
        }

        private void FormConfig_Load(object sender, EventArgs e)
        {

                string query = string.Empty;

                query = "select [УслугиДоговора] from [TableКонфигурацияУслугиДоговора] " +
                                  "WHERE idConfig = 1";

                int intFlag = Convert.ToInt32(ТаблицаБД.GetTableSQL(query, "TabConfig").Rows[0]["УслугиДоговора"]);

                if (intFlag == 1)
                {
                    this.checkBox1.Checked = true;                  
                }
                else
                {
                    this.checkBox1.Checked = false;                  
                }

            }

        
    }
}
