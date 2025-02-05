namespace NorthWindAPI.Controllers.Models.Requests
{
    public class CustomerRequest
    {
        /// <summary>
        /// Unique string identifier
        /// </summary>
        /// <example>RBEE</example>
        public required string CompanyIdentifier { get; set; }
        /// <summary>
        /// Company full name
        /// </summary>
        /// <example>The Very Best Bakery</example>
        public required string CompanyName { get; set; }
        /// <summary>
        /// Company contact info
        /// </summary>
        public required ContactRequest ContactInfo { get; set; }
        /// <summary>
        /// Company location address
        /// </summary>
        public required AddressRequest Address { get; set; }
    }
}
