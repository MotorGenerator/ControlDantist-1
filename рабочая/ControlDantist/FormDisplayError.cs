using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlDantist
{
    public partial class FormDisplayError : Form
    {
        private DataTable dtServ;
        private DataTable dtFile;

        public string LkServer;
        public string LkFile;

        /// <summary>
        /// Хранит данные с сервера
        /// </summary>
        public DataTable DtServer
        {
            get
            {
                return dtServ;
            }
            set
            {
                dtServ = value;
            }
        }

        /// <summary>
        /// Хранит данные сиз файла выгрузки
        /// </summary>
        public DataTable DtFile
        {
            get
            {
                return dtFile;
            }
            set
            {
                dtFile = value;
            }
        }

        public FormDisplayError()
        {
            InitializeComponent();
        }

        private void FormDisplayError_Load(object sender, EventArgs e)
        {
            //Отобразим данные с сервера
            DataRow rs = this.DtServer.Rows[0];

            this.label3.Text = "Фамилия:  " + rs["Фамилия"].ToString().Trim();
            this.label5.Text = "Имя: " + rs["Имя"].ToString().Trim();
            this.label7.Text = "Отчество: " + rs["Отчество"].ToString().Trim();
            this.label8.Text = "д.р. : " + Convert.ToDateTime(rs["ДатаРождения"]).ToShortDateString();
            this.label10.Text = "ул. : " + rs["улица"].ToString().Trim();
            this.label11.Text = "дом: " + rs["НомерДома"].ToString().Trim();
            this.label12.Text = "корп.: " + rs["корпус"].ToString().Trim();
            this.label13.Text = "кв.: " + rs["НомерКвартиры"].ToString().Trim();

            this.label14.Text = "паспорт: " + rs["СерияПаспорта"].ToString().Trim() + "  " + rs["НомерПаспорта"].ToString().Trim();
            this.label15.Text = "выдан: " + rs["КемВыданПаспорт"].ToString().Trim();
            this.label16.Text = "дата выдачи: " + Convert.ToDateTime(rs["ДатаВыдачиПаспорта"]).ToShortDateString();

            this.label17.Text = "документ: " + rs["СерияДокумента"].ToString().Trim() + " " + rs["НомерДокумента"].ToString().Trim();
            this.label18.Text = "выдан: " + rs["КемВыданДокумент"].ToString().Trim();
            this.label19.Text = "дата выдачи: " + Convert.ToDateTime(rs["ДатаВыдачиДокумента"]).ToShortDateString();


            if (this.DtFile.Rows.Count > 0)
            {
                //Отобразим данные из файла выгрузки
                DataRow rf = this.DtFile.Rows[0];

                this.label32.Text = "Фамилия:  " + rf["Фамилия"].ToString().Trim();
                this.label31.Text = "Имя: " + rf["Имя"].ToString().Trim();
                this.label30.Text = "Отчество: " + rf["Отчество"].ToString().Trim();
                this.label29.Text = "д.р. : " + Convert.ToDateTime(rf["ДатаРождения"]).ToShortDateString();
                this.label27.Text = "ул. : " + rf["улица"].ToString().Trim();
                this.label26.Text = "дом: " + rf["НомерДома"].ToString().Trim();
                this.label25.Text = "корп.: " + rf["корпус"].ToString().Trim();
                this.label24.Text = "кв.: " + rf["НомерКвартиры"].ToString().Trim();

                this.label23.Text = "паспорт: " + rf["СерияПаспорта"].ToString().Trim() + "  " + rf["НомерПаспорта"].ToString().Trim();
                this.label22.Text = "выдан: " + rf["КемВыданПаспорт"].ToString().Trim();
                this.label21.Text = "дата выдачи: " + Convert.ToDateTime(rf["ДатаВыдачиПаспорта"]).ToShortDateString();

                this.label20.Text = "документ: " + rf["СерияДокумента"].ToString().Trim() + " " + rf["НомерДокумента"].ToString().Trim();
                this.label6.Text = "выдан: " + rf["КемВыданДокумент"].ToString().Trim();
                this.label4.Text = "дата выдачи: " + Convert.ToDateTime(rf["ДатаВыдачиДокумента"]).ToShortDateString();
            }
            else
            {
                this.label32.Text = "Льготник отсутствует в базе данных";
            }

            // Отобразим льготные категории на сервере и в файле.
            this.label9.Text = LkServer.Trim();
            this.label28.Text = LkFile.Trim();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
