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

        public PatreonRequest<PatreonResponse<User, UserRelationships>, User, UserRelationships> Identity(
            Action<IRequestBuilder<User, UserRelationships>> builderAction)
        {
            var builder = new RequestBuilder<User, UserRelationships>(null, null);
            builderAction(builder);

            return new PatreonRequest<PatreonResponse<User, UserRelationships>, User, UserRelationships>(
                this,
                "/api/oauth2/v2/identity",
                builder.Fields,
                builder.Includes);
        }

        // public ISingleRequestBuilder<Campaign, CampaignRelationships> Campaign(string campaignId) =>
        //     new FieldSelector<Campaign, CampaignRelationships>(
        //         this,
        //         string.Concat("/api/oauth2/v2/campaigns/", campaignId));
        //
        // public ICollectionRequestBuilder<Campaign, CampaignRelationships> Campaigns() =>
        //     new FieldSelector<Campaign, CampaignRelationships>(this, "/api/oauth2/v2/campaigns");
        //
        // public ISingleRequestBuilder<Member, MemberRelationships> Member(string memberId) =>
        //     new FieldSelector<Member, MemberRelationships>(
        //         this,
        //         string.Concat("/api/oauth2/v2/members/", memberId));
        //
        // public ICollectionRequestBuilder<Member, MemberRelationships> Members(string campaignId) =>
        //     new FieldSelector<Member, MemberRelationships>(
        //         this,
        //         string.Concat("/api/oauth2/v2/campaigns/", campaignId, "/members"));
        //
        // public ISingleRequestBuilder<Post, PostRelationships> Post(string postId) =>
        //     new FieldSelector<Post, PostRelationships>(
        //         this,
        //         string.Concat("/api/oauth2/v2/posts/", postId));
        //
        // public ICollectionRequestBuilder<Post, PostRelationships> Posts(string campaignId) =>
        //     new FieldSelector<Post, PostRelationships>(
        //         this,
        //         string.Concat("/api/oauth2/v2/campaigns/", campaignId, "/posts"));


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

        public async Task<TResponse> Call<TResponse,TAttribute, TRelationship>(string url)
            where TResponse : IPatreonResponse<TAttribute, TRelationship>
            where TRelationship : IRelationship
        {
            var content = await SendAsync(url);

            var result =
                JsonSerializer.Deserialize<TResponse>(content, JsonSerializerOptions);

            // ResolveRelationship(content, (id, type, json) => result.Data.Relationships.AssignRelationship(id, type, json));
            // ResolveRelationship(content,
            //                     (id, type, json) =>
            //                     {
            //                         foreach (var d in result.Data)
            //                             d.Relationships.AssignRelationship(id, type, json);
            //                     });
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