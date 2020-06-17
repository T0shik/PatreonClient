using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models.Relationships
{
    public class PledgeRelationships : IRelationship
    {
        [JsonPropertyName("campaign")] public PatreonResponse<Campaign, CampaignRelationships> Campaign { get; set; }
        [JsonPropertyName("patreon")] public PatreonResponse<User, UserRelationships> Patreon { get; set; }
        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
            if (Campaign?.Data != null)
            {
                Campaign.Data = includes.FirstOrDefault(x => x.Id == Campaign.Data.Id) as
                                    PatreonData<Campaign, CampaignRelationships>;

                Campaign.Data?.Relationships?.AssignRelationship(includes);
            }
            if (Patreon?.Data != null)
            {
                Patreon.Data = includes.FirstOrDefault(x => x.Id == Patreon.Data.Id) as
                                   PatreonData<User, UserRelationships>;

                Patreon.Data?.Relationships?.AssignRelationship(includes);
            }
        }
    }
}