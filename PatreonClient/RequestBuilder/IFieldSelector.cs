using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PatreonClient.Models;

namespace PatreonClient.RequestBuilder
{
    public interface IFieldSelector<TResponse, TAttributes, TRelationships>
        : IRequestBuilder<TResponse, TAttributes, TRelationships>
        where TResponse : PatreonResponseBase<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        IRelationshipSelector<TResponse, TAttributes, TRelationships> SelectFields(
            Expression<Func<TAttributes, object>> selector = null);
    }
    
    internal class FieldSelector<TResponse, TAttributes, TRelationships>
        : RequestBuilder<TResponse, TAttributes, TRelationships>,
          IFieldSelector<TResponse, TAttributes, TRelationships>
        where TResponse : PatreonResponseBase<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        internal FieldSelector(string url, bool withParams = false) : base(null, null, url, withParams) { }

        internal FieldSelector(List<Field> fields, List<string> includes, string url, bool withParams)
            : base(fields, includes, url, withParams) { }

        public IRelationshipSelector<TResponse, TAttributes, TRelationships> SelectFields(Expression<Func<TAttributes, object>> selector)
        {
            var dataIdentifier = typeof(TAttributes).Name.ToLowerInvariant();
            Fields.Add(selector == null ? Field.All<TAttributes>(dataIdentifier) : Field.Create<TAttributes>(dataIdentifier, selector));
            return new RelationshipSelector<TResponse, TAttributes, TRelationships>(Fields, Includes, Url, WithParams);
        }
    }
}