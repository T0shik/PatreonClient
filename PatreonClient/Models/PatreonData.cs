namespace PatreonClient.Models
{
    public class PatreonData<T>
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public T Attributes { get; set; }
    }
}