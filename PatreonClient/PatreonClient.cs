using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;

namespace PatreonClient
{
    public class PatreonClient
    {
        private readonly HttpClient _client;

        public PatreonClient(HttpClient client)
        {
            _client = client;
        }

        public UserRequestBuilder GetIdentity() => new UserRequestBuilder(this);
        public CampaignsRequestBuilder GetCampaigns() => new CampaignsRequestBuilder(this);
        public CampaignRequestBuilder GetCampaign(string campaignId) => new CampaignRequestBuilder(this, campaignId);
        public MemberRequestBuilder GetMember(string memberId) => new MemberRequestBuilder(this, memberId);
        public MembersRequestBuilder GetMembers(string campaignId) => new MembersRequestBuilder(this, campaignId);

        public async Task<T> SendAsync<T, TAttr>(string url)
            where T : PatreonResponseBase<TAttr>
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine(content);

            var result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            var doc = JsonDocument.Parse(Encoding.UTF8.GetBytes(content));

            var obj = (IEnumerable<JsonProperty>) doc.RootElement.EnumerateObject();
            var included = obj.FirstOrDefault(x => x.Name == "included");

            var array = included.Value.EnumerateArray();

            foreach (var el in array)
            {
                var type = el.EnumerateObject().FirstOrDefault(x => x.Name == "type").Value.ToString();

                if (type == "campaign")
                {
                    result.Includes.Add("campaign",
                                        JsonSerializer.Deserialize<PatreonData<CampaignAttributes>>(
                                            el.ToString(), new JsonSerializerOptions
                                            {
                                                PropertyNameCaseInsensitive
                                                    = true
                                            }));
                }
                else if (type == "user")
                {
                    result.Includes.Add("user",
                                        JsonSerializer.Deserialize<PatreonData<UserAttributes>>(
                                            el.ToString(), new JsonSerializerOptions
                                            {
                                                PropertyNameCaseInsensitive
                                                    = true
                                            }));
                }
            }

            return result;
        }
    }
}