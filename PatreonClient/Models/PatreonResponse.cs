namespace PatreonClient.Models
{
    public class PatreonResponse<T> : PatreonResponseBase<T>
    {
        public PatreonData<T> Data { get; set; }
        public Links Links { get; set; }
    }
}