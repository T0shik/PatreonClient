using System;
using System.Linq.Expressions;
using PatreonClient.Models;
using PatreonClient.Responses;

namespace PatreonClient.RequestBuilders
{
    public interface INestedRelationshipSelector<TResponse, TAttributes, TOrigin, TNext>
        : IRelationshipSelector<TResponse, TAttributes, TOrigin>
        where TResponse : PatreonResponseBase<TAttributes, TOrigin>
        where TOrigin : IRelationship
        where TNext : IRelationship
    {
        public INestedRelationshipSelector<TResponse, TAttributes, TOrigin, TRel> ThenInclude<TAttr, TRel>(
            Expression<Func<TNext, PatreonResponseBase<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TRel : IRelationship;
    }
}