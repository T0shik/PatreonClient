using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;

namespace PatreonClient
{
    public class UserRequestBuilder : RequestBuilderBase<UserAttributes>
    {
        public UserRequestBuilder(PatreonClient client)
            : base(client) { }

        public UserRequestBuilder SelectFields(Expression<Func<UserAttributes, object>> selector)
        {
            SelectFields("user", selector);
            return this;
        }

        public UserRequestBuilder IncludeCampaign(Expression<Func<CampaignAttributes, object>> selector)
        {
            Include("campaign", "campaign", selector);
            return this;
        }

        public Task<PatreonResponse<UserAttributes>> GetUser()
        {
            return GetSingle("/api/oauth2/v2/identity");
        }
    }
}