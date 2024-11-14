using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Services.ResponseDto
{
    public class ShippingDto
    {
        public int Id { get; set; }
        public required string ShippedDate { get; set; }
        public required string ShipCarrier { get; set; }
        public decimal ShipCost { get; set; }
        public required string ShipName { get; set; }
        public required AddressDto Address { get; set; }

        [SetsRequiredMembers]
        public ShippingDto()
        {
            ShippedDate = "";
            ShipCarrier = "";
            ShipCost = 0;
            ShipName = "";
            Address = new AddressDto();
        }

    }
}
