using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using PatreonClient.Models;
using PatreonClient.Requests;

namespace PatreonClient.Responses
{
    public class PatreonCollectionResponse<TAttributes, TRelationships> : PatreonResponseBase<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        [JsonPropertyName("data")] public IEnumerable<PatreonData<TAttributes, TRelationships>> Data { get; set; }
        [JsonPropertyName("meta")] public Meta Meta { get; set; }
        [JsonPropertyName("type")] public string Type { get; set; }

        public bool HasMore => !string.IsNullOrEmpty(Links?.Next);

        public PatreonRequest<PatreonCollectionResponse<TAttributes, TRelationships>, TAttributes, TRelationships> NextRequest()
        {
            if (!HasMore)
            {
                throw new InvalidOperationException("No more data to fetch");
            }
            return new PatreonRequest<PatreonCollectionResponse<TAttributes, TRelationships>, TAttributes, TRelationships>(Links.Next);
        }
    }
}