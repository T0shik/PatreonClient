using PatreonClient.Models;

namespace PatreonClient.Requests
{
    public class ParameterizedPatreonRequest<TResponse, TAttribute, TRelationship>
        : PatreonRequest<TResponse, TAttribute, TRelationship>
        where TResponse : IPatreonResponse<TAttribute, TRelationship>
        where TRelationship : IRelationship
    {
        public ParameterizedPatreonRequest(string url) : base(url) { }
    }
}