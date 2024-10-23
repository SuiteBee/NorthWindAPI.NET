using NorthWindAPI.Controllers.Models.Requests;
using NorthWindAPI.Services.ResponseDto;

namespace NorthWindAPI.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDto>> ListOrders();
        public Task<OrderDto> FindOrder(int id);
        public Task<OrderDto> ProcessNewOrder(NewOrderRequest newOrder);
    }
}

