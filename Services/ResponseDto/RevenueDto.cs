namespace NorthWindAPI.Services.ResponseDto
{
    public class RevenueDto
    {
        /// <summary>
        /// Revenue year
        /// </summary>
        /// <example>2024</example>
        public string? Year { get; set; }
        /// <summary>
        /// Total revenue for the year
        /// </summary>
        /// <example>400.40</example>
        public decimal Total { get; set; }
        /// <summary>
        /// Total revenue for quarter one
        /// </summary>
        /// <example>100.10</example>
        public decimal QuarterOne { get; set; }
        /// <summary>
        /// Total revenue for quarter two
        /// </summary>
        /// <example>100.10</example>
        public decimal QuarterTwo { get; set; }
        /// <summary>
        /// Total revenue for quarter three
        /// </summary>
        /// <example>100.10</example>
        public decimal QuarterThree { get; set; }
        /// <summary>
        /// Total revenue for quarter four
        /// </summary>
        /// <example>100.10</example>
        public decimal QuarterFour { get; set; }
    }
}
