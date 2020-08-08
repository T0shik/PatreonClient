using System.Text.Json;

namespace PatreonClient
{
    public static class Settings
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
}