using System.Collections.Generic;
using System.ComponentModel;
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

        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
            if (Campaign != null)
            {
                Campaign.Data = includes.FirstOrDefault(x => x.Id == Campaign.Data.Id) as
                                    PatreonData<Campaign, CampaignRelationships>;

                Campaign.Data?.Relationships?.AssignRelationship(includes);
            }
            if (Memberships != null)
            {
                foreach (var membership in Memberships.Data)
                {
                    var include = includes.FirstOrDefault(x => x.Id == membership.Id) as PatreonData<Member, MemberRelationships>;
                    membership.Attributes = include?.Attributes;
                    membership.Relationships = include?.Relationships;
                    membership.Relationships?.AssignRelationship(includes);
                }
            }
        }
    }
}