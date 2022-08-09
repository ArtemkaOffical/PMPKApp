using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PMPK.Utils
{
    static class Extensions
    {
        //Возвращает истину , если форма не открыта
        public static bool FormIsNotOpen(Form form)
        {
            if (form == null || form.IsDisposed)
            {
                return true;
            }
            return false;
        }

        //метод позволяет гарантированно открыть только одну форму, применяется только для форм унаследованных от клсса Form
        public static void ShowForm<T>(ref T form, bool showDialog = true, Form mdiParent = null) where T : Form, new()
        {
            if (FormIsNotOpen(form))
            {
                form = new T();
                form.MdiParent = mdiParent;
                if (showDialog)
                {
                    form.ShowDialog();
                }
                else
                {
                    form.Show();
                }
            }
        }

        //метод для вызова одного и того же действия для дочерних контроллеров и главного контролера
        public static void ForAllControls(this Control parent, Action<Control> action)
        {
            foreach (Control c in parent.Controls)
            {
                action(c);
                ForAllControls(c, action);
            }
        }

        //возвращает значение строки по заданной маске
        public static string Mask(this string value, string mask, char substituteChar = '#')
        {
            int valueIndex = 0;
            try
            {
                return new string(mask.Select(maskChar => maskChar == substituteChar ? value[valueIndex++] : maskChar).ToArray());
            }
            catch (IndexOutOfRangeException e)
            {
                throw new Exception("Value too short to substitute all substitute characters in the mask", e);
            }
        }

       //Метод позволяет добавить колонки для DataGridView
        public static void AddColumns(this DataGridView dataGridView, params string[] columnNames)
        {

            foreach (var item in columnNames)
            {
                (dataGridView.DataSource as DataTable).Columns.Add(item, typeof(string));
            }

        }

       //Авто-размер для колонок по всей форме
        public static void AutoSizeColumns(this DataGridView targetGrid)
        {
            if (targetGrid.Columns.Count < 1)
                return;
            var gridTable = new DataTable();
            gridTable = (DataTable)targetGrid.DataSource;
            targetGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            using (var gfx = targetGrid.CreateGraphics())
            {

                for (int i = 0; i < gridTable.Columns.Count; i++)
                {
                    string[] colStringCollection = gridTable.AsEnumerable().Where(r => r.Field<object>(i) != null).Select(r => r.Field<object>(i).ToString()).ToArray();
                    colStringCollection = colStringCollection.OrderBy((x) => x.Length).ToArray();

                    if (colStringCollection.Length > 0)
                    {
   
                        string longestColString = colStringCollection.Last();
                        var colWidth = gfx.MeasureString(longestColString, targetGrid.Font);
                        if (colWidth.Width > targetGrid.Columns[i].HeaderCell.Size.Width)
                        {
                            targetGrid.Columns[i].Width = (int)colWidth.Width;
                        }
                        else
                        {
                            targetGrid.Columns[i].Width = targetGrid.Columns[i].HeaderCell.Size.Width;
                        }
                    }
                    else
                    {
                        targetGrid.Columns[i].Width = targetGrid.Columns[i].HeaderCell.Size.Width;
                    }
                }

            }
        }     

        //Преобразует лист данных в DataTable
        public static DataTable ConvertToDataTable<T>(this IEnumerable<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(typeof(string)) ?? typeof(string));
            }

            return table;
        }
    }
}
