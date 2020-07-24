# Patreon Client

This Patreon client is an experimental client, written for .NET Standard.

## How to use
Currently, you need to build the code yourself, and you need to pass in some parameters from your [Patreon v2 Client](https://www.patreon.com/portal/registration/register-clients), these values you want are "Creator's Access Token". An example of this is:

```csharp

var baseClient = new HttpClient{BaseAddress =  new Uri("https://www.patreon.com")};

baseClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Creator's Access Token");

var client = new PatreonHttpClient(baseClient, null );

var request = RequestBuilder.Identity.SelectFields().Include(e => e.Campaign).Build();

var res = await client.GetAsync(request);

```

## This is all confusing! What does any of this mean?

Your best bet will be to watch the YouTube series on this:

- [Playlist](https://www.youtube.com/playlist?list=PLOeFnOV9YBa50wIC-UAlHSrl-3HSlecGx)
- [Introduction](https://youtu.be/T9ai1-SE2ls)
- [Demo](https://youtu.be/boKPQ0Dq-LM)
- Project Overview on:
  - [Models](https://youtu.be/pWjIQaWPpDI)
  - [Requests](https://youtu.be/XPR5RRv-lU8)
  - [Request Builder](https://youtu.be/gZeST4o0osE)
  - [Response Binding](https://youtu.be/HFGAgalti-E)
- [Tasks Overview](https://youtu.be/FdzjmgSRse4)
- [Testing and Contributing](https://youtu.be/lJhPvJLlGFU)

# License

[WTFPL](LICENSE)