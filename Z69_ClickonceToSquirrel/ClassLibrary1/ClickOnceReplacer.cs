using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z69_ClickOnceReplacer
{
    public class ClickOnceReplacer
    {
        public Dictionary<string, string> FilesForMove; // <string FilePath, string NewPath>
        public List<string> FilesAddToReestr; // <string FilePath>

        //Делегат для подписи дополнительной логики до установки
        public delegate void SomethingLogicBeforeSetup();
        public SomethingLogicBeforeSetup somethingLogicBeforeSetup;

        //Производит замену технологии
        public void ReplaceTech(string squirrelSetupPath, bool moveFiles, bool addFilesToReestr)
        {
            Console.WriteLine("Начало процедуры замены ClickOnce на Squirrel.Windows...");

            if (this.FilesForMove.Count != 0)
            { 
                this.MoveFiles(true);
            }

            if (this.FilesAddToReestr.Count != 0)
            {
                this.AddFilesToReestr();
            }

            this.somethingLogicBeforeSetup();
            this.SetupSqurrelVariant(squirrelSetupPath);
            
            //Было круто бы тут добавить проверочку на существоание приложения в Апдате юзера...

            Console.WriteLine("Замена ClickOnce на Squirrel.Windows завершено");
        }

        public void SetupSqurrelVariant(string squirrelSetupPath)
        {
            Console.WriteLine("Установка Squirrel-варианта приложения...");

            //---------------------------------------------------------------------------
            //Добавить логику проверки на конец пути, чтобы там было Releases\Setup.exe
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
                    this.AddFileToReestr(newFilePath);
                }
            }
        }

        //Метод добавления пути файлов в реестр (Список этого объекта)
        public void AddFilesToReestr()
        {
            AddFilesToReestr(this.FilesAddToReestr);
        }
        public void AddFilesToReestr(List<string> filesName)
        {
            Console.WriteLine("Процедура добавления файлов в реестр...");

            foreach (string file in filesName)
            {
                this.AddFileToReestr(file);
            }
        }

        private void AddFileToReestr(string FilePath)
        {
            Console.WriteLine($"Типа добавил файл {FilePath} в реестр");
        }
    }
}
