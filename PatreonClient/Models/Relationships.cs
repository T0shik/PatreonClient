using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models
{
    public class Relationships
    {
        [JsonPropertyName("campaign")]
        public PatreonResponse<Campaign> Campaign { get; set; }
        [JsonPropertyName("user")]
        public PatreonResponse<User> User { get; set; }
        [JsonPropertyName("creator")]
        public PatreonResponse<User> Creator
        {
            get => User;
            set => User = value;
        }
        public PatreonCollectionResponse<Member> Memberships { get; set; }
    }
}