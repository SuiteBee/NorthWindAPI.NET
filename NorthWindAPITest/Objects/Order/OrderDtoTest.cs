using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPITest.Objects.Order
{
    public class OrderDtoTest
    {
        /// <summary>
        /// Order total and subtotal = 0
        /// </summary>
        public static OrderDto FullOrderNoTotals = new OrderDto
        {
            OrderId = 12345,
            OrderDate = "2025-01-01",
            OrderSubtotal = 0,
            OrderTotal = 0,
            Items = new List<OrderItemDto> {
                new OrderItemDto
                {
                    ProductName = "Jack's New England Clam Chowder",
                    CategoryName = "Seafood",
                    ItemPrice = 9.65M,
                    PurchasePrice = 7.7M,
                    Quantity = 10,
                    TotalPrice = 96.50M,
                    Discount = 0,
                    DiscountAmt = 0,
                    FinalPrice = 96.50M
                },
                new OrderItemDto
                {
                    ProductName = "Manjimup Dried Apples",
                    CategoryName = "Produce",
                    ItemPrice = 53,
                    PurchasePrice = 42.4M,
                    Quantity = 35,
                    TotalPrice = 1855,
                    Discount = 0.15D,
                    DiscountAmt = 278.25M,
                    FinalPrice = 1576.75M
                },
                new OrderItemDto
                {
                    ProductName = "Louisiana Fiery Hot Pepper Sauce",
                    CategoryName = "Condiments",
                    ItemPrice = 21.05M,
                    PurchasePrice = 16.8M,
                    Quantity = 15,
                    TotalPrice = 315.75M,
                    Discount = 0.15D,
                    DiscountAmt = 47.36M,
                    FinalPrice = 268.39M
                }
            },
            SendTo = new ShippingDto 
            { 
                ShipCost = 65.83M
            }
        };
    }
}
