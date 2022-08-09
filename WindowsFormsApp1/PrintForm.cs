using Microsoft.Office.Interop.Word;
using PMPK.DAL;
using PMPK.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace PMPK
{
    public partial class PrintForm : Form
    {
        private UnitOfWork _unitOfWork;
        private Children _child;
        private Word._Application _oWord = new Word.Application();
        private Dictionary<string, string> _dataForTemplate = new Dictionary<string, string>();
        public PrintForm(int id)
        {
            InitializeComponent();
            _unitOfWork = new UnitOfWork();
            _child = _unitOfWork.ChildrenRepository.Get(includeProperties: "PMPKs,Documents,Parents").ToList().FirstOrDefault(x => x.Id == id);
            comboBox1.DataSource = Program.templateFiles.Keys.ToList();      
            FillDictionaryDataOfEntity(_child);
            FillDictionaryDataOfEntity(_child.PMPKs.Last());
            //foreach (var item in _child.Parents)
            //{
            //    FillDictionaryDataOfEntity(item);
            //    FillDictionaryDataOfEntity(item.Passport);             
            //}
            //foreach (var item in _child.Documents)
            //{
            //    FillDictionaryDataOfEntity(item);
            //}
        }
        private void FillDictionaryDataOfEntity<TEntity>(TEntity entity) where TEntity : class 
        {
            Type typeEntity = entity.GetType();
            foreach (var item in typeEntity.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {             
                if(item.Name != "Id") 
                {
                   
                    _dataForTemplate.Add(item.Name, item.GetValue(entity)?.ToString());
                }
            }
          
        }
        private _Document GetDoc(string path)
        {
            _Document oDoc = _oWord.Documents.Add(path);
            SetTemplate(oDoc);
            return oDoc;
        }
        private void SetTemplate(Word._Document oDoc)
        {
          
            foreach (var item in _dataForTemplate)
            {
                if (oDoc.Bookmarks.Exists(item.Key)) 
                {
                    oDoc.Bookmarks[item.Key].Range.Text = item.Value;
                }
            }
        }

        private void PrintForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void Print_Click(object sender, EventArgs e)
        {
            string fileName = String.Empty;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "excel files (*.doc)|*.doc|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
                
                _Document oDoc = GetDoc(Program.templateFiles[comboBox1.Text]);
                oDoc.SaveAs(FileName: fileName);
                oDoc.Close();  
            }
            saveFileDialog1.Dispose();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            
            this.Close();
            this.Dispose();
        }
    }
}
