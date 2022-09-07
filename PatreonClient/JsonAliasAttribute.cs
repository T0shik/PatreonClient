using System;

namespace PatreonClient;

public class JsonAliasAttribute : Attribute
{
    public string Name { get; }

    public JsonAliasAttribute(string name)
    {
        Name = name;
    }
}