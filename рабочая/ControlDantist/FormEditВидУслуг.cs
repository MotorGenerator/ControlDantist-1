using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Classes;

namespace ControlDantist
{
    public partial class FormEditВидУслуг : Form
    {
        List<КлассификаторУслуг> list;
        private bool flagEdit = false;
        private string sConnect = string.Empty;

        private int id_классификаторУслуг = 0;

        public FormEditВидУслуг()
        {
            InitializeComponent();
        }

        public FormEditВидУслуг(List<КлассификаторУслуг> listKY, string sConnection)
            : this()
        {
            list = listKY;

            // По умолчанию формаработает в режиме добавления записей.
            flagEdit = false;

            id_классификаторУслуг = 0;

            sConnect = sConnection;
        }

        private void FormEditВидУслуг_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {

            // После изменения Классификатора услуг внесём изменения в раскрывающийся список.
            DataSetClass ds = new DataSetClass(sConnect);
            DataSetHospital dsh = ds.GetDataHospital();

            //// Отобразим данные в классификаторе услуг.
            //this.comboBox1.DataSource = dsh.ТаблицаКлассификаторУслуг;
            //this.comboBox1.DisplayMember = "КлассификаторУслуг1";
            //this.comboBox1.ValueMember = "Id_КодУслуг";

            //int id = dsh.ТаблицаКлассификаторУслуг.Where(w => w.КлассификаторУслуг1.Trim() == this.comboBox1.Text.Trim()).Select(w => w).First().Id_КодУслуг;

            //// Загрузим данные в DataGridView.
            //DataLoad(id);


            this.dataGridView1.DataSource = dsh.ТаблицаКлассификаторУслуг;
            this.dataGridView1.Columns["Id_КодУслуг"].Visible = false;
            this.dataGridView1.Columns["КлассификаторУслуг1"].Visible = true;
            this.dataGridView1.Columns["КлассификаторУслуг1"].Width = 520;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Переведём форму в режим редактирования записи.
            flagEdit = true;

            // Изменим текст на кнопке Сохранить
            this.btnOk.Text = "Изменить";

            // Получим id выбранной услуги.
            id_классификаторУслуг = (int)this.dataGridView1.CurrentRow.Cells["Id_КодУслуг"].Value;

            // Отобразим выбранный пункт для редактирования.
            this.textBox1.Text = this.dataGridView1.CurrentRow.Cells["КлассификаторУслуг1"].Value.ToString().Trim();

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (flagEdit == false)
            {
                // Добавим запись.
                string query = "insert into КлассификаторУслуги (КлассификаторУслуги) " +
                             "values(' " + this.textBox1.Text.Trim() + "') ";

                OleDBExecuteQuery quer = new OleDBExecuteQuery(sConnect);
                quer.QueryExcecute(query);
                LoadData();

                // Обнулим поле редактирования.
                this.textBox1.Text = string.Empty;

            }
            else if(flagEdit == true)
            {
                // Изменим запись.
                string query = "update КлассификаторУслуги " +
                              "set КлассификаторУслуги = ' " + this.textBox1.Text.Trim() + "' " +
                              "where id_кодУслуги = " + id_классификаторУслуг + " ";

                OleDBExecuteQuery quer = new OleDBExecuteQuery(sConnect);
                quer.QueryExcecute(query);
                LoadData();

                // редактируем записи.
                this.btnOk.Text = "Сохранить";

                // Обнулим поле редактирования.
                this.textBox1.Text = string.Empty;
            }

        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                int id = (int)this.dataGridView1.CurrentRow.Cells["Id_КодУслуг"].Value;

                string message = this.dataGridView1.CurrentRow.Cells["КлассификаторУслуг1"].Value.ToString();

                FormMessage fMessage = new FormMessage("Удалить - " + message);
                fMessage.ShowDialog();

                if (fMessage.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    // Так как эта фигня Access не позволяет всё выполнить в одной транзакции тогда идём не правильным путём.
                    // Удалим запись.
                    string query = "delete from ВидУслуги " +
                                    "where id_кодУслуги in (select id_кодУслуги from КлассификаторУслуги " +
                                    " where id_кодУслуги = " + id + " ) "; // +
                    string query2 = "delete from КлассификаторУслуги " +
                                    "where id_кодУслуги = " + id + " ";

                    try
                    {
                        OleDBExecuteQuery quer = new OleDBExecuteQuery(sConnect);
                        quer.QueryExcecute(query);

                        OleDBExecuteQuery quer2 = new OleDBExecuteQuery(sConnect);
                        quer.QueryExcecute(query2);


                        LoadData();

                        this.textBox1.Text = string.Empty;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message,"Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        //- код-2147467259
                        //MessageBox.Show("");
                    }
                }
                else
                {
                    // Отмена.
                    return;
                }
                
            }
        }
    }
}
