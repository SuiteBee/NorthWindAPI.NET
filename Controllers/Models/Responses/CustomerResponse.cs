using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Controllers.Models.Responses
{
    public class CustomerResponse
    {
        /// <summary>
        /// Unique string identifier
        /// </summary>
        /// <example>CUST</example>
        public required string Id { get; set; }
        /// <summary>
        /// Company full name
        /// </summary>
        /// <example>The Very Best Bakery</example>
        public required string CompanyName { get; set; }
        /// <summary>
        /// Company contact info
        /// </summary>
        public required ContactResponse ContactInfo { get; set; }
        /// <summary>
        /// Company location address
        /// </summary>
        public required AddressResponse Address { get; set; }

        [SetsRequiredMembers]
        public CustomerResponse()
        {
            Id = "";
            CompanyName = "";
            ContactInfo = new ContactResponse();
            Address = new AddressResponse();
        }
    }
}
