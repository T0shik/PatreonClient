using System.Collections.Generic;
using System.Linq;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;
using PatreonClient.Models.Relationships;

namespace PatreonClient.Tests.Mocks
{
    public class MockFactory
    {
        public static PatreonResponse<A> CreateRelationship<A>(PatreonResponse<A> response) =>
            new PatreonResponse<A>
            {
                Data = new PatreonData<A>
                {
                    Id = response.Data.Id,
                    Type = response.Data.Type,
                },
                Links = response.Links
            };

        public static PatreonResponse<A, R> CreateRelationship<A, R>(PatreonResponse<A, R> response)
            where R : IRelationship =>
            new PatreonResponse<A, R>
            {
                Data = Strip(response.Data),
                Links = response.Links
            };

        public static PatreonCollectionResponse<A, R> CreateRelationship<A, R>(PatreonCollectionResponse<A, R> response)
            where R : IRelationship =>
            new PatreonCollectionResponse<A, R>
            {
                Data = response.Data.Select(Strip),
                Links = response.Links,
                Meta = response.Meta,
            };

        public static PatreonCollectionResponse<A, R> CreateRelationship<A, R>(params PatreonData<A, R>[] datas)
            where R : IRelationship =>
            new PatreonCollectionResponse<A, R>
            {
                Data = datas.Select(Strip),
            };

        private static PatreonData<A, R> Strip<A, R>(PatreonData<A, R> data)
            where R : IRelationship =>
            new PatreonData<A, R>
            {
                Id = data.Id,
                Type = data.Type,
            };
    }
}