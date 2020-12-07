using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Classes;

namespace ControlDantist
{
    public partial class FormEditDateHospital : Form
    {
        // Хранит источник данных.
        private DataSetHospital dt;

        // Хранит строку подключения к БД.
        private string sConnect = string.Empty;

        // Хранит id поликлинники.
        private int id_поликлинники = 0;

        // Хранит id вида услуг.
        private int id_ВидУслуг = 0;

        // Флаг указывает что форма находиться в режиме записи втавки (false) или в режиме редактирования записи (true).
        private bool flagEdit = false;

        public FormEditDateHospital()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор окна. Наверное не лучшее решение.
        /// </summary>
        /// <param name="dsh"></param>
        /// <param name="sConnection"></param>
        public FormEditDateHospital(DataSetHospital dsh, string sConnection):this()
        {
            if (dsh != null)
            {
                dt = dsh;
                if (dt.IdHospital != null)
                {
                    if (dt.ТаблицаКлассификаторУслуг.Count > 0)
                    {
                        if (dt.ТаблицаВидУслуг.Count > 0)
                        {
                            // Передадим источник данных.
                            dt = dsh;

                            // Передадим строку подключения.
                            sConnect = sConnection;
                        }
                        else
                        {
                            throw new ExceptionUser("Нет данных в таблице Вид услуг");
                        }
                    }
                    else
                    {
                        throw new ExceptionUser("Нет данных в таблице Классификатор услуг");
                    }

                }
                else
                {
                    throw new ExceptionUser("Отсутствует id поликлиники");
                }
            }
        }

