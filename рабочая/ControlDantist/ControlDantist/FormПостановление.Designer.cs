namespace ControlDantist
{
    partial class FormПостановление
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblNum = new System.Windows.Forms.Label();
            this.txtНомер = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtПостановление = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.lblПо = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.ldlStart = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNum
            // 
            this.lblNum.AutoSize = true;
            this.lblNum.Location = new System.Drawing.Point(20, 149);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(124, 13);
            this.lblNum.TabIndex = 0;
            this.lblNum.Text = "Номер постановления:";
            // 
            // txtНомер
            // 
            this.txtНомер.Location = new System.Drawing.Point(150, 146);
            this.txtНомер.Name = "txtНомер";
            this.txtНомер.Size = new System.Drawing.Size(80, 20);
            this.txtНомер.TabIndex = 1;
            this.txtНомер.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtПостановление);
            this.groupBox1.Location = new System.Drawing.Point(16, 172);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Название постановления";
            // 
            // txtПостановление
            // 
            this.txtПостановление.Enabled = false;
            this.txtПостановление.Location = new System.Drawing.Point(7, 20);
            this.txtПостановление.Multiline = true;
            this.txtПостановление.Name = "txtПостановление";
            this.txtПостановление.Size = new System.Drawing.Size(445, 74);
            this.txtПостановление.TabIndex = 0;
            this.txtПостановление.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.maskedTextBox1);
            this.groupBox2.Controls.Add(this.lblПо);
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Controls.Add(this.ldlStart);
            this.groupBox2.Location = new System.Drawing.Point(16, 272);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(329, 55);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Срок действия";
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.Location = new System.Drawing.Point(189, 19);
            this.maskedTextBox1.Mask = "00/00/0000";
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(134, 20);
            this.maskedTextBox1.TabIndex = 3;
            this.maskedTextBox1.ValidatingType = typeof(System.DateTime);
            // 
            // lblПо
            // 
            this.lblПо.AutoSize = true;
            this.lblПо.Location = new System.Drawing.Point(163, 24);
            this.lblПо.Name = "lblПо";
            this.lblПо.Size = new System.Drawing.Size(19, 13);
            this.lblПо.TabIndex = 2;
            this.lblПо.Text = "по";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.Location = new System.Drawing.Point(27, 20);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(130, 20);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // ldlStart
            // 
            this.ldlStart.AutoSize = true;
            this.ldlStart.Location = new System.Drawing.Point(8, 24);
            this.ldlStart.Name = "ldlStart";
            this.ldlStart.Size = new System.Drawing.Size(13, 13);
            this.ldlStart.TabIndex = 0;
            this.ldlStart.Text = "с";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(399, 340);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Закрыть";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(318, 340);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "ОК";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(236, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "от";
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.Location = new System.Drawing.Point(260, 145);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(130, 20);
            this.dateTimePicker3.TabIndex = 7;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(3, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(471, 136);
            this.dataGridView1.TabIndex = 8;
            // 
            // FormПостановление
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 370);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.dateTimePicker3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtНомер);
            this.Controls.Add(this.lblNum);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormПостановление";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Основание принятия стоматологических услуг";
            this.Load += new System.EventHandler(this.FormПостановление_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNum;
        private System.Windows.Forms.TextBox txtНомер;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtПостановление;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblПо;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label ldlStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.MaskedTextBox maskedTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}