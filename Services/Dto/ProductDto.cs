namespace NorthWindAPI.Services.Dto
{
    public class ProductDto
    {
        public string? ProductName { get; set; }
        public required string CategoryName { get; set; }
        public required string CategoryDescription { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemsRemaining { get; set; }
    }
}
