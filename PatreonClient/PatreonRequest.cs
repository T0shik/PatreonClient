namespace PatreonClient;

public class PatreonRequest<TResponse>
{
    internal PatreonRequest(string url) => Url = url;
    public string Url { get; }
}

public class PatreonRequest<TIn, TResponse>
{
    private string _url;
    internal PatreonRequest(string url) => _url = url;
    public PatreonRequest<TResponse> For(TIn parameter) => new(string.Format(_url, parameter));
}