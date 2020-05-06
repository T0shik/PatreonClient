using System;
using System.Text.Json.Serialization;

namespace PatreonClient.Models.Attributes
{
    public class MemberAttributes
    {
        [JsonPropertyName("email")] public string Email { get; set; }
        [JsonPropertyName("full_name")] public string FullName { get; set; }
    }
}