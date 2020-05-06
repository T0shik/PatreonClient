using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models
{
    public class PatreonData<T>
    {
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("attributes")] public T Attributes { get; set; }
        [JsonPropertyName("relationships")] public Relationships Relationships { get; set; }
    }
}