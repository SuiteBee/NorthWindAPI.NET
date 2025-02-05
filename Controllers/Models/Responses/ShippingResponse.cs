namespace NorthWindAPI.Controllers.Models.Responses
{
    public class ShippingResponse
    {
        /// <summary>
        /// Unique identifier for shipping carrier
        /// </summary>
        /// <example>1</example>
        public int CarrierId { get; set; }
        /// <summary>
        /// Date product marked as shipped
        /// </summary>
        /// <example>2024-01-01</example>
        public required string ShippedDate { get; set; }
        /// <summary>
        /// Shipping carrier name
        /// </summary>
        /// <example>Speedy Express</example>
        public required string ShipCarrier { get; set; }
        /// <summary>
        /// Shipping cost
        /// </summary>
        /// <example>99.99</example>
        public decimal ShipCost { get; set; }
        /// <summary>
        /// Shipment addressed to
        /// </summary>
        /// <example>Vins et alcools Chevalier</example>
        public required string ShipName { get; set; }
        public required AddressResponse Address { get; set; }
    }
}
