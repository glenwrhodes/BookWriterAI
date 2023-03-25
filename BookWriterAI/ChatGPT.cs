using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace BookWriterAI
{

    public struct ChatMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }


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

    public struct ChatChoice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("message")]
        public ChatMessage Message { get; set; }

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }
    }

    public struct ChatUsage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }

        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }

        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
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

    }

    public class ContentNode
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string FullPlot { get; set; }
        public string Context { get; set; }
        public int Depth { get; set; }

        public List<ContentNode> Children { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public ContentNode Parent { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public ContentNode PreviousSibling { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public ContentNode NextSibling { get; set; }


        public string GetFullContext()
        {
            if (Parent == null)
            {
                return Context;
            }
            else
            {
                return Parent.GetFullContext() + " " + Context;
            }
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
            // If at maximum depth, generate narrative/prose
            if (Depth == maxDepth)
            {
                // ChatGPT prompt for generating the narrative/prose content
                string prompt = "Write book content for " + Title + ". The context is: " + GetFullContext() + ".";
                ChatResponse response = await session.SendMessageAsync(prompt);
                FullPlot = response.Choices[0].Message.Content;
            }
            else
            {
                // ChatGPT prompt for generating the current node's content
                string prompt = "Generate content for " + Title + ". The context is: " + GetFullContext() + ". Create sub-elements for this section.";
                ChatResponse response = await session.SendMessageAsync(prompt);

                // Extract the generated content and create child nodes
                // You can use any method to parse the response and create child nodes
                //List<ContentNode> childNodes = ParseResponse(response);
                List<ContentNode> childNodes = new List<ContentNode>();

                // Set parent, depth, and sibling information for each child node
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
                }

                // Add child nodes to the current node's Children list
                Children.AddRange(childNodes);

                // Recursively generate content for child nodes
                foreach (ContentNode childNode in Children)
                {
                    await childNode.GenerateContent(session, maxDepth, temperature);
                }
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
        public List<ChatMessage> messages;
        public int tokenCount = 0;
        public const int maxTokenCount = 7192;
        public const int responseTokenLimit = 1000;

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

        public async Task<ChatResponse> SendMessageAsync(string content)
        {
            // Add the user message to the list of messages
            AddMessage("user", content);

            // Create an instance of HttpClient to make the API request
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _apiKey);
            client.Timeout = TimeSpan.FromSeconds(300);

            // Define the API request payload
            ChatGPTRequest requestBody = new ChatGPTRequest
            {
                Model = _model,
                Temperature = 0.9f,
                MaxTokens = 1024,
                Messages = messages.ToArray()
            };

            // Set JsonSerializerOptions to use camelCase
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            // Serialize the request object to JSON
            string jsonRequest = System.Text.Json.JsonSerializer.Serialize(requestBody, jsonOptions);

            // Prepare the request content
            HttpContent contents = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Make the API request and receive the response
            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", contents);

            // Read the response content as a string
            string responseContent = await response.Content.ReadAsStringAsync();

            Debug.WriteLine(responseContent);

            // Deserialize the response JSON to a ChatResponse object
            ChatResponse chatResponse = System.Text.Json.JsonSerializer.Deserialize<ChatResponse>(responseContent, jsonOptions);

            // Add the assistant message to the list of messages
            AddMessage("assistant", chatResponse.Choices[0].Message.Content);

            return chatResponse;
        }
    }
}
