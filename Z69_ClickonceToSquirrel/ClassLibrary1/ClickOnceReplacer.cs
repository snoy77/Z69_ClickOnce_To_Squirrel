using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;

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
        private RegistryKey registryKey = Registry.CurrentUser;
        private RegistryKey appRegistryKey;

        public Dictionary<string, string> FilesForMove = new Dictionary<string, string>(); // <string FilePath, string NewPath>
        public List<string> FilesForMoveJust = new List<string>(); // <string FilePath>. Эти файлы будут просто перемещены в новую папку AppData\Local\[appName]_Data
        public List<string> FilesAddToReestr = new List<string>(); // <string FilePath>

        //Делегат для подписи дополнительной логики до установки
        public delegate void SomethingLogicBeforeSetup();
        public SomethingLogicBeforeSetup somethingLogicBeforeSetup;

        public delegate void SomethingLogicAfterSetup();
        public SomethingLogicAfterSetup somethingLogicAfterSetup;

        
        public void StartReplaceProcces(string appName, string squirrelSetupPath, bool addFilesPathToRegistry, bool createDataAppFolder)
        {
            this.AppName = appName;
            this.SquirrelSetupPath = squirrelSetupPath;
            this.DoAddFilesIntoRegistry = addFilesPathToRegistry;
            this.CreateDataAppFolder = createDataAppFolder;

            //Производит замену технологии
                // - AppName - имя приложения. Понадобиться если нужно будет создавать реестр
                    //для приложения, поэтому желательно соотносить с именем приложения в Squirrel версии приложения,
                    //чтобы они к одному ключу обращались


            Console.WriteLine("Начало процедуры замены ClickOnce на Squirrel.Windows...");
            if (this.CreateDataAppFolder)
            {
                this.dataAppFolderName = this.currentUserAppDataLocalFolder + $"\\{appName}_Data";
                Directory.CreateDirectory(this.dataAppFolderName);
            }
            if (this.FilesForMove.Count != 0)
            { 
                this.MoveFiles(this.DoAddFilesIntoRegistry);
            }

            if (this.FilesAddToReestr.Count != 0)
            {
                this.createAppKeyInRegistry(appName);
                this.AddFilesPathToRegistry();
            }

            if(this.somethingLogicBeforeSetup!= null)
                this.somethingLogicBeforeSetup();
            this.SetupSqurrelVariant(squirrelSetupPath);
            if (this.somethingLogicAfterSetup != null)
                this.somethingLogicAfterSetup();
            
            //Было круто бы тут добавить проверочку на существоание приложения в Апдате юзера...

            Console.WriteLine("Замена ClickOnce на Squirrel.Windows завершено");
        }

        public void SetupSqurrelVariant(string squirrelSetupPath)
        {
            Console.WriteLine("Установка Squirrel-варианта приложения...");

            //---------------------------------------------------------------------------
            //@Добавить логику проверки на конец пути, чтобы там было Releases\Setup.exe@
            //Но надо улучшить

            if (squirrelSetupPath.EndsWith(@"\Releases"))
            {
                squirrelSetupPath += @"\Setup.exe";
            }
            else if (squirrelSetupPath.EndsWith(@"\Releases\"))
            {
                squirrelSetupPath += @"Setup.exe";
            }
            else if (!squirrelSetupPath.EndsWith(@"\Releases"))
            {
                squirrelSetupPath += @"\Releases\Setup.exe";
            }
            //---------------------------------------------------------------------------

            Process.Start(squirrelSetupPath).WaitForExit();
        }

        public void createAppKeyInRegistry(string appName)
        {
            //Создал ключи в реестре
        }
        public void MoveFiles(bool AddFilesIntoReestr)
        {
            //Метод перетаскивания файлов в новый путь
            Console.WriteLine("Процедура перемещения отмеченных файлов...");

            foreach (var fileMove in FilesForMove)
            {
                File.Move(fileMove.Key, fileMove.Value);
            }

            if (this.CreateDataAppFolder)
            {
                if (this.FilesForMoveJust.Count != 0)
                {
                    foreach (string file in this.FilesForMoveJust)
                    {
                        File.Move(file, this.dataAppFolderName + $"\\{Path.GetFileName(file)}");
                    }
                }
            }

            if (AddFilesIntoReestr)
            {
                //Здесь красивее сделать
                foreach (string newFilePath in FilesForMove.Values)
                {
                   this.FilesAddToReestr.Add(newFilePath);
                }
            }
        }

        //Метод добавления пути файлов в реестр (Список этого объекта)
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
            
        }
    }
}
