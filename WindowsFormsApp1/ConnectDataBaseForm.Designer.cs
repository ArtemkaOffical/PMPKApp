namespace PMPK
{
    partial class ConnectDataBaseForm
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
            this.ServerDataBase = new System.Windows.Forms.TextBox();
            this.NameDataBase = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ConnectToDataBase = new System.Windows.Forms.Button();
            this.AutoConnecetToDataBase = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ServerDataBase
            // 
            this.ServerDataBase.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ServerDataBase.Location = new System.Drawing.Point(171, 58);
            this.ServerDataBase.Multiline = true;
            this.ServerDataBase.Name = "ServerDataBase";
            this.ServerDataBase.Size = new System.Drawing.Size(210, 44);
            this.ServerDataBase.TabIndex = 1;
            // 
            // NameDataBase
            // 
            this.NameDataBase.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NameDataBase.Location = new System.Drawing.Point(171, 163);
            this.NameDataBase.Multiline = true;
            this.NameDataBase.Name = "NameDataBase";
            this.NameDataBase.Size = new System.Drawing.Size(210, 44);
            this.NameDataBase.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Адрес базы данных";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Имя базы данных";
            // 
            // ConnectToDataBase
            // 
            this.ConnectToDataBase.Location = new System.Drawing.Point(51, 391);
            this.ConnectToDataBase.Name = "ConnectToDataBase";
            this.ConnectToDataBase.Size = new System.Drawing.Size(330, 54);
            this.ConnectToDataBase.TabIndex = 5;
            this.ConnectToDataBase.Text = "Подключиться к базе данных";
            this.ConnectToDataBase.UseVisualStyleBackColor = true;
            this.ConnectToDataBase.Click += new System.EventHandler(this.ConnectToDataBase_Click);
            // 
            // AutoConnecetToDataBase
            // 
            this.AutoConnecetToDataBase.Location = new System.Drawing.Point(51, 451);
            this.AutoConnecetToDataBase.Name = "AutoConnecetToDataBase";
            this.AutoConnecetToDataBase.Size = new System.Drawing.Size(330, 50);
            this.AutoConnecetToDataBase.TabIndex = 6;
            this.AutoConnecetToDataBase.Text = "Автоматическое подключение к базе данных";
            this.AutoConnecetToDataBase.UseVisualStyleBackColor = true;
            this.AutoConnecetToDataBase.Click += new System.EventHandler(this.AutoConnecetToDataBase_Click);
            // 
            // ConnectDataBaseForm
            // 
            this.ClientSize = new System.Drawing.Size(435, 565);
            this.Controls.Add(this.AutoConnecetToDataBase);
            this.Controls.Add(this.ConnectToDataBase);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NameDataBase);
            this.Controls.Add(this.ServerDataBase);
            this.Name = "ConnectDataBaseForm";
            this.Text = "Подключение к БД";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConnectDataBaseForm_FormClosing);
            this.Load += new System.EventHandler(this.ConnectDataBaseForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ServerDataBase;
        private System.Windows.Forms.TextBox NameDataBase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ConnectToDataBase;
        private System.Windows.Forms.Button AutoConnecetToDataBase;
    }
}