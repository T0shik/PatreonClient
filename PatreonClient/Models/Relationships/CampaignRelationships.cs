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
            if (Creator?.Data != null)
            {
                Creator.Data = includes.FirstOrDefault(x => x.Id == Creator.Data.Id) as
                                   PatreonData<User, UserRelationships>;

                Creator.Data?.Relationships?.AssignRelationship(includes);
            }
            if (Tiers != null)
            {
                foreach (var tier in Tiers.Data)
                {
                    var include = includes.FirstOrDefault(x => x.Id == tier.Id) as PatreonData<Tier, TierRelationships>;
                    tier.Attributes = include?.Attributes;
                    tier.Relationships = include?.Relationships;
                    tier.Relationships?.AssignRelationship(includes);
                }
            }
            if (Benefits != null)
            {
                foreach (var benefit in Benefits.Data)
                {
                    var include = includes.FirstOrDefault(x => x.Id == benefit.Id) as PatreonData<Benefit, BenefitRelationships>;
                    benefit.Attributes = include?.Attributes;
                    benefit.Relationships = include?.Relationships;
                    benefit.Relationships?.AssignRelationship(includes);
                }
            }
            if (Goals != null)
            {
                foreach (var goal in Goals.Data)
                {
                    var include = includes.FirstOrDefault(x => x.Id == goal.Id) as PatreonData<Goal, GoalRelationships>;
                    goal.Attributes = include?.Attributes;
                    goal.Relationships = include?.Relationships;
                    goal.Relationships?.AssignRelationship(includes);
                }
            }
        }
    }
}