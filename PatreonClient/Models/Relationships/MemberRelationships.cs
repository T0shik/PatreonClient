using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models.Relationships
{
    public class MemberRelationships : IRelationship
    {
        [JsonPropertyName("address")] public PatreonResponse<Address, AddressRelationships> Address { get; set; }
        [JsonPropertyName("campaign")] public PatreonResponse<Campaign, CampaignRelationships> Campaign { get; set; }
        [JsonPropertyName("tiers")] public PatreonCollectionResponse<Tier, TierRelationships> Tiers { get; set; }
        [JsonPropertyName("user")] public PatreonResponse<User, UserRelationships> User { get; set; }

        [JsonPropertyName("pledge_history")]
        public PatreonCollectionResponse<Pledge, PledgeRelationships> PledgeHistory { get; set; }

        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
            if (Address?.Data != null)
            {
                Address.Data = includes.FirstOrDefault(x => x.Id == Address.Data.Id) as
                                   PatreonData<Address, AddressRelationships>;

                Address.Data?.Relationships?.AssignRelationship(includes);
            }

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

            if (Tiers != null)
            {
                foreach (var tier in Tiers.Data)
                {
                    var include = includes.FirstOrDefault(x => x.Id == tier.Id) as PatreonData<Tier, TierRelationships>;
                    tier.Attributes = include?.Attributes;
                    tier.Relationships = include?.Relationships;
                    tier.Relationships?.AssignRelationship(includes);
                }
            }

            if (PledgeHistory != null)
            {
                foreach (var pledge in PledgeHistory.Data)
                {
                    var include = includes.FirstOrDefault(x => x.Id == pledge.Id)
                                      as PatreonData<Pledge, PledgeRelationships>;
                    pledge.Attributes = include?.Attributes;
                    pledge.Relationships = include?.Relationships;
                    pledge.Relationships?.AssignRelationship(includes);
                }
            }
        }
    }
}