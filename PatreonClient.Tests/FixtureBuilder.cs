using System.Linq;
using AutoFixture;
using PatreonClient.Models;
using PatreonClient.Responses;

namespace PatreonClient.Tests
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
                           .Create();
        }

        public PatreonData<TAttribute, TRelationship> CreateData<TAttribute, TRelationship>()
            where TRelationship : IRelationship
        {
            return _fixture.Build<PatreonData<TAttribute, TRelationship>>()
                           .Without(x => x.Relationships)
                           .Create();
        }

        public PatreonResponse<TAttribute, TRelationship> CreateResponse<TAttribute, TRelationship>()
            where TRelationship : IRelationship
        {
            var data = CreateData<TAttribute, TRelationship>();
            return _fixture.Build<PatreonResponse<TAttribute, TRelationship>>()
                           .With(x => x.Data, data)
                           .Create();
        }

        public PatreonCollectionResponse<TAttribute, TRelationship> CreateCollectionResponse<TAttribute, TRelationship>(int count)
            where TRelationship : IRelationship
        {
            var data = Enumerable.Range(0, count).Select(_ => CreateData<TAttribute, TRelationship>()).ToList();
            return _fixture.Build<PatreonCollectionResponse<TAttribute, TRelationship>>()
                           .With(x => x.Data, data)
                           .Create();
        }
    }
}