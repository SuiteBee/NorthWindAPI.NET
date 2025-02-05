namespace NorthWindAPI.Controllers.Models.Requests
{
    public class OrderDetailRequest
    {
        /// <summary>
        /// Unique product identifier
        /// </summary>
        /// <example>1</example>
        public required int ProductId { get; set; }
        /// <summary>
        /// Quantity of product ordered
        /// </summary>
        /// <example>12</example>
        public required int Quantity { get; set; }
        /// <summary>
        /// Discount percentage to apply
        /// </summary>
        /// <example>0.05</example>
        public required decimal Discount { get; set; }

    }
}
