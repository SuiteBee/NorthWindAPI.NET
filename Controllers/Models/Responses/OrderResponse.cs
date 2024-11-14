using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Controllers.Models.Responses
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public required string OrderDate { get; set; }
        public decimal OrderSubtotal { get; set; }
        public decimal OrderTotal { get; set; }
        public bool Fullfilled { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public required CustomerDto OrderedBy { get; set; } = new CustomerDto();
        public required EmployeeDto CompletedBy { get; set; } = new EmployeeDto();
        public required ShippingDto SendTo { get; set; } = new ShippingDto();
    }
}
