using System;
using System.Linq.Expressions;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;

namespace PatreonClient
{
    public class MembersRequestBuilder : RequestBuilderBase<Member, MemberAttributes>
    {
        public MembersRequestBuilder(PatreonClient client, string campaignId)
            : base(client, $"/api/oauth2/v2/campaigns/{campaignId}/members") { }

        public MembersRequestBuilder SelectFields(Expression<Func<MemberAttributes, object>> selector)
        {
            SelectFields("member", selector);
            return this;
        }

        public MembersRequestBuilder IncludeUser(Expression<Func<UserAttributes, object>> selector)
        {
            Include("user", "user", selector);
            return this;
        }

        public MembersRequestBuilder IncludeCampaign(Expression<Func<CampaignAttributes, object>> selector)
        {
            Include("campaign", "campaign", selector);
            return this;
        }
    }
}