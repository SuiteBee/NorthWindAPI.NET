using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPITest.Objects.Order
{
    public class OrderItemDtoTest
    {
        /// <summary>
        /// Order Item with no discount
        /// </summary>
        public static OrderItemDto OrderItemOne = new OrderItemDto
        {
            ProductName = "Jack's New England Clam Chowder",
            CategoryName = "Seafood",
            ItemPrice = 9.65M,
            PurchasePrice = 7.7M,
            Quantity = 10,
            Discount = 0,

            TotalPrice = 0,
            DiscountAmt = 0,
            FinalPrice = 0
        };

        /// <summary>
        /// Order Item with discount
        /// </summary>
        public static OrderItemDto OrderItemTwo = new OrderItemDto
        {
            ProductName = "Manjimup Dried Apples",
            CategoryName = "Produce",
            ItemPrice = 53,
            PurchasePrice = 42.4M,
            Quantity = 35,
            Discount = 0.15D,

            TotalPrice = 0,
            DiscountAmt = 0,
            FinalPrice = 0
        };

        /// <summary>
        /// Order Item with decimal price and discount
        /// </summary>
        public static OrderItemDto OrderItemThree = new OrderItemDto
        {
            ProductName = "Louisiana Fiery Hot Pepper Sauce",
            CategoryName = "Condiments",
            ItemPrice = 21.05M,
            PurchasePrice = 16.8M,
            Quantity = 15,
            Discount = 0.15D,

            TotalPrice = 0,
            DiscountAmt = 0,
            FinalPrice = 0
        };
    }
}
