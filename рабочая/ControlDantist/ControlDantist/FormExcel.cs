using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Classes;


using System.Globalization;
using System.Threading;


namespace ControlDantist
{
    public partial class FormExcel : Form
    {
        private List<RowExcel> listExecel;

        /// <summary>
        /// Хранит список льготников импортированных из файла Excel
        /// </summary>
        public List<RowExcel> ListExcel
        {
            get
            {
                return listExecel;
            }
            set
            {
                listExecel = value;
            }
        }

        public FormExcel()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormExcel_Load(object sender, EventArgs e)
        {
            CultureInfo newCInfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            newCInfo.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = newCInfo;

            //Отобразим льготников если они есть в файлах Excel
            this.dataGridView1.DataSource = this.ListExcel;
            this.dataGridView1.Columns["Num"].HeaderText = "Номер договора";
            this.dataGridView1.Columns["Num"].DisplayIndex = 0;
            this.dataGridView1.Columns["Num"].Visible = true;

            this.dataGridView1.Columns["Фамилия"].HeaderText = "Фамилия";
            this.dataGridView1.Columns["Фамилия"].DisplayIndex = 1;
            this.dataGridView1.Columns["Фамилия"].Visible = true;

            this.dataGridView1.Columns["Имя"].HeaderText = "Имя";
            this.dataGridView1.Columns["Имя"].DisplayIndex = 2;
            this.dataGridView1.Columns["Имя"].Visible = true;

            this.dataGridView1.Columns["Отчество"].HeaderText = "Отчество";
            this.dataGridView1.Columns["Отчество"].DisplayIndex = 3;
            this.dataGridView1.Columns["Отчество"].Visible = true;


        }
    }
}