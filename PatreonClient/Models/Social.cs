using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PatreonClient.Models;

public class Social
{
    [JsonPropertyName("scopes")]
    public IEnumerable<string> Scopes { get; set; }
    [JsonPropertyName("url")]
    public string Url { get; set; }
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }
}