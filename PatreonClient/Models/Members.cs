using PatreonClient.Models.Attributes;

namespace PatreonClient.Models
{
    public class Members : PatreonCollectionResponse<MemberAttributes>
    {
        public PatreonData<UserAttributes> User =>
            Includes.TryGetValue("user", out var value) ? (PatreonData<UserAttributes>) value : null;

        public PatreonData<CampaignAttributes> Campaign =>
            Includes.TryGetValue("campaign", out var value) ? (PatreonData<CampaignAttributes>) value : null;
    }
}