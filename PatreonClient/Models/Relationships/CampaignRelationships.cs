using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models.Relationships
{
    public class CampaignRelationships : IRelationship
    {
        [JsonPropertyName("creator")] public PatreonResponse<User, UserRelationships> Creator { get; set; }
        [JsonPropertyName("tiers")] public PatreonCollectionResponse<Tier, TierRelationships> Tiers { get; set; }

        public bool AssignRelationship(string id, string type, string json)
        {
            if (type.Equals("creator"))
            {
                Creator.Data = JsonSerializer.Deserialize<PatreonData<User, UserRelationships>>(json);
                return true;
            }

            if (type.Equals("tier"))
            {
                var data = JsonSerializer.Deserialize<PatreonData<Tier, EmptyRelationship>>(json);
                var target = Tiers.Data?.FirstOrDefault(x => x.Id.Equals(data.Id));
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