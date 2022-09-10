using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;
using PatreonClient.Models.Relationships;

namespace PatreonClient;

public class PatreonClient
{
    private readonly HttpClient _client;

    public PatreonClient(HttpClient client)
    {
        _client = client;
    }

    public PatreonClient(string apiKey)
    {
        _client = new()
        {
            BaseAddress = new("https://www.patreon.com"),
            DefaultRequestHeaders =
            {
                Authorization =  new("Bearer", apiKey),
            }
        };
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<PatreonResponse<TAttribute, TRelationship>> GetAsync<TAttribute, TRelationship>(
        PatreonRequest<PatreonResponse<TAttribute, TRelationship>> request
    )
        where TRelationship : IRelationship
    {
        var content = await SendAsync(request.Url);

        return ParseResponse<TAttribute, TRelationship>(content);
    }

    public async Task<PatreonCollectionResponse<TAttribute, TRelationship>> GetAsync<TAttribute, TRelationship>(
        PatreonRequest<PatreonCollectionResponse<TAttribute, TRelationship>> request
    )
        where TRelationship : IRelationship
    {
        var content = await SendAsync(request.Url);

        return ParseCollectionResponse<TAttribute, TRelationship>(content);
    }

    public async Task<List<PatreonData<TAttribute, TRelationship>>> GetAllAsync<TAttribute, TRelationship>(
        PatreonRequest<PatreonCollectionResponse<TAttribute, TRelationship>> request
    )
        where TRelationship : IRelationship
    {
        var result = new List<PatreonData<TAttribute, TRelationship>>();
        var response = await GetAsync(request);
        result.AddRange(response.Data);
        while (response.HasMore)
        {
            response = await GetAsync(response.NextRequest());
            result.AddRange(response.Data);
        }

        return result;
    }

    public async IAsyncEnumerable<PatreonData<TAttribute, TRelationship>> EnumerateAllAsync<TAttribute, TRelationship>(
        PatreonRequest<PatreonCollectionResponse<TAttribute, TRelationship>> request
    )
        where TRelationship : IRelationship
    {
        var response = await GetAsync(request);
        foreach (var data in response.Data)
        {
            yield return data;
        }

        while (response.HasMore)
        {
            response = await GetAsync(response.NextRequest());
            foreach (var data in response.Data)
            {
                yield return data;
            }
        }
    }

    private PatreonResponse<TAttribute, TRelationship> ParseResponse<TAttribute, TRelationship>(string content)
        where TRelationship : IRelationship
    {
        var result = JsonSerializer.Deserialize<PatreonResponse<TAttribute, TRelationship>>(content, JsonSerializerOptions);

        var includes = AggregateIncludes(content).ToList();
        if (includes.Count > 0)
        {
            result.Data.Relationships.AssignRelationship(includes);
        }

        return result;
    }

    private PatreonCollectionResponse<TAttribute, TRelationship> ParseCollectionResponse<TAttribute, TRelationship>(string content)
        where TRelationship : IRelationship
    {
        var result = JsonSerializer.Deserialize<PatreonCollectionResponse<TAttribute, TRelationship>>(content, JsonSerializerOptions);

        var includes = AggregateIncludes(content).ToList();
        if (includes.Count > 0)
        {
            foreach (var d in result.Data)
                d.Relationships.AssignRelationship(includes);
        }

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
}