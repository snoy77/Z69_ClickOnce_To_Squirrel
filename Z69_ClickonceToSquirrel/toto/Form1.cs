using System;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Reflection;
using Microsoft.Win32;
using Squirrel;
using toto.Properties;

using Z69_ClickOnceReplacer;
using Z70_SquirrelHelper;

namespace toto
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //this.ReplaceClickOnce();
            this.CheckUpdate();

            RegistryKey appRegistryKey =
                Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey(Application.ProductName);


            string result = appRegistryKey.GetValue("textFile1.txt").ToString();
            if (Settings.Default.textFilePath != result)
                if (result != null)
                {
                    Settings.Default.textFilePath = result;
                }

            SquirrelHelper squirrelHelper = new SquirrelHelper(Application.ProductName, Settings.
        }
        public void ReplaceClickOnce()
        {
            ClickOnceReplacer clickOnceReplacer =
                new ClickOnceReplacer(Application.ProductName, @"C:\apps\toto_sq", true, true);
            clickOnceReplacer.FilesForMoveJust.Add(Settings.Default.textFilePath);
            clickOnceReplacer.StartReplaceProcces();
        }
        private async Task CheckUpdate()
        {
            using (UpdateManager updateManager = new UpdateManager(@"C:\apps\test_sq\Releases"))
            {
                await updateManager.CheckForUpdate();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.AppendAllLines(Settings.Default.textFilePath, new string[]
            {
                "oleg - 1"
            });
            listBox1.Items.Clear();
            listBox1.Items.AddRange(File.ReadAllLines(Settings.Default.textFilePath));
        }
    }
}
