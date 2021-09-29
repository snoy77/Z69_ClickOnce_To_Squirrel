using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using Squirrel;
using toto.Properties;

using Z69_ClickOnceReplacer;
using FastRegData;


namespace toto
{
    public partial class Form1 : Form
    {
        Z69_ClickOnceReplacer.ClickOnceReplacer clickOnceReplacer;

        public Form1()
        {
          
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("Надо обновиться. Нажмите \"ок\" для продолжения");
            //clickOnceReplacer = new ClickOnceReplacer();
            //clickOnceReplacer.FilesForMoveJust.Add(Settings.Default.textFilePath);
            //clickOnceReplacer.StartReplaceProcces("toto", @"С:\apps\toto_sq\Releases\Setup.exe", true, true);
            //this.Close();

            FastRegistry fastRegistry = new FastRegistry("toto");
            fastRegistry.CreateAppKey();
            MessageBox.Show(fastRegistry.GetValueOf("dbPath"));
        }
        private async Task CheckUpdate()
        {
            using (UpdateManager updateManager = new UpdateManager(@"C:\apps\test_sq\Release"))
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
