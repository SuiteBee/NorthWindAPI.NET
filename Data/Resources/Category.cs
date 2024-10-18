namespace NorthWindAPI.Data.Resources
{
    public class Category : Entity
    {
        public required string CategoryName { get; set; }
        public required string Description { get; set; }
    }
}
