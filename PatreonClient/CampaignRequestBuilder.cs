using System;
using System.Linq.Expressions;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;

namespace PatreonClient
{
    public class CampaignRequestBuilder : RequestBuilderBase<Campaign, CampaignAttributes>
    {
        public CampaignRequestBuilder(PatreonClient client, string campaignId)
            : base(client, $"/api/oauth2/v2/campaigns/{campaignId}") { }

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
    }
}