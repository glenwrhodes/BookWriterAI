using System.Net.Http.Json;
using System;
using System.Text;
//using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics.Eventing.Reader;

namespace BookWriterAI
{

    public partial class Form1 : Form
    {
        ChatSession session = new ChatSession("", "");
        Book book = new Book();
        //ActList acts = new ActList();
        //ChapterList chapters = new ChapterList();
        ContentNode bookIdea = new ContentNode();
        ContentNode bookOutline = new ContentNode();
        //double temperature = 0.8;
        int NumActs = 6;
        string CharacterSummary = "";

        private string configFilePath = "config.json";

        public static string CreateFilenameFromString(string input)
        {
            // Replace any invalid characters with an underscore
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string validString = Regex.Replace(input, "[" + invalidChars + "]", "_");

            // Replace multiple consecutive underscores with a single underscore
            validString = Regex.Replace(validString, "_{2,}", "_");

            // Replace spaces with underscores
            validString = validString.Replace(' ', '_');

            // Remove any leading or trailing underscores
            validString = validString.Trim('_');

            return validString;
        }

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
                    session._temperature = double.Parse(config["Temperature"].ToString());
                }
            }
        }

        private void SaveConfiguration()
        {
            var config = new Dictionary<string, object>
            {
                { "ApiKey", session._apiKey },
                { "Model", session._model },
                { "Temperature", session._temperature }
            };

            string json = JsonConvert.SerializeObject(config);
            File.WriteAllText(configFilePath, json);
        }

        public Form1()
        {
            InitializeComponent();

            book = new Book();
            book.BookTitle = "Untitled";

            book.Chapters = new List<Chapter>();
            book.Chapters = new List<Chapter>();

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
            saveFileDialog.FileName = CreateFilenameFromString(book.BookTitle);

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
            openFileDialog.FileName = "";

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

                bookIdea = new ContentNode
                {
                    Depth = 0,
                    Title = "Book Idea",
                    Summary = book.ShortBookSummary,
                    Context = "Book Idea: " + IdeaTextBox.Text
                };

                bookOutline = new ContentNode
                {
                    Depth = 1,
                    Title = "Book Outline",
                    Summary = book.ShortBookSummary,
                    FullPlot = book.LongBookPlot,
                    Context = "Book Outline: " + book.LongBookPlot
                };

                bookIdea.AddChild(bookOutline);

                bookIdea.ContentGenerated += OnContentGenerated;
                bookOutline.ContentGenerated += OnContentGenerated;

                CharacterSummary = "Characters in this book: ";

                foreach (var c in book.Characters)
                {
                    CharacterSummary += GenerateCharacterSummary(c);
                }

                GenerateActsButton.Enabled = true;
                GenerateChapterButton.Enabled = true;

                UpdateTreeViewBook();
                UpdateTreeViewActs();
                UpdateTreeViewChapters();

            }
        }

        private void OnContentGenerated(string content)
        {
            // Update the BookTextBox with the new content
            BookTextBox.AppendText(content);
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

        public string GenerateCharacterSummaryForChapter(Chapter inChapter)
        {
            List<CharacterDetail> charactersInChapter = GetCharacterDetailsForChapter(book, inChapter);

            string CharacterSummary = "Characters in this chapter: ";

            foreach (var c in charactersInChapter)
            {
                CharacterSummary += GenerateCharacterSummary(c);
            }

            UpdateCharacterIntroductionStatus(book, inChapter);

            return CharacterSummary;
        }

        public void UpdateCharacterIntroductionStatus(Book book, Chapter chapter)
        {
            foreach (var characterId in chapter.CharactersPresent)
            {
                CharacterDetail characterDetail = book.Characters.FirstOrDefault(c => c.ID == characterId.ID);

                int index = book.Characters.IndexOf(characterDetail);
                CharacterDetail updatedCharacterDetail = characterDetail;
                updatedCharacterDetail.AlreadyIntroduced = true;
                book.Characters[index] = updatedCharacterDetail;

            }
        }

        public List<CharacterDetail> GetCharacterDetailsForChapter(Book book, Chapter chapter)
        {
            List<CharacterDetail> characterDetails = new List<CharacterDetail>();

            foreach (var characterId in chapter.CharactersPresent)
            {
                CharacterDetail characterDetail = book.Characters.FirstOrDefault(c => c.ID == characterId.ID);
                 characterDetails.Add(characterDetail);
            }

            return characterDetails;
        }

        public string GenerateCharacterSummary(CharacterDetail c)
        {
            string introductionStatus = c.AlreadyIntroduced
                ? "This character has already been introduced, so don't introduce again."
                : "This is the first time seeing this character in the book, so make sure to introduce the character.";

            return $"ID: {c.ID}) {c.FirstName} {c.LastName}, Age {c.Age}, {c.Gender}. {c.Appearance}. The {c.Role}. {introductionStatus}";
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
            propertiesForm.Temperature = session._temperature;

            if (propertiesForm.ShowDialog() == DialogResult.OK)
            {
                // Update values from the PropertiesForm
                session._apiKey = propertiesForm.ApiKey;
                session._model = propertiesForm.Model;
                session._temperature = propertiesForm.Temperature;

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

            string[] literaryStyles = new string[]
            {
                "Comedy",
                "Drama",
                "Epic",
                "Horror",
                "Mystery",
                "Romance",
                "Satire",
                "Science Fiction",
                "Fantasy",
                "Thriller",
                "Western",
                "Adventure",
                "Historical Fiction",
                "Dystopian",
                "Gothic",
                "Magical Realism",
                "Mythology",
                "Fairy Tale",
                "Crime",
                "Biography"
            };

            StyleBox.Items.AddRange(literaryStyles);


            string[] popularAuthors = new string[]
{
                "William Shakespeare",
                "Agatha Christie",
                "J.K. Rowling",
                "Stephen King",
                "Jane Austen",
                "Charles Dickens",
                "Ernest Hemingway",
                "Mark Twain",
                "George Orwell",
                "F. Scott Fitzgerald",
                "J.R.R. Tolkien",
                "Virginia Woolf",
                "Leo Tolstoy",
                "James Joyce",
                "Emily Bronte",
                "Oscar Wilde",
                "Haruki Murakami",
                "Gabriel Garcia Marquez",
                "John Steinbeck",
                "Arthur Conan Doyle"
};

            string[] modernAuthors = new string[]
            {
                "Michael Crichton",
                "Dan Brown",
                "Nicholas Sparks",
                "Jodi Picoult",
                "Neil Gaiman",
                "Khaled Hosseini",
                "Gillian Flynn",
                "Malcolm Gladwell",
                "Yuval Noah Harari",
                "Chimamanda Ngozi Adichie"
            };

            AuthorBox.Items.AddRange(popularAuthors);
            AuthorBox.Items.AddRange(modernAuthors);



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
                Depth = 0,
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

        // Generates the tree-based method
        private async void GenerateTreeButton_Click(object sender, EventArgs e)
        {
            await bookOutline.GenerateContent(session, 4, 0.8);
        }


        private async void GenerateIdeaButton_Click(object sender, EventArgs e)
        {
            GenerateIdeaButton.Enabled = false;
            GenerateIdeaBtn.Enabled = false;

            ProgressBar.Value = 50;

            ChatResponse response = await session.SendMessageAsync("Create a detailed plot for this book idea, of at least 250 pages: " + IdeaTextBox.Text + ". In the variable LongBookPlot, create an overall arc, with subplots, surprises and twists throughout. Be very detailed. This is the summary of the entire book. Return it in a JSON formatted string with fields, BookTitle, ShortBookSummary, LongBookPlot, SuggestedChapterCount (string), An array of Characters where each character has a FirstName, LastName, Age (string), Gender, ShortBio, FiveWordBio (eg. The best friend of John), Appearance, Role (Protagonist, protagonist's friend, Antagonist, etc), ID (int, a unique ID for each character).");

            // Set JsonSerializerOptions to use camelCase

            // Deserialize the response JSON to a ChatResponse object

            //var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            Debug.WriteLine(response.Choices[0].Message.Content);
            try
            {
                book = JsonConvert.DeserializeObject<Book>(response.Choices[0].Message.Content); //, jsonOptions);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error parsing JSON for detailed plot. " + ex.Message, "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ProgressBar.Value = 0;
                return;
            }

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
                Depth = 1,
                Title = "Book Outline",
                Summary = book.ShortBookSummary,
                FullPlot = book.LongBookPlot,
                Context = "Book Outline: " + book.LongBookPlot
            };

            bookOutline.ContentGenerated += OnContentGenerated;

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

            ActList acts = JsonConvert.DeserializeObject<ActList>(response.Choices[0].Message.Content); //, jsonOptions);
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

            if (book.Chapters != null)
                book.Chapters.Clear();

            book.Chapters = new List<Chapter>();

            UpdateTreeViewChapters();

            for (int i = 0; i < book.Acts.Count; i++)
            {
                ProgressBar.Value = (i + 1) * mult;

                int count = (int)MathF.Ceiling(float.Parse(book.SuggestedChapterCount) * percentages[i]);
                string actContext = "This is Act " + (i + 1) + " out of 6 acts. The act's plot is: " + book.Acts[i].ActFullPlot + ". ";

                // Adding context for the last chapter of the act
                string lastChapterContext = "";
                if (i < book.Acts.Count - 1)
                {
                    lastChapterContext = "The last chapter in this act should not wrap up the entire book or imply an ending, but rather transition smoothly into the next act.";
                }
                else
                {
                    lastChapterContext = "The last chapter in this act should serve as the conclusion to the entire book.";
                }

                ChatResponse response = await session.SendMessageAsync("Create " + count + " chapter outlines, starting at chapter " + cCount + " for act " + (i + 1) + " of a book. " + actContext + lastChapterContext + ". Think about which characters are present. Return it in a JSON formatted string with an array of chapters, each containing ChapterTitle, ChapterSummary, ChapterFullPlot, CurrentActNum (integer), CharactersPresent (Array) with property ID (int, the unique ID of the character from the book outline).");

                cCount += count;

                //var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                Debug.WriteLine(response.Choices[0].Message.Content);
                ChapterList tchapters = JsonConvert.DeserializeObject<ChapterList>(response.Choices[0].Message.Content);

                var json = JsonConvert.SerializeObject(tchapters, Formatting.Indented);
                Debug.WriteLine(json);
                ChapterTextBox.Text += json;
                book.Chapters.AddRange(tchapters.Chapters);

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
                    var t = bookIdea.FindNodeByTitle(book.Acts[i].ActTitle);

                    if (t != null)
                    {
                        t.Context += ". " + actContext;
                        t.AddChild(chapterNode);
                    }
                }

            }

            ProgressBar.Value = 0;
            //book.Chapters = chapters.Chapters;
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

                BookTextBox.Text += Environment.NewLine + chapterTitle + Environment.NewLine;

                // Break down the chapter into subchapters (e.g., 3 subchapters)

                string subchapterContent = ". ";

                //string BreakChapterString = "Break the following chapter into " + subchapterCount + " sections. The chapter is " + chapterTitle + " plot is " + chapterContext + ". Intelligently weave a connected plot between the sections. Do not expand beyond the scope of the specified plot. Output the result as a JSON object with an array of 'Sections', with elements SectionName, SectionPlot, SectionDetailedPlot.";
                string BreakChapterString = "Break the following chapter into " + subchapterCount + " sections. The chapter is " + chapterTitle + " plot is " + chapterContext + ". Intelligently weave a connected plot between the sections. Do not expand beyond the scope of the specified plot. Output the section plot with 4 paragraphs, separated by periods, indicating what should happen throughout the section. Output the result as a JSON object with an array of 'Sections', with elements SectionName, SectionPlot, SectionDetailedPlot, An array CharactersPresent with FirstName, LastName, for which characters are in this chapter.";

                ChatResponse subsectionResponse;
                try
                {
                    subsectionResponse = await session.SendMessageAsync(BreakChapterString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error talking to server for base subsection creation. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProgressBar.Value = 0;
                    break;
                }

                string subsectionContents = subsectionResponse.Choices[0].Message.Content.Trim();

                string jsonPattern = @"^(\{(?:[^{}]|(?<o>\{)|(?<-o>\}))+(?(o)(?!))\})";

                Match match = Regex.Match(subsectionContents, jsonPattern);
                if (match.Success)
                {

                    Debug.WriteLine(subsectionContents);
                    SubSections sections;

                    try
                    {
                        sections = JsonConvert.DeserializeObject<SubSections>(subsectionContents);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error parsing JSON for subsection. " + ex.Message, "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ProgressBar.Value = 0;
                        break;
                    }
                    var chapterNode = bookIdea.FindNodeByTitle(chapterTitle);

                    Act curAct = book.Acts[chapter.CurrentActNum - 1];

                    int sc = 0;

                    foreach (var s in sections.Sections)
                    {

                        ProgressBar.Value = osc * mult + 1;

                        //string subchapterPrompt = "Write the detailed narrative book prose of 500 words for this section of chapter " + (cc + 1) + ", where the section plot is: " + s.SectionDetailedPlot + ". Write about EXACTLY what is specified in the section plot, do not deviate or expand. For reference, the overall chapter is called " + chapterTitle + ". " + CharacterSummary;
                        // string subchapterPrompt = "To follow is a description of a narrative. I'd like to you expand upon it in more detail. Write the 200 word expansion like a novel with description, narrative and dialog. For some context, this section is part of chapter " + (cc + 1) + " in a book. The whole chapter is called: " + chapterTitle + ", and the book is called " + book.BookTitle + ", and " + CharacterSummary + ". Ok, the description to rewrite, expand on, embellish upon and write dialog for (in the past-tense, " + StyleBox.Text + " style of " + AuthorBox.Text + ") is: \"" + s.SectionDetailedPlot + "\". Write ONLY what is specified in the plot outline. Do not expand beyond the end of the plot outline. Write in the past tense ALWAYS please. Write now.";
                        string subchapterPrompt = "To follow is a description of a narrative. I'd like to you expand upon it in more detail. Write the 200 word expansion like a novel with description, narrative and dialog. For some context, this section is part of chapter " + (cc + 1) + " in a book. The whole chapter is called: " + chapterTitle + ", and the book is called " + book.BookTitle + ", and " + GenerateCharacterSummaryForChapter(chapter) + ". Ok, the description to rewrite, expand on, embellish upon and write dialog for (in the past-tense, " + StyleBox.Text + " style of " + AuthorBox.Text + ") is: \"" + s.SectionDetailedPlot + "\". Write ONLY what is specified in the plot outline. Do not expand beyond the end of the plot outline. Write in the past tense ALWAYS please. Write now.";
                        //string subchapterPrompt = "Write a detailed narrative book prose of 500 words for section " + (sc + 1) + " of " + subchapterCount + ", of chapter " + (cc + 1) + " in a book with " + book.SuggestedChapterCount + " chapters and " + NumActs + " acts. The current act is Act " + (chapter.CurrentActNum) + " with the plot: " + curAct.ActFullPlot + ". This section should follow the plot: \"" + s.SectionDetailedPlot + "\" and should not conclude the story. For reference, the overall chapter is called " + chapterTitle + " with the plot: " + chapterContext + ". " + CharacterSummary;
                        //string subchapterPrompt = "Write the detailed narrative book prose for part " + (i + 1) + " of " + subchapterCount + ", of the chapter titled '" + chapterTitle + "'. The chapter's plot is: " + chapterNode.FullPlot + ". Follow the previous section, which was:" + previousText;
                        //string subchapterPrompt = "Write the detailed narrative book prose to follow " + (i + 1) + " of " + subchapterCount + ", in the chapter titled '" + chapterTitle + "'. The expected chapter's plot is: " + chapterNode.GetFullContext() + ". The previous chapter was:" + previousText;
                        Debug.WriteLine(subchapterPrompt);

                        ChatResponse response;

                        try
                        {
                            response = await session.SendMessageAsync(subchapterPrompt);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error talking to server for narrative subsection creation. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ProgressBar.Value = 0;
                            break;
                        }


                        string pattern = @"\s*[^.!?]*[.!?](?=\s*$)";
                        string responseMinusLastSentence = Regex.Replace(response.Choices[0].Message.Content, pattern, "");
                        subchapterContent = "\n" + responseMinusLastSentence;

                        ContentNode subchapterNode = new ContentNode()
                        {
                            Title = chapterTitle + ", Subchapter " + (sc + 1),
                            FullPlot = response.Choices[0].Message.Content
                        };

                        previousText = responseMinusLastSentence;

                        if (chapterNode != null)
                        {
                            chapterNode.AddChild(subchapterNode);
                        }

                        BookTextBox.Text += Environment.NewLine + Environment.NewLine + responseMinusLastSentence;

                        osc++;
                        sc++;
                    }

                    // Combine the subchapter content to form the full chapter
                    string fullChapter = chapterTitle + "\n" + subchapterContent + "\n\n";
                    cc++;
                    // Append the chapter to the final book content
                    //BookTextBox.Text += fullChapter;
                }
                else
                {
                    MessageBox.Show("Couldn't parse JSON from ChatGPT in generation of book subsections. " + session._model + " may have sent malformed JSON.", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Debug.WriteLine("Couldn't parse JSON from ChatGPT in generation of book subsections. " + session._model + " may have sent malformed JSON.");
                    break;
                }

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
