using System.Collections.Generic;
using System.Linq;
using PatreonClient.Responses;

namespace PatreonClient.Models
{
	public static class IRelationshipExtensions
	{
		public static IRelationship AssignData<BaseType>(this IRelationship relationship, IReadOnlyCollection<PatreonData> includes, PatreonResponse<BaseType> baseType)
		{
			if (baseType?.Data == null) return relationship;

			baseType.Data = includes.FirstOrDefault(x => x.Id == baseType.Data.Id) as PatreonData<BaseType>;

			return relationship;
		}

		public static IRelationship AssignDataAndRelationship<BaseType, RelationshipType>(this IRelationship relationship, IReadOnlyCollection<PatreonData> includes, PatreonResponse<BaseType, RelationshipType> baseType) where RelationshipType : IRelationship
		{
			if (baseType?.Data == null) return relationship;

			baseType.Data = includes.FirstOrDefault(x => x.Id == baseType.Data.Id) as PatreonData<BaseType, RelationshipType>;

			baseType.Data?.Relationships?.AssignRelationship(includes);

			return relationship;
		}

		public static IRelationship AssignCollectionAttributesAndRelationships<BaseType, RelationshipType>(this IRelationship relationship, IReadOnlyCollection<PatreonData> includes, PatreonCollectionResponse<BaseType, RelationshipType> baseCollection) where RelationshipType : IRelationship
		{
			if (baseCollection?.Data == null) return relationship;

			foreach (var collectionInstance in baseCollection.Data)
			{
				var include = includes.FirstOrDefault(x => x.Id == collectionInstance.Id) as PatreonData<BaseType, RelationshipType>;
				if (include != null)
				{
					collectionInstance.Attributes = include.Attributes;
					collectionInstance.Relationships = include.Relationships;
				}

				collectionInstance.Relationships?.AssignRelationship(includes);
			}

			return relationship;
		}
	}
}
