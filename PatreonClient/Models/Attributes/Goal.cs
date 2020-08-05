using System;
using System.Text.Json.Serialization;
using PatreonClient.Models.Relationships;

namespace PatreonClient.Models.Attributes
{
    [ItemRelationship("goal", typeof(GoalRelationships))]
    public class Goal
    {
        [JsonPropertyName("amount_cents")] public int AmountCents { get; set; }
        [JsonPropertyName("title")] public string Title { get; set; }
        [JsonPropertyName("description")] public string Description { get; set; }
        [JsonPropertyName("created_at")] public DateTimeOffset CreatedAt { get; set; }
        [JsonPropertyName("reached_at")] public DateTimeOffset? ReachedAt { get; set; }
        [JsonPropertyName("completed_percentage")] public int CompletedPercentage { get; set; }
    }
}