using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COalBOLder
{
    public partial class FileDisplay : UserControl
    {
        Folder folder;
        public FileDisplay()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
        public String FileLocation
        {
            get { return folder?.Location ?? ""; } set { LoadDirectory(value); }
        }

        public void LoadDirectory(string fileLoc)
        {
            folder = new Folder(fileLoc);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int X = 0 , Y = 0;
            if (folder != null)
                folder.Draw(e.Graphics, Font,ref X,ref Y);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            int Y = e.Location.Y / 20;
            SearchFolder(folder,Y);
            Invalidate();
        }
        public EventHandler FileChangedEvent = new EventHandler((o,e) => { });
        public string FileChanged { get { return fileChanged; } set { fileChanged = value; FileChangedEvent.Invoke(this, new EventArgs()); } }
        private string fileChanged;
        void  SearchFolder(Folder binder, int Y)
        {
            if (Y == binder.RungStart)
            {
                binder.Collapsed = !binder.Collapsed;
                return;
            }
            else if (Y <= binder.RungEnd)
            {
                foreach (var item in folder.Where(f=>f.RungEnd > Y).OrderByDescending(f=>f.RungStart))
                {
                    if (item.RungStart <= Y)
                    {
                        SearchFolder(item, Y);
                        return;
                    }
                }
                FileChanged = binder.Files[Y-folder.Last().RungEnd].Item1;
                return;
            }
        }
    }
    public class Folder : List<Folder>
    {
        public string Name;
        public string Location;
        public bool Collapsed = false;
        public int RungStart = 0;
        public int RungEnd = 0;
        public List<(string,string)> Files = new List<(string, string)>();
        public Folder(string fileLoc)
        {
            Location = fileLoc;
            Name = Regex.Match(fileLoc, @"[^\\]+$").Value;
            foreach (var item in Directory.GetDirectories(fileLoc))
            {
                this.Add(new Folder(item));
            }
            foreach (var item in Directory.GetFiles(fileLoc))
            {
                Files.Add((item, Regex.Match(item, @"[^\\]+$").Value));
            }
        }
        public void Draw(Graphics g, Font f, ref int X, ref int Y)
        {
            RungStart = Y / 20;
            g.DrawString(Name, f, Brushes.Black, X + 14, Y);
            if (Collapsed)
            {
                g.DrawPolygon(Pens.Black, new PointF[] { new Point(X + 2, Y + 4), new Point(X + 10, Y + 9), new Point(X + 2, Y + 14) });
                Y += 20;
            }
            else
            {
                g.DrawPolygon(Pens.Black, new PointF[] { new Point(X + 4, Y + 4), new Point(X + 9, Y + 12), new Point(X + 14, Y + 4) });
                Y += 20;
                X += 20;
                foreach (var item in this)
                {
                    item.Draw(g, f, ref X, ref Y);
                }
                foreach (var item in Files)
                {
                    g.DrawString(item.Item2, f, Brushes.Black, X, Y);
                    Y += 20;
                }
                X -= 20;
            }
            RungEnd = Y / 20;
        }
    }
}
