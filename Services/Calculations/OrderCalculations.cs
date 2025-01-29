using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Calculations
{
    public static class OrderCalculations
    {
        public static void OrderTotal(OrderDto dto)
        {
            dto.OrderSubtotal = dto.Items.Sum(x => x.FinalPrice);
            dto.OrderTotal = dto.OrderSubtotal + dto.SendTo.ShipCost;
        }

        public static void ItemTotals(OrderItemDto dto)
        {
            dto.TotalPrice = GetTotalPrice(dto.ItemPrice, dto.Quantity);
            dto.DiscountAmt = GetDiscount(dto.TotalPrice, dto.Discount);
            dto.FinalPrice = GetFinalPrice(dto.TotalPrice, dto.Discount);
        }

        public static decimal GetTotalPrice(decimal cost, int quantity)
        {
            return Math.Round(cost * quantity, 2);
        }

        public static decimal GetDiscount(decimal total, double discount)
        {
            return Math.Round(total * (decimal)discount, 2);
        }

        public static decimal GetFinalPrice(decimal total, double discount)
        {
            if (discount > 0)
            {
                decimal finalPrice = total * (decimal)(1 - discount);
                return Math.Round(finalPrice, 2);
            }
            else
            {
                return total;
            }
        }
    }
}
