using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DantistLibrary;

namespace ControlDantist
{
    public partial class FormИсправленияУслугДоговоров : Form
    {
        /// <summary>
        /// Реестр.
        /// </summary>
        public List<Unload> ListUnload { get; set; }

        /// <summary>
        /// Хранит данные для записи.
        /// </summary>
        private DataTable tabUpdate;

        public FormИсправленияУслугДоговоров()
        {
            InitializeComponent();
        }

        private void btnNumbContr_Click(object sender, EventArgs e)
        {
            foreach (Unload un in ListUnload)
            {
                if (un.Договор.Rows[0]["НомерДоговора"].ToString().Trim().ToLower() == this.comboBox1.Text.Trim().ToLower())
                {
                    this.dataGridView1.DataSource = un.УслугиПоДоговору;

                    tabUpdate = un.УслугиПоДоговору;

                    //List<DataRow> listRows = un.УслугиПоДоговору.Rows.Cast<DataRow>().ToList();

                    this.label2.Text = un.УслугиПоДоговору.Rows.Count.ToString().Trim() + " строк ";

                    // Список для хранения суммы.
                    List<decimal> listSumm = new List<decimal>();

                    foreach (DataRow row in un.УслугиПоДоговору.Rows)
                    {
                        decimal sumItem = Convert.ToDecimal(row["Сумма"]);
                        listSumm.Add(sumItem);
                    }

                    this.label3.Text = listSumm.Sum().ToString("c").Trim();

                    DataClasses1DataContext dc = new DataClasses1DataContext();
                    if (dc.Договор.Where(w => w.НомерДоговора.ToLower().Trim() == this.comboBox1.Text.ToLower().Trim() && w.ФлагПроверки == true).Select(w => w).Count() > 0)
                    {
                        int idДоговор = dc.Договор.Where(w => w.НомерДоговора.ToLower().Trim() == this.comboBox1.Text.ToLower().Trim() && w.ФлагПроверки == true).Select(w => w).OrderByDescending(w => w.id_договор).First().id_договор;
                        this.lblIdContract.Text = idДоговор.ToString();
                    }
                }

            }
        }

        private void FormИсправленияУслугДоговоров_Load(object sender, EventArgs e)
        {
            // Получим номер поликлинники.
            string numHosp = ListUnload.First().Поликлинника.Rows[0]["КодПоликлинники"].ToString().Trim();
            this.label1.Text = numHosp.Trim();

            List<string> listNumDoc = new List<string>();

            // Сделаем двойную работу для удобства.
            foreach (Unload un in ListUnload)
            {
                listNumDoc.Add(un.Договор.Rows[0]["НомерДоговора"].ToString().Trim().ToLower());
            }

            this.comboBox1.DataSource = listNumDoc;
            this.comboBox1.DisplayMember = "НомерДоговора";


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            StringBuilder builder = new StringBuilder();

            string delete = "delete dbo.УслугиПоДоговору " +
                            "where id_договор = "+ this.lblIdContract.Text +" ";

            builder.Append(delete);

            foreach (DataRow row in this.tabUpdate.Rows)
            {
                string query = " INSERT INTO [УслугиПоДоговору] " +
                                   "([НаименованиеУслуги] " +
                                   ",[цена] " +
                                   ",[Количество] " +
                                   ",[id_договор] " +
                                   ",[НомерПоПеречню] " +
                                   ",[Сумма] " +
                                   ",[ТехЛист]) " +
                             "VALUES " +
                                   " ( '" + row["НаименованиеУслуги"].ToString().Trim() + "' " +
                                   ","+ row["цена"].ToString().Replace(",",".").Trim()  + " " +
                                   ", "+ row["Количество"].ToString().Trim()  + " " +
                                   ", "+ this.lblIdContract.Text +" " +
                                   ", '" + row["НомерПоПеречню"].ToString().Trim() + "' " +
                                   ", " + row["Сумма"].ToString().Replace(",", ".").Trim() + " " +
                                   ", "+ row["ТехЛист"].ToString().Trim()  + " )" ;

                builder.Append(query);
            }

            string asd = builder.ToString();

            DataClasses1DataContext dc = new DataClasses1DataContext();
            //dc.ExecuteQuery<УслугиПоДоговору>(builder.ToString().Trim());

            using(SqlConnection con = new SqlConnection())
            {
                //try
                //{
                    con.ConnectionString = dc.Connection.ConnectionString.Trim();
                    con.Open();

                    SqlCommand com = new SqlCommand(builder.ToString(), con);
                    com.ExecuteNonQuery();
                //}
                //catch
                //{
                //    MessageBox.Show("Ошибка при записи в БД");
                //    return;
                //}
            }

            System.Windows.Forms.MessageBox.Show("Изменения внесены");

            this.Close();
        }
    }
}
