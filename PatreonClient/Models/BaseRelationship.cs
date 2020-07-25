using System.Collections.Generic;
using System.Linq;
using PatreonClient.Responses;

namespace PatreonClient.Models
{
    public abstract class BaseRelationShip : IRelationship
    {
        protected internal BaseRelationShip AssignData<TAttributes>(IReadOnlyCollection<PatreonData> includes,
            PatreonResponse<TAttributes> baseType)
        {
            if (baseType?.Data == null) return this;

            baseType.Data = includes.FirstOrDefault(x => x.Id == baseType.Data.Id) as PatreonData<TAttributes>;

            return this;
        }

        protected internal BaseRelationShip AssignDataAndRelationship<TAttributes, TRelationships>(
            IReadOnlyCollection<PatreonData> includes, PatreonResponse<TAttributes, TRelationships> baseType)
            where TRelationships : IRelationship
        {
            if (baseType?.Data == null) return this;

            baseType.Data =
                includes.FirstOrDefault(x => x.Id == baseType.Data.Id) as PatreonData<TAttributes, TRelationships>;

            baseType.Data?.Relationships?.AssignRelationship(includes);

            return this;
        }

        protected internal BaseRelationShip AssignCollectionAttributesAndRelationships<TAttributes, TRelationships>(
            IReadOnlyCollection<PatreonData> includes,
            PatreonCollectionResponse<TAttributes, TRelationships> baseCollection) where TRelationships : IRelationship
        {
            if (baseCollection?.Data == null) return this;

            foreach (var collectionInstance in baseCollection.Data)
            {
                var include =
                    includes.FirstOrDefault(x => x.Id == collectionInstance.Id) as
                        PatreonData<TAttributes, TRelationships>;
                if (include != null)
                {
                    collectionInstance.Attributes = include.Attributes;
                    collectionInstance.Relationships = include.Relationships;
                }

                collectionInstance.Relationships?.AssignRelationship(includes);
            }

            return this;
        }

        public void AssignRelationship(IReadOnlyCollection<PatreonData> includes)
        {
            throw new System.NotImplementedException();
        }
    }
}