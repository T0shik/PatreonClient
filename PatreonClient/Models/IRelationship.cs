using System.Collections.Generic;

namespace PatreonClient.Models
{
    public interface IRelationship
    {
        void AssignRelationship(IReadOnlyCollection<PatreonData> includes);
    }
}