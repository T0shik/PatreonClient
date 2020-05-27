using PatreonClient.Models.Relationships;

namespace PatreonClient.Models
{
    public interface IPatreonResponse<TAttr, TRel>
        where TRel : IRelationship {    }
}