        private void FormEditDateHospital_Load(object sender, EventArgs e)
        {
            // Присвоим данные id поликлинники.
            id_поликлинники = dt.IdHospital;

            // Отобразим данные в классификаторе услуг.
            this.comboBox1.DataSource = dt.ТаблицаКлассификаторУслуг;
            this.comboBox1.DisplayMember = "КлассификаторУслуг1";
            this.comboBox1.ValueMember = "Id_КодУслуг";

            int id = dt.ТаблицаКлассификаторУслуг.Where(w => w.КлассификаторУслуг1.Trim() == this.comboBox1.Text.Trim()).Select(w => w).First().Id_КодУслуг;

            // Загрузим данные в DataGridView.
            DataLoad(id);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DataLoad(int id)
        {
            DataSetClass ds = new DataSetClass(sConnect);
            DataSetHospital dsh = ds.GetDataHospital();

            // Обнулим источник DataGridView.
            this.dataGridView1.DataSource = null;
            // Отобразим вид услуг.
            //this.dataGridView1.DataSource = dt.ТаблицаВидУслуг.Where(w=>w.Id_КодУслуги == id).Select(w=>w).ToList();
            this.dataGridView1.DataSource = dsh.ТаблицаВидУслуг.Where(w => w.Id_КодУслуги == id).Select(w => w).ToList();
            this.dataGridView1.Columns["ВидУслуги1"].Visible = true;
            this.dataGridView1.Columns["ВидУслуги1"].Width = 360;
            this.dataGridView1.Columns["ВидУслуги1"].HeaderText = "Вид услуг";
            this.dataGridView1.Columns["Id_Поликлинники"].Visible = false;
            this.dataGridView1.Columns["Выбрать"].Visible = false;
            this.dataGridView1.Columns["Id_КодУслуги"].Visible = false;
            this.dataGridView1.Columns["ТехЛист"].Visible = false;
            this.dataGridView1.Columns["Цена"].Visible = true;
            this.dataGridView1.Columns["Цена"].Width = 60;
            this.dataGridView1.Columns["НомерПоПеречню"].Visible = true;
            this.dataGridView1.Columns["НомерПоПеречню"].Width = 130;
            this.dataGridView1.Columns["Id_ВидУслуг"].Visible = false;

            this.dataGridView1.Columns["НомерПоПеречню"].DisplayIndex = 0;
            this.dataGridView1.Columns["ВидУслуги1"].DisplayIndex = 1;
            this.dataGridView1.Columns["Цена"].DisplayIndex = 2;
            
            // Обнулим поля редактирования.
            this.textBox1.Text = string.Empty;
            this.textBox2.Text = string.Empty;
            this.textBox3.Text = string.Empty;

        }

        

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            DataSetClass ds = new DataSetClass(sConnect);
            DataSetHospital dsh = ds.GetDataHospital();

            var listValid = dsh.ТаблицаКлассификаторУслуг.Where(w => w.КлассификаторУслуг1.Trim() == this.comboBox1.Text.Trim()).Select(w => w).ToList();

            if (listValid.Count > 0)
            {
                id_ВидУслуг = dsh.ТаблицаКлассификаторУслуг.Where(w => w.КлассификаторУслуг1.Trim() == this.comboBox1.Text.Trim()).Select(w => w).First().Id_КодУслуг;

                // Загрузим данные в DataGridView.
                DataLoad(id_ВидУслуг);
            }
            else if (listValid.Count == 0)
            {
                id_ВидУслуг = 0;

                // Загрузим данные в DataGridView.
                DataLoad(id_ВидУслуг);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataSetClass ds = new DataSetClass(sConnect);
            DataSetHospital dsh = ds.GetDataHospital();

            // Переведём форму в режим редактирования.
            flagEdit = true;
            this.btnSave.Text = "Сохранитить изменения";

            // Получим id записи.
            id_ВидУслуг = (int)this.dataGridView1.CurrentRow.Cells["Id_ВидУслуг"].Value;

            if (id_ВидУслуг != null)
            {
                //var curRow = dt.ТаблицаВидУслуг.Where(w => w.Id_ВидУслуг == id_ВидУслуг).Select(w => w).First();
                var curRow = dsh.ТаблицаВидУслуг.Where(w => w.Id_ВидУслуг == id_ВидУслуг).Select(w => w).First();
                this.textBox1.Text = curRow.ВидУслуги1.Trim();
                this.textBox2.Text = curRow.НомерПоПеречню.Trim();
                this.textBox3.Text = curRow.Цена.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int id = id_ВидУслуг;

            DataSetClass ds = new DataSetClass(sConnect);
            DataSetHospital dsh = ds.GetDataHospital();

            if (flagEdit == false)
            {
                // Добавляем записи.
                string query = "insert into ВидУслуги(ВидУслуги,Цена,id_поликлинника,НомерПоПеречню,Выбрать,id_кодУслуги,ТехЛист) " +
                                "values('" + this.textBox1.Text.Trim() + "'," + this.textBox3.Text + "," + id_поликлинники + ",'" + this.textBox2.Text.Trim() + "',False," + id + ",0) ";

                OleDBExecuteQuery quer = new OleDBExecuteQuery(dt.StringConnection);
                quer.QueryExcecute(query);
                LoadData(dsh);

            }
            else
            {

                string query = "update ВидУслуги " +
                               "set ВидУслуги = ' "+ this.textBox1.Text.Trim() +"' " +
                               ", НомерПоПеречню = '"+ this.textBox2.Text.Trim() +"' " +
                               ", Цена = "+ this.textBox3.Text +" " +
                               "where id_услуги = " + id_ВидУслуг + " ";

                OleDBExecuteQuery quer = new OleDBExecuteQuery(dt.StringConnection);
                quer.QueryExcecute(query);
                LoadData(dsh);

                // редактируем записи.
                this.btnSave.Text = "Сохранить";
            }
        }

        private void LoadData(DataSetHospital dsh)
        {

            int id = dsh.ТаблицаКлассификаторУслуг.Where(w => w.КлассификаторУслуг1.Trim() == this.comboBox1.Text.Trim()).Select(w => w).First().Id_КодУслуг;

            // Загрузим данные в DataGridView.
            DataLoad(id);

        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {


            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DataSetClass ds = new DataSetClass(sConnect);
                DataSetHospital dsh = ds.GetDataHospital();

                int id = (int)this.dataGridView1.CurrentRow.Cells["Id_ВидУслуг"].Value;

                string message = this.dataGridView1.CurrentRow.Cells["ВидУслуги1"].Value.ToString();

                FormMessage fMessage = new FormMessage("Удалить - " + message);
                fMessage.ShowDialog();

                if (fMessage.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    // Удалим запись.
                    string query = "delete from ВидУслуги " +
                                   "where id_услуги = "+ id +" ";

                    OleDBExecuteQuery quer = new OleDBExecuteQuery(dt.StringConnection);
                    quer.QueryExcecute(query);
                    LoadData(dsh);
                    
                    this.textBox1.Text = string.Empty;
                    this.textBox2.Text = string.Empty;
                    this.textBox3.Text = string.Empty;
                }
                else
                {
                    // Отмена.
                    return;
                }
                
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            FormEditВидУслуг form = new FormEditВидУслуг(dt.ТаблицаКлассификаторУслуг,dt.StringConnection);
            form.ShowDialog();

            if (form.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                //// После изменения Классификатора услуг внесём изменения в раскрывающийся список.
                //DataSetClass ds = new DataSetClass(sConnect);
                //DataSetHospital dsh = ds.GetDataHospital();

                //// Отобразим данные в классификаторе услуг.
                //this.comboBox1.DataSource = dsh.ТаблицаКлассификаторУслуг;
                //this.comboBox1.DisplayMember = "КлассификаторУслуг1";
                //this.comboBox1.ValueMember = "Id_КодУслуг";

                //int id = dsh.ТаблицаКлассификаторУслуг.Where(w => w.КлассификаторУслуг1.Trim() == this.comboBox1.Text.Trim()).Select(w => w).First().Id_КодУслуг;

                //// Загрузим данные в DataGridView.
                //DataLoad(id);

                UpdateКлассификаторУслуг();


            }
            else if (form.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                UpdateКлассификаторУслуг();
            }
        }

        private void UpdateКлассификаторУслуг()
        {
            // После изменения Классификатора услуг внесём изменения в раскрывающийся список.
                DataSetClass ds = new DataSetClass(sConnect);
                DataSetHospital dsh = ds.GetDataHospital();

                // Отобразим данные в классификаторе услуг.
                this.comboBox1.DataSource = dsh.ТаблицаКлассификаторУслуг;
                this.comboBox1.DisplayMember = "КлассификаторУслуг1";
                this.comboBox1.ValueMember = "Id_КодУслуг";

                int id = dsh.ТаблицаКлассификаторУслуг.Where(w => w.КлассификаторУслуг1.Trim() == this.comboBox1.Text.Trim()).Select(w => w).First().Id_КодУслуг;

                // Загрузим данные в DataGridView.
                DataLoad(id);

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

    }
}
