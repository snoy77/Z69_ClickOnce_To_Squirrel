using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;
using toto.Properties;

namespace Z70_SquirrelHelper
{
     class SquirrelHelper
    {
        /// <summary>
        /// Объект предоставляет методы и свойства, упрощающие некоторую работу с публикацией на технологии squirrel.windows
        /// v: 1.0.0
        /// </summary>

        public string AppName;
        private RegistryKey registryKey;

        public SquirrelHelper(string appName)
        {
            this.AppName = appName;

            this.registryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey(appName);
        }
        public void CompareAndDefineParameters(string keys)
        {
            string result = "";
            foreach (SettingsProperty property in Settings.Default.Properties)
            {
                result = this.GetValueFromRegistry(property.Name);
                if (property.DefaultValue != result)
                    if(result != null) 
                        property.DefaultValue = result;
                    else
                    {
                        //Здесь надо чтобы sqlhelper создал ключ-значение в реесте со значением... а каким.
                        //Поэтому и надо написать функцию мува файлов, если те не существуют в конечном пути, а затем только првоерять на проперти всё.

                        //У МЕНЯ ЖЕ ЕСТЬ БЛИН В АПДАТЕ ИХ ПАПКА ААААААААААААААААААААААААААААА 
                        //Под устал((((
                    }

            }
        }

        public string GetValueFromRegistry(string key)
        {
            return this.registryKey.GetValue(key).ToString();
        }
    }
}
