namespace NorthWindAPI.Controllers.Models.Requests
{
    public class PriceRequest
    {
        /// <summary>
        /// Product cost
        /// </summary>
        /// <example>18.50</example>
        public decimal ItemPrice { get; set; }
    }
}
