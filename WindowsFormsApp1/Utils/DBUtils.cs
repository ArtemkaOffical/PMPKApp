using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace PMPK
{
    public static class DBUtils
    {
        //Метод для перезаписи наименования колонок, если существует атриббут JsonProperty(Name)
        public static void RewriteColumnsTextOnJsonProperty(this DataGridView dataGridView, string type)
        {
            Type childrenType = Type.GetType(type);

            foreach (DataGridViewColumn item in dataGridView.Columns)
            {
                var property = childrenType.GetProperty(item.HeaderText).GetCustomAttributes(typeof(JsonPropertyNameAttribute), true);
                if (property.Length != 0)
                {
                    item.HeaderText = (property[0] as JsonPropertyNameAttribute).Name;
                }
            }

        }

        //Метод для получения строки подключения из файла конфигурации
        public static string GetConnectionString()
        {
            if (Program.data.TryGetKey("DataBase.StringConnection", out string value))
            {
                return value;
            }
            else
            {
                Program.data["DataBase"]["StringConnection"] = value;
                Program.parser.WriteFile("config.ini", Program.data);
                return value;
            }
        }

        //Метод для восстановления БД из файла
        public static void RestoreBD()
        {
            string directory = string.Empty;
            string database = GetConnectionString().Split(";")[1].Split("=")[1];
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "bak files (*.bak)|*.bak|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                directory = openFileDialog1.FileName.ToString();
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    try
                    {
                        string sqlStmt2 = string.Format("ALTER DATABASE [" + database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                        SqlCommand bu2 = new SqlCommand(sqlStmt2, con);
                        bu2.ExecuteNonQuery();

                        string sqlStmt3 = "USE MASTER RESTORE DATABASE [" + database + "] FROM DISK='" + directory + "'WITH REPLACE;";
                        SqlCommand bu3 = new SqlCommand(sqlStmt3, con);
                        bu3.ExecuteNonQuery();

                        string sqlStmt4 = string.Format("ALTER DATABASE [" + database + "] SET MULTI_USER");
                        SqlCommand bu4 = new SqlCommand(sqlStmt4, con);
                        bu4.ExecuteNonQuery();

                        MessageBox.Show("database restoration done successefully");
                        con.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                MessageBox.Show("БД была восстановленна из файла");
            }
           
            openFileDialog1.Dispose();
           
        }

        //Метод для архивации БД в файл
        public static void BackUpDB()
        {
            string directory = String.Empty;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "bak files (*.bak)|*.bak|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                directory = saveFileDialog1.FileName;
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    string cmd = "BACKUP DATABASE [" + GetConnectionString().Split(";")[1].Split("=")[1] + "] TO DISK='" + directory + "'";
                    using (SqlCommand command = new SqlCommand(cmd, con))
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }
                        command.ExecuteNonQuery();
                        con.Close();
                    }
                }
                MessageBox.Show("Была создана резервная копия БД");
            }      
            saveFileDialog1.Dispose();   
        }

        //Поиск имен sql server на пк
        public static IEnumerable<string> ListLocalSqlInstances()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                using (var hive = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    foreach (string item in ListLocalSqlInstances(hive))
                    {
                        yield return item;
                    }
                }

                using (var hive = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                {
                    foreach (string item in ListLocalSqlInstances(hive))
                    {
                        yield return item;
                    }
                }
            }
            else
            {
                foreach (string item in ListLocalSqlInstances(Registry.LocalMachine))
                {
                    yield return item;
                }
            }
        }

        //Поиск имен sql server на пк
        private static IEnumerable<string> ListLocalSqlInstances(RegistryKey hive)
        {
            const string keyName = @"Software\Microsoft\Microsoft SQL Server";
            const string valueName = "InstalledInstances";
            const string defaultName = "MSSQLSERVER";

            using (var key = hive.OpenSubKey(keyName, false))
            {
                if (key == null) return Enumerable.Empty<string>();

                var value = key.GetValue(valueName) as string[];
                if (value == null) return Enumerable.Empty<string>();

                for (int index = 0; index < value.Length; index++)
                {
                    if (string.Equals(value[index], defaultName, StringComparison.OrdinalIgnoreCase))
                    {
                        value[index] = ".";
                    }
                    else
                    {
                        value[index] = @".\" + value[index];
                    }
                }

                return value;
            }
        }
    }
}
