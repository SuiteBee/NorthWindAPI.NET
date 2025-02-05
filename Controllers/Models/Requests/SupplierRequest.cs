namespace NorthWindAPI.Controllers.Models.Requests
{
    public class SupplierRequest
    {
        /// <summary>
        /// Distributor Name
        /// </summary>
        /// <example>Exotic Liquids</example>
        public string Company { get; set; }
        public ContactRequest ContactInfo { get; set; }
        public AddressRequest Address { get; set; }
    }
}
