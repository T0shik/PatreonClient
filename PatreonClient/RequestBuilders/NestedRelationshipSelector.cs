using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using PatreonClient.Models;
using PatreonClient.Responses;

namespace PatreonClient.RequestBuilders
{
    internal class NestedRelationshipSelector<TResponse, TAttributes, TOrigin, TNext>
        : RelationshipSelector<TResponse, TAttributes, TOrigin>,
          INestedRelationshipSelector<TResponse, TAttributes, TOrigin, TNext>
        where TResponse : PatreonResponseBase<TAttributes, TOrigin>
        where TOrigin : IRelationship
        where TNext : IRelationship
    {
        private readonly string _path;

        public NestedRelationshipSelector(
            string path,
            List<Field> fields,
            List<string> includes,
            string url,
            bool withParams)
            : base(fields, includes, url, withParams)
        {
            _path = path;
        }

        public INestedRelationshipSelector<TResponse, TAttributes, TOrigin, TRel> ThenInclude<TAttr, TRel>(
            Expression<Func<TNext, PatreonResponseBase<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TRel : IRelationship
        {
            var path = HandleIncludesAndFields<TAttr>(relationshipSelector, fieldSelector);
            return new NestedRelationshipSelector<TResponse, TAttributes, TOrigin, TRel>(path, Fields, Includes, Url, WithParams);
        }

        private string HandleIncludesAndFields<TAttr>(
            Expression relationshipSelector,
            Expression fieldSelector = null)
        {
            if (!(relationshipSelector is LambdaExpression lambda))
                throw new ArgumentException(nameof(relationshipSelector));

            if (!(lambda.Body is MemberExpression body))
                throw new ArgumentException(nameof(relationshipSelector));

            var attribute = (JsonPropertyNameAttribute) body.Member.GetCustomAttribute(typeof(JsonPropertyNameAttribute));

            var path = string.Concat(_path, ".", attribute.Name);
            if (Includes.Contains(path)) return path;

            Includes.Add(path);
            Fields.Add(fieldSelector == null ? Field.All<TAttr>() : Field.Create<TAttr>(fieldSelector));

            return path;
        }
    }
}