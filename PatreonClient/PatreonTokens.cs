using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PatreonClient;

public interface IPatreonTokens
{
    string AccessToken { get; }
    string RefreshToken { get; }
    Task RefreshTokens();
}

public abstract class PatreonTokens : IPatreonTokens
{
    private readonly HttpClient _client;
    private readonly PatreonClientConfig _config;
    private readonly SemaphoreSlim _sync;

    public PatreonTokens(
        HttpClient client,
        PatreonClientConfig config
    )
    {
        _client = client;
        _config = config;
        _sync = new SemaphoreSlim(1);
    }

    protected abstract Tokens GetTokens();
    protected abstract Task SaveTokensAsync(Tokens response);

    public string AccessToken => GetTokens().AccessToken;
    public string RefreshToken => GetTokens().RefreshToken;

    public virtual async Task RefreshTokens()
    {
        var refreshToken = GetTokens().RefreshToken;
        var available = await _sync.WaitAsync(0);

        if (available)
        {
            try
            {
                var tokens = await _client.RefreshAccessTokenAsync(_config.ClientId, _config.ClientSecret, refreshToken);
                await SaveTokensAsync(tokens);
            }
            finally
            {
                _sync.Release();
            }
        }
        else
        {
            await _sync.WaitAsync();
            _sync.Release();
        }
    }
}

/// <summary>
/// Access Token Only. WARNING: USE ONLY FOR TESTING.
/// </summary>
public class AccessTokenOnly : PatreonTokens
{
    public AccessTokenOnly(string accessToken) : base(null, null)
    {
        _tokens = new Tokens() { AccessToken = accessToken };
    }

    private readonly Tokens _tokens;

    protected override Tokens GetTokens() => _tokens;

    public override Task RefreshTokens()
    {
        throw new NotImplementedException("refreshing tokens not supported with AccessTokenOnly strategy");
    }

    protected override Task SaveTokensAsync(Tokens response)
    {
        throw new NotImplementedException("saving tokens not supported with AccessTokenOnly strategy");
    }
}

/// <summary>
/// Store & refresh tokens in memory. WARNING: USE ONLY FOR TESTING.
/// </summary>
public class InMemoryPatreonTokens : PatreonTokens
{
    public InMemoryPatreonTokens(
        HttpClient client,
        PatreonClientConfig config,
        Tokens tokens
    ) : base(client, config)
    {
        _tokens = tokens;
    }

    private Tokens _tokens;

    protected override Tokens GetTokens() => _tokens;

    protected override Task SaveTokensAsync(Tokens response)
    {
        _tokens = response;
        return Task.CompletedTask;
    }
}