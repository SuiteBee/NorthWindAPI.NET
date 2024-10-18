using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Services.Dto
{
    public class CustomerDto
    {
        public required string Id { get; set; }
        public required string CompanyName { get; set; }
        public required ContactDto ContactInfo { get; set; }

        [SetsRequiredMembers]
        public CustomerDto()
        {
            Id = "";
            CompanyName = "";
            ContactInfo = new ContactDto();
        }
    }
}
