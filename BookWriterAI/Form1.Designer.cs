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
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(splitContainer1);
            groupBox1.Controls.Add(GenerateIdeaButton);
            groupBox1.Location = new Point(16, 185);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1646, 267);
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
            splitContainer1.Size = new Size(1482, 218);
            splitContainer1.SplitterDistance = 30;
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
            OutlineTextBox.Size = new Size(30, 218);
            OutlineTextBox.TabIndex = 2;
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(1448, 218);
            treeView1.TabIndex = 0;
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
            IdeaTextBox.Location = new Point(144, 30);
            IdeaTextBox.Multiline = true;
            IdeaTextBox.Name = "IdeaTextBox";
            IdeaTextBox.ScrollBars = ScrollBars.Vertical;
            IdeaTextBox.Size = new Size(1484, 68);
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
            groupBox2.Size = new Size(1632, 315);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Chapters";
            // 
            // splitContainer3
            // 
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
            splitContainer3.Size = new Size(1482, 271);
            splitContainer3.SplitterDistance = 35;
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
            ChapterTextBox.Size = new Size(35, 271);
            ChapterTextBox.TabIndex = 4;
            // 
            // treeView3
            // 
            treeView3.Dock = DockStyle.Fill;
            treeView3.Location = new Point(0, 0);
            treeView3.Name = "treeView3";
            treeView3.Size = new Size(1443, 271);
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
            groupBox3.Size = new Size(1646, 117);
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
            groupBox4.Size = new Size(1646, 259);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Acts";
            // 
            // splitContainer2
            // 
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
            splitContainer2.Size = new Size(1482, 211);
            splitContainer2.SplitterDistance = 33;
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
            ActsText.Size = new Size(33, 211);
            ActsText.TabIndex = 4;
            // 
            // treeView2
            // 
            treeView2.Dock = DockStyle.Fill;
            treeView2.Location = new Point(0, 0);
            treeView2.Name = "treeView2";
            treeView2.Size = new Size(1445, 211);
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
            GenerateBookButton.Location = new Point(16, 1044);
            GenerateBookButton.Name = "GenerateBookButton";
            GenerateBookButton.Size = new Size(137, 34);
            GenerateBookButton.TabIndex = 4;
            GenerateBookButton.Text = "Generate Book";
            GenerateBookButton.UseVisualStyleBackColor = true;
            GenerateBookButton.Click += GenerateBookButton_Click;
            // 
            // BookTextBox
            // 
            BookTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            BookTextBox.Location = new Point(160, 1044);
            BookTextBox.Multiline = true;
            BookTextBox.Name = "BookTextBox";
            BookTextBox.ReadOnly = true;
            BookTextBox.ScrollBars = ScrollBars.Vertical;
            BookTextBox.Size = new Size(1488, 168);
            BookTextBox.TabIndex = 5;
            // 
            // ProgressBar
            // 
            ProgressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ProgressBar.Location = new Point(16, 1227);
            ProgressBar.Name = "ProgressBar";
            ProgressBar.Size = new Size(1632, 26);
            ProgressBar.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1674, 1265);
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
    }
}