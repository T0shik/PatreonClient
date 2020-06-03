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

        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
            if (Creator != null)
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
                }
            }
        }
    }
}