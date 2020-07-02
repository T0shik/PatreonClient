using System.Text.Json.Serialization;

namespace PatreonClient.Models
{
    public class Links
    {
        [JsonPropertyName("self")] public string Self { get; set; }
        [JsonPropertyName("next")] public string Next { get; set; }
    }
}