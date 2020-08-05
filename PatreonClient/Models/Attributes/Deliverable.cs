using System;
using System.Text.Json.Serialization;
using PatreonClient.Models.Relationships;

namespace PatreonClient.Models.Attributes
{
    [ItemRelationship("deliverable", typeof(DeliverableRelationships))]
    public class Deliverable
    {
        [JsonPropertyName("completed_at")] public DateTimeOffset? CompletedAt { get; set; }
        [JsonPropertyName("delivery_status")] public string Title { get; set; }
        [JsonPropertyName("due_at")] public DateTimeOffset DueAt { get; set; }
    }
}