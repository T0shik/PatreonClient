using System.Collections.Generic;
using PatreonClient.Internals;
using PatreonClient.Models;
using PatreonClient.Requests;

namespace PatreonClient.RequestBuilders
{
    internal abstract class RequestBuilder<TResponse, TAttributes, TRelationships>
        : IRequestBuilder<TResponse, TAttributes, TRelationships>
        where TResponse : IPatreonResponse<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        internal bool WithParams { get; }
        internal string Url { get; }
        internal List<Field> Fields { get; }
        internal List<string> Includes { get; }

        internal RequestBuilder(List<Field> fields, List<string> includes, string url, bool withParams)
        {
            WithParams = withParams;
            Url = url;
            Fields = fields ?? new List<Field>();
            Includes = includes ?? new List<string>();
        }

        public IPatreonRequest<TResponse, TAttributes, TRelationships> Build()
        {
            var url = BuildUrl();
            return WithParams
                       ? new ParameterizedPatreonRequest<TResponse, TAttributes, TRelationships>(url)
                       : new PatreonRequest<TResponse, TAttributes, TRelationships>(url);
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
    }
}