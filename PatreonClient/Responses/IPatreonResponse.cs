using PatreonClient.Models;

namespace PatreonClient.Responses
{
    public interface IPatreonResponse<TAttr, TRel>
        where TRel : IRelationship {    }
}