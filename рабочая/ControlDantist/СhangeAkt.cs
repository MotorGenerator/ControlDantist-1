using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using ControlDantist.Classes;
using DantistLibrary;
using System.Data.Linq;
using System.Configuration;
using System.Data.Common;

namespace ControlDantist
{
    public partial class СhangeAkt : Form
    {
         //Хранит временную таблицу с номерами актов выполненных работ.
        private DataTable tabTempAkt;
        private DbTransaction transaction;
        

        public СhangeAkt()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получим путь к базе данных.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Файл базы данных";
            openFileDialog1.Filter = "|*.mdb";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtBdPach.Text = openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Закроем форму.
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Выполним запись в БД.
            if (this.txtBdPach.Text.Trim() != "")
            {
                //================Считаем данные из указанной БД

                //получим строку подключения к указанной БД
                string connectStringAcess = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.txtBdPach.Text + " ";
                using (OleDbConnection con = new OleDbConnection(connectStringAcess))
                {
                    con.Open();
                    OleDbTransaction transact = con.BeginTransaction();

                    //Получим классификатор услуг
                    //string queryКлУслуг = "select * from АктВыполнненныхРабот";

                    string queryКлУслуг = "select * from АктВыполнненныхРабот where НомерАкта = '5/890/706'";
                    tabTempAkt = ТаблицаБД.GetTable(queryКлУслуг, "АктВыполнненныхРабот", con, transact);

                    //Test.
                    DataTable iTest = tabTempAkt;

                    ////Получим виды услуг
                    //string queryВидУслуг = "select * from ВидУслуги";
                    //tabВидУслуги = ТаблицаБД.GetTable(queryВидУслуг, "КлассификаторУслуги", con, transact);

                }

    

                string scon = ConfigurationSettings.AppSettings["connect"].ToString();

                DataClasses1DataContext dc = new DataClasses1DataContext(scon);

                // Выполним всё в одной транзакции.
                //try
                //{
                //    // Откроем соединение.
                //    dc.Connection.Open();

                //    // ОБявим что открываем транзакцию.
                //    transaction = dc.Connection.BeginTransaction();

                //    // Присвоим транзакцию текущему контексту данных.
                //    dc.Transaction = transaction;

                    // Получим таблицу актовВыполненныхРабот.
                    Table<АктВыполненныхРабот> tabAct = dc.GetTable<АктВыполненныхРабот>();

                    // Получим таблицу договор.
                    Table<Договор> tabContract = dc.GetTable<Договор>();

                    foreach (DataRow row in tabTempAkt.Rows)
                    {
                        string номерАкта = row["НомерАкта"].ToString();
                        DateTime date = Convert.ToDateTime(row["ДатаПодписания"]);

                        // Получим номер акта.
                        string numAct = tabAct.Where(t => t.НомерАкта == номерАкта).Select(t => t).First().НомерАкта.Trim();

                        // По номеру акта найдём id_договор.
                        int id_договор = tabAct.Where(t => t.НомерАкта == номерАкта).Select(t => t).First().id_договор;

                        // Перепишем дату акта в таблице АктВыполненныхРабот.
                        АктВыполненныхРабот акт = tabAct.Where(t => t.НомерАкта == номерАкта).Select(t => t).First<АктВыполненныхРабот>();
                        акт.ДатаПодписания = date;

                        // Обновим объект в базе.
                        //dc.SubmitChanges();


                        // Перепишем дату в таблице договор.
                        Договор договор = tabContract.Where(tc => tc.id_договор == id_договор && tc.ФлагНаличияАкта == true && tc.ФлагПроверки == true).Select(tc => tc).First<Договор>();
                        договор.ДатаАктаВыполненныхРабот = date;

                        // Обновим объект в базе.
                        dc.SubmitChanges();
                    }

                    
                //}
                //catch
                //{
                //    //if (transaction != null)
                //    if (transaction != null)
                //    {
                //        transaction.Rollback();
                //    }
                //}
                //finally
                //{
                //    // на всякий случай неплохо бы закрыть соединение принудительно
                //    if (dc.Connection.State != System.Data.ConnectionState.Closed)
                //    {
                //        dc.Connection.Close();
                //    }
                //}


                // Закроем форму.
                this.Close();
                
            }
        }
    }
}
