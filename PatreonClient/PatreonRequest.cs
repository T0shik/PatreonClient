using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;
using PatreonClient.Models.Relationships;

namespace PatreonClient
{
    public class PatreonRequest<TResponse, TAttribute, TRelationship>
        where TResponse : IPatreonResponse<TAttribute, TRelationship>
        where TRelationship : IRelationship
    {
        public PatreonRequest(string url, IEnumerable<Field> fields, IReadOnlyCollection<string> includes) =>
            Url = BuildUrl(url, fields, includes);

        public string Url { get; }

        private static string BuildUrl(string url, IEnumerable<Field> fields, IReadOnlyCollection<string> includes)
        {
            var result = "";
            var hasInclude = includes.Count > 0;
            var ampersand = false;
            if (hasInclude)
            {
                result = string.Concat("?include=", string.Join(',', includes));
                ampersand = true;
            }

            foreach (var field in fields)
            {
                result = string.Concat(result, field.ToString(ampersand ? "&" : "?"));
                ampersand = true;
            }

            return string.Concat(url, result);
        }
    }

    public class PatreonParameterizedRequest<TResponse, TAttribute, TRelationship>
        : PatreonRequest<TResponse, TAttribute, TRelationship>
        where TResponse : IPatreonResponse<TAttribute, TRelationship>
        where TRelationship : IRelationship
    {
        public PatreonParameterizedRequest(string url, IEnumerable<Field> fields, IReadOnlyCollection<string> includes) : base(url, fields, includes) { }
    }
}