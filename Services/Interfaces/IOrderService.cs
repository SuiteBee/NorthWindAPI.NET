using NorthWindAPI.Services.Dto;

namespace NorthWindAPI.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<OrderDto> FindOrder(int id);
    }
}

