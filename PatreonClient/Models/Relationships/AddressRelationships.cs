using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models.Relationships
{
    public class AddressRelationships : IRelationship
    {
        [JsonPropertyName("user")] public PatreonResponse<User, UserRelationships> User { get; set; }
        [JsonPropertyName("campaigns")] public PatreonCollectionResponse<Campaign, CampaignRelationships> Campaigns { get; set; }
        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
            if (User?.Data != null)
            {
                User.Data = includes.FirstOrDefault(x => x.Id == User.Data.Id) as
                                PatreonData<User, UserRelationships>;

                User.Data?.Relationships?.AssignRelationship(includes);
            }
            if (Campaigns != null)
            {
                foreach (var campaign in Campaigns.Data)
                {
                    var include = includes.FirstOrDefault(x => x.Id == campaign.Id) as PatreonData<Campaign, CampaignRelationships>;
                    campaign.Attributes = include?.Attributes;
                    campaign.Relationships = include?.Relationships;
                    campaign.Relationships?.AssignRelationship(includes);
                }
            }
        }
    }
}