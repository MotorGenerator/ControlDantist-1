using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Classes;
using DantistLibrary;
using ControlDantist.Repository;
using System.Collections.Generic;
using System.Linq;
using ControlDantist.Repository;

namespace ControlDantist
{
    public partial class FormInfoЛьготник : Form
    {
        private Unload unload;
        DataRow rГород;
        private string город = string.Empty;

        // Переменная для хранения номера договора.
        private int idContract = 0;

        private bool flagId = false;

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


        public FormInfoЛьготник(int idContract)
        {
            InitializeComponent();

            this.idContract = idContract;

            flagId = true;

        }

        private void FormInfoЛьготник_Load(object sender, EventArgs e)
        {
            UnitDate unitDate = new UnitDate();

            FiltrRepositoryДоговор filtrRepositoryДоговор = new FiltrRepositoryДоговор(unitDate);

            // ПОлучим текущий договор.
            ControlDantist.Repository.Договор contract = unitDate.ДоговорRepository.FiltrДоговор(this.idContract);

            if (contract != null)
            {

                if (contract != null)
                {
                    // Получим id льготника.
                    int idPerson = (int)contract.id_льготник;

                    //var person = unitDate.ЛьготникRepository.Select(idPerson).FirstOrDefault();
                    var person = unitDate.ЛьготникRepository.FiltrЛьготник(idPerson);

                    if (person != null)
                    {
                        //Отобразим ФИО
                        this.lblF.Text = person.Фамилия.Trim();
                        this.lblNam.Text = person.Имя.Trim();
                        this.lblFat.Text = person.Отчество.Trim() + " " + person.ДатаРождения.Value.ToShortDateString() +" г.р.";

                        //отобразим серию и номер документа дающегно парво на льготу
                        this.lblSeria.Text = person.СерияДокумента.Trim();
                        this.lblPasport.Text = person.НомерДокумента.Trim() + " выдан " + person.ДатаВыдачиДокумента.Value.ToShortDateString();

                        //отобразим серию и номер паспрота льготника
                        this.lblSerPass.Text =person.СерияПаспорта.Trim();
                        this.lblNumPass.Text = person.НомерПаспорта.Trim() + " выдан " + person.ДатаВыдачиПаспорта.Value.ToShortDateString();

                        var city = unitDate.НаселенныйПунктRepository.FiltrНаселенныйПункт((int)person.id_насПункт);

                        //отобразим адрес
                        this.lblStreet.Text = "н.п. " + city.Наименование.Trim() + "  ул. " + person.улица.Trim();
                        this.lblKorp.Text = "дом " + person.НомерДома;
                        this.lblHous.Text = "корп. " + person.корпус;
                        this.lblEpartment.Text = "кв. " + person.НомерКвартиры;

                        var lk = unitDate.ЛьготнаяКатегорияRepository.Select((int)person.id_льготнойКатегории).FirstOrDefault();

                        if (lk != null)
                        {
                            //отобразим льготную категорию
                            this.lblLK.Text = lk.ЛьготнаяКатегория1.Trim();
                        }

                    }
                }

            }

            /*
            if (flagId == false)
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
            else
            {
                UnitDate unitDate = new UnitDate();

                FiltrRepositoryДоговор filtrRepository = new FiltrRepositoryДоговор(unitDate.ДоговорRepository);

                // ПОлучим текущий договор.
                ControlDantist.Repository.Договор contract = filtrRepository.Select(this.idContract);

                if (contract != null)
                {

                   if (contract != null)
                    {
                        // Получим id льготника.
                        int idPerson = (int)contract.id_льготник;

                        //var person = unitDate.ЛьготникRepository.Select(idPerson).FirstOrDefault();
                        var person = unitDate.ЛьготникRepository.FiltrЛьготник(idPerson);

                        if (person != null)
                        {
                            //Отобразим ФИО
                           this.lblF.Text = person.Фамилия.Trim();
                           this.lblNam.Text = person.Имя.Trim();
                           this.lblFat.Text = person.Отчество.Trim();

                            //отобразим серию и номер документа дающегно парво на льготу
                            this.lblSeria.Text = person.СерияПаспорта.Trim();
                            this.lblPasport.Text = person.НомерПаспорта.Trim();

                            //отобразим серию и номер паспрота льготника
                            this.lblSerPass.Text = person.СерияДокумента.Trim();
                            this.lblNumPass.Text = person.НомерДокумента.Trim();


                            //FiltrRepositoryГород filtrRepositoryThoun = new FiltrRepositoryГород(unitDate.НаселенныйПунктRepository);

                            //var city = filtrRepositoryThoun.Select((int)person.id_насПункт);

                            var city = unitDate.НаселенныйПунктRepository.FiltrНаселенныйПункт((int)person.id_насПункт);

                            //отобразим адрес
                            this.lblStreet.Text = "н.п. " + city.Наименование.Trim() + "  ул. " + person.улица.Trim();
                            this.lblKorp.Text = "дом " + person.НомерДома;
                            this.lblHous.Text = "корп. " + person.корпус;
                            this.lblEpartment.Text = "кв. " + person.НомерКвартиры;

                            var lk = unitDate.ЛьготнаяКатегорияRepository.Select((int)person.id_льготнойКатегории).FirstOrDefault();

                            if (lk != null)
                            {
                                //отобразим льготную категорию
                                this.lblLK.Text = lk.ЛьготнаяКатегория1.Trim();
                            }

                        }
                    }

                    //unitDate.ДоговорRepository.SelectAll();
                }
            }

    */
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}