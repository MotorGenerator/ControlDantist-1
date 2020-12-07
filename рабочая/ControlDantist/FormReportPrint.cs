using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Repozirories;
using ControlDantist.Reports;

namespace ControlDantist
{
    public partial class FormReportPrint : Form
    {
        public FormReportPrint()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Получение данных из базы данных.
            ReportYearRepozitory repoz = new ReportYearRepozitory();
            DataTable dataReport = repoz.GetData();

            // Коллекция классов описывающая отчет.
            ReportDataToList reportToList = new ReportDataToList();
            List<ReportYear>listDataReport = reportToList.ConvertToList(dataReport);

            if (listDataReport != null)
            {

                // Передадим данные для отчета в фабричный метод.
                PrintReportForYear reportForYear = new PrintReportForYear();
                //ReportInformToYearM reportForYear = new ReportInformToYearM();
                reportForYear.Print(listDataReport);
               
            }
            else
            {
                MessageBox.Show("Данных для печати нет, или возникла ошибка");
            }

        }
    }
}
