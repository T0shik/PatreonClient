using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models.Relationships
{
    public class TierRelationships : IRelationship
    {
        [JsonPropertyName("tier_image")] public PatreonResponse<Media, EmptyRelationship> TierImage { get; set; }

        public bool AssignRelationship(string id, string type, string json)
        {
            throw new System.NotImplementedException();
        }
    }
}