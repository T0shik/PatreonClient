using System.Collections.Generic;
using System.Text;

namespace PatreonClient.Requests;

internal class RequestBuilder
{
    internal string BaseUrl { get; }
    internal List<Field> Fields { get; } = new();
    internal List<string> Includes { get; } = new();

    internal RequestBuilder(string baseUrl)
    {
        BaseUrl = baseUrl;
    }

    internal string BuildUrl()
    {
        var sb = new StringBuilder(BaseUrl);
        var hasInclude = Includes.Count > 0;
        var ampersand = false;
        if (hasInclude)
        {
            sb.Append("?include=");
            sb.AppendJoin(',', Includes);
            ampersand = true;
        }

        foreach (var field in Fields)
        {
            sb.Append(field.ToString(ampersand ? "&" : "?"));
            ampersand = true;
        }

        return sb.ToString();
    }
}