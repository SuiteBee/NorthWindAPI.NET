namespace NorthWindAPI.Controllers.Models.Requests
{
    public class OrderDetailRequest
    {
        public required int ProductId { get; set; }
        public required int Quantity { get; set; }
        public required decimal Discount { get; set; }

    }
}
