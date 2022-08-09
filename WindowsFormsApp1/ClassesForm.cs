using PMPK.DAL;
using PMPK.Interfaces;
using PMPK.Models;
using PMPK.Utils;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PMPK
{
    public partial class ClassesForm : Form, IForm
    {
        private UnitOfWork _unitOfWork;

        public ClassesForm()
        {
            InitializeComponent();
            _unitOfWork = new UnitOfWork();
            UpdateDataGridView();
        }

        //Сохраняет данные
        private void Button1_Click(object sender, EventArgs e)
        {
            if (CanSave())
            {
                MessageBox.Show("Данные сохранены!");
                Save();
                UpdateDataGridView();
            }
        }
        //Закрывает форму
        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Обновляет данные в dataGridView
        private void UpdateDataGridView()
        {
            _unitOfWork.Save();
            DataTable data = _unitOfWork.ClassesRepository.Get().ToList().ConvertToDataTable();
            dataGridView1.DataSource = data;
            dataGridView1.CurrentCell = dataGridView1[0, 0];
            dataGridView1.AutoSizeColumns();
        }

        //Обновляет данные в dataGridView при нажатии на кнопку
        private void Button3_Click(object sender, EventArgs e)
        {
            _unitOfWork.ClassesRepository.Insert(new Classes());

            UpdateDataGridView();
        }

        //Возвращает истину, если все условия прошли проверки успешно
        public bool CanSave()
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (string.IsNullOrEmpty(dataGridView1["Name", dataGridView1.Rows[i].Index].Value.ToString()))
                {
                    MessageBox.Show($"В строке {dataGridView1.Rows[i].Index} у столбца Name пустая строка, пожалуйста заполните её");
                    return false;
                };
                if (string.IsNullOrEmpty(dataGridView1["Type", dataGridView1.Rows[i].Index].Value.ToString()))
                {
                    MessageBox.Show($"В строке {dataGridView1.Rows[i].Index} у столбца Type пустая строка, пожалуйста заполните её");
                    return false;
                };


            }
            return true;
        }

        //Сохраняет сущность в таблице БД
        public void Save()
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                Classes currentClass = _unitOfWork.ClassesRepository.GetByID(Convert.ToInt32(dataGridView1["Id", dataGridView1.Rows[i].Index].Value));
                currentClass.Name = dataGridView1["Name", dataGridView1.Rows[i].Index].Value.ToString();
                currentClass.Type = dataGridView1["Type", dataGridView1.Rows[i].Index].Value.ToString();
            }
        }

        //Удаляет сущность из таблицы в БД
        public void Delete()
        {
            if (dataGridView1.CurrentRow != null)
            {
                if ((dataGridView1.CurrentRow.Index >= 0 && dataGridView1.CurrentRow.Index != dataGridView1.RowCount - 1))
                {
                    _unitOfWork.ClassesRepository.Delete(Convert.ToInt32(dataGridView1["Id", dataGridView1.CurrentRow.Index].Value));
                    UpdateDataGridView();
                }
            }
        }

        //Вызывается после закрытия формы
        private void ClassesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _unitOfWork.Dispose();
        }

        //Удаляет данные из dataGridView 
        private void Button4_Click(object sender, EventArgs e)
        {
            Delete();
        }

        //Вызывается при закрытии формы
        private void ClassesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
