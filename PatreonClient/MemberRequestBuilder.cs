using System;
using System.Linq.Expressions;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;

namespace PatreonClient
{
    public class MemberRequestBuilder : RequestBuilderBase<Member, MemberAttributes>
    {
        public MemberRequestBuilder(PatreonClient client, string memberId)
            : base(client, $"/api/oauth2/v2/members/{memberId}") { }

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
    }
}