using System.Text.Json.Serialization;

namespace PatreonClient.Models.Attributes
{
    public class Post
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}