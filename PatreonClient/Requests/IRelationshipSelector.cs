using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using PatreonClient.Internals;
using PatreonClient.Models;

namespace PatreonClient.Requests;

public interface IRelationshipSelector<TAttributes, TRelationships>
    where TRelationships : IRelationship
{
    public INestedRelationshipSelector<TAttributes, TRelationships, TRel> Include<TAttr, TRel>(
        Expression<Func<TRelationships, PatreonResponseBase<TAttr, TRel>>> relationshipSelector,
        Expression<Func<TAttr, object>> fieldSelector = null)
        where TRel : IRelationship;
}

internal class RelationshipSelector<TAttributes, TRelationships>
    : FieldSelector<TAttributes, TRelationships>,
        IRelationshipSelector<TAttributes, TRelationships>
    where TRelationships : IRelationship
{
    public RelationshipSelector(RequestBuilder builder) : base(builder)
    {
    }

    public INestedRelationshipSelector<TAttributes, TRelationships, TRel> Include<TAttr, TRel>(
        Expression<Func<TRelationships, PatreonResponseBase<TAttr, TRel>>> relationshipSelector,
        Expression<Func<TAttr, object>> fieldSelector = null)
        where TRel : IRelationship
    {
        var path = HandleIncludesAndFields<TAttr>(relationshipSelector, fieldSelector);
        return new NestedRelationshipSelector<TAttributes, TRelationships, TRel>(
            path,
            Builder
        );
    }

    private string HandleIncludesAndFields<TAttr>(
        Expression relationshipSelector,
        Expression fieldSelector = null)
    {
        if (!(relationshipSelector is LambdaExpression lambda))
            throw new ArgumentException(nameof(relationshipSelector));

        if (!(lambda.Body is MemberExpression body))
            throw new ArgumentException(nameof(relationshipSelector));

        var attribute = (JsonPropertyNameAttribute)body.Member.GetCustomAttribute(typeof(JsonPropertyNameAttribute));
        var includesIdentifier = attribute.Name;

        var alias = typeof(TAttr).GetCustomAttribute(typeof(JsonAliasAttribute)) as JsonAliasAttribute;
        var fieldIdentifier = alias?.Name ?? typeof(TAttr).Name.ToLowerInvariant();

        if (Builder.Includes.Contains(includesIdentifier)) return includesIdentifier;

        Builder.Includes.Add(includesIdentifier);
        Builder.Fields.Add(fieldSelector == null ? Field.All<TAttr>(fieldIdentifier) : Field.Create<TAttr>(fieldIdentifier, fieldSelector));

        return includesIdentifier;
    }
}