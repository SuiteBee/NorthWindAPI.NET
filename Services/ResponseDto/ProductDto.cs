using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Services.ResponseDto
{
    public class ProductDto
    {
        public required string ProductName { get; set; }
        public required string CategoryName { get; set; }
        public required string CategoryDescription { get; set; }
        public decimal ItemPrice { get; set; }
        public required SupplierDto Supplier { get; set; } = new SupplierDto();

        [SetsRequiredMembers]
        public ProductDto()
        {
            ProductName = "";
            CategoryName = "";
            CategoryDescription = "";
            ItemPrice = 0M;
        }
    }
}
