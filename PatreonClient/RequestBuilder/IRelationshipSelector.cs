using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using PatreonClient.Internals;
using PatreonClient.Models;

namespace PatreonClient.RequestBuilder
{
    public interface IRelationshipSelector<TResponse, TAttributes, TRelationships>
        : IRequestBuilder<TResponse, TAttributes, TRelationships>
        where TResponse : PatreonResponseBase<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        public INestedRelationshipSelector<TResponse, TAttributes, TRelationships, TRel> Include<TAttr, TRel>(
            Expression<Func<TRelationships, PatreonResponseBase<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TRel : IRelationship;
    }
        internal class RelationshipSelector<TResponse, TAttributes, TRelationships>
        : FieldSelector<TResponse, TAttributes, TRelationships>,
            IRelationshipSelector<TResponse, TAttributes, TRelationships>
        where TResponse : PatreonResponseBase<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        public RelationshipSelector(List<Field> fields, List<string> includes, string url, bool withParams)
            : base(fields, includes, url, withParams)
        {
        }

        public INestedRelationshipSelector<TResponse, TAttributes, TRelationships, TRel> Include<TAttr, TRel>(
            Expression<Func<TRelationships, PatreonResponseBase<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TRel : IRelationship
        {
            var path = HandleIncludesAndFields<TAttr>(relationshipSelector, fieldSelector);
            return new NestedRelationshipSelector<TResponse, TAttributes, TRelationships, TRel>(
                path,
                Fields,
                Includes,
                Url,
                WithParams);
        }

        private string HandleIncludesAndFields<TAttr>(
            Expression relationshipSelector,
            Expression fieldSelector = null)
        {
            if (!(relationshipSelector is LambdaExpression lambda))
                throw new ArgumentException(nameof(relationshipSelector));

            if (!(lambda.Body is MemberExpression body))
                throw new ArgumentException(nameof(relationshipSelector));

            var attribute = (JsonPropertyNameAttribute)body.Member.GetCustomAttribute(typeof(JsonPropertyNameAttribute));
            var includesIdentifier = attribute.Name;
            
            var alias = typeof(TAttr).GetCustomAttribute(typeof(JsonAliasAttribute)) as JsonAliasAttribute;
            var fieldIdentifier = alias?.Name ?? typeof(TAttr).Name.ToLowerInvariant();

            if (Includes.Contains(includesIdentifier)) return includesIdentifier;

            Includes.Add(includesIdentifier);
            Fields.Add(fieldSelector == null ? Field.All<TAttr>(fieldIdentifier) : Field.Create<TAttr>(fieldIdentifier, fieldSelector));

            return includesIdentifier;
        }
    }

}