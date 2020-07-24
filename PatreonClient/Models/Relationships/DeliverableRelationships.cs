using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;
using PatreonClient.Responses;

namespace PatreonClient.Models.Relationships
{
    public class DeliverableRelationships : IRelationship
    {
        [JsonPropertyName("campaign")] public PatreonResponse<Campaign, CampaignRelationships> Campaign { get; set; }
        [JsonPropertyName("benefit")] public PatreonResponse<Benefit, BenefitRelationships> Benefit { get; set; }
        [JsonPropertyName("member")] public PatreonResponse<Member, MemberRelationships> Member { get; set; }
        [JsonPropertyName("user")] public PatreonResponse<User, UserRelationships> User { get; set; }

        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
            this.AssignDataAndRelationship(includes, Campaign)
	            .AssignDataAndRelationship(includes, Benefit)
	            .AssignDataAndRelationship(includes, Member)
	            .AssignDataAndRelationship(includes, User);
        }
    }
}
