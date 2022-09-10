using PatreonClient.Models;
using PatreonClient.Responses;

namespace PatreonClient.Requests.Builder
{
    public interface IRequestBuilder<TResponse, TAttributes, TRelationships>
        where TResponse : PatreonResponseBase<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        IPatreonRequest<TResponse, TAttributes, TRelationships> Build();
    }
}