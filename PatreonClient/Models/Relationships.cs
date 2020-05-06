using PatreonClient.Models.Attributes;

namespace PatreonClient.Models
{
    public class Relationships
    {
        public PatreonResponse<CampaignAttributes> Campaign { get; set; }
        public PatreonResponse<UserAttributes> User { get; set; }
        public PatreonResponse<UserAttributes> Creator { get; set; }
    }
}