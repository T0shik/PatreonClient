using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;
using PatreonClient.Responses;

namespace PatreonClient.Models.Relationships
{
    public class TierRelationships : IRelationship
    {
        [JsonPropertyName("campaign")] public PatreonResponse<Campaign, CampaignRelationships> Campaign { get; set; }
        [JsonPropertyName("tier_image")] public PatreonResponse<Media> TierImage { get; set; }
        [JsonPropertyName("benefits")] public PatreonCollectionResponse<Benefit, BenefitRelationships> Benefits { get; set; }

        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
            if (Campaign?.Data != null)
            {
                Campaign.Data = includes.FirstOrDefault(x => x.Id == Campaign.Data.Id) as
                                    PatreonData<Campaign, CampaignRelationships>;

                Campaign.Data?.Relationships?.AssignRelationship(includes);
            }
            if (TierImage?.Data != null)
            {
                TierImage.Data = includes.FirstOrDefault(x => x.Id == TierImage.Data.Id) as PatreonData<Media>;
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
        }
    }
}