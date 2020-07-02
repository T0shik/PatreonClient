using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PatreonClient.Models.Attributes;
using PatreonClient.Responses;

namespace PatreonClient.Models.Relationships
{
    public class DeliverableRelationships : IRelationship
    {
        [JsonPropertyName("campaign")] public PatreonResponse<Campaign, CampaignRelationships> Campaign { get; set; }
        [JsonPropertyName("benefit")] public PatreonResponse<Benefit, BenefitRelationships> Benefit { get; set; }
        [JsonPropertyName("member")] public PatreonResponse<Member, MemberRelationships> Member { get; set; }
        [JsonPropertyName("user")] public PatreonResponse<User, UserRelationships> User { get; set; }

        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
            if (Campaign?.Data != null)
            {
                Campaign.Data = includes.FirstOrDefault(x => x.Id == Campaign.Data.Id) as
                                    PatreonData<Campaign, CampaignRelationships>;

                Campaign.Data?.Relationships?.AssignRelationship(includes);
            }
            if (Benefit?.Data != null)
            {
                Benefit.Data = includes.FirstOrDefault(x => x.Id == Benefit.Data.Id) as
                                   PatreonData<Benefit, BenefitRelationships>;

                Benefit.Data?.Relationships?.AssignRelationship(includes);
            }
            if (Member?.Data != null)
            {
                Member.Data = includes.FirstOrDefault(x => x.Id == Member.Data.Id) as
                                  PatreonData<Member, MemberRelationships>;

                Member.Data?.Relationships?.AssignRelationship(includes);
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