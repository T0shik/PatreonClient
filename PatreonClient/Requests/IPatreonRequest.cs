using PatreonClient.Models;

namespace PatreonClient.Requests
{
    public interface IPatreonRequest<TResponse, TAttribute, TRelationship>
        where TResponse : IPatreonResponse<TAttribute, TRelationship>
        where TRelationship : IRelationship
    {
        string Url { get; }
    }
}