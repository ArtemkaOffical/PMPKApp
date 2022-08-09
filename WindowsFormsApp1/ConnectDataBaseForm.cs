using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace PMPK
{
    public partial class ConnectDataBaseForm : Form
    {
        public ConnectDataBaseForm()
        {
            InitializeComponent();
        }

        //Подключеник БД через данные из формы
        private void ConnectToDataBase_Click(object sender, EventArgs e)
        {
            string ServerDB = ServerDataBase.Text;
            string NameDB = NameDataBase.Text;
            Program.data["DataBase"]["StringConnection"] = @$"server={ServerDB}; DataBase={NameDB}; Trusted_Connection=True;";
            Program.parser.WriteFile("config.ini", Program.data);
          
            if (!string.IsNullOrEmpty(ServerDB) && !string.IsNullOrEmpty(NameDB))
            {
                using (SqlConnection connection = new SqlConnection(DBUtils.GetConnectionString()))
                {
                    try
                    {
                        connection.Open();
                        MessageBox.Show("Соединение прошло успешно", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connection.Close();
                        Close();
                    }
                    catch
                    {
                        MessageBox.Show("Не найдены запущенные SQL сервера. Обратитесь к разработчику.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        connection.Close();
                    }

                }
            }
            else
            {
                MessageBox.Show("Заполните поля выше", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Автоматическое подключение к БД
        private void AutoConnecetToDataBase_Click(object sender, EventArgs e)
        {

            var server = DBUtils.ListLocalSqlInstances().FirstOrDefault();
            using (SqlConnection connection = new SqlConnection(@$"server={server};Trusted_Connection=True;"))
            {
                try
                {
                    connection.Open();
                    if (!Database.Exists(@$"server={server}; DataBase=diplomDB; Trusted_Connection=True;"))
                    {
                        SqlCommand myCommand = new SqlCommand("create DATABASE diplomDB", connection);
                        myCommand.ExecuteNonQuery();
                        Program.data["DataBase"]["StringConnection"] = $"server={server}; DataBase=diplomDB; Trusted_Connection=True;";
                        
                        Program.parser.WriteFile("config.ini", Program.data);
                        MessageBox.Show("Автоматическое соединение прошло успешно", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        Program.data["DataBase"]["StringConnection"] = $"server={server}; DataBase=diplomDB; Trusted_Connection=True;";
                        Program.parser.WriteFile("config.ini", Program.data);
                    }

                    connection.Close();
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Не найдены запущенные SQL сервера или база данных уже существует.\nОбратитесь к разработчику.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    connection.Close();
                }
            }

        }
        
        //Вызывается при загрузке формы
        private void ConnectDataBaseForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(DBUtils.GetConnectionString()))
            {
                string url = DBUtils.GetConnectionString();

                ServerDataBase.Text = url?.Split(";")[0]?.Split("=")[1];
                NameDataBase.Text = url.Split(";")[1].Split("=")[1];
            }
        }
        
        //Вызывается при закрытии формы
        private void ConnectDataBaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
