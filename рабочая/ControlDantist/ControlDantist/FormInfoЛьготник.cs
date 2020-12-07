using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Classes;
using DantistLibrary;

namespace ControlDantist
{
    public partial class FormInfoЛьготник : Form
    {
        private Unload unload;
        DataRow rГород;
        private string город = string.Empty;

        public Unload Unloads
        {
            get
            {
                return unload;
            }
            set
            {
                unload = value;
            }
        }

        public FormInfoЛьготник()
        {
            InitializeComponent();

        }

        private void FormInfoЛьготник_Load(object sender, EventArgs e)
        {
            Unload vr = this.Unloads;
            

            DataRow rowL = vr.Льготник.Rows[0];

            string фамилия = rowL["Фамилия"].ToString().Trim();
            string имя = rowL["Имя"].ToString().Trim();
            string отчество = rowL["Отчество"].ToString().Trim() + " " + Convert.ToDateTime(rowL["ДатаРождения"]).ToShortDateString().Trim() + " г.р.";

            //string серияПаспорта = rowL["СерияПаспорта"].ToString().Trim();
            //string номерПаспорта = rowL["НомерПаспорта"].ToString().Trim();

            //Вместо паспорат поставим серю и номер документа дающего право на льготу ДатаВыдачиДокумента
            string серияПаспорта = rowL["СерияДокумента"].ToString().Trim();
            string номерПаспорта = rowL["НомерДокумента"].ToString().Trim() + " выдан  " + Convert.ToDateTime(rowL["ДатаВыдачиДокумента"]).ToShortDateString().Trim();

            //получим серию и номер паспорта
            string серияПаспортаЛьготника = rowL["СерияПаспорта"].ToString().Trim();
            string номерПаспортаЛьготника = rowL["НомерПаспорта"].ToString().Trim() + " выдан  " + Convert.ToDateTime(rowL["ДатаВыдачиПаспорта"]).ToShortDateString().Trim();

            //получим адрес
            
            if (vr.НаселённыйПункт.Rows.Count != 0)
            {
                rГород = vr.НаселённыйПункт.Rows[0];
                город = rГород["Наименование"].ToString().Trim();
            }

            
            string улица = rowL["улица"].ToString().Trim();
            string номерДома = rowL["НомерДома"].ToString().Trim();

            string номерКорпуса = rowL["корпус"].ToString().Trim();
            string номерКв = rowL["НомерКвартиры"].ToString().Trim();

            //Отобразим ФИО
            lblF.Text = фамилия;
            lblNam.Text = имя;
            lblFat.Text = отчество;

            //отобразим серию и номер документа дающегно парво на льготу
            lblSeria.Text = серияПаспорта;
            lblPasport.Text = номерПаспорта;

            //отобразим серию и номер паспрота льготника
            lblSerPass.Text = серияПаспортаЛьготника;
            lblNumPass.Text = номерПаспортаЛьготника;



            //отобразим адрес
            lblStreet.Text = "н.п. " + город + "  ул. " + улица;
            lblHous.Text = "дом " + номерДома;
            lblKorp.Text = "корп. " + номерКорпуса;
            lblEpartment.Text = "кв. " + номерКв;

            //отобразим льготную категорию
            lblLK.Text = vr.ЛьготнаяКатегория.Trim();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}