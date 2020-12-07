using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Repozirories;
using ControlDantist.ReportVipNet;

namespace ControlDantist
{
    public partial class FiltrPersonForVipNet : Form
    {
        private UnitWork unit;

        public FiltrPersonForVipNet()
        {
            InitializeComponent();

            unit = new UnitWork();

            // Получим спискок районов.
            this.comboBox1.DataSource = unit.RegionRepository.GetAll();
            this.comboBox1.ValueMember = "idRegion";
            this.comboBox1.DisplayMember = "NameRegion";
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            IRegistr registr = new Registr(unit);
            
            // ID район.
            registr.IdRegion = Convert.ToInt16(this.comboBox1.SelectedValue);

            // Дата начала отчетного периода.
            registr.DateStartPeriod = this.dateTimePicker1.Value;

            // Дата окончания отчетного периода.
            registr.DateEndPeriod = this.dateTimePicker2.Value;

            // Наименование района области.
            registr.NameRegion = this.comboBox1.Text.ToString();

            bool flagLetter = registr.GetPersons();

            if (flagLetter == false)
            {
                MessageBox.Show("Договоров с " + this.comboBox1.Text + " р-н нет");
            }

            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
