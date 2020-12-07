namespace ControlDantist
{
    partial class FormValidContract
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.записатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.договорОтменитьПроверкуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменитьСтатусВНепрошедшийПроверкуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtИмя = new System.Windows.Forms.TextBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.аннулироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(1, 32);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(926, 365);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.записатьToolStripMenuItem,
            this.договорОтменитьПроверкуToolStripMenuItem,
            this.изменитьСтатусВНепрошедшийПроверкуToolStripMenuItem,
            this.аннулироватьToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(351, 114);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // записатьToolStripMenuItem
            // 
            this.записатьToolStripMenuItem.Name = "записатьToolStripMenuItem";
            this.записатьToolStripMenuItem.Size = new System.Drawing.Size(350, 22);
            this.записатьToolStripMenuItem.Text = "Установить как прошедший проверку";
            this.записатьToolStripMenuItem.Click += new System.EventHandler(this.записатьToolStripMenuItem_Click);
            // 
            // договорОтменитьПроверкуToolStripMenuItem
            // 
            this.договорОтменитьПроверкуToolStripMenuItem.Name = "договорОтменитьПроверкуToolStripMenuItem";
            this.договорОтменитьПроверкуToolStripMenuItem.Size = new System.Drawing.Size(350, 22);
            this.договорОтменитьПроверкуToolStripMenuItem.Text = "Отменить акт";
            this.договорОтменитьПроверкуToolStripMenuItem.Click += new System.EventHandler(this.договорОтменитьПроверкуToolStripMenuItem_Click);
            // 
            // изменитьСтатусВНепрошедшийПроверкуToolStripMenuItem
            // 
            this.изменитьСтатусВНепрошедшийПроверкуToolStripMenuItem.Name = "изменитьСтатусВНепрошедшийПроверкуToolStripMenuItem";
            this.изменитьСтатусВНепрошедшийПроверкуToolStripMenuItem.Size = new System.Drawing.Size(350, 22);
            this.изменитьСтатусВНепрошедшийПроверкуToolStripMenuItem.Text = "Изменить статус на \"НЕПРОШЕДШИЙ ПРОВЕРКУ\"";
            this.изменитьСтатусВНепрошедшийПроверкуToolStripMenuItem.Click += new System.EventHandler(this.изменитьСтатусВНепрошедшийПроверкуToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Номер договора:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(112, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(335, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "фамилия льготника:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(452, 6);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(221, 20);
            this.textBox2.TabIndex = 4;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(839, 412);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(679, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Имя";
            // 
            // txtИмя
            // 
            this.txtИмя.Location = new System.Drawing.Point(715, 6);
            this.txtИмя.Name = "txtИмя";
            this.txtИмя.Size = new System.Drawing.Size(100, 20);
            this.txtИмя.TabIndex = 6;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(821, 4);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 7;
            this.btnFind.Text = "Найти";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // аннулироватьToolStripMenuItem
            // 
            this.аннулироватьToolStripMenuItem.Name = "аннулироватьToolStripMenuItem";
            this.аннулироватьToolStripMenuItem.Size = new System.Drawing.Size(350, 22);
            this.аннулироватьToolStripMenuItem.Text = "Аннулировать";
            this.аннулироватьToolStripMenuItem.Click += new System.EventHandler(this.аннулироватьToolStripMenuItem_Click);
            // 
            // FormValidContract
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 447);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtИмя);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormValidContract";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormValidContract";
            this.Load += new System.EventHandler(this.FormValidContract_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem записатьToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtИмя;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.ToolStripMenuItem договорОтменитьПроверкуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изменитьСтатусВНепрошедшийПроверкуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem аннулироватьToolStripMenuItem;
    }
}