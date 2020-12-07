using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ControlDantist
{
    public partial class FormSum : Form
    {
        private decimal sumПрошедших;
        private decimal sumНеПрошедших;

        /// <summary>
        /// Хранит сумму прошедших договоров
        /// </summary>
        public decimal SumПрошедших
        {
            get
            {
                return sumПрошедших;
            }
            set
            {
                sumПрошедших = value;
            }
        }


        /// <summary>
        /// Хранит сумму не прошедших или отложенных договоров
        /// </summary>
        public decimal SumНеПрошедших
        {
            get
            {
                return sumНеПрошедших;
            }
            set
            {
                sumНеПрошедших = value;
            }
        }


        public FormSum()
        {
            InitializeComponent();
        }

        private void FormSum_Load(object sender, EventArgs e)
        {
            this.label1.Text = "Сумма договоров прошедших проверку = " + this.SumПрошедших.ToString("c");
            this.label2.Text = "Сумма договоров не прошедших проверку = " + this.SumНеПрошедших.ToString("c");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}