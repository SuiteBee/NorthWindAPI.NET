namespace NorthWindAPI.Data.Resources
{
    public class Order : Entity
    {
        public string? CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public string? OrderDate { get; set; }
        public string? ShippedDate { get; set; }
        public int ShipVia { get; set; }
        public decimal Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }
    }
}
