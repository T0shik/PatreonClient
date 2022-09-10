using System;
using PatreonClient.Models.Attributes;
using PatreonClient.Models.Relationships;
using PatreonClient.Requests;

namespace PatreonClient;

public static class PatreonRequestBuilder
{
    public static PatreonRequest<PatreonResponse<User, UserRelationships>> Identity(Action<IFieldSelector<User, UserRelationships>> action)
    {
        var builder = new RequestBuilder("/api/oauth2/v2/identity");
        action(new FieldSelector<User, UserRelationships>(builder));
        return new(builder.BuildUrl());
    }

    public static PatreonRequest<string, PatreonResponse<Campaign, CampaignRelationships>> Campaign(Action<IFieldSelector<Campaign, CampaignRelationships>> action)
    {
        var builder = new RequestBuilder("/api/oauth2/v2/campaigns/{0}");
        action(new FieldSelector<Campaign, CampaignRelationships>(builder));
        return new(builder.BuildUrl());
    }

    public static PatreonRequest<PatreonCollectionResponse<Campaign, CampaignRelationships>> Campaigns(Action<IFieldSelector<Campaign, CampaignRelationships>> action)
    {
        var builder = new RequestBuilder("/api/oauth2/v2/campaigns");
        action(new FieldSelector<Campaign, CampaignRelationships>(builder));
        return new(builder.BuildUrl());
    }

    public static PatreonRequest<string, PatreonResponse<Member, MemberRelationships>> Member(Action<IFieldSelector<Member, MemberRelationships>> action)
    {
        var builder = new RequestBuilder("/api/oauth2/v2/members/{0}");
        action(new FieldSelector<Member, MemberRelationships>(builder));
        return new(builder.BuildUrl());
    }

    public static PatreonRequest<string, PatreonCollectionResponse<Member, MemberRelationships>> CampaignMembers(Action<IFieldSelector<Member, MemberRelationships>> action)
    {
        var builder = new RequestBuilder("/api/oauth2/v2/campaigns/{0}/members");
        action(new FieldSelector<Member, MemberRelationships>(builder));
        return new(builder.BuildUrl());
    }

    public static PatreonRequest<string, PatreonResponse<Post, PostRelationships>> Post(Action<IFieldSelector<Post, PostRelationships>> action)
    {
        var builder = new RequestBuilder("/api/oauth2/v2/posts/{0}");
        action(new FieldSelector<Post, PostRelationships>(builder));
        return new(builder.BuildUrl());
    }

    public static PatreonRequest<string, PatreonCollectionResponse<Member, MemberRelationships>> CampaignPosts(Action<IFieldSelector<Member, MemberRelationships>> action)
    {
        var builder = new RequestBuilder("/api/oauth2/v2/campaigns/{0}/posts");
        action(new FieldSelector<Member, MemberRelationships>(builder));
        return new(builder.BuildUrl());
    }
}