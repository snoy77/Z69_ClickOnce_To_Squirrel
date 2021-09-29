using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Z70_SquirrelHelper
{
    public class SquirrelHelper
    {
        /// <summary>
        /// Объект предоставляет методы и свойства, упрощающие некоторую работу с публикацией на технологии squirrel.windows
        /// </summary>


        public string AppName;

        public string AppVersion;

        public RegistryKey appRegistryKey; // onlyRead


        public SquirrelHelper(string appName)
        {
            this.AppName = appName;
        }

        public void FindAppKeyRegisry(string appKey)
        {
            this.appRegistryKey = Registry.CurrentUser;
            this.appRegistryKey = appRegistryKey.CreateSubKey("SOFTWARE");
            this.appRegistryKey = appRegistryKey.OpenSubKey(appKey);
        }
        public string getValueFromRegistry(string key)
        {
            if (this.appRegistryKey == null)
            {
                this.FindAppKeyRegisry(this.AppName);
            }

            return appRegistryKey.GetValue(key, null).ToString();
        }
        
    }
}
