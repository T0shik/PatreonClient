using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using PatreonClient.Models;
using PatreonClient.Models.Relationships;

namespace PatreonClient;

public abstract class PatreonResponseBase<TAttr, TRel> 
    where TRel : IRelationship
{
    public Links Links { get; set; }
}

public class PatreonResponse<TAttributes> : PatreonResponseBase<TAttributes, Empty>
{
    public PatreonData<TAttributes> Data { get; set; }
}

public class PatreonResponse<TAttributes, TRelationships>
    : PatreonResponseBase<TAttributes, TRelationships>
    where TRelationships : IRelationship
{
    public PatreonData<TAttributes, TRelationships> Data { get; set; }
}

public class PatreonCollectionResponse<TAttributes, TRelationships> 
    : PatreonResponseBase<TAttributes, TRelationships>
    where TRelationships : IRelationship
{
    [JsonPropertyName("data")] public IEnumerable<PatreonData<TAttributes, TRelationships>> Data { get; set; }
    [JsonPropertyName("meta")] public Meta Meta { get; set; }

    public bool HasMore => !string.IsNullOrEmpty(Links?.Next);

    public PatreonRequest<PatreonCollectionResponse<TAttributes, TRelationships>> NextRequest()
    {
        if (!HasMore)
        {
            throw new InvalidOperationException("No more data to fetch");
        }

        return new(Links.Next);
    }
}