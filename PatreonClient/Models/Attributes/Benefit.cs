using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PatreonClient.Models.Attributes;

public class Benefit
{
    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; }
    [JsonPropertyName("benefit_type")] public string BenefitType { get; set; }
    [JsonPropertyName("rule_type")] public string RuleType { get; set; }
    [JsonPropertyName("created_at")] public DateTimeOffset CreatedAt { get; set; }
    [JsonPropertyName("delivered_deliverables_count")] public int DeliveredDeliverablesCount { get; set; }
    [JsonPropertyName("not_delivered_deliverables_count")] public int NotDeliveredDeliverablesCount { get; set; }
    [JsonPropertyName("deliverables_due_today_count")] public int DeliveredDueTodayCount { get; set; }
    [JsonPropertyName("next_deliverable_due_date")] public DateTimeOffset? NextDeliverableDueDate { get; set; }
    [JsonPropertyName("tiers_count")] public int TiersCount { get; set; }
    [JsonPropertyName("is_deleted")] public bool Deleted { get; set; }
    [JsonPropertyName("is_published")] public bool Published { get; set; }
    [JsonPropertyName("is_ended")] public bool Ended { get; set; }
    [JsonPropertyName("app_external_id")] public string AppExternalId { get; set; }
    [JsonPropertyName("app_meta")] public Dictionary<string, string> AppMeta { get; set; }

}