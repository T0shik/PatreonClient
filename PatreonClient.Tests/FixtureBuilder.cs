using System.Collections.Generic;
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

        public PatreonData<TAttribute, TRelationship> CreateData<TAttribute, TRelationship>(TRelationship relationship)
            where TRelationship : IRelationship
        {
            return _fixture.Build<PatreonData<TAttribute, TRelationship>>()
                           .With(x => x.Relationships, relationship)
                           .Create();
        }

        public PatreonResponse<TAttribute, TRelationship> CreateInitResponse<TAttribute, TRelationship>()
            where TRelationship : IRelationship
        {
            var data = CreateData<TAttribute, TRelationship>();
            var response = _fixture.Build<PatreonResponse<TAttribute, TRelationship>>()
                                   .With(x => x.Data, data)
                                   .Create();

            return response;
        }

        public (PatreonResponse<TAttribute, TRelationship> Response, PatreonData Data) CreateResponse<TAttribute, TRelationship>()
            where TRelationship : IRelationship
        {
            var data = CreateData<TAttribute, TRelationship>();
            var response = _fixture.Build<PatreonResponse<TAttribute, TRelationship>>()
                                   .With(x => x.Data,
                                         new PatreonData<TAttribute, TRelationship>
                                         {
                                             Id = data.Id,
                                             Type = data.Type
                                         })
                                   .Create();

            return (response, data);
        }

        public (PatreonResponse<TAttribute, TRelationship> Response, PatreonData Data) CreateResponse<TAttribute, TRelationship>(TRelationship relationship)
            where TRelationship : IRelationship
        {
            var data = CreateData<TAttribute, TRelationship>(relationship);
            var response = _fixture.Build<PatreonResponse<TAttribute, TRelationship>>()
                                   .With(x => x.Data,
                                         new PatreonData<TAttribute, TRelationship>
                                         {
                                             Id = data.Id,
                                             Type = data.Type
                                         })
                                   .Create();

            return (response, data);
        }

        public (PatreonCollectionResponse<TAttribute, TRelationship> Response, IEnumerable<PatreonData> Data) CreateCollectionResponse<TAttribute, TRelationship>(int count)
            where TRelationship : IRelationship
        {
            var data = Enumerable.Range(0, count).Select(_ => CreateData<TAttribute, TRelationship>()).ToList();
            var response = _fixture.Build<PatreonCollectionResponse<TAttribute, TRelationship>>()
                                   .With(x => x.Data,
                                         data.Select(x => new PatreonData<TAttribute, TRelationship>
                                         {
                                             Id = x.Id,
                                             Type = x.Type
                                         }))
                                   .Create();

            return (response, data);
        }

        public (PatreonCollectionResponse<TAttribute, TRelationship> Response, IEnumerable<PatreonData> Data) CreateCollectionResponse<TAttribute, TRelationship>(IEnumerable<TRelationship> relationships)
            where TRelationship : IRelationship
        {
            var data = relationships.Select(CreateData<TAttribute, TRelationship>).ToList();
            var response = _fixture.Build<PatreonCollectionResponse<TAttribute, TRelationship>>()
                                   .With(x => x.Data,
                                         data.Select(x => new PatreonData<TAttribute, TRelationship>
                                         {
                                             Id = x.Id,
                                             Type = x.Type
                                         }))
                                   .Create();

            return (response, data);
        }
    }
}