using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PatreonClient.Models;

namespace PatreonClient
{
    public abstract class RequestBuilderBase<TResponse, TAttribute>
        where TResponse : PatreonResponseBase<TAttribute>
    {
        private readonly PatreonClient _client;
        private readonly string Url;
        private List<Field> Fields { get;  } = new List<Field>();
        private List<string> Includes { get;  } = new List<string>();

        protected RequestBuilderBase(PatreonClient client, string url)
        {
            // todo null check
            _client = client;
            Url = url;
        }

        public void SelectFields<T>(string fieldName, Expression<Func<T, object>> selector)
        {
            Fields.Add(new Field(fieldName, selector));
        }

        public void Include<T>(
            string fieldName,
            string includeName,
            Expression<Func<T, object>> selector)
        {
            Fields.Add(new Field(fieldName, selector));
            Includes.Add(includeName);
        }

        private string BuildUrl()
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

            return string.Concat(Url, result);
        }

        public Task<TResponse> Call()
        {
            return _client.SendAsync<TResponse, TAttribute>(BuildUrl());
        }
    }
}