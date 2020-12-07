using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using Microsoft.Office.Interop.Word;
using System.Runtime.Serialization.Formatters.Binary;

using ControlDantist.Classes;
using DantistLibrary;
using ControlDantist.WriteClassDB;

namespace ControlDantist
{
    public partial class FormValidOut : Form
    {
        private Dictionary<string, ValidateContract> validReestr;
        private Dictionary<string, Unload> unload;
        
        //коллекция хранит выбранные проекты договоров для сохранения в БД
        private Dictionary<string,Unload> listSave;

        //коллекция хранит проекты оговоров которые не выбраны (не прошли проверку)
        private Dictionary<string, Unload> listPrintNoSave;

        //коллекция хранит проекты выбранные проекты договоров
        private Dictionary<string, Unload> listPrintSave;

        // Коллекция для хранения данных для статистики.
        private Dictionary<string, Unload> listPrintStatistic;

        //коллекция хранит проекты договоров нуждающихся в дополнительной проверке
        private Dictionary<string, Unload> listPrintAddCheck;
        
        //хранит серию документа
        string серияДок = string.Empty;

        //хранит номер документа
        string номерДокумента = string.Empty;

        //хранит дату выдачи документа
        string датаВыдДок = string.Empty;

        //Флаг хранит установку времени 
        private bool setupTime = false;

        private bool writBD;
        private Unload iv;

        private bool реестрДоговоров;

        private List<ErrorReestr> listControl;

        public List<ErrorReestr> РеестрДоговоров
        {
            get
            {
                return listControl;
            }
            set
            {
                listControl = value;
            }
        }



        /// <summary>
        /// Усвойство устанавливает что форма будет отображать реестр выполненных договоров
        /// </summary>
        public bool РеестрВыполненныхДоговоров
        {
            get
            {
                return реестрДоговоров;
            }
            set
            {
                реестрДоговоров = value;
            }
        }

        /// <summary>
        ///Указывает что записывае всю базу
        /// </summary>
        public bool ЗаписатьБазу
        {
            get
            {
                return writBD;
            }
            set
            {
                writBD = value;
            }
        }

        //Пока зарезервируем
        //private bool flagWrite;

        ///// <summary>
        ///// Свойство хранит флаг указывающий что форма должна работать на запись всех договоров
        ///// </summary>
        //public bool FlagWrite
        //{
        //    get
        //    {
        //        return flagWrite;
        //    }
        //    set
        //    {
        //        flagWrite = value;
        //    }
        //}

        /// <summary>
        /// Список с результатом проверки проектов договоров
        /// </summary>
        public Dictionary<string, ValidateContract> ПроектыДоговоров
        {
            get
            {
                return validReestr;
            }
            set
            {
                validReestr = value;
            }
        }


        /// <summary>
        /// Список выгруженных проектов договоров подлежащих проверке
        /// </summary>
        public Dictionary<string, Unload> ВыгрузкаПроектДоговоров
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




        public FormValidOut()
        {
            InitializeComponent();
        }

