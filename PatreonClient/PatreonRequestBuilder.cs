using PatreonClient.Models;
using PatreonClient.Models.Attributes;
using PatreonClient.Models.Relationships;
using PatreonClient.RequestBuilder;

namespace PatreonClient
{
    public static class PatreonRequestBuilder
    {
        public static IFieldSelector<PatreonResponse<User, UserRelationships>, User, UserRelationships> Identity =>
            new FieldSelector<PatreonResponse<User, UserRelationships>, User, UserRelationships>("/api/oauth2/v2/identity");

        public static IFieldSelector<PatreonResponse<Campaign, CampaignRelationships>, Campaign, CampaignRelationships> Campaign =>
            new FieldSelector<PatreonResponse<Campaign, CampaignRelationships>, Campaign, CampaignRelationships>("/api/oauth2/v2/campaigns/{0}", true);

        public static IFieldSelector<PatreonCollectionResponse<Campaign, CampaignRelationships>, Campaign, CampaignRelationships> Campaigns =>
            new FieldSelector<PatreonCollectionResponse<Campaign, CampaignRelationships>, Campaign, CampaignRelationships>("/api/oauth2/v2/campaigns");

        public static IFieldSelector<PatreonResponse<Member, MemberRelationships>, Member, MemberRelationships> Member =>
            new FieldSelector<PatreonResponse<Member, MemberRelationships>, Member, MemberRelationships>("/api/oauth2/v2/members/{0}", true);

        public static IFieldSelector<PatreonCollectionResponse<Member, MemberRelationships>, Member, MemberRelationships> Members =>
            new FieldSelector<PatreonCollectionResponse<Member, MemberRelationships>, Member, MemberRelationships>("/api/oauth2/v2/campaigns/{0}/members", true);

        public static IFieldSelector<PatreonResponse<Post, PostRelationships>, Post, PostRelationships> Post =>
            new FieldSelector<PatreonResponse<Post, PostRelationships>, Post, PostRelationships>("/api/oauth2/v2/posts/{0}", true);

        public static IFieldSelector<PatreonCollectionResponse<Post, PostRelationships>, Post, PostRelationships> Posts =>
            new FieldSelector<PatreonCollectionResponse<Post, PostRelationships>, Post, PostRelationships>("/api/oauth2/v2/campaigns/{0}/posts", true);
    }
}