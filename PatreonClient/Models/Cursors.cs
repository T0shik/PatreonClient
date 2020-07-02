using System.Text.Json.Serialization;

namespace PatreonClient.Models
{
    public class Cursors
    {
        [JsonPropertyName("next")]
        public string Next { get; set; }
    }
}