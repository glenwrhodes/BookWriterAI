using System.Net.Http.Json;
using System;
using System.Text;
//using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace BookWriterAI
{

    public partial class Form1 : Form
    {
        ChatSession session = new ChatSession("", "");
        Book book = new Book();
        ActList acts = new ActList();
        ChapterList chapters = new ChapterList();
        ContentNode bookIdea = new ContentNode();
        ContentNode bookOutline = new ContentNode();
        double temperature = 0.8;
        int NumActs = 6;
        string CharacterSummary = "";

        private string configFilePath = "config.json";

        private void LoadConfiguration()
        {
            if (File.Exists(configFilePath))
            {
                string json = File.ReadAllText(configFilePath);

                var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                if (config != null)
                {
                    session._apiKey = config["ApiKey"].ToString();
                    session._model = config["Model"].ToString();
                    temperature = double.Parse(config["Temperature"].ToString());
                }
            }
        }

        private void SaveConfiguration()
        {
            var config = new Dictionary<string, object>
            {
                { "ApiKey", session._apiKey },
                { "Model", session._model },
                { "Temperature", temperature }
            };

            string json = JsonConvert.SerializeObject(config);
            File.WriteAllText(configFilePath, json);
        }

        public Form1()
        {
            InitializeComponent();
            chapters.Chapters = new List<Chapter>();
            acts.Chapters = new List<Chapter>();

            string model = "gpt-4";
            string apiKey = "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";

            // Create a new ChatSession instance
            session = new ChatSession(model, apiKey);
            //SaveConfiguration();
            LoadConfiguration();

            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem fileMenuItem = new ToolStripMenuItem("File");
            ToolStripMenuItem propertiesMenuItem = new ToolStripMenuItem("Properties");
            ToolStripMenuItem jsonExportMenuItem = new ToolStripMenuItem("Export Tree");
            ToolStripMenuItem jsonSave = new ToolStripMenuItem("Save Book");
            ToolStripMenuItem jsonLoad = new ToolStripMenuItem("Load Book");

            fileMenuItem.DropDownItems.Add(propertiesMenuItem);
            fileMenuItem.DropDownItems.Add(jsonExportMenuItem);
            fileMenuItem.DropDownItems.Add(jsonSave);
            fileMenuItem.DropDownItems.Add(jsonLoad);

            menuStrip.Items.Add(fileMenuItem);
            this.Controls.Add(menuStrip);

            propertiesMenuItem.Click += PropertiesMenuItem_Click;
            jsonExportMenuItem.Click += jsonExportMenuItem_Click;
            jsonSave.Click += jsonSave_Click;
            jsonLoad.Click += jsonLoad_Click;
        }


        private void jsonSave_Click(object? sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Book Files (*.bk)|*.bk|All Files (*.*)|*.*";
            saveFileDialog.DefaultExt = "bk";
            saveFileDialog.FileName = "untitled";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                var bookJson = JsonConvert.SerializeObject(book); //, jsonOptions);
                File.WriteAllText(filePath, bookJson);
            }
        }

        private void jsonLoad_Click(object? sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Book Files (*.bk)|*.bk|All Files (*.*)|*.*";
            openFileDialog.DefaultExt = "bk";
            openFileDialog.FileName = "untitled";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                var bookJson = File.ReadAllText(filePath);
                book = JsonConvert.DeserializeObject<Book>(bookJson); //, jsonOptions);

                var json = JsonConvert.SerializeObject(book, Formatting.Indented);
                OutlineTextBox.Text = json;

                json = JsonConvert.SerializeObject(book.Acts, Formatting.Indented);
                ActsText.Text = json;

                json = JsonConvert.SerializeObject(book.Chapters, Formatting.Indented);
                ChapterTextBox.Text = json;

                CharacterSummary = "Characters in this book: ";

                foreach (var c in book.Characters)
                {
                    CharacterSummary += GenerateCharacterSummary(c);
                }

                UpdateTreeViewBook();
                UpdateTreeViewActs();
                UpdateTreeViewChapters();

            }
        }

        public void UpdateTreeViewBook()
        {
            PopulateTreeViewFromJson(treeView1, JsonConvert.SerializeObject(book));
            treeView1.Nodes[0].Text = "Book - " + book.BookTitle;
        }

        public void UpdateTreeViewActs()
        {
            PopulateTreeViewFromJson(treeView2, "{\"Acts\":" + JsonConvert.SerializeObject(book.Acts) + "}");
            treeView2.Nodes[0].Text = "Book - " + book.BookTitle;
        }

        public void UpdateTreeViewChapters()
        {
            PopulateTreeViewFromJson(treeView3, "{\"Chapters\":" + JsonConvert.SerializeObject(book.Chapters) + "}");
            treeView3.Nodes[0].Text = "Book - " + book.BookTitle;
        }

        public string GenerateCharacterSummary(CharacterDetail c)
        {
            return (c.FirstName + " " + c.LastName + ", " + c.Age + " age, " + c.Gender + ". " + c.Appearance + ". ");
        }

        private void jsonExportMenuItem_Click(object? sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
            saveFileDialog.DefaultExt = "json";
            saveFileDialog.FileName = "savedBook";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                string json = bookIdea.ToJson();
                File.WriteAllText(filePath, json);
            }
        }

        private void PropertiesMenuItem_Click(object? sender, EventArgs e)
        {
            PropertiesForm propertiesForm = new PropertiesForm();
            // Set current values
            propertiesForm.ApiKey = session._apiKey;
            propertiesForm.Model = session._model;
            propertiesForm.Temperature = temperature;

            if (propertiesForm.ShowDialog() == DialogResult.OK)
            {
                // Update values from the PropertiesForm
                session._apiKey = propertiesForm.ApiKey;
                session._model = propertiesForm.Model;
                temperature = propertiesForm.Temperature;

                // Save the configuration to a file
                SaveConfiguration();
            }

            //throw new Exception();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Define the API parameters

            GenerateIdeaButton.Enabled = false;
            GenerateActsButton.Enabled = false;
            GenerateChapterButton.Enabled = false;
        }

        private async Task<string> SendMessageAsync(string message)
        {
            ChatResponse response = await session.SendMessageAsync(message);
            return response.Choices[0].Message.Content;
        }

        private async void GenerateIdeaBtn_Click(object sender, EventArgs e)
        {
            GenerateIdeaBtn.Enabled = false;
            GenerateIdeaButton.Enabled = false;
            GenerateActsButton.Enabled = false;
            GenerateChapterButton.Enabled = false;
            ProgressBar.Value = 80;
            ChatResponse response = await session.SendMessageAsync("Generate a random book idea, and return to me just that idea, no preamble. Be creative. Do not encapsulate in quotes.");
            IdeaTextBox.Text = response.Choices[0].Message.Content;
            session.Reset();
            ProgressBar.Value = 0;
            bookIdea = new ContentNode
            {
                Title = "Book Idea",
                Summary = IdeaTextBox.Text,
                Context = "Book Idea: " + IdeaTextBox.Text
            };

            GenerateIdeaBtn.Enabled = true;
            GenerateIdeaButton.Enabled = true;
        }


        private void PopulateTreeViewFromJson(TreeView treeView, string jsonString)
        {
            JObject jsonObject = JObject.Parse(jsonString);
            treeView.Nodes.Clear();
            TreeNode rootNode = new TreeNode("Book");
            treeView.Nodes.Add(rootNode);
            PopulateNodeWithJToken(rootNode, jsonObject);
        }

        private void PopulateNodeWithJToken(TreeNode node, JToken token)
        {
            if (token is JObject jobject)
            {
                foreach (KeyValuePair<string, JToken> property in jobject)
                {
                    TreeNode propertyNode = new TreeNode($"{property.Key}");
                    node.Nodes.Add(propertyNode);
                    PopulateNodeWithJToken(propertyNode, property.Value);
                }
            }
            else if (token is JArray jarray)
            {
                for (int i = 0; i < jarray.Count; i++)
                {
                    TreeNode arrayNode = new TreeNode($"[{i}]");
                    node.Nodes.Add(arrayNode);
                    PopulateNodeWithJToken(arrayNode, jarray[i]);
                }
            }
            else
            {
                node.Text += $": {token.ToString()}";
            }
        }


        private async void GenerateIdeaButton_Click(object sender, EventArgs e)
        {
            GenerateIdeaButton.Enabled = false;
            GenerateIdeaBtn.Enabled = false;

            ProgressBar.Value = 50;

            ChatResponse response = await session.SendMessageAsync("Create a detailed plot for this book idea, of at least 250 pages: " + IdeaTextBox.Text + ". Return it in a JSON formatted string with fields, BookTitle, ShortBookSummary, LongBookPlot, SuggestedChapterCount (string), An array of Characters where each character has a FirstName, LastName, Age (string), Gender, ShortBio, FiveWordBio, Appearance.");

            // Set JsonSerializerOptions to use camelCase

            // Deserialize the response JSON to a ChatResponse object

            //var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            Debug.WriteLine(response.Choices[0].Message.Content);
            book = JsonConvert.DeserializeObject<Book>(response.Choices[0].Message.Content); //, jsonOptions);
            //book.SuggestedChapterCount = "26"; // Temporary
            var json = JsonConvert.SerializeObject(book, Formatting.Indented);

            UpdateTreeViewBook();

            CharacterSummary = "Characters in this book: ";

            foreach (var c in book.Characters)
            {
                CharacterSummary += GenerateCharacterSummary(c);
            }

            Debug.WriteLine(CharacterSummary.Length + " " + CharacterSummary);

            Debug.WriteLine(json);
            OutlineTextBox.Text = json;
            GenerateIdeaButton.Enabled = true;
            GenerateActsButton.Enabled = true;
            GenerateIdeaBtn.Enabled = true;

            ProgressBar.Value = 0;

            bookOutline = new ContentNode
            {
                Title = "Book Outline",
                Summary = book.ShortBookSummary,
                FullPlot = book.LongBookPlot,
                Context = "Book Outline: " + book.LongBookPlot
            };

            bookIdea.AddChild(bookOutline);
        }


        private async void GenerateActsButton_Click(object sender, EventArgs e)
        {
            GenerateActsButton.Enabled = false;

            ProgressBar.Value = 50;

            ChatResponse response = await session.SendMessageAsync("Divide the book into " + NumActs + " main acts, and generate detailed act outlines, including specific plot details, for each act for this book idea: " + book.LongBookPlot + ". Return it in a JSON formatted string with array of Acts, each containing ActTitle, ActSummary, ActFullPlot");

            // Set JsonSerializerOptions to use camelCase

            // Deserialize the response JSON to a ChatResponse object
            //var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            acts = JsonConvert.DeserializeObject<ActList>(response.Choices[0].Message.Content); //, jsonOptions);
            var json = JsonConvert.SerializeObject(acts, Formatting.Indented);
            Debug.WriteLine(json);
            ActsText.Text = json;
            book.Acts = acts.Acts;

            ProgressBar.Value = 0;

            UpdateTreeViewBook();
            UpdateTreeViewActs();

            foreach (var act in book.Acts)
            {
                ContentNode actNode = new ContentNode
                {
                    Title = act.ActTitle,
                    Summary = act.ActSummary,
                    FullPlot = act.ActFullPlot,
                    Context = act.ActTitle + ": " + act.ActSummary
                };
                bookOutline.AddChild(actNode);
            }

            GenerateActsButton.Enabled = true;
            GenerateChapterButton.Enabled = true;
        }

        private async void GenerateChapterButton_Click(object sender, EventArgs e)
        {
            GenerateChapterButton.Enabled = false;
            ChapterTextBox.Text = "";

            float[] percentages = new[] { 0.15f, 0.15f, 0.2f, 0.2f, 0.15f, 0.15f };

            int cCount = 1;
            int mult = 100 / NumActs;

            for (int i = 0; i < acts.Acts.Count; i++)
            {
                ProgressBar.Value = (i + 1) * mult;

                int count = (int)MathF.Ceiling(float.Parse(book.SuggestedChapterCount) * percentages[i]);
                string actContext = "This is Act " + (i + 1) + " out of 6 acts. The act's plot is: " + acts.Acts[i].ActFullPlot + ". ";

                // Adding context for the last chapter of the act
                string lastChapterContext = "";
                if (i < acts.Acts.Count - 1)
                {
                    lastChapterContext = "The last chapter in this act should not wrap up the entire book or imply an ending, but rather transition smoothly into the next act.";
                }
                else
                {
                    lastChapterContext = "The last chapter in this act should serve as the conclusion to the entire book.";
                }

                ChatResponse response = await session.SendMessageAsync("Create " + count + " chapter outlines, starting at chapter " + cCount + " for act " + (i + 1) + " of a book. " + actContext + lastChapterContext + " Return it in a JSON formatted string with an array of chapters, each containing ChapterTitle, ChapterSummary, ChapterFullPlot");

                cCount += count;

                //var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                ChapterList tchapters = JsonConvert.DeserializeObject<ChapterList>(response.Choices[0].Message.Content);

                var json = JsonConvert.SerializeObject(tchapters, Formatting.Indented);
                Debug.WriteLine(json);
                ChapterTextBox.Text += json;
                chapters.Chapters.AddRange(tchapters.Chapters);

                foreach (var chapter in tchapters.Chapters)
                {
                    ContentNode chapterNode = new ContentNode
                    {
                        Title = chapter.ChapterTitle,
                        Summary = chapter.ChapterSummary,
                        FullPlot = chapter.ChapterFullPlot,
                        Context = "Chapter: " + chapter.ChapterTitle
                    };
                    // Assuming you have a reference to the corresponding act node
                    var t = bookIdea.FindNodeByTitle(acts.Acts[i].ActTitle);

                    if (t != null)
                    {
                        t.Context += ". " + actContext;
                        t.AddChild(chapterNode);
                    }
                }

            }

            ProgressBar.Value = 0;
            book.Chapters = chapters.Chapters;
            UpdateTreeViewBook();
            UpdateTreeViewChapters();
            GenerateChapterButton.Enabled = true;
        }



        private async void GenerateBookButton_Click(object sender, EventArgs e)
        {
            GenerateBookButton.Enabled = false;
            BookTextBox.Text = "";
            int subchapterCount = 3;

            string previousText = "";

            int cnt = 0;
            int cc = 0;
            int mult = 100 / (book.Chapters.Count() * subchapterCount);
            int osc = 0;

            ProgressBar.Value = 1;

            // Assuming chapters variable contains the generated chapters from the previous function
            foreach (var chapter in book.Chapters)
            {

                string chapterTitle = chapter.ChapterTitle;
                string chapterContext = chapter.ChapterFullPlot;

                BookTextBox.Text += Environment.NewLine + chapterTitle + Environment.NewLine + Environment.NewLine;

                // Break down the chapter into subchapters (e.g., 3 subchapters)

                string subchapterContent = ". ";

                string BreakChapterString = "Break the following chapter into " + subchapterCount + " sections. The chapter is " + chapterTitle + " plot is " + chapterContext + ". Do not expand beyond the scope of the specified plot. Return the result as a JSON object with an array of 'Sections', with elements SectionName, SectionPlot, SectionDetailedPlot.";

                ChatResponse subsectionResponse = await session.SendMessageAsync(BreakChapterString);

                string subsectionContents = subsectionResponse.Choices[0].Message.Content.Trim();

                Debug.WriteLine(subsectionContents);
                SubSections sections = JsonConvert.DeserializeObject<SubSections>(subsectionContents);

                var chapterNode = bookIdea.FindNodeByTitle(chapterTitle);

                int sc = 0;

                foreach (var s in sections.Sections)
                {

                    ProgressBar.Value = osc * mult + 1;

                    string subchapterPrompt = "Write the detailed narrative book prose of 500 words for this section of chapter " + (cc + 1) + ", where the section plot is: " + s.SectionDetailedPlot + ". Write about EXACTLY what is specified in the section plot, do not deviate or expand. For reference, the overall chapter is called " + chapterTitle + ". " + CharacterSummary;
                    //string subchapterPrompt = "Write the detailed narrative book prose for part " + (i + 1) + " of " + subchapterCount + ", of the chapter titled '" + chapterTitle + "'. The chapter's plot is: " + chapterNode.FullPlot + ". Follow the previous section, which was:" + previousText;
                    //string subchapterPrompt = "Write the detailed narrative book prose to follow " + (i + 1) + " of " + subchapterCount + ", in the chapter titled '" + chapterTitle + "'. The expected chapter's plot is: " + chapterNode.GetFullContext() + ". The previous chapter was:" + previousText;
                    Debug.WriteLine(subchapterPrompt);

                    ChatResponse response = await session.SendMessageAsync(subchapterPrompt);

                    subchapterContent = "\n\n" + response.Choices[0].Message.Content;

                    ContentNode subchapterNode = new ContentNode()
                    {
                        Title = chapterTitle + ", Subchapter " + (sc + 1),
                        FullPlot = response.Choices[0].Message.Content
                    };

                    previousText = subchapterContent;

                    if (chapterNode != null)
                    {
                        chapterNode.AddChild(subchapterNode);
                    }

                    BookTextBox.Text += Environment.NewLine + Environment.NewLine + subchapterContent;

                    osc++;
                    sc++;
                }

                // Combine the subchapter content to form the full chapter
                string fullChapter = chapterTitle + "\n" + subchapterContent + "\n\n";
                cc++;
                // Append the chapter to the final book content
                //BookTextBox.Text += fullChapter;
            }

            ProgressBar.Value = 0;

            GenerateBookButton.Enabled = true;
        }

        private void IdeaTextBox_TextChanged(object sender, EventArgs e)
        {
            GenerateIdeaButton.Enabled = true;
        }
    }
}
