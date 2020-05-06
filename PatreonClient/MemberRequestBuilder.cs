using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;

namespace PatreonClient
{
    public class MemberRequestBuilder : RequestBuilderBase<MemberAttributes>
    {
        public MemberRequestBuilder(PatreonClient client)
            : base(client) { }

        public MemberRequestBuilder SelectFields(Expression<Func<MemberAttributes, object>> selector)
        {
            SelectFields("member", selector);
            return this;
        }

        public MemberRequestBuilder IncludeUser(Expression<Func<UserAttributes, object>> selector)
        {
            Include("user", "user", selector);
            return this;
        }

        public MemberRequestBuilder IncludeCampaign(Expression<Func<CampaignAttributes, object>> selector)
        {
            Include("campaign", "campaign", selector);
            return this;
        }

        public Task<PatreonResponse<MemberAttributes>> GetMember(string memberId)
        {
            return GetSingle($"/api/oauth2/v2/members/{memberId}");
        }

        public Task<PatreonCollectionResponse<MemberAttributes>> GetMembers(string campaignId)
        {
            return GetCollection($"/api/oauth2/v2/campaigns/{campaignId}/members");
        }
    }
}