using PMPK.DAL;
using PMPK.Models;
using PMPK.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PMPK
{
    public partial class ChildrenForm : Form
    {
        private UnitOfWork _unitOfWork;
        private ChildForm _childAdd;
        private PrintForm _printForm;
        private int _row = 0;
        private DataView _data;
        private List<string> _query;
        List<Children> _children;
        public ChildrenForm()
        {

            InitializeComponent();
            _unitOfWork = new UnitOfWork();
            _children = _unitOfWork.ChildrenRepository.Get(includeProperties: "PMPKs,Documents,Parents").ToList();
            dataGridView1.CellClick += DataGridView1_CellClick;
            dataGridView1.CellMouseDoubleClick += DataGridView1_CellMouseDoubleClick;

            DataTable data = _unitOfWork.ChildrenRepository.Get(includeProperties: "PMPKs").ConvertToDataTable();
            dataGridView1.DataSource = data;
            dataGridView1.CurrentCell = dataGridView1[0, _row];
            dataGridView1.Columns[0].Frozen = true;
            dataGridView1.AutoSizeColumns();

            comboBox2.DataSource = _unitOfWork.ClassesRepository.Get(q => q.Name);
            comboBox3.DataSource = _unitOfWork.AProgramsRepository.Get(program => program.Name);

            comboBox2.MaxDropDownItems = _unitOfWork.ClassesRepository.Count() == 0 ? 1 : _unitOfWork.ClassesRepository.Count();
            comboBox3.MaxDropDownItems = _unitOfWork.AProgramsRepository.Count() == 0 ? 1 : _unitOfWork.AProgramsRepository.Count();

            comboBox2.SelectedItem = null;
            comboBox3.SelectedItem = null;

            DBEnd.Image = Properties.Resources.arrow;
            DBStart.Image = Properties.Resources.arrow_04;
            prevRow.Image = Properties.Resources.arrow_02;
            nextRow.Image = Properties.Resources.arrow_03;

            dataGridView1.RewriteColumnsTextOnJsonProperty("PMPK.Models.Children");
            UpdateChildrenList();
        }
       
        //Вызывается при двйоном клике мыши по данным в таблице
        private void DataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int indexRow = e.RowIndex;
            if (indexRow >= 0)
            {
                if ((_row >= 0 && _row != dataGridView1.RowCount - 1))
                {
                    if (Extensions.FormIsNotOpen(_childAdd))
                    {
                        _childAdd = new ChildForm(Convert.ToInt32(dataGridView1["Id", e.RowIndex].Value));
                        _childAdd.ShowDialog();
                        _childAdd.Dispose();

                    }
                }
            }
        }
        //Вызывается при клике мыши по данным в таблице
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != -1)
            {
                _row = dataGridView1.CurrentRow.Index;
            }
        }
       
        //Задает стартовую позицию в textbox
        private void StartPositionIndex(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate ()
            {
                ((MaskedTextBox)sender).Select(0, 0);
            });
        }
       
        //Получает результыт из datetime и присваивает их в textbox
        private void GetResult(MaskedTextBox textBox, DateTimePicker dateTime)
        {
            textBox.Text = dateTime.Value.ToString();
        }

        //Использует метод GetResult
        private void ForFilterDateB_ValueChanged(object sender, EventArgs e)
        {
            GetResult(filterDateB, ((DateTimePicker)sender));
        }
       
        //Использует метод GetResult
        private void ForFilterPMPKF_ValueChanged(object sender, EventArgs e)
        {
            GetResult(filterPMPKF, ((DateTimePicker)sender));
        }

        //Использует метод GetResult
        private void ForFilterPMPKT_ValueChanged(object sender, EventArgs e)
        {
            GetResult(filterPMPKT, ((DateTimePicker)sender));
        }
      
        //Предыдущая запись в таблице
        private void PrevRow_Click(object sender, EventArgs e)
        {
            if (_row < dataGridView1.RowCount)
            {
                if (_row != dataGridView1.RowCount - 1)
                {
                    dataGridView1.Rows[_row].Selected = false;
                    dataGridView1.Rows[++_row].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1[0, _row];
                }
            }
        }
       
        //Следущая запись в таблице
        private void NextRow_Click(object sender, EventArgs e)
        {
            if (_row >= 0)
            {
                if (_row != 0)
                {
                    dataGridView1.Rows[_row].Selected = false;
                    dataGridView1.Rows[--_row].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1[0, _row];
                }
            }
        }
       
        //Первая запись в таблице
        private void DBStart_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.CurrentCell = dataGridView1[0, 0];
                dataGridView1.Rows[0].Selected = true;
                _row = 0;
            }
        }
       
        //Последняя запись в таблице
        private void DBEnd_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.Rows.Count - 1];
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                _row = dataGridView1.Rows.Count - 1;
            }
        }
        //Добавляет строку в таблицу и БД
        private void AddRow_Click(object sender, EventArgs e)
        {
            if (Extensions.FormIsNotOpen((_childAdd)))
            {
                Children child = new Children()
                {
                    Documents = new List<Document>()
                    {
                        new Document(){Passport = new Passport("Passport")},
                        new Document(){Passport = new Passport("Polis")},
                        new Document(){Passport = new Passport("Disability")},
                        new Document(){Passport = new Passport("Birth certificate")},
                    },
                    Parents = new List<Parent>()
                    {
                        new Parent(){ Passport = new Passport("Passport")},
                        new Parent(){ Passport = new Passport("Passport")},
                        new Parent(){ Passport = new Passport("Passport")},
                    }
                };
                _unitOfWork.ChildrenRepository.Insert(child);
                _unitOfWork.Save();
                _children = _unitOfWork.ChildrenRepository.Get(includeProperties: "PMPKs,Documents,Parents").ToList();
                _childAdd = new ChildForm(child.Id);
                _childAdd.ShowDialog();
                _childAdd.Dispose();
            }
        }
        //Удаляет строку из таблицы ибд
        private void DeleteRow_Click(object sender, EventArgs e)
        {
            if ((_row >= 0 && _row != dataGridView1.RowCount - 1))
            {
                int idChild = Convert.ToInt32(dataGridView1["Id", _row].Value.ToString());
                Children child = _children.ToList().FirstOrDefault(x => x.Id == idChild);
                if (MessageBox.Show("Вы действительно хотите удалить строку?", "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    child.Documents.ForEach(d =>
                    {
                        _unitOfWork.PassportRepository.Delete(_unitOfWork.DocumentsRepository.Get(includeProperties: "Passport").Where(currentDocument => currentDocument.Id == d.Id).FirstOrDefault().Passport);
                    });
                    child.Parents.ForEach(p =>
                    {
                        _unitOfWork.PassportRepository.Delete(_unitOfWork.ParentsRepository.Get(includeProperties: "Passport").Where(currentParent => currentParent.Id == p.Id).FirstOrDefault().Passport);
                    });
                    _unitOfWork.ChildrenRepository.Delete(child);
                    _unitOfWork.Save();
                    UpdateChildrenList();
                    _row--;
                    if (_row == -1) _row = 0;
                    dataGridView1.CurrentCell = dataGridView1[0, _row];
                }
            }
        }
       
        //Выполняет поиск по базе через установленные фильтры
        private void SearchBtn_Click(object sender, EventArgs e)
        {
            _query = new List<string>();
            if (!string.IsNullOrEmpty(textBox1.Text)) { _query.Add($"FullName LIKE '%{textBox1.Text}%'"); }
            if (!string.IsNullOrEmpty(textBox2.Text)) { _query.Add($"Age = {Convert.ToInt32(textBox2.Text)}"); }
            if (!string.IsNullOrEmpty(comboBox1.Text)) { _query.Add($"Sex = '{Convert.ToChar(comboBox1.Text)}'"); }
            if (!string.IsNullOrEmpty(comboBox2.Text)) { _query.Add($"Class = '{comboBox2.Text}'"); }
            if (!string.IsNullOrEmpty(comboBox3.Text)) { _query.Add($"Программа = '{comboBox3.Text}'"); }
            if (!string.IsNullOrEmpty(filterPMPKF.Text) && !string.IsNullOrEmpty(filterPMPKT.Text))
            {
                _query.Add($"[Дата следующего ПМПК] >= '{filterPMPKF.Text.Mask("##.##.####")}' AND [Дата следующего ПМПК] <= '{filterPMPKT.Text.Mask("##.##.####")}'");
            }
            if (checkControl.Checked) _query.Add($"Особый контроль = '{Convert.ToInt32(checkControl.Checked)}'");
            if (_gia9.Checked) _query.Add($"ГИА 9 = '{Convert.ToInt32(_gia9.Checked)}'");
            if (_gia11.Checked) _query.Add($"ГИА 11 = '{Convert.ToInt32(_gia11.Checked)}'");
            if (_perv.Checked) _query.Add($"Особый контроль = '{Convert.ToInt32(_perv.Checked)}'");
            if (checkMalIm.Checked) _query.Add($"MalIm = '{Convert.ToInt32(checkMalIm.Checked)}'");
            if (checkInvalid.Checked) _query.Add($"Инвалид = '{Convert.ToInt32(checkInvalid.Checked)}'");
            if (checkMnogodet.Checked) _query.Add($"Mnogodet = '{Convert.ToInt32(checkMnogodet.Checked)}'");
            if (checkMSE.Checked) _query.Add($"МСЭ = '{Convert.ToInt32(checkMSE.Checked)}'");
            if (checkOVZ.Checked) _query.Add($"ОВЗ = '{Convert.ToInt32(checkOVZ.Checked)}'");
            if (checkSirota.Checked) _query.Add($"Sirota = '{Convert.ToInt32(checkSirota.Checked)}'");
            _data = (dataGridView1.DataSource as DataTable).DefaultView;
            _data.RowFilter = string.Join(" AND ", _query).Trim();
            dataGridView1.DataSource = _data.ToTable();
        }
       
        //Чистит фильтр для поиска
        private void ClearFilterBtn_Click(object sender, EventArgs e)
        {
            if (_data != null)
            {
                _data = (dataGridView1.DataSource as DataTable).DefaultView;
                _data.RowFilter = string.Empty;
            }
            if (_query != null) _query.Clear();
            UpdateChildrenList();
        }
       
        //Обновляет данные контекста из репозитория
        private void UpdateChildrenRepository()
        {

            foreach (var child in _unitOfWork.ChildrenRepository.Get().ToList())
            {
                _unitOfWork.ChildrenRepository.Update(child);
                _unitOfWork.ChildrenRepository.Reload(child);
                foreach (var pmpk in child.PMPKs.ToList())
                {
                    _unitOfWork.PMPKsRepository.Update(pmpk);
                    _unitOfWork.PMPKsRepository.Reload(pmpk);
                }
            }
            _unitOfWork.Save();
            _children = _unitOfWork.ChildrenRepository.Get(includeProperties: "PMPKs,Documents,Parents").ToList();
        }
       
        //Обновляет данные формы
        public void UpdateChildrenList()
        {
            UpdateChildrenRepository();

            dataGridView1.DataSource = _children.ConvertToDataTable();

            (dataGridView1.DataSource as DataTable).Columns.Remove("Documents");
            (dataGridView1.DataSource as DataTable).Columns.Remove("PMPKs");
            (dataGridView1.DataSource as DataTable).Columns.Remove("Parents");
            dataGridView1.AddColumns("ОВЗ", "№ протокола", "Адрес", "Дата ПМПК", "Дата следующего ПМПК", "Особый контроль", "Инвалид", "Первично", "Программа", "МСЭ", "ГИА 9", "ГИА 11");

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {

                dataGridView1["DateOfBirth", i].Value = dataGridView1["DateOfBirth", i].Value.ToString().Split(" ")[0];
                dataGridView1["ОВЗ", i].Value = _children[i].PMPKs.LastOrDefault()?.OVZ;
                dataGridView1["№ протокола", i].Value = _children[i]?.PMPKs?.LastOrDefault()?.NumberOfProtocol ?? 0;
                dataGridView1["Дата ПМПК", i].Value = Convert.ToString(_children[i].PMPKs.LastOrDefault()?.DateOfPMPK.ToString().Split(" ")[0]) ?? string.Empty;
                dataGridView1["Дата следующего ПМПК", i].Value = Convert.ToString(_children[i].PMPKs.LastOrDefault()?.DateOfNextPMPK.ToString().Split(" ")[0]) ?? string.Empty;
                dataGridView1["Инвалид", i].Value = _children[i].PMPKs.LastOrDefault()?.Invalid;
                dataGridView1["Особый контроль", i].Value = _children[i].PMPKs.LastOrDefault()?.Control;
                dataGridView1["Программа", i].Value = _children[i].PMPKs.LastOrDefault()?.AProgram ?? string.Empty;
                dataGridView1["МСЭ", i].Value = _children[i].PMPKs.LastOrDefault()?.MSE;
                dataGridView1["ГИА 9", i].Value = _children[i].PMPKs.LastOrDefault()?.GIA9;
                dataGridView1["ГИА 11", i].Value = _children[i].PMPKs.LastOrDefault()?.GIA11;
                dataGridView1["Первично", i].Value = _children[i].PMPKs.LastOrDefault()?.FirstPriem;
                dataGridView1["Адрес", i].Value = _children[i].PMPKs.LastOrDefault()?.Address ?? string.Empty;
            }
        }
       
        //вызывается при закрытии формы
        private void ChildrenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _unitOfWork.Dispose();
        }
       
        //Экспорт данных из БД в эксель
        private void Export_Click(object sender, EventArgs e)
        {
            UpdateChildrenRepository();
            Excel.Application excel = new Excel.Application();
            Excel.Workbook w = excel.Workbooks.Open(@$"{Environment.CurrentDirectory}\shablon.xls");
            Excel.Worksheet q = w.Sheets[1];
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                var child = _children.FirstOrDefault(x => x.Id == Convert.ToInt32(dataGridView1["Id", i].Value));
                var PMPK = child?.PMPKs?.LastOrDefault();
                q.Cells[i + 4, 1] = (i + 1).ToString();
                if (PMPK?.NumberOfProtocol != null)
                {
                    q.Cells[i + 4, 2] = dataGridView1["№ протокола", i].Value;
                }
                q.Cells[i + 4, 3] = dataGridView1["Дата ПМПК", i].Value.ToString();
                q.Cells[i + 4, 4] = dataGridView1["FullName", i].Value.ToString().Split(" ")[0];
                q.Cells[i + 4, 5] = dataGridView1["FullName", i].Value.ToString().Split(" ")[1];
                q.Cells[i + 4, 6] = dataGridView1["FullName", i].Value.ToString().Split(" ")[2];
                q.Cells[i + 4, 7] = dataGridView1["Sex", i].Value;
                q.Cells[i + 4, 8] = dataGridView1["DateOfBirth", i].Value;
                q.Cells[i + 4, 9] = dataGridView1["Age", i].Value;
                q.Cells[i + 4, 13] = dataGridView1["Адрес", i].Value;
                if (PMPK?.WhereStuding != null)
                {
                    q.Cells[i + 4, 14] = PMPK?.WhereStuding == "на дому" ? 1 : 0;
                }
                q.Cells[i + 4, 15] = PMPK?.WhereStuding;
                q.Cells[i + 4, 18] = PMPK?.LocationOfEventPMPK;
                q.Cells[i + 4, 35] = PMPK?.AProgram;
                Excel.Range r = q.get_Range($"O{i + 4}", $"Q{i + 4}");
                r.Merge(true);
                r.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                q.Cells[i + 4, 19] = PMPK?.Programm;
                q.Cells[i + 4, 20] = PMPK?.MSE;
                q.Cells[i + 4, 21] = PMPK?.GIA9;
                if (PMPK?.FirstPriem != null)
                {
                    if (PMPK?.FirstPriem == 1)
                    {
                        q.Cells[i + 4, 23] = 1;
                    }
                    else { q.Cells[i + 4, 24] = 1; }
                }

                q.Cells[i + 4, 25] = PMPK?.CommissionWithdrawal;
                q.Cells[i + 4, 26] = PMPK?.OVZ;
                q.Cells[i + 4, 27] = PMPK?.Invalid;
                if (Convert.ToInt32(dataGridView1["Age", i].Value) <= 6)
                {
                    q.Cells[i + 4, 28] = 1;
                }
            }
            q.get_Range("A3", "AN3").AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            Excel.Range v = q.get_Range($"D2", $"F3");
            v.Merge(true);
            v.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            string fileName = String.Empty;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "excel files (*.xls)|*.xls|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
                w.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                w.Close(0);
                excel.Quit();
            }
            else 
            {
                w.Close(0);
                excel.Quit();
            }    
            saveFileDialog1.Dispose();
        }
       
        //Импорт всех историй
        private void Import_Click(object sender, EventArgs e)
        {
            UpdateChildrenList();
            string fileName = String.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "excel files (*.xls)|*.xls|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
                Excel.Application excel = new Excel.Application();
                Excel.Workbook workbook = excel.Workbooks.Open(fileName);
                Excel.Worksheet wokrsheet = workbook.Sheets[1];
                for (int i = 4; i <= wokrsheet.UsedRange.Rows.Count; i++)
                {
                    var child = _children.FirstOrDefault(x => x.FullName == $"{(wokrsheet.Cells[i, 4] as Excel.Range).Value2} {(wokrsheet.Cells[i, 5] as Excel.Range).Value2} {(wokrsheet.Cells[i, 6] as Excel.Range).Value2}");
                    ImportPMPK(child, wokrsheet, i);

                }

                UpdateChildrenList();
                workbook.Close(0);
                excel.Quit();
            }
           
            openFileDialog.Dispose();
        }
       
        //Импорт истории из эксель в БД
        private void ImportPMPK(Children child, Excel.Worksheet worksheet, int rowIndex)
        {
            if ((worksheet.Cells[rowIndex, 2] as Excel.Range).Value2 != null)
            {
                if (child != null)
                {

                    var pmpk = child.PMPKs.FirstOrDefault(x => x.NumberOfProtocol == Convert.ToInt32((worksheet.Cells[rowIndex, 2] as Excel.Range).Value2));
                    if (pmpk != null)
                    {

                        if ((worksheet.Cells[rowIndex, 23] as Excel.Range).Value2 == 1)
                        {


                            pmpk.Address = Convert.ToString((worksheet.Cells[rowIndex, 13] as Excel.Range).Value2);
                            pmpk.DateOfPMPK = Program.StringToDateTime(Convert.ToString((string)(worksheet.Cells[rowIndex, 3] as Excel.Range).Value2).Replace(".", string.Empty).Trim());
                            pmpk.CommissionWithdrawal = Convert.ToString((worksheet.Cells[rowIndex, 25] as Excel.Range).Value2);
                            pmpk.NumberOfProtocol = Convert.ToInt32((worksheet.Cells[rowIndex, 2] as Excel.Range).Value2);
                            pmpk.WhereStuding = Convert.ToString((worksheet.Cells[rowIndex, 15] as Excel.Range).Value2);
                            pmpk.Programm = Convert.ToByte((worksheet.Cells[rowIndex, 19] as Excel.Range).Value2);
                            pmpk.MSE = Convert.ToByte((worksheet.Cells[rowIndex, 20] as Excel.Range).Value2);
                            pmpk.GIA9 = Convert.ToByte((worksheet.Cells[rowIndex, 21] as Excel.Range).Value2);
                            pmpk.OVZ = Convert.ToByte((worksheet.Cells[rowIndex, 26] as Excel.Range).Value2);
                            pmpk.Invalid = Convert.ToByte((worksheet.Cells[rowIndex, 27] as Excel.Range).Value2);
                            pmpk.AProgram = Convert.ToString((worksheet.Cells[rowIndex, 35] as Excel.Range).Value2);
                            pmpk.FirstPriem = Convert.ToByte(1);

                        }
                        else
                        {
                            if (pmpk.FirstPriem != 1)
                            {
                                AddPMPK(child, worksheet, rowIndex);
                                AddPMPK(child, worksheet, rowIndex, 0, true);
                            }
                            else
                            {
                                AddPMPK(child, worksheet, rowIndex, 0, true);
                            }
                        }
                    }
                    else
                    {
                        AddPMPK(child, worksheet, rowIndex);
                    }
                }
                else
                {
                    Children newChild = new Children()
                    {
                        FullName = $"{(worksheet.Cells[rowIndex, 4] as Excel.Range).Value2} {(worksheet.Cells[rowIndex, 5] as Excel.Range).Value2} {(worksheet.Cells[rowIndex, 6] as Excel.Range).Value2}",
                        DateOfBirth = DateTime.Parse((worksheet.Cells[rowIndex, 8] as Excel.Range).Value2),
                        Sex = Convert.ToChar((worksheet.Cells[rowIndex, 7] as Excel.Range).Value2),
                        Age = Convert.ToInt32((worksheet.Cells[rowIndex, 9] as Excel.Range).Value2),
                        Documents = new List<Document>()
                        {
                            new Document(){Passport = new Passport("Passport")},
                            new Document(){Passport = new Passport("Polis")},
                            new Document(){Passport = new Passport("Disability")},
                            new Document(){Passport = new Passport("Birth certificate")},
                        },
                        Parents = new List<Parent>()
                        {
                            new Parent(){ Passport = new Passport("Passport")},
                            new Parent(){ Passport = new Passport("Passport")},
                            new Parent(){ Passport = new Passport("Passport")},
                        },
                        PMPKs = new List<Models.PMPK>(),

                    };
                    if ((worksheet.Cells[rowIndex, 23] as Excel.Range).Value2 == 1)
                    {
                        AddPMPK(newChild, worksheet, rowIndex);
                    }
                    else
                    {
                        AddPMPK(newChild, worksheet, rowIndex);
                        AddPMPK(newChild, worksheet, rowIndex, 0, true);
                    }
                    _unitOfWork.ChildrenRepository.Insert(newChild);

                }
                UpdateChildrenRepository();
            }
        }
        
        //Добавить сущность в БД
        private void AddPMPK(Children child, Excel.Worksheet worksheet, int rowIndex, int firstPMPK = 1, bool autoNumberOfProtocol = false)
        {
            int numberOfProtocol;
            try
            {
                if (!autoNumberOfProtocol)
                {
                    numberOfProtocol = Convert.ToInt32((worksheet.Cells[rowIndex, 2] as Excel.Range).Value2);
                }
                else
                {
                    numberOfProtocol = Convert.ToInt32((worksheet.Cells[rowIndex, 2] as Excel.Range).Value2) + new Random().Next();
                }
                child.PMPKs.Add(new Models.PMPK()
                {
                    Address = Convert.ToString((worksheet.Cells[rowIndex, 13] as Excel.Range).Value2),
                    DateOfPMPK = Program.StringToDateTime(Convert.ToString((string)(worksheet.Cells[rowIndex, 3] as Excel.Range).Value2).Replace(".", string.Empty).Trim()),
                    CommissionWithdrawal = Convert.ToString((worksheet.Cells[rowIndex, 25] as Excel.Range).Value2),
                    NumberOfProtocol = numberOfProtocol,
                    WhereStuding = Convert.ToString((worksheet.Cells[rowIndex, 15] as Excel.Range).Value2),
                    Programm = Convert.ToByte((worksheet.Cells[rowIndex, 19] as Excel.Range).Value2),
                    MSE = Convert.ToByte((worksheet.Cells[rowIndex, 20] as Excel.Range).Value2),
                    GIA9 = Convert.ToByte((worksheet.Cells[rowIndex, 21] as Excel.Range).Value2),
                    OVZ = Convert.ToByte((worksheet.Cells[rowIndex, 26] as Excel.Range).Value2),
                    Invalid = Convert.ToByte((worksheet.Cells[rowIndex, 27] as Excel.Range).Value2),
                    AProgram = Convert.ToString((worksheet.Cells[rowIndex, 35] as Excel.Range).Value2),
                    FirstPriem = Convert.ToByte(firstPMPK),

                });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show("Одна из ячеек имеет неверный формат или является пустой!");
            }

        }

        //Вывод шаблона на печать
        private void Print_Button_Click(object sender, EventArgs e)
        {
            if ((_row >= 0 && _row != dataGridView1.RowCount - 1))
            {
                if (Extensions.FormIsNotOpen((_printForm)))
                {
                    _printForm = new PrintForm(Convert.ToInt32(dataGridView1["Id", _row].Value));
                    _printForm.ShowDialog();
                    _printForm.Dispose();
                }
            }
           
        }
    }

}
