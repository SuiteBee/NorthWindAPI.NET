namespace NorthWindAPI.Controllers.Models.Requests
{
    public class ContactRequest
    {
        /// <summary>
        /// Company rep contact name
        /// </summary>
        /// <example>Ryan Bee</example>
        public required string ContactName { get; set; }
        /// <summary>
        /// Company rep position title
        /// </summary>
        /// <example>Supreme Overlord</example>
        public required string ContactTitle { get; set; }
        /// <summary>
        /// Phone number
        /// </summary>
        /// <example>123-456-7890</example>
        public required string Phone { get; set; }
        /// <summary>
        /// Optional fax number
        /// </summary>
        /// <example>123-456-7890</example>
        public string? Fax { get; set; }
    }
}
