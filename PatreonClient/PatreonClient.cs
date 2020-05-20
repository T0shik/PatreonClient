using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;
using PatreonClient.Models.Relationships;

namespace PatreonClient
{
    public class PatreonClient
    {
        private readonly HttpClient _client;

        public PatreonClient(HttpClient client)
        {
            _client = client;
        }

        public RequestBuilderBase<User, UserRelationships> Identity() =>
            new RequestBuilderBase<User, UserRelationships>(this, "/api/oauth2/v2/identity");

        public RequestBuilderBase<Campaign, CampaignRelationships> Campaign(string campaignId) =>
            new RequestBuilderBase<Campaign, CampaignRelationships>(
                this,
                string.Concat("/api/oauth2/v2/campaigns/", campaignId));

        public RequestBuilderBase<Campaign, CampaignRelationships> Campaigns() =>
            new RequestBuilderBase<Campaign, CampaignRelationships>(this, "/api/oauth2/v2/campaigns");

        public RequestBuilderBase<Member, MemberRelationships> Member(string memberId) =>
            new RequestBuilderBase<Member, MemberRelationships>(
                this,
                string.Concat("/api/oauth2/v2/members/", memberId));

        public RequestBuilderBase<Member, MemberRelationships> Members(string campaignId) =>
            new RequestBuilderBase<Member, MemberRelationships>(
                this,
                string.Concat("/api/oauth2/v2/campaigns/", campaignId, "/members"));

        public RequestBuilderBase<Post, PostRelationships> Post(string postId) =>
            new RequestBuilderBase<Post, PostRelationships>(
                this,
                string.Concat("/api/oauth2/v2/posts/", postId));

        public RequestBuilderBase<Post, PostRelationships> Posts(string campaignId) =>
            new RequestBuilderBase<Post, PostRelationships>(
                this,
                string.Concat("/api/oauth2/v2/campaigns/", campaignId, "/posts"));

        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private async Task<string> SendAsync(string url)
        {
            Console.WriteLine(url);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Bad Request {content}");
            }

            Console.WriteLine(content);
            return content;
        }

        public async Task<PatreonResponse<TAttribute, TRelationship>> GetSingle<TAttribute, TRelationship>(string url)
            where TRelationship : IRelationship
        {
            var content = await SendAsync(url);

            var result =
                JsonSerializer.Deserialize<PatreonResponse<TAttribute, TRelationship>>(content, JsonSerializerOptions);

            ResolveRelationship(content, (id, type, json) => result.Data.Relationships.AssignRelationship(id, type, json));

            return result;
        }

        public async Task<PatreonCollectionResponse<TAttribute, TRel>> GetCollection<TAttribute, TRel>(string url)
            where TRel : IRelationship
        {
            var content = await SendAsync(url);

            Console.WriteLine(content);

            var result =
                JsonSerializer.Deserialize<PatreonCollectionResponse<TAttribute, TRel>>(
                    content,
                    JsonSerializerOptions);

            ResolveRelationship(content,
                                (id, type, json) =>
                                {
                                    foreach (var d in result.Data)
                                        d.Relationships.AssignRelationship(id, type, json);
                                });

            return result;
        }

        private delegate void AddToAttribute(string id, string type, string element);

        private static void ResolveRelationship(string content, AddToAttribute addToAttribute)
        {
            var doc = JsonDocument.Parse(Encoding.UTF8.GetBytes(content));

            var obj = (IEnumerable<JsonProperty>) doc.RootElement.EnumerateObject();
            var included = obj.FirstOrDefault(x => x.Name == "included");

            var array = included.Value.EnumerateArray();

            foreach (var el in array)
            {
                var type = el.EnumerateObject().FirstOrDefault(x => x.Name == "type").Value.ToString();
                var id = el.EnumerateObject().FirstOrDefault(x => x.Name == "id").Value.ToString();
                addToAttribute(id, type, el.ToString());
            }
        }
    }
}