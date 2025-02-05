namespace NorthWindAPI.Services.ResponseDto
{
    public class CarrierDto
    {
        /// <summary>
        /// Unique carrier identifier
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }
        /// <summary>
        /// Carrier name
        /// </summary>
        /// <example>Speedy Express</example>
        public string? CompanyName { get; set; }
        /// <summary>
        /// Carrier phone number
        /// </summary>
        /// <example>123-456-7890</example>
        public string? Phone { get; set; }
    }
}
