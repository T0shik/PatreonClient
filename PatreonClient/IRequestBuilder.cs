using System;
using System.Linq.Expressions;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;
using PatreonClient.Models.Relationships;

namespace PatreonClient
{
    public interface IFieldSelector<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        public IRequestBuilder<TAttributes, TRelationships> SelectFields(
            Expression<Func<TAttributes, object>> selector = null);
    }

    public interface IRequestBuilder<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        public IRequestBuilder<TAttributes, TRelationships, TRel> Include<TAttr, TRel>(
            Expression<Func<TRelationships, IPatreonResponse<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TRel : IRelationship;
    }

    public interface IRequestBuilder<TAttributes, TOrigin, TNext> : IRequestBuilder<TAttributes, TOrigin>
        where TOrigin : IRelationship
        where TNext : IRelationship
    {
        public IRequestBuilder<TAttributes, TOrigin, TRel> ThenInclude<TAttr, TRel>(
            Expression<Func<TNext, IPatreonResponse<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TRel : IRelationship;
    }
}