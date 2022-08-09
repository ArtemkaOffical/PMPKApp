using PMPK.DAL;
using PMPK.Models;
using System;
using System.Windows.Forms;

namespace PMPK
{
    public partial class InfoAboutOrganization : Form
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();
        public InfoAboutOrganization()
        {
            InitializeComponent();
        }

        //Вызывается при загрузке формы
        private void InfoAboutOrganization_Load(object sender, EventArgs e)
        {

            if (_unitOfWork.OrganizationRepository.Count() != 0)
            {
                Organization organization = _unitOfWork.OrganizationRepository.FirstOrDefault();
                AdressBox.Text = organization.Adress;
                NameBox.Text = organization.Name;
                ShortNameBox.Text = organization.ShortName;
                PhoneBox.Text = organization.Phone;
                EmailBox.Text = organization.Email;
                INNBox.Text = organization.INN;
                OGRNBox.Text = organization.OGRN;
                BIKBox.Text = organization.BIK;
                KSCBox.Text = organization.Ksc;
                RSCBox.Text = organization.Rsc;
                BankBox.Text = organization.Bank;
            }

        }

        //Закрывает форму
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Сохраняет сущность в таблице БД
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (_unitOfWork.OrganizationRepository.Count() != 0)
            {
                Organization organization = _unitOfWork.OrganizationRepository.FirstOrDefault();
                organization.Adress = AdressBox.Text;
                organization.Name = NameBox.Text;
                organization.ShortName = ShortNameBox.Text;
                organization.Phone = PhoneBox.Text;
                organization.Email = EmailBox.Text;
                organization.INN = INNBox.Text;
                organization.OGRN = OGRNBox.Text;
                organization.BIK = BIKBox.Text;
                organization.Ksc = KSCBox.Text;
                organization.Rsc = RSCBox.Text;
                organization.Bank = BankBox.Text;
            }
            else
            {
                _unitOfWork.OrganizationRepository.Insert(new Organization
                {
                    Adress = AdressBox.Text,
                    Name = NameBox.Text,
                    ShortName = ShortNameBox.Text,
                    Phone = PhoneBox.Text,
                    Email = EmailBox.Text,
                    INN = INNBox.Text,
                    OGRN = OGRNBox.Text,
                    BIK = BIKBox.Text,
                    Ksc = KSCBox.Text,
                    Rsc = RSCBox.Text,
                    Bank = BankBox.Text,
                });
            }
            _unitOfWork.Save();
            this.Close();

        }

        //Вызывается при закрытии формы
        private void InfoAboutOrganization_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
