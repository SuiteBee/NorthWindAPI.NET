namespace NorthWindAPI.Controllers.Models.Requests
{
    public class StockRequest
    {
        /// <summary>
        /// Quantity of product left in stock
        /// </summary>
        /// <example>99</example>
        public int StockAmt { get; set; }
    }
}
