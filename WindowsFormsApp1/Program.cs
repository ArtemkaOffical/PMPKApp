using PMPK.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace PMPK
{
    static class Program
    {
        public static IniParser.FileIniDataParser parser = new IniParser.FileIniDataParser();
        public static IniParser.Model.IniData data;
        private static ConnectDataBaseForm _connectToDataBase;
        private static FileSystemWatcher _fileSystemWatcher = new FileSystemWatcher(Environment.CurrentDirectory+ "\\templates", "*.doc*");
        public static Dictionary<string, string> templateFiles = new Dictionary<string, string>();
        //Точка входа в приложение и настройка некоторых параметров
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!Directory.Exists(Environment.CurrentDirectory + "\\templates"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\templates");
            }
            if (!File.Exists(Environment.CurrentDirectory + "\\config.ini"))
            {
                File.Create(Environment.CurrentDirectory + "\\config.ini").Close();
            }
            if (!File.Exists(Environment.CurrentDirectory + "\\shablon.xls"))
            {
                File.WriteAllBytes(Environment.CurrentDirectory + "\\shablon.xls", Properties.Resources.Shablon);
            }
            data = parser.ReadFile("config.ini");
            if (string.IsNullOrEmpty(DBUtils.GetConnectionString()))
            {
                Extensions.ShowForm(ref _connectToDataBase);
            }
            
            _fileSystemWatcher.EnableRaisingEvents = true;
            _fileSystemWatcher.Created += _fileSystemWatcher_Created;
            _fileSystemWatcher.Deleted += _fileSystemWatcher_Deleted;
            _fileSystemWatcher.Renamed += _fileSystemWatcher_Renamed;
            Application.Run(new MainForm());
        }
        //Изменяем словрь шаблонов, если файл был переименован в папке с шаблонами
        private static void _fileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (templateFiles.ContainsKey(e.OldName))
            {
                templateFiles.Remove(e.OldName);
                templateFiles.Add(e.Name,e.FullPath);
            }
        }
        //Изменяем словрь шаблонов, если файл был удален из папки с шаблонами
        private static void _fileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (templateFiles.ContainsKey(e.Name))
            {
                templateFiles.Remove(e.Name);
            }
        }
        //Изменяем словрь шаблонов, если появился новый файл в папке с шаблонами
        private static void _fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (!templateFiles.ContainsKey(e.Name)) 
            {
                templateFiles.Add(e.Name, e.FullPath);
            }
        }
        //Перевод из строки в DateTime
        public static DateTime? StringToDateTime(string s)
        {
            if (s.Length == 0 || string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
            {
                return null;
            }
            else
            {
                return DateTime.ParseExact(s, "ddMMyyyy", null);
            }
        }
    }
}
