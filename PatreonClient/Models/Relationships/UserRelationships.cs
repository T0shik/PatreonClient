using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;
using PatreonClient.Responses;

namespace PatreonClient.Models.Relationships
{
    public class UserRelationships : BaseRelationship
    {
        [JsonPropertyName("campaign")] public PatreonResponse<Campaign, CampaignRelationships> Campaign { get; set; }

        [JsonPropertyName("memberships")]
        public PatreonCollectionResponse<Member, MemberRelationships> Memberships { get; set; }

        public override void AssignRelationship(IReadOnlyCollection<PatreonData> includes) =>
            AssignDataAndRelationship(includes, Campaign)
                .AssignCollectionAttributesAndRelationships(includes, Memberships);
    }
}
