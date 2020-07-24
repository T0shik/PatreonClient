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
            this.AssignDataAndRelationship(includes, Campaign)
	            .AssignData(includes, TierImage)
	            .AssignCollectionAttributesAndRelationships(includes, Benefits);
        }
    }
}
