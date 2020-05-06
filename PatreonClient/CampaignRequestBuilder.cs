using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;

namespace PatreonClient
{
    public class CampaignRequestBuilder : RequestBuilderBase<CampaignAttributes>
    {
        public CampaignRequestBuilder(PatreonClient client)
            : base(client) { }

        public CampaignRequestBuilder SelectFields(Expression<Func<CampaignAttributes, object>> selector)
        {
            SelectFields("campaign", selector);
            return this;
        }

        public CampaignRequestBuilder IncludeCreator(Expression<Func<UserAttributes, object>> selector)
        {
            Include("user", "creator", selector);
            return this;
        }

        public Task<PatreonResponse<CampaignAttributes>> GetCampaign(string campaignId)
        {
            return GetSingle($"/api/oauth2/v2/campaigns/{campaignId}");
        }

        public Task<PatreonCollectionResponse<CampaignAttributes>> GetCampaigns()
        {
            return GetCollection("/api/oauth2/v2/campaigns");
        }
    }
}