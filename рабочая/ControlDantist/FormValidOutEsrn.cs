using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlDantist.ClassValidRegions;
using ControlDantist.DisplayRegistr;
using ControlDantist.DisplayDatePerson;
using ControlDantist.Letter;
using ControlDantist.Repository;
using ControlDantist.Classes;
using ControlDantist.ValidPersonContract;
using ControlDantist.LetterClassess;
using DantistLibrary;


namespace ControlDantist
{
    public partial class FormValidOutEsrn : Form
    {
        private Dictionary<string, PersonValidEsrn> dictionary;

        private string инн = string.Empty;

        /// <summary>
        /// Id файла с ррестром файлов проектов договоров.
        /// </summary>
        public int IdFileRegistr { get; set; }

        public FormValidOutEsrn(Dictionary<string, PersonValidEsrn> dictionary, string инн)
        {
            InitializeComponent();

            if (dictionary != null)
            {
                this.dictionary = dictionary;

                this.инн = инн;
            }
            else
            {
                throw new Exception("Отсутствуют данные в реестре проектов догворов");
            }
        }

        private void FormValidOutEsrn_Load(object sender, EventArgs e)
        {
            DisplayRegistrProject displayRegistrProject = new DisplayRegistrProject(dictionary);

            // 
            this.dataGridView1.DataSource = displayRegistrProject.GetRegistr();

            // Отформатируем DataGrid.
            ViewDataGrid();

            // Выведим строку итого: количество договоров и общая сумма договоров.
            var countContracts = displayRegistrProject.GetRegistr().Count;
            if (countContracts > 0)
            {
                string итого = "кол-во договоров - " + countContracts + " на сумму " + displayRegistrProject.GetSumContracts().ToString("c");

                this.lblCount.Text = "";

                this.lblCount.Text = итого;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ViewDataGrid()
        {
            this.dataGridView1.Columns["IdContract"].Visible = false;
            this.dataGridView1.Columns["НомерДоговора"].Width = 60;
            this.dataGridView1.Columns["FlagValidEsrn"].HeaderText = "Проверка ЭСРН";
            this.dataGridView1.Columns["FlagValidServices"].HeaderText = "Проверка услуг";
            this.dataGridView1.Columns["FlagSaveContract"].HeaderText = "Сохранить в БД";
            this.dataGridView1.Columns["SumContract"].HeaderText = "Сумма договора";
            this.dataGridView1.Columns["Адрес"].HeaderText = "Адрес";
            this.dataGridView1.Columns["Адрес"].Visible = false;
            this.dataGridView1.Columns["СерияНомерУдостоверения"].HeaderText = "Серия номер удостоверения";
            this.dataGridView1.Columns["СерияНомерУдостоверения"].Visible = false;

        }

        private void dataGridView1_Click(object sender, EventArgs e)
          {
            this.txtЛьготник.Text = "";
            this.txtАдрес.Text = "";
            this.txtДокумент.Text = "";

            // Отображать данные по льготнику будем только у прошедших проверку.
            DataGridViewRow row = this.dataGridView1.CurrentRow;

            int idContract = Convert.ToInt32(row.Cells["IdContract"].Value);

            if (Convert.ToBoolean(row.Cells["FlagValidEsrn"].Value) == true)
            {
                DisplayResultValidate displayResultValidate = new DisplayResultValidate(idContract);

                DatePersonForDisplay dp = displayResultValidate.GetFioPerson(this.dictionary);

                this.txtЛьготник.Text = dp.Фио + " " + dp.ДатаРождения + " г. р.";
                this.txtАдрес.Text =  dp.Адрес;
                this.txtДокумент.Text = dp.Удостоверение;
            }

            if (Convert.ToBoolean(row.Cells["FlagValidServices"].Value) == false)
            {
                // Выведим разницу в услугах у льготников которые не прошли проверку по услугам.
                ShowDifference showDifference = new ShowDifference(idContract);

                // 
                var list = showDifference.Display();

                if(list != null && list.Count > 0)
                {
                    this.dataError.DataSource = list;

                    this.txtЛьготник.Text = "";
                    this.txtАдрес.Text = "";
                    this.txtДокумент.Text = "";
                }
                else
                {
                    var listServ = showDifference.DisplayErrorServer();

                    this.dataCorrect.DataSource = listServ;

                    this.txtЛьготник.Text = "";
                    this.txtАдрес.Text = "";
                    this.txtДокумент.Text = "";
                }

                // Покажем услуги которых нет на сервере.

            }

        }

        private void btnDist_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in this.dataGridView1.Rows)
            {
                string numContract = row.Cells["НомерДоговора"].Value.ToString();

                if (Convert.ToBoolean(row.Cells["FlagValidEsrn"].Value) == true && (Convert.ToBoolean(row.Cells["FlagValidServices"].Value) == true))
                {
                    row.Cells["FlagSaveContract"].Value = true;
                }
                else
                {
                    //MessageBox.Show("Льготник - "+ row.Cells["ФиоЛьготник"].Value.ToString()  + " Договор № - " + numContract.Trim() + "  не может быть записан");
                    MessageBox.Show("Льготник " + row.Cells["ФиоЛьготник"].Value.ToString().Trim() + "  не может быть записан");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Соберем всех прошедших проверку.
            List<PersonValidEsrn> dictionaryTrue = dictionary.Values.Where(w => w.flagValidEsrn == true && w.flagValidMedicalServices == true).ToList();

            ILetter tryLetter = new ПодтвержденныеПисьма(dictionaryTrue, инн);

            tryLetter.Print();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Соберем всех прошедших роверку.
            // Соберем всех непрошедших проверку.
            List<PersonValidEsrn> dictionaryTrue = dictionary.Values.Where(w => w.flagValidEsrn == false || w.flagValidMedicalServices == false).ToList();

            ILetter tryLetter = new НеПодтвержденные(dictionaryTrue, инн);

            tryLetter.Print();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            int idContract = Convert.ToInt32(this.dataGridView1.CurrentRow.Cells["IdContract"].Value);

            // Проверим значение флага проверки льготника по ЭСРН.
            bool flagValidEsrn = Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells["FlagValidEsrn"].Value);

            // Получим текущий контракт.
            var currContract = dictionary.Values.Where(w => w.IdContract == idContract).FirstOrDefault();

            // Если Установлен флаг проверки в True.
            if (flagValidEsrn == true)
            {
                // Установим что льготнико прошел проверку.
                if(currContract != null)
                {
                    currContract.flagValidEsrn = true;
                    currContract.FlagValidPersonFioEsrn = true;
                    currContract.FlagValidPersonPassword = true;
                }
            }
            else
            {
                // Если флаг установлен в False.
                if (currContract != null)
                {
                    // То установим, что льгоник не прошел проверку.
                    currContract.flagValidEsrn = false;
                    currContract.FlagValidPersonFioEsrn = false;
                    currContract.FlagValidPersonPassword = false;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Данные для отчета о наличии договоров у льготников.
            List<PrintContractsValidate> listDoc = new List<PrintContractsValidate>();

            List<ValidItemsContract> listContracts = new List<ValidItemsContract>();

            //Узнаем содержатся ли ещё договора у текущего льготника
            using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
            {
                con.Open();

                SqlTransaction transact = con.BeginTransaction();

                // Пройдемся по всем договорам и проверим есть ли оплаченные.
                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    // Получим номер догвора.
                    string numContract = row.Cells["НомерДоговора"].Value.ToString();

                    //  Для ускорения вставляем старый код.
                    // узнаем есть ли на сервере такой номер договора который прошёл проверку
                    string queryNumDogServ = "select count(НомерДоговора) from Договор where LOWER(RTRIM(LTRIM(НомерДоговора))) = LOWER(RTRIM(LTRIM('" + numContract + "'))) and  ФлагПроверки = 'True' ";

                    //string queryNumDogServ = "select count(НомерДоговора) from Договор where LOWER(RTRIM(LTRIM(НомерДоговора))) = LOWER(RTRIM(LTRIM('" + numContract + "'))) ";

                    // Таблица с результатами проверки.
                    DataTable tab = ТаблицаБД.GetTableSQL(queryNumDogServ, "ПроверкаДоговора", con, transact);

                    // Получим ID контракта.
                    int idContract = Convert.ToInt32(row.Cells["IdContract"].Value);

                    //Тест коментарий снять 18.08.2020 года.
                    if (Convert.ToInt32(tab.Rows[0][0]) != 0)
                    {
                        // Выведим сообщение что такой льготник уже существует.
                        FormModalPerson fmp = new FormModalPerson();
                        fmp.NumContract = numContract;
                        fmp.ShowDialog();

                        if (fmp.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            // Получим текущий договор.
                            var contract = dictionary.Values.Where(w => w.IdContract == idContract).FirstOrDefault();

                            // Пометим договор как не прошщедший проверку.
                            contract.flagValidEsrn = false;

                            //если такой договор уже в БД существует то сразу переходим к другой итерации
                            continue;
                        }
                    }


                    var person = dictionary.Values.Where(w => w.IdContract == idContract).FirstOrDefault();

                    if(Convert.ToBoolean(row.Cells["FlagSaveContract"].Value) == false)
                    {
                        // Пометим как не прошедший проверку.
                        person.flagValidEsrn = false;

                        // Пометим что договор отправлен на доработку.
                    }


                    // Проверим есть ли у данного льготника ещё заключенные договора.
                    ValidContractForPerson validContract = new ValidContractForPerson(person.фамилия, person.имя, person.отчество.Do(x => x, ""), Convert.ToDateTime(person.датаРождения));
                    //validContract.listContracts = listContracts;
                    validContract.SetSqlConnection(con);
                    validContract.SetSqlTransaction(transact);
                    validContract.SetNumContract(numContract);
                    PrintContractsValidate договор = validContract.GetContract();

                    listDoc.Add(договор);
                   
                }

            }

            if (listDoc.Count > 0)
            {
                WordReport wordPrint = new WordReport(listDoc);

                DocPrint docPrint = new DocPrint(wordPrint);
                docPrint.Execute();
            }

            var предохранитьель = "";

            //Предложим запись в БД 
            FormDalogWriteBD formDialog = new FormDalogWriteBD();
            formDialog.ShowDialog();

            if (formDialog.DialogResult == System.Windows.Forms.DialogResult.OK)
            {

                // Установим уровни изоляции транзакций.
                var option = new System.Transactions.TransactionOptions();
                option.IsolationLevel = System.Transactions.IsolationLevel.Serializable;

                // Добавим льготника и адрес в БД.
                // Внесём данные в таблицу в едино транзакции.
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, option))
                {
                    try
                    {
                        UnitDate unitDate = new UnitDate();

                        foreach (var itm in dictionary.Values)
                        {

                            var contract = unitDate.ДоговорRepository.SelectContract(itm.IdContract);

                            // Льготник прошёл проверку.
                            if (itm.flagValidEsrn == true)
                            {
                                // Файл прошёл проверку.
                                contract.ФлагПроверки = true;

                                // Флаг ожидания проверку и True так как файл проверен.
                                contract.flagОжиданиеПроверки = true;

                                // Дата записи договора при проверке.
                                contract.ДатаЗаписиДоговора = (DateTime)DateTime.Now.Date;

                                // Дата проверки.
                                contract.ДатаПроверки = (DateTime)DateTime.Now.Date;

                                // кто записал проверку.
                                contract.logWrite = MyAplicationIdentity.GetUses();

                                // Отправим флаг на доработку.
                                contract.ФлагВозвратНаДоработку = false;
                            }
                            else
                            {
                                // Установим флаг ожидания провреки в true, так как файл уже проверен.
                                contract.flagОжиданиеПроверки = true;

                                // Отправим флаг на доработку.
                                contract.ФлагВозвратНаДоработку = true;

                                // Установим флаг проверки как не прошедший проверку.
                                contract.ФлагПроверки = false;

                                // Дата записи договора при проверке.
                                contract.ДатаЗаписиДоговора = (DateTime)DateTime.Now.Date;

                                // Дата проверки.
                                contract.ДатаПроверки = (DateTime)DateTime.Now.Date;

                                // кто записал проверку.
                                contract.logWrite = MyAplicationIdentity.GetUses();
                            }

                            // Сохраним изменения в БД.
                            unitDate.ДоговорRepository.Update(contract);

                            ProjectRegistrFiles projectRegistrFiles = unitDate.ProjectRegistrFilesRepository.Select(this.IdFileRegistr).OrderByDescending(w=>w.IdProjectRegistr).FirstOrDefault();

                            if (projectRegistrFiles != null)
                            {
                                // Реестр прошёл проверку.
                                projectRegistrFiles.flagValidateRegistr = true;

                                // Сохраним изменения в БД.
                                unitDate.ProjectRegistrFilesRepository.Update(projectRegistrFiles);
                            }

                        }

                        // Завершим транзакцию.
                        scope.Complete();

                        MessageBox.Show("Договора сохранены");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            // Закрыл окно.
            this.Close();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

            if (dataGridView1.CurrentCell.ColumnIndex.Equals(7) && e.RowIndex != -1)
            {
                // Проверим какой флаг установлен.
                bool flagValidEsrn = Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells["FlagValidEsrn"].Value);

                // Флаг наличия проверки медицинских услуг.
                bool flagValidServices = Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells["FlagValidServices"].Value);

                if (flagValidEsrn == false || flagValidServices == false)
                {
                    MessageBox.Show("Договор не прошел проверку");

                    return;
                }

            }

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridView1.ClearSelection();
            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

            DataGridViewRow rows = this.dataGridView1.Rows[e.RowIndex];

            //получим номер договора
            //this.ПроектыДоговоров;
            int idContract = Convert.ToInt32(rows.Cells["IdContract"].Value.Do(x=>x,0));

            FormInfoЛьготник formInfoЛьготник = new FormInfoЛьготник(idContract);
            formInfoЛьготник.Show();



        }
    }
}
