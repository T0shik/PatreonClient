using System;
using System.Text.Json.Serialization;

namespace PatreonClient.Models.Attributes
{
    [JsonAlias("pledge-event")]
    public class Pledge
    {
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("date")] public DateTimeOffset Date { get; set; }
        [JsonPropertyName("payment_status")] public string Status { get; set; }
        [JsonPropertyName("tier_title")] public string TierTitle { get; set; }
        [JsonPropertyName("tier_id")] public string TierId { get; set; }
        [JsonPropertyName("amount_cents")] public int AmountCents { get; set; }
        [JsonPropertyName("currency_code")] public string CurrencyCode { get; set; }
    }
}