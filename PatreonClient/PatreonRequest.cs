using PatreonClient.Models;

namespace PatreonClient
{
    public interface IPatreonRequest<TResponse, TAttribute, TRelationship>
        where TResponse : PatreonResponseBase<TAttribute, TRelationship>
        where TRelationship : IRelationship
    {
        string Url { get; }
    }
    
    public class PatreonRequest<TResponse, TAttribute, TRelationship> 
        : IPatreonRequest<TResponse, TAttribute, TRelationship>
        where TResponse : PatreonResponseBase<TAttribute, TRelationship>
        where TRelationship : IRelationship
    {
        internal PatreonRequest(string url) => Url = url;
        public string Url { get; }
    }
    
    public class ParameterizedPatreonRequest<TResponse, TAttribute, TRelationship>
        : PatreonRequest<TResponse, TAttribute, TRelationship>
        where TResponse : PatreonResponseBase<TAttribute, TRelationship>
        where TRelationship : IRelationship
    {
        public ParameterizedPatreonRequest(string url) : base(url) { }
    }
    

}