using PatreonClient.Models;

namespace PatreonClient.Responses
{
    public abstract class PatreonResponseBase<TAttr, TRel>
        where TRel : IRelationship
    {
        public Links Links { get; set; }
    }
}