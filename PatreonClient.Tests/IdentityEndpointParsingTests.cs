using System;
using System.Collections.Generic;
using System.Linq;
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
using MockFactory = PatreonClient.Tests.Mocks.MockFactory;

namespace PatreonClient.Tests
{
    public class IdentityEndpointParsingTests
    {
        private readonly FixtureBuilder _builder;
        private readonly CompareLogic _compareLogic;

        public IdentityEndpointParsingTests()
        {
            _builder = new FixtureBuilder();
            _compareLogic = new CompareLogic {Config = {IgnoreObjectTypes = true}};
        }

        private static PatreonClient CreateClient(object data)
        {
            var json = JsonSerializer.Serialize(data,
                                                new JsonSerializerOptions
                                                {
                                                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
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

            return new PatreonClient(client);
        }

        [Fact]
        public async Task Parses()
        {
            // Arrange
            var tierImages = new List<PatreonResponse<Media>>
            {
                _builder.CreateResponse<Media>(),
                _builder.CreateResponse<Media>(),
                _builder.CreateResponse<Media>(),
            };
            var benefits = _builder.CreateCollectionResponse<Benefit, BenefitRelationships>(3);
            var benefitsList = benefits.Data.ToList();
            var tiers = _builder.CreateCollectionResponse<Tier, TierRelationships>(3);
            tiers.Data = tiers.Data.Select((d, i) =>
            {
                d.Relationships = new TierRelationships
                {
                    TierImage = MockFactory.CreateRelationship(tierImages[i]),
                    Benefits = MockFactory.CreateRelationship(benefitsList[i])
                };
                return d;
            });

            var campaign = _builder.CreateResponse<Campaign, CampaignRelationships>();
            campaign.Data.Relationships = new CampaignRelationships
            {
                Tiers = MockFactory.CreateRelationship(tiers),
                Benefits = MockFactory.CreateRelationship(benefits),
            };

            var identity = _builder.CreateResponse<User, UserRelationships>();
            identity.Data.Relationships = new UserRelationships
            {
                Campaign = MockFactory.CreateRelationship(campaign),
            };

            var includes = new List<PatreonData>();
            includes.Add(campaign.Data);
            includes.AddRange(tiers.Data);
            includes.AddRange(benefits.Data);
            includes.AddRange(tierImages.Select(x => x.Data));

            var patreonClient = CreateClient(new MockResponse
            {
                Data = identity.Data,
                Included = includes,
                Links = identity.Links
            });

            var request = PatreonRequestBuilder.Identity.SelectFields().Build();

            // Act
            var response = await patreonClient.GetAsync(request);

            // Arrange 2
            tiers.Data = tiers.Data.Select((d, i) =>
            {
                d.Relationships = new TierRelationships
                {
                    TierImage = tierImages[i],
                    Benefits = new PatreonCollectionResponse<Benefit, BenefitRelationships>
                    {
                        Data = new[] {benefitsList[i]}
                    }
                };
                return d;
            });
            campaign.Data.Relationships = new CampaignRelationships
            {
                Tiers = tiers,
                Benefits = benefits,
            };
            identity.Data.Relationships = new UserRelationships
            {
                Campaign = campaign
            };

            // Assert
            var result = _compareLogic.Compare(identity, response);
            Assert.True(result.AreEqual);
        }
    }
}