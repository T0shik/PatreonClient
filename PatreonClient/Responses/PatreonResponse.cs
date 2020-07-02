using PatreonClient.Models;
using PatreonClient.Models.Relationships;

namespace PatreonClient.Responses
{
    public class PatreonResponse<TAttributes> : PatreonResponseBase<TAttributes, Empty>
    {
        public PatreonData<TAttributes> Data { get; set; }
    }
    public class PatreonResponse<TAttributes, TRelationships> : PatreonResponseBase<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        public PatreonData<TAttributes, TRelationships> Data { get; set; }
    }
}