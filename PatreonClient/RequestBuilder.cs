using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using PatreonClient.Models;
using PatreonClient.Models.Relationships;
using JsonAttr = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace PatreonClient
{
    internal class RequestBuilder<TAttributes, TRelationships> : IRequestBuilder<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        internal List<Field> Fields { get; }
        internal List<string> Includes { get; }

        internal RequestBuilder(List<Field> fields, List<string> includes)
        {
            Fields = fields ?? new List<Field>();
            Includes = includes ?? new List<string>();
        }

        public IRequestBuilder<TAttributes, TRelationships> SelectFields(
            Expression<Func<TAttributes, object>> selector)
        {
            Fields.Add(selector == null ? Field.All<TAttributes>() : Field.Create(selector));
            return this;
        }

        public IRequestBuilder<TAttributes, TRelationships, TRel> Include<TAttr, TRel>(
            Expression<Func<TRelationships, IPatreonResponse<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TRel : IRelationship
        {
            Include(relationshipSelector, fieldSelector);
            return new RequestBuilder<TAttributes, TRelationships, TRel>(this);
        }

        protected void Include<TAttr>(
            Expression relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
        {
            if (!(relationshipSelector is LambdaExpression lambda))
                throw new ArgumentException(nameof(relationshipSelector));

            if (!(lambda.Body is MemberExpression body))
                throw new ArgumentException(nameof(relationshipSelector));

            var attribute = (JsonAttr) body.Member.GetCustomAttribute(typeof(JsonAttr));

            if (Includes.Contains(attribute.Name)) return;

            Includes.Add(attribute.Name);
            Fields.Add(fieldSelector == null ? Field.All<TAttr>() : Field.Create(fieldSelector));
        }
    }

    internal class RequestBuilder<TAttributes, TOrigin, TNext> : RequestBuilder<TAttributes, TOrigin>,
                                                                 IRequestBuilder<TAttributes, TOrigin, TNext>
        where TOrigin : IRelationship
        where TNext : IRelationship
    {
        internal RequestBuilder(RequestBuilder<TAttributes, TOrigin> builder) : base(builder.Fields, builder.Includes) { }

        public IRequestBuilder<TAttributes, TOrigin, TRel> ThenInclude<TResponse, TAttr, TRel>(
            Expression<Func<TNext, TResponse>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TResponse : IPatreonResponse<TAttr, TRel> where TRel : IRelationship
        {
            Include(relationshipSelector, fieldSelector);
            return new RequestBuilder<TAttributes, TOrigin, TRel>(this);
        }
    }
}