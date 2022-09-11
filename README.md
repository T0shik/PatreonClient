# Non-Official C# Patreon Client


### Quick Start

```csharp
// create client
var patreonClient = new Patreon("api-key");

// build request - save for later
var request = PatreonRequestBuilder.Identity(b => b.SelectFields());

// execute request
var response = await patreonClient.GetAsync(request);
```
### Requests
```csharp
public static class PatreonRequestBuilder
{
    // get identity
    PatreonRequestBuilder.Identity(builder => ...);
    // get campaign by id
    PatreonRequestBuilder.Campaign(builder => ...);
    // get campaigns
    PatreonRequestBuilder.Campaigns(builder => ...);
    // get member by id
    PatreonRequestBuilder.Member(builder => ...);
    // get campaign members by campaign id
    PatreonRequestBuilder.CampaignMembers(builder => ...);
    // get post by id
    PatreonRequestBuilder.Post(builder => ...);
    // get posts by campaign id
    PatreonRequestBuilder.CampaignPosts(builder => ...);
}
```


### Parameters
```csharp
var request = PatreonRequestBuilder.CampaignMembers(b => b.SelectFields());

var response = await patreonClient.GetAsync(request.For("campaign_id"));
```

### Selecting Specific Fields
```csharp
var request = PatreonRequestBuilder.CampaignMembers(
    b => b.SelectFields(x => new
    {
        x.FullName,
        x.LastChargeDate,
        x.LifetimeSupportCents
    })
);
```

### Including Related Data

```csharp
var campaignMembersRequest = PatreonRequestBuilder.CampaignMembers(
    b => b.SelectFields()
          .Include(x => x.Tiers)
);
```

### Related Data Specific Fields

```csharp
var campaignMembersRequest = PatreonRequestBuilder.CampaignMembers(
    b => b.SelectFields()
          .Include(x => x.Tiers, x => new
          {
              x.Title
          })
);
```

### Nested Related Data

```csharp
var campaignMembersRequest = PatreonRequestBuilder.CampaignMembers(
    b => b.SelectFields()
          .Include(x => x.Tiers, x => new
          {
              x.Title
          })
          .ThenInclude(x => x.Benefits)
);
```