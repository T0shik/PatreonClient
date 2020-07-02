using System.Text.Json.Serialization;

namespace PatreonClient.Models
{
    public class Pagination
    {
        [JsonPropertyName("total")] public int Total { get; set; }
        [JsonPropertyName("cursors")] public Cursors Cursors { get; set; }
    }
}