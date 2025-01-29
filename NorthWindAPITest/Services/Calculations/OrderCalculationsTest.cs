using NorthWindAPI.Services.ResponseDto;
using NorthWindAPI.Services.Calculations;
using NorthWindAPITest.Objects.Order;
using NUnit.Framework;
using Microsoft.VisualStudio.TestPlatform;
using NUnit.Framework.Legacy;

namespace NorthWindAPITest.Services.Calculations
{
    [TestFixture]
    public class OrderCalculationsTest
    {
        [Test]
        public void OrderTotal_Test()
        {
            OrderDto testObj = OrderDtoTest.FullOrderNoTotals;

            OrderCalculations.OrderTotal(testObj);

            ClassicAssert.AreEqual(testObj.OrderSubtotal, 1941.64);
            ClassicAssert.AreEqual(testObj.OrderTotal, 2007.47);
        }

        [Test]
        public void ItemTotalsNoDiscount_Test()
        {
            OrderItemDto testObj = OrderItemDtoTest.OrderItemOne;

            OrderCalculations.ItemTotals(testObj);
            ClassicAssert.AreEqual(testObj.TotalPrice, 96.50M);
            ClassicAssert.AreEqual(testObj.DiscountAmt, 0.00M);
            ClassicAssert.AreEqual(testObj.FinalPrice, 96.50M);
        }

        [Test]
        public void ItemTotalsWithDiscount_Test()
        {
            OrderItemDto testObj = OrderItemDtoTest.OrderItemTwo;

            OrderCalculations.ItemTotals(testObj);

            ClassicAssert.AreEqual(testObj.TotalPrice, 1855);
            ClassicAssert.AreEqual(testObj.DiscountAmt, 278.25M);
            ClassicAssert.AreEqual(testObj.FinalPrice, 1576.75M);
        }

        [Test]
        public void ItemTotalsWithDecimalAndDiscount_Test()
        {
            OrderItemDto testObj = OrderItemDtoTest.OrderItemThree;

            OrderCalculations.ItemTotals(testObj);

            ClassicAssert.AreEqual(testObj.TotalPrice, 315.75M);
            ClassicAssert.AreEqual(testObj.DiscountAmt, 47.36M);
            ClassicAssert.AreEqual(testObj.FinalPrice, 268.39M);
        }

        [Test]
        public void GetTotalPrice_Test()
        {
            decimal testCost = 5.99M;
            int testQuantity = 9;

            decimal result = OrderCalculations.GetTotalPrice(testCost, testQuantity);

            ClassicAssert.AreEqual(result, 53.91M);
        }

        [Test]
        public void GetDiscount_Test()
        {
            decimal testTotal = 54321.71M;
            double testDiscount = 0.33D;

            decimal result = OrderCalculations.GetDiscount(testTotal, testDiscount);

            ClassicAssert.AreEqual(result, 17926.16M);
        }

        [Test]
        public void GetDiscountZero_Test()
        {
            decimal testTotal = 54321.71M;
            double testDiscount = 0.0D;

            decimal result = OrderCalculations.GetDiscount(testTotal, testDiscount);

            ClassicAssert.AreEqual(result, 0);
        }

        [Test]
        public void GetFinalPrice_Test()
        {
            decimal testTotal = 54321.71M;
            double testDiscount = 0.33D;

            decimal result = OrderCalculations.GetFinalPrice(testTotal, testDiscount);

            ClassicAssert.AreEqual(result, 36395.55);
        }

        [Test]
        public void GetFinalPriceNoDiscount_Test()
        {
            decimal testTotal = 54321.71M;
            double testDiscount = 0.0D;

            decimal result = OrderCalculations.GetFinalPrice(testTotal, testDiscount);

            ClassicAssert.AreEqual(result, 54321.71M);
        }
    }
}
