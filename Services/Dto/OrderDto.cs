﻿namespace NorthWindAPI.Services.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public required string OrderDate { get; set; }
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
        public required CustomerDto OrderedBy { get; set; } = new CustomerDto();
        public required EmployeeDto CompletedBy { get; set; } = new EmployeeDto();
        public required ShippingDto SendTo { get; set; } = new ShippingDto();
    }
}
