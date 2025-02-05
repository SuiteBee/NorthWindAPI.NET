using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Controllers.Models.Responses
{
    public class AddressResponse
    {
        /// <summary>
        /// Street name and number
        /// </summary>
        /// <example>123 Test St.</example>
        public required string Street { get; set; }
        /// <summary>
        /// City name
        /// </summary>
        /// <example>Cleveland</example>
        public required string City { get; set; }
        /// <summary>
        /// Zip code
        /// </summary>
        /// <example>44144</example>
        public required string PostalCode { get; set; }
        /// <summary>
        /// Country name
        /// </summary>
        /// <example>USA</example>
        public required string Country { get; set; }
        /// <summary>
        /// Global region
        /// </summary>
        /// <example>North America</example>
        public required string Region { get; set; }

        [SetsRequiredMembers]
        public AddressResponse()
        {
            Street = "";
            City = "";
            PostalCode = "";
            Country = "";
            Region = "";
        }
    }
}
