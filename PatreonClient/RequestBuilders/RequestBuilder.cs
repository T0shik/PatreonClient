using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.WebUtilities;
using PatreonClient.Models;
using PatreonClient.Requests;
using PatreonClient.Responses;

namespace PatreonClient.RequestBuilders
{
    internal abstract class RequestBuilder<TResponse, TAttributes, TRelationships>
        : IRequestBuilder<TResponse, TAttributes, TRelationships>
        where TResponse : PatreonResponseBase<TAttributes, TRelationships>
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
            var queryString = new Dictionary<string, string>();

            if (Includes.Any())
            {
                queryString.Add("include", string.Join(',', Includes));
            }

            foreach (var field in Fields)
            {
                queryString.Add($"fields[{field.Type.Name.ToLowerInvariant()}]", string.Join(',', field.Fields));
            }

            return QueryHelpers.AddQueryString(Url, queryString);
        }
    }
}