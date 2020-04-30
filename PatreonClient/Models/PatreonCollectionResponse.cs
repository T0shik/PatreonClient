using System.Collections.Generic;

namespace PatreonClient.Models
{
    public class PatreonCollectionResponse<T> : PatreonResponseBase<T>
    {
        public IEnumerable<PatreonData<T>> Data { get; set; }
        public Meta Meta { get; set; }
    }
}