using System.Collections.Generic;

namespace PatreonClient.Models.Relationships
{
    public class EmptyRelationship : IRelationship
    {
        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
        }
    }
}