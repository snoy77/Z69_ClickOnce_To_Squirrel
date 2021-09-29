using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace FastRegData
{
    public class FastRegistry
    {
        public string AppName;
        public RegistryKey AppRegistryKey;

        public FastRegistry(string appName)
        {
            this.AppName = appName;
        }

        public void CreateAppKey()
        {
            this.AppRegistryKey = Registry.CurrentUser;
            this.AppRegistryKey = this.AppRegistryKey.CreateSubKey("SOFTWARE");
            this.AppRegistryKey = this.AppRegistryKey.CreateSubKey($"{AppName}");
        }
        public string GetValueOf(string KeyName)
        {
            return this.AppRegistryKey.GetValue(KeyName,null).ToString();
        }

        public Dictionary<string, string> GetValues(List<string> Keys)
        {
            return new Dictionary<string, string>();
        }

        public void SetValue()
        {

        }

        public void CreateKeyAndValue()
        {

        }

        public void CreateKeysAndValues()
        {

        }
    }
}
