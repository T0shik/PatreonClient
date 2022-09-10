using System;
using System.Text.Json.Serialization;

namespace PatreonClient.Models.Attributes;

public class Member
{
    [JsonPropertyName("patron_status")] public string PatreonStatus { get; set; }
    [JsonPropertyName("is_follower")] public bool Follower { get; set; }
    [JsonPropertyName("full_name")] public string FullName { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; }

    [JsonPropertyName("pledge_relationship_start")]
    public DateTimeOffset? PledgeRelationshipStart { get; set; }

    [JsonPropertyName("lifetime_support_cents")]
    public int LifetimeSupportCents { get; set; }

    [JsonPropertyName("currently_entitled_amount_cents")]
    public int CurrentlyEntitledAmountCents { get; set; }

    [JsonPropertyName("last_charge_date")] public DateTimeOffset? LastChargeDate { get; set; }

    [JsonPropertyName("last_charge_status")]
    public string LastChargeStatus { get; set; }

    [JsonPropertyName("note")] public string Note { get; set; }

    [JsonPropertyName("will_pay_amount_cents")]
    public int WillPayAmountCentsName { get; set; }
}