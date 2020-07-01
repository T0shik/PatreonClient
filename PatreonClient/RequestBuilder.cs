using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using PatreonClient.Models;
using PatreonClient.Models.Attributes;
using PatreonClient.Models.Relationships;
using PatreonClient.Requests;
using JsonAttr = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace PatreonClient
{
    public class RequestBuilder
    {
        public RequestBuilder() { }

        public static IFieldSelector<PatreonResponse<User, UserRelationships>, User, UserRelationships> Identity()
        {
            return new FieldSelector<PatreonResponse<User, UserRelationships>, User, UserRelationships>(
                null,
                null,
                "/api/oauth2/v2/identity",
                false);
        }

        // public static PatreonParameterizedRequest<PatreonResponse<Campaign, CampaignRelationships>, Campaign,
        //         CampaignRelationships>
        //     Campaign(Action<IFieldSelector<Campaign, CampaignRelationships>> builderAction)
        // {
        //     var builder = new FieldSelector<Campaign, CampaignRelationships>(null, null);
        //     builderAction(builder);
        //
        //     return new PatreonParameterizedRequest<PatreonResponse<Campaign, CampaignRelationships>, Campaign,
        //         CampaignRelationships>(
        //         "/api/oauth2/v2/campaigns/{0}",
        //         builder.Fields,
        //         builder.Includes);
        // }

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

    internal abstract class RequestBuilderBase<TResponse, TAttributes, TRelationships>
        : IRequestBuilderBase<TResponse, TAttributes, TRelationships>
        where TResponse : IPatreonResponse<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        internal bool WithParams { get; }
        internal string Url { get; }
        internal List<Field> Fields { get; }
        internal List<string> Includes { get; }

        internal RequestBuilderBase(List<Field> fields, List<string> includes, string url, bool withParams)
        {
            WithParams = withParams;
            Url = url;
            Fields = fields ?? new List<Field>();
            Includes = includes ?? new List<string>();
        }

        public IPatreonRequest<TResponse, TAttributes, TRelationships> Build()
        {
            var url = BuildUrl();
            return WithParams
                       ? new ParameterizedPatreonRequest<TResponse, TAttributes, TRelationships>(url)
                       : new PatreonRequest<TResponse, TAttributes, TRelationships>(url);
        }

        private string BuildUrl()
        {
            var result = "";
            var hasInclude = Includes.Count > 0;
            var ampersand = false;
            if (hasInclude)
            {
                result = string.Concat("?include=", string.Join(',', Includes));
                ampersand = true;
            }

            foreach (var field in Fields)
            {
                result = string.Concat(result, field.ToString(ampersand ? "&" : "?"));
                ampersand = true;
            }

            return string.Concat(Url, result);
        }
    }

    internal class FieldSelector<TResponse, TAttributes, TRelationships>
        : RequestBuilderBase<TResponse, TAttributes, TRelationships>,
          IFieldSelector<TResponse, TAttributes, TRelationships>
        where TResponse : IPatreonResponse<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        public FieldSelector(List<Field> fields, List<string> includes, string url, bool withParams)
            : base(fields, includes, url, withParams) { }

        public IRequestBuilder<TResponse, TAttributes, TRelationships> SelectFields(
            Expression<Func<TAttributes, object>> selector)
        {
            Fields.Add(selector == null ? Field.All<TAttributes>() : Field.Create<TAttributes>(selector));
            return new RequestBuilder<TResponse, TAttributes, TRelationships>(Fields, Includes, Url, WithParams);
        }
    }

    internal class RequestBuilder<TResponse, TAttributes, TRelationships>
        : FieldSelector<TResponse, TAttributes, TRelationships>,
          IRequestBuilder<TResponse, TAttributes, TRelationships>
        where TResponse : IPatreonResponse<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        public RequestBuilder(List<Field> fields, List<string> includes, string url, bool withParams)
            : base(fields, includes, url, withParams) { }

        public IRequestBuilder<TResponse, TAttributes, TRelationships, TRel> Include<TAttr, TRel>(
            Expression<Func<TRelationships, IPatreonResponse<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TRel : IRelationship
        {
            var path = HandleIncludesAndFields<TAttr>(relationshipSelector, fieldSelector);
            return new RequestBuilder<TResponse, TAttributes, TRelationships, TRel>(
                path,
                Fields,
                Includes,
                Url,
                WithParams);
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

    internal class RequestBuilder<TResponse, TAttributes, TOrigin, TNext>
        : RequestBuilder<TResponse, TAttributes, TOrigin>,
          IRequestBuilder<TResponse, TAttributes, TOrigin, TNext>
        where TResponse : IPatreonResponse<TAttributes, TOrigin>
        where TOrigin : IRelationship
        where TNext : IRelationship
    {
        private readonly string _path;

        public RequestBuilder(
            string path,
            List<Field> fields,
            List<string> includes,
            string url,
            bool withParams)
            : base(fields, includes, url, withParams)
        {
            _path = path;
        }

        public IRequestBuilder<TResponse, TAttributes, TOrigin, TRel> ThenInclude<TAttr, TRel>(
            Expression<Func<TNext, IPatreonResponse<TAttr, TRel>>> relationshipSelector,
            Expression<Func<TAttr, object>> fieldSelector = null)
            where TRel : IRelationship
        {
            var path = HandleIncludesAndFields<TAttr>(relationshipSelector, fieldSelector);
            return new RequestBuilder<TResponse, TAttributes, TOrigin, TRel>(path, Fields, Includes, Url, WithParams);
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