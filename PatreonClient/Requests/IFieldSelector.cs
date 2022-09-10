using System;
using System.Linq.Expressions;
using PatreonClient.Models;

namespace PatreonClient.Requests;

public interface IFieldSelector<TAttributes, TRelationships>
    where TRelationships : IRelationship
{
    IRelationshipSelector<TAttributes, TRelationships> SelectFields(
        Expression<Func<TAttributes, object>> selector = null);
}

internal class FieldSelector<TAttributes, TRelationships>
    : IFieldSelector<TAttributes, TRelationships>
    where TRelationships : IRelationship
{
    protected readonly RequestBuilder Builder;

    internal FieldSelector(RequestBuilder builder)
    {
        Builder = builder;
    }

    public IRelationshipSelector<TAttributes, TRelationships> SelectFields(Expression<Func<TAttributes, object>> selector)
    {
        var dataIdentifier = typeof(TAttributes).Name.ToLowerInvariant();
        Builder.Fields.Add(selector == null ? Field.All<TAttributes>(dataIdentifier) : Field.Create<TAttributes>(dataIdentifier, selector));
        return new RelationshipSelector<TAttributes, TRelationships>(Builder);
    }
}