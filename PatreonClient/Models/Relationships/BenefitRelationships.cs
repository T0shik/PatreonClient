using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models.Relationships
{
    public class BenefitRelationships : IRelationship
    {
        [JsonPropertyName("campaign")] public PatreonResponse<Campaign, CampaignRelationships> Campaign { get; set; }
        [JsonPropertyName("tiers")] public PatreonCollectionResponse<Tier, TierRelationships> Tiers { get; set; }
        [JsonPropertyName("deliverables")] public PatreonCollectionResponse<Deliverable, DeliverableRelationships> Deliverables { get; set; }

        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
            if (Campaign?.Data != null)
            {
                Campaign.Data = includes.FirstOrDefault(x => x.Id == Campaign.Data.Id) as
                                    PatreonData<Campaign, CampaignRelationships>;

                Campaign.Data?.Relationships?.AssignRelationship(includes);
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
            if (Deliverables != null)
            {
                foreach (var deliverable in Deliverables.Data)
                {
                    var include = includes.FirstOrDefault(x => x.Id == deliverable.Id) as PatreonData<Deliverable, DeliverableRelationships>;
                    deliverable.Attributes = include?.Attributes;
                    deliverable.Relationships = include?.Relationships;
                    deliverable.Relationships?.AssignRelationship(includes);
                }
            }
        }
    }
}