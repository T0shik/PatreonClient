using System.Text.Json.Serialization;

namespace PatreonClient.Models;

public abstract class PatreonData
{
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; }
}

public class PatreonData<TAttributes> : PatreonData
{
    [JsonPropertyName("attributes")] public TAttributes Attributes { get; set; }
}

public class PatreonData<TAttributes, TRelationships> : PatreonData<TAttributes>
    where TRelationships : IRelationship
{
    [JsonPropertyName("relationships")] public TRelationships Relationships { get; set; }
}