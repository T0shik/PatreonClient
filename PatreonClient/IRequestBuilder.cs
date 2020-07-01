using System;
using System.Linq.Expressions;
using PatreonClient.Models;
using PatreonClient.Requests;

namespace PatreonClient
{
    public interface IRequestBuilderBase<TResponse, TAttributes, TRelationships>
        where TResponse : IPatreonResponse<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        IPatreonRequest<TResponse, TAttributes, TRelationships> Build();
    }

    public interface IFieldSelector<TResponse, TAttributes, TRelationships>
        : IRequestBuilderBase<TResponse, TAttributes, TRelationships>
        where TResponse : IPatreonResponse<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        IRequestBuilder<TResponse, TAttributes, TRelationships> SelectFields(
            Expression<Func<TAttributes, object>> selector = null);
    }

    public interface IRequestBuilder<TResponse, TAttributes, TRelationships>
        : IRequestBuilderBase<TResponse, TAttributes, TRelationships>
        where TResponse : IPatreonResponse<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        public IRequestBuilder<TResponse, TAttributes, TRelationships, TRel> Include<TAttr, TRel>(
            Expression<Func<TRelationships, IPatreonResponse<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TRel : IRelationship;
    }

    public interface IRequestBuilder<TResponse, TAttributes, TOrigin, TNext>
        : IRequestBuilder<TResponse, TAttributes, TOrigin>
        where TResponse : IPatreonResponse<TAttributes, TOrigin>
        where TOrigin : IRelationship
        where TNext : IRelationship
    {
        public IRequestBuilder<TResponse, TAttributes, TOrigin, TRel> ThenInclude<TAttr, TRel>(
            Expression<Func<TNext, IPatreonResponse<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TRel : IRelationship;
    }
}