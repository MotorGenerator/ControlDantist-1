using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Classes;
using System.IO;

namespace ControlDantist
{
    public partial class FormConfigSearch : Form
    {
        public FormConfigSearch()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Сохраним конфигурацию поиска льготника по базе данных ЭСРН

            ////Поиск по ФИО
            //if (this.chkFIO.Checked == true && this.chkDR.Checked == false && this.chkS.Checked == false && this.chkDV.Checked == false)
            //{
            //    UpdateSearch up = new UpdateSearch(1);
            //    up.Execute();
            //}

            ////Поиск по ФИО и дате рождения
            //if (this.chkFIO.Checked == true && this.chkDR.Checked == true && this.chkS.Checked == false && this.chkDV.Checked == false)
            //{
            //    UpdateSearch up = new UpdateSearch(2);
            //    up.Execute();
            //}

            ////Поиск по ФИО, дате рождения и по серии документа предоставляющего льготу
            //if (this.chkFIO.Checked == true && this.chkDR.Checked == true && this.chkS.Checked == true && this.chkDV.Checked == false)
            //{
            //    UpdateSearch up = new UpdateSearch(3);
            //    up.Execute();
            //}

            ////Поиск по ФИО, дате рождения,серии документа предоставляющего льготу и по дате документа предоставляющее льготу
            //if (this.chkFIO.Checked == true && this.chkDR.Checked == true && this.chkS.Checked == true && this.chkDV.Checked == true)
            //{
            //    UpdateSearch up = new UpdateSearch(4);
            //    up.Execute();
            //}

            ////Поиск по ФИО, дате рождения,серии документа предоставляющего льготу и по дате документа предоставляющее льготу
            //if (this.chkFIO.Checked == true && this.chkDR.Checked == true && this.chkS.Checked == true && this.chkDV.Checked == true && this.chkPasport.Checked == true)
            //{
            //    UpdateSearch up = new UpdateSearch(5);
            //    up.Execute();
            //}

            //======================================

            //Поиск по ФИО
            if (rbFIO.Checked == true)
            {
                UpdateSearch up = new UpdateSearch(1);
                up.Execute();
            }

            //Поиск по ФИО и дате рождения
            if (rbFIO2.Checked == true)
            {
                UpdateSearch up = new UpdateSearch(2);
                up.Execute();
            }

            //Поиск по ФИО, дате рождения и по серии документа предоставляющего льготу
            if (rbFIO3.Checked == true)
            {
                UpdateSearch up = new UpdateSearch(3);
                up.Execute();
            }

            //Поиск по ФИО, дате рождения,серии документа предоставляющего льготу и по дате документа предоставляющее льготу
            if (rbFIO4.Checked == true)
            {
                UpdateSearch up = new UpdateSearch(4);
                up.Execute();
            }

            //Поиск по ФИО, дате рождения,серии документа предоставляющего льготу и по дате документа предоставляющее льготу
            if (rbFIO5.Checked == true)
            {
                UpdateSearch up = new UpdateSearch(5);
                up.Execute();
            }



            //закроем форму
            this.Close();
        }

        private void FormConfigSearch_Load(object sender, EventArgs e)
        {
            //string query = "select configSearch from ConfgSearch";
            //int iConfig = 0;
            string iConfig = string.Empty;

            //using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
            //{
            //    con.Open();
            //    SqlTransaction transact = con.BeginTransaction();
            //    iConfig = Convert.ToInt32(ТаблицаБД.GetTableSQL(query, "ConfgSearch", con, transact).Rows[0][0]);
            //}

            //Прочитаем файл конфигурации Config.dll (для конфигурирования выгрузки реестра в файл)
            using (FileStream fs = File.OpenRead("Config.dll"))
            using (TextReader read = new StreamReader(fs))
            {
                iConfig = read.ReadLine();
                //if (sConfig == "1")
                //{
                //    //Разрешаем выгрузку реестра в файл
                //    this.chkВыгрузки.Checked = true;
                //}
                //else
                //{
                //    //запрещаем выгрузку реестра в файл
                //    this.chkВыгрузки.Checked = false;
                //}

                //if (iConfig == "1")
                //{
                //    this.chkFIO.Checked = true;
                //}

                //if (iConfig == "2")
                //{
                //    this.chkFIO.Checked = true;
                //    this.chkDR.Checked = true;
                //}

                //if (iConfig == "3")
                //{
                //    this.chkFIO.Checked = true;
                //    this.chkDR.Checked = true;
                //    this.chkS.Checked = true;
                //}

                //if (iConfig == "4")
                //{
                //    this.chkFIO.Checked = true;
                //    this.chkDR.Checked = true;
                //    this.chkS.Checked = true;
                //    this.chkDV.Checked = true;
                //}

                //if (iConfig == "5")
                //{
                //    this.chkFIO.Checked = true;
                //    this.chkDR.Checked = true;
                //    this.chkS.Checked = true;
                //    this.chkDV.Checked = true;
                //    this.chkPasport.Checked = true;
                //}


                if (iConfig == "1")
                {
                    this.rbFIO.Checked = true;
                }

                if (iConfig == "2")
                {
                    this.rbFIO2.Checked = true;
                }

                if (iConfig == "3")
                {
                    this.rbFIO3.Checked = true;
                }

                if (iConfig == "4")
                {
                    this.rbFIO4.Checked = true;
                }

                if (iConfig == "5")
                {
                    this.rbFIO5.Checked = true;
                }

            }

        }
    }
}