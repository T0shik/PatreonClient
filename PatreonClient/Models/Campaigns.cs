using PatreonClient.Models.Attributes;

namespace PatreonClient.Models
{
    public class Campaigns : PatreonCollectionResponse<CampaignAttributes>
    {
        public PatreonData<UserAttributes> Creator =>
            Includes.TryGetValue("user", out object value) ? (PatreonData<UserAttributes>)value : null;
    }
}