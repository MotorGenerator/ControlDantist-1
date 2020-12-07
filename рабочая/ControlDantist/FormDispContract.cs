using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ControlDantist.Classes;

namespace ControlDantist
{
    public partial class FormDispContract : Form
    {
        private DataTable tabContract;
        private DataTable tabPerson;

        /// <summary>
        /// Хранит id договора
        /// </summary>
        public int IdДоговор { get; set; }
        
            

        /// <summary>
        /// Принимаеи и хранит таблицу с данными по контракту
        /// </summary>
        public DataTable ДанныеПоКонтракту
        {
            get
            {
                return tabContract;
            }
            set
            {
                tabContract = value;
            }
        }

        /// <summary>
        /// Принимает и хранит данные по льготнику
        /// </summary>
        public DataTable ДанныеПоЛьготнику
        {
            get
            {
                return tabPerson;
            }
            set
            {
                tabPerson = value;
            }
        }
                

        public FormDispContract()
        {
            InitializeComponent();
        }

        private void FormDispContract_Load(object sender, EventArgs e)
        {

            DataTable tab = new DataTable();
            DataColumn col0 = new DataColumn("НаименованиеУслуги", typeof(string));
            tab.Columns.Add(col0);
            DataColumn col1 = new DataColumn("цена", typeof(string));
            tab.Columns.Add(col1);
            DataColumn col2 = new DataColumn("Количество", typeof(string));
            tab.Columns.Add(col2);
            DataColumn col3 = new DataColumn("Сумма", typeof(string));
            tab.Columns.Add(col3);
            //DataColumn col4 = new DataColumn("Дата", typeof(string));
            //tab.Columns.Add(col4);

            decimal sum = 0.0m;

            foreach(DataRow row in ДанныеПоКонтракту.Rows)
            {
                DataRow row1 = tab.NewRow();
                row1["НаименованиеУслуги"] = row["НаименованиеУслуги"].ToString().Trim();
                row1["цена"] = Convert.ToDecimal(row["цена"]).ToString("c").Trim();
                row1["Количество"] = row["Количество"].ToString().Trim();

                sum = Math.Round(Math.Round(sum, 2) + Math.Round(Convert.ToDecimal(row["Сумма"]), 2), 2);

                row1["Сумма"] = Convert.ToDecimal(row["Сумма"]).ToString("c").Trim();
                tab.Rows.Add(row1);
            }

            this.lblSum.Text = sum.ToString("c").Trim();

            //Отобразим и отформатируем данные в таблице
            this.dataGridView1.DataSource = tab;


            this.dataGridView1.Columns["НаименованиеУслуги"].Width = 200;
            this.dataGridView1.Columns["НаименованиеУслуги"].DisplayIndex = 0;

            this.dataGridView1.Columns["цена"].Width = 100;
            this.dataGridView1.Columns["цена"].DisplayIndex = 1;

            this.dataGridView1.Columns["Количество"].Width = 100;
            this.dataGridView1.Columns["Количество"].DisplayIndex = 2;

            this.dataGridView1.Columns["Сумма"].Width = 200;
            this.dataGridView1.Columns["Сумма"].DisplayIndex = 3;

           // this.dataGridView1.DataSource = ДанныеПоКонтракту;

            DataRow rowL = this.ДанныеПоЛьготнику.Rows[0];

            this.lblДоговор.Text = rowL["НомерДоговора"].ToString().Trim();
            string дата = Convert.ToDateTime(rowL["ДатаДоговора"]).ToShortDateString().Trim();

            if (дата == "01.01.1900")
            {
                this.lblДатаДоговора.Text = "Договор не подписан";
            }
            else
            {
                this.lblДатаДоговора.Text = дата;
            }

            //Получим и выведим на форму ФИО льготника
            string фамилия = rowL["Фамилия"].ToString().Trim();
            string имя = rowL["Имя"].ToString().Trim();
            string отчество = rowL["Отчество"].ToString().Trim();
            string датаРождения = Convert.ToDateTime(rowL["ДатаРождения"]).ToShortDateString().Trim();

            string FIO = фамилия + " " + имя + " " + отчество + " " + датаРождения + " г. рождения ";
            this.lblФИО.Text = FIO.Trim();

            //Получим и выведим на форму адрес льготника
            string улица = rowL["улица"].ToString().Trim();
            
            string номерДома = "д. " + rowL["НомерДома"].ToString().Trim();

            string корп = string.Empty;
            string корпус = rowL["корпус"].ToString().Trim();

            if(корпус.Length != 0)
            {
                корп = "корп. " + корпус;
            }

            string кв = "кв. " + rowL["НомерКвартиры"].ToString().Trim();
            string адрес = улица + " " + номерДома + " " + корп + " " + кв;

            this.lblAdress.Text = "Адрес " + адрес;

            //Получим и выведим паспорт
            string номерПаспорта = "Паспорт " + rowL["СерияПаспорта"].ToString().Trim() + "  № " + rowL["НомерПаспорта"].ToString().Trim() + " выдан " + Convert.ToDateTime(rowL["ДатаВыдачиПаспорта"]).ToShortDateString().Trim();
            this.lblLПаспорт.Text = номерПаспорта.Trim();

            //Получим и выведим документ на основании которого выведена льгота
            string документ = "Серия документа " + rowL["СерияДокумента"].ToString().Trim() + " № " + rowL["НомерДокумента"].ToString().Trim() + " выдан " + Convert.ToDateTime(rowL["ДатаВыдачиДокумента"]).ToShortDateString().Trim();
            this.lblDoc.Text = документ.Trim();

            //========Выведим статус договора (т.е договор прошёл проверку или договор не прошёл проверку)
            //для этого  получим id договора
            //string stringStstus = "select ФлагПроверки from dbo.Договор where id_договор = "+ this.IdДоговор +" ";
            //bool flagStatus = Convert.ToBoolean(ТаблицаБД.GetTableSQL(stringStstus, "Договор").Rows[0]["ФлагПроверки"]);

            bool flagStatus = Convert.ToBoolean(rowL["ФлагПроверки"]);

            if (flagStatus == true)
            {
                txtStatus.Text  = " Договор прошёл проверку ";
            }
            else
            {
                txtStatus.Text = " Договор не прошёл проверку ";
            }
           

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}