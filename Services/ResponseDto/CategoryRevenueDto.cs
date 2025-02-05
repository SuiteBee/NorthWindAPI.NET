namespace NorthWindAPI.Services.ResponseDto
{
    public class CategoryRevenueDto
    {
        /// <summary>
        /// Revenue year
        /// </summary>
        /// <example>2024</example>
        public int Year { get; set; }
        /// <summary>
        /// Revenue total for beverages
        /// </summary>
        /// <example>999.99</example>
        public decimal Beverages { get; set; }
        /// <summary>
        /// Revenue total for condiments
        /// </summary>
        /// <example>999.99</example>
        public decimal Condiments { get; set; }
        /// <summary>
        /// Revenue total for confections
        /// </summary>
        /// <example>999.99</example>
        public decimal Confections { get; set; }
        /// <summary>
        /// Revenue total for dairy
        /// </summary>
        /// <example>999.99</example>
        public decimal Dairy { get; set; }
        /// <summary>
        /// Revenue total for grains
        /// </summary>
        /// <example>999.99</example>
        public decimal Grains { get; set; }
        /// <summary>
        /// Revenue total for meat
        /// </summary>
        /// <example>999.99</example>
        public decimal Meat { get; set; }
        /// <summary>
        /// Revenue total for produce
        /// </summary>
        /// <example>999.99</example>
        public decimal Produce { get; set; }
        /// <summary>
        /// Revenue total for seafood
        /// </summary>
        /// <example>999.99</example>
        public decimal Seafood { get; set; }
    }
}
