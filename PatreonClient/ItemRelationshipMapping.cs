using System;
using System.Reflection;
using PatreonClient.Models;

namespace PatreonClient
{
    internal class ItemRelationshipMapping
    {
        public string Type { get; set; }
        internal Type DecodedType { get; set; }
        internal MethodInfo Deserializer { get; set; }

        public PatreonData Deserialize(string Json)
        {
            return Deserializer.Invoke(null, new object[] {Json, Settings.JsonSerializerOptions }) as PatreonData;
        }
    }
}