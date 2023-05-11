using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookWriterAI
{

    // The struct for the GPT-3 response Message
    public struct ChatMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    // The struct for the GPT-3 request
    public struct ChatGPTRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("temperature")]
        public float Temperature { get; set; }

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonPropertyName("messages")]
        public ChatMessage[] Messages { get; set; }
    }

    // The struct for the GPT-3 response
    public struct ChatResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("created")]
        public long Created { get; set; }

        [JsonPropertyName("choices")]
        public ChatChoice[] Choices { get; set; }

        [JsonPropertyName("usage")]
        public ChatUsage Usage { get; set; }
    }

    // The struct for the GPT-3 response Choices
    public struct ChatChoice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("message")]
        public ChatMessage Message { get; set; }

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }
    }

    // The struct for the GPT-3 response Usage
    public struct ChatUsage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }

        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }

        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }

    // TODO: Finish this and integrate into main design
    public struct BookSettings
    {
        public int MaxChapters { get; set; }

        public int SubSectionsPerChapter { get; set; }

        public int SubSubSectionSentences { get; set; }

    }

    public struct Book
    {
        [JsonPropertyName("BookTitle")]
        public string BookTitle { get; set; }

        [JsonPropertyName("ShortBookSummary")]
        public string ShortBookSummary { get; set; }

        [JsonPropertyName("LongBookPlot")]
        public string LongBookPlot { get; set; }

        [JsonPropertyName("SuggestedChapterCount")]
        public string SuggestedChapterCount { get; set; }

        [JsonPropertyName("Characters")]
        public List<CharacterDetail> Characters { get; set; }

        [JsonPropertyName("Acts")]
        public List<Act> Acts { get; set; }

        [JsonPropertyName("Chapters")]
        public List<Chapter> Chapters { get; set; }

    }

    public struct Chapter
    {
        [JsonPropertyName("ChapterTitle")]
        public string ChapterTitle { get; set; }

        [JsonPropertyName("ChapterSummary")]
        public string ChapterSummary { get; set; }

        [JsonPropertyName("ChapterFullPlot")]
        public string ChapterFullPlot { get; set; }

        [JsonPropertyName("CurrentActNum")]
        public int CurrentActNum { get; set; }

        [JsonPropertyName("CharactersPresent")]
        public List<CharacterDetail> CharactersPresent { get; set; }

        [JsonPropertyName("Sections")]
        public List<SubSection> Sections { get; set; } // TODO: Add this to the JSON
    }

    public struct ChapterList
    {
        [JsonPropertyName("Chapters")]
        public List<Chapter> Chapters { get; set; }
    }

    public struct Act
    {
        [JsonPropertyName("ActTitle")]
        public string ActTitle { get; set; }

        [JsonPropertyName("ActSummary")]
        public string ActSummary { get; set; }

        [JsonPropertyName("ActFullPlot")]
        public string ActFullPlot { get; set; }

    }

    public struct SubSection
    {
        [JsonPropertyName("SectionName")]
        public string SectionName { get; set; }

        [JsonPropertyName("SectionPlot")]
        public string SectionPlot { get; set; }

        [JsonPropertyName("SectionDetailedPlot")]
        public string SectionDetailedPlot { get; set; }

        [JsonPropertyName("CharactersPresent")]
        public List<CharacterDetail> CharactersPresent { get; set; }

        [JsonPropertyName("GeneratedText")]
        public string GeneratedText { get; set; }

    }

    public struct SubSections
    {
        [JsonPropertyName("Sections")]
        public List<SubSection> Sections { get; set; }
    }

    public struct ActList
    {
        [JsonPropertyName("Acts")]
        public List<Act> Acts { get; set; }

        [JsonPropertyName("Chapters")]
        public List<Chapter> Chapters { get; set; }

    }

    public struct CharacterDetail
    {
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("LastName")]
        public string LastName { get; set; }

        [JsonPropertyName("Age")]
        public string Age { get; set; }

        [JsonPropertyName("Gender")]
        public string Gender { get; set; }

        [JsonPropertyName("ShortBio")]
        public string ShortBio { get; set; }

        [JsonPropertyName("Appearance")]
        public string Appearance { get; set; }

        [JsonPropertyName("FiveWordBio")]
        public string FiveWordBio { get; set; }

        [JsonPropertyName("Role")]
        public string Role { get; set; }

        [JsonPropertyName("AlreadyIntroduced")]
        public bool AlreadyIntroduced { get; set; }

        [JsonPropertyName("ID")]
        public int ID { get; set; }

    }

    public struct ContentNodes
    {
        public List<ContentNode> Contents { get; set; }
    }

    public class ContentNode
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string FullPlot { get; set; }
        public string Context { get; set; }
        public int Depth { get; set; }
        public int Branches = 3;

        public List<ContentNode> Children { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public ContentNode Parent { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public ContentNode PreviousSibling { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public ContentNode NextSibling { get; set; }

        public delegate void ContentGeneratedHandler(string content);
        public event ContentGeneratedHandler ContentGenerated;

        public string GetFullContext()
        {
            string fullContext;

            if (Parent == null)
            {
                fullContext = Context;
            }
            else
            {
                fullContext = Parent.GetFullContext() + " " + FullPlot;
            }

            // Remove newlines and trim whitespace
            fullContext = fullContext.Replace("\n", " ").Replace("\r", " ").Trim();

            return fullContext;
        }

        public ContentNode()
        {
            Children = new List<ContentNode>();
            Title = "";
            Summary = "";
            FullPlot = "";
            Context = "";
            Depth = 0;
            //Parent = parent;
            //PreviousSibling = previousSibling;
            //NextSibling = null;
            if (PreviousSibling != null)
            {
                PreviousSibling.NextSibling = this;
            }
        }

        public async Task GenerateContent(ChatSession session, int maxDepth, double temperature)
        {
            // First, generate the content for the current node
            if (Depth != maxDepth)
            {
                string prompt = "Generate content for \"" + Title + "\". The context is: \"" + GetFullContext() + "\". Create " + Branches + " detailed subsections for this section. Return the results as a JSON object with an array called Contents, with properties Title, Summary, FullPlot";
                Debug.WriteLine(prompt);
                ChatResponse response = await session.SendMessageAsync(prompt);

                ContentNodes tContents = JsonConvert.DeserializeObject<ContentNodes>(response.Choices[0].Message.Content);
                List<ContentNode> childNodes = tContents.Contents;

                for (int i = 0; i < childNodes.Count; i++)
                {
                    ContentNode childNode = childNodes[i];
                    childNode.Parent = this;
                    childNode.Depth = Depth + 1;

                    if (i > 0)
                    {
                        childNode.PreviousSibling = childNodes[i - 1];
                    }

                    if (i < childNodes.Count - 1)
                    {
                        childNode.NextSibling = childNodes[i + 1];
                    }

                    // Subscribe to the ContentGenerated event for each child node
                    childNode.ContentGenerated += ContentGenerated;
                }

                Children.AddRange(childNodes);
            }

            // Now that the parent node's context is generated, generate content for child nodes recursively
            if (Depth < maxDepth)
            {
                foreach (ContentNode childNode in Children)
                {
                    await childNode.GenerateContent(session, maxDepth, temperature);
                }
            }
            else
            {
                // At maximum depth, generate narrative/prose
                string prompt = "Write narrative prose book content for " + Title + ". This must be novel prose in the style of Stephen King, with descriptions and dialog. The context is: " + GetFullContext() + ".";
                Debug.WriteLine(prompt);
                ChatResponse response = await session.SendMessageAsync(prompt);
                FullPlot = response.Choices[0].Message.Content;
                ContentGenerated?.Invoke(FullPlot);
            }
        }

        public void AddChild(ContentNode child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public ContentNode FindNodeByTitle(string title)
        {
            if (Title == title)
            {
                return this;
            }

            foreach (var child in Children)
            {
                ContentNode foundNode = child.FindNodeByTitle(title);
                if (foundNode != null)
                {
                    return foundNode;
                }
            }

            return null;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }


    public class ChatSession
    {
        public string _apiKey;
        public string _model;
        public int _maxTokens = 2048;
        public List<ChatMessage> messages;
        public int tokenCount = 0;
        public const int maxTokenCount = 7192;
        public const int responseTokenLimit = 1000;
        public double _temperature = 0.8;

        public ChatSession(string model, string apiKey)
        {
            _model = model;
            _apiKey = apiKey;
            messages = new List<ChatMessage>();
        }

        private int CountTokens(string text)
        {
            int count = text.Length;

            return count;
        }

        public void AddMessage(string role, string content)
        {
            int messageTokens = CountTokens(content);
            tokenCount += messageTokens;
            messages.Add(new ChatMessage { Role = role, Content = content });

            // Remove messages if token count exceeds the limit
            while (tokenCount > maxTokenCount)
            {
                int removedMessageTokens = CountTokens(messages[0].Content);
                tokenCount -= removedMessageTokens;
                messages.RemoveAt(0);
            }
        }

        public void Reset()
        {
            messages = new List<ChatMessage>();
            tokenCount = 0;
        }

        public async Task<ChatResponse> SendMessageAsync(string content, string context = "")
        {
            try
            {
                // Add the user message to the list of messages
                AddMessage("user", content);

                // Create an instance of HttpClient to make the API request
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _apiKey);
                client.Timeout = TimeSpan.FromSeconds(600);

                // Define the API request payload
                ChatGPTRequest requestBody = new ChatGPTRequest
                {
                    Model = _model,
                    Temperature = (float)_temperature,
                    MaxTokens = _maxTokens,
                    Messages = messages.ToArray()
                };

                // Set JsonSerializerOptions to use camelCase
                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                // Serialize the request object to JSON
                string jsonRequest = System.Text.Json.JsonSerializer.Serialize(requestBody, jsonOptions);

                // Prepare the request content
                HttpContent contents = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response;

                // Make the API request and receive the response
                try
                {
                    response = await client.PostAsync("https://api.openai.com/v1/chat/completions", contents);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(context + ": Error talking to server. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new Exception($"{context}: Error talking to server: {ex.Message}");
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{context}: Error during ChatGPT API request: {response.StatusCode} - {response.ReasonPhrase}");
                }

                // Read the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine(responseContent);

                // Deserialize the response JSON to a ChatResponse object
                ChatResponse chatResponse = System.Text.Json.JsonSerializer.Deserialize<ChatResponse>(responseContent, jsonOptions);

                // Add the assistant message to the list of messages
                AddMessage("assistant", chatResponse.Choices[0].Message.Content);

                return chatResponse;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(context + ": An error occurred while connecting to the server. Please check your internet connection and try again. If the problem persists, the server might be down. Error details: " + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show(context + ": An unexpected error occurred. Please try again. Error details: " + ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }

    public class PageNumberEventHandler : IEventHandler
    {
        public void HandleEvent(Event currentEvent)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            int pageNumber = pdfDoc.GetPageNumber(page);

            PdfCanvas pdfCanvas = new PdfCanvas(page);
            iText.Kernel.Geom.Rectangle pageSize = page.GetPageSize();
            Canvas canvas = new Canvas(pdfCanvas, pageSize);

            // Set the footer text
            Paragraph footerText = new Paragraph($"Page {pageNumber}")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(9);

            // Add the footer text to the bottom margin of the page
            canvas.ShowTextAligned(footerText, pageSize.GetWidth() / 2, 20, TextAlignment.CENTER);

            canvas.Close();
        }
    }

}
