using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Services.ResponseDto
{
    public class CustomerDto
    {
        public required string Id { get; set; }
        public required string CompanyName { get; set; }
        public required ContactDto ContactInfo { get; set; }
        public required AddressDto Address { get; set; }

        [SetsRequiredMembers]
        public CustomerDto()
        {
            Id = "";
            CompanyName = "";
            ContactInfo = new ContactDto();
            Address = new AddressDto();
        }
    }
}
