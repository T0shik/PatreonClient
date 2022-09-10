using System.Text.Json.Serialization;

namespace PatreonClient.Models;

public class Meta
{
    [JsonPropertyName("pagination")]
    public Pagination Pagination { get; set; }
}