namespace NorthWindAPI.Services.ResponseDto
{
    public class CategoryHeatmapDto
    {
        /// <summary>
        /// Numeric month of the year
        /// </summary>
        /// <example>1</example>
        public int Month { get; set; }
        /// <summary>
        /// Number of sales for beverages
        /// </summary>
        /// <example>1</example>
        public int Beverages { get; set; }
        /// <summary>
        /// Number of sales for condiments
        /// </summary>
        /// <example>2</example>
        public int Condiments { get; set; }
        /// <summary>
        /// Number of sales for confections
        /// </summary>
        /// <example>3</example>
        public int Confections { get; set; }
        /// <summary>
        /// Number of sales for dairy
        /// </summary>
        /// <example>4</example>
        public int Dairy { get; set; }
        /// <summary>
        /// Number of sales for grains
        /// </summary>
        /// <example>5</example>
        public int Grains { get; set; }
        /// <summary>
        /// Number of sales for meat
        /// </summary>
        /// <example>6</example>
        public int Meat { get; set; }
        /// <summary>
        /// Number of sales for produce
        /// </summary>
        /// <example>7</example>
        public int Produce { get; set; }
        /// <summary>
        /// Number of sales for seafood
        /// </summary>
        /// <example>8</example>
        public int Seafood { get; set; }
    }
}
