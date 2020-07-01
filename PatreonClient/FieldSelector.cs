using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PatreonClient.Internals;
using PatreonClient.Models;
using PatreonClient.RequestBuilders;

namespace PatreonClient
{
    internal class FieldSelector<TResponse, TAttributes, TRelationships>
        : RequestBuilder<TResponse, TAttributes, TRelationships>,
          IFieldSelector<TResponse, TAttributes, TRelationships>
        where TResponse : IPatreonResponse<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        internal FieldSelector(string url, bool withParams = false) : base(null, null, url, withParams) { }

        internal FieldSelector(List<Field> fields, List<string> includes, string url, bool withParams)
            : base(fields, includes, url, withParams) { }

        public IRelationshipSelector<TResponse, TAttributes, TRelationships> SelectFields(Expression<Func<TAttributes, object>> selector)
        {
            Fields.Add(selector == null ? Field.All<TAttributes>() : Field.Create<TAttributes>(selector));
            return new RelationshipSelector<TResponse, TAttributes, TRelationships>(Fields, Includes, Url, WithParams);
        }
    }
}