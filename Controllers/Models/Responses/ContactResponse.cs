using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Controllers.Models.Responses
{
    public class ContactResponse
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
        /// Optional honorific
        /// </summary>
        /// <example>Mr.</example>
        public string CourtestyTitle { get; set; }
        /// <summary>
        /// Phone number
        /// </summary>
        /// <example>123-456-7890</example>
        public required string Phone { get; set; }
        /// <summary>
        /// Optional fax number
        /// </summary>
        /// <example>123-456-7890</example>
        public string Fax { get; set; }
        /// <summary>
        /// Optional web address
        /// </summary>
        /// <example>www.companywebsite.com</example>
        public string Website { get; set; }

        [SetsRequiredMembers]
        public ContactResponse()
        {
            ContactName = "";
            ContactTitle = "";
            CourtestyTitle = "";
            Phone = "";
            Fax = "";
            Website = "";
        }
    }
}
