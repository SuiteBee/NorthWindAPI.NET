using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Services.ResponseDto
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required string CategoryName { get; set; }
        public required string CategoryDescription { get; set; }
        public decimal ItemPrice { get; set; }
        public int StockAmt { get; set; }
        public bool InStock { get; set; }
        public bool Discontinued { get; set; }
        public required SupplierDto SuppliedBy { get; set; } = new SupplierDto();

        [SetsRequiredMembers]
        public ProductDto()
        {
            ProductName = "";
            CategoryName = "";
            CategoryDescription = "";
        }
    }
}
