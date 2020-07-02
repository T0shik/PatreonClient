using PatreonClient.Models;
using PatreonClient.Responses;

namespace PatreonClient.Requests
{
    public interface IPatreonRequest<TResponse, TAttribute, TRelationship>
        where TResponse : PatreonResponseBase<TAttribute, TRelationship>
        where TRelationship : IRelationship
    {
        string Url { get; }
    }
}