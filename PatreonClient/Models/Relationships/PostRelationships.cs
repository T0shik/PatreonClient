using System.Collections.Generic;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models.Relationships
{
    public class PostRelationships : IRelationship
    {
        [JsonPropertyName("campaign")] public PatreonResponse<Campaign, CampaignRelationships> Campaign { get; set; }
        [JsonPropertyName("user")] public PatreonResponse<User, UserRelationships> User { get; set; }
        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
            throw new System.NotImplementedException();
        }
    }
}