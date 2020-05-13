using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;

namespace PatreonClient
{
    public class MemberRequestBuilder : RequestBuilderBase<Member>
    {
        public MemberRequestBuilder(PatreonClient client)
            : base(client) { }

        public MemberRequestBuilder Fields(Expression<Func<Member, object>> selector = null)
        {
            SelectFields(selector);
            return this;
        }

        public MemberRequestBuilder IncludeUser(Expression<Func<User, object>> selector)
        {
            Include("user", selector);
            return this;
        }

        public MemberRequestBuilder IncludeCampaign(Expression<Func<Campaign, object>> selector)
        {
            Include("campaign", selector);
            return this;
        }

        public Task<PatreonResponse<Member>> GetMember(string memberId)
        {
            return GetSingle($"/api/oauth2/v2/members/{memberId}");
        }

        public Task<PatreonCollectionResponse<Member>> GetMembers(string campaignId)
        {
            return GetCollection($"/api/oauth2/v2/campaigns/{campaignId}/members");
        }
    }
}