using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Moq.Protected;
using PatreonClient.Models.Attributes;
using PatreonClient.Models.Relationships;
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

        public static PatreonHttpClient CreateClient<T>(T data)
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
        public async Task Do()
        {
            var identity = _builder.CreateResponse<User, UserRelationships>();
            identity.Data.Relationships = new UserRelationships
            {
                Campaign = _builder.CreateResponse<Campaign, CampaignRelationships>(),
                Memberships = _builder.CreateCollectionResponse<Member, MemberRelationships>(3)
            };

            var patreonClient = CreateClient(identity);

            var request = RequestBuilder.Identity
                                        .SelectFields()
                                        .Include(x => x.Campaign)
                                        .Include(x => x.Memberships)
                                        .Build();

            var response = await patreonClient.GetAsync(request);
            var compareLogic = new CompareLogic {Config = {IgnoreObjectTypes = true}};
            var result = compareLogic.Compare(identity, response);

            Assert.True(result.AreEqual);
        }
    }
}