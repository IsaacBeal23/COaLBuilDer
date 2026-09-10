using System.Diagnostics;
using System.Text;

namespace COalBOLder
{
    public partial class Form1 : Form
    {
        public Form1(string location, string name, bool r = false)
        {
            InitializeComponent();
            this.location = location;
            Directory.CreateDirectory(this.location);
            this.name = name;
            fileDisplay1.FileLocation = (this.location);
            if (r)
            {
                fileDisplay1.FileChanged = this.location + "\\" + this.name + ".cbl";
            }
            else
            {
                textEditor1.CreateIdDivision(name);
            }
        }

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
