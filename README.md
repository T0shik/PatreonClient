# Non-Official C# Patreon Client


## Quick Start

```csharp
// create client for testing
var patreonClient = new Patreon(new(), new AccessTokenOnly("personal_access_token"));

// build request - save for later
var request = PatreonRequestBuilder.Identity(b => b.SelectFields());

// execute request
var response = await patreonClient.GetAsync(request);
```

## HttpClient for Production
Patreon doesn't allow client credentials flow and there are no API keys, so the client comes with token management in mind.

### Implementation Example

```csharp
public class EFCorePatreonTokens : PatreonTokens, IPatreonTokens
{
    public EFCorePatreonTokens(
        HttpClient client,
        PatreonClientConfig config,
        MyDbContext ctx
    ) : base(client, config)
    {
        _ctx = ctx;
    }
    
    protected override Task<Tokens> GetTokens() {
        // your reading tokens logic
    }

    protected override Task SaveTokensAsync(Tokens response)
    {
        // your saving token logic
    }
}

var patreon = new Patreon(new(), new EFCorePatreonTokens(...));
```

### Example with Marten

```csharp
public class PatreonTokensInMarten : PatreonTokens
{
    private readonly Settings _settings;
    private readonly IDocumentSession _session;

    public PatreonTokensInMarten(
        HttpClient client,
        PatreonClientConfig config,
        Settings settings,
        IDocumentSession session
    ) : base(client, config)
    {
        _settings = settings;
        _session = session;
    }

    // todo: replace me with GetUserId in current scope service
    private const string PersonalTokens = nameof(PersonalTokens);

    private Tokens? Tokens { get; set; }

    public override async Task<Tokens> GetTokens()
    {
        // try load from db
        if (Tokens == null)
        {
            Tokens = await _session.LoadAsync<TokensDoc>(PersonalTokens);
        }

        // try load from appsettings
        if (Tokens == null)
        {
            Tokens = new TokensDoc()
            {
                AccessToken = _settings.PatreonAccessToken,
                RefreshToken = _settings.PatreonRefreshToken,
            };
        }

        return Tokens;
    }

    protected override async Task SaveTokensAsync(Tokens response)
    {
        var doc = new TokensDoc()
        {
            Id = PersonalTokens,
            AccessToken = response.AccessToken,
            RefreshToken = response.RefreshToken,
            ExpiresIn = response.ExpiresIn,
            Scope = response.Scope,
            TokenType = response.TokenType,
        };
        Tokens = doc;
        _session.Store(doc);
        await _session.SaveChangesAsync();
    }
}

public class TokensDoc : Tokens
{
    public string Id { get; set; }
}
```

## How to use

### Requests
```csharp
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

## Authenticating a Patron

This example requires that the user is first directed to the following URL 
(usually through a "Log in with Patreon" button).
```
https://www.patreon.com/oauth2/authorize?response_type=code&client_id={CLIENT_ID}&redirect_uri={REDIRECT_URI}
```
Where CLIENT_ID is the ID of the creator's client app and REDIRECT_URI is the URI Patreon will call once the user 
is authenticated.  
See https://docs.patreon.com/#step-1-registering-your-client for more information.

The `code` can then be extracted from the query string of the redirect URI and processed as follows to generate
the usual `Tokens` object used in other examples. This token will have fewer permissions than your creator token,
so it should only be used for actions specific to this user.

```csharp
using var httpClient = new HttpClient();
var tokens = httpClient.GenerateAccessTokenAsync(clientId, clientSecret, code);
// Store the tokens in a way that your PatreonTokens implementation will read from
```