using System.Collections.Generic;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models.Relationships
{
    public class TierRelationships : IRelationship
    {
        [JsonPropertyName("tier_image")] public PatreonResponse<Media, EmptyRelationship> TierImage { get; set; }

        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
            throw new System.NotImplementedException();
        }
    }
}