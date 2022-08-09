using PMPK.DAL;
using PMPK.Interfaces;
using PMPK.Models;
using PMPK.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PMPK
{
    public partial class ChildForm : Form, IForm
    {
        private UnitOfWork _unitOfWork;
        private int _row = 0;
        private Children _child;
        private PMPKForm _pmpkForm;
        private List<Parent> _passportsParent;
        private List<Document> _passportsChild;

        //Констурктор класса с входным параметром id
        public ChildForm(int id)
        {
            InitializeComponent();
            _unitOfWork = new UnitOfWork();
            _child = _unitOfWork.ChildrenRepository.Get(includeProperties: "PMPKs,Documents,Parents").ToList().FirstOrDefault(x => x.Id == id);
            _passportsParent = _child.Parents.ToList();
            _passportsChild = _child.Documents.ToList();
            DataTable data = _child?.PMPKs.ConvertToDataTable();
            dataGridView1.DataSource = data;
            dataGridView1.CurrentCell = dataGridView1[0, _row];
            dataGridView1.Columns[0].Frozen = true;
            dataGridView1.AutoSizeColumns();
        }
        //Задает стартовую позицию ввода для TextBox при клике на него
        private void StartPositionIndex(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate ()
            {
                ((MaskedTextBox)sender).Select(0, 0);
                ((MaskedTextBox)sender).SelectAll();
            });
        }
        //Удаляет сущность из таблицы PMPKs в БД
        private void DeletePMPK_Click(object sender, EventArgs e)
        {
            if ((_row >= 0 && _row != dataGridView1.RowCount - 1))
            {
                if (MessageBox.Show("Вы действительно хотите удалить строку?", "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    int idPMPK = Convert.ToInt32(dataGridView1["Id", _row].Value.ToString());
                    _unitOfWork.PMPKsRepository.Update(_unitOfWork.PMPKsRepository.GetByID(idPMPK));
                    _unitOfWork.PMPKsRepository.Delete(idPMPK);
                    _unitOfWork.Save();
                    DataTable data = _child.PMPKs.ToList().ConvertToDataTable();
                    dataGridView1.DataSource = data;
                    _row--;
                    if (_row == -1) _row = 0;
                    dataGridView1.CurrentCell = dataGridView1[0, _row];
                    UpdateChildrenForm();
                }
            }
        }

        //Создает новую сущность в таблице PMPKs
        private void AddPMPK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_child.FullName)) 
            {
                if (Extensions.FormIsNotOpen(_pmpkForm))
                {
                    Models.PMPK pmpk = new Models.PMPK();
                    _child.PMPKs.Add(pmpk);
                    _unitOfWork.Save();
                    _pmpkForm = new PMPKForm(_child.Id, pmpk.Id);
                    _pmpkForm.ShowDialog();
                    _pmpkForm.Dispose();
                }
            }
            else
            {
                MessageBox.Show(@"Заполните данные ребенка и нажмите кнопку \""Сохранить""\");
            }
        }
        //Закрывает форму
        private void Button2_Click(object sender, EventArgs e)
        {
            foreach (var item in _unitOfWork.PMPKsRepository.Get())
            {
                _unitOfWork.PMPKsRepository.Update(item);
            }
            _unitOfWork.Save();
            Close();
        }
        //Сохраняет информацию о ребенке
        private void Button1_Click(object sender, EventArgs e)
        {
            if (CanSave())
            {
                Save();
            }
            else
            {
                tableLayoutPanel4.Controls.Owner.ForAllControls((x) =>
                {
                    if ((x is MaskedTextBox) || (x is ComboBox) || (x is TextBox)) x.BackColor = Color.Yellow;
                });
                MessageBox.Show("Заполните данные ребенка и укажите корректно возвраст с датой!");
            }
        }
        //Проверка данных перед зкрытием формы
        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CanSave())
            {
                if (MessageBox.Show("Не заполнены данные о ребенке! Если Вы действительно хотите удалить запись о ребенке, нажмите ОК", "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Delete();
                }
                else e.Cancel = true;
            }
        }

        //Возвращает истину , если все условия проверки прошли успешно
        public bool CanSave()
        {
            if ((!int.TryParse(_age.Text, out int x) || x < 0 || x > 120) || string.IsNullOrEmpty(_fullName.Text) || string.IsNullOrEmpty(_sex.Text) || string.IsNullOrEmpty(_dateOfBirth.Text))
            {
                return false;
            }
            return true;
        }
        //Удаляет сущность из таблицы Children в БД
        public void Delete()
        {

            _child.Documents.ForEach(d =>
            {
                _unitOfWork.PassportRepository.Delete(_unitOfWork.DocumentsRepository.Get(includeProperties: "Passport").Where(currentDocument => currentDocument.Id == d.Id).FirstOrDefault().Passport);
            });

            _child.Parents.ForEach(p =>
            {
                _unitOfWork.PassportRepository.Delete(_unitOfWork.ParentsRepository.Get(includeProperties: "Passport").Where(currentParent => currentParent.Id == p.Id).FirstOrDefault().Passport);
            });

            foreach (var item in _child.PMPKs.ToList())
            {
                _unitOfWork.PMPKsRepository.Reload(item);
            }

            _unitOfWork.ChildrenRepository.Delete(_child);
            _unitOfWork.Save();
            UpdateChildrenForm();
        }
        //Обновляет данные по детям в другой форме
        private void UpdateChildrenForm()
        {
            var childrenForm = Application.OpenForms.OfType<ChildrenForm>().FirstOrDefault();
            childrenForm.UpdateChildrenList();
        }

        //Сохраняет данные текущей формы
        public void Save()
        {
           
            try
            {
                if (Regex.IsMatch(_fullName.Text, "([А-ЯЁ][а-яё]+[\\-\\s]?){3,}"))
                {
                    DateTime now = DateTime.Today;
                    DateTime bday = DateTime.ParseExact(_dateOfBirth.Text, "ddMMyyyy", null);
                    int age = now.Year - bday.Year;
                    if (bday > now.AddYears(-age)) age--;
                    _age.Text = age.ToString();
                        _child.FullName = _fullName.Text;
                        _child.Age = age;
                        _child.Sex = Convert.ToChar(_sex.Text);
                        _child.DateOfBirth = DateTime.ParseExact(_dateOfBirth.Text, "ddMMyyyy", null);
                        _child.Mnogodet = Convert.ToByte(checkBox1.Checked);
                        _child.MalIm = Convert.ToByte(checkBox2.Checked);
                        _child.Sirota = Convert.ToByte(checkBox3.Checked);

                        if (_passportsParent.Count != 0)
                        {

                            Passport motherPassport = _passportsParent[0].GetFirstPropertyOfEntity(_unitOfWork.ParentsRepository, "Passport").Passport;
                            _passportsParent[0].FullName = _motherFullName.Text;
                            _passportsParent[0].Phone = _motherPhone.Text;
                            _passportsParent[0].DateOfBirth = DateTime.TryParseExact(_motherDateOfBirth.Text, "ddMMyyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date) ? (DateTime?)date : null;
                            motherPassport.Number = int.TryParse(_motherPassportNumber.Text, out int result) ? (int?)result : null;
                            motherPassport.Series = int.TryParse(_motherPassportSerial.Text, out result) ? (int?)result : null;
                            motherPassport.DateOfIssue = Program.StringToDateTime(_motherPassportDate.Text);
                            motherPassport.IssuedByWhom = _motherPassportIssued.Text;


                            Passport fatherPassport = _passportsParent[1].GetFirstPropertyOfEntity(_unitOfWork.ParentsRepository, "Passport").Passport;
                            _passportsParent[1].FullName = _fatherFullName.Text;
                            _passportsParent[1].Phone = _fatherPhone.Text;
                            _passportsParent[1].DateOfBirth = DateTime.TryParseExact(_fatherDateOfBirth.Text, "ddMMyyyy", null, System.Globalization.DateTimeStyles.None, out date) ? (DateTime?)date : null;
                            fatherPassport.Number = int.TryParse(_fatherPassportNumber.Text, out result) ? (int?)result : null;
                            fatherPassport.Series = int.TryParse(_fatherPassportSerial.Text, out result) ? (int?)result : null;
                            fatherPassport.DateOfIssue = Program.StringToDateTime(_fatherPassportDate.Text);
                            fatherPassport.IssuedByWhom = _fatherPassportIssued.Text;

                            Passport opecPassport = _passportsParent[2].GetFirstPropertyOfEntity(_unitOfWork.ParentsRepository, "Passport").Passport;
                            _passportsParent[2].FullName = _opecFullName.Text;
                            _passportsParent[2].Phone = _opecPhone.Text;
                            _passportsParent[2].DateOfBirth = DateTime.TryParseExact(_opecDateOfBirth.Text, "ddMMyyyy", null, System.Globalization.DateTimeStyles.None, out date) ? (DateTime?)date : null;
                            opecPassport.Number = int.TryParse(_opecPassportNumber.Text, out result) ? (int?)result : null;
                            opecPassport.Series = int.TryParse(_opecPassportSerial.Text, out result) ? (int?)result : null;
                            opecPassport.DateOfIssue = Program.StringToDateTime(_opecPassportDate.Text);
                            opecPassport.IssuedByWhom = _opecPassportIssued.Text;
                        }

                        if (_passportsChild.Count != 0)
                        {

                            Passport passport = _passportsChild[0].GetFirstPropertyOfEntity(_unitOfWork.DocumentsRepository, "Passport").Passport;

                            passport.Number = int.TryParse(_passportNumber.Text, out int result) ? (int?)result : null;
                            passport.Series = int.TryParse(_passportSerial.Text, out result) ? (int?)result : null;
                            passport.DateOfIssue = Program.StringToDateTime(_passportDate.Text);
                            passport.IssuedByWhom = _passportIssued.Text;

                            Passport polis = _passportsChild[1].GetFirstPropertyOfEntity(_unitOfWork.DocumentsRepository, "Passport").Passport;

                            polis.Number = int.TryParse(_polisNumber.Text, out result) ? (int?)result : null;
                            polis.Series = int.TryParse(_polisSerial.Text, out result) ? (int?)result : null;
                            polis.DeliveryDate = Program.StringToDateTime(_polisDate.Text);
                            polis.DateOfIssue = Program.StringToDateTime(_polisDateIssued.Text);
                            polis.INN = int.TryParse(_polisINN.Text, out result) ? (int?)result : null;
                            polis.IssuedByWhom = _polisIssued.Text;

                            Passport invalid = _passportsChild[2].GetFirstPropertyOfEntity(_unitOfWork.DocumentsRepository, "Passport").Passport;

                            invalid.Number = int.TryParse(_InvalidNumber.Text, out result) ? (int?)result : null;
                            invalid.DeliveryDate = Program.StringToDateTime(_InvalidDate.Text);
                            invalid.IssuedByWhom = _InvalidIssued.Text;

                            Passport birth = _passportsChild[3].GetFirstPropertyOfEntity(_unitOfWork.DocumentsRepository, "Passport").Passport;

                            birth.Number = int.TryParse(_birthNumber.Text, out result) ? (int?)result : null;
                            birth.Series = int.TryParse(_birthSerial.Text, out result) ? (int?)result : null;
                            birth.DateOfIssue = Program.StringToDateTime(_birthDate.Text);
                            birth.IssuedByWhom = _birthIssued.Text;

                        }

                        _unitOfWork.Save();
                    _unitOfWork.ChildrenRepository.Update(_child);
                        UpdateChildrenForm();
                   
                }
                else { MessageBox.Show("Пожалуйста введите ФИО по маске: Фамилия Имя Отчество"); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Запись не сохранена из-за непредвиденной ошибки, обратитесь к разработчику.");
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        //Обновляет список посещения историй
        public void UpdatePMPKsChildList(DataTable data)
        {
            foreach (var item in _child.PMPKs.ToList())
            {
                _unitOfWork.PMPKsRepository.Reload(item);
            }
            _unitOfWork.ChildrenRepository.Reload(_child);
            dataGridView1.DataSource = data;
        }
        //Получает данные с datetime и присваивает их в textbox
        private void GetResult(MaskedTextBox textBox, DateTimePicker dateTime)
        {
            textBox.Text = dateTime.Value.ToString();
        }
        //При выборе даты выполняет функцию GetResult
        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            GetResult(_dateOfBirth, dateTimePicker1);
        }
        //Метод вызывается после закрытия формы
        private void ChildForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _unitOfWork.Dispose();
        }
        //Метод вызывается при загрузке формы
        private void ChildForm_Load(object sender, EventArgs e)
        {
            _fullName.Text = _child.FullName;
            _age.Text = _child.Age.ToString();
            _sex.Text = _child.Sex.ToString();
            _dateOfBirth.Text = _child.DateOfBirth.ToString();
            checkBox1.Checked = Convert.ToBoolean(_child.Mnogodet);
            checkBox2.Checked = Convert.ToBoolean(_child.MalIm);
            checkBox3.Checked = Convert.ToBoolean(_child.Sirota);

            if (_passportsParent.Count != 0)
            {

                Passport motherPassport = _passportsParent[0].GetFirstPropertyOfEntity(_unitOfWork.ParentsRepository, "Passport").Passport;
                _motherFullName.Text = _passportsParent[0].FullName;
                _motherPhone.Text = _passportsParent[0].Phone;
                _motherDateOfBirth.Text = _passportsParent[0].DateOfBirth.ToString();
                _motherPassportNumber.Text = motherPassport.Number.ToString();
                _motherPassportSerial.Text = motherPassport.Series.ToString();


                _motherPassportDate.Text = motherPassport.DateOfIssue.ToString();
                _motherPassportIssued.Text = motherPassport.IssuedByWhom;

                Passport fatherPassport = _passportsParent[1].GetFirstPropertyOfEntity(_unitOfWork.ParentsRepository, "Passport").Passport;
                _fatherFullName.Text = _passportsParent[1].FullName;
                _fatherPhone.Text = _passportsParent[1].Phone;
                _fatherDateOfBirth.Text = _passportsParent[1].DateOfBirth.ToString();
                _fatherPassportNumber.Text = fatherPassport.Number.ToString();
                _fatherPassportSerial.Text = fatherPassport.Series.ToString();
                _fatherPassportDate.Text = fatherPassport.DateOfIssue.ToString();
                _fatherPassportIssued.Text = fatherPassport.IssuedByWhom;

                Passport opecPassport = _passportsParent[2].GetFirstPropertyOfEntity(_unitOfWork.ParentsRepository, "Passport").Passport;
                _opecFullName.Text = _passportsParent[2].FullName;
                _opecPhone.Text = _passportsParent[2].Phone;
                _opecDateOfBirth.Text = _passportsParent[2].DateOfBirth.ToString();
                _opecPassportNumber.Text = opecPassport.Number.ToString();
                _opecPassportSerial.Text = opecPassport.Series.ToString();
                _opecPassportDate.Text = opecPassport.DateOfIssue.ToString();
                _opecPassportIssued.Text = opecPassport.IssuedByWhom;
            }

            if (_passportsChild.Count != 0)
            {
                Passport passport = _passportsChild[0].GetFirstPropertyOfEntity(_unitOfWork.DocumentsRepository, "Passport").Passport;

                _passportNumber.Text = passport.Number.ToString();
                _passportSerial.Text = passport.Series.ToString();
                _passportDate.Text = passport.DateOfIssue.ToString();
                _passportIssued.Text = passport.IssuedByWhom;

                Passport polis = _passportsChild[1].GetFirstPropertyOfEntity(_unitOfWork.DocumentsRepository, "Passport").Passport;

                _polisNumber.Text = polis.Number.ToString();
                _polisSerial.Text = polis.Series.ToString();
                _polisDate.Text = polis.DeliveryDate.ToString();
                _polisDateIssued.Text = polis.DateOfIssue.ToString();
                _polisINN.Text = polis.INN.ToString();
                _polisIssued.Text = polis.IssuedByWhom;

                Passport invalid = _passportsChild[2].GetFirstPropertyOfEntity(_unitOfWork.DocumentsRepository, "Passport").Passport;

                _InvalidNumber.Text = invalid.Number.ToString();
                _InvalidDate.Text = invalid.DeliveryDate.ToString();
                _InvalidIssued.Text = invalid.IssuedByWhom;

                Passport birth = _passportsChild[3].GetFirstPropertyOfEntity(_unitOfWork.DocumentsRepository, "Passport").Passport;

                _birthNumber.Text = birth.Number.ToString();
                _birthSerial.Text = birth.Series.ToString();
                _birthDate.Text = birth.DateOfIssue.ToString();
                _birthIssued.Text = birth.IssuedByWhom;
            }
        }
        //Метод вызывается при двойном клике по данным в таблице DataGridView
        private void DataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int indexRow = e.RowIndex;
            int idPMPK = Convert.ToInt32(dataGridView1["Id", _row].Value.ToString());
            if (indexRow >= 0)
            {
                if ((_row >= 0 && _row != dataGridView1.RowCount - 1))
                {
                    if (Extensions.FormIsNotOpen(_pmpkForm))
                    {
                        _pmpkForm = new PMPKForm(_child.Id, idPMPK);
                        _pmpkForm.ShowDialog();                    
                    }
                }
            }
        }
        //Метод вызывается при клике по данным в таблице DataGridView
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != -1)
            {
                _row = dataGridView1.CurrentRow.Index;
            }
        }
    }
}
