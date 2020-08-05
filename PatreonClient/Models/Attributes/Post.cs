using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using PatreonClient.Models.Relationships;

namespace PatreonClient.Models.Attributes
{
    [ItemRelationship("post", typeof(PostRelationships))]
    public class Post
    {
        [JsonPropertyName("title")] public string Title { get; set; }
        [JsonPropertyName("content")] public string Content { get; set; }
        [JsonPropertyName("is_paid")] public bool Paid { get; set; }
        [JsonPropertyName("is_public")] public bool Public { get; set; }
        [JsonPropertyName("published_at")] public DateTimeOffset? PublishedAt { get; set; }
        [JsonPropertyName("url")] public string Url { get; set; }
        [JsonPropertyName("embed_data")] public Dictionary<string, string> EmbedData { get; set; }
        [JsonPropertyName("embed_url")] public string EmbedUrl { get; set; }
        [JsonPropertyName("app_id")] public int? AppId { get; set; }
        [JsonPropertyName("app_status")] public string AppStatus { get; set; }
    }
}