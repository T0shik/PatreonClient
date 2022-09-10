using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using PatreonClient.Internals;
using PatreonClient.Models;

namespace PatreonClient.Requests;

public interface INestedRelationshipSelector<TAttributes, TOrigin, TNext>
    : IRelationshipSelector<TAttributes, TOrigin>
    // where TResponse : PatreonResponseBase<TAttributes, TOrigin>
    where TOrigin : IRelationship
    where TNext : IRelationship
{
    public INestedRelationshipSelector<TAttributes, TOrigin, TRel> ThenInclude<TAttr, TRel>(
        Expression<Func<TNext, PatreonResponseBase<TAttr, TRel>>> relationshipSelector,
        Expression<Func<TAttr, object>> fieldSelector = null)
        where TRel : IRelationship;
}
    
internal class NestedRelationshipSelector<TAttributes, TOrigin, TNext>
    : RelationshipSelector<TAttributes, TOrigin>,
        INestedRelationshipSelector<TAttributes, TOrigin, TNext>
    where TOrigin : IRelationship
    where TNext : IRelationship
{
    private readonly string _path;

    public NestedRelationshipSelector(string path, RequestBuilder builder)
        : base(builder)
    {
        _path = path;
    }

    public INestedRelationshipSelector<TAttributes, TOrigin, TRel> ThenInclude<TAttr, TRel>(
        Expression<Func<TNext, PatreonResponseBase<TAttr, TRel>>> relationshipSelector,
        Expression<Func<TAttr, object>> fieldSelector = null)
        where TRel : IRelationship
    {
        var path = HandleIncludesAndFields<TAttr>(relationshipSelector, fieldSelector);
        return new NestedRelationshipSelector<TAttributes, TOrigin, TRel>(path, Builder);
    }

    private string HandleIncludesAndFields<TAttr>(
        Expression relationshipSelector,
        Expression fieldSelector = null)
    {
        if (relationshipSelector is not LambdaExpression lambda)
            throw new ArgumentException(nameof(relationshipSelector));

        if (lambda.Body is not MemberExpression body)
            throw new ArgumentException(nameof(relationshipSelector));

        var attribute = (JsonPropertyNameAttribute)body.Member.GetCustomAttribute(typeof(JsonPropertyNameAttribute));
        var includesIdentifier = attribute.Name;

        var alias = typeof(TAttr).GetCustomAttribute(typeof(JsonAliasAttribute)) as JsonAliasAttribute;
        var fieldIdentifier = alias?.Name ?? typeof(TAttr).Name.ToLowerInvariant();

        var path = string.Concat(_path, ".", includesIdentifier);
        if (Builder.Includes.Contains(path)) return path;

        Builder.Includes.Add(path);
        Builder.Fields.Add(
            fieldSelector == null
                ? Field.All<TAttr>(fieldIdentifier)
                : Field.Create<TAttr>(fieldIdentifier, fieldSelector)
        );

        return path;
    }
}