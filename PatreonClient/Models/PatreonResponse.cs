using PatreonClient.Models.Attributes;

namespace PatreonClient.Models
{
    public class PatreonResponse<T>
    {
        public PatreonData<T> Data { get; set; }
        public Links Links { get; set; }
    }
}