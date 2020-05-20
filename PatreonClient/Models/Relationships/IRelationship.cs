namespace PatreonClient.Models.Relationships
{
    public interface IRelationship
    {
        bool AssignRelationship(string id, string type, string json);
    }
}