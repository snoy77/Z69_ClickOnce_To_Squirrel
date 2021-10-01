using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;
using System.Windows.Forms;

namespace Z69_ClickOnceReplacer
{
    public class ClickOnceReplacer
    {
        public string currentUserAppDataLocalFolder
        {
            get
            {
                return Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
                    .ToString() + @"\Local";
            }
        }

        public string AppName;
        public string SquirrelSetupPath;
        public bool DoAddFilesIntoRegistry;
        public bool CreateDataAppFolder;

        private string dataAppFolderName;
        private RegistryKey appRegistryKey;

        public Dictionary<string, string> FilesForMove = new Dictionary<string, string>(); // <string FilePath, string NewPath>
        public List<string> FilesForMoveJust = new List<string>(); // <string FilePath>. Эти файлы будут просто перемещены в новую папку AppData\Local\[appName]_Data
        public List<string> FilesAddToReestr = new List<string>(); // <string FilePath>

        //Делегат для подписи дополнительной логики до установки
        public delegate void SomethingLogicBeforeSetup();
        public SomethingLogicBeforeSetup somethingLogicBeforeSetup;

        public delegate void SomethingLogicAfterSetup();
        public SomethingLogicAfterSetup somethingLogicAfterSetup;

        public ClickOnceReplacer(string appName, string squirrelSetupPath, bool doAddFilesIntoRegistry, bool createDataAppFolder)
        {
            this.AppName = appName;
            this.SquirrelSetupPath = squirrelSetupPath;
            this.DoAddFilesIntoRegistry = doAddFilesIntoRegistry;
            this.CreateDataAppFolder = createDataAppFolder;
        }
        public void StartReplaceProcces()
        {
            //Производит замену технологии
                // - AppName - имя приложения. Понадобиться если нужно будет создавать реестр
                    //для приложения, поэтому желательно соотносить с именем приложения в Squirrel версии приложения,
                    //чтобы они к одному ключу обращались


            Console.WriteLine("--------------------Начало процедуры замены ClickOnce на Squirrel.Windows--------------------");
            if (this.CreateDataAppFolder)
            {
               Console.WriteLine($"- StartReplaceProcces: Создание папки приложения...");
                this.dataAppFolderName = this.currentUserAppDataLocalFolder + $"\\{this.AppName}_Data";
                Directory.CreateDirectory(this.dataAppFolderName);
                Console.WriteLine($"- StartReplaceProcces: directoryName: {dataAppFolderName}, DirectoryEx: {Directory.Exists(dataAppFolderName)}");
            }
            if (this.FilesForMove.Count != 0 || this.FilesForMoveJust.Count != 0)
            { 
                this.MoveFiles(this.DoAddFilesIntoRegistry);
            }

            if (this.DoAddFilesIntoRegistry && this.FilesAddToReestr.Count != 0)
            {
                this.CreateAppKeyInRegistry(this.AppName);
                this.AddFilesPathToRegistry();
            }


            //Сама установка Squirrel-варианта приложения
            if(this.somethingLogicBeforeSetup!= null)
                this.somethingLogicBeforeSetup();

            this.SetupSqurrelVariant(this.SquirrelSetupPath);
            
            if (this.somethingLogicAfterSetup != null)
                this.somethingLogicAfterSetup();
            
            //Было круто бы тут добавить проверочку на существоание приложения в Апдате юзера...

            //Удаление ярлыка appref-ms, и желательного самого того прилоежния. Но про приложение считаю достаточно опасно, а вот ярлык нормально

            Console.WriteLine("--------------------Замена ClickOnce на Squirrel.Windows завершено--------------------");

        }

        public void SetupSqurrelVariant(string squirrelSetupPath)
        {
            Console.WriteLine("Установка Squirrel-варианта приложения...");

            //---------------------------------------------------------------------------
            //@Добавить логику проверки на конец пути, чтобы там было Releases\Setup.exe@
            //Но надо улучшить

            //Система от дураков всё портит

            //if (squirrelSetupPath.EndsWith(@"\Releases"))
            //{
            //    squirrelSetupPath += @"\Setup.exe";
            //}
            //else if (squirrelSetupPath.EndsWith(@"\Releases\"))
            //{
            //    squirrelSetupPath += @"Setup.exe";
            //}
            //else if (!squirrelSetupPath.EndsWith(@"\Releases"))
            //{
            //    squirrelSetupPath += @"\Releases\Setup.exe";
            //}
            //---------------------------------------------------------------------------

            Console.WriteLine($"Вот тут стартует: {squirrelSetupPath}");
            Process.Start(squirrelSetupPath).WaitForExit();
        }

        public void MoveFiles(bool AddFilesIntoReestr)
        {
            //Метод перетаскивания файлов в новый путь
            Console.WriteLine("Процедура перемещения отмеченных файлов...");


            foreach (var file in FilesForMove)
            {
                Console.WriteLine($"-- File: {file.Key} to {file.Value}...");
                if (!File.Exists(file.Value))
                {
                   
                    File.Copy(file.Key, file.Value);
                }
                else
                {
                    Console.WriteLine("Файл в конечном пути уже существует!");
                }
            }

            if (this.FilesForMoveJust.Count != 0 && this.dataAppFolderName != null)
            {
                string newPath;
                foreach (string file in this.FilesForMoveJust)
                {
                    newPath = this.dataAppFolderName + $"\\{Path.GetFileName(file)}";
                    Console.WriteLine($"-- File: {file} to {newPath}...");

                    if (!File.Exists(newPath))
                        File.Copy(file, newPath);
                    else
                        Console.WriteLine("Файл в конечном пути уже существует!");
                }
            }


            if (AddFilesIntoReestr)
            {
                //Здесь красивее сделать
                foreach (string newFilePath in FilesForMove.Values)
                {
                    this.FilesAddToReestr.Add(newFilePath);
                }

                foreach (string file in this.FilesForMoveJust)
                {
                    this.FilesAddToReestr.Add(this.dataAppFolderName + $"\\{Path.GetFileName(file)}");
                }
            }
        }

        //МЕТОДЫ РАБОТЫ С РЕЕСТРОМ
        public void CreateAppKeyInRegistry(string appName)
        {
            this.appRegistryKey = Registry.CurrentUser;
            this.appRegistryKey = this.appRegistryKey.CreateSubKey("SOFTWARE");
            this.appRegistryKey = this.appRegistryKey.CreateSubKey(this.AppName);
        }
        
        public void AddFilesPathToRegistry()
        {
            AddFilesPathToRegistry(this.FilesAddToReestr);
        }
        public void AddFilesPathToRegistry(List<string> filesName)
        {
            Console.WriteLine("Процедура добавления файлов в реестр...");

            foreach (string file in filesName)
            {
                this.AddFileToRegistry(file);
            }
        }
        private void AddFileToRegistry(string FilePath)
        {
            Console.WriteLine($"Добавление в реестр: Ключ:{Path.GetFileName(FilePath)}, Значение: {FilePath}");
            this.appRegistryKey.SetValue(Path.GetFileName(FilePath), FilePath);
        }
    }
}
