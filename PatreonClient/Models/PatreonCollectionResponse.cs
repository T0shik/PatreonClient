using System.Collections.Generic;
using PatreonClient.Models.Attributes;

namespace PatreonClient.Models
{
    public class PatreonCollectionResponse<T>
    {
        public IEnumerable<PatreonData<T>> Data { get; set; }
        public Meta Meta { get; set; }
    }
}