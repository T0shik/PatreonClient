using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PatreonClient;

public static class PatreonHttpClientExtensions
{
    public static Task<Tokens> RefreshAccessTokenAsync(
        this HttpClient client,
        string clientId,
        string clientSecret,
        string refreshToken
    )
    {
        var uri = "https://www.patreon.com/api/oauth2/token" +
                  "?grant_type=refresh_token" +
                  $"&refresh_token={refreshToken}" +
                  $"&client_id={clientId}" +
                  $"&client_secret={clientSecret}";

        return RequestTokens(client, uri);
    }

    public static Task<Tokens> GenerateAccessTokenAsync(
        this HttpClient client,
        string clientId,
        string clientSecret,
        string oneTimeCode
    )
    {
        var uri = "https://www.patreon.com/api/oauth2/token" +
                  "?grant_type=authorization_code" +
                  $"&code={oneTimeCode}" +
                  $"&client_id={clientId}" +
                  $"&client_secret={clientSecret}";

        return RequestTokens(client, uri);
    }

    private static async Task<Tokens> RequestTokens(HttpClient client, string uri)
    {
        var responseMessage = await client.PostAsync(uri, null);
        if (!responseMessage.IsSuccessStatusCode) throw new FailedToRefreshAccessToken();
        
        var str = await responseMessage.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Tokens>(str);
    }
}

public class Tokens
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; }
    [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; }
    [JsonPropertyName("scope")] public string Scope { get; set; }
    [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
    [JsonPropertyName("token_type")] public string TokenType { get; set; }

    public override string ToString()
    {
        return "AccessTokenResponse{{\n" +
               $"AccessToken:{AccessToken}" +
               $"RefreshToken:{RefreshToken}" +
               $"Scope:{Scope}" +
               $"ExpiresIn:{ExpiresIn}" +
               $"TokenType:{TokenType}" +
               "\n}}";
    }
}