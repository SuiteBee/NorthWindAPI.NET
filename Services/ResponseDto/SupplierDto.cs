using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Services.ResponseDto
{
    public class SupplierDto
    {
        public required string Company { get; set; }
        public required ContactDto ContactInfo { get; set; } = new ContactDto();
        public required AddressDto Address { get; set; } = new AddressDto();

        [SetsRequiredMembers]
        public SupplierDto()
        {
            Company = "";
        }
    }
}
