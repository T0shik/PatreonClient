using PatreonClient.Models;
using PatreonClient.Models.Relationships;

namespace PatreonClient.Responses
{
    public class PatreonResponse<TAttributes> : IPatreonResponse<TAttributes, Empty>
    {
        public PatreonData<TAttributes> Data { get; set; }
        public Links Links { get; set; }
    }
    public class PatreonResponse<TAttributes, TRelationships> : IPatreonResponse<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        public PatreonData<TAttributes, TRelationships> Data { get; set; }
        public Links Links { get; set; }
    }
}