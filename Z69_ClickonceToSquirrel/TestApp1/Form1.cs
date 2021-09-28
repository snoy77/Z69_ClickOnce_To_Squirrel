using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Squirrel;
using System.Diagnostics;

namespace TestApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Приложение требует обновления.\nДождитесь окончания обновления.");

            SomethingLogicBeforeSetup();

            //Запуск установки приложения-squirrel версии
                //(путь к папке-установщику (к папке с обновлениями приложения с Squirrel.Windows технологией))
            Process.Start(@"C:\apps\testApps\TestApp1_sq\Releases\Setup.exe");
            this.Close();
            //Закрытие так как автоматически откроется установленное прилдожение Squirrel версии
        }

        private void SomethingLogicBeforeSetup()
        {
            //Какая-нибудь логика перед установкой
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello world!");
        }
    }
}
