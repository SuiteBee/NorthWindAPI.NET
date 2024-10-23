namespace NorthWindAPI.Controllers.Models.Requests
{
    public class ContactRequest
    {
        public required string ContactName { get; set; }
        public required string ContactTitle { get; set; }
        public required string Phone { get; set; }
        public string? Fax { get; set; }
    }
}
