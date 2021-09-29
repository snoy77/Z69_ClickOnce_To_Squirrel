using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConsoleApp1.Properties;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var set = Settings.Default.Properties;

            foreach (SettingsProperty property in set)
            {
                Console.WriteLine(property.Name);
                Console.WriteLine(property.DefaultValue);
            }
            
            Console.ReadLine();
        }
    }
}