        private void FormValidOut_Load(object sender, EventArgs e)
        {
            if (this.РеестрВыполненныхДоговоров == false)
            {
                //Установим флаг выбора даты в false
                setupTime = false;

                // Загрузим проекты договоров которые прошли проверку.
                Dictionary<string, ValidateContract> validUnload = this.ПроектыДоговоров;

                //Создадим список для отображения результатов проверки
                List<ContractDisplay> list = new List<ContractDisplay>();

                //коллекция хранит выбранные для сохранения в БД проекты договоров
                listSave = new Dictionary<string, Unload>();

                //коллекция хранит договора прошедшие проверку
                listPrintSave = new Dictionary<string, Unload>();

                //коллекция хранит договроа не прошедшие проверку
                listPrintNoSave = new Dictionary<string, Unload>();

                //коллекция хранит проекты договоров нуждающихся в дополнительной проверке
                listPrintAddCheck = new Dictionary<string, Unload>();

                //переменная хранит сумму проектов договоров в текущем реестре (списке договоров)
                decimal sumCount = 0.0m;

                // ПРойдемся по коллекции с проектами договоров которые прошли проверку.
                foreach (string keyVK in validUnload.Keys)
                {
                    ContractDisplay contract = new ContractDisplay();
                    contract.НомерДоговора = keyVK.Trim();

                   //// Получим ID района и СНИЛС льготника.
                   // if (this.ПроектыДоговоров.ContainsKey(keyVK.Trim()))
                   // {
                   //     // Получим id района области.
                   //     contract.IdRegion = this.ПроектыДоговоров[keyVK.Trim()].IdRegionEsrn;

                   //     // Получим СНИЛС льготника.
                   //     contract.Sinls = this.ПроектыДоговоров[keyVK.Trim()].SnilsPerson;
                   // }


                    //Новый поиск
                    if (ВыгрузкаПроектДоговоров.ContainsKey(keyVK.Trim()))
                    {
                        iv = ВыгрузкаПроектДоговоров[keyVK.Trim()];
                    }

                    //Получим под текущий ключ реестр договоров
                    //Unload iv = ВыгрузкаПроектДоговоров[keyVK.Trim()];


                    //Узнаем из поля объекта реестр договоров ФИО льготника
                    DataRow rowL = iv.Льготник.Rows[0];

                    string фамилия = rowL["Фамилия"].ToString();
                    string имя = rowL["Имя"].ToString();
                    string отчество = rowL["Отчество"].ToString();

                    //Присвоим списку индикации ФИО льготника
                    contract.ФИО = фамилия + " " + имя + " " + отчество;
                    contract.ПроверкаЭСРН = validUnload[keyVK].FlagPersonЭСРН;
                    contract.ПроверкаУслуг = validUnload[keyVK].flagErrorSumm;

                    //Получим сумму по текущему договору
                    DataTable tabSum = iv.УслугиПоДоговору;

                    decimal sum = 0.0m;
                    foreach (DataRow r in tabSum.Rows)
                    {
                        sum = sum + Convert.ToDecimal(r["Сумма"]);
                    }

                    //Присвоим 
                    contract.SumService = sum.ToString("c");

                    //Получим сумму по реестру
                    sumCount = sumCount + sum;

                    list.Add(contract);
                }

                //отобразим общую сумму в реестре проектов договоров  и количество договоров
                this.lblCount.Text += " общая сумма " + sumCount.ToString("c") + " количество договоров " + list.Count.ToString() + "  шт.";


                //Отобразим результат проверки в гриде
                this.dataGridView1.DataSource = list;

                //Настроим названия колонок
                this.dataGridView1.Columns["НомерДоговора"].HeaderText = "Номер договора";
                this.dataGridView1.Columns["НомерДоговора"].ReadOnly = true;
                this.dataGridView1.Columns["НомерДоговора"].DisplayIndex = 0;

                this.dataGridView1.Columns["ФИО"].HeaderText = "ФИО льготника";
                this.dataGridView1.Columns["ФИО"].ReadOnly = true;
                this.dataGridView1.Columns["ФИО"].DisplayIndex = 1;

                this.dataGridView1.Columns["ПроверкаЭСРН"].HeaderText = "Результат проверки в ЭСРН";
                this.dataGridView1.Columns["ПроверкаЭСРН"].DisplayIndex = 2;

                this.dataGridView1.Columns["ПроверкаУслуг"].HeaderText = "Результат проверки услуг";
                this.dataGridView1.Columns["ПроверкаУслуг"].DisplayIndex = 3;

                //this.dataGridView1.Columns["СохранитьДоговор"].Visible = false;
                this.dataGridView1.Columns["СохранитьДоговор"].HeaderText = "Сохранить договор";
                this.dataGridView1.Columns["СохранитьДоговор"].DisplayIndex = 4;

                //Запретим редактировать колонку Сохранить договор
                this.dataGridView1.Columns["СохранитьДоговор"].ReadOnly = true;
                this.dataGridView1.Columns["SumService"].HeaderText = "Сумма договора";
                this.dataGridView1.Columns["SumService"].DisplayIndex = 5;
                this.dataGridView1.Columns["SumService"].ReadOnly = true;

                //this.dataGridView1.Columns["IdRegion"].DisplayIndex = 6;
                //this.dataGridView1.Columns["IdRegion"].Visible = false;

                //this.dataGridView1.Columns["Sinls"].DisplayIndex = 7;
                //this.dataGridView1.Columns["Sinls"].Visible = false;
            }

            if (this.РеестрВыполненныхДоговоров == true)
            {
                // Заполним DataGrid данными о проверки реестра в ЭСРН.
                this.dataGridView1.DataSource = this.РеестрДоговоров;

            }

            //Настроим названия колонок
            //this.dataGridView1.Columns["НомерДоговора"].HeaderText = "Номер договора";
            //this.dataGridView1.Columns["НомерДоговора"].ReadOnly = true;
            //this.dataGridView1.Columns["НомерДоговора"].DisplayIndex = 0;

            //this.dataGridView1.Columns["ФИО"].HeaderText = "ФИО льготника";
            //this.dataGridView1.Columns["ФИО"].ReadOnly = true;
            //this.dataGridView1.Columns["ФИО"].DisplayIndex = 1;
            //this.dataGridView1.Columns["ПроверкаЭСРН"].HeaderText = "Результат проверки в ЭСРН";
            //this.dataGridView1.Columns["ПроверкаЭСРН"].DisplayIndex = 2;
            //this.dataGridView1.Columns["ПроверкаУслуг"].HeaderText = "Результат проверки услуг";
            //this.dataGridView1.Columns["ПроверкаУслуг"].DisplayIndex = 3;
            ////this.dataGridView1.Columns["СохранитьДоговор"].Visible = false;
            //this.dataGridView1.Columns["СохранитьДоговор"].HeaderText = "Сохранить договор";
            //this.dataGridView1.Columns["СохранитьДоговор"].DisplayIndex = 4;

            ////Запретим редактировать колонку Сохранить договор
            //this.dataGridView1.Columns["СохранитьДоговор"].ReadOnly = true;
            //this.dataGridView1.Columns["SumService"].HeaderText = "Сумма договора";
            //this.dataGridView1.Columns["SumService"].DisplayIndex = 5;
            //this.dataGridView1.Columns["SumService"].ReadOnly = true;


            ////Скроем последний столбец (Новая версия программы)
            

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
                Dictionary<string, ValidateContract> validUnload = this.ПроектыДоговоров;

                //Получим номер договора 
                string numDog = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();

                //if (this.dataGridView1.CurrentRow.Cells[2].Value.ToString() == "True")
                //{
                //    var asd = "True";
                //}
                //else
                //{
                //    //if (this.dataGridView1.Columns[2].HeaderText == "Результат проверки в ЭСРН")
                //    if (this.dataGridView1.CurrentCell.ColumnIndex == 2)
                //    {

                //        var dListtCOntractTest = this.listSave;


                //        FormRegions formRegion = new FormRegions();
                //        DialogResult dRezult = formRegion.ShowDialog();

                //        if (dRezult == System.Windows.Forms.DialogResult.OK)
                //        {
                //            var idRegion = formRegion.IdRegion;

                //            // Пометим договор как прошедший проверку и установим id района области.
                //            unload[numDog].IdRegionEsrn = idRegion.ToString();
                //       }
                //    }
                //}

                

                //if (this.dataGridView1.CurrentCell.ColumnIndex == 2 || this.dataGridView1.CurrentCell.ColumnIndex == 1)
                //{
                    //Выведим данные по льготнику из ЭСРН
                    List<ЭСРНvalidate> льготник = validUnload[numDog].СписокДокументов;
                    string фио = string.Empty;

                    try
                    {
                        foreach (ЭСРНvalidate эсрн in льготник)
                        {

                            //фио = эсрн.Фамилия + " " + эсрн.Имя + " " + эсрн.Отчество + " Дата рождения - " + эсрн.ДатаРождения + " Название документа - " + эсрн.НазваниеДокумента + " " + эсрн.СерияДокумента + " " + эсрн.НомерДокумента + " Адрес: " + эсрн.Адрес; 
                            this.txtЛьготник.Text = эсрн.Фамилия.Trim() + " " + эсрн.Имя.Trim() + " " + эсрн.Отчество.Trim() + " " + эсрн.ДатаРождения.Trim() + "г.р.";
                            this.txtАдрес.Text = " Адрес: " + эсрн.Адрес.Trim();

                            серияДок = string.Empty;

                            //Проверим если серия документа равна NULl
                            if (эсрн.СерияДокумента != null)
                            {
                                серияДок = эсрн.СерияДокумента.Trim();
                            }
                            else
                            {
                                серияДок = "";
                            }

                            номерДокумента = string.Empty;
                            if (эсрн.НомерДокумента != null)
                            {
                                номерДокумента = эсрн.НомерДокумента.Trim();
                            }

                            датаВыдДок = string.Empty;
                            if (эсрн.ДатаВыдачиДокумента != null)
                            {
                                датаВыдДок = эсрн.ДатаВыдачиДокумента.Trim();
                            }
                            else
                            {
                                датаВыдДок = "";
                            }

                            this.txtДокумент.Text = эсрн.НазваниеДокумента.Trim() + " " + серияДок.Trim() + " " + номерДокумента + " выдан " + датаВыдДок;
                        }
                    }
                    catch
                    {
                        this.txtЛьготник.Text = "Льготник с заданными параметрами в базе ЭСРН отсутствует";
                        this.txtАдрес.Text = "";
                        this.txtДокумент.Text = "";
                    }


                    //Выведим информацию о льготнике
                    //this.txtЛьготник.Text = фио;


                //}


                //if (this.dataGridView1.CurrentCell.ColumnIndex == 2 || this.dataGridView1.CurrentCell.ColumnIndex == 1 || this.dataGridView1.CurrentCell.ColumnIndex == 3)
                //{
                    //Выведим данные по льготнику из ЭСРН
                    List<ЭСРНvalidate> льготник1 = validUnload[numDog].СписокДокументов;
                    string фио1 = string.Empty;


                    try
                    {

                        foreach (ЭСРНvalidate эсрн in льготник1)
                        {


                            //фио = эсрн.Фамилия + " " + эсрн.Имя + " " + эсрн.Отчество + " Дата рождения - " + эсрн.ДатаРождения + " Название документа - " + эсрн.НазваниеДокумента + " " + эсрн.СерияДокумента + " " + эсрн.НомерДокумента + " Адрес: " + эсрн.Адрес; 
                            this.txtЛьготник.Text = эсрн.Фамилия.Trim() + " " + эсрн.Имя.Trim() + " " + эсрн.Отчество.Trim() + " " + эсрн.ДатаРождения.Trim() + "г.р.";
                            this.txtАдрес.Text = " Адрес: " + эсрн.Адрес.Trim();

                            серияДок = string.Empty;

                            //Проверим если серия документа равна NULl
                            if (эсрн.СерияДокумента != null)
                            {
                                серияДок = эсрн.СерияДокумента.Trim();
                            }
                            else
                            {
                                серияДок = "";
                            }

                            номерДокумента = string.Empty;
                            if (эсрн.НомерДокумента != null)
                            {
                                номерДокумента = эсрн.НомерДокумента.Trim();
                            }

                            датаВыдДок = string.Empty;
                            if (эсрн.ДатаВыдачиДокумента != null)
                            {
                                датаВыдДок = эсрн.ДатаВыдачиДокумента.Trim();
                            }
                            else
                            {
                                датаВыдДок = "";
                            }

                            this.txtДокумент.Text = эсрн.НазваниеДокумента.Trim() + " " + серияДок.Trim() + " " + номерДокумента + " выдан " + датаВыдДок;

                            //this.txtДокумент.Text = эсрн.НазваниеДокумента.Trim() + " " + эсрн.СерияДокумента.Trim() + " " + эсрн.НомерДокумента.Trim() + " выдан " + эсрн.ДатаВыдачиДокумента;
                        }
                    }
                    catch
                    {
                        this.txtЛьготник.Text = "Льготник с заданными параметрами в базе ЭСРН отсутствует";
                        this.txtАдрес.Text = "";
                        this.txtДокумент.Text = "";
                    }

                    //если список расхождений не равен null

                    if (validUnload[numDog].СписокРасхождений != null)
                    {
                        List<ErrorsReestrUnload> errorUbload = validUnload[numDog].СписокРасхождений;

                        //список для хранения корректных данных
                        List<DateService> listCorrect = new List<DateService>();

                        //Список для хранения ошибочных данных
                        List<DateService> listError = new List<DateService>();

                        //Соберём правильные и ошибочные данные 
                        foreach (ErrorsReestrUnload date in errorUbload)
                        {
                            //Запишем правильные данные
                            DateService dataCorrect = new DateService();

                            //не будем писать в коллекцию пустые строки
                            //if (date.НаименованиеУслуги != "" || date.НаименованиеУслуги != null)
                            if (date.НаименованиеУслуги != string.Empty)
                            {
                                dataCorrect.Наименование = date.НаименованиеУслуги;

                                dataCorrect.Цена = date.Цена.ToString("c");
                                dataCorrect.Сумма = date.Сумма.ToString("c");

                                //Запишем правлитьные данные в коллекцию
                                listCorrect.Add(dataCorrect);
                            }

                            DateService dataError = new DateService();

                            //Не будем писать в коллекцию пустые строки
                            //if (date.ErrorНаименованиеУслуги != "" || date.ErrorНаименованиеУслуги != null)
                            if (date.ErrorНаименованиеУслуги != string.Empty || date.ErrorНаименованиеУслуги != null)
                            {
                                dataError.Наименование = date.ErrorНаименованиеУслуги;

                                dataError.Цена = date.ErrorЦена.ToString("c");
                                dataError.Сумма = date.ErrorСумма.ToString("c");

                                //Запишем ошибочные данные в коллекцию
                                listError.Add(dataError);
                            }
                        }

                        //отобразим правильные данные
                        this.dataCorrect.DataSource = listCorrect;

                        //отобразим ошибочные данные
                        this.dataError.DataSource = listError;
                    }
                    else
                    {
                        //отобразим правильные данные
                        this.dataCorrect.DataSource = null;

                        //отобразим ошибочные данные
                        this.dataError.DataSource = null;
                    }

                //}


                ////Если пользователь выбрал галочку в столбце Сохранить договор
                ////Выберим договоры для выгрузки в реестр
                string numberDog = string.Empty;
                if (this.dataGridView1.CurrentRow.Cells["СохранитьДоговор"].Selected == true)
                {

                    try
                    {
                        //если выбрали договор
                        numberDog = this.dataGridView1.CurrentRow.Cells["НомерДоговора"].Value.ToString().Trim();

                        bool bolУслуг = Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells["ПроверкаУслуг"].Value);
                        bool boolЭСРН = Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells["ПроверкаЭСРН"].Value);
                        //bool boolФИО = Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells["ФИО"].Value);

                        //Unload unloadSave = ВыгрузкаПроектДоговоров[numberDog];

                        if (bolУслуг == true && boolЭСРН == true)// && boolФИО == true)
                        {
                            this.dataGridView1.CurrentRow.Cells["СохранитьДоговор"].ReadOnly = false;
                            //this.listSave.Add(numberDog, unloadSave);
                        }
                        else
                        {
                            MessageBox.Show("Договор № " + numberDog + " не возможно записать в базу данных");
                        }
                    }
                    catch
                    {
                        //если пользователь вторично выбрал ячейку значит он хочет снять флажок и убрать из коллекции договор
                        numberDog = this.dataGridView1.CurrentRow.Cells["НомерДоговора"].Value.ToString().Trim();
                        //this.listSave.Remove(numberDog);
                    }
                }
            //}
            //catch
            //{
            //    MessageBox.Show("Где то ошибка");
            //}

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool flagУслуг = Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells["ПроверкаУслуг"].Value);
            bool flagЭСРН = Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells["ПроверкаЭСРН"].Value);

            bool test = Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells["СохранитьДоговор"].Value);

            if (flagУслуг == true && flagЭСРН == true)
            {
                this.dataGridView1.CurrentRow.Cells["СохранитьДоговор"].ReadOnly = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Найдём договор на текущего льготника который под текущем номером который уже прошёл проверку.
            //Если такой договор есть то мы запретим загружать данный файл выгрузки

            List<PrintContractsValidate> listContract = new List<PrintContractsValidate>();

            // Пройдёмся по таблице DataGridView и поместим в коллекцию на сохранения только договора
            // у которых ВыгрузкаПроектДоговоров столбце Сохранить договор
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                // Порлучим номер договора
                string numberDog = row.Cells["НомерДоговора"].Value.ToString().Trim();

                // Старая реализация программы оставим вдруг понадобиться на потом
                // запишим договора у которых в столбце Сохранить договор стоит отметка 
                if (Convert.ToBoolean(row.Cells["СохранитьДоговор"].Value) == true)
                {
                    // Получим договор из выгрузки.
                    Unload unloadSave = ВыгрузкаПроектДоговоров[numberDog];

                    
                    // Получим экземпляр договора прошедшего проверку.
                    ValidateContract unloadValid = this.ПроектыДоговоров[numberDog];

                    // Установим проверку на случай если район не найден в ЭСРН, а установлен в ручную.
                    if (unloadValid.IdRegionEsrn != null)
                    {
                        // Получим ID район области.
                        unloadSave.IdRegionEsrn = unloadValid.IdRegionEsrn;
                    

                        // Присвоим ID района области для списка который пойдет под запись в БД.
                        //unloadSave.IdRegionEsrn = unloadValid.IdRegionEsrn;

                    }

                    // Получим СНИЛС льготника прошедшего проверку.
                    unloadSave.SnilsPerson = unloadValid.SnilsPerson;

                    // Установим флаг в TRUE о том что проект прошёл проверку
                    unloadSave.FalgWrite = true;

                    //Запишем в словарь
                    this.listSave.Add(numberDog, unloadSave);
                }
                else
                {
                    /*
                    * В связи с изменениями просто записываем все проекты договоров в базу с в том числе и не прошедшие проверку
                    */

                    if (ВыгрузкаПроектДоговоров.ContainsKey(numberDog.Trim()))
                    {
                        // Получим из библиотеки проект договора.
                        this.iv = ВыгрузкаПроектДоговоров[numberDog.Trim()];
                    }

                    // Установим флаг в FALSE говорящий о том что проект договора не прошёл проверку
                    //unloadSave.FalgWrite = false;
                    this.iv.FalgWrite = false;

                    // Запишем в словарь все проекты договоров
                    //this.listSave.Add(numberDog, unloadSave);
                    this.listSave.Add(numberDog, this.iv);
                }
            }

            //если список проектов договоров не равен 0
            if (listSave.Count != 0)
            {
                //преобразуем словарь в список
                Dictionary<string, Unload> iTest = this.listSave;

                //Создадим список элементов типа Unload
                List<Unload> listUnload = new List<Unload>();

                //Узнаем содержатся ли ещё договора у текущего льготника
                using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
                {
                    con.Open();
                    SqlTransaction transact = con.BeginTransaction();

                    foreach (Unload unload in listSave.Values)
                    {

                        //Получим номер текущего договора хранящейся в файле
                        string numDogFile = unload.Договор.Rows[0]["НомерДоговора"].ToString().Trim();

                        if (numDogFile == "8/5624")
                        {
                            var dTest = "";
                        }

                        // узнаем есть ли на сервере такой номер договора который прошёл проверку
                        string queryNumDogServ = "select count(НомерДоговора) from Договор where LOWER(RTRIM(LTRIM(НомерДоговора))) = LOWER(RTRIM(LTRIM('" + numDogFile + "'))) and  ФлагПроверки = 'True' ";

                        // Таблица с результатами проверки.
                        DataTable tab = ТаблицаБД.GetTableSQL(queryNumDogServ, "ПроверкаДоговора", con, transact);

                        // Тест коментарий снять 24,09,2018 года.
                        if (Convert.ToInt32(tab.Rows[0][0]) != 0)
                        {
                            // Выведим сообщение что такой льготник уже существует.
                            FormModalPerson fmp = new FormModalPerson();
                            fmp.NumContract = numDogFile;
                            fmp.ShowDialog();

                            if (fmp.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                //если такой договор уже в БД существует то сразу переходим к другой итерации
                                //continue;
                            }
                        }
                 

                        listUnload.Add(unload);

                        //Получим ФИО льготника
                        DataRow rowL = unload.Льготник.Rows[0];

                        //Получим поликлиннику
                        DataRow rowH = unload.Поликлинника.Rows[0];

                        // Запроc показывает что в других поликлинниках данный льготник не получал услуги
                        string query = "select НомерДоговора,ДатаДоговора from dbo.Договор " +
                                              "where id_льготник in ( " +
                                              "SELECT [id_льготник] FROM [Льготник] " +
                                              "where [Фамилия] = '" + rowL["Фамилия"].ToString() + "' and Имя = '" + rowL["Имя"].ToString() + "' and Отчество = '" + rowL["Отчество"].ToString() + "' and ДатаРождения = '" + rowL["ДатаРождения"].ToString().Trim() + "') ";// and id_поликлинника <> (select top 1 id_поликлинника from dbo.Поликлинника where ИНН = '" + rowH["ИНН"].ToString().Trim() + "' order by id_поликлинника desc) ";

 
                        DataTable tabUnload = ТаблицаБД.GetTableSQL(query, "Договор", con, transact);
                        

                        //хранит данные для текущего льготника
                        PrintContractsValidate contr = new PrintContractsValidate();

                        string фамилия = rowL["Фамилия"].ToString();
                        string имя = rowL["Имя"].ToString();

                        string отчество = rowL["Отчество"].ToString();
                        string датаРождения = Convert.ToDateTime(rowL["ДатаРождения"]).ToShortDateString();

                        //Присвоим списку индикации ФИО льготника
                        string ФИО = фамилия + " " + имя + " " + отчество;

                        //получим текущий номер договора
                        DataRow rowDog = unload.Договор.Rows[0];
                        string numDog = rowDog["НомерДоговора"].ToString().Trim();

                     
                        contr.ФИО_Номер_ТекущийДоговор = ФИО.Trim();
                        contr.НомерТекущийДоговор = numDog.Trim();

                        //Динамическая строка для хранения 
                        StringBuilder builder = new StringBuilder();

                        // Получим договоры которые уже есть у льготника
                        foreach(DataRow row in tabUnload.Rows)
                        {
                            //получим номер договора
                            string номДог = row["НомерДоговора"].ToString().Trim();
                            string датаДог = Convert.ToDateTime(row["ДатаДоговора"]).ToShortDateString();

                            string договор = номДог + " от " + датаДог + " ; ";

                            //если договор не подписан то и дату не выводим
                            if (датаДог.Trim() == "01.01.1900")
                            {
                                договор = номДог + " ;";
                            }
                            
                            builder.Append(договор);
                        }

                        //если договоров нет
                        if (builder.Length == 0)
                        {
                            contr.НомераДоговоров = "нет";
                        }
                        else
                        {
                            contr.НомераДоговоров = "заключённые договора - " + builder.ToString().Trim();
                        }

                         //string querDubl = "select dbo.Договор.id_договор, НомерДоговора,ДатаДоговора, dbo.АктВыполненныхРабот.НомерАкта,dbo.АктВыполненныхРабот.ДатаПодписания  from dbo.Договор " +
                         //              " INNER  JOIN dbo.АктВыполненныхРабот " +
                         //              " ON dbo.Договор.id_договор = dbo.АктВыполненныхРабот.id_договор " +
                         //               " where id_льготник  " +
                         //               " in ( SELECT [id_льготник] FROM [Льготник] " +
                         //               "where [Фамилия] = '" + rowL["Фамилия"].ToString() + "' and Имя = '" + rowL["Имя"].ToString() + "' and Отчество = '" + rowL["Отчество"].ToString() + "' and ДатаРождения = '" + rowL["ДатаРождения"].ToString().Trim() + "') ";

                        string querDubl = " select dbo.Договор.id_договор, НомерДоговора,Договор.ДатаЗаписиДоговора,ДатаДоговора, dbo.АктВыполненныхРабот.НомерАкта, " +
                                          " dbo.АктВыполненныхРабот.ДатаПодписания  from dbo.Договор left outer JOIN dbo.АктВыполненныхРабот " +
                                          " ON dbo.Договор.id_договор = dbo.АктВыполненныхРабот.id_договор " +
                                          " where id_льготник  " +
                                          " in ( SELECT [id_льготник] FROM [Льготник] " +
                                          " where [Фамилия] = '" + rowL["Фамилия"].ToString() + "' and Имя = '" + rowL["Имя"].ToString() + "' and Отчество = '" + rowL["Отчество"].ToString() + "' and ДатаРождения = '" + rowL["ДатаРождения"].ToString().Trim() + "') " +
                                          " and Договор.ФлагПроверки = 'True' ";
                  

                        DataTable tabContr = ТаблицаБД.GetTableSQL(querDubl, "Дубли", con, transact);
                        
                        // Список с номерами ранее заключонных договоров для текущего льготника (как в текущей поликлиннике так и в других поликлинниках)
                        StringBuilder listNumDog = new StringBuilder();

                        foreach (DataRow row in tabContr.Rows)
                        {
                            //listNumDog.Append(row["НомерДоговора"].ToString().Trim() + " от - " + Convert.ToDateTime(row["ДатаПодписания"]).ToShortDateString() + "; ");

                            if (DBNull.Value != row["НомерДоговора"])
                            {
                                listNumDog.Append('\n' + " " + row["НомерДоговора"].ToString().Trim());

                                //if (DBNull.Value != row["ДатаЗаписиДоговора"])
                                //{
                                //    listNumDog.Append("дата записи договора - " + Convert.ToDateTime(row["ДатаЗаписиДоговора"]).ToShortDateString());
                                //}
                            }

                            if(DBNull.Value != row["НомерАкта"])
                            {

                                //if (DBNull.Value != row["ДатаДоговора"])
                                //{
                                //    listNumDog.Append(" дата договора - " + Convert.ToDateTime(row["ДатаДоговора"]).ToShortDateString());
                                //}

                                //listNumDog.Append(" акт № " + row["НомерАкта"].ToString().Trim());// + " от - " + Convert.ToDateTime(row["ДатаПодписания"]).ToShortDateString());

                                if (DBNull.Value != row["ДатаПодписания"])
                                {

                                    listNumDog.Append(" от - " + Convert.ToDateTime(row["ДатаПодписания"]).ToShortDateString() + "; ");
                                }
                            }
                            
                        }

                        contr.СписокДоговоров = listNumDog.ToString();

                        // Запишем договора в коллекцию
                        listContract.Add(contr);
                    }
                }


                List<Unload> test = listUnload;
                // Запишем в БД
                //MessageBox.Show("Записываем в БД");


                //Запишем данные в документ word
                List<PrintContractsValidate> lTest = listContract;

                string filName = System.Windows.Forms.Application.StartupPath + @"\Шаблон\Список договоров.doc";

                //Создаём новый Word.Application
                Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();

                //Загружаем документ
                Microsoft.Office.Interop.Word.Document doc = null;

                object fileName = filName;
                object falseValue = false;
                object trueValue = true;
                object missing = Type.Missing;

                doc = app.Documents.Open(ref fileName, ref missing, ref trueValue,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing);

                //Горизонтальное ориентирование листа
                //doc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;


                object bookNaziv = "таблица";
                Range wrdRng = doc.Bookmarks.get_Item(ref  bookNaziv).Range;

                object behavior = Microsoft.Office.Interop.Word.WdDefaultTableBehavior.wdWord8TableBehavior;
                object autobehavior = Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitWindow;

                Microsoft.Office.Interop.Word.Table table = doc.Tables.Add(wrdRng, 1, 4, ref behavior, ref autobehavior);
                table.Range.ParagraphFormat.SpaceAfter = 6;
                table.Columns[1].Width = 40;
                table.Columns[2].Width = 140;
                table.Columns[3].Width = 80;
                table.Columns[4].Width = 200;
                table.Borders.Enable = 1; // Рамка - сплошная линия
                table.Range.Font.Name = "Times New Roman";
                table.Range.Font.Size = 10;
                //счётчик строк
                int i = 1;

                //Список с данными для таблицы
                List<ContractListItem> list = new List<ContractListItem>();

                //Сформируем шапку таблицы
                ContractListItem шапка = new ContractListItem();
                шапка.Num = "№ п/п";
                шапка.FIO = "ФИО льготника";
                шапка.NumCurrentContract = "Номер текущего договора";
                //шапка.NumContracts = "Ранее заключённые договора в других поликлинниках";
                шапка.NumContr = "Ранее заключённые договора";

                list.Add(шапка);

                //Счётчик
                int iCount = 1;

                //Заполним данными таблицу
                foreach (PrintContractsValidate un in listContract)
                {
                    //Заполним данными таблицу
                    ContractListItem item = new ContractListItem();

                    //Порядковый номер
                    item.Num = iCount.ToString().Trim();

                    //Запишем ФИО льготника
                    item.FIO = un.ФИО_Номер_ТекущийДоговор.Trim();

                    //номер текущего договора
                    item.NumCurrentContract = un.НомерТекущийДоговор.Trim();

                    //Запишем номера договоров
                    //item.NumContracts = un.НомераДоговоров.Trim();
                    item.NumContr = un.СписокДоговоров.Trim();

                    list.Add(item);

                    //Увеличим счётчик на 1
                    iCount++;
                }

                //Заполним таблицу
                int k = 1;
                //запишем данные в таблицу
                foreach (ContractListItem item in list)
                {
                    //table.Cell(i, 1).Range.Text = i.ToString();//item.НомерПорядковый;
                    table.Cell(k, 1).Range.Text = item.Num;

                    table.Cell(k, 2).Range.Text = item.FIO;

                    table.Cell(k, 3).Range.Text = item.NumCurrentContract;

                    //table.Cell(k, 4).Range.Text = item.NumContracts;
                    table.Cell(k, 4).Range.Text = item.NumContr;


                    //doc.Words.Count.ToString();
                    Object beforeRow1 = Type.Missing;
                    table.Rows.Add(ref beforeRow1);

                    k++;
                }

                table.Rows[k].Delete();

                //Отобразим документ
                app.Visible = true;

                List<Unload> listTest = listUnload;

                foreach (var iTestW in listUnload)
                {
                    var asd = iTestW;
                }

                //Предложим запись в БД 
                FormDalogWriteBD formDialog = new FormDalogWriteBD();
                formDialog.ShowDialog();

                if (formDialog.DialogResult == System.Windows.Forms.DialogResult.OK)
                {


                    WriteBD writBD = new WriteBD(listUnload);

                    // Запишем в БД.
                    writBD.Write();

                    // 
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Выберите договора для записи в базу данных");
            }
        }

        private void btnDist_Click(object sender, EventArgs e)
        {
            //Выделим установим все галочки в DataGridView в значение true

            //Проставим везде галочки
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {

                //// Удалить после отладки.
                //row.Cells["ПроверкаУслуг"].Value = true;
                //row.Cells["ПроверкаЭСРН"].Value = true;

                DataGridViewCheckBoxCell check1 = (DataGridViewCheckBoxCell)row.Cells["ПроверкаУслуг"];
                DataGridViewCheckBoxCell check2 = (DataGridViewCheckBoxCell)row.Cells["ПроверкаЭСРН"];
                if (Convert.ToBoolean(check1.FormattedValue) == true && Convert.ToBoolean(check2.FormattedValue) == true)
                {
                    row.Cells["СохранитьДоговор"].Value = true;

                }
                else
                {
                    //string message = "Льготник " + row.Cells["ФИО"].Value.ToString().Trim() + "  не может быть записан на сервер";
                    //string caption = "Ошибка";
                    //MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    //DialogResult result;

                    //// Displays the MessageBox.

                    //result = MessageBox.Show(this, message, caption, buttons);

                    //if (result == DialogResult.Yes)
                    //{
                    //    row.Cells["СохранитьДоговор"].Value = true;
                    //}

                    MessageBox.Show("Льготник " + row.Cells["ФИО"].Value.ToString().Trim() + "  не может быть записан");
                }
                
            }


            //DataGridViewCheckBoxColumn col1 = (DataGridViewCheckBoxColumn)this.dataGridView1.Columns[2];
            //DataGridViewCheckBoxColumn col2 = (DataGridViewCheckBoxColumn)this.dataGridView1.Columns[3];
            //DataGridViewCheckBoxColumn col3 = (DataGridViewCheckBoxColumn)this.dataGridView1.Columns[4];
            //col1.TrueValue = false;
            //col2.TrueValue = false;
            //col3.TrueValue = false;
            

        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            //если пользователь выбрал дату
            //this.setupTime = true;

            this.button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Обнулим списки
            this.listPrintSave.Clear();
            this.listPrintNoSave.Clear();

            ListLoad();

            if (listPrintSave.Count != 0)
            {
                Dictionary<string, Unload> iTest = listPrintSave;

                //распечатаем документ word
                //Хранит инн поликлинники
                string инн = string.Empty;

                //хранит адресат
                string адрес = string.Empty;

                //хранит обращение (Уважаемый Андрей Юрьевич)
                string обращение = string.Empty;

                //хранит инициалы
                string fio = string.Empty;

                //Распечатаем документ Word
                //string filName = System.Windows.Forms.Application.StartupPath + @"\Шаблон\Письмо Возврат.doc";
                //string filName = @"D:\Проекты\ControlDantist\ControlDantist\bin\Debug\Шаблон\Письмо  Возврат.doc";
                string filName = System.Windows.Forms.Application.StartupPath + @"\Шаблон\Письмо В работу.doc";


                //Создаём новый Word.Application
                Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();

                //Загружаем документ
                Microsoft.Office.Interop.Word.Document doc = null;

                object fileName = filName;
                object falseValue = false;
                object trueValue = true;
                object missing = Type.Missing;

                doc = app.Documents.Open(ref fileName, ref missing, ref trueValue,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing);

                //Горизонтальное ориентирование листа
                //doc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;


                object bookNaziv = "таблица";
                Range wrdRng = doc.Bookmarks.get_Item(ref  bookNaziv).Range;

                object behavior = Microsoft.Office.Interop.Word.WdDefaultTableBehavior.wdWord8TableBehavior;
                object autobehavior = Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitWindow;

                Microsoft.Office.Interop.Word.Table table = doc.Tables.Add(wrdRng, 1, 3, ref behavior, ref autobehavior);
                table.Range.ParagraphFormat.SpaceAfter = 6;
                table.Columns[1].Width = 40;
                table.Columns[2].Width = 140;
                table.Columns[3].Width = 300;
                table.Borders.Enable = 1; // Рамка - сплошная линия
                table.Range.Font.Name = "Times New Roman";
                table.Range.Font.Size = 10;
                //счётчик строк
                int i = 1;

                //Список для хранения данных для таблицы
                List<ReestrNoPrintDog> listItem = new List<ReestrNoPrintDog>();

                //Сформируем шапку таблицы
                ReestrNoPrintDog шапка = new ReestrNoPrintDog();
                шапка.Number = "№ п/п";
                шапка.NumDog = "№ договора";
                шапка.Fio = "ФИО";

                listItem.Add(шапка);

                //Счётчик
                int iCount = 1;

                //Заполним данными таблицу
                foreach (Unload un in listPrintSave.Values)
                {
                    //Строка таблицы
                    ReestrNoPrintDog item = new ReestrNoPrintDog();
                    item.Number = iCount.ToString();

                    //Получим ИНН поликлинники
                    инн = un.Поликлинника.Rows[0]["ИНН"].ToString().Trim();

                    DataRow rowDog = un.Договор.Rows[0];
                    item.NumDog = rowDog["НомерДоговора"].ToString().Trim();

                    //Получим ФИО льготника
                    DataRow rowЛьготник = un.Льготник.Rows[0];

                    string фамилия = rowЛьготник["Фамилия"].ToString().Trim();
                    string имя = rowЛьготник["Имя"].ToString().Trim();
                    string отчество = rowЛьготник["Отчество"].ToString().Trim();

                    //
                    string фио = фамилия + " " + имя + " " + отчество;
                    item.Fio = фио.Trim();

                    listItem.Add(item);

                    iCount++;
                }


                //Заполним таблицу
                int k = 1;
                //запишем данные в таблицу
                foreach (ReestrNoPrintDog item in listItem)
                {
                    //table.Cell(i, 1).Range.Text = i.ToString();//item.НомерПорядковый;
                    table.Cell(k, 1).Range.Text = item.Number;

                    table.Cell(k, 2).Range.Text = item.NumDog;
                    table.Cell(k, 3).Range.Text = item.Fio;


                    //doc.Words.Count.ToString();
                    Object beforeRow1 = Type.Missing;
                    table.Rows.Add(ref beforeRow1);

                    k++;
                }

                table.Rows[k].Delete();

                //Загрузим адрес с даннми о поликлиннике
                using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
                {
                    con.Open();

                    SqlTransaction transact = con.BeginTransaction();

                    //получим id поликлинники
                    string queryINN = "SELECT [F2],[F3],ФИО FROM [Лист1$] where id in (select id_поликлинника from dbo.Поликлинника where ИНН = " + инн + ") ";
                    DataRow row = ТаблицаБД.GetTableSQL(queryINN, "Лист1$", con, transact).Rows[0];

                    //получим адрес
                    адрес = row["F2"].ToString().Trim();

                    //получим обращение
                    обращение = row["F3"].ToString().Trim();

                    //получим фио
                    fio = row["ФИО"].ToString().Trim();

                }

                //Заполним документ WORD данными
                ////Номер договора

                object wdrepl = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt = "адресат";
                object newtxt = (object)адрес;
                //object frwd = true;
                object frwd = false;
                doc.Content.Find.Execute(ref searchtxt, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd, ref missing, ref missing, ref newtxt, ref wdrepl, ref missing, ref missing,
                ref missing, ref missing);

                //Запишем кому
                object wdrepl1 = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt1 = "кому";
                object newtxt1 = (object)обращение;
                //object frwd = true;
                object frwd1 = false;
                doc.Content.Find.Execute(ref searchtxt1, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd1, ref missing, ref missing, ref newtxt1, ref wdrepl1, ref missing, ref missing,
                ref missing, ref missing);

                //запишем фио
                object wdrepl2 = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt2 = "fio";
                object newtxt2 = (object)fio;
                //object frwd = true;
                object frwd2 = false;
                doc.Content.Find.Execute(ref searchtxt2, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd2, ref missing, ref missing, ref newtxt2, ref wdrepl2, ref missing, ref missing,
                ref missing, ref missing);

                //Отобразим документ
                app.Visible = true;
            }
            else
            {
                MessageBox.Show("Нет данных для письма.");
            }
        }

        
        /// <summary>
        /// Собирает список договоров которые прошли и не прошли проверку
        /// </summary>
        private void ListLoad()
        {
            //Обнулим список
            this.listPrintSave.Clear();
            this.listPrintNoSave.Clear();
            this.listPrintAddCheck.Clear();


            //Пройдёмся по таблице DataGridView и поместим в коллекцию на сохранения только договора
            //у которых ВыгрузкаПроектДоговоров столбце Сохранить договор
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                //Старая реализация программы оставим вдруг понадобиться на потом
                //запишим договора у которых в столбце Сохранить договор стоит отметка 
                //if (Convert.ToBoolean(row.Cells["СохранитьДоговор"].Value) == true)
                //{
                //    string numberDog = row.Cells["НомерДоговора"].Value.ToString().Trim();
                //    Unload unloadSave = ВыгрузкаПроектДоговоров[numberDog];

                //    //Запишем в словарь
                //    //this.listPrintSave.Add(numberDog, unloadSave);
                //    this.listPrintSave.Add(numberDog, unloadSave);
                //}
                //else
                //{
                //    string numberDog = row.Cells["НомерДоговора"].Value.ToString().Trim();
                //    Unload unloadSave = ВыгрузкаПроектДоговоров[numberDog];

                //    //Запишем в словарь
                //    this.listPrintNoSave.Add(numberDog, unloadSave);
                //}

                    //ПРОШЕДШИЕ ПРОВЕРКУ
                    if (Convert.ToBoolean(row.Cells["СохранитьДоговор"].Value) == true && Convert.ToBoolean(row.Cells["ПроверкаЭСРН"].Value) == true)
                    {
                        string numberDog = row.Cells["НомерДоговора"].Value.ToString().Trim();
                        Unload unloadSave = ВыгрузкаПроектДоговоров[numberDog];

                        //Запишем в словарь
                        //this.listPrintSave.Add(numberDog, unloadSave);
                        this.listPrintSave.Add(numberDog, unloadSave);
                    }
                   
                    ////НЕ ПРОШЕДШИЕ ПРОВЕРКУ
                    if (Convert.ToBoolean(row.Cells["СохранитьДоговор"].Value) == false && Convert.ToBoolean(row.Cells["ПроверкаЭСРН"].Value) == false)
                    {
                        string numberDog = row.Cells["НомерДоговора"].Value.ToString().Trim();
                        Unload unloadSave = ВыгрузкаПроектДоговоров[numberDog];

                        //Запишем в словарь
                        this.listPrintNoSave.Add(numberDog, unloadSave);
                    }

                    //Дополнительная проверка
                    if (Convert.ToBoolean(row.Cells["СохранитьДоговор"].Value) == false && Convert.ToBoolean(row.Cells["ПроверкаЭСРН"].Value) == true)
                    {
                        string numberDog = row.Cells["НомерДоговора"].Value.ToString().Trim();
                        Unload unloadSave = ВыгрузкаПроектДоговоров[numberDog];

                        //Запишем в словарь
                        this.listPrintAddCheck.Add(numberDog, unloadSave);
                    }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Обнулим списки
            this.listPrintSave.Clear();
            this.listPrintNoSave.Clear();

            //Хранит инн поликлинники
            string инн = string.Empty;

            //хранит адресат
            string адрес = string.Empty;

            //хранит обращение (Уважаемый Андрей Юрьевич)
            string обращение = string.Empty;

            //хранит инициалы
            string fio = string.Empty;


            ListLoad();

            Dictionary<string, Unload> iTest = listPrintNoSave;

            if (listPrintNoSave.Count != 0)
            {
                //Распечатаем документ Word
                //string filName = System.Windows.Forms.Application.StartupPath + @"\Шаблон\Письмо Возврат.doc";
                //string filName = @"D:\Проекты\ControlDantist\ControlDantist\bin\Debug\Шаблон\Письмо  Возврат.doc";
                string filName = System.Windows.Forms.Application.StartupPath + @"\Шаблон\Письмо  Возврат.doc";


                //Создаём новый Word.Application
                Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();

                //Загружаем документ
                Microsoft.Office.Interop.Word.Document doc = null;

                object fileName = filName;
                object falseValue = false;
                object trueValue = true;
                object missing = Type.Missing;

                doc = app.Documents.Open(ref fileName, ref missing, ref trueValue,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing);

                //Горизонтальное ориентирование листа
                //doc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;


                object bookNaziv = "таблица";
                Range wrdRng = doc.Bookmarks.get_Item(ref  bookNaziv).Range;

                object behavior = Microsoft.Office.Interop.Word.WdDefaultTableBehavior.wdWord8TableBehavior;
                object autobehavior = Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitWindow;

                Microsoft.Office.Interop.Word.Table table = doc.Tables.Add(wrdRng, 1, 4, ref behavior, ref autobehavior);
                table.Range.ParagraphFormat.SpaceAfter = 6;
                table.Columns[1].Width = 40;
                table.Columns[2].Width = 80;
                table.Columns[3].Width = 250;
                table.Columns[4].Width = 150;
                table.Borders.Enable = 1; // Рамка - сплошная линия
                table.Range.Font.Name = "Times New Roman";
                table.Range.Font.Size = 10;
                //счётчик строк
                int i = 1;

                //Список для хранения данных для таблицы
                List<ReestrNoPrintDog> listItem = new List<ReestrNoPrintDog>();

                //Сформируем шапку таблицы
                ReestrNoPrintDog шапка = new ReestrNoPrintDog();
                шапка.Number = "№ п/п";
                шапка.NumDog = "№ договора";
                шапка.Fio = "ФИО";
                шапка.Примечание = "Примечание";

                listItem.Add(шапка);

                //Счётчик
                int iCount = 1;

                //Заполним данными таблицу
                foreach (Unload un in listPrintNoSave.Values)
                {
                    //Строка таблицы
                    ReestrNoPrintDog item = new ReestrNoPrintDog();
                    item.Number = iCount.ToString();

                    //Получим ИНН поликлинники
                    инн = un.Поликлинника.Rows[0]["ИНН"].ToString().Trim();

                    DataRow rowDog = un.Договор.Rows[0];
                    item.NumDog = rowDog["НомерДоговора"].ToString().Trim();

                    //Получим ФИО льготника
                    DataRow rowЛьготник = un.Льготник.Rows[0];

                    string фамилия = rowЛьготник["Фамилия"].ToString().Trim();
                    string имя = rowЛьготник["Имя"].ToString().Trim();
                    string отчество = rowЛьготник["Отчество"].ToString().Trim();

                    //
                    string фио = фамилия + " " + имя + " " + отчество;
                    item.Fio = фио.Trim();

                    item.Примечание = "";

                    listItem.Add(item);

                    iCount++;
                }


                //Заполним таблицу
                int k = 1;
                //запишем данные в таблицу
                foreach (ReestrNoPrintDog item in listItem)
                {

                    table.Cell(k, 1).Range.Text = item.Number;

                    table.Cell(k, 2).Range.Text = item.NumDog;
                    table.Cell(k, 3).Range.Text = item.Fio;
                    
                    //if (k == 1)
                    //{

                    //}

                    //if (k > 1)
                    //{
                    //    table.Cell(k, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    //}
                    
                    table.Cell(k, 4).Range.Text = item.Примечание.Trim();

                    //doc.Words.Count.ToString();
                    Object beforeRow1 = Type.Missing;
                    table.Rows.Add(ref beforeRow1);

                    k++;
                }

                table.Rows[k].Delete();

                //Загрузим адрес с даннми о поликлиннике
                using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
                {
                    con.Open();

                    SqlTransaction transact = con.BeginTransaction();

                    //получим id поликлинники
                    string queryINN = "SELECT [F2],[F3],ФИО FROM [Лист1$] where id in (select id_поликлинника from dbo.Поликлинника where ИНН = " + инн + ") ";
                    DataRow row = ТаблицаБД.GetTableSQL(queryINN, "Лист1$", con, transact).Rows[0];

                    //получим адрес
                    адрес = row["F2"].ToString().Trim();

                    //получим обращение
                    обращение = row["F3"].ToString().Trim();

                    //получим фио
                    fio = row["ФИО"].ToString().Trim();

                }

                //Заполним документ WORD данными
                ////Номер договора

                object wdrepl = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt = "адресат";
                object newtxt = (object)адрес;
                //object frwd = true;
                object frwd = false;
                doc.Content.Find.Execute(ref searchtxt, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd, ref missing, ref missing, ref newtxt, ref wdrepl, ref missing, ref missing,
                ref missing, ref missing);

                //Запишем кому
                object wdrepl1 = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt1 = "кому";
                object newtxt1 = (object)обращение;
                //object frwd = true;
                object frwd1 = false;
                doc.Content.Find.Execute(ref searchtxt1, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd1, ref missing, ref missing, ref newtxt1, ref wdrepl1, ref missing, ref missing,
                ref missing, ref missing);

                //запишем фио
                object wdrepl2 = WdReplace.wdReplaceAll;
                //object searchtxt = "GreetingLine";
                object searchtxt2 = "fio";
                object newtxt2 = (object)fio;
                //object frwd = true;
                object frwd2 = false;
                doc.Content.Find.Execute(ref searchtxt2, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd2, ref missing, ref missing, ref newtxt2, ref wdrepl2, ref missing, ref missing,
                ref missing, ref missing);

                //Отобразим документ
                app.Visible = true;
            }
            else
            {
                MessageBox.Show("Не подтверждённых договоров нет");
            }

        }

        //
        private void btnExel_Click(object sender, EventArgs e)
        {
            ListLoad();
            Dictionary<string, Unload> iTest = listPrintSave;

            //Список данных льготников
            List<RowExcel> list = new List<RowExcel>();

            if (listPrintSave.Count != 0)
            {
                using(SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
                {
                    con.Open();
                    SqlTransaction transact = con.BeginTransaction();

                    //пройдемся по льготникам которые прошли проверку
                    foreach (Unload un in listPrintSave.Values)//this.ПроектыДоговоров
                    {
                        //Получим ФИО льготника
                        DataRow rowЛьготник = un.Льготник.Rows[0];
                        string фамилия = rowЛьготник["Фамилия"].ToString().Trim();

                        string имя = rowЛьготник["Имя"].ToString().Trim();
                        string отчество = rowЛьготник["Отчество"].ToString().Trim();

                        string queryFIO = "SELECT [Договор] " +
                                          ",[Фамилия] " +
                                          ",[Имя] " +
                                          ",[Отчество] " +
                                          "FROM [ТаблицаExcel] " +
                                          "where Фамилия like '%" + фамилия + "%' and Имя like '%" + имя + "%' and Отчество like '%" + отчество + "%' ";

                        //Получим таблицу содержащую
                        DataTable tabЛьготник = ТаблицаБД.GetTableSQL(queryFIO, "Excel", con, transact);

                        //Запишем льготников в список
                        foreach (DataRow row in tabЛьготник.Rows)
                        {
                            RowExcel exe = new RowExcel();
                            //exe.Num = row["НомерДоговора"].ToString().Trim();
                            exe.Num = row[0].ToString().Trim();

                            exe.Фамилия = row["Фамилия"].ToString().Trim();
                            exe.Имя = row["Имя"].ToString().Trim();

                            exe.Отчество = row["Отчество"].ToString().Trim();
                            list.Add(exe);
                        }
                    }
                }
            }

            //если список с совпдениям льготников пустой
            if (list.Count != 0)
            {
                //Передадим в форму список 
                FormExcel fexe = new FormExcel();
                fexe.TopMost = true;

                //Передадим в форму данные
                fexe.ListExcel = list;
                fexe.Show();
            }
            else
            {
                MessageBox.Show("Совпадений не найдено");
            }


        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{

            //    this.dataGridView1.ClearSelection();
            //    this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            //    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

            //    DataGridViewRow rows = this.dataGridView1.Rows[e.RowIndex];

            //    //получим номер договора
            //    //this.ПроектыДоговоров;
            //    string numDog = rows.Cells[0].Value.ToString().Trim();

            //    Unload vr = this.ВыгрузкаПроектДоговоров[numDog];

            //    //отобразим форму
            //    FormInfoЛьготник info = new FormInfoЛьготник();

            //    //передадим данные в форму
            //    info.Unloads = vr;
                
            //    //отобразим форму
            //    info.Show();

            // }
        }

        private void информацияОЛьготникеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridView1.ClearSelection();
            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

            DataGridViewRow rows = this.dataGridView1.Rows[e.RowIndex];

            //получим номер договора
            //this.ПроектыДоговоров;
            string numDog = rows.Cells[0].Value.ToString().Trim();

            Unload vr = this.ВыгрузкаПроектДоговоров[numDog];

            //отобразим форму
            FormInfoЛьготник info = new FormInfoЛьготник();

            //передадим данные в форму
            info.Unloads = vr;

            //отобразим форму
            info.Show();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            //Обнулим списки
            this.listPrintSave.Clear();
            this.listPrintNoSave.Clear();
            this.listPrintAddCheck.Clear();

            //Хранит инн поликлинники
            string инн = string.Empty;

            //хранит адресат
            string адрес = string.Empty;

            //хранит обращение (Уважаемый Андрей Юрьевич)
            string обращение = string.Empty;

            //хранит инициалы
            string fio = string.Empty;


            ListLoad();

            if (this.listPrintAddCheck.Count != 0)
            {

            }
            else
            {
                MessageBox.Show("Нет данных для письма");
            }

            //Dictionary<string, Unload> lTest = this.listPrintAddCheck;

            //string iTest = "Test";
        }

        private void btnSum_Click(object sender, EventArgs e)
        {
            //переменная для хранения суммы договоров прошедших проверку
            decimal sumПрошедшие = 0.0m;

            //переменная для хранения суммы не прошедших проверку
            decimal sumНеПрошед = 0.0m;

            foreach(DataGridViewRow row in this.dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells["СохранитьДоговор"].Value) == true)
                {
                   string s =  row.Cells["SumService"].Value.ToString();
                   string sum = s.Replace("р.", " ");

                   decimal sumTest = decimal.Parse(sum.Trim());
                   sumПрошедшие = Math.Round(Math.Round(sumПрошедшие, 2) + sumTest);
                }
                else
                {
                    string s = row.Cells["SumService"].Value.ToString();
                    string sum = s.Replace("р.", " ");

                    decimal sumTest = decimal.Parse(sum.Trim());
                    sumНеПрошед = Math.Round(Math.Round(sumНеПрошед, 2) + sumTest);
                }
            }

            FormSum fSum = new FormSum();
            fSum.SumПрошедших = sumПрошедшие;
            
            fSum.SumНеПрошедших = sumНеПрошед;
            fSum.ShowDialog();


        }

        private void проверкаУслугToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Проставим везде галочки
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                row.Cells["ПроверкаУслуг"].Value = false;
            }
        }

        private void проверкаЭСРНToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Проставим везде галочки
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                row.Cells["ПроверкаЭСРН"].Value = false;
            }
        }

        private void сохранитьДоговорToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Проставим везде галочки
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                row.Cells["СохранитьДоговор"].Value = false;
            }
        }

        private void btnLK_Click(object sender, EventArgs e)
        {

            // Данные для статистики.
            listPrintStatistic = new Dictionary<string, Unload>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {

                //ПРОШЕДШИЕ ПРОВЕРКУ
                if (Convert.ToBoolean(row.Cells["СохранитьДоговор"].Value) == true && Convert.ToBoolean(row.Cells["ПроверкаЭСРН"].Value) == true)
                {
                    string numberDog = row.Cells["НомерДоговора"].Value.ToString().Trim();
                    Unload unloadSave = ВыгрузкаПроектДоговоров[numberDog];

                    //Запишем в словарь
                    //this.listPrintSave.Add(numberDog, unloadSave);
                    this.listPrintStatistic.Add(numberDog, unloadSave);
                }
            }

            // Список для хранения проектов договоров.
            List<СтатистикаПректовДоговоров> listStat = new List<СтатистикаПректовДоговоров>();

            //Dictionary<string, Unload> list = this.listPrintStatistic;

            //foreach (Unload item in list.Values)
            foreach (Unload item in listPrintStatistic.Values)
            {
                СтатистикаПректовДоговоров it = new СтатистикаПректовДоговоров();
                it.ЛьготнаяКатегория = item.ЛьготнаяКатегория.Trim();

                it.НомерДоговора = item.Договор.Rows[0]["НомерДоговора"].ToString().Trim();

                it.Фамилия = item.Льготник.Rows[0]["Фамилия"].ToString().Trim();
                it.Имя = item.Льготник.Rows[0]["Имя"].ToString().Trim();
                it.Отчество = item.Льготник.Rows[0]["Отчество"].ToString().Trim();

                decimal sum = 0.0m;

                foreach(DataRow rSum in item.УслугиПоДоговору.Rows)
                {
                    sum += Convert.ToDecimal(rSum["Сумма"]);
                }

                it.СтоимостьУслугПоДоговору = sum;

                listStat.Add(it);
            }

            // Выведим на печать.
            WordLetterStatistic word = new WordLetterStatistic(listStat);
            word.PrintDoc();


            var asd = "asd";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                // Удалить после отладки.
                row.Cells["ПроверкаУслуг"].Value = true;
                row.Cells["ПроверкаЭСРН"].Value = true;
                row.Cells["СохранитьДоговор"].Value = true;
            }
        }

       
                
    }
}