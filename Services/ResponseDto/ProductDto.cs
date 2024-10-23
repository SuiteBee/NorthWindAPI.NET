using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Services.ResponseDto
{
    public class ProductDto
    {
        public required string ProductName { get; set; }
        public required string CategoryName { get; set; }
        public decimal PurchasePrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public double Discount { get; set; }
        public decimal FinalPrice { get; set; }

        [SetsRequiredMembers]
        public ProductDto()
        {
            ProductName = "";
            CategoryName = "";
            PurchasePrice = 0M;
            Quantity = 0;
            TotalPrice = 0M;
            Discount = .00;
            FinalPrice = 0M;
        }

    }
}
