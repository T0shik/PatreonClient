using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Moq.Protected;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;
using PatreonClient.Models.Relationships;
using PatreonClient.Responses;
using PatreonClient.Tests.Mocks;
using Xunit;

namespace PatreonClient.Tests
{
    public class Bogus
    {
        private readonly FixtureBuilder _builder;

        public Bogus()
        {
            _builder = new FixtureBuilder();
        }

        private static PatreonHttpClient CreateClient<T>(T data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                            "SendAsync",
                            ItExpr.IsAny<HttpRequestMessage>(),
                            ItExpr.IsAny<CancellationToken>()
                        )
                       .ReturnsAsync(new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(json)
                        })
                       .Verifiable();

            var client = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://www.test.com/")
            };

            return new PatreonHttpClient(client);
        }

        [Fact]
        public async Task UserResponseParsed()
        {
            var identity = _builder.CreateInitResponse<User, UserRelationships>();

            // var creator = _builder.CreateResponse<User, UserRelationships>();
            var tiers = _builder.CreateCollectionResponse<Tier, TierRelationships>(2);
            var benefits = _builder.CreateCollectionResponse<Benefit, BenefitRelationships>(2);
            var goals = _builder.CreateCollectionResponse<Goal, GoalRelationships>(2);
            var campaignRelationships = new CampaignRelationships
            {
                Creator = new PatreonResponse<User, UserRelationships>
                {
                    Data = new PatreonData<User, UserRelationships>
                    {
                        Id = identity.Data.Id,
                        Type = identity.Data.Type
                    },
                    Links = null,
                },
                Tiers = tiers.Response,
                Benefits = benefits.Response,
                Goals = goals.Response,
            };
            var campaign = _builder.CreateResponse<Campaign, CampaignRelationships>(campaignRelationships);
            var memberships = _builder.CreateCollectionResponse<Member, MemberRelationships>(3);
            var userRelationships = new UserRelationships
            {
                Campaign = campaign.Response,
                Memberships = memberships.Response,
            };
            identity.Data.Relationships = userRelationships;

            var includes = new List<PatreonData>();
            includes.Add(campaign.Data);
            includes.AddRange(memberships.Data);
            // includes.Add(creator.Data);
            includes.AddRange(tiers.Data);
            includes.AddRange(benefits.Data);
            includes.AddRange(goals.Data);

            var patreonClient = CreateClient(new MockResponse
            {
                Data = identity.Data,
                Includes = includes,
                Links = identity.Links
            });

            var request = RequestBuilder.Identity.SelectFields().Build();

            var response = await patreonClient.GetAsync(request);
            var compareLogic = new CompareLogic {Config = {IgnoreObjectTypes = true}};
            var result = compareLogic.Compare(identity, response);

            Assert.True(result.AreEqual);
        }
    }
}