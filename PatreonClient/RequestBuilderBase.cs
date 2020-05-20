using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Relationships;
using JsonAttr = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace PatreonClient
{
    public class RequestBuilderBase<TAttribute, TRelationship>
        where TRelationship : IRelationship
    {
        private readonly PatreonClient _client;
        private readonly string _url;
        private List<Field> Fields { get; } = new List<Field>();
        private List<string> Includes { get; } = new List<string>();

        public RequestBuilderBase(PatreonClient client, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(nameof(url));
            }

            _client = client ?? throw new ArgumentNullException(nameof(client));
            _url = url;
        }

        public RequestBuilderBase<TAttribute, TRelationship> SelectFields()
        {
            Fields.Add(Field.All<TAttribute>());
            return this;
        }

        public RequestBuilderBase<TAttribute, TRelationship> SelectFields(Expression<Func<TAttribute, object>> selector)
        {
            Fields.Add(Field.Create(selector));
            return this;
        }

        public RequestBuilderBase<TAttribute, TRelationship> Include<TAttr, TRel>(
            Expression<Func<TRelationship, PatreonResponse<TAttr, TRel>>> relationshipSelector)
            where TRel : IRelationship
        {
            Include<TAttr>(relationshipSelector, null);
            return this;
        }

        public RequestBuilderBase<TAttribute, TRelationship> Include<TAttr, TRel>(
            Expression<Func<TRelationship, PatreonResponse<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector)
            where TRel : IRelationship
        {
            Include<TAttr>(relationshipSelector, fieldSelector);
            return this;
        }

        public RequestBuilderBase<TAttribute, TRelationship> Include<TAttr, TRel>(
            Expression<Func<TRelationship, PatreonCollectionResponse<TAttr, TRel>>> relationshipSelector)
            where TRel : IRelationship
        {
            Include<TAttr>(relationshipSelector, null);
            return this;
        }

        public RequestBuilderBase<TAttribute, TRelationship> Include<TAttr, TRel>(
            Expression<Func<TRelationship, PatreonCollectionResponse<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector)
            where TRel : IRelationship
        {
            Include<TAttr>(relationshipSelector, fieldSelector);
            return this;
        }

        private void Include<TAttr>(
            Expression relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
        {
            if (!(relationshipSelector is LambdaExpression lambda))
            {
                throw new ArgumentException(nameof(relationshipSelector));
            }

            if (!(lambda.Body is MemberExpression body))
            {
                throw new ArgumentException(nameof(relationshipSelector));
            }

            var attribute = (JsonAttr) body.Member.GetCustomAttribute(typeof(JsonAttr));
            Includes.Add(attribute.Name);
            Fields.Add(fieldSelector == null ? Field.All<TAttr>() : Field.Create(fieldSelector));
        }

        private string BuildUrl(string url)
        {
            var result = "";
            var hasInclude = Includes.Count > 0;
            var ampersand = false;
            if (hasInclude)
            {
                result = string.Concat("?include=", string.Join(',', Includes));
                ampersand = true;
            }

            foreach (var field in Fields)
            {
                result = string.Concat(result, field.ToString(ampersand ? "&" : "?"));
                ampersand = true;
            }

            return string.Concat(url, result);
        }

        public Task<PatreonResponse<TAttribute, TRelationship>> GetSingle()
        {
            return _client.GetSingle<TAttribute, TRelationship>(BuildUrl(_url));
        }

        public Task<PatreonCollectionResponse<TAttribute, TRelationship>> GetCollection()
        {
            return _client.GetCollection<TAttribute, TRelationship>(BuildUrl(_url));
        }
    }
}