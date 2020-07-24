using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;
using PatreonClient.Responses;

namespace PatreonClient.Models.Relationships
{
    public class CampaignRelationships : IRelationship
    {
        [JsonPropertyName("creator")] public PatreonResponse<User, UserRelationships> Creator { get; set; }
        [JsonPropertyName("tiers")] public PatreonCollectionResponse<Tier, TierRelationships> Tiers { get; set; }
        [JsonPropertyName("benefits")] public PatreonCollectionResponse<Benefit, BenefitRelationships> Benefits { get; set; }
        [JsonPropertyName("goals")] public PatreonCollectionResponse<Goal, GoalRelationships> Goals { get; set; }

        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
				this.AssignDataAndRelationship(includes, Creator)
					.AssignCollectionAttributesAndRelationships(includes, Tiers)
					.AssignCollectionAttributesAndRelationships(includes, Benefits)
					.AssignCollectionAttributesAndRelationships(includes, Goals);
        }
    }
}
