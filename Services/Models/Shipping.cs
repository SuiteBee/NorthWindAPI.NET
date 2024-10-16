namespace NorthWindAPI.Services.Models
{
    public class Shipping
    {
        public string? ShippedDate { get; set; }
        public required string ShipCarrier { get; set; }
        public decimal ShipCost { get; set; }
        public required string ShipName { get; set; }
        public required Address Address { get; set; }

    }
}
