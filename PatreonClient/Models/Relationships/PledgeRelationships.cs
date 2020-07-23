using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;
using PatreonClient.Responses;

namespace PatreonClient.Models.Relationships
{
    public class PledgeRelationships : IRelationship
    {
        [JsonPropertyName("campaign")] public PatreonResponse<Campaign, CampaignRelationships> Campaign { get; set; }
        [JsonPropertyName("patreon")] public PatreonResponse<User, UserRelationships> Patreon { get; set; }

        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
            this.AssignDataAndRelationship(includes, Campaign)
	            .AssignDataAndRelationship(includes, Patreon);
        }
    }
}