using System;

namespace PatreonClient
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ItemRelationshipAttribute : Attribute
    {
        public ItemRelationshipAttribute(string jsonName)
        {
            if (string.IsNullOrWhiteSpace(jsonName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(jsonName));
            }

            JsonName = jsonName;
        }

        public ItemRelationshipAttribute(string jsonName, Type relationshipType)
        {
            if (string.IsNullOrWhiteSpace(jsonName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(jsonName));
            }

            JsonName = jsonName;
            RelationshipType = relationshipType;
        }
        public string JsonName { get; set; }
        public Type RelationshipType { get; set; }
    }
}