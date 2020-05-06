using System;
using System.Text.Json.Serialization;

namespace PatreonClient.Models.Attributes
{
    public class UserAttributes
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }
    }
}