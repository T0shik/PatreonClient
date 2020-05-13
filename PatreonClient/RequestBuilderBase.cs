using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;

namespace PatreonClient
{
    public abstract class RequestBuilderBase<T>
    {
        private readonly PatreonClient _client;
        private List<Field> Fields { get; } = new List<Field>();
        private List<string> Includes { get; } = new List<string>();

        protected RequestBuilderBase(PatreonClient client)
        {
            // todo null check
            _client = client;
        }

        public void SelectFields(Expression<Func<T, object>> selector)
        {
            Fields.Add(selector == null ? Field.All<T>() : Field.Create(selector));
        }

        public void Include<TAttr>(string includeName, Expression<Func<TAttr, object>> selector)
        {
            Fields.Add(Field.Create(selector));
            Includes.Add(includeName);
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

        public Task<PatreonResponse<T>> GetSingle(string url)
        {
            return _client.GetSingle<T>(BuildUrl(url));
        }

        public Task<PatreonCollectionResponse<T>> GetCollection(string url)
        {
            return _client.GetCollection<T>(BuildUrl(url));
        }
    }
}