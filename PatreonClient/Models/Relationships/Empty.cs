using System;
using System.Collections.Generic;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models.Relationships
{
    public class Empty : IRelationship
    {
        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes) =>
            throw new InvalidOperationException();
    }
}