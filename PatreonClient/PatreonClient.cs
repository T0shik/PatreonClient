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

        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public Task<TResponse> GetAsync<TResponse, TAttribute, TRelationship>(
            IPatreonRequest<TResponse, TAttribute, TRelationship> request,
            string parameter = null)
            where TResponse : PatreonResponseBase<TAttribute, TRelationship>
            where TRelationship : IRelationship
        {
            if (request is ParameterizedPatreonRequest<TResponse, TAttribute, TRelationship>)
            {
                if (string.IsNullOrEmpty(parameter))
                {
                    throw new ArgumentException($"{nameof(parameter)} is required for {typeof(TAttribute).Name}");
                }
                return SendAsync<TResponse, TAttribute, TRelationship>(string.Format(request.Url, parameter));
            }

            if (request is PatreonRequest<TResponse, TAttribute, TRelationship>)
            {
                return SendAsync<TResponse, TAttribute, TRelationship>(request.Url);
            }

            throw new ArgumentException($"invalid {nameof(request)}");
        }

        public async Task<List<PatreonData<TAttribute, TRelationship>>> ListAllAsync<TResponse, TAttribute, TRelationship>(
            IPatreonRequest<TResponse, TAttribute, TRelationship> request,
            string parameter = null
            )
            where TResponse : PatreonCollectionResponse<TAttribute, TRelationship>
            where TRelationship : IRelationship
        {
            var result = new List<PatreonData<TAttribute, TRelationship>>();
            var response = await GetAsync(request, parameter);
            result.AddRange(response.Data);
            while (response.HasMore)
            {
                response = await SendAsync<TResponse, TAttribute, TRelationship>(response.Links.Next);
                result.AddRange(response.Data);
            }

            return result;
        }

        public async IAsyncEnumerable<TResponse> GetAllAsync<TResponse, TAttribute, TRelationship>(
            IPatreonRequest<TResponse, TAttribute, TRelationship> request,
            string parameter = null)
            where TResponse : PatreonCollectionResponse<TAttribute, TRelationship>
            where TRelationship : IRelationship
        {
            var response = await GetAsync(request, parameter);
            yield return response;
            while (response.HasMore)
            {
                response = await SendAsync<TResponse, TAttribute, TRelationship>(response.Links.Next);
                yield return response;
            }
        }

        private async Task<TResponse> SendAsync<TResponse, TAttribute, TRelationship>(string url)
            where TResponse : PatreonResponseBase<TAttribute, TRelationship>
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
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Bad Request {content}");
            }
            
            return content;
        }

        private IEnumerable<PatreonData> AggregateIncludes(string content)
        {
            var doc = JsonDocument.Parse(Encoding.UTF8.GetBytes(content));
            if (!doc.RootElement.TryGetProperty("included", out var included))
            {
                yield break;
            }

            foreach (var el in included.EnumerateArray())
            {
                var type = el.EnumerateObject().FirstOrDefault(x => x.Name == "type").Value.ToString();
                if (type.Equals("campaign"))
                {
                    yield return
                        JsonSerializer.Deserialize<PatreonData<Campaign, CampaignRelationships>>(el.ToString(), JsonSerializerOptions);
                }
                else if (type.Equals("member"))
                {
                    yield return JsonSerializer.Deserialize<PatreonData<Member, MemberRelationships>>(el.ToString(), JsonSerializerOptions);
                }
                else if (type.Equals("user") || type.Equals("creator") || type.Equals("patron"))
                {
                    yield return JsonSerializer.Deserialize<PatreonData<User, UserRelationships>>(el.ToString(), JsonSerializerOptions);
                }
                else if (type.Equals("tier"))
                {
                    var tier = JsonSerializer.Deserialize<PatreonData<Tier, TierRelationships>>(el.ToString(), JsonSerializerOptions);
                    yield return tier;
                }
                else if (type.Equals("media"))
                {
                    yield return JsonSerializer.Deserialize<PatreonData<Media>>(el.ToString(), JsonSerializerOptions);
                }
                else if (type.Equals("benefit"))
                {
                    yield return JsonSerializer.Deserialize<PatreonData<Benefit, BenefitRelationships>>(el.ToString(), JsonSerializerOptions);
                }
                else if (type.Equals("deliverable"))
                {
                    yield return JsonSerializer.Deserialize<PatreonData<Deliverable, DeliverableRelationships>>(el.ToString(), JsonSerializerOptions);
                }
                else if (type.Equals("address"))
                {
                    yield return JsonSerializer.Deserialize<PatreonData<Address, AddressRelationships>>(el.ToString(), JsonSerializerOptions);
                }
                else if (type.Equals("goal"))
                {
                    yield return JsonSerializer.Deserialize<PatreonData<Goal, GoalRelationships>>(el.ToString(), JsonSerializerOptions);
                }
                else if (type.Equals("post"))
                {
                    yield return JsonSerializer.Deserialize<PatreonData<Post, PostRelationships>>(el.ToString(), JsonSerializerOptions);
                }
                else if (type.Equals("pledge-event"))
                {
                    yield return JsonSerializer.Deserialize<PatreonData<Pledge, PledgeRelationships>>(el.ToString(), JsonSerializerOptions);
                }
            }
        }

        private static void DistributeIncludes<TAttr, TRel>(
            IReadOnlyCollection<PatreonData> includes,
            PatreonResponseBase<TAttr, TRel> result)
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