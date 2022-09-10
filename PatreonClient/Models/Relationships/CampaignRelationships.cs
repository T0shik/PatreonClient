using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models.Relationships
{
    public class CampaignRelationships : BaseRelationship
    {
        [JsonPropertyName("creator")] public PatreonResponse<User, UserRelationships> Creator { get; set; }
        [JsonPropertyName("tiers")] public PatreonCollectionResponse<Tier, TierRelationships> Tiers { get; set; }

        [JsonPropertyName("benefits")]
        public PatreonCollectionResponse<Benefit, BenefitRelationships> Benefits { get; set; }

        [JsonPropertyName("goals")] public PatreonCollectionResponse<Goal, GoalRelationships> Goals { get; set; }

        public override void AssignRelationship(IReadOnlyCollection<PatreonData> includes) =>
            AssignDataAndRelationship(includes, Creator)
                .AssignCollectionAttributesAndRelationships(includes, Tiers)
                .AssignCollectionAttributesAndRelationships(includes, Benefits)
                .AssignCollectionAttributesAndRelationships(includes, Goals);
    }
}
