using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;
using PatreonClient.Responses;

namespace PatreonClient.Models.Relationships
{
    public class MemberRelationships : BaseRelationShip, IRelationship
    {
        [JsonPropertyName("address")] public PatreonResponse<Address, AddressRelationships> Address { get; set; }
        [JsonPropertyName("campaign")] public PatreonResponse<Campaign, CampaignRelationships> Campaign { get; set; }
        [JsonPropertyName("tiers")] public PatreonCollectionResponse<Tier, TierRelationships> Tiers { get; set; }
        [JsonPropertyName("user")] public PatreonResponse<User, UserRelationships> User { get; set; }

        [JsonPropertyName("pledge_history")]
        public PatreonCollectionResponse<Pledge, PledgeRelationships> PledgeHistory { get; set; }

        public new void AssignRelationship(IReadOnlyCollection<PatreonData> includes) =>
            AssignDataAndRelationship(includes, Address)
                .AssignDataAndRelationship(includes, Campaign)
                .AssignCollectionAttributesAndRelationships(includes, Tiers)
                .AssignDataAndRelationship(includes, User)
                .AssignCollectionAttributesAndRelationships(includes, PledgeHistory);
    }
}