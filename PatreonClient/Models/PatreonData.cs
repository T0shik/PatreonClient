using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;
using PatreonClient.Models.Relationships;

namespace PatreonClient.Models
{
    public class PatreonData<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("attributes")] public TAttributes Attributes { get; set; }
        [JsonPropertyName("relationships")] public TRelationships Relationships { get; set; }
    }
}