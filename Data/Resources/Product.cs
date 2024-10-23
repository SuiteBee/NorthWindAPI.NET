namespace NorthWindAPI.Data.Resources
{
    public class Product : Entity
    {
        public string? ProductName { get; set; }
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }
        public int ReorderLevel { get; set; }
        public int Discontinued { get; set; }
    }
}
