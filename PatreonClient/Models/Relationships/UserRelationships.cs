using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models.Relationships
{
    public class UserRelationships : IRelationship
    {
        [JsonPropertyName("campaign")] public PatreonResponse<Campaign, CampaignRelationships> Campaign { get; set; }

        [JsonPropertyName("memberships")]
        public PatreonCollectionResponse<Member, MemberRelationships> Memberships { get; set; }

        public bool AssignRelationship(string id, string type, string json)
        {
            if (type.Equals("campaign"))
            {
                Campaign.Data = JsonSerializer.Deserialize<PatreonData<Campaign, CampaignRelationships>>(json);
                return true;
            }

            if (type.Equals("memberships"))
            {
                Memberships.Data =
                    JsonSerializer.Deserialize<IEnumerable<PatreonData<Member, MemberRelationships>>>(json);
                return true;
            }

            return false;
        }
    }
}