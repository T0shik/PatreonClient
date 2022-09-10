using System;
using System.Text.Json.Serialization;

namespace PatreonClient.Models.Attributes;

public class Campaign
{
    [JsonPropertyName("summary")] public string Summary { get; set; }
    [JsonPropertyName("creation_name")] public string CreationName { get; set; }
    [JsonPropertyName("pay_per_name")] public string PayPerName { get; set; }
    [JsonPropertyName("one_liner")] public string OneLiner { get; set; }
    [JsonPropertyName("main_video_embed")] public string MainVideoEmbed { get; set; }
    [JsonPropertyName("main_video_url")] public string MainVideoUrl { get; set; }
    [JsonPropertyName("image_url")] public string ImageUrl { get; set; }
    [JsonPropertyName("image_small_url")] public string ImageSmallUrl { get; set; }
    [JsonPropertyName("thanks_video_url")] public string ThanksVideoUrl { get; set; }
    [JsonPropertyName("thanks_embed")] public string ThanksEmbed { get; set; }
    [JsonPropertyName("thanks_msg")] public string ThanksMessage { get; set; }
    [JsonPropertyName("is_monthly")] public bool IsMonthly { get; set; }
    [JsonPropertyName("has_rss")] public bool HasRss { get; set; }

    [JsonPropertyName("has_sent_rss_notify")]
    public bool HasSentRssNotify { get; set; }

    [JsonPropertyName("rss_feed_title")] public string RssFeedTitle { get; set; }
    [JsonPropertyName("rss_artwork_url")] public string RssArtworkUrl { get; set; }
    [JsonPropertyName("is_nsfw")] public bool IsNsfw { get; set; }

    [JsonPropertyName("is_charged_immediately")]
    public bool? IsChargedImmediately { get; set; }

    [JsonPropertyName("created_at")] public DateTimeOffset? CreatedAt { get; set; }
    [JsonPropertyName("published_at")] public DateTimeOffset? PublishedAt { get; set; }
    [JsonPropertyName("pledge_url")] public string PledgeUrl { get; set; }
    [JsonPropertyName("patron_count")] public int PatreonCount { get; set; }

    [JsonPropertyName("discord_server_id")]
    public string DiscordServerId { get; set; }

    [JsonPropertyName("google_analytics_id")]
    public string GoogleAnalyticsId { get; set; }

    [JsonPropertyName("show_earnings")] public string ShowEarnings { get; set; }
    [JsonPropertyName("vanity")] public string Vanity { get; set; }
    [JsonPropertyName("url")] public string Url { get; set; }
}