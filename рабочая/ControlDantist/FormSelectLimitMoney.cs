using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlDantist.BalanceContract;
using ControlDantist.Repository;

namespace ControlDantist
{
    public partial class FormSelectLimitMoney : Form
    {
        private bool flagSum = false;

        /// <summary>
        /// Запрещает редактирование 
        /// </summary>
        public bool FlagEditLimit { get; set; }

        public string Money { get; set; }

        /// <summary>
        /// Ключи льготных категорий.
        /// </summary>
        public string Keys { get; set; }

        // id выплаты.
        public int IdMoney { get; set; }

        /// <summary>
        /// Текущий год.
        /// </summary>
        public int Year { get; set; }

        public FormSelectLimitMoney()
        {
            InitializeComponent();
        }

        private void FormSelectLimitMoney_Load(object sender, EventArgs e)
        {
            // Доступ к источнику данных.
            UnitDate unitDate = new UnitDate();

            // Список содержащий вспомогательные классы.
            List<SelectPreferency> listSP = new List<SelectPreferency>();

            int iCount = 0;

            foreach(var item in unitDate.ЛьготнаяКатегорияRepository.SelectAll())
            {
                SelectPreferency sp = new SelectPreferency();
                sp.IdLK = item.id_льготнойКатегории;
                sp.NameLK = item.ЛьготнаяКатегория1.Trim();

                sp.Flag = false;

                if(iCount > 0)
                listSP.Add(sp);

                iCount++;
            }

            if (this.FlagEditLimit == true)
            {
                string[] keys = this.Keys.Split(' ');

                foreach(var key in keys)
                {
                    if (key.Length > 0)
                    {
                        var itemLK = listSP.Where(w => w.IdLK == Convert.ToInt16(key)).FirstOrDefault();

                        if (itemLK != null)
                        {
                            itemLK.Flag = true;
                        }
                    }
                }
                
            }

                this.dataGridView1.DataSource = listSP;
            
            // Отобразим лимит по выбранным категориям.
            this.textBox1.Text = this.Money;

            DisplayLK();
        }

        private void DisplayLK()
        {
            this.dataGridView1.Columns["IdLK"].Visible = false;
            this.dataGridView1.Columns["NameLK"].Width = 400;
            this.dataGridView1.Columns["NameLK"].HeaderText = "Льготная категория";
            this.dataGridView1.Columns["Flag"].Width = 80;
            this.dataGridView1.Columns["Flag"].HeaderText = "Выбрать";
            this.dataGridView1.Columns["IdLimitedMoney"].Visible = false;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.flagSum = true;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
                // Проверим отмечены ли льготные категории и введен ли лимит денежных средств для этих категори.
                flagSum = false;

                // Список для хранения выбранных id льготных категорий.
                List<int> listId = new List<int>();

                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[2].Value) == true)
                    {
                        // Добавим выбранный ID.
                        listId.Add(Convert.ToInt16(row.Cells[0].Value));

                        flagSum = true;
                    }
                }

                // Доступ к источнику данных.
                UnitDate unitDate = new UnitDate();

            if (this.FlagEditLimit == false)
            {

                // Проверим заполненность формы.
                if (string.IsNullOrEmpty(this.textBox1.Text) == false && flagSum == true)
                {
                    try
                    {
                        // Выполним в единой транзакции добавление лимитов.
                        // Установим уровни изоляции транзакций.
                        var option = new System.Transactions.TransactionOptions();
                        option.IsolationLevel = System.Transactions.IsolationLevel.Serializable;

                        // Добавим льготника и адрес в БД.
                        // Внесём данные в таблицу в единой транзакции.
                        using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, option))
                        {
                            // Запишем лимит для указанных льготных категорий.
                            LimittMoney limittMoney = new LimittMoney();
                            limittMoney.Limit = Convert.ToDecimal(this.textBox1.Text);

                            Year year = new Year();
                            year.Year1 = this.Year;

                            var yarCurrent = unitDate.YearRepository.Select(year);

                            if (yarCurrent != null)
                            {
                                limittMoney.idYear = yarCurrent.intYear;
                            }
                            else
                            {
                                throw new Exception();
                            }

                            unitDate.LimitMoneyRepository.Insert(limittMoney);

                            foreach (int id in listId)
                            {
                                // Запишем данные в указанные льготные категории.
                                LimitPreferenceCategory limitPreferenceCategory = new LimitPreferenceCategory();
                                limitPreferenceCategory.idLimitMoney = limittMoney.idLimitMoney;
                                limitPreferenceCategory.id_льготнойКатегории = id;

                                unitDate.LimitPreferenceCategoryRepository.Insert(limitPreferenceCategory);
                            }

                            // Завершим транзакцию.
                            scope.Complete();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Лимит средств в БД не записан, произошла ошибка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("Не выбрана льготная категория или не внесена сумма лимита денежных средств", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Обновим данные.
                // Выполним в единой транзакции добавление лимитов.
                // Установим уровни изоляции транзакций.
                var option = new System.Transactions.TransactionOptions();
                option.IsolationLevel = System.Transactions.IsolationLevel.Serializable;

                // Добавим льготника и адрес в БД.
                // Внесём данные в таблицу в единой транзакции.
                using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, option))
                {

                    try
                    {
                        // Найдем запись обозначающую лимит.
                        LimittMoney limittMoney = unitDate.LimitMoneyRepository.Select(this.IdMoney).FirstOrDefault();

                        if (limittMoney != null)
                        {
                            limittMoney.Limit = Convert.ToDecimal(this.textBox1.Text);
                        }

                        // Обновим лимит.
                        unitDate.LimitMoneyRepository.Update(limittMoney);

                        // Найдем id льготных категорий.
                        var listCategory = unitDate.LimitPreferenceCategoryRepository.Select(limittMoney.idLimitMoney);

                        if (listCategory != null)
                        {
                            unitDate.LimitPreferenceCategoryRepository.Delete(limittMoney.idLimitMoney);
                        }

                        foreach (int id in listId)
                        {
                            // Запишем данные в указанные льготные категории.
                            LimitPreferenceCategory limitPreferenceCategory = new LimitPreferenceCategory();
                            limitPreferenceCategory.idLimitMoney = limittMoney.idLimitMoney;
                            limitPreferenceCategory.id_льготнойКатегории = id;

                            unitDate.LimitPreferenceCategoryRepository.Insert(limitPreferenceCategory);
                        }

                        // Завершим транзакцию.
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                }
            }

                this.Close();
        }

        private void FormSelectLimitMoney_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flagSum == false)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 44) // цифры, клавиша BackSpace и запятая
            {
                e.Handled = true;
            }
        }
    }
}
