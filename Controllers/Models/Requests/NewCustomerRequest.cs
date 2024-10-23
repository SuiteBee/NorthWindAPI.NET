namespace NorthWindAPI.Controllers.Models.Requests
{
    public class NewCustomerRequest
    {
        public required string CompanyIdentifier { get; set; }
        public required string CompanyName { get; set; }
        public required ContactRequest ContactInfo { get; set; }
        public required AddressRequest Address { get; set; }
    }
}
