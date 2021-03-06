﻿using System;
using System.Linq.Expressions;
using PatreonClient.Models;
using PatreonClient.Responses;

namespace PatreonClient.RequestBuilders
{
    public interface IFieldSelector<TResponse, TAttributes, TRelationships>
        : IRequestBuilder<TResponse, TAttributes, TRelationships>
        where TResponse : PatreonResponseBase<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        IRelationshipSelector<TResponse, TAttributes, TRelationships> SelectFields(
            Expression<Func<TAttributes, object>> selector = null);
    }
}