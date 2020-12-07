using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExcelLibrary;
using ExcelLibrary.SpreadSheet;
using System.Text.RegularExpressions;
using ControlDantist.Classes;
using DantistLibrary;
using ClassLibrary1;


namespace ControlDantist
{
    public partial class FormWriteBD : Form
    {
        // Флаг указывает что в файле нет записи.
        private bool flagNull = false;

        /// <summary>
        /// Хранит данные.
        /// </summary>
        public ClassЗаписьБД ЗаписьБД { get; set; }

        public FormWriteBD()
        {
            InitializeComponent();
            lblРезультатПоиска.ForeColor = Color.Green;
        }

        private void btnFindFile_Click(object sender, EventArgs e)
        {
            // Обнулим флаг.
            flagNull = false;

            // Обнулим все поля.
            this.txtФамилия.Text = "";
            txtИмя.Text = "";
            txtОтчество.Text = "";
            txtСтоимость.Text = "";

            // Установим цвет зелёный.
            lblРезультатПоиска.ForeColor = Color.Green;


            this.lblРезультатПоиска.Text = "";

            // Откроем файл Excel.
            if (this.txtНомерДоговора.Text != "")
            {
                string fileName = Application.StartupPath + "\\Стоматполиклиника № 1_1.xls";

                Workbook book = Workbook.Load(fileName);
                Worksheet sheet = book.Worksheets[0];


                for (int i = 1; i <= 1476; i++)
                {


                    Cell cellЦелеваяСтатьяСаратова = sheet.Cells[i, 2];
                    string район = cellЦелеваяСтатьяСаратова.StringValue;

                    string pattern = "\\s+";
                    string replacement = " ";
                    Regex rgx = new Regex(pattern);
                    string result = rgx.Replace(район, replacement);

                    // Получим массив данных.
                    string[] array = result.Split(' ');

                    // Узнаем сколько количество элементов в массиве.
                    int iCountArray = array.Count();

                    // Переменная для хранения номер договора.
                    string numContract = string.Empty;

                    // Если  запись в ячейке в виде № 1/22 01.01.2013.
                    if (iCountArray == 3)
                    {
                       // Получим номер договора.
                        numContract = array[1].Trim();
                    }

                    // Если  запись в ячейке в виде 1/22 01.01.2013.
                    if (iCountArray == 2)
                    {
                        // Получим номер договора.
                        numContract = array[0].Trim();
                    }

                    // Если  запись в ячейке в виде 1/22.
                    if (iCountArray == 1)
                    {
                        // Получим номер договора.
                        numContract = array[0].Trim();
                    }

                    if (numContract == this.txtНомерДоговора.Text.Trim())
                    {
                        this.lblРезультатПоиска.Text = "Договор в файле НАЙДЕН!!!";

                        // Установим флаг в положение запись найдена.
                        flagNull = true;

                        // Получим Фио льготника
                        ClassЗаписьБДЛьготник льготник = new ClassЗаписьБДЛьготник();

                        Cell cellФамилия = sheet.Cells[i, 3];
                        льготник.Фамилия = cellФамилия.StringValue.Trim();

                        Cell cellИмя = sheet.Cells[i, 4];
                        льготник.Имя = cellИмя.StringValue.Trim();

                        Cell cellОтчество = sheet.Cells[i, 5];
                        льготник.Отчество = cellОтчество.StringValue.Trim();

                        ClassЗаписьБДДоговор договор = new ClassЗаписьБДДоговор();

                        try
                        {
                            // Получим Сумму акта выполненных работ.
                            Cell cellСумма = sheet.Cells[i, 12];
                            договор.СуммаАкткВыполненныхРабот = Convert.ToDecimal(cellСумма.StringValue);
                        }
                        catch
                        {
                            договор.СуммаАкткВыполненныхРабот = 0.0m;
                        }

                        ClassЗаписьБД записьБД = new ClassЗаписьБД();
                        //записьБД.Договор = договор;
                        записьБД.Льготник = льготник;

                        // Запишем данные в свойство формы.
                        this.ЗаписьБД = записьБД;

                        // Отобразим ФИО льготника.
                        this.txtФамилия.Text = льготник.Фамилия.Trim();
                        txtИмя.Text = льготник.Имя.Trim();
                        txtОтчество.Text = льготник.Отчество.Trim();
                        txtСтоимость.Text = договор.СуммаАкткВыполненныхРабот.ToString();


                        break;

                    }
                }

                
            }
            else
            {
                MessageBox.Show("Заполните номер договора");
            }

            if(flagNull == false)
            {
                this.lblРезультатПоиска.Text = "Поиск результатов не дал";
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Установим цвет lbl зелёным.
            lblРезультатПоиска.ForeColor = Color.Green;

            ClassLibrary1.DataClasses1DataContext dc = new ClassLibrary1.DataClasses1DataContext();

            // Найдём договор в БД.
            var contracts = dc.Договор.Where(cont => cont.НомерДоговора == txtНомерДоговора.Text.Trim() && cont.ФлагПроверки == true).Select(cont => cont);

            if (contracts.Count() == 1)
            {
                lblРезультатПоиска.Text = "Договор в базе данных НАЙДЕН!!!";

                int id_льготник = 0;
                // Получим id_льготник.

                foreach (var item in contracts)
                {
                    id_льготник = Convert.ToInt32(item.id_льготник);
                }

                var query = dc.Льготник.Where(w => w.id_льготник == id_льготник);
                

            }
            if(contracts.Count() > 1)
            {
                lblРезультатПоиска.Text = "Найдено 2 и более договоров";
                lblРезультатПоиска.ForeColor = Color.Red;
            }

            if (contracts.Count() == 0)
            {
                lblРезультатПоиска.Text = "Поиск результатов не дал";
            }
        }

        private void FormWriteBD_Load(object sender, EventArgs e)
        {
            // Заполним combo box льготными категориями.

           ClassLibrary1.DataClasses1DataContext dc = new ClassLibrary1.DataClasses1DataContext();
           IEnumerable<string> query = dc.ЛьготнаяКатегория.Select(l => l.ЛьготнаяКатегория1.Trim());

           this.cmbЛьготнаяКатегория.DataSource = query;
           //this.cmbЛьготнаяКатегория.Text = dc.ЛьготнаяКатегория.Select(

           IEnumerable<string> queryDoc = dc.ТипДокумента.Select(d => d.НаименованиеТипаДокумента.Trim());
           this.cmbДокумент.DataSource = queryDoc;

           var hosp = dc.Поликлинника.Select(h => h.НаименованиеПоликлинники);
           this.comboBox1.DataSource = hosp;
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            ClassLibrary1.DataClasses1DataContext dc = new ClassLibrary1.DataClasses1DataContext();
            
            // Запишем Льготника.
            ClassLibrary1.Льготник person = new ClassLibrary1.Льготник();

            // Запишем ФИО.
            person.Фамилия = txtФамилия.Text.Trim();
            person.Имя = txtИмя.Text.Trim();
            person.Отчество = txtОтчество.Text.Trim();

            // Дата рождения.
            person.ДатаРождения = Convert.ToDateTime(dtДатаРождения.Text).Date;

            // id льготной категории.
            person.id_льготнойКатегории = dc.ЛьготнаяКатегория.Where(l => l.ЛьготнаяКатегория1 == cmbЛьготнаяКатегория.Text.Trim()).Select(l => l.id_льготнойКатегории).First();

            // Записшем адрес льготника.
            person.улица = txtУлица.Text.Trim();
            person.НомерДома = txtДом.Text.Trim();
            person.корпус = txtКорпус.Text.Trim();

            person.НомерКвартиры = txtКвартира.Text.Trim();

            // Запишем паспортные данные.
            person.СерияПаспорта = txtСерияПаспорта.Text.Trim();
            person.НомерПаспорта = txtНомерПаспорта.Text.Trim();
            person.ДатаВыдачиПаспорта = Convert.ToDateTime(dtДатаВыдачиПаспорта.Text).Date;
            person.КемВыданПаспорт = txtКемВыданПаспорт.Text.Trim();

            // Запишем тип документа.
            person.id_документ = dc.ТипДокумента.Where(doc => doc.НаименованиеТипаДокумента == cmbДокумент.Text.Trim()).Select(doc => doc.id_документ).First();

            // Запишем серию документа.
            person.СерияДокумента = textBox3.Text.Trim();

            // Запишем номер документа.
            person.НомерДокумента = textBox5.Text.Trim();

            // Запишем дату выдачи документа.
            person.ДатаВыдачиДокумента = Convert.ToDateTime(dateTimePicker5.Text).Date;

            // Кем выдан документ.
            person.КемВыданДокумент = textBox1.Text.Trim();

            dc.Льготник.InsertOnSubmit(person);
            dc.SubmitChanges();

            // Узнаем id Льготника.
            int idЛьготник = person.id_льготник;


            // Получим данные о договоре.
            //ClassLibrary1.ClassЗаписьБДДоговор contract = new ClassLibrary1.ClassЗаписьБДДоговор();
            ClassLibrary1.Договор contract = new ClassLibrary1.Договор();

            // Получим номер реестра.
            contract.НомерРеестра = txtНомерРеестра.Text.Trim();

            // Дату реестра.
            contract.ДатаРеестра = Convert.ToDateTime(dtДатаРеестра.Text).Date;

            // Получим id льготной категории.
            
            int idЛьготКатегория = dc.ЛьготнаяКатегория.Where(id => id.ЛьготнаяКатегория1 == this.cmbЛьготнаяКатегория.Text.Trim()).Select(id => id.id_льготнойКатегории).First();

            // Запишем id льготной категории.
            contract.id_льготнаяКатегория = idЛьготКатегория;

            // Запишем Номер договора.
            contract.НомерДоговора = txtНомерДоговора.Text.Trim();

            // Запишем дату договора.
            contract.ДатаДоговора = Convert.ToDateTime(dateДоговора.Text).Date;

            // Запишем дату акта выполненных работ.
            contract.ДатаАктаВыполненныхРабот = Convert.ToDateTime(dateTimePicker1.Text);

            // Запишем сумму акта выполненных работ.
            contract.СуммаАктаВыполненныхРабот = Convert.ToDecimal(txtСтоимость.Text);

            contract.ФлагНаличияДоговора = true;

            // Id поликлинники (id первой поликлинники = 92).
            //contract.id_поликлинника = 92;


            int idHosp = dc.Поликлинника.Where(h => h.НаименованиеПоликлинники == comboBox1.Text).Select(h => h.id_поликлинника).First();
            contract.id_поликлинника = idHosp;

            // Запишем что акт существует.
            contract.ФлагНаличияАкта = true;

            // Номер счёт фактуры.
            contract.НомерСчётФактрура = textBox6.Text.Trim();

            // Запишем дату счёт фактуры.
            contract.ДатаСчётФактура = Convert.ToDateTime(dateTimePicker3.Text).Date;

            // Запишем id льготника.
            contract.id_льготник = idЛьготник;

            // Так как у нас в БД id комитета = 1.
            contract.id_комитет = 1;

            // Запишем лог что это заливка.
            contract.logWrite = "Заливка базы 2013";

            contract.ДатаЗаписиДоговора = DateTime.Now.Date;

            // Флаг проверки.
            contract.ФлагПроверки = true;

            // Совершим глупость запишем всё в разных транзакциях.
            //dc.Договор.InsertOnSubmit(contract);
            dc.Договор.InsertOnSubmit(contract);
            dc.SubmitChanges();

            // Узнаем id договора.
            int id_договорWrit = contract.id_договор;



            // Запишем акт выполненных работ.
            ClassLibrary1.АктВыполненныхРабот act = new ClassLibrary1.АктВыполненныхРабот();

            // Номер акта.
            act.НомерАкта = txtНомерДоговора.Text.Trim() + "/" + textBox2.Text.Trim();

            // Запишем id договора.
            act.id_договор = id_договорWrit;

            // Установим флаг в подписи акта.
            act.ФлагПодписания = "True";

            // Дата подписания акта.
            act.ДатаПодписания = Convert.ToDateTime(dateTimePicker1.Text).Date;

            // Номер реестра.
            act.НомерРеестра = txtНомерРеестра.Text.Trim();

            // Дата реестра.
            act.ДатаРеестра = Convert.ToDateTime(dtДатаРеестра.Text).Date;

            // Номер счёт фактуры.
            act.НомерСчётФактуры = textBox6.Text.Trim();

            // Дата счёт фактуры.
            act.ДатаСчётФактуры = Convert.ToDateTime(dateTimePicker3.Text).Date;

            // Запишем лог.
            act.logWrite = "Заливка базы 2013";

            // Запишем текущую дату.
            DateTime dt = DateTime.Now;
            act.logDate = dt.Date.ToShortDateString();

            dc.АктВыполненныхРабот.InsertOnSubmit(act);
            dc.SubmitChanges();


            // Запишем стоимость услуг по текущему договору.
            ClassLibrary1.УслугиПоДоговору услуги = new ClassLibrary1.УслугиПоДоговору();
            услуги.id_договор = id_договорWrit;
            услуги.НаименованиеУслуги = "Стоимость услуг заливка базы 2013";
            услуги.Сумма = Convert.ToDecimal(this.txtСтоимость.Text);

            dc.УслугиПоДоговору.InsertOnSubmit(услуги);
            dc.SubmitChanges();

            // Обнулим все поля ввода.
            txtНомерРеестра.Text = "";
            txtФамилия.Text = "";
            txtИмя.Text = "";
            txtОтчество.Text = "";
            txtУлица.Text = "";
            txtДом.Text = "";
            txtКорпус.Text = "";
            txtКвартира.Text = "";
            txtСерияПаспорта.Text = "";
            txtНомерПаспорта.Text = "";
            txtКемВыданПаспорт.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
            textBox1.Text = "";
            txtНомерДоговора.Text = "";
            textBox2.Text = "";
            txtСтоимость.Text = "";
            textBox6.Text = "";
            dtДатаРеестра.Text = "";
            dtДатаРождения.Text = "";
            dtДатаВыдачиПаспорта.Text = "";
            dateTimePicker5.Text = "";
            dateДоговора.Text = "";
            dateTimePicker1.Text = "";
            dateTimePicker3.Text = "";
            

        }
    }
}
