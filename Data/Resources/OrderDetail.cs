namespace NorthWindAPI.Data.Resources
{
    public class OrderDetail : EntityAlt
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
    }
}
