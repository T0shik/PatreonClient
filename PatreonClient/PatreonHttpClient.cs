using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;
using PatreonClient.Models.Relationships;
using PatreonClient.Requests;
using PatreonClient.Responses;

namespace PatreonClient
{
    public class PatreonHttpClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<PatreonHttpClient> _logger;

        public PatreonHttpClient(HttpClient client, ILogger<PatreonHttpClient> logger, string AccessToken) 
        {
            _client = client;
            _logger = logger;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            _client.BaseAddress = new Uri("https://www.patreon.com");
        }

        // For backwards compat
        public PatreonHttpClient(HttpClient client, ILogger<PatreonHttpClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<TResponse> GetAsync<TResponse, TAttribute, TRelationship>(
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
                return await SendAsync<TResponse, TAttribute, TRelationship>(string.Format(request.Url, parameter));
            }

            if (request is PatreonRequest<TResponse, TAttribute, TRelationship>)
            {
                return await SendAsync<TResponse, TAttribute, TRelationship>(request.Url);
            }

            throw new ArgumentException($"Invalid {nameof(request)}");
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
            var content = await _client.GetStringAsync(url);
            _logger?.LogTrace(content);
            
            var result = JsonSerializer.Deserialize<TResponse>(content, JsonSerializerOptions);

            var includes = AggregateIncludes(content).ToList();
            if (includes.Count > 0)
                DistributeIncludes(includes, result);

            return result;
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