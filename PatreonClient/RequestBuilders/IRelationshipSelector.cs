using System;
using System.Linq.Expressions;
using PatreonClient.Models;
using PatreonClient.Responses;

namespace PatreonClient.RequestBuilders
{
    public interface IRelationshipSelector<TResponse, TAttributes, TRelationships>
        : IRequestBuilder<TResponse, TAttributes, TRelationships>
        where TResponse : IPatreonResponse<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        public INestedRelationshipSelector<TResponse, TAttributes, TRelationships, TRel> Include<TAttr, TRel>(
            Expression<Func<TRelationships, IPatreonResponse<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TRel : IRelationship;
    }
}