namespace BookWriterAI
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
            groupBox1 = new GroupBox();
            splitContainer1 = new SplitContainer();
            OutlineTextBox = new TextBox();
            treeView1 = new TreeView();
            GenerateTreeButton = new Button();
            GenerateIdeaButton = new Button();
            IdeaTextBox = new TextBox();
            groupBox2 = new GroupBox();
            splitContainer3 = new SplitContainer();
            ChapterTextBox = new TextBox();
            treeView3 = new TreeView();
            GenerateChapterButton = new Button();
            groupBox3 = new GroupBox();
            GenerateIdeaBtn = new Button();
            groupBox4 = new GroupBox();
            splitContainer2 = new SplitContainer();
            ActsText = new TextBox();
            treeView2 = new TreeView();
            GenerateActsButton = new Button();
            GenerateBookButton = new Button();
            BookTextBox = new TextBox();
            ProgressBar = new ProgressBar();
            label1 = new Label();
            StyleBox = new ComboBox();
            label2 = new Label();
            AuthorBox = new ComboBox();
            label3 = new Label();
            ChapterUpDown = new NumericUpDown();
            ChatTextBox = new TextBox();
            button1 = new Button();
            ChatOutputBox = new TextBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ChapterUpDown).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(splitContainer1);
            groupBox1.Controls.Add(GenerateTreeButton);
            groupBox1.Controls.Add(GenerateIdeaButton);
            groupBox1.Location = new Point(16, 185);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1684, 267);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Book Outline";
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(144, 30);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(OutlineTextBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(treeView1);
            splitContainer1.Size = new Size(1512, 218);
            splitContainer1.SplitterDistance = 31;
            splitContainer1.TabIndex = 4;
            // 
            // OutlineTextBox
            // 
            OutlineTextBox.Dock = DockStyle.Fill;
            OutlineTextBox.Location = new Point(0, 0);
            OutlineTextBox.Multiline = true;
            OutlineTextBox.Name = "OutlineTextBox";
            OutlineTextBox.ReadOnly = true;
            OutlineTextBox.ScrollBars = ScrollBars.Vertical;
            OutlineTextBox.Size = new Size(31, 218);
            OutlineTextBox.TabIndex = 2;
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(1477, 218);
            treeView1.TabIndex = 0;
            // 
            // GenerateTreeButton
            // 
            GenerateTreeButton.Location = new Point(0, 214);
            GenerateTreeButton.Name = "GenerateTreeButton";
            GenerateTreeButton.Size = new Size(144, 34);
            GenerateTreeButton.TabIndex = 3;
            GenerateTreeButton.Text = "Generate Tree";
            GenerateTreeButton.UseVisualStyleBackColor = true;
            GenerateTreeButton.Click += GenerateTreeButton_Click;
            // 
            // GenerateIdeaButton
            // 
            GenerateIdeaButton.Location = new Point(13, 28);
            GenerateIdeaButton.Name = "GenerateIdeaButton";
            GenerateIdeaButton.Size = new Size(112, 34);
            GenerateIdeaButton.TabIndex = 3;
            GenerateIdeaButton.Text = "Generate";
            GenerateIdeaButton.UseVisualStyleBackColor = true;
            GenerateIdeaButton.Click += GenerateIdeaButton_Click;
            // 
            // IdeaTextBox
            // 
            IdeaTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            IdeaTextBox.Location = new Point(178, 30);
            IdeaTextBox.Multiline = true;
            IdeaTextBox.Name = "IdeaTextBox";
            IdeaTextBox.ScrollBars = ScrollBars.Vertical;
            IdeaTextBox.Size = new Size(1480, 68);
            IdeaTextBox.TabIndex = 0;
            IdeaTextBox.TextChanged += IdeaTextBox_TextChanged;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(splitContainer3);
            groupBox2.Controls.Add(GenerateChapterButton);
            groupBox2.Location = new Point(16, 723);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1684, 315);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Chapters";
            // 
            // splitContainer3
            // 
            splitContainer3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer3.Location = new Point(144, 30);
            splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(ChapterTextBox);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(treeView3);
            splitContainer3.Size = new Size(1512, 271);
            splitContainer3.SplitterDistance = 36;
            splitContainer3.TabIndex = 5;
            // 
            // ChapterTextBox
            // 
            ChapterTextBox.Dock = DockStyle.Fill;
            ChapterTextBox.Location = new Point(0, 0);
            ChapterTextBox.Multiline = true;
            ChapterTextBox.Name = "ChapterTextBox";
            ChapterTextBox.ReadOnly = true;
            ChapterTextBox.ScrollBars = ScrollBars.Vertical;
            ChapterTextBox.Size = new Size(36, 271);
            ChapterTextBox.TabIndex = 4;
            // 
            // treeView3
            // 
            treeView3.Dock = DockStyle.Fill;
            treeView3.Location = new Point(0, 0);
            treeView3.Name = "treeView3";
            treeView3.Size = new Size(1472, 271);
            treeView3.TabIndex = 0;
            // 
            // GenerateChapterButton
            // 
            GenerateChapterButton.Location = new Point(6, 30);
            GenerateChapterButton.Name = "GenerateChapterButton";
            GenerateChapterButton.Size = new Size(112, 34);
            GenerateChapterButton.TabIndex = 0;
            GenerateChapterButton.Text = "Generate";
            GenerateChapterButton.UseVisualStyleBackColor = true;
            GenerateChapterButton.Click += GenerateChapterButton_Click;
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3.Controls.Add(GenerateIdeaBtn);
            groupBox3.Controls.Add(IdeaTextBox);
            groupBox3.Location = new Point(16, 62);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(1684, 117);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Idea";
            // 
            // GenerateIdeaBtn
            // 
            GenerateIdeaBtn.Location = new Point(13, 30);
            GenerateIdeaBtn.Name = "GenerateIdeaBtn";
            GenerateIdeaBtn.Size = new Size(112, 34);
            GenerateIdeaBtn.TabIndex = 4;
            GenerateIdeaBtn.Text = "Generate";
            GenerateIdeaBtn.UseVisualStyleBackColor = true;
            GenerateIdeaBtn.Click += GenerateIdeaBtn_Click;
            // 
            // groupBox4
            // 
            groupBox4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox4.Controls.Add(splitContainer2);
            groupBox4.Controls.Add(GenerateActsButton);
            groupBox4.Location = new Point(16, 458);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(1684, 259);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Acts";
            // 
            // splitContainer2
            // 
            splitContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer2.Location = new Point(144, 33);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(ActsText);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(treeView2);
            splitContainer2.Size = new Size(1512, 211);
            splitContainer2.SplitterDistance = 34;
            splitContainer2.TabIndex = 6;
            // 
            // ActsText
            // 
            ActsText.Dock = DockStyle.Fill;
            ActsText.Location = new Point(0, 0);
            ActsText.Multiline = true;
            ActsText.Name = "ActsText";
            ActsText.ReadOnly = true;
            ActsText.ScrollBars = ScrollBars.Vertical;
            ActsText.Size = new Size(34, 211);
            ActsText.TabIndex = 4;
            // 
            // treeView2
            // 
            treeView2.Dock = DockStyle.Fill;
            treeView2.Location = new Point(0, 0);
            treeView2.Name = "treeView2";
            treeView2.Size = new Size(1474, 211);
            treeView2.TabIndex = 0;
            // 
            // GenerateActsButton
            // 
            GenerateActsButton.Location = new Point(13, 30);
            GenerateActsButton.Name = "GenerateActsButton";
            GenerateActsButton.Size = new Size(112, 34);
            GenerateActsButton.TabIndex = 5;
            GenerateActsButton.Text = "Generate";
            GenerateActsButton.UseVisualStyleBackColor = true;
            GenerateActsButton.Click += GenerateActsButton_Click;
            // 
            // GenerateBookButton
            // 
            GenerateBookButton.Location = new Point(199, 1132);
            GenerateBookButton.Name = "GenerateBookButton";
            GenerateBookButton.Size = new Size(181, 34);
            GenerateBookButton.TabIndex = 4;
            GenerateBookButton.Text = "Generate Book";
            GenerateBookButton.UseVisualStyleBackColor = true;
            GenerateBookButton.Click += GenerateBookButton_Click;
            // 
            // BookTextBox
            // 
            BookTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            BookTextBox.Location = new Point(12, 1172);
            BookTextBox.Multiline = true;
            BookTextBox.Name = "BookTextBox";
            BookTextBox.ReadOnly = true;
            BookTextBox.ScrollBars = ScrollBars.Vertical;
            BookTextBox.Size = new Size(367, 88);
            BookTextBox.TabIndex = 5;
            // 
            // ProgressBar
            // 
            ProgressBar.Dock = DockStyle.Bottom;
            ProgressBar.Location = new Point(0, 1276);
            ProgressBar.MarqueeAnimationSpeed = 10;
            ProgressBar.Name = "ProgressBar";
            ProgressBar.Size = new Size(1712, 26);
            ProgressBar.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 1042);
            label1.Name = "label1";
            label1.Size = new Size(49, 25);
            label1.TabIndex = 7;
            label1.Text = "Style";
            // 
            // StyleBox
            // 
            StyleBox.FormattingEnabled = true;
            StyleBox.Location = new Point(12, 1069);
            StyleBox.Name = "StyleBox";
            StyleBox.Size = new Size(174, 33);
            StyleBox.Sorted = true;
            StyleBox.TabIndex = 8;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 1104);
            label2.Name = "label2";
            label2.Size = new Size(67, 25);
            label2.TabIndex = 7;
            label2.Text = "Author";
            // 
            // AuthorBox
            // 
            AuthorBox.FormattingEnabled = true;
            AuthorBox.Location = new Point(12, 1133);
            AuthorBox.Name = "AuthorBox";
            AuthorBox.Size = new Size(174, 33);
            AuthorBox.Sorted = true;
            AuthorBox.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(200, 1042);
            label3.Name = "label3";
            label3.Size = new Size(140, 25);
            label3.TabIndex = 7;
            label3.Text = "Starting Chapter";
            label3.Click += label3_Click;
            // 
            // ChapterUpDown
            // 
            ChapterUpDown.Location = new Point(200, 1071);
            ChapterUpDown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            ChapterUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            ChapterUpDown.Name = "ChapterUpDown";
            ChapterUpDown.Size = new Size(180, 31);
            ChapterUpDown.TabIndex = 9;
            ChapterUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            ChapterUpDown.ValueChanged += ChapterUpDown_ValueChanged;
            // 
            // ChatTextBox
            // 
            ChatTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ChatTextBox.Location = new Point(385, 1229);
            ChatTextBox.Name = "ChatTextBox";
            ChatTextBox.Size = new Size(1184, 31);
            ChatTextBox.TabIndex = 10;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Location = new Point(1575, 1229);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 11;
            button1.Text = "Send";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // ChatOutputBox
            // 
            ChatOutputBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ChatOutputBox.Location = new Point(385, 1044);
            ChatOutputBox.Multiline = true;
            ChatOutputBox.Name = "ChatOutputBox";
            ChatOutputBox.ScrollBars = ScrollBars.Vertical;
            ChatOutputBox.Size = new Size(1302, 179);
            ChatOutputBox.TabIndex = 12;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1712, 1302);
            Controls.Add(ChatOutputBox);
            Controls.Add(button1);
            Controls.Add(ChatTextBox);
            Controls.Add(ChapterUpDown);
            Controls.Add(AuthorBox);
            Controls.Add(StyleBox);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(ProgressBar);
            Controls.Add(BookTextBox);
            Controls.Add(GenerateBookButton);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel1.PerformLayout();
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ChapterUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox OutlineTextBox;
        private TextBox IdeaTextBox;
        private Button GenerateIdeaButton;
        private GroupBox groupBox2;
        private TextBox ChapterTextBox;
        private Button GenerateChapterButton;
        private GroupBox groupBox3;
        private Button GenerateIdeaBtn;
        private GroupBox groupBox4;
        private Button GenerateActsButton;
        private TextBox ActsText;
        private Button GenerateBookButton;
        private TextBox BookTextBox;
        private ProgressBar ProgressBar;
        private SplitContainer splitContainer1;
        private TreeView treeView1;
        private SplitContainer splitContainer3;
        private TreeView treeView3;
        private SplitContainer splitContainer2;
        private TreeView treeView2;
        private Label label1;
        private ComboBox StyleBox;
        private Label label2;
        private ComboBox AuthorBox;
        private Button GenerateTreeButton;
        private Label label3;
        private NumericUpDown ChapterUpDown;
        private TextBox ChatTextBox;
        private Button button1;
        private TextBox ChatOutputBox;
    }
}