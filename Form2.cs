using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace COalBOLder
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            if (Exists = File.Exists(loc = Directory.GetCurrentDirectory() + "\\Favourite.TXT"))
            {
                textBox1.Text = File.ReadAllText(loc);
            }
        }

        bool Exists;
        string loc;
        FolderBrowserDialog fd = new FolderBrowserDialog();

        private void button1_Click(object sender, EventArgs e)
        {
            if (fd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fd.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Exists)
            {
                File.WriteAllText(loc,textBox1.Text);
            }
            Form1 form1 = new Form1(textBox1.Text + "\\" + textBox2.Text, textBox2.Text);
            this.Hide();
            form1.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog() { Filter = "Project|*.PRJ" })
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    string l, n;
                    using (var s = new StreamReader(fd.OpenFile()))
                    {
                        l = s.ReadLine();
                        n = s.ReadLine();
                    }
                    Form1 form1 = new Form1(l, n, true);
                    this.Hide();
                    form1.ShowDialog();
                    this.Close();
                }
        }
    }
}
