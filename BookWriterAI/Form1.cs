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
using Xceed.Words.NET;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using System.Drawing.Printing;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Microsoft.VisualBasic.FileIO;
using iText.Kernel.Events;

using iTextParagraph = iText.Layout.Element.Paragraph;
using XceedParagraph = Xceed.Document.NET.Paragraph;

//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BookWriterAI
{

    public partial class Form1 : Form
    {


//
        // The ChatSession instance
        public ChatSession session = new ChatSession("", "");

        // The Book object
        public Book book = new Book();
        //ActList acts = new ActList();
        //ChapterList chapters = new ChapterList();

        // The Book Idea node which is a ContentNode that is the root of the book outline
        public ContentNode bookIdea = new ContentNode();

        // The Book Outline node which is a ContentNode that is the root of the book outline
        public ContentNode bookOutline = new ContentNode();
        //double temperature = 0.8;

        // The number of acts in the book
        public int NumActs = 6;

        // The max number of acts in the book
        public int MaxChapters = 36;

        // The summary of the characters in the book
        public string CharacterSummary = "";

        // The path to the configuration file
        private string configFilePath = "config.json";

        string previousText = "Nevermind, ignore this. ";

        int cc = 0; // Chapter count
        int osc = 0; // Overall subchapter count

        int subchapterCount = 3; // Number of subchapters per chapter
        int subSubChapterCount = 4; // Number of sub-subchapters per subchapter
        int mult = 6; // Multiplier for subchapter count

        int startingChapter = 0;

        // Creates a filename from a string by replacing invalid characters with underscores and removing leading and trailing underscores and spaces
        public static string CreateFilenameFromString(string input)
        {
            // Replace any invalid characters with an underscore
            string invalidChars = Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string validString = Regex.Replace(input, "[" + invalidChars + "]", "_");

            // Replace multiple consecutive underscores with a single underscore
            validString = Regex.Replace(validString, "_{2,}", "_");

            // Replace spaces with underscores
            validString = validString.Replace(' ', '_');

            // Remove any leading or trailing underscores
            validString = validString.Trim('_');

            return validString;
        }

        // Load the configuration from a file
        private void LoadConfiguration()
        {
            if (File.Exists(configFilePath))
            {
                // Load the configuration from the file
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

        public void ExportBookToPdf(Book book, string fileName)
        {
            //string fileName = book.BookTitle + ".pdf";

                PdfWriter writer = new PdfWriter(fileName);
                PdfDocument pdfDoc = new PdfDocument(writer);
                iText.Layout.Document document = new iText.Layout.Document(pdfDoc, new PageSize(5 * 72, 8 * 72), false);

                // Set the margins
                document.SetMargins(72 * 0.6f, 72 * 0.5f, 72 * 0.5f, 72 * 0.6f);

                // Add the title page
                Paragraph title = new Paragraph(book.BookTitle)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetFontSize(28);
                document.Add(title);
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new PageNumberEventHandler());

                // Add the chapters
                foreach (Chapter chapter in book.Chapters)
                {

                    string cTitle = chapter.ChapterTitle;
                    cTitle = Regex.Replace(cTitle, @"^Chapter \d+:", "");
                    cTitle.Trim();

                    // Add the chapter title
                    Paragraph chapterTitle = new Paragraph(cTitle)
                        .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFontSize(12);
                    document.Add(chapterTitle);

                    // Add the chapter sections
                    if (chapter.Sections != null)
                    {
                        foreach (SubSection section in chapter.Sections)
                        {
                            if (!string.IsNullOrEmpty(section.GeneratedText))
                            {
                                // Add the section text
                                Paragraph sectionText = new Paragraph(section.GeneratedText)
                                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                                    .SetTextAlignment(TextAlignment.JUSTIFIED)
                                    .SetFontSize(9);
                                document.Add(sectionText);
                            }
                        }

                    }

                    // Add a new page for the next chapter
                    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                }

                // Add page numbers to the bottom margin
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                // Close the document
                document.Close();
            
        }

        public void ExportBookToDocX(Book book, string fileName)
        {

                using (DocX document = DocX.Create(fileName))
                {
                    // Add the title page
                    XceedParagraph title = document.InsertParagraph(book.BookTitle)
                        .FontSize(28)
                        .Bold();
                    title.Alignment = Xceed.Document.NET.Alignment.center;

                    document.InsertSectionPageBreak();

                    // Add the chapters
                    foreach (Chapter chapter in book.Chapters)
                    {
                        string cTitle = chapter.ChapterTitle;
                        cTitle = Regex.Replace(cTitle, @"^Chapter \d+:", "");
                        cTitle.Trim();

                        // Add the chapter title
                        XceedParagraph chapterTitle = document.InsertParagraph(cTitle)
                            .FontSize(16)
                            .Bold();
                        chapterTitle.Alignment = Xceed.Document.NET.Alignment.left;

                        // Add the chapter sections
                        if (chapter.Sections != null)
                        {
                            foreach (SubSection section in chapter.Sections)
                            {
                                if (!string.IsNullOrEmpty(section.GeneratedText))
                                {
                                    // Add the section text
                                    XceedParagraph sectionText = document.InsertParagraph(section.GeneratedText + Environment.NewLine).FontSize(14);
                                    sectionText.Alignment = Xceed.Document.NET.Alignment.left;
                                }
                            }
                        }

                        // Add a new page for the next chapter
                        document.InsertSectionPageBreak();
                    }

                    // Save the document
                    document.Save();
            }
        }

        public void ExportBookToTxt(Book book, string fileName)
        {

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                // Add the title page
                writer.WriteLine(book.BookTitle);
                writer.WriteLine(); // Empty line to separate content

                // Add the chapters
                foreach (Chapter chapter in book.Chapters)
                {
                    string cTitle = chapter.ChapterTitle;
                    cTitle = Regex.Replace(cTitle, @"^Chapter \d+:", "");
                    cTitle.Trim();

                    // Add the chapter title
                    writer.WriteLine(cTitle);
                    writer.WriteLine(); // Empty line to separate content

                    // Add the chapter sections
                    if (chapter.Sections != null)
                    {
                        foreach (SubSection section in chapter.Sections)
                        {
                            if (!string.IsNullOrEmpty(section.GeneratedText))
                            {
                                // Add the section text
                                writer.WriteLine(section.GeneratedText);
                                writer.WriteLine(); // Empty line to separate content
                            }
                        }
                    }

                    writer.WriteLine(); // Empty line to separate chapters
                }
            }

        }

        public void ExportBook(Book book)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PDF Files (*.pdf)|*.pdf|Word Documents (*.docx)|*.docx|Text Files (*.txt)|*.txt";
            saveFileDialog1.Title = "Save Book";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                string fileExtension = System.IO.Path.GetExtension(saveFileDialog1.FileName);

                switch (fileExtension.ToLower())
                {
                    case ".pdf":
                        ExportBookToPdf(book, saveFileDialog1.FileName);
                        break;
                    case ".docx":
                        ExportBookToDocX(book, saveFileDialog1.FileName);
                        break;
                    case ".txt":
                        ExportBookToTxt(book, saveFileDialog1.FileName);
                        break;
                    default:
                        MessageBox.Show("Invalid file format selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
        }

        // Save the configuration to a file
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

        bool IsChapterDone(Chapter chapter)
        {
            foreach (var section in chapter.Sections)
            {
                if (string.IsNullOrEmpty(section.GeneratedText))
                {
                    return false;
                }
            }
            return true;
        }

        bool IsBookDone(Book book)
        {
            foreach (var chapter in book.Chapters)
            {
                if (!IsChapterDone(chapter))
                {
                    return false;
                }
            }
            return true;
        }

        string ExportBookToString(Book book)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var chapter in book.Chapters)
            {
                sb.AppendLine(chapter.ChapterTitle);

                foreach (var section in chapter.Sections)
                {
                    sb.AppendLine(section.GeneratedText);
                    sb.AppendLine();
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public Form1()
        {
            InitializeComponent();

            book = new Book();
            book.BookTitle = "Untitled";

            book.Chapters = new List<Chapter>();
            book.Chapters = new List<Chapter>();

            string model = "gpt-4";
            string apiKey = "sk-YOUR_API_KEY";

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
            ToolStripMenuItem doExport = new ToolStripMenuItem("Export Book");

            fileMenuItem.DropDownItems.Add(propertiesMenuItem);
            fileMenuItem.DropDownItems.Add(jsonExportMenuItem);
            fileMenuItem.DropDownItems.Add(jsonSave);
            fileMenuItem.DropDownItems.Add(jsonLoad);
            fileMenuItem.DropDownItems.Add(doExport);

            menuStrip.Items.Add(fileMenuItem);
            this.Controls.Add(menuStrip);

            propertiesMenuItem.Click += PropertiesMenuItem_Click;
            jsonExportMenuItem.Click += jsonExportMenuItem_Click;
            jsonSave.Click += jsonSave_Click;
            jsonLoad.Click += jsonLoad_Click;
            doExport.Click += doExport_Click;

        }

        private void doExport_Click(object? sender, EventArgs e)
        {
            ExportBook(book);
        }

        /// <summary>
        /// Called when the user clicks the Properties menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jsonSave_Click(object? sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Book Files (*.bk)|*.bk|All Files (*.*)|*.*";
            saveFileDialog.DefaultExt = "bk";
            saveFileDialog.FileName = CreateFilenameFromString(book.BookTitle);

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                var bookJson = JsonConvert.SerializeObject(book, Formatting.Indented); //, jsonOptions);
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

                //if (int.Parse(book.SuggestedChapterCount) > MaxChapters) book.SuggestedChapterCount = MaxChapters.ToString();

                UpdateTreeViewBook();
                UpdateTreeViewActs();
                UpdateTreeViewChapters();

            }
        }

        // A delegate type for hooking up change notifications.
        private void OnContentGenerated(string content)
        {
            // Update the BookTextBox with the new content
            BookTextBox.AppendText(content);
        }

        // Updates the tree view with the current book
        public void UpdateTreeViewBook()
        {
            PopulateTreeViewFromJson(treeView1, JsonConvert.SerializeObject(book));
            treeView1.Nodes[0].Text = "Book - " + book.BookTitle;
            treeView1.Nodes[0].Expand();
        }

        // Updates the tree view with the current acts
        public void UpdateTreeViewActs()
        {
            PopulateTreeViewFromJson(treeView2, "{\"Acts\":" + JsonConvert.SerializeObject(book.Acts) + "}");
            treeView2.Nodes[0].Text = "Acts - " + book.BookTitle;
            treeView2.Nodes[0].Expand();
        }

        // Updates the tree view with the current chapters
        public void UpdateTreeViewChapters()
        {
            PopulateTreeViewFromJson(treeView3, "{\"Chapters\":" + JsonConvert.SerializeObject(book.Chapters) + "}");
            treeView3.Nodes[0].Text = "Chapters - " + book.BookTitle;
            treeView3.Nodes[0].Expand();

            ChapterUpDown.Maximum = book.Chapters.Count;

        }

        // Generates the overall summary details for the characters in the book
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

        // Update the character details to indicate that they have been introduced in the chapter
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

        // Get a list of character details for the characters present in the chapter
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

        // Generate a summary of the character including the fields: ID, FirstName, LastName, Age, Gender, Appearance, Role, and AlreadyIntroduced
        public string GenerateCharacterSummary(CharacterDetail c)
        {
            string introductionStatus = c.AlreadyIntroduced
                ? "This character has already been introduced, so don't introduce or describe again."
                : c.ShortBio + ". This is the first time seeing this character in the book, so make sure to introduce and describe the character. ";

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

        // The form load
        private void Form1_Load(object sender, EventArgs e)
        {
            // Define the API parameters

            // Disable the generation buttons until the user has entered an idea
            GenerateIdeaButton.Enabled = false;
            GenerateActsButton.Enabled = false;
            GenerateChapterButton.Enabled = false;

            // Populate the stylebox with the list of literary styles
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

            // Populate the author box with the list of popular authors
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

        // Generates a short book idea.
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

        // Fills the tree view with the JSON data
        private void PopulateTreeViewFromJson(TreeView treeView, string jsonString)
        {
            JObject jsonObject = JObject.Parse(jsonString);
            treeView.Nodes.Clear();
            TreeNode rootNode = new TreeNode("Book");
            treeView.Nodes.Add(rootNode);
            PopulateNodeWithJToken(rootNode, jsonObject);
        }

        // Fills the tree view with the JSON data
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
                    JToken firstChildToken = jarray[i].Children().FirstOrDefault();
                    string nodeName = firstChildToken != null ? $"{i + 1}. {firstChildToken.First}" : $"[{i + 1}]";
                    TreeNode arrayNode = new TreeNode(nodeName);
                    node.Nodes.Add(arrayNode);
                    PopulateNodeWithJToken(arrayNode, jarray[i]);
                }
            }
            else
            {
                node.Text += $": {token.ToString()}";
            }
        }

        // Generate a gaussian distribution of percentages that sum up to 1, used to generate the number of chapters in each act.
        public static float[] GenerateChapterPercentages(int numActs, float peak, float width, float minValue = 0.05f)
        {
            float[] percentages = new float[numActs];
            float sum = 0;

            for (int i = 0; i < numActs; i++)
            {
                float x = (float)i / (numActs - 1); // Normalize i to a value between 0 and 1
                percentages[i] = peak * (float)Math.Exp(-(Math.Pow(x - 0.5, 2) / (2 * Math.Pow(width, 2))));
                sum += percentages[i];
            }
            // Normalize the percentages so they sum up to 1
            for (int i = 0; i < numActs; i++)
            {
                percentages[i] /= sum;
            }

            // Ensure minimum value for each percentage
            float minSum = minValue * numActs;
            if (sum < minSum)
            {
                for (int i = 0; i < numActs; i++)
                {
                    percentages[i] = (percentages[i] * (1 - minSum)) + minValue;
                }
            }

            return percentages;

        }

        // Generates the tree-based output
        private async void GenerateTreeButton_Click(object sender, EventArgs e)
        {
            await bookOutline.GenerateContent(session, 4, 0.8);
        }

        // Generates the book outline
        private async void GenerateIdeaButton_Click(object sender, EventArgs e)
        {
            GenerateIdeaButton.Enabled = false;
            GenerateIdeaBtn.Enabled = false;

            ProgressBar.Value = 50;

            ChatResponse response = await session.SendMessageAsync("Create a detailed plot for this book idea, of at least 250 pages: " + IdeaTextBox.Text + ". In the variable LongBookPlot, create an overall arc, with subplots, surprises and twists throughout. Be very detailed. This is the summary of the entire book. Return it in a JSON formatted string with fields, BookTitle, ShortBookSummary, LongBookPlot, SuggestedChapterCount (string), An array of Characters where each character has a FirstName, LastName, Age (string), Gender, ShortBio, FiveWordBio (eg. The best friend of John), Appearance, Role (Protagonist, protagonist's friend, Antagonist, etc), ID (int, a unique ID for each character).", "Book Outline Creation");

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

            if (int.Parse(book.SuggestedChapterCount) > MaxChapters) book.SuggestedChapterCount = MaxChapters.ToString();

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

        // Generates the acts for the book
        private async void GenerateActsButton_Click(object sender, EventArgs e)
        {
            GenerateActsButton.Enabled = false;

            ProgressBar.Value = 50;

            ChatResponse response = await session.SendMessageAsync("Divide the book into " + NumActs + " main acts, and generate detailed act outlines, including specific plot details, for each act for this book idea: " + book.LongBookPlot + ". Return it in a JSON formatted string with array of Acts, each containing ActTitle, ActSummary, ActFullPlot", "Acts Creation");

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

        // Generates the chapters for the book
        private async void GenerateChapterButton_Click(object sender, EventArgs e)
        {
            GenerateChapterButton.Enabled = false;
            ChapterTextBox.Text = ""; // Clear the chapter text box

            float[] percentages = new[] { 0.15f, 0.15f, 0.2f, 0.2f, 0.15f, 0.15f };
            //float[] percentages = GenerateChapterPercentages(6, 0.2f, 0.25f);
            //if (int.Parse(book.SuggestedChapterCount) > 36) book.SuggestedChapterCount = "36";

            int cCount = 1; // Chapter count
            int mult = 100 / NumActs; // Multiplier for progress bar

            if (book.Chapters != null)
                book.Chapters.Clear(); // Clear the chapters if they already exist

            book.Chapters = new List<Chapter>();

            UpdateTreeViewChapters(); // Update the tree view

            for (int i = 0; i < book.Acts.Count; i++)
            {
                ProgressBar.Value = (i + 1) * mult; // Update the progress bar

                int count = (int)MathF.Ceiling(float.Parse(book.SuggestedChapterCount) * percentages[i]); // Number of chapters to generate for this act (based on the percentage of the total chapters).
                string actContext = "This is Act " + (i + 1) + " out of 6 acts. The act's plot is: " + book.Acts[i].ActFullPlot + ". "; // Adding context for the act number 

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

                ChatResponse response;
                string msg = "Create " + count + " chapter outlines, starting at chapter " + cCount + " for act " + (i + 1) + " of a book. " + actContext + lastChapterContext + ". Think about which characters are present, and whether this chapter continues the plot or is a flashback for plot context. Return it in a JSON formatted string with an array named Chapters, each element containing ChapterTitle, ChapterSummary, ChapterFullPlot, CurrentActNum (integer), CharactersPresent (Array) with property FirstName and ID (int, the unique ID of the character from the book outline).";
                Debug.WriteLine(msg);

                try
                {
                    response = await session.SendMessageAsync(msg);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error talking to server for base chapter creation. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProgressBar.Value = 0;
                    break;
                }

                cCount += count; // Increment the chapter count by the number of chapters created

                //var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                Debug.WriteLine(response.Choices[0].Message.Content);

                ChapterList tchapters = new ChapterList();

                try
                {
                    tchapters = JsonConvert.DeserializeObject<ChapterList>(response.Choices[0].Message.Content);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error parsing result from chapter creation. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProgressBar.Value = 0;
                    break;
                }

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

                UpdateTreeViewBook();
                UpdateTreeViewChapters();

            }

            ProgressBar.Value = 0;
            //book.Chapters = chapters.Chapters;
            UpdateTreeViewBook();
            UpdateTreeViewChapters();
            GenerateChapterButton.Enabled = true;
        }


        // This function generates a random number of subchapters for each chapter and then generates the subchapters for each chapter, and then generates the book prose for each subsection.
        private async void GenerateBookButton_Click(object sender, EventArgs e)
        {
            GenerateBookButton.Enabled = false;
            BookTextBox.Text = "";

            previousText = "Nevermind, ignore this. "; // Reset the previous text

            cc = 0; // Chapter count
            osc = 0; // Overall subchapter count
            mult = 100 / (book.Chapters.Count()); // 100 divided by (number of chapters * number of subchapters) which is the multiplier for the progress bar

            ProgressBar.Value = 1; // Set the progress bar to 1 which is almost the same as 0, but it will show that the progress bar is thinking

            // Iterates through the list of characters in the book outline and sets the character's AlreadyIntroduced flag to false
            for (int i = 0; i < book.Characters.Count; i++)
            {
                var character = book.Characters[i];
                character.AlreadyIntroduced = false; // Set the character's AlreadyIntroduced flag to false
                book.Characters[i] = character; // Update the character in the book outline
            }

            // Assuming chapters variable contains the generated chapters from the previous function
            for (int i = startingChapter; i < book.Chapters.Count; i++)
            {

                int res = await GenerateChapter(i); // Generate the chapter prose 
                await Task.Delay(2000); // Wait 1 second before generating the next chapter

                if (res == 0)
                {
                    MessageBox.Show("The chapter generation failed. Either the section creation failed, or the chapter generation failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }

            }

            // Enable the generate book button
            ProgressBar.Value = 0;

            GenerateBookButton.Enabled = true;
        }

        private async Task<int> GenerateChapter(int i)
        {
            Chapter chapter = book.Chapters[i]; // The chapter that is being generated
            string chapterTitle = chapter.ChapterTitle;
            string chapterFullPlot = chapter.ChapterFullPlot;

            BookTextBox.Text += Environment.NewLine + chapterTitle + Environment.NewLine; // Add chapter title to the book text box

            string subchapterContent = ". "; // This will hold the content of the subchapters

            // This is the string that will be sent to the server to break the chapter into subchapters
            string BreakChapterString = "We have a chapter to break into sections. The chapter is \"" + chapterTitle + "\", plot is \"" + chapterFullPlot + "\". Intelligently divide this plot into " + subchapterCount + " sections. Do not expand beyond the scope or events of the chapter plot. Make sure to include details like location and time. Output each section plot as " + subSubChapterCount + " paragraphs, separated by periods, indicating what should happen throughout the section. Output the result as a JSON object with an array of 'Sections', with elements SectionName, SectionPlot, SectionDetailedPlot, An array CharactersPresent with FirstName, LastName, for which characters are in this chapter.";
            Debug.WriteLine(BreakChapterString);    // Write the string to the debug window
            ChatResponse subsectionResponse; // This will hold the response from the server

            if (i == 0)
            {
                previousText = "Nevermind. Ignore this.."; // Reset the previous text
            }
            else 
                previousText = book.Chapters[i - 1].Sections[book.Chapters[i - 1].Sections.Count - 1].GeneratedText;

            ProgressBar.Value = (i * mult) + 1;

            // Send the string to the server and get the response
            try
            {
                subsectionResponse = await session.SendMessageAsync(BreakChapterString, "Subchapter Creation");
            }
            catch (Exception ex)
            {
                // If there is an error, display it and stop the process
                MessageBox.Show("Error talking to server for base subsection creation. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ProgressBar.Value = 0;
                return 0;
            }

            // Get the response from the server
            string subsectionContents = subsectionResponse.Choices[0].Message.Content.Trim();

            // This is the pattern that will be used to find the JSON object in the response
            string jsonPattern = @"^(\{(?:[^{}]|(?<o>\{)|(?<-o>\}))+(?(o)(?!))\})";

            // Find the JSON object in the response
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
                    return 0;
                }

                await Task.Delay(2000); // Wait 2 seconds

                var chapterNode = bookIdea.FindNodeByTitle(chapterTitle); // Find the chapter node in the tree view

                Act curAct = book.Acts[chapter.CurrentActNum - 1]; // Get the current act and subtract 1 because the act numbers start at 1, but the array index starts at 0

                int sc = 0; // Subchapter count is used to keep track of the subchapter number

                for (int j = 0; j < sections.Sections.Count; j++)
                {
                    SubSection section = sections.Sections[j];
                    // ProgressBar.Value = cc * mult + 1; // Update the progress bar using the chapter count and the multiplier, which is 100 divided by the number of chapters

                    string subchapterPrompt = "To follow is a description of a narrative. I'd like to you expand upon it in more detail. Write an expansion like a novel, with description, narrative and dialog. For some context, this section is part of chapter " + (cc + 1) + " in a book. The whole chapter is called: " + chapterTitle + ", and the book is called " + book.BookTitle + ", and Character bios: " + GenerateCharacterSummaryForChapter(chapter) + ". Ok, the description to rewrite, expand on, embellish upon and write dialog for (in the past-tense, " + StyleBox.Text + " style of " + AuthorBox.Text + ") The plot description is: \"" + section.SectionDetailedPlot + "\". Write ONLY what is specified in the plot outline. Don't take elements from the character's bio, and use them to prematurely reveal plot points outside the plot description. Do not expand beyond the end of the plot outline. Write in the past tense ALWAYS please. Don't mention that this is a book, or a chapter, or an act. Also, for your reference ONLY, the last section was: \"" + previousText + "\". Ok, write now."; // Write the section plot

                    Debug.WriteLine(subchapterPrompt);

                    ChatResponse response;

                    try
                    {
                        response = await session.SendMessageAsync(subchapterPrompt, "Chapter Text Creation");
                        await Task.Delay(2000); // Wait 2 seconds
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error talking to server for narrative subsection creation. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ProgressBar.Value = 0;
                        return 0;
                    }

                    string pattern = @"\s*[^.!?]*[.!?](?=\s*$)";
                    string responseMinusLastSentence = Regex.Replace(response.Choices[0].Message.Content, pattern, ""); // Remove the last sentence from the response
                    responseMinusLastSentence = Regex.Replace(responseMinusLastSentence, pattern, "");  // again, remove the last sentence from the response. These two lines effectively remove the last two sentences from the response, which is the prompt that was sent to the server
                                                                                                        // The reason I remove the last two sentences is because ChatGPT tends to summarize the chapter in the last two sentences, and I don't want that in the book

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

                    section.GeneratedText = responseMinusLastSentence;

                    sections.Sections[j] = section;

                    BookTextBox.Text += Environment.NewLine + Environment.NewLine + responseMinusLastSentence;

                    BookTextBox.SelectionStart = BookTextBox.Text.Length;
                    BookTextBox.ScrollToCaret();

                    osc++; // Increment the overall section counter
                    sc++;
                }

                chapter.Sections = sections.Sections; // Add the sections to the chapter
                book.Chapters[i] = chapter;

                // Combine the subchapter content to form the full chapter
                string fullChapter = chapterTitle + "\n" + subchapterContent + "\n\n";
                cc++;
                // Append the chapter to the final book content
                //BookTextBox.Text += fullChapter;
            }
            else
            {
                // Couldn't find the JSON object in the response
                MessageBox.Show("Couldn't parse JSON from ChatGPT in generation of book subsections. " + session._model + " may have sent malformed JSON.", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine("Couldn't parse JSON from ChatGPT in generation of book subsections. " + session._model + " may have sent malformed JSON.");
                return 0;
            }

            return 1;
        }

        private void IdeaTextBox_TextChanged(object sender, EventArgs e)
        {
            GenerateIdeaButton.Enabled = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void ChapterUpDown_ValueChanged(object sender, EventArgs e)
        {
            startingChapter = (int)ChapterUpDown.Value - 1;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string TextToSend = ChatTextBox.Text;
            ChatTextBox.Text = "";
            ChatOutputBox.Text += "You: " + TextToSend + Environment.NewLine;
            ChatTextBox.Focus();
            ChatResponse response = await session.SendMessageAsync(TextToSend, "Chat");
            ChatOutputBox.Text += "GPT: " + response.Choices[0].Message.Content + Environment.NewLine;
        }
    }
}
