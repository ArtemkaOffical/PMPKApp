namespace PMPK
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._menu = new System.Windows.Forms.MenuStrip();
            this._help = new System.Windows.Forms.ToolStripMenuItem();
            this._references = new System.Windows.Forms.ToolStripMenuItem();
            this._childrens = new System.Windows.Forms.ToolStripMenuItem();
            this._classes = new System.Windows.Forms.ToolStripMenuItem();
            this._specialists = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._placeOfPmpk = new System.Windows.Forms.ToolStripMenuItem();
            this._formOfStudy = new System.Windows.Forms.ToolStripMenuItem();
            this._educationalOrganization = new System.Windows.Forms.ToolStripMenuItem();
            this._educationalProgram = new System.Windows.Forms.ToolStripMenuItem();
            this._whereStudies = new System.Windows.Forms.ToolStripMenuItem();
            this._settings = new System.Windows.Forms.ToolStripMenuItem();
            this._connectDB = new System.Windows.Forms.ToolStripMenuItem();
            this._infoOrganization = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._restoreDB = new System.Windows.Forms.ToolStripMenuItem();
            this._backupBD = new System.Windows.Forms.ToolStripMenuItem();
            this._instruction = new System.Windows.Forms.ToolStripMenuItem();
            this._aboutProgram = new System.Windows.Forms.ToolStripMenuItem();
            this._menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _menu
            // 
            this._menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._help,
            this._references,
            this._settings});
            this._menu.Location = new System.Drawing.Point(0, 0);
            this._menu.Name = "_menu";
            this._menu.Size = new System.Drawing.Size(874, 24);
            this._menu.TabIndex = 0;
            this._menu.Text = "menuStrip1";
            // 
            // _help
            // 
            this._help.Name = "_help";
            this._help.Size = new System.Drawing.Size(65, 20);
            this._help.Text = "&Справка";
            // 
            // _references
            // 
            this._references.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._childrens,
            this._classes,
            this._specialists,
            this.toolStripSeparator1,
            this._placeOfPmpk,
            this._formOfStudy,
            this._educationalOrganization,
            this._educationalProgram,
            this._whereStudies});
            this._references.Name = "_references";
            this._references.Size = new System.Drawing.Size(94, 20);
            this._references.Text = "Справочники";
            // 
            // _childrens
            // 
            this._childrens.Name = "_childrens";
            this._childrens.Size = new System.Drawing.Size(247, 22);
            this._childrens.Text = "Справочник детей";
            // 
            // _classes
            // 
            this._classes.Name = "_classes";
            this._classes.Size = new System.Drawing.Size(247, 22);
            this._classes.Text = "Справочник классов/групп";
            // 
            // _specialists
            // 
            this._specialists.Name = "_specialists";
            this._specialists.Size = new System.Drawing.Size(247, 22);
            this._specialists.Text = "Справочник специалистов";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(244, 6);
            // 
            // _placeOfPmpk
            // 
            this._placeOfPmpk.Name = "_placeOfPmpk";
            this._placeOfPmpk.Size = new System.Drawing.Size(247, 22);
            this._placeOfPmpk.Text = "Районы обучения";
            // 
            // _formOfStudy
            // 
            this._formOfStudy.Name = "_formOfStudy";
            this._formOfStudy.Size = new System.Drawing.Size(247, 22);
            this._formOfStudy.Text = "Форма обучения";
            // 
            // _educationalOrganization
            // 
            this._educationalOrganization.Name = "_educationalOrganization";
            this._educationalOrganization.Size = new System.Drawing.Size(247, 22);
            this._educationalOrganization.Text = "Образовательные организации";
            // 
            // _educationalProgram
            // 
            this._educationalProgram.Name = "_educationalProgram";
            this._educationalProgram.Size = new System.Drawing.Size(247, 22);
            this._educationalProgram.Text = "Образовательные программы";
            // 
            // _whereStudies
            // 
            this._whereStudies.Name = "_whereStudies";
            this._whereStudies.Size = new System.Drawing.Size(247, 22);
            this._whereStudies.Text = "Где обучается";
            // 
            // _settings
            // 
            this._settings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._connectDB,
            this._infoOrganization,
            this.toolStripSeparator2,
            this._restoreDB,
            this._backupBD});
            this._settings.Name = "_settings";
            this._settings.Size = new System.Drawing.Size(79, 20);
            this._settings.Text = "Настройки";
            // 
            // _connectDB
            // 
            this._connectDB.Name = "_connectDB";
            this._connectDB.Size = new System.Drawing.Size(238, 22);
            this._connectDB.Text = "Подключение к БД";
            // 
            // _infoOrganization
            // 
            this._infoOrganization.Name = "_infoOrganization";
            this._infoOrganization.Size = new System.Drawing.Size(238, 22);
            this._infoOrganization.Text = "Сведения оганизации";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(235, 6);
            // 
            // _restoreDB
            // 
            this._restoreDB.Name = "_restoreDB";
            this._restoreDB.Size = new System.Drawing.Size(238, 22);
            this._restoreDB.Text = "Восстановление БД";
            // 
            // _backupBD
            // 
            this._backupBD.Name = "_backupBD";
            this._backupBD.Size = new System.Drawing.Size(238, 22);
            this._backupBD.Text = "Создать резервную копию БД";
            // 
            // _instruction
            // 
            this._instruction.Name = "_instruction";
            this._instruction.Size = new System.Drawing.Size(180, 22);
            this._instruction.Text = "Инструкция";
            // 
            // _aboutProgram
            // 
            this._aboutProgram.Name = "_aboutProgram";
            this._aboutProgram.Size = new System.Drawing.Size(180, 22);
            this._aboutProgram.Text = "О программе";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(874, 393);
            this.Controls.Add(this._menu);
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Главная";
            this._menu.ResumeLayout(false);
            this._menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        



        #endregion

        private System.Windows.Forms.MenuStrip _menu;
        private System.Windows.Forms.ToolStripMenuItem _help;
        private System.Windows.Forms.ToolStripMenuItem _aboutProgram;
        private System.Windows.Forms.ToolStripMenuItem _references;
        private System.Windows.Forms.ToolStripMenuItem _instruction;
        private System.Windows.Forms.ToolStripMenuItem _settings;
        private System.Windows.Forms.ToolStripMenuItem _connectDB;
        private System.Windows.Forms.ToolStripMenuItem _infoOrganization;
        private System.Windows.Forms.ToolStripMenuItem _childrens;
        private System.Windows.Forms.ToolStripMenuItem _classes;
        private System.Windows.Forms.ToolStripMenuItem _specialists;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem _placeOfPmpk;
        private System.Windows.Forms.ToolStripMenuItem _formOfStudy;
        private System.Windows.Forms.ToolStripMenuItem _educationalOrganization;
        private System.Windows.Forms.ToolStripMenuItem _educationalProgram;
        private System.Windows.Forms.ToolStripMenuItem _whereStudies;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem _restoreDB;
        private System.Windows.Forms.ToolStripMenuItem _backupBD;
    }
}

