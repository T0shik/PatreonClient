using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PatreonClient.Models
{
    public class PatreonCollectionResponse<T>
    {
        [JsonPropertyName("data")] public IEnumerable<PatreonData<T>> Data { get; set; }
        [JsonPropertyName("meta")] public Meta Meta { get; set; }
        [JsonPropertyName("type")] public string Type { get; set; }
    }
}