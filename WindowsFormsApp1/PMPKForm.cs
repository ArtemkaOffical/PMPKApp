using PMPK.DAL;
using PMPK.Interfaces;
using PMPK.Models;
using PMPK.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PMPK
{
    public partial class PMPKForm : Form, IForm
    {
        private int _idChild;
        private int _idPMPK;
        private UnitOfWork _unitOfWork;
        private Models.PMPK _currentPMPK;
        private Children _child;

        //Конструктор с 2 входными параметрами, для поиска данных в базе данных по id
        public PMPKForm(int idChild, int idPMPK)
        {

            InitializeComponent();
            _unitOfWork = new UnitOfWork();
            _idChild = idChild;
            _idPMPK = idPMPK;
            _child = _unitOfWork.ChildrenRepository.Get(includeProperties: "PMPKs").ToList().FirstOrDefault(x => x.Id == _idChild);
            _currentPMPK = _child.PMPKs.ToList().FirstOrDefault(x => x.Id == _idPMPK);

            _placeOfPMPK.DataSource = _unitOfWork.PlaceOfPMPKsRepository.Get(x => x.Name);
            _formStudy.DataSource = _unitOfWork.FormStudiesRepository.Get(x => x.Name);
            _studyOrg.DataSource = _unitOfWork.EdOrgsRepository.Get(x => x.Name);
            _studyProgram.DataSource = _unitOfWork.AProgramsRepository.Get(x => x.Name);
            _whereStudy.DataSource = _unitOfWork.PlaceStydiesRepository.Get(x => x.Name);
            _whomBy.DataSource = _unitOfWork.SpecialistsRepository.Get(x => x.FullName);
            _groupOrClass.DataSource = _unitOfWork.ClassesRepository.Get(x => x.Name);

            _comboOwner.DataSource = _unitOfWork.SpecialistsRepository.Get().Where(x=>x.Type.ToLower()=="руководитель").Select(r => r.FullName).ToList();
            _comboPedagog.DataSource = _unitOfWork.SpecialistsRepository.Get().Where(q => q.Type.ToLower() == "педагог").Select(r => r.FullName).ToList();

            _comboDefect.DataSource = _unitOfWork.SpecialistsRepository.Get().Where(x => x.Type.ToLower() == "дефектолог").Select(r => r.FullName).ToList();
            _comboPsiho.DataSource = _unitOfWork.SpecialistsRepository.Get().Where(x => x.Type.ToLower() == "психиатр").Select(r => r.FullName).ToList();
            _comboPsycho.DataSource = _unitOfWork.SpecialistsRepository.Get().Where(x => x.Type.ToLower() == "психолог").Select(r => r.FullName).ToList();
            _comboLogoped.DataSource = _unitOfWork.SpecialistsRepository.Get().Where(x => x.Type.ToLower() == "логопед").Select(r => r.FullName).ToList();

            tableLayoutPanel1.Controls.Owner.ForAllControls((x) => 
            {
            if((x is ComboBox)) 
                {
                    ((ComboBox)x).MaxDropDownItems = 50;
                    ((ComboBox)x).Text = string.Empty;
                }
            
            });
        }
        //Вызывается при загрузке формы
        private void PMPKForm_Load(object sender, EventArgs e)
        {
            if (_currentPMPK != null)
            {
                _address.Text = _currentPMPK.Address;
                label3.Text = _child.FullName;
                _formStudy.Text = _currentPMPK.FormStudy;
                _nextPMPKDate.Text = _currentPMPK.DateOfNextPMPK.ToString();
                _studyOrg.Text = _currentPMPK.Organization;
                _groupOrClass.Text = _currentPMPK.Class;
                _placeOfPMPK.Text = _currentPMPK.LocationOfEventPMPK;
                _dateOfPMPK.Text = _currentPMPK.DateOfPMPK.ToString();
                _number.Text = _currentPMPK.NumberOfProtocol.ToString();
                _perv.Checked = Convert.ToBoolean(_currentPMPK.FirstPriem);
                _direction.Checked = Convert.ToBoolean(_currentPMPK.Direction);
                _whomBy.Text = _currentPMPK.SentByToPMPK;
                _studyProgram.Text = _currentPMPK.AProgram;
                _whereStudy.Text = _currentPMPK.WhereStuding;
                _comment.Text = _currentPMPK.CommissionWithdrawal;
                _OVZ.Checked = Convert.ToBoolean(_currentPMPK.OVZ);
                _invalid.Checked = Convert.ToBoolean(_currentPMPK.Invalid);
                _control.Checked = Convert.ToBoolean(_currentPMPK.Control);
                _MSE.Checked = Convert.ToBoolean(_currentPMPK.MSE);
                _GIA9.Checked = Convert.ToBoolean(_currentPMPK.GIA9);
                _GIA11.Checked = Convert.ToBoolean(_currentPMPK.GIA11);
                _programm.Checked = Convert.ToBoolean(_currentPMPK.Programm);
                _notRuss.Checked = Convert.ToBoolean(_currentPMPK.NonRuss);
                _comboLogoped.Text = _currentPMPK.Logopedist;
                _comboDefect.Text = _currentPMPK.Defectologist;
                _comboPsycho.Text = _currentPMPK.Psychologist;
                _comboPsiho.Text = _currentPMPK.Psychiatrist;
                _comboOwner.Text = _currentPMPK.Pedagog;
                _comboPedagog.Text = _currentPMPK.OwnerPMPK;

            };
        }
        //Вызывается при после закрытия формы
        private void PMPKForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _unitOfWork.Dispose();
        }
        //Обновляет данные формы
        private void UpdateChilForm()
        {
            var childForm = Application.OpenForms.OfType<ChildForm>().FirstOrDefault();
            childForm.UpdatePMPKsChildList(_child.PMPKs.ToList().ConvertToDataTable());
        }
        //Задает стартовую позицию для texbox
        private void StartPositionIndex(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate ()
            {
                ((MaskedTextBox)sender).Select(0, 0);
                ((MaskedTextBox)sender).SelectAll();
            });
        }
        //Сохраняет сущность в БД
        private void Button1_Click(object sender, EventArgs e)
        {

            if (_child.PMPKs.Count == 1)
            {
                _perv.Checked = true;
            }
            if (CanSave())
            {
                if (string.IsNullOrEmpty(_dateOfPMPK.Text) || string.IsNullOrEmpty(_nextPMPKDate.Text))
                {
                    MessageBox.Show("Заполните дату ПМПК и следующую дату ПМПК");
                }
                else Save();
            }
            else
            {
                Controls.Owner.ForAllControls((x) =>
                {
                    if ((x is MaskedTextBox) || (x is ComboBox) || (x is TextBox)) x.BackColor = Color.Yellow;
                });
                MessageBox.Show("Заполните данные!");
            }
        }

        //Закрывает форму
        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }
      
        //Вызывается при закрытии формы
        private void PMPKForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CanSave())
            {
                if (MessageBox.Show("Не заполнены данные! Если Вы действительно хотите удалить запись о ребенке, нажмите ОК", "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Delete();
                }
                else e.Cancel = true;
            }
        }

        //Возвращает истину, если все условия прошли проверки успешно
        public bool CanSave()
        {
            if (string.IsNullOrEmpty(_placeOfPMPK.Text) || string.IsNullOrEmpty(_whereStudy.Text) || string.IsNullOrEmpty(_address.Text) || string.IsNullOrEmpty(_number.Text) || string.IsNullOrEmpty(_formStudy.Text))
            {
                return false;
            }
            return true;
        }
        //Сохраняет сущность в таблице БД
        public void Save()
        {
            if (_currentPMPK != null)
            {
                _currentPMPK.Address = _address.Text;
                _child.FullName = label3.Text;
                _currentPMPK.FormStudy = _formStudy.Text;
                _currentPMPK.Organization = _studyOrg.Text;
                _currentPMPK.Class = _groupOrClass.Text;
                _currentPMPK.LocationOfEventPMPK = _placeOfPMPK.Text;
                _currentPMPK.DateOfPMPK = Program.StringToDateTime(_dateOfPMPK.Text);
                _currentPMPK.DateOfNextPMPK = Program.StringToDateTime(_nextPMPKDate.Text);
                _currentPMPK.NumberOfProtocol = int.TryParse(_number.Text, out int result) ? result : 0;
                _currentPMPK.FirstPriem = Convert.ToByte(_perv.Checked);
                _currentPMPK.Direction = Convert.ToByte(_direction.Checked);
                _currentPMPK.SentByToPMPK = _whomBy.Text;
                _currentPMPK.AProgram = _studyProgram.Text;
                _currentPMPK.WhereStuding = _whereStudy.Text;
                _currentPMPK.CommissionWithdrawal = _comment.Text;
                _currentPMPK.OVZ = Convert.ToByte(_OVZ.Checked);
                _currentPMPK.Invalid = Convert.ToByte(_invalid.Checked);
                _currentPMPK.Control = Convert.ToByte(_control.Checked);
                _currentPMPK.MSE = Convert.ToByte(_MSE.Checked);
                _currentPMPK.GIA9 = Convert.ToByte(_GIA9.Checked);
                _currentPMPK.GIA11 = Convert.ToByte(_GIA11.Checked);
                _currentPMPK.NonRuss = Convert.ToByte(_notRuss.Checked);
                _currentPMPK.Logopedist = _comboLogoped.Text;
                _currentPMPK.Defectologist = _comboDefect.Text;
                _currentPMPK.Psychologist = _comboPsycho.Text;
                _currentPMPK.Psychiatrist = _comboPsiho.Text;
               _currentPMPK.Pedagog = _comboOwner.Text;
                _currentPMPK.OwnerPMPK = _comboPedagog.Text;
                _currentPMPK.Programm = Convert.ToByte(_programm.Checked);
                _unitOfWork.Save();
                UpdateChilForm();
                UpdateChildrenForm();
            };

        }
        //Обновляет данные другой формы
        private void UpdateChildrenForm()
        {
            var childrenForm = Application.OpenForms.OfType<ChildrenForm>().FirstOrDefault();
            childrenForm.UpdateChildrenList();
        }
        //Удаляет сущность из таблицы в БД
        public void Delete()
        {
            _child.PMPKs.Remove(_currentPMPK);
            _unitOfWork.Save();
            UpdateChilForm();
            UpdateChildrenForm();
        }
    }
}
