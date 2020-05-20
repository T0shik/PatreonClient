using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models.Relationships
{
    public class CampaignRelationships: IRelationship
    {
        [JsonPropertyName("creator")] public PatreonResponse<User, UserRelationships> Creator { get; set; }
        public bool AssignRelationship(string id, string type, string json)
        {
            throw new System.NotImplementedException();
        }
    }
}