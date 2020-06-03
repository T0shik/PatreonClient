using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public Task<TResponse> SendAsync<TResponse, TAttribute, TRelationship>(
            PatreonParameterizedRequest<TResponse, TAttribute, TRelationship> request,
            string parameter)
            where TResponse : IPatreonResponse<TAttribute, TRelationship>
            where TRelationship : IRelationship
        {
            return SendAsync<TResponse, TAttribute, TRelationship>(string.Format(request.Url, parameter));
        }

        public Task<TResponse> SendAsync<TResponse, TAttribute, TRelationship>(
            PatreonRequest<TResponse, TAttribute, TRelationship> request)
            where TResponse : IPatreonResponse<TAttribute, TRelationship>
            where TRelationship : IRelationship
        {
            return SendAsync<TResponse, TAttribute, TRelationship>(request.Url);
        }

        private async Task<TResponse> SendAsync<TResponse, TAttribute, TRelationship>(string url)
            where TResponse : IPatreonResponse<TAttribute, TRelationship>
            where TRelationship : IRelationship
        {
            var content = await SendAsync(url);

            var result = JsonSerializer.Deserialize<TResponse>(content, JsonSerializerOptions);

            var includes = AggregateIncludes(content).ToList();
            if (includes.Count > 0)
                DistributeIncludes(includes, result);

            return result;
        }

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

        private IEnumerable<PatreonData> AggregateIncludes(string content)
        {
            var doc = JsonDocument.Parse(Encoding.UTF8.GetBytes(content));
            var obj = (IEnumerable<JsonProperty>) doc.RootElement.EnumerateObject();
            var included = obj.FirstOrDefault(x => x.Name == "included");

            var array = included.Value.EnumerateArray();

            foreach (var el in array)
            {
                var type = el.EnumerateObject().FirstOrDefault(x => x.Name == "type").Value.ToString();
                if (type.Equals("campaign"))
                {
                    yield return
                        JsonSerializer.Deserialize<PatreonData<Campaign, CampaignRelationships>>(el.ToString());
                }
                else if (type.Equals("membership"))
                {
                    yield return JsonSerializer.Deserialize<PatreonData<Member, MemberRelationships>>(el.ToString());
                }
                else if (type.Equals("user") || type.Equals("creator"))
                {
                    yield return JsonSerializer.Deserialize<PatreonData<User, UserRelationships>>(el.ToString());
                }
                else if (type.Equals("tier"))
                {
                    yield return JsonSerializer.Deserialize<PatreonData<Tier, TierRelationships>>(el.ToString());
                }
            }
        }

        private static void DistributeIncludes<TAttr, TRel>(
            IReadOnlyCollection<PatreonData> includes,
            IPatreonResponse<TAttr, TRel> result)
            where TRel : IRelationship
        {
            if (result is PatreonResponse<TAttr, TRel> single)
                single.Data.Relationships.AssignRelationship(includes);

            else if (result is PatreonCollectionResponse<TAttr, TRel> collection)
                foreach (var d in collection.Data)
                    d.Relationships.AssignRelationship(includes);
        }
    }
}