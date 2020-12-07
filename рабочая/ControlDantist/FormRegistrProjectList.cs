using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlDantist.ValidateRegistrProject;
using ControlDantist.Repository;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DantistLibrary;
using ControlDantist.Classes;


namespace ControlDantist
{
    public partial class FormRegistrProjectList : Form
    {
        public IQueryable<IRegistrItem> RegistrItems { get; set; }

        /// <summary>
        /// Флаг проверки серверов.
        /// </summary>
        public bool FlagConnectServer { get; set; }
        public FormRegistrProjectList()
        {
            InitializeComponent();
        }

        private void FormRegistrProjectList_Load(object sender, EventArgs e)
        {
            // Заполним данными DataGrid.
            this.dataGridView1.DataSource = this.RegistrItems;

            // Отформатируем DataGrid.
            DisplayLK();

        }

        private void DisplayLK()
        {
            this.dataGridView1.Columns["IdProjectRegistr"].Visible = false;
            this.dataGridView1.Columns["NumberLetter"].Width = 370;
            this.dataGridView1.Columns["NumberLetter"].HeaderText = "Номер входящего письма";
            this.dataGridView1.Columns["DataLetter"].Width = 80;
            this.dataGridView1.Columns["DataLetter"].HeaderText = "Дата письма";
            this.dataGridView1.Columns["StatusValide"].Width = 80;
            this.dataGridView1.Columns["StatusValide"].HeaderText = "Ожидание проверки";

            //this.dataGridView1.Columns.Add(new DataGridViewColumn());

        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            // Получим id записи.
            UnitDate unitDate = new UnitDate();

            FiltrRepositoryДоговор filtrRepositoryДоговор = new FiltrRepositoryДоговор(unitDate);

            // Получим id файла реестра проектов договоров.
            int idFile = Convert.ToInt32(this.dataGridView1.CurrentRow.Cells["IdProjectRegistr"].Value);

            // Получим статус ожидания провреки.
            bool flagValid = Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells["StatusValide"].Value);

            if(flagValid == true)
            {
                MessageBox.Show("Реестр уже прошёл проверку.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //this.Close();

                //// Завершим проверку.
                //return;
             
            }

            // Открываем проекты договорв находящихся в статусе ожидающих проверку.
            var договорs = unitDate.ViewЛьготникДоговорРеестрRepository.Select(idFile);

            // Скачаем файл и посмотрим льгоотную категорию.
            Dictionary<string, Unload> unload = unitDate.ProjectRegistrFilesRepository.SelectFiles(idFile);

            //Создадим список объектов для хранения результатов проверки проектов договоров
            Dictionary<string, ValidateContract> listValContr = new Dictionary<string, ValidateContract>();

            string льготнаяКатегория = string.Empty;

            if (unload != null)
            {
                льготнаяКатегория = unload.FirstOrDefault().Value.ЛьготнаяКатегория;

                //договорs.Count()

                if (договорs.Count() > 0)
                {
                    // Получим ИНН поликлинники.
                    string инн = unload.Values.FirstOrDefault().Поликлинника.Rows[0]["инн"].ToString();

                    EsrnValidate esrnValidate = new EsrnValidate(договорs,льготнаяКатегория);

                    var result = esrnValidate.ValidateList();
                    
                    //Выведим результат проверки на экран
                    FormValidOutEsrn formOutVal = new FormValidOutEsrn(result, инн);
                    formOutVal.IdFileRegistr = idFile;

                    formOutVal.Show();

                    formOutVal.WindowState = FormWindowState.Normal;

                }
                else
                {
                    MessageBox.Show("Нетданных в базе по реестру");
                }
            }
            else
            {
                MessageBox.Show("Нет файла в реестре");
            }

            return;

            if(unload != null)
            {
                //переменная конфигурации
                int iConfig;

                //Создадим список объектов для хранения результатов проверки проектов договоров
                //Dictionary<string, ValidateContract> listValContr = new Dictionary<string, ValidateContract>();

                //Получим конфигурацию поиска льготника в базе данных ЭСРН
                using (FileStream fs = File.OpenRead("Config.dll"))
                using (TextReader read = new StreamReader(fs))
                {
                    iConfig = Convert.ToInt32(read.ReadLine().Trim());
                    fs.Close();
                    read.Close();
                }


                //Установим для нашей программы текущую директорию для корректного считывания пути к БД
                Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;

                // Это для кривых договоров.
                //Dictionary<string, Unload> unload2 = unload.Where(w => w.Key != "СпКмрСо/1554").ToDictionary(w => w.Key,w=>w.Value);


                //проверим льготников по базе данных в ЭСРН
                ValidateЭСРН эсрн = new ValidateЭСРН(unload, listValContr, iConfig);

                //EsrnValidate esrnValidate = new EsrnValidate();

                // Укажем подключать ся ли к серверам или нет.
                эсрн.FlagConnectServer = this.FlagConnectServer;

                if (FlagConnectServer == true)
                {
                    эсрн.FlagЗагрузки = true;
                }

                // Выполним сверку проектов договоров с базами данных ЭСРН.
                Dictionary<string, ValidateContract> validReestr = эсрн.ValidateList();//.Validate();

                //Выведим результат проверки на экран
                FormValidOut formOutVal = new FormValidOut();

                //Передадим в форму выгруженные договора
                formOutVal.ПроектыДоговоров = validReestr;

                //передадим в результаты проверки договоров
                formOutVal.ВыгрузкаПроектДоговоров = unload;

                //Выведим форму на первое место
                //formOutVal.TopMost = true;


                formOutVal.Show();
            }
            else
            {
                MessageBox.Show("Нет файла в реестре");
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
