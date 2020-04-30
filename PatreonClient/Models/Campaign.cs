using PatreonClient.Models.Attributes;

namespace PatreonClient.Models
{
    public class Campaign : PatreonResponse<CampaignAttributes>
    {
        public PatreonData<UserAttributes> Creator =>
            Includes.TryGetValue("user", out object value) ? (PatreonData<UserAttributes>)value : null;
    }
}