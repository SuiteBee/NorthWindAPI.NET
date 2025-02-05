using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers.Models.Responses
{
    public class SupplierResponse
    {
        /// <summary>
        /// Distributor Name
        /// </summary>
        /// <example>Exotic Liquids</example>
        public required string Company { get; set; }
        public required ContactResponse ContactInfo { get; set; }
        public required AddressResponse Address { get; set; }
    }
}
