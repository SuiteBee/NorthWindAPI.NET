﻿namespace NorthWindAPI.Services.ResponseDto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public required string OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public List<OrderItemDto> Products { get; set; } = new List<OrderItemDto>();
        public required CustomerDto OrderedBy { get; set; } = new CustomerDto();
        public required EmployeeDto CompletedBy { get; set; } = new EmployeeDto();
        public required ShippingDto SendTo { get; set; } = new ShippingDto();
    }
}
