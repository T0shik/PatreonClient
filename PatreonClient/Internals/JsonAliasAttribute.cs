using System;

namespace PatreonClient.Internals;

internal class JsonAliasAttribute : Attribute
{
    internal string Name { get; }

    internal JsonAliasAttribute(string name)
    {
        Name = name;
    }
}