namespace NorthWindAPI.Services.Dto
{
    public class ShippingDto
    {
        public string? ShippedDate { get; set; }
        public required string ShipCarrier { get; set; }
        public decimal ShipCost { get; set; }
        public required string ShipName { get; set; }
        public required AddressDto Address { get; set; }

    }
}
