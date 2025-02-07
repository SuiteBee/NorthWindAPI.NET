namespace NorthWindAPI.Controllers.Models.Requests
{
    public class ShipRequest
    {
        /// <summary>
        /// List of order ID's
        /// </summary>
        /// <example>[1,2,3]</example>
        public required List<int> OrderIds { get; set; }
        /// <summary>
        /// Date shipped
        /// </summary>
        /// <example>2024-01-01</example>
        public string? ShipDate { get; set; }
    }
}
