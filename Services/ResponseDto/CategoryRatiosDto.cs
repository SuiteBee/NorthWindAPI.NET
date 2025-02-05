namespace NorthWindAPI.Services.ResponseDto
{
    public class CategoryRatiosDto
    {
        /// <summary>
        /// Product category
        /// </summary>
        /// <example>Meat/Poultry</example>
        public string? Category { get; set; }
        /// <summary>
        /// Percentage of total revenue for category
        /// </summary>
        /// <example>13</example>
        public decimal Percentage { get; set; }
    }
}
