using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.Resources
{
    public class Order : Entity
    {
        public required string CustomerId { get; set; }
        public required int EmployeeId { get; set; }
        public required string OrderDate { get; set; }
        public string? ShippedDate { get; set; }
        public int ShipVia { get; set; }
        public decimal Freight { get; set; }
        public required string ShipName { get; set; }
        public required string ShipAddress { get; set; }
        public required string ShipCity { get; set; }
        public required string ShipRegion { get; set; }
        public required string ShipPostalCode { get; set; }
        public required string ShipCountry { get; set; }
    }
}
