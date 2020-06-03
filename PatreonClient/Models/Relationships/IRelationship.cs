using System.Collections.Generic;

namespace PatreonClient.Models.Relationships
{
    public interface IRelationship
    {
        void AssignRelationship(IReadOnlyCollection<PatreonData> includes);
    }
}