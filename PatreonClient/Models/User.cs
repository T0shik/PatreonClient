using PatreonClient.Models.Attributes;

namespace PatreonClient.Models
{
    public class User : PatreonResponse<UserAttributes>
    {
        public PatreonData<CampaignAttributes> Campaign =>
            Includes.TryGetValue("campaign", out object value) ? (PatreonData<CampaignAttributes>)value : null;
    }
}