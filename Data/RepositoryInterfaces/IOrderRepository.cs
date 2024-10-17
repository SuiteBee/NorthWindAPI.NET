using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.RepositoryInterfaces
{
    public interface IOrderRepository
    {
        public Task<Order?> FindOrder(int id);

        public Task<OrderDetail?> FindDetail(int orderId);
    }
}
