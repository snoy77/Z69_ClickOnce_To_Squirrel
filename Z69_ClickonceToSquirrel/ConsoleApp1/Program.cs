using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string a = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).ToString();
            Console.WriteLine(a+@"\Local\");
            Console.ReadLine();
        }
    }
}
