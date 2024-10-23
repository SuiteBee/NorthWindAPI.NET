using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Services.ResponseDto
{
    public class AddressDto
    {
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string PostalCode { get; set; }
        public required string Country { get; set; }
        public required string Region { get; set; }

        [SetsRequiredMembers]
        public AddressDto()
        {
            Street = "";
            City = "";
            PostalCode = "";
            Country = "";
            Region = "";
        }
    }
}
