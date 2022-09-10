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