using System.Collections.Generic;
using System.Linq;

namespace PatreonClient.Models;

public abstract class BaseRelationship : IRelationship
{
    public abstract void AssignRelationship(IReadOnlyCollection<PatreonData> includes);

    protected internal BaseRelationship AssignData<TAttributes>(IReadOnlyCollection<PatreonData> includes,
        PatreonResponse<TAttributes> baseType)
    {
        if (baseType?.Data == null) return this;

        baseType.Data = includes.FirstOrDefault(x => x.Id == baseType.Data.Id) as PatreonData<TAttributes>;

        return this;
    }

    protected internal BaseRelationship AssignDataAndRelationship<TAttributes, TRelationships>(
        IReadOnlyCollection<PatreonData> includes, PatreonResponse<TAttributes, TRelationships> baseType)
        where TRelationships : IRelationship
    {
        if (baseType?.Data == null) return this;

        baseType.Data =
            includes.FirstOrDefault(x => x.Id == baseType.Data.Id) as PatreonData<TAttributes, TRelationships>;

        baseType.Data?.Relationships?.AssignRelationship(includes);

        return this;
    }

    protected internal BaseRelationship AssignCollectionAttributesAndRelationships<TAttributes, TRelationships>(
        IReadOnlyCollection<PatreonData> includes,
        PatreonCollectionResponse<TAttributes, TRelationships> baseCollection) where TRelationships : IRelationship
    {
        if (baseCollection?.Data == null) return this;

        foreach (var collectionInstance in baseCollection.Data)
        {
            if (includes.FirstOrDefault(x => x.Id == collectionInstance.Id) is PatreonData<TAttributes, TRelationships> include)
            {
                collectionInstance.Attributes = include.Attributes;
                collectionInstance.Relationships = include.Relationships;
            }

            collectionInstance.Relationships?.AssignRelationship(includes);
        }

        return this;
    }
}