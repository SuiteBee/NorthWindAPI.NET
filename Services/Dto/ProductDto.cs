using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Services.Dto
{
    public class ProductDto
    {
        public required string ProductName { get; set; }
        public required string CategoryName { get; set; }
        public decimal ItemPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public decimal FinalPrice { get; set; }

        [SetsRequiredMembers]
        public ProductDto()
        {
            ProductName = "";
            CategoryName = "";
            ItemPrice = 0M;
            Quantity = 0;
            Discount = .00;
            FinalPrice = 0M;
        }

    }
}
