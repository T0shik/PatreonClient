using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using PatreonClient.Internals;
using PatreonClient.Models;

namespace PatreonClient.RequestBuilders
{
    internal class RelationshipSelector<TResponse, TAttributes, TRelationships>
        : FieldSelector<TResponse, TAttributes, TRelationships>,
          IRelationshipSelector<TResponse, TAttributes, TRelationships>
        where TResponse : IPatreonResponse<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        public RelationshipSelector(List<Field> fields, List<string> includes, string url, bool withParams)
            : base(fields, includes, url, withParams) { }

        public INestedRelationshipSelector<TResponse, TAttributes, TRelationships, TRel> Include<TAttr, TRel>(
            Expression<Func<TRelationships, IPatreonResponse<TAttr, TRel>>> relationshipSelector,
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

            var attribute = (JsonPropertyNameAttribute) body.Member.GetCustomAttribute(typeof(JsonPropertyNameAttribute));

            if (Includes.Contains(attribute.Name)) return attribute.Name;

            Includes.Add(attribute.Name);
            Fields.Add(fieldSelector == null ? Field.All<TAttr>() : Field.Create<TAttr>(fieldSelector));

            return attribute.Name;
        }
    }
}