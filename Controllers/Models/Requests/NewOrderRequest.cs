namespace NorthWindAPI.Controllers.Models.Requests
{
    public class NewOrderRequest
    {
        public required string CustomerId { get; set; }
        public required int EmployeeId { get;set; }
        public required int CarrierId { get; set; }
        public required decimal ShipCost { get; set; }
        public required string ShipName { get; set; }
        public required AddressRequest Address { get; set; }
        public required IEnumerable<OrderDetailRequest> OrderDetail { get; set; }

    }
}
