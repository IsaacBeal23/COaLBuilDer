using System.Diagnostics;
using System.Text;

namespace COalBOLder
{
    public partial class Form1 : Form
    {
        public bool DarkMode { get { return darkMode; } set { darkMode = value;
                if (darkMode)
                {
                    this.BackColor = Color.FromArgb(30, 30, 30);
                    textEditor1.BackColor = Color.FromArgb(50, 50, 50);
                    textEditor1.ForeColor = Color.White;
                    fileDisplay1.BackColor = Color.FromArgb(50, 50, 50);
                    fileDisplay1.ForeColor = Color.White;
                    foreach (Control con in this.Controls)
                    {
                        if (con is Label l)
                        {
                            l.ForeColor = Color.White;
                        }
                        else if (con is Button tb)
                        {
                            tb.BackColor = Color.FromArgb(70, 70, 70);
                            tb.ForeColor = Color.White;
                        }
                        else if (con is SplitContainer sc)
                        {
                            sc.BackColor = Color.FromArgb(30, 30, 30);
                            sc.ForeColor = Color.White;
                        }
                        else
                        {
                            con.BackColor = Color.FromArgb(50, 50, 50);
                            con.ForeColor = Color.White;
                        }
                    }
                }
                else
                {
                    this.BackColor = SystemColors.Control;
                    foreach (Control con in this.Controls)
                    {
                        if (con is Label l)
                        {
                            l.ForeColor = SystemColors.ControlText;
                        }
                        else
                        {
                            foreach (Control con2 in con.Controls)
                            {
                                con2.BackColor = SystemColors.Control;
                                con2.ForeColor = SystemColors.ControlText;
                            }
                            con.BackColor = SystemColors.Control;
                            con.ForeColor = SystemColors.ControlText;
                        }
                    }
                }
            } }
        private bool darkMode;
        public Form1(string location, string name, Dictionary<string, Color> cs, bool DarkMode, bool r = false)
        {
            InitializeComponent();
            this.DarkMode = DarkMode;
            this.location = location;
            Directory.CreateDirectory(this.location);
            this.name = name;
            if (cs.Count > 0)
                textEditor1.ColourScheme = cs;
            fileDisplay1.FileChangedEvent += ChangeFile;
            fileDisplay1.FileLocation = (this.location);
            if (r)
            {
                fileDisplay1.FileChanged = this.location + "\\" + this.name + ".cbl";
            }
            else
            {
                textEditor1.CreateIdDivision(name);
                textEditor1.isCobol = true;
            }
            MessageBox.Show("This is a beta version of COalBOLder, please report any bugs to the developer.");
        }

        Dictionary<string, Color> CS = new Dictionary<string, Color>();

        public void ChangeFile(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(fileDisplay1.FileChanged))
            {
                textEditor1.LoadFile(fileDisplay1.FileChanged);
            }
        }

        string location;
        string name;

        private void button1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            if (fileDisplay1.FileChanged != null)
            {
                File.WriteAllText(fileDisplay1.FileChanged, textEditor1.Text);
            }
            else
            {
                File.WriteAllText($"{location}\\{name}.cbl", textEditor1.Text);
            }
            fileDisplay1.LoadDirectory(location);
        }

        private void Compile()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            psi.ArgumentList.Add("-NoProfile");
            psi.ArgumentList.Add("-Command");
            psi.ArgumentList.Add(
                $"cobc -x -o \"{location}\\{name}.exe\" \"{location}\\{name}.cbl\""
            );

            using var process = Process.Start(psi)!;

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            process.WaitForExit();
            process.Dispose();
            MessageBox.Show(output);
            MessageBox.Show(error);
        }

        Process process;

        private void Run()
        {
            process = new Process();

            process.StartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(location, $"{name}.exe"),
                WorkingDirectory = location,
                UseShellExecute = true
            };

            process.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Compile();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Save();
            Compile();
            Run();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Save();
            Compile();
            File.WriteAllText($"{location}\\{name}.PRJ", location + "\n" + name);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Serviced At current Time");
        }
    }
}
