namespace NorthWindAPI.Services.ResponseDto
{
    public class TotalsDto
    {
        /// <summary>
        /// Global order count
        /// </summary>
        /// <example>999</example>
        public int NumOrders { get; set; }
        /// <summary>
        /// Global revenue total
        /// </summary>
        /// <example>999999.99</example>
        public decimal RevenueTotal { get; set; }
        /// <summary>
        /// Global number of customers
        /// </summary>
        /// <example>999</example>
        public int NumClients { get; set; }
        /// <summary>
        /// Global number of available products
        /// </summary>
        /// <example>111</example>
        public int NumProducts { get; set; }
        /// <summary>
        /// Global number of products sold
        /// </summary>
        /// <example>999</example>
        public int ProductsSold { get; set; }
        /// <summary>
        /// Global number of countries with customers
        /// </summary>
        /// <example>9</example>
        public int Countries { get; set; }
    }
}
