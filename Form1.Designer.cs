namespace COalBOLder
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button7 = new Button();
            textEditor1 = new TextEditor();
            splitContainer1 = new SplitContainer();
            fileDisplay1 = new FileDisplay();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.TabStop = false;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(93, 12);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 0;
            button2.TabStop = false;
            button2.Text = "Compile";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(174, 12);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 0;
            button3.TabStop = false;
            button3.Text = "Run";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(255, 12);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 0;
            button4.TabStop = false;
            button4.Text = "S/C/R";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button5.Location = new Point(688, 12);
            button5.Name = "button5";
            button5.Size = new Size(100, 23);
            button5.TabIndex = 0;
            button5.TabStop = false;
            button5.Text = "Set Folder";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button7
            // 
            button7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button7.Location = new Point(582, 12);
            button7.Name = "button7";
            button7.Size = new Size(100, 23);
            button7.TabIndex = 0;
            button7.TabStop = false;
            button7.Text = "Save Project";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // textEditor1
            // 
            textEditor1.AccessFile = "C:\\Users\\David Beal\\Documents\\COBOL\\HelloWorld\\HelloWorld.cbl";
            textEditor1.BackColor = Color.White;
            textEditor1.BackgroundImageLayout = ImageLayout.None;
            textEditor1.CursorX = 0;
            textEditor1.CursorY = 0;
            textEditor1.Dock = DockStyle.Fill;
            textEditor1.Font = new Font("Consolas", 12F);
            textEditor1.Location = new Point(0, 0);
            textEditor1.Name = "textEditor1";
            textEditor1.Size = new Size(563, 397);
            textEditor1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(12, 41);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(textEditor1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(fileDisplay1);
            splitContainer1.Size = new Size(776, 397);
            splitContainer1.SplitterDistance = 563;
            splitContainer1.TabIndex = 2;
            // 
            // fileDisplay1
            // 
            fileDisplay1.BackColor = Color.White;
            fileDisplay1.Dock = DockStyle.Fill;
            fileDisplay1.Location = new Point(0, 0);
            fileDisplay1.Name = "fileDisplay1";
            fileDisplay1.Size = new Size(209, 397);
            fileDisplay1.TabIndex = 0;
            fileDisplay1.FileChangedEvent += ChangeFile;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer1);
            Controls.Add(button7);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button7;
        private TextEditor textEditor1;
        private SplitContainer splitContainer1;
        private FileDisplay fileDisplay1;
    }
}
