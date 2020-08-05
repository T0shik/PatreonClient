using System;
using System.Text.Json.Serialization;
using PatreonClient.Models.Relationships;

namespace PatreonClient.Models.Attributes
{
    [ItemRelationship("address", typeof(AddressRelationships))]
    public class Address
    {
        [JsonPropertyName("addressee")] public string Recipient { get; set; }
        [JsonPropertyName("line_1")] public string Line1 { get; set; }
        [JsonPropertyName("line_2")] public string Line2 { get; set; }
        [JsonPropertyName("postal_code")] public string PostCode { get; set; }
        [JsonPropertyName("city")] public string City { get; set; }
        [JsonPropertyName("state")] public string State { get; set; }
        [JsonPropertyName("country")] public string Country { get; set; }
        [JsonPropertyName("phone_number")] public string Telephone { get; set; }
        [JsonPropertyName("created_at")] public DateTimeOffset CreatedAt { get; set; }
    }
}