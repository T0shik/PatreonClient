using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models.Relationships
{
    public class AddressRelationships : BaseRelationship
    {
        [JsonPropertyName("user")] public PatreonResponse<User, UserRelationships> User { get; set; }

        [JsonPropertyName("campaigns")]
        public PatreonCollectionResponse<Campaign, CampaignRelationships> Campaigns { get; set; }

        public override void AssignRelationship(IReadOnlyCollection<PatreonData> includes) =>
            AssignDataAndRelationship(includes, User)
                .AssignCollectionAttributesAndRelationships(includes, Campaigns);
    }
}
