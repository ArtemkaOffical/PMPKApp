using PMPK.Utils;
using System;
using System.Windows.Forms;

namespace PMPK
{
    public partial class MainForm : Form
    {
        private InfoAboutOrganization _infoAboutOrganization;
        private ConnectDataBaseForm _connectToDataBase;
        private ChildrenForm _children;
        private WhereStudiesForm _whereStudForm;
        private PlacePMPK _placePmpkForm;
        private EdOrgForm _edOrgForm;
        private EdProgramForm _edProgramForm;
        private FormStudiesForm _formStudForm;
        private AboutProgram _aboutProgramForm;
        private ClassesForm _classesForm;
        private SpecialistsForm _specialistsForm;
        public MainForm()
        {
            InitializeComponent();

            _whereStudies.Click += WhereStudies_Click;
            _placeOfPmpk.Click += PlaceOfPmpk_Click;
            _educationalOrganization.Click += EducationalOrganization_Click;
            _educationalProgram.Click += EducationalProgram_Click;
            _formOfStudy.Click += FormOfStudy_Click;
            _classes.Click += Classes_Click;
            _specialists.Click += Specialists_Click;
            _aboutProgram.Click += AboutProgram_Click;
            _instruction.Click += Instruction_Click;
            _childrens.Click += Childrens_Click;
            _connectDB.Click += ConnectDB_Click;
            _infoOrganization.Click += InfoOrganization_Click;
            _restoreDB.Click += RestoreDB_Click; ;
            _backupBD.Click += BackupBD_Click; ;
        }

        //Открывает и создает форму
        private void BackupBD_Click(object sender, EventArgs e)
        {
            DBUtils.BackUpDB();
        }

        //Открывает и создает форму
        private void RestoreDB_Click(object sender, EventArgs e)
        {
            DBUtils.RestoreBD();
        }

        //Открывает и создает форму
        private void Specialists_Click(object sender, EventArgs e)
        {
            Extensions.ShowForm(ref _specialistsForm,false, this);
        }

        //Открывает и создает форму
        private void Classes_Click(object sender, EventArgs e)
        {
            Extensions.ShowForm(ref _classesForm, false, this);
        }

        //Открывает и создает форму
        private void FormOfStudy_Click(object sender, EventArgs e)
        {
            Extensions.ShowForm(ref _formStudForm, false, this);
        }

        //Открывает и создает форму
        private void EducationalProgram_Click(object sender, EventArgs e)
        {
            Extensions.ShowForm(ref _edProgramForm, false, this);
        }

        //Открывает и создает форму
        private void EducationalOrganization_Click(object sender, EventArgs e)
        {
            Extensions.ShowForm(ref _edOrgForm, false, this);
        }

        //Открывает и создает форму
        private void PlaceOfPmpk_Click(object sender, EventArgs e)
        {
            Extensions.ShowForm(ref _placePmpkForm, false, this);
        }

        //Открывает и создает форму
        private void WhereStudies_Click(object sender, EventArgs e)
        {
            Extensions.ShowForm(ref _whereStudForm, false, this);
        }

        //Открывает и создает форму
        private void Childrens_Click(object sender, EventArgs e)
        {
            Extensions.ShowForm(ref _children, false, this);
        }

        //Открывает и создает форму
        private void AboutProgram_Click(object sender, EventArgs e)
        {
            Extensions.ShowForm(ref _aboutProgramForm);
        }

        //Открывает и создает форму
        private void Instruction_Click(object sender, EventArgs e)
        {
            MessageBox.Show("инструкция");
        }

        //Открывает и создает форму
        private void ConnectDB_Click(object sender, EventArgs e)
        {
            Extensions.ShowForm(ref _connectToDataBase);
        }

        //Открывает и создает форму
        private void InfoOrganization_Click(object sender, EventArgs e)
        {
            Extensions.ShowForm(ref _infoAboutOrganization, true);
        }

   
    }
}
