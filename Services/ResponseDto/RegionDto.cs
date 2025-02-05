namespace NorthWindAPI.Services.ResponseDto
{
    public class RegionDto
    {
        /// <summary>
        /// Global region
        /// </summary>
        /// <example>North America</example>
        public string Region { get; set; }
        /// <summary>
        /// Country name
        /// </summary>
        /// <example>USA</example>
        public string Country { get; set; }
    }
}
