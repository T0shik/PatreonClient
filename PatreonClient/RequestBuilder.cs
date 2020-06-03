using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;
using PatreonClient.Models.Relationships;
using JsonAttr = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace PatreonClient
{
    public static class RequestBuilder
    {
        public static PatreonRequest<PatreonResponse<User, UserRelationships>, User, UserRelationships> Identity(
            Action<IFieldSelector<User, UserRelationships>> builderAction)
        {
            var builder = new FieldSelector<User, UserRelationships>(null, null);
            builderAction(builder);

            return new PatreonRequest<PatreonResponse<User, UserRelationships>, User, UserRelationships>(
                "/api/oauth2/v2/identity",
                builder.Fields,
                builder.Includes);
        }

        public static PatreonParameterizedRequest<PatreonResponse<Campaign, CampaignRelationships>, Campaign,
                CampaignRelationships>
            Campaign(Action<IFieldSelector<Campaign, CampaignRelationships>> builderAction)
        {
            var builder = new FieldSelector<Campaign, CampaignRelationships>(null, null);
            builderAction(builder);

            return new PatreonParameterizedRequest<PatreonResponse<Campaign, CampaignRelationships>, Campaign,
                CampaignRelationships>(
                "/api/oauth2/v2/campaigns/{0}",
                builder.Fields,
                builder.Includes);
        }

        // public ICollectionRequestBuilder<Campaign, CampaignRelationships> Campaigns() =>
        //     new FieldSelector<Campaign, CampaignRelationships>(this, "/api/oauth2/v2/campaigns");
        //
        // public ISingleRequestBuilder<Member, MemberRelationships> Member(string memberId) =>
        //     new FieldSelector<Member, MemberRelationships>(
        //         this,
        //         string.Concat("/api/oauth2/v2/members/", memberId));
        //
        // public ICollectionRequestBuilder<Member, MemberRelationships> Members(string campaignId) =>
        //     new FieldSelector<Member, MemberRelationships>(
        //         this,
        //         string.Concat("/api/oauth2/v2/campaigns/", campaignId, "/members"));
        //
        // public ISingleRequestBuilder<Post, PostRelationships> Post(string postId) =>
        //     new FieldSelector<Post, PostRelationships>(
        //         this,
        //         string.Concat("/api/oauth2/v2/posts/", postId));
        //
        // public ICollectionRequestBuilder<Post, PostRelationships> Posts(string campaignId) =>
        //     new FieldSelector<Post, PostRelationships>(
        //         this,
        //         string.Concat("/api/oauth2/v2/campaigns/", campaignId, "/posts"));
    }

    internal class FieldSelector<TAttributes, TRelationships> : IFieldSelector<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        internal List<Field> Fields { get; }
        internal List<string> Includes { get; }

        internal FieldSelector(List<Field> fields, List<string> includes)
        {
            Fields = fields ?? new List<Field>();
            Includes = includes ?? new List<string>();
        }

        public IRequestBuilder<TAttributes, TRelationships> SelectFields(Expression<Func<TAttributes, object>> selector)
        {
            Fields.Add(selector == null ? Field.All<TAttributes>() : Field.Create<TAttributes>(selector));
            return new RequestBuilder<TAttributes, TRelationships>(Fields, Includes);
        }
    }

    internal class RequestBuilder<TAttributes, TRelationships> : FieldSelector<TAttributes, TRelationships>,
                                                                 IRequestBuilder<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        internal RequestBuilder(List<Field> fields, List<string> includes) : base(fields, includes) { }

        public IRequestBuilder<TAttributes, TRelationships, TRel> Include<TAttr, TRel>(
            Expression<Func<TRelationships, IPatreonResponse<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TRel : IRelationship
        {
            var path = HandleIncludesAndFields<TAttr>(relationshipSelector, fieldSelector);
            return new RequestBuilder<TAttributes, TRelationships, TRel>(path, Fields, Includes);
        }

        private string HandleIncludesAndFields<TAttr>(
            Expression relationshipSelector,
            Expression fieldSelector = null)
        {
            if (!(relationshipSelector is LambdaExpression lambda))
                throw new ArgumentException(nameof(relationshipSelector));

            if (!(lambda.Body is MemberExpression body))
                throw new ArgumentException(nameof(relationshipSelector));

            var attribute = (JsonAttr) body.Member.GetCustomAttribute(typeof(JsonAttr));

            if (Includes.Contains(attribute.Name)) return attribute.Name;

            Includes.Add(attribute.Name);
            Fields.Add(fieldSelector == null ? Field.All<TAttr>() : Field.Create<TAttr>(fieldSelector));

            return attribute.Name;
        }
    }

    internal class RequestBuilder<TAttributes, TOrigin, TNext> : RequestBuilder<TAttributes, TOrigin>,
                                                                 IRequestBuilder<TAttributes, TOrigin, TNext>
        where TOrigin : IRelationship
        where TNext : IRelationship
    {
        private readonly string _path;

        internal RequestBuilder(string path, List<Field> fields, List<string> includes) : base(fields, includes)
        {
            _path = path;
        }

        public IRequestBuilder<TAttributes, TOrigin, TRel> ThenInclude<TAttr, TRel>(
            Expression<Func<TNext, IPatreonResponse<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TRel : IRelationship
        {
            var path = HandleIncludesAndFields<TAttr>(relationshipSelector, fieldSelector);
            return new RequestBuilder<TAttributes, TOrigin, TRel>(path, Fields, Includes);
        }

        private string HandleIncludesAndFields<TAttr>(
            Expression relationshipSelector,
            Expression fieldSelector = null)
        {
            if (!(relationshipSelector is LambdaExpression lambda))
                throw new ArgumentException(nameof(relationshipSelector));

            if (!(lambda.Body is MemberExpression body))
                throw new ArgumentException(nameof(relationshipSelector));

            var attribute = (JsonAttr) body.Member.GetCustomAttribute(typeof(JsonAttr));

            var path = string.Concat(_path, ".", attribute.Name);
            if (Includes.Contains(path)) return path;

            Includes.Add(path);
            Fields.Add(fieldSelector == null ? Field.All<TAttr>() : Field.Create<TAttr>(fieldSelector));

            return path;
        }
    }
}