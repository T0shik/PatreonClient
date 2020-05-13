using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;

namespace PatreonClient
{
    public class CampaignRequestBuilder : RequestBuilderBase<Campaign>
    {
        public CampaignRequestBuilder(PatreonClient client)
            : base(client) { }

        public CampaignRequestBuilder Fields(Expression<Func<Campaign, object>> selector = null)
        {
            SelectFields(selector);
            return this;
        }

        public CampaignRequestBuilder IncludeCreator(Expression<Func<User, object>> selector)
        {
            Include("creator", selector);
            return this;
        }

        public Task<PatreonResponse<Campaign>> GetCampaign(string campaignId)
        {
            return GetSingle($"/api/oauth2/v2/campaigns/{campaignId}");
        }

        public Task<PatreonCollectionResponse<Campaign>> GetCampaigns()
        {
            return GetCollection("/api/oauth2/v2/campaigns");
        }
    }
}