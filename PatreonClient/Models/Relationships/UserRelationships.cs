using System.Collections.Generic;
using System.Linq;
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

            if (type.Equals("membership"))
            {
                var data = JsonSerializer.Deserialize<PatreonData<Member, MemberRelationships>>(json);
                var target = Memberships.Data?.FirstOrDefault(x => x.Id.Equals(data.Id));
                if (target != null)
                {
                    target.Attributes = data.Attributes;
                    return true;
                }
            }

            return false;
        }
    }
}