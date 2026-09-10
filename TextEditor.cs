using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COalBOLder
{
    public partial class TextEditor : UserControl
    {
        public string AccessFile { get { return accessFile; } set { LoadFile(value); } }
        private string accessFile;
        public bool isCobol = false;
        SizeF s;
        string ClipBoard2 = "";
        public TextEditor()
        {
            InitializeComponent();
            HScroll = true;
            VScroll = true;
            DoubleBuffered = true;
            Font = new Font("Consolas", 12, FontStyle.Regular);
            /*using var path = new GraphicsPath();
            path.AddString("M", Font.FontFamily, (int)Font.Style, Font.Size, Point.Empty, StringFormat.GenericDefault);
            s = path.GetBounds().Size;
            s.Width *= 1.5f; s.Height *= 2;*/
            s = TextRenderer.MeasureText("M", Font, Size.Empty, TextFormatFlags.NoPadding);
        }
        public override string Text { get => string.Concat(lines); set => base.Text = value; }
        public void LoadFile(string value)
        {
            accessFile = value;
            lines = File.ReadAllLines(value).ToList();
            isCobol = value.EndsWith(".cbl");
            Invalidate();
        }

        public void CreateIdDivision(string value, bool NotClass = true)
        {
            lines.Insert(0,(NotClass ? "PROGRAM-ID. " : "CLASS-ID. ") + value);
            lines.Insert(0, "IDENTIFICATION DIVISION.");
        }
        List<string> lines = new List<string>();
        public int CursorX { get { return cursorX; }
            set { if (CursorY < lines.Count) { if (value > lines[CursorY].Length) { int c = CursorY++; if (c != CursorY) cursorX = 0; } else if (value < 0) { int c = CursorY--; if (c != CursorY) cursorX = lines[CursorY].Length; } else { cursorX = value; } } } }
        private int cursorX = 0;
        public int CursorY
        {
            get { return cursorY; }
            set { if (value >= 0 && value < lines.Count) cursorY = value; }
        }
        int CursorPosX = -1;
        int CursorPosY = -1;
        private int cursorY = 0;
        private void UpdateScrollArea()
        {
            int maxWidth = 0;

            foreach (var item in lines)
            {
                maxWidth = Math.Max(maxWidth,item.Length);
            }

            AutoScrollMinSize = new Size(
                (int)Math.Ceiling(maxWidth*s.Width),
                (int)Math.Ceiling(lines.Count*s.Height));
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                    CursorX--;
                    break;
                case Keys.Right:
                    CursorX++;
                    break;
                case Keys.Up:
                    if (cursorY > 0)
                        CursorX = Math.Min(CursorX, lines[--CursorY].Length);
                    break;
                case Keys.Down:
                    if (cursorY < lines.Count-1)
                        CursorX = Math.Min(CursorX, lines[++CursorY].Length);
                    break;
                case Keys.Tab:
                    for (int i = 4-(CursorX % 4); i > 0; i--)
                    {
                        lines[CursorY] = Insert(CursorX++, ' ', lines[CursorY]);
                    }
                    break;
                case Keys.Space:
                    lines[CursorY] = Insert(CursorX++, ' ', lines[CursorY]);
                    break;
                case Keys.Enter:
                    lines.Insert(CursorY + 1, "");
                    CursorY++;
                    lines[CursorY] = lines[CursorY - 1].Substring(CursorX);
                    lines[CursorY - 1] = lines[CursorY - 1].Substring(0, CursorX);
                    CursorX = 0;
                    break;
                case Keys.Back:
                    if (CursorPosX != -1)
                    {
                        Destroy();
                    }
                    else if (cursorX == 0)
                    {
                        if (cursorY > 0)
                        {
                            cursorX = lines[CursorY - 1].Length;
                            lines[cursorY - 1] = lines[cursorY - 1] + lines[cursorY];
                            lines.RemoveAt(cursorY--);
                        }
                    }
                    else
                    {
                        CursorX--;
                        lines[CursorY] = lines[CursorY].Remove(CursorX, 1);
                    }
                    break;
                case Keys.Delete:
                    if (CursorPosX != -1)
                    {
                        Destroy();
                    }
                    else if (CursorX == lines[cursorY].Length)
                    {
                        if (CursorY < lines.Count - 1)
                        {
                            lines[CursorY] = lines[CursorY] + lines[CursorY + 1];
                            lines.RemoveAt(CursorY + 1);
                        }
                    }
                    else
                    {
                        lines[CursorY] = lines[CursorY].Remove(CursorX, 1);
                    }
                    break;
            }
            UpdateScrollArea();
            Invalidate();
            return base.ProcessDialogKey(keyData);
        }

        private void Destroy()
        {
            if (CursorPosX == -1)
            { return; }
            if (CursorPosY == CursorY)
            {
                int SX = Math.Min(cursorX, CursorPosX);
                int WX = Math.Abs(cursorX - CursorPosX);
                lines[cursorY] = lines[cursorY].Remove(SX, WX);
                CursorPosX = SX;
                cursorX = SX;
            }
            else if (CursorPosY < cursorY)
            {
                lines[CursorPosY] = lines[CursorPosY].Remove(CursorPosX);
                lines[CursorY] = lines[CursorY].Substring(CursorX);
                for (int i = CursorPosY + 1; i < CursorY; i++)
                {
                    lines.RemoveAt(CursorPosY+1);
                }
                cursorY = CursorPosY;
                cursorX = CursorPosX;
            }
            else
            {
                lines[CursorY] = lines[CursorY].Remove(CursorX);
                lines[CursorPosY] = lines[CursorPosY].Substring(CursorPosX);
                for (int i = CursorY + 1; i < CursorPosY; i++)
                {
                    lines.RemoveAt(CursorY + 1);
                }
                CursorPosY = cursorY;
                CursorPosX = cursorX;
            }
        }
        private void Paste(bool b) {
            string paste = b ? ClipBoard2 : Clipboard.GetText(TextDataFormat.UnicodeText);

            if (string.IsNullOrEmpty(paste))
                return;

            paste = paste.Replace("\r\n", "\n").Replace("\r", "\n");

            string[] pastedLines = paste.Split('\n');

            string currentLine = lines[CursorY];

            CursorX = Math.Clamp(CursorX, 0, currentLine.Length);

            string before = currentLine[..CursorX];
            string after = currentLine[CursorX..];

            if (pastedLines.Length == 1)
            {
                lines[CursorY] = before + pastedLines[0] + after;

                CursorX += pastedLines[0].Length;
            }
            else
            {
                lines[CursorY] = before + pastedLines[0];

                for (int i = 1; i < pastedLines.Length - 1; i++)
                {
                    lines.Insert(CursorY + i, pastedLines[i]);
                }

                lines.Insert(
                    CursorY + pastedLines.Length - 1,
                    pastedLines[^1] + after);

                CursorY += pastedLines.Length - 1;
                CursorX = pastedLines[^1].Length;
            }

            UpdateScrollArea();
            Invalidate();
        }
        private void Copy(bool b)
        {
            if (CursorPosX == -1)
                return;
            int startX = CursorPosX;
            int startY = CursorPosY;

            int endX = CursorX;
            int endY = CursorY;

            if (startY > endY || (startY == endY && startX > endX))
            {
                (startX, endX) = (endX, startX);
                (startY, endY) = (endY, startY);
            }

            var result = new StringBuilder();

            if (startY == endY)
            {
                string line = lines[startY];

                startX = Math.Clamp(startX, 0, line.Length);
                endX = Math.Clamp(endX, 0, line.Length);

                result.Append(line[startX..endX]);
            }
            else
            {
                string firstLine = lines[startY];

                startX = Math.Clamp(startX, 0, firstLine.Length);

                result.Append(firstLine[startX..]);
                result.Append(Environment.NewLine);

                for (int y = startY + 1; y < endY; y++)
                {
                    result.Append(lines[y]);
                    result.Append(Environment.NewLine);
                }

                string lastLine = lines[endY];

                endX = Math.Clamp(endX, 0, lastLine.Length);

                result.Append(lastLine[..endX]);
            }
            if (b)
                ClipBoard2 = result.ToString();
            else
                Clipboard.SetText(result.ToString());
        }


        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Focus();
        }
        private string Insert(int pos, char c, string str)
        {
            return str.Substring(0,pos) + c + str.Substring(pos);
        }
        public static List<string> SplitSymbols(string input)
        {
            var result = new List<string>();

            if (string.IsNullOrEmpty(input))
                return result;

            int start = 0;

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                
                if (!char.IsLetterOrDigit(c) && c != '-')
                {
                    if (i > start)
                        result.Add(input[start..i]);
                    if (c == '"')
                    {
                        start = i;
                        int j = input.IndexOf('"', i + 1);

                        if (j > start)
                        {
                            result.Add(input[start..j]);
                            i = j;
                        }
                    }
                    result.Add(c.ToString());

                    start = i + 1;
                }
            }

            if (start < input.Length)
                result.Add(input[start..]);

            return result;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (char.IsLetter(e.KeyChar))
            {
                lines[CursorY] = Insert(CursorX, e.KeyChar, lines[CursorY]);
                cursorX++;
            }
            else if (char.IsDigit(e.KeyChar))
            {
                lines[CursorY] = Insert(CursorX, e.KeyChar, lines[CursorY]);
                CursorX++;
            }
            else if (char.IsSymbol(e.KeyChar))
            {
                lines[CursorY] = Insert(CursorX, e.KeyChar, lines[CursorY]);
                CursorX++;
            }
            else if (char.IsControl(e.KeyChar))
            {
                switch (e.KeyChar)
                {
                    case ('\u0001'):
                        cursorX = 0;
                        cursorY = 0;
                        CursorPosY = lines.Count - 1;
                        CursorPosX = lines[CursorPosY].Length;
                        break;
                    case ('\u0016'):
                        Destroy();
                        Paste((Control.ModifierKeys & Keys.Shift) == Keys.Shift);
                        break;
                    case ('\u0018'):
                        Copy((Control.ModifierKeys & Keys.Shift) == Keys.Shift);
                        Destroy();
                        break;
                    case ('\u0003'):
                        Copy((Control.ModifierKeys & Keys.Shift) == Keys.Shift);
                        break;
                }
            }
            UpdateScrollArea();
            Invalidate();
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            (CursorX, CursorY) = CalcCursorPos(e.Location);
            (CursorPosX, CursorPosY) = (-1, -1);
            Invalidate();
        }
        private (int, int) CalcCursorPos(Point p)
        {
            int y = Math.Min((int)((p.Y - VerticalScroll.Value) / s.Height),lines.Count-1);
            return (Math.Min((int)((p.X - HorizontalScroll.Value) / s.Width), lines[y].Length), y);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (Control.MouseButtons == MouseButtons.Left)
            {
                (CursorPosX, CursorPosY) = CalcCursorPos(PointToClient(MousePosition));
                if ((CursorPosX, CursorPosY) == (CursorX, CursorY))
                    (CursorPosX, CursorPosY) = (-1, -1);
                Invalidate();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int scrollx = 0;
            int scrolly = 0;
            float y = 0;
            foreach (var line in lines)
            {
                float x = 0;
                if (isCobol)
                {
                    foreach (var word in SplitSymbols(line))
                    {

                        Brush brush = word.ToUpperInvariant() switch
                        {
                            "IDENTIFICATION" => Brushes.Blue,
                            "PROGRAM-ID" => Brushes.Blue,
                            "AUTHOR" => Brushes.Blue,
                            "ENVIRONMENT" => Brushes.Blue,
                            "CONFIGURATION" => Brushes.Blue,
                            "DATA" => Brushes.Blue,
                            "WORKING-STORAGE" => Brushes.Blue,
                            "PROCEDURE" => Brushes.Blue,
                            "DIVISION" => Brushes.DarkGreen,
                            "SECTION" => Brushes.DarkGreen,
                            "DISPLAY" => Brushes.DarkOrange,
                            "ACCEPT" => Brushes.DarkOrange,
                            "PIC" => Brushes.DarkOrange,
                            "COMPUTE" => Brushes.DarkOrange,
                            "ADD" => Brushes.DarkOrange,
                            "SUBTRACT" => Brushes.DarkOrange,
                            "MULTIPLY" => Brushes.DarkOrange,
                            "DIVIDE" => Brushes.DarkOrange,
                            "BY" => Brushes.DarkOrange,
                            "TO" => Brushes.DarkOrange,
                            "FROM" => Brushes.DarkOrange,
                            "STOP" => Brushes.DarkOrange,
                            "RUN" => Brushes.DarkOrange,
                            _ => (float.TryParse(word, out float f)) ? Brushes.Red : word.StartsWith('"') ? Brushes.Green : Brushes.Black
                        };
                        e.Graphics.DrawString(word, Font, brush, x - HorizontalScroll.Value, y - VerticalScroll.Value);
                        x += s.Width * word.Length;
                    }
                }
                else
                {
                    if (line.Length > 1000)
                        e.Graphics.DrawString(line.Substring(0,1000), Font, Brushes.Black, -HorizontalScroll.Value, y - VerticalScroll.Value);
                    else
                        e.Graphics.DrawString(line, Font, Brushes.Black, - HorizontalScroll.Value, y - VerticalScroll.Value);
                }
                y += s.Height;
            }
            if (CursorPosX > -1)
            {
                if (CursorY == CursorPosY)//Same Line
                {
                    int SX = Math.Min(cursorX, CursorPosX);
                    int WX = Math.Abs(cursorX - CursorPosX);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 255)), 2.5f + s.Width * SX, s.Height * cursorY, s.Width * WX, s.Height);
                }
                else if (CursorY > CursorPosY)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 255)), 2.5f + s.Width * CursorPosX, s.Height * CursorPosY, s.Width * (lines[CursorPosY].Length-CursorPosX), s.Height);
                    for (int i = CursorPosY + 1; i < CursorY; i++)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 255)), 2.5f, s.Height * i, s.Width * lines[i].Length, s.Height);
                    }
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 255)), 2.5f, s.Height * CursorY, s.Width * CursorX, s.Height);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 255)), 2.5f + s.Width * CursorX, s.Height * CursorY, s.Width * (lines[CursorY].Length - CursorX), s.Height);
                    for (int i = CursorY + 1; i < CursorPosY; i++)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 255)), 2.5f, s.Height * i, s.Width * lines[i].Length, s.Height);
                    }
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 255)), 2.5f, s.Height * CursorPosY, s.Width * CursorPosX, s.Height);
                }
            }
            e.Graphics.DrawLine(Pens.Black, 2.5f + CursorX * s.Width, CursorY * s.Height, 2.5f + CursorX * s.Width, (CursorY + 1) * s.Height);
        }
    }
}
