using System.Linq;
using AutoFixture;
using PatreonClient.Models;
using PatreonClient.Responses;

namespace PatreonClient.Tests.Mocks
{
    public class FixtureBuilder
    {
        private readonly Fixture _fixture;

        public FixtureBuilder()
        {
            _fixture = new Fixture();
        }

        public PatreonData<TAttribute> CreateData<TAttribute>()
        {
            return _fixture.Build<PatreonData<TAttribute>>()
                           .With(x => x.Type, typeof(TAttribute).Name.ToLowerInvariant())
                           .Create();
        }

        public PatreonData<TAttribute, TRelationship> CreateData<TAttribute, TRelationship>()
            where TRelationship : IRelationship
        {
            return _fixture.Build<PatreonData<TAttribute, TRelationship>>()
                           .With(x => x.Type, typeof(TAttribute).Name.ToLowerInvariant())
                           .Without(x => x.Relationships)
                           .Create();
        }

        public PatreonResponse<TAttribute> CreateResponse<TAttribute>()
        {
            var data = CreateData<TAttribute>();

            var response = _fixture.Build<PatreonResponse<TAttribute>>()
                                   .With(x => x.Data, data)
                                   .Create();

            return response;
        }

        public PatreonResponse<TAttribute, TRelationship> CreateResponse<TAttribute, TRelationship>()
            where TRelationship : IRelationship
        {
            var data = CreateData<TAttribute, TRelationship>();

            var response = _fixture.Build<PatreonResponse<TAttribute, TRelationship>>()
                                   .With(x => x.Data, data)
                                   .Create();

            return response;
        }

        public PatreonCollectionResponse<TAttribute, TRelationship> CreateCollectionResponse<TAttribute, TRelationship>(int count, bool init = false)
            where TRelationship : IRelationship
        {
            var data = Enumerable.Range(0, count).Select(_ => CreateData<TAttribute, TRelationship>()).ToList();

            var builder = _fixture.Build<PatreonCollectionResponse<TAttribute, TRelationship>>()
                                  .With(x => x.Data, data);

            if (!init)
            {
                builder.Without(x => x.Meta)
                       .Without(x => x.Links);
            }

            return builder.Create();
        }

    }
}