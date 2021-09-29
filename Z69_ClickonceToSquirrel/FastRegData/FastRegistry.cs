using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegData
{
    public class FastRegistry
    {
        private string appName;
        private string AppName
        {
            get { return this.appName;}
            set { this.appName = value; }
        }

        public FastRegistry(string appName)
        {
            this.AppName = appName;
        }

        public string GetValueOf(string KeysName)
        {
            return "Value";
        }

        public Dictionary<string, string> GetValues(List<string> Keys)
        {
            return new Dictionary<string, string>();
        }
    }
}
