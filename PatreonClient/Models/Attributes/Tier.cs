using System;
using System.Text.Json.Serialization;
using PatreonClient.Models.Relationships;

namespace PatreonClient.Models.Attributes
{
    [ItemRelationship("tier", typeof(TierRelationships))]
    public class Tier
    {
        [JsonPropertyName("amount_cents")] public int AmountCents { get; set; }
        [JsonPropertyName("user_limit")] public int? UserLimit { get; set; }
        [JsonPropertyName("remaining")] public int? Remaining { get; set; }
        [JsonPropertyName("description")] public string Description { get; set; }
        [JsonPropertyName("requires_shipping")] public bool RequiresShipping { get; set; }
        [JsonPropertyName("created_at")] public DateTimeOffset CreatedAt  { get; set; }
        [JsonPropertyName("url")] public string Url { get; set; }
        [JsonPropertyName("patron_count")] public int PatreonCount { get; set; }
        [JsonPropertyName("post_count")] public int? PostCount { get; set; }
        [JsonPropertyName("discord_role_ids")] public object DiscordRoleIds { get; set; }
        [JsonPropertyName("title")] public string Title { get; set; }
        [JsonPropertyName("image_url")] public string ImageUrl { get; set; }
        [JsonPropertyName("edited_at")] public DateTimeOffset EditedAt  { get; set; }
        [JsonPropertyName("published")] public bool Published { get; set; }
        [JsonPropertyName("published_at")] public DateTimeOffset? PublishedAt { get; set; }
        [JsonPropertyName("unpublished_at")] public DateTimeOffset? UnpublishedAt { get; set; }
    }
}