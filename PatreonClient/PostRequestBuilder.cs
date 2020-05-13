using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;

namespace PatreonClient
{
    public class PostRequestBuilder : RequestBuilderBase<Post>
    {
        public PostRequestBuilder(PatreonClient client)
            : base(client) { }

        public PostRequestBuilder Fields(Expression<Func<Post, object>> selector = null)
        {
            SelectFields(selector);
            return this;
        }

        public PostRequestBuilder IncludeUser(Expression<Func<User, object>> selector)
        {
            Include("user", selector);
            return this;
        }

        public Task<PatreonResponse<Post>> GetPost(string postId)
        {
            return GetSingle(string.Concat("/api/oauth2/v2/posts/", postId));
        }

        public Task<PatreonCollectionResponse<Post>> GetPosts(string campaignId)
        {
            return GetCollection(string.Concat("/api/oauth2/v2/campaigns/", campaignId, "/posts"));
        }
    }
}