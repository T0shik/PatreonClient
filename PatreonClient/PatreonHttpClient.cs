using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
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
            if (string.IsNullOrWhiteSpace(AccessToken))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(AccessToken));
            }

            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            _client.BaseAddress = new Uri("https://www.patreon.com");
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
            _logger?.LogDebug($"Calling {url}");
            
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

                var decodeType = GetType().Assembly.GetTypes().First(e => e.GetCustomAttributes(typeof(ItemRelationshipAttribute), false) is ItemRelationshipAttribute[] a && a.Any(f => f.JsonName == type));

                var attr = decodeType.GetCustomAttribute<ItemRelationshipAttribute>();
                
                if (attr.RelationshipType is null)
                {
                    var patreonDataClass = typeof(PatreonData<>).MakeGenericType(new[] {decodeType});

                    var method = typeof(JsonSerializer).GetMethods().FirstOrDefault(e => e.Name == "Deserialize").MakeGenericMethod(patreonDataClass);

                    yield return method.Invoke(null, new object[]{el.ToString(), JsonSerializerOptions}) as PatreonData;
                }
                else
                {
                    var patreonDataClass = typeof(PatreonData<,>).MakeGenericType(new[] { decodeType, attr.RelationshipType });

                    var method = typeof(JsonSerializer).GetMethods().FirstOrDefault(e => e.Name == "Deserialize").MakeGenericMethod(patreonDataClass);

                    yield return method.Invoke(null, new object[] { el.ToString(), JsonSerializerOptions }) as PatreonData;
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