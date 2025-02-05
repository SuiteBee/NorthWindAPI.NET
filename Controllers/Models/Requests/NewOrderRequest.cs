namespace NorthWindAPI.Controllers.Models.Requests
{
    public class NewOrderRequest
    {
        /// <summary>
        /// Unique customer identifier
        /// </summary>
        /// <example>RBEE</example>
        public required string CustomerId { get; set; }
        /// <summary>
        /// Unique employee identifier
        /// </summary>
        /// <example>1</example>
        public required int EmployeeId { get; set; }
        /// <summary>
        /// Unique shipping carrier identifier
        /// </summary>
        /// <example>1</example>
        public required int CarrierId { get; set; }
        /// <summary>
        /// Shipping cost
        /// </summary>
        /// <example>99.99</example>
        public required decimal ShipCost { get; set; }
        /// <summary>
        /// Shipping carrier name
        /// </summary>
        /// <example>Speedy Express</example>
        public required string ShipName { get; set; }
        public required AddressRequest Address { get; set; }
        public required IEnumerable<OrderDetailRequest> OrderDetail { get; set; }

    }
}
