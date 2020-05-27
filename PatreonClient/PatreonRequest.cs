using System.Collections.Generic;
using System.Threading.Tasks;
using PatreonClient.Models;
using PatreonClient.Models.Relationships;

namespace PatreonClient
{
    public class PatreonRequest<TResponse, TAttribute, TRelationship>
        where TResponse : IPatreonResponse<TAttribute, TRelationship>
        where TRelationship : IRelationship
    {
        private readonly PatreonClient _client;
        private readonly string _url;
        private readonly List<Field> _fields;
        private readonly List<string> _includes;

        public PatreonRequest(PatreonClient client, string url, List<Field> fields, List<string> includes)
        {
            _client = client;
            _url = url;
            _fields = fields;
            _includes = includes;
        }

        private string BuildUrl(string url)
        {
            var result = "";
            var hasInclude = _includes.Count > 0;
            var ampersand = false;
            if (hasInclude)
            {
                result = string.Concat("?include=", string.Join(',', _includes));
                ampersand = true;
            }

            foreach (var field in _fields)
            {
                result = string.Concat(result, field.ToString(ampersand ? "&" : "?"));
                ampersand = true;
            }

            return string.Concat(url, result);
        }

        public Task<TResponse> Call()
        {
            return _client.Call<TResponse, TAttribute, TRelationship>(BuildUrl(_url));
        }
    }
}