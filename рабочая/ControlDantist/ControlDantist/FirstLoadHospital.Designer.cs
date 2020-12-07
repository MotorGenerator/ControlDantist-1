namespace ControlDantist
{
    partial class FirstLoadHospital
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
            this.lblHospital = new System.Windows.Forms.Label();
            this.txtNameHospital = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtИНН = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtBdPach = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnПостановление = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHospital
            // 
            this.lblHospital.AutoSize = true;
            this.lblHospital.Location = new System.Drawing.Point(12, 9);
            this.lblHospital.Name = "lblHospital";
            this.lblHospital.Size = new System.Drawing.Size(81, 13);
            this.lblHospital.TabIndex = 0;
            this.lblHospital.Text = "Поликлинника";
            // 
            // txtNameHospital
            // 
            this.txtNameHospital.Location = new System.Drawing.Point(99, 6);
            this.txtNameHospital.Name = "txtNameHospital";
            this.txtNameHospital.Size = new System.Drawing.Size(274, 20);
            this.txtNameHospital.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ИНН";
            // 
            // txtИНН
            // 
            this.txtИНН.Location = new System.Drawing.Point(99, 32);
            this.txtИНН.Name = "txtИНН";
            this.txtИНН.Size = new System.Drawing.Size(100, 20);
            this.txtИНН.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.txtBdPach);
            this.groupBox1.Location = new System.Drawing.Point(11, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(362, 57);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "База данных";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(330, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 23);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtBdPach
            // 
            this.txtBdPach.Location = new System.Drawing.Point(7, 20);
            this.txtBdPach.Name = "txtBdPach";
            this.txtBdPach.Size = new System.Drawing.Size(317, 20);
            this.txtBdPach.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(300, 177);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.Location = new System.Drawing.Point(219, 177);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Добавить";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.btnПостановление);
            this.groupBox2.Location = new System.Drawing.Point(11, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(362, 57);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Постановление";
            // 
            // btnПостановление
            // 
            this.btnПостановление.Location = new System.Drawing.Point(330, 14);
            this.btnПостановление.Name = "btnПостановление";
            this.btnПостановление.Size = new System.Drawing.Size(25, 23);
            this.btnПостановление.TabIndex = 1;
            this.btnПостановление.UseVisualStyleBackColor = true;
            this.btnПостановление.Click += new System.EventHandler(this.btnПостановление_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(7, 14);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(317, 37);
            this.textBox1.TabIndex = 2;
            // 
            // FirstLoadHospital
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 211);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtИНН);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNameHospital);
            this.Controls.Add(this.lblHospital);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FirstLoadHospital";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добавить поликлиннику";
            this.Load += new System.EventHandler(this.FirstLoadHospital_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHospital;
        private System.Windows.Forms.TextBox txtNameHospital;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtИНН;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtBdPach;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnПостановление;
        private System.Windows.Forms.TextBox textBox1;
    }
}