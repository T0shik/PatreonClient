using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models.Relationships
{
    public class DeliverableRelationships : BaseRelationship
    {
        [JsonPropertyName("campaign")] public PatreonResponse<Campaign, CampaignRelationships> Campaign { get; set; }
        [JsonPropertyName("benefit")] public PatreonResponse<Benefit, BenefitRelationships> Benefit { get; set; }
        [JsonPropertyName("member")] public PatreonResponse<Member, MemberRelationships> Member { get; set; }
        [JsonPropertyName("user")] public PatreonResponse<User, UserRelationships> User { get; set; }

        public override void AssignRelationship(IReadOnlyCollection<PatreonData> includes) =>
            AssignDataAndRelationship(includes, Campaign)
                .AssignDataAndRelationship(includes, Benefit)
                .AssignDataAndRelationship(includes, Member).AssignDataAndRelationship(includes, User);
    }
}
