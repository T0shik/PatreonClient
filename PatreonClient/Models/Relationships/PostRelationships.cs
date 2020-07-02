using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;
using PatreonClient.Responses;

namespace PatreonClient.Models.Relationships
{
    public class PostRelationships : IRelationship
    {
        [JsonPropertyName("campaign")] public PatreonResponse<Campaign, CampaignRelationships> Campaign { get; set; }
        [JsonPropertyName("user")] public PatreonResponse<User, UserRelationships> User { get; set; }
        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
            if (Campaign?.Data != null)
            {
                Campaign.Data = includes.FirstOrDefault(x => x.Id == Campaign.Data.Id) as
                                    PatreonData<Campaign, CampaignRelationships>;

                Campaign.Data?.Relationships?.AssignRelationship(includes);
            }

            if (User?.Data != null)
            {
                User.Data = includes.FirstOrDefault(x => x.Id == User.Data.Id) as
                                PatreonData<User, UserRelationships>;

                User.Data?.Relationships?.AssignRelationship(includes);
            }
        }
    }
}