using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace COalBOLder
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            if (Exists = File.Exists(loc = Directory.GetCurrentDirectory() + "\\Favourite.TXT"))
            {
                XDocument xmlDoc = XDocument.Load(loc);
                var c = xmlDoc.Root!.Element("Favourite");
                textBox1.Text = xmlDoc.Root!.Element("Favourite")?.Value ?? "";
                IEnumerable<XElement> x;
                if ((x = xmlDoc.Root!.Descendants("ColourScheme")).Count() > 0)
                {
                    foreach (var colour in x.Descendants())
                    {
                        foreach (var item in colour.Descendants())
                        {
                            colourScheme.Add(item.Value, Color.FromArgb((255<<24)+int.Parse(colour.Name.LocalName.Substring(1), NumberStyles.HexNumber, CultureInfo.InvariantCulture)));
                        }
                    }
                }
            }
        }

        Dictionary<string, Color> colourScheme = new Dictionary<string, Color>();

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
                File.WriteAllText(loc,$"<Conditions>\n\t<Favourite>{textBox1.Text}</Favourite>\n\t<ColourScheme>\n\t\t<aFF0000>\n\t\t\t<a1>#numbers</a1>\n\t\t</aFF0000>\n\t\t<a00FF00>\n\t\t\t<a1>DIVISION</a1>\n\t\t<a2>SECTION</a2>\n\t\t</a00FF00>\n\t\t<a0000FF>\n\t\t\t<a1>PIC</a1>\n\t\t</a0000FF>\n\t</ColourScheme>\n</Conditions>");
            }
            Form1 form1 = new Form1(textBox1.Text + "\\" + textBox2.Text, textBox2.Text, colourScheme);
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
                    Form1 form1 = new Form1(l, n, colourScheme, true);
                    this.Hide();
                    form1.ShowDialog();
                    this.Close();
                }
        }
    }
}
