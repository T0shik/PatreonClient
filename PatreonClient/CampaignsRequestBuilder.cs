using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;

namespace PatreonClient
{
    public class CampaignsRequestBuilder : RequestBuilderBase<Campaigns, CampaignAttributes>
    {
        public CampaignsRequestBuilder(PatreonClient client)
            : base(client, "/api/oauth2/v2/campaigns") { }

        public CampaignsRequestBuilder SelectFields(Expression<Func<CampaignAttributes, object>> selector)
        {
            SelectFields("campaign", selector);
            return this;
        }

        public CampaignsRequestBuilder IncludeCreator(Expression<Func<UserAttributes, object>> selector)
        {
            Include("user", "creator", selector);
            return this;
        }
    }
}