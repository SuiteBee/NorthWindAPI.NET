namespace NorthWindAPI.Services.Models
{
    public class Contact
    {
        public required string ContactName { get; set; }
        public required string ContactTitle { get; set; }
        public string? CourtestyTitle { get; set; }
        public required string Phone { get; set; }
        public string? Fax { get; set; }
    }
}
