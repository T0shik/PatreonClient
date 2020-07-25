using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;
using PatreonClient.Responses;

namespace PatreonClient.Models.Relationships
{
    public class PostRelationships : BaseRelationShip, IRelationship
    {
        [JsonPropertyName("campaign")] public PatreonResponse<Campaign, CampaignRelationships> Campaign { get; set; }
        [JsonPropertyName("user")] public PatreonResponse<User, UserRelationships> User { get; set; }

        public new void AssignRelationship(IReadOnlyCollection<PatreonData> includes) =>
            AssignDataAndRelationship(includes, Campaign)
                .AssignDataAndRelationship(includes, User);
    }
}