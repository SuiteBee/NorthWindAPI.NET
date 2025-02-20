﻿using System.Diagnostics.CodeAnalysis;

namespace NorthWindAPI.Services.ResponseDto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; } = "";
        public int EmployeeId { get; set; }
        public int CarrierId { get; set; }
        public required string OrderDate { get; set; } = "";
        public decimal OrderSubtotal { get; set; }
        public decimal OrderTotal { get; set; }
        public required List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public required ShippingDto SendTo { get; set; } = new ShippingDto();
    }
}
