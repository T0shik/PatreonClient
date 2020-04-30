using System.Collections.Generic;

namespace PatreonClient.Models
{
    public abstract class PatreonResponseBase<T>
    {
        public Dictionary<string, object> Includes { get; } = new Dictionary<string, object>();
    }
}