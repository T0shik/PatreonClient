using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;

namespace PatreonClient
{
    public class UserRequestBuilder : RequestBuilderBase<User>
    {
        public UserRequestBuilder(PatreonClient client)
            : base(client) { }

        public UserRequestBuilder Fields(Expression<Func<User, object>> selector = null)
        {
            SelectFields(selector);
            return this;
        }

        public UserRequestBuilder IncludeCampaign(Expression<Func<Campaign, object>> selector)
        {
            Include("campaign", selector);
            return this;
        }

        public UserRequestBuilder IncludeMemberships(Expression<Func<Member, object>> selector)
        {
            Include("memberships", selector);
            return this;
        }

        public Task<PatreonResponse<User>> GetUser()
        {
            return GetSingle("/api/oauth2/v2/identity");
        }
    }
}