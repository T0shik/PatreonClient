using PatreonClient.Models.Attributes;
using PatreonClient.Models.Relationships;

namespace PatreonClient.Models
{
    public class PatreonResponse<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        public PatreonData<TAttributes, TRelationships> Data { get; set; }
        public Links Links { get; set; }
    }
}