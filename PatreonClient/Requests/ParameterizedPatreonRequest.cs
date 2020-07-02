using PatreonClient.Models;
using PatreonClient.Responses;

namespace PatreonClient.Requests
{
    public class ParameterizedPatreonRequest<TResponse, TAttribute, TRelationship>
        : PatreonRequest<TResponse, TAttribute, TRelationship>
        where TResponse : PatreonResponseBase<TAttribute, TRelationship>
        where TRelationship : IRelationship
    {
        public ParameterizedPatreonRequest(string url) : base(url) { }
    }
}