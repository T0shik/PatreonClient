namespace PatreonClient.Models.Relationships
{
    public class EmptyRelationship : IRelationship
    {
        public bool AssignRelationship(string id, string type, string json)
        {
            return true;
        }
    }
}