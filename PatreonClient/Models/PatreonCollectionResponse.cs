using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PatreonClient.Models.Relationships;

namespace PatreonClient.Models
{
    public class PatreonCollectionResponse<TAttributes, TRelationships> : IPatreonResponse<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        [JsonPropertyName("data")] public IEnumerable<PatreonData<TAttributes, TRelationships>> Data { get; set; }
        [JsonPropertyName("meta")] public Meta Meta { get; set; }
        [JsonPropertyName("type")] public string Type { get; set; }
    }
}