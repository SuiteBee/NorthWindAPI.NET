namespace NorthWindAPI.Services.Models
{
    public class Customer
    {
        public required string Id { get; set; }
        public required string CompanyName { get; set; }
        public required Contact ContactInfo { get; set; }
    }
}
