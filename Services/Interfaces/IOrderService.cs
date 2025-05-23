﻿using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDto>> ListOrders();
        public Task<OrderDto> FindOrder(int orderId);
        public Task<IEnumerable<CarrierDto>> Carriers();
        public Task<OrderDto> ProcessNewOrder(NewOrderRequest newOrder);
        public Task<OrderDto> MarkAsShipped(int orderId, string? shippedDate = null);
        public Task<IEnumerable<OrderDto>> MarkAsShipped(ShipRequest orders);
        public Task RemoveOrder(int orderId);
    }
}

