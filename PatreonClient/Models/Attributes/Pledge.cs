using System;
using System.Text.Json.Serialization;
using PatreonClient.Models.Relationships;

namespace PatreonClient.Models.Attributes
{
    [ItemRelationship("pledge", typeof(PledgeRelationships))]
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