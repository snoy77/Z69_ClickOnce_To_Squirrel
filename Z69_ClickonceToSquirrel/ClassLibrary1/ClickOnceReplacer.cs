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
        private RegistryKey registryKey = Registry.CurrentUser;
        private RegistryKey appRegistryKey;

        public Dictionary<string, string> FilesForMove; // <string FilePath, string NewPath>
        public List<string> FilesAddToReestr; // <string FilePath>

        //Делегат для подписи дополнительной логики до установки
        public delegate void SomethingLogicBeforeSetup();
        public SomethingLogicBeforeSetup somethingLogicBeforeSetup;

        public delegate void SomethingLogicAfterSetup();
        public SomethingLogicAfterSetup somethingLogicAfterSetup;

        
        public void StartReplaceProcces(string AppName, string squirrelSetupPath, bool moveFiles, bool addMoveFilesIntoRegedit)
        {
            //Производит замену технологии
                // - AppName - имя приложения. Понадобиться если нужно будет создавать реестр
                    //для приложения, поэтому желательно соотносить с именем приложения в Squirrel версии приложения,
                    //чтобы они к одному ключу обращались


            Console.WriteLine("Начало процедуры замены ClickOnce на Squirrel.Windows...");

            if (this.FilesForMove.Count != 0)
            { 
                this.MoveFiles(addMoveFilesIntoRegedit);
            }

            if (this.FilesAddToReestr.Count != 0)
            {
                this.appRegistryKey = registryKey.CreateSubKey(AppName);
                this.AddFilesPathToRegedit();
            }

            this.somethingLogicBeforeSetup();
            this.SetupSqurrelVariant(squirrelSetupPath);
            this.somethingLogicAfterSetup();
            
            //Было круто бы тут добавить проверочку на существоание приложения в Апдате юзера...

            Console.WriteLine("Замена ClickOnce на Squirrel.Windows завершено");
        }

        public void SetupSqurrelVariant(string squirrelSetupPath)
        {
            Console.WriteLine("Установка Squirrel-варианта приложения...");

            //---------------------------------------------------------------------------
            //Добавить логику проверки на конец пути, чтобы там было Releases\Setup.exe
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
        

        //Метод перетаскивания файлов в новый путь
        public void MoveFiles(bool AddFilesIntoReestr)
        {
            Console.WriteLine("Процедура перемещения отмеченных файлов...");

            foreach (var fileMove in FilesForMove)
            {
                File.Move(fileMove.Key, fileMove.Value);
            }

            if (AddFilesIntoReestr)
            {
                foreach (string newFilePath in FilesForMove.Values)
                {
                    this.AddFileToRegedit(newFilePath);
                }
            }
        }


        //Метод добавления пути файлов в реестр (Список этого объекта)
        public void AddFilesPathToRegedit()
        {
            AddFilesPathToRegedit(this.FilesAddToReestr);
        }
        public void AddFilesPathToRegedit(List<string> filesName)
        {
            Console.WriteLine("Процедура добавления файлов в реестр...");

            foreach (string file in filesName)
            {
                this.AddFileToRegedit(file);
            }
        }
        private void AddFileToRegedit(string FilePath)
        {
            Console.WriteLine($"Типа добавил файл {FilePath} в реестр");
        }
    }
}